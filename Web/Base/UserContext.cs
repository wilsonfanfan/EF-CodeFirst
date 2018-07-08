using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using X.Common;
using X.IService.Authentication;

namespace X.Web.Base
{
    /// <summary>
    /// 用户上下文
    /// </summary>
    public class UserContext
    {
        /// <summary>
        /// 获取当前用户
        /// </summary>
        public static Models.UserSession CurrentUserSession()
        {
            IAuthenticationService authenticationService = DIContainer.Resolve<IAuthenticationService>();
            var currentUser = authenticationService.GetAuthenticatedUser();
            if (currentUser != null)
                return currentUser;
            return null;
        }
    }
}