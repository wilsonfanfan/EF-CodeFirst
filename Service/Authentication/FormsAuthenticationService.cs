using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using X.IDao;
using X.IService.Authentication;
using X.Common;
using X.Models;

namespace X.Service.Authentication
{
    public class FormsAuthenticationService : IAuthenticationService
    {
        public IUserAccountDao repository { get; set; }
        public FormsAuthenticationService(IUserAccountDao _repository)
        {
            repository = _repository;
            ExpirationTimeSpan = TimeSpan.FromDays(30);
        }

        #region 登录
        public TimeSpan ExpirationTimeSpan { get; set; }
        private UserSession signedInUser;
        private bool _isAuthenticated;

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="user">登录的用户</param>
        /// <param name="createPersistentCookie">是否记住密码</param>
        //public void SignIn(Models.UserAccount user, bool createPersistentCookie)
        //{
        //    var iUser = new UserSession() { UserId = user.Id, UserName = user.UserName };
        //    iUser.RoleList = new List<string>();
           
        //    var exp = DateTime.UtcNow.AddMinutes(30);
        //    var now = DateTime.Now;
        //    var userData = String.Concat(Convert.ToString(user.Id), ";", user.Tenant, ";" + string.Join(",", iUser.RoleList));
        //    var ticket = new FormsAuthenticationTicket(
        //     1 /*version*/,
        //     user.UserName,
        //     now,
        //     now.Add(ExpirationTimeSpan),
        //     createPersistentCookie,
        //     userData,
        //     FormsAuthentication.FormsCookiePath);

        //    var encryptedTicket = FormsAuthentication.Encrypt(ticket);


        //    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
        //    {
        //        HttpOnly = true,
        //        Secure = FormsAuthentication.RequireSSL,
        //        Path = FormsAuthentication.FormsCookiePath
        //    };
        //    var httpContext = new HttpContextWrapper(HttpContext.Current);

        //    if (FormsAuthentication.CookieDomain != null)
        //    {
        //        cookie.Domain = FormsAuthentication.CookieDomain;
        //    }
        //    if (createPersistentCookie)
        //    {
        //        cookie.Expires = ticket.Expiration;
        //    }
        //    httpContext.Response.Cookies.Add(cookie);
        //    //FormsAuthentication.SetAuthCookie(userName, rememberPassword);
        //    //UserSession _signedInUser = GetAuthenticatedUser();
        //    //if (_signedInUser != null)
        //    //{
        //    //}

        //    signedInUser = iUser;
        //    _isAuthenticated = true;
        //    repository.Update(t => t.Id == user.Id, () => new Entity.UserAccount { LastLoginedIP = Common.Util.GetIP(), LastLoginedTime = Common.Util.UtcDateTime, AccessFailedCount = 0 });
        //}
        #endregion

        #region 注销
        /// <summary>
        /// 注销
        /// </summary>
        public void SignOut()
        {
            UserSession _signedInUser = GetAuthenticatedUser();
            signedInUser = null;
            _isAuthenticated = false;
            FormsAuthentication.SignOut();

            if (_signedInUser != null)
            {
                //UserIdToUserNameDictionary.RemoveUserName(_signedInUser.UserName);
            }
            var httpContext = new HttpContextWrapper(HttpContext.Current);
            var rFormsCookie = new HttpCookie(FormsAuthentication.FormsCookieName, "")
            {
                Expires = DateTime.Now.AddYears(-1),
            };
        }

        #endregion

