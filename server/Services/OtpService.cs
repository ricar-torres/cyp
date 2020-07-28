using System;
using System.Linq;
using Microsoft.Extensions.Options;
using WebApi.Entities.Identity;
using WebApi.Helpers;

namespace WebApi.Services
{

    public static class OtpType
    {
        public const string EMAIL_VALIDATION = "EMAIL_VALIDATION";
    }

    public interface IOtpService
    {
        OneTimePassword New(string type ,AppUser user);
        bool Validate(string type, AppUser user, string otp);
        int GenerateOTP();
    }

    public class OtpService : IOtpService
    {
        private readonly DataContext _context;
        private readonly AppSettings _appSettings;


        public OtpService(DataContext context, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings.Value;
        }

        public OneTimePassword New(string type, AppUser user)
        {
            var otp = new OneTimePassword
            {
                ApplicationId = user.ApplicationId,
                UserName = user.UserName,
                OTP = GenerateOTP().ToString(),
                Type = type,
                ValidDays = _appSettings.OtpValidDays
            };

            var list = _context.OneTimePassword.Where(t => t.UserName == user.UserName && t.DelFlag == false);
            if (list != null) {
                foreach(var item in list)
                {
                    item.DelFlag = true;
                    _context.OneTimePassword.Update(item);
                }
            }
            _context.OneTimePassword.Add(otp);
            _context.SaveChanges();

            return otp;
        }

        //Generate RandomNo
        public int GenerateOTP()
        {
            int _min = 10000;
            int _max = 99999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }

        public bool Validate(string type, AppUser user, string otp)
        {

            var item = _context.OneTimePassword.Where(x => x.ApplicationId == user.ApplicationId && x.UserName == user.UserName && x.OTP == otp && x.Type == type && x.DelFlag == false).FirstOrDefault();
            if (item != null) {
                var dayDiff = DateTime.Now - item.CreateDt;
                if (dayDiff.Days < item.ValidDays) {
                    item.UpdDt = DateTime.Now;
                    item.DelFlag = true;
                    _context.OneTimePassword.Update(item);
                    _context.SaveChanges();
                    return true;
                }
                    
            }
            return false;

        }
    }
}
