using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Models
{
    /// <summary>
    /// 网站基本信息
    /// </summary>
    public class SiteConfig
    {
        /// <summary>
        /// 网站名称
        /// </summary>
        public string WebName { get; set; }
        /// <summary>
        /// 网站域名
        /// </summary>
        public string WebUrl { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string WebTel { get; set; }
        /// <summary>
        /// 管理员邮箱
        /// </summary>
        public string WebMail { get; set; }
        /// <summary>
        /// 网站备案号
        /// </summary>
        public string WebCrod { get; set; }
        /// <summary>
        /// 网站首页标题
        /// </summary>
        public string WebTitle { get; set; }
        /// <summary>
        /// 页面关健词
        /// </summary>
        public string WebKeyword { get; set; }
        /// <summary>
        /// 页面描述
        /// </summary>
        public string WebDescription { get; set; }
        /// <summary>
        /// 网站版权信息
        /// </summary>
        public string WebCopyright { get; set; }
        /// <summary>
        /// 网站页脚
        /// </summary>
        public string WebFooter { get; set; }
        /// <summary>
        /// bbs域名
        /// </summary>
        public string BBSUrl { get; set; }
    }
}
