using X.IDao;


namespace X.IService.Authentication
{
    /// <summary>
    /// 用于身份认证的接口
    /// </summary>
    /// <remarks>实例的生命周期为每HttpRequest</remarks>
    public interface IAuthenticationService : IDependency
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userName">登录的用户</param>
        /// <param name="createPersistentCookie">是否记住</param>
        void SignIn(Models.UserAccount user, bool createPersistentCookie);

        /// <summary>
        /// 注销
        /// </summary>
        void SignOut();

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="user">待创建的用户</param>
        /// <param name="password">密码</param>
        /// <param name="err">错误</param>
        /// <returns>创建成功返回IUser，创建失败返回null</returns>
        Models.UserAccount CreateUser(Models.UserAccount user, string password, out string err);

        /// <summary>
        /// 获取当前认证的用户
        /// </summary>
        /// <returns>
        /// 当前用户未通过认证则返回null
        /// </returns>
        Models.UserAccount GetCurrentUser();

        /// <summary>
        /// 验证提供的用户名和密码是否匹配
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="password"></param>
        /// <param name="err"></param>
        /// <returns></returns>
        Models.UserAccount ValidateUser(string loginName, string password, out string err);

        /// <summary>
        /// 获取当前认证的用户
        /// </summary>
        /// <returns>
        /// 当前用户未通过认证则返回null
        /// </returns>
        Models.UserSession GetAuthenticatedUser();

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="password">密码</param>
        /// <param name="err">错误</param>
        bool ResetPassword(long userId, string password, out string err);


    }
}
