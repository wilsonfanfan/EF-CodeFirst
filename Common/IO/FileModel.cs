using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Common.IO
{
    /// <summary>
    /// 文件模型
    /// </summary>
    [Serializable]
    public class FileModel
    {
        /// <summary>
        /// 名称
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// 虚拟路径
        /// </summary>
        public String Url { get; set; }

        /// <summary>
        /// 物理路径
        /// </summary>
        public String Path { get; set; }

        /// <summary>
        /// 文件大小B
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        /// 文件扩展名
        /// </summary>
        public String Extension { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; }
        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime LastWriteTime { get; set; }
        /// <summary>
        /// 完整名称
        /// </summary>
        public String FullName { get; set; }
    }
}
