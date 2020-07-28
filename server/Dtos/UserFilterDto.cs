using System;
namespace WebApi.Dtos
{
    public class UserFilterDto
    {
        public string FullName { get; set; }
         public string UserName { get; set; }
        public string Email { get; set; }
        public int LoginProviderId { get; set; }

    }
}
