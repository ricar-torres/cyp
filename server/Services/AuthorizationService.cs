using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;

namespace WebApi.Services
{
    public class AuthorizationService
    {

        public bool IsUserAuthorized(AuthorizationFilterContext actionContext)
        {
            var authHeader = FetchFromHeader(actionContext); //fetch authorization token from header


            if (authHeader != null)
            {
                var auth = new AuthorizationService();
                JwtSecurityToken userPayloadToken = GenerateUserClaimFromJWT(authHeader);

                if (userPayloadToken != null)
                {

                    //var identity = auth.PopulateUserIdentity(userPayloadToken);
                    //string[] roles = { "All" };
                    //var genericPrincipal = new GenericPrincipal(identity, roles);
                    //Thread.CurrentPrincipal = genericPrincipal;
                    //var authenticationIdentity = Thread.CurrentPrincipal.Identity as JWTAuthenticationIdentity;
                    //if (authenticationIdentity != null && !String.IsNullOrEmpty(authenticationIdentity.UserName))
                    //{
                    //    authenticationIdentity.UserId = identity.UserId;
                    //    authenticationIdentity.UserName = identity.UserName;
                    //}
                    return true;
                }

            }
            return false;


        }

        public JwtSecurityToken GenerateUserClaimFromJWT(string authToken)
        {

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
                //IssuerSigningKey = signingKey
            };
            var tokenHandler = new JwtSecurityTokenHandler();

            SecurityToken validatedToken;

            try
            {
                tokenHandler.ValidateToken(authToken, tokenValidationParameters, out validatedToken);
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

    }
}
