using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Models
{
    /// <summary>
    /// 邮件发送设置
    /// </summary>
    public class EmailConfig
    {
        public EmailConfig()
        {
            Port = 25;
        }
        /// <summary>
        /// STMP服务器
        /// </summary>
        public string EmailSmtp { get; set; }
        /// <summary>
        /// SSL加密连接
        /// </summary>
        public int IsSsl { get; set; }
        /// <summary>
        /// SMTP端口
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// 发件人地址
        /// </summary>
        public string EmailFrom { get; set; }
        /// <summary>
        /// 邮箱账号
        /// </summary>
        public string AccountName { get; set; }
        /// <summary>
        /// 邮箱密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 发件人昵称
        /// </summary>
        public string NickName { get; set; }
    }
}
