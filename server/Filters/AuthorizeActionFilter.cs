using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApi.Enums;
using WebApi.Services;

namespace WebApi.Filters
{
    public class AuthorizeAttribute : TypeFilterAttribute
    {
        public AuthorizeAttribute(PermissionItem item, PermissionAction action)
        : base(typeof(AuthorizeActionFilter))
        {
            Arguments = new object[] { item, action };
        }
    }

    public class AuthorizeActionFilter : IAuthorizationFilter
    {
        private readonly PermissionItem _item;
        private readonly PermissionAction _action;
        private IAuthService _authService;

        public AuthorizeActionFilter(PermissionItem item, PermissionAction action, IAuthService authService)
        {
            _item = item;
            _action = action;
            _authService = authService;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {

            bool isAuthorized = _authService.IsUserAuthorized(context, _item, _action); // MumboJumboFunction(context.HttpContext.User, _item, _action); // :)

            if (!isAuthorized)
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
