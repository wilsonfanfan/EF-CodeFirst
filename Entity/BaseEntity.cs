using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace X.Entity
{
    /// <summary>
    /// 可持久化到数据库的数据模型基类
    /// </summary>
    [DataContract]
    public abstract class BaseEntity
    {
        /// <summary>
        /// ID
        /// </summary>
        [DataMember]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [DataMember]
        public DateTime CreatedTime { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        [DataMember]
        [IsIgnore]
        public long CreaterID { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        [DataMember]
        [IsIgnore]
        public DateTime? ModifiedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        [DataMember]
        [IsIgnore]
        public long ModifierID { get; set; }
        /// <summary>
        /// 删除时间
        /// </summary>
        [DataMember]
        [IsIgnore]
        public DateTime? DeletedTime { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        [DataMember]
        [IsIgnore]
        public bool IsDeleted { get; set; }
        /// <summary>
        /// 删除人
        /// </summary>
        [DataMember]
        [IsIgnore]
        public long DeleterID { get; set; }

        ///// <summary>
        ///// 获取或设置 版本控制标识，用于处理并发
        ///// </summary>
        //[ConcurrencyCheck]
        //[Timestamp]
        //[DataMember]
        //public byte[] RowVersion { get; set; }
    }
    /// <summary>
    /// Hide property generation for T4 template to generate common models
    /// </summary>
    public class IsIgnoreAttribute : Attribute
    {
    }
}