        #region 当前用户
        /// <summary>
        /// 获取当前认证的用户
        /// </summary>
        /// <returns>
        /// 当前用户未通过认证则返回null
        /// </returns>
        public UserSession GetAuthenticatedUser()
        {
            var httpContext = new HttpContextWrapper(HttpContext.Current);
            if (httpContext == null || !httpContext.Request.IsAuthenticated || !(httpContext.User.Identity is FormsIdentity))
            {
                return null;
            }
            if (signedInUser != null && _isAuthenticated)
            {
                return signedInUser;
            }
            var formsIdentity = (FormsIdentity)httpContext.User.Identity;
            var userData = formsIdentity.Ticket.UserData ?? "";
            var userDataSegments = userData.Split(';');
            if (userDataSegments.Length < 3)
            {
                return null;
            }

            var userDataId = userDataSegments[0];
            var userDataTenant = userDataSegments[1];
            var userDataRole = userDataSegments[2];

            if (!String.Equals(userDataTenant, "Default", StringComparison.Ordinal))
            {
                return null;
            }

            int userId;
            if (!int.TryParse(userDataId, out userId))
            {
                return null;
            }

            signedInUser = new UserSession();
            signedInUser.UserName = httpContext.User.Identity.Name;

            //signedInUser.UserId = UserIdToUserNameDictionary.GetUserId(signedInUser.UserName);
            //if (signedInUser.UserId <= 0)
            //{
            //    var model = repository.GetUserByUserName(signedInUser.UserName);
            //    if (model != null)
            //    {
            //        signedInUser.UserId = model.Id;
            //        _UserAccount = Mapper.Map<Models.UserAccount>(model);
            //        UserIdToUserNameDictionary.SetUserName(signedInUser.UserName, model.Id);
            //    }
            //}
            if (userId <= 0 || signedInUser.UserId != userId)
            {
                return null;
            }
            //signedInUser.RoleList = new List<string>();
            //if (!userDataRole.IsNull())
            //{
            //    var userRole = userDataRole.Split(",");
            //    if (userRole.Contains(RoleNames.LoginManage))
            //    {
            //        signedInUser.RoleList.Add(RoleNames.LoginManage);
            //    }
            //    if (userRole.Contains(RoleNames.SuperAdministrator))
            //    {
            //        signedInUser.RoleList.Add(RoleNames.SuperAdministrator);
            //    }
            //}
            _isAuthenticated = true;
            return signedInUser;
        }
        public Models.UserAccount _UserAccount;
        /// <summary>
        /// 获取当前认证的用户
        /// </summary>
        /// <returns>
        /// 当前用户未通过认证则返回null
        /// </returns>
        public Models.UserAccount GetCurrentUser()
        {
            try
            {
                if (_UserAccount != null && _isAuthenticated)
                {
                    return _UserAccount;
                }
                signedInUser = GetAuthenticatedUser();
                if (signedInUser != null && signedInUser.UserId > 0 && _isAuthenticated)
                {
                    var entity = repository.GetById(signedInUser.UserId);
                    return Mapper.Map<Models.UserAccount>(entity);
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
        #endregion




        #region 检查密码
        /// <summary>
        /// 检测用户密码是否适合站点设置
        /// </summary>
        /// <param name="newPassword">Password to be verified.</param>
        /// <param name="errorMessage">Error message to return.</param>
        /// <returns>True if compliant, otherwise False.</returns>
        private static bool ValidatePassword(string newPassword, out string errorMessage)
        {
            /// <summary>
            /// 密码最小长度
            /// </summary>
            int minRequiredPasswordLength = 5;
            /// <summary>
            /// 密码中包含的最少特殊字符数
            /// </summary>
            int minRequiredNonAlphanumericCharacters = 0;

            errorMessage = "";

            if (string.IsNullOrEmpty(newPassword))
            {
                errorMessage = "密码不能为空";
                return false;
            }

            if (newPassword.Length < minRequiredPasswordLength)
            {
                errorMessage = string.Format("密码长度不能少于 {0} 个字符", minRequiredPasswordLength);
                return false;
            }

            int nonAlphaNumChars = 0;
            for (int i = 0; i < newPassword.Length; i++)
            {
                if (!char.IsLetterOrDigit(newPassword, i))
                    nonAlphaNumChars++;
            }
            if (nonAlphaNumChars < minRequiredNonAlphanumericCharacters)
            {
                errorMessage = string.Format("您的密码中特殊字符太少，请至少输入 {0} 个特殊字符（如：*、$等）", minRequiredNonAlphanumericCharacters);
                return false;
            }

            return true;
        }
        #endregion

        #region 重置密码
        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="password">密码</param>
        /// <param name="err">错误</param>
        public bool ResetPassword(long userId, string password, out string err)
        {
            err = "";
            //检查密码
            if (!ValidatePassword(password, out err))
            {
                return false;
            }
            var entity = repository.GetById(userId);
            if (entity == null)
            {
                err = "用户不存在";
                return false;
            }
            //entity.SecretKey = DESEncrypt.GenerateKey(32);
            //entity.PasswordHash = DESEncrypt.EncryptPwd(password, entity.SecretKey);
            var CurrentUser = GetAuthenticatedUser();
            if (CurrentUser == null)
            {
                err = "登陆状态丢失";
                return false;
            }
            var result = repository.Update(entity, CurrentUser.UserId, "SecretKey", "PasswordHash");
            return result;
        }
        #endregion

        public void SignIn(UserAccount user, bool createPersistentCookie)
        {
            throw new NotImplementedException();
        }

        public UserAccount CreateUser(UserAccount user, string password, out string err)
        {
            throw new NotImplementedException();
        }

        public UserAccount ValidateUser(string loginName, string password, out string err)
        {
            throw new NotImplementedException();
        }
    }
}
