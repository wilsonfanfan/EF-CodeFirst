using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Models
{
    /// <summary>
    /// 短信平台设置
    /// </summary>
    public class SmsConfig
    {
        /// <summary>
        /// 短信API地址
        /// </summary>
        public string ApiBaseUrl { get; set; }
        /// <summary>
        /// 短信平台登录账户名
        /// </summary>
        public string AccountName { get; set; }
        /// <summary>
        /// 短信平台登录密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 企业编号
        /// </summary>
        public string CorpID { get; set; }
        /// <summary>
        /// 企业编号
        /// </summary>
        public string ProductID { get; set; }

        /// <summary>
        /// 短信模板 {code}  {date}  {time}
        /// </summary>
        public string SendTemplate { get; set; }

        /// <summary>
        /// 间隔时间（秒）
        /// </summary>
        public int IntervalTime { get; set; }

        /// <summary>
        /// 有效时间（分钟）
        /// </summary>
        public int ValidTime { get; set; }

        /// <summary>
        /// IP限制次数(每天)
        /// </summary>
        public int IPLimitNumber { get; set; }
    }
}
