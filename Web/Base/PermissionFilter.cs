using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using X.Common;
using X.Constant;
using X.IDao;
using X.IService;

namespace X.Web.Base
{
    //繼承AuthorizeAttribute，並Override OnAuthorization()
    /// <summary>
    /// 后台身份验证
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class PermissionFilter : AuthorizeAttribute
    {
        private bool requirePermission = false;
        /// <summary>
        /// 是否需要系统管理员权限
        /// </summary>
        public bool RequirePermission
        {
            get { return requirePermission; }
            set { requirePermission = value; }
        }

        #region IAuthorizationFilter 成员

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            if (!AuthorizeCore(filterContext))
            {
                // auth failed, redirect to login page
                // filterContext.Cancel = true;
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.Result = new JsonNetResult() { Data = new IResponse<bool>() { Status = false, ErrMsg = "您必须先以管理员身份登录下后台，才能继续操作" } };
                }
                else
                {
                    if (requirePermission)
                    {
                        filterContext.Result = new RedirectResult("/WebAdmin/Home/LogOff");
                    }
                    else
                    {
                        filterContext.Result = new RedirectResult(FormsAuthentication.LoginUrl);
                    }
                }
                return;
            }

            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new JsonNetResult() { Data = new IResponse<bool>() { Status = false, ErrMsg = "您无权限操作" } };
            }
            else
            {
                filterContext.Result = new ContentResult() { Content = "<script>alert(\"您无权限操作\");history.back();</script>" };
            }
            return;
        }
        #endregion
        protected virtual bool AuthorizeCore(AuthorizationContext filterContext)
        {
            if (filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true) || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
            {
                return true;
            }
            var currentUser = UserContext.CurrentUserSession();
            if (currentUser != null)
            {
                return true;
            }
            return false;
        }

        // This method must be thread-safe since it is called by the thread-safe OnCacheAuthorization() method.

    }
}