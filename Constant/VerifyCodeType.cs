using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Constant
{
    /// <summary>
    /// 验证码用处
    /// </summary>
    public static class CodeUse
    {
        [Description("注册")]
        public const string Register = "Register";

        [Description("找回密码")]
        public const string Forget = "Forget";

        [Description("修改密码")]
        public const string ChangePassword = "ChangePassword";
    }
    /// <summary>
    /// 验证码状态
    /// </summary>
    public static class CodeStatus
    {
        [Description("未验证")]
        public const string Unverified = "Unverified";

        [Description("已验证")]
        public const string Verified = "Verified";

        [Description("作废")]
        public const string Obsoleted = "Obsoleted";

        [Description("发送失败")]
        public const string SendFailed = "SendFailed";
    }
    /// <summary>
    /// 验证码类型
    /// </summary>
    public static class CodeType
    {
        /// <summary>
        /// 短信
        /// </summary>
        [Description("短信")]
        public const string SMS = "SMS";

        /// <summary>
        /// 邮箱
        /// </summary>
        [Description("邮箱")]
        public const string Email = "Email";
    }
}
