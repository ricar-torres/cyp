using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Novell.Directory.Ldap;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using WebApi.Dtos;
using WebApi.Entities.Identity;
using WebApi.Enums;
using WebApi.Helpers;

namespace WebApi.Services
{

    public interface IAuthService
    {
        IActionResult Login(int applicationId, string username, string password, string type);
        AppUser SignUp(UserDto userDto);
        IEnumerable<Claim> GetClaims(AppUser user);
        AppUser CheckUserName(int ApplicationId, string username);
        bool IsUserAuthorized(AuthorizationFilterContext actionContext, PermissionItem item, PermissionAction action);
        string CreateClaimCode(PermissionItem item, PermissionAction action);
        bool ValidateUserRequireField(AppUser user, string password, out string exception);
        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt);
        string GeneratePassword(bool includeLowercase, bool includeUppercase, bool includeNumeric, bool includeSpecial, bool includeSpaces, int lengthOfPassword);
        bool PasswordIsValid(bool includeLowercase, bool includeUppercase, bool includeNumeric, bool includeSpecial, bool includeSpaces, string password);
        string ResetPasswod(string email);
    }

    public class AuthService : Controller, IAuthService
    {
        private DataContext _context;
        private readonly AppSettings _appSettings;
        private IMapper _mapper;
        private readonly LdapConnection _connection;


        public AuthService(DataContext context, IOptions<AppSettings> appSettings, IMapper mapper)
        {
            _context = context;
            _appSettings = appSettings.Value;
            _mapper = mapper;
            _connection = new LdapConnection
            {
                SecureSocketLayer = false
            };
        }

        public IActionResult Login(int applicationId, string username, string password, string type)
        {

            try
            {
                var application = _context.Application.Where(x => x.Id == applicationId).FirstOrDefault();

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || application == null)
                    return BadRequest();

                var user = _context.AppUser.Include(u => u.Roles).ThenInclude(r => r.Role).SingleOrDefault(x => x.UserName == username && x.ApplicationId == application.Id && x.UserType == type && x.DelFlag == false);

                // check if username exists
                if (user == null)
                    return NotFound();

                if (user.Roles.Count < 1)
                    return Forbid();

                if (user.LoginProviderId == (int)LoginProviderEnum.Local)
                {
                    // check if password is correct
                    if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                        return Forbid();
                }
                else
                {
                    if (user.LoginProviderId == (int)LoginProviderEnum.LDAP)
                    {
                        // check if password is correct
                        if (!VerifyPasswordLDap(username, password))
                            return Forbid();
                    }
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

                ClaimsIdentity claims = new ClaimsIdentity();
                claims.AddClaims(GetClaims(user));
                //user.Claims = new string[];
                user.Claims = claims.Claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value).ToArray();

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Audience = "http://www.example.com",
                    Issuer = "self",
                    Subject = claims,
                    Expires = DateTime.UtcNow.AddDays(_appSettings.TokenValidDays),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                

                var token = tokenHandler.CreateToken(tokenDescriptor);
                user.Token = tokenHandler.WriteToken(token);

                // authentication successful
                return Ok(new
                {
                    user.Id,
                    Username = user.UserName,
                    user.FirstName,
                    user.LastName,
                    user.Token,
                    user.ApplicationId,
                    user.EmailConfirmed,
                    claims = user.Claims,
                    user.IsChgPwd,
                    user.UserType,
                    user.Reference1
                });

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public AppUser SignUp(UserDto userDto) {

            try
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(userDto.Password, out passwordHash, out passwordSalt);

                var user = _mapper.Map<AppUser>(userDto);
                user.Email = user.UserName;
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                user.IsChgPwd = false;
                user.EmailConfirmed = false;
                user.UserType = UserType.USER;
                user.LoginProviderId = 1;
                user.FCreateUserId = 0;
                user.CreateDt = DateTime.Now;
                user.DelFlag = false;

                _context.AppUser.Add(user);
                _context.SaveChanges();

                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<Claim> GetClaims(AppUser user)
        {

            try
            {
                ClaimsIdentity claims = new ClaimsIdentity();
                String claimVal = string.Empty;

                claims.AddClaim(new Claim(ClaimTypes.Name, user.Id.ToString()));
                claims.AddClaim(new Claim(ClaimTypes.Email, string.IsNullOrEmpty(user.Email) ? user.UserName : user.Email));
                claims.AddClaim(new Claim(ClaimTypes.GroupSid, user.ApplicationId.ToString()));

                //var roleMenu = _context.RoleMenu
                //                       .Include(r => r.Permissions)
                //                       .Where(m => user.Roles.Any(ur => ur.RoleId == m.RoleId) && m.Permissions.Count() > 0).ToList();

                var roles = user.Roles.Select(x => x.RoleId);
                var roleMenu = _context.RoleMenu
                                       .Include(r => r.Permissions)
                                       .Where(m => roles.Contains(m.RoleId) && m.Permissions.Count() > 0).ToList();

                foreach (var mp in roleMenu)
                {
                    foreach (var p in mp.Permissions.ToList())
                    {
                        claimVal = CreateClaimCode((PermissionItem)mp.MenuItemId, (PermissionAction)p.PermissionId);

                        if (!claims.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == claimVal))
                            claims.AddClaim(new Claim(ClaimTypes.Role, claimVal));
                    }
                }

                return claims.Claims;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        //public User LoginLDap(string username, string password)
        //{
        //    _connection.Connect(_appSettings.LDAP.Url, 139);
        //    _connection.Bind(_appSettings.LDAP.BindDn, _appSettings.LDAP.BindCredentials);

        //    string company = "hacienda";
        //    string groupFilter = string.Empty;
        //    //var searchFilter = string.Format(_appSettings.LDAP.SearchFilter, username);
        //   //string searchFilter = $"(&(objectClass=user)(objectCategory=person)(company={company}){groupFilter})";
        //    string searchFilter = $"(samaccountname=*{username}*)";

        //    var result = _connection.Search(
        //        string.Empty, //_appSettings.LDAP.SearchBase,
        //        LdapConnection.SCOPE_SUB,
        //        "(sAMAccountName=ggl3782)", //searchFilter,
        //        new[] { MemberOfAttribute, DisplayNameAttribute, SAMAccountNameAttribute },
        //        false
        //    );

        //    var result2 = _connection.Search(
        //        string.Empty, //_appSettings.LDAP.SearchBase,
        //        LdapConnection.SCOPE_SUB,
        //        "(objectClass=*)", //searchFilter,
        //        null ,//new[] { MemberOfAttribute, DisplayNameAttribute, SAMAccountNameAttribute },
        //        false
        //    );

        //    var result3 = _connection.Search(
        //        string.Empty, //_appSettings.LDAP.SearchBase,
        //        LdapConnection.SCOPE_SUB,
        //        @"(sAMAccountName=hacienda\\GGL3782)", //searchFilter,
        //        new[] { MemberOfAttribute, DisplayNameAttribute, SAMAccountNameAttribute },
        //        false
        //    );

        //    var result4 = _connection.Search(
        //        string.Empty, //_appSettings.LDAP.SearchBase,
        //        LdapConnection.SCOPE_SUB,
        //        @"(sAMAccountName=hacienda\\Gustavo.Gomez)", //searchFilter,
        //        new[] { MemberOfAttribute, DisplayNameAttribute, SAMAccountNameAttribute },
        //        false
        //    );

        //    try
        //    {
        //        var user = result.Next();
        //        if (user != null)
        //        {
        //            _connection.Bind(user.DN, password);
        //            if (_connection.Bound)
        //            {
        //                return new User
        //                {
        //                    //DisplayName = user.getAttribute(DisplayNameAttribute).StringValue,
        //                    UserName = user.getAttribute(SAMAccountNameAttribute).StringValue//,
        //                    //IsAdmin = user.getAttribute(MemberOfAttribute).StringValueArray.Contains(_config.AdminCn)
        //                };
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        throw new AppException("Login failed.");
        //    }
        //    _connection.Disconnect();
        //    return null;
        //}

        public AppUser CheckUserName(int ApplicationId, string username)
        {

            try
            {

                var payload = _context.AppUser.Where(u => u.ApplicationId == ApplicationId && u.UserName == username).FirstOrDefault();
                return payload;

            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public bool IsUserAuthorized(AuthorizationFilterContext actionContext, PermissionItem item, PermissionAction action)
        {
            var authHeader = FetchFromHeader(actionContext); //fetch authorization token from header


            if (authHeader != null)
            {
                var auth = new AuthorizationService();
                JwtSecurityToken userPayloadToken = GenerateUserClaimFromJWT(authHeader);

                if (userPayloadToken != null)
                {

                    //return userPayloadToken.Claims.Any(x => x.Value == CreateClaimCode(item, action));
                    return userPayloadToken.Claims.Any(x => x.Value == CreateClaimCode(item, action)) && userPayloadToken.Claims.Any(x => x.Type == "groupsid" && !string.IsNullOrEmpty(x.Value));


                    //var identity = auth.PopulateUserIdentity(userPayloadToken);
                    //string[] claims = userPayloadToken.Claims.;

                    //var genericPrincipal = new GenericPrincipal(identity, roles);
                    //Thread.CurrentPrincipal = genericPrincipal;
                    //var authenticationIdentity = Thread.CurrentPrincipal.Identity as JWTAuthenticationIdentity;
                    //if (authenticationIdentity != null && !String.IsNullOrEmpty(authenticationIdentity.UserName))
                    //{
                    //    authenticationIdentity.UserId = identity.UserId;
                    //    authenticationIdentity.UserName = identity.UserName;
                    //}
                    //return true;
                }

            }
            return false;


        }

        public string CreateClaimCode(PermissionItem item, PermissionAction action)
        {

            return $"{item.ToString()}.{action.ToString()}";

        }

        public bool ValidateUserRequireField(AppUser user, string password, out string exception)
        {

            exception = string.Empty;

            // validation
            if (user.LoginProviderId == 0)
            {
                exception = "LoginProviderId is required";
                return false;
                //throw new AppException("LoginProviderId is required");
            }

            if (!_context.LoginProvider.Any(lp => lp.Id == user.LoginProviderId && lp.DelFlag == false))
            {
                exception = "LoginProviderId is invalid";
                return false;
                //throw new AppException("LoginProviderId is invalid");
            }

            if (string.IsNullOrWhiteSpace(password) && user.LoginProviderId == (int)LoginProviderEnum.Local)
            {
                exception = "Password is required";
                return false;
                //throw new AppException("Password is required");
            }

            if (string.IsNullOrWhiteSpace(user.UserName))
            {
                exception = "UserName is required";
                return false;
                //throw new AppException("UserName is required");
            }

            return true;

        }

        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            try
            {
                if (password == null) throw new ArgumentNullException("password");
                if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

                using (var hmac = new System.Security.Cryptography.HMACSHA512())
                {
                    passwordSalt = hmac.Key;
                    passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            try
            {
                if (password == null) throw new ArgumentNullException("password");
                if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
                if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
                if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

                using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
                {
                    var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                    for (int i = 0; i < computedHash.Length; i++)
                    {
                        if (computedHash[i] != storedHash[i]) return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GeneratePassword(bool includeLowercase, bool includeUppercase, bool includeNumeric, bool includeSpecial, bool includeSpaces, int lengthOfPassword)
        {
            try
            {
                const int MAXIMUM_IDENTICAL_CONSECUTIVE_CHARS = 2;
                const string LOWERCASE_CHARACTERS = "abcdefghijklmnopqrstuvwxyz";
                const string UPPERCASE_CHARACTERS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                const string NUMERIC_CHARACTERS = "0123456789";
                const string SPECIAL_CHARACTERS = @"!#$%&*@\";
                const string SPACE_CHARACTER = " ";
                const int PASSWORD_LENGTH_MIN = 8;
                const int PASSWORD_LENGTH_MAX = 128;

                if (lengthOfPassword < PASSWORD_LENGTH_MIN || lengthOfPassword > PASSWORD_LENGTH_MAX)
                {
                    return "Password length must be between 8 and 128.";
                }

                string characterSet = "";

                if (includeLowercase)
                {
                    characterSet += LOWERCASE_CHARACTERS;
                }

                if (includeUppercase)
                {
                    characterSet += UPPERCASE_CHARACTERS;
                }

                if (includeNumeric)
                {
                    characterSet += NUMERIC_CHARACTERS;
                }

                if (includeSpecial)
                {
                    characterSet += SPECIAL_CHARACTERS;
                }

                if (includeSpaces)
                {
                    characterSet += SPACE_CHARACTER;
                }

                char[] password = new char[lengthOfPassword];
                int characterSetLength = characterSet.Length;

                System.Random random = new System.Random();
                for (int characterPosition = 0; characterPosition < lengthOfPassword; characterPosition++)
                {
                    password[characterPosition] = characterSet[random.Next(characterSetLength - 1)];

                    bool moreThanTwoIdenticalInARow =
                        characterPosition > MAXIMUM_IDENTICAL_CONSECUTIVE_CHARS
                        && password[characterPosition] == password[characterPosition - 1]
                        && password[characterPosition - 1] == password[characterPosition - 2];

                    if (moreThanTwoIdenticalInARow)
                    {
                        characterPosition--;
                    }
                }

                return string.Join(null, password);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool PasswordIsValid(bool includeLowercase, bool includeUppercase, bool includeNumeric, bool includeSpecial, bool includeSpaces, string password)
        {
            try
            {
                const string REGEX_LOWERCASE = @"[a-z]";
                const string REGEX_UPPERCASE = @"[A-Z]";
                const string REGEX_NUMERIC = @"[\d]";
                const string REGEX_SPECIAL = @"([!#$%&*@\\])+";
                const string REGEX_SPACE = @"([ ])+";

                bool lowerCaseIsValid = !includeLowercase || (includeLowercase && Regex.IsMatch(password, REGEX_LOWERCASE));
                bool upperCaseIsValid = !includeUppercase || (includeUppercase && Regex.IsMatch(password, REGEX_UPPERCASE));
                bool numericIsValid = !includeNumeric || (includeNumeric && Regex.IsMatch(password, REGEX_NUMERIC));
                bool symbolsAreValid = !includeSpecial || (includeSpecial && Regex.IsMatch(password, REGEX_SPECIAL));
                bool spacesAreValid = !includeSpaces || (includeSpaces && Regex.IsMatch(password, REGEX_SPACE));

                return lowerCaseIsValid && upperCaseIsValid && numericIsValid && symbolsAreValid && spacesAreValid;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string ResetPasswod(string email)
        {
            try
            {
                var user = _context.AppUser.Where(x=> x.UserName == email && x.DelFlag == false && x.IsLock == false).FirstOrDefault();
                if (user != null)
                {
                    var newPassword = GeneratePassword(true, true, true, true, false, 10);
                    byte[] passwordHash, passwordSalt;
                    CreatePasswordHash(newPassword, out passwordHash, out passwordSalt);

                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;
                    user.IsChgPwd = true;

                    _context.AppUser.Attach(user);
                    _context.Entry(user).Property(x => x.PasswordHash).IsModified = true;
                    _context.Entry(user).Property(x => x.PasswordSalt).IsModified = true;
                    _context.Entry(user).Property(x => x.IsChgPwd).IsModified = true;

                    _context.SaveChanges();

                    return newPassword;
                }

                return "";
            }
            catch (Exception)
            {
                return "";
            }
        }

        #region "PRIVATE MENBERS"

        private bool VerifyPasswordLDap(string username, string password)
        {

            //bool retVal = true;

            try
            {
                _connection.Connect(_appSettings.LDAP.Url, LdapConnection.DEFAULT_PORT);
                // _connection.StartTls();
                //_connection.Bind(_appSettings.LDAP.BindDn, _appSettings.LDAP.BindCredentials);
                _connection.Bind(username, password);
                return true;

            }
            catch (Exception)
            {
                //retVal = false;
            }
            finally
            {

                if (_connection.Connected)
                    _connection.Disconnect();

            }

            return false;

        }

        private JwtSecurityToken GenerateUserClaimFromJWT(string authToken)
        {

            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidAudiences = new string[]
                {
                    "http://www.example.com",
                },

                ValidIssuers = new string[]
                {
                  "self",
                },
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            SecurityToken validatedToken;

            try
            {
                tokenHandler.ValidateToken(authToken.Replace("Bearer ", ""), tokenValidationParameters, out validatedToken);
            }
            catch (Exception)
            {
                return null;

            }

            return validatedToken as JwtSecurityToken;

        }

        private string FetchFromHeader(AuthorizationFilterContext actionContext)
        {
            string requestToken = null;

            //var authRequest = actionContext.Headers.Authorization;
            //Microsoft.Extensions.Primitives.Extensions.
            actionContext.HttpContext.Request.Headers.TryGetValue("Authorization", out var authorizationToken);

            //if (authorizationToken != null)
            //{
            requestToken = authorizationToken.ToString();
            //}

            return requestToken;
        }


        #endregion


    }
}