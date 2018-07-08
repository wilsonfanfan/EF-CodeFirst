using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Constant
{
    public static class LogType
    {
        /// <summary>
        /// 登陆
        /// </summary>
        [Description("登陆")]
        public const string Logoned = "Logoned";

        /// <summary>
        /// 增加
        /// </summary>
        [Description("增加")]
        public const string Added = "Added";

        /// <summary>
        /// 修改属性 
        /// </summary>
        [Description("修改属性")]
        public const string Modified = "Modified";

        /// <summary>
        /// 修改内容
        /// </summary>
        [Description("修改内容")]
        public const string Edited = "Edited";

        /// <summary>
        /// 删除
        /// </summary>
        [Description("删除")]
        public const string Deleted = "Deleted";
    }
}
