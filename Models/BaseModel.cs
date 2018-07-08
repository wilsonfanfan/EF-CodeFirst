using System;
using System.Runtime.Serialization;

namespace X.Models
{
    /// <summary>
    /// 可持久化到数据库的数据模型基类
    /// </summary>
    [Serializable]
    public class BaseModel
    {
        /// <summary>
        /// ID
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; }
    }
}
