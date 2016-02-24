using System;
using System.Web;
using BrCms.Framework.Security;

namespace BrCms.Framework.Mvc
{
    public class WebWorkContext : IWorkContext
    {
        private readonly HttpContextBase _httpContext;
        private readonly IBrUserManageService _userManageService;
        private const string USER_COOKIE = "USER";

        public WebWorkContext(HttpContextBase httpContext, IBrUserManageService userManageService)
        {
            this._httpContext = httpContext;
            this._userManageService = userManageService;
        }

        protected virtual HttpCookie GetCustomerCookie()
        {
            if (this._httpContext == null || this._httpContext.Request == null) return null;

            return this._httpContext.Request.Cookies[USER_COOKIE];
        }

        protected virtual void SetCustomerCookie(object customerGuid)
        {
            if (this._httpContext == null || this._httpContext.Response == null) return;
            var cookie = new HttpCookie(USER_COOKIE)
            {
                HttpOnly = true,
                Value = customerGuid.ToString(),
                Expires = customerGuid == Guid.Empty ? DateTime.Now.AddMonths(-1) : DateTime.Now.AddMinutes(30)
            };

            this._httpContext.Response.Cookies.Remove(USER_COOKIE);
            this._httpContext.Response.Cookies.Add(cookie);
        }

        private IUser _cachedUser;
        public IUser CurrentUser
        {
            get
            {
                var userCookie = this.GetCustomerCookie();
                if (userCookie == null || String.IsNullOrEmpty(userCookie.Value))
                {
                    return this._cachedUser;
                }
                if (userCookie.Value is Guid)
                {
                    this._userManageService.GetUserByGuid((Guid)userCookie.Value);
                }
                IUser user = this._userManageService.GetUserByGuid(userGuid);
                //validation
                if (user.Deleted || !user.Active) return this._cachedUser;
                this.SetCustomerCookie(user.UserId);
                this._cachedUser = user;
                return this._cachedUser;
            }
            set
            {
                this.SetCustomerCookie(value.UserId);
                this._cachedUser = value;

            }
        }

        public string WorkingThemeName
        {
            //当前主题
            get { return ""; }
        }
    }
}
