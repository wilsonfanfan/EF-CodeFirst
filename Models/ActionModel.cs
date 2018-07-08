using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Models
{
    public class ActionModel
    {
        /// <summary>
        /// 控制器
        /// </summary>
        public String AreaName { get; set; }
        /// <summary>
        /// 控制器
        /// </summary>
        public String ControlName { get; set; }
        /// <summary>
        /// 行为
        /// </summary>
        public String ActionName { get; set; }
        /// <summary>
        /// PathAndQuery
        /// </summary>
        public String PathAndQuery { get; set; }
    }
}
