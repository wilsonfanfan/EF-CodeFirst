using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace X.Entity
{
    [DataContract]
    [Table("UserAccount")]
    public class UserAccount : BaseEntity
    {
        [DataMember]
        [Required]
        [StringLength(50)]
        public string Tenant { get; set; }

        /// <summary>
        /// 是否管理
        /// </summary>
        [DataMember]
        public bool IsManage { get; set; }
        /// <summary>
        /// 是否超级管理员
        /// </summary>
        [DataMember]
        public bool IsSuperAdministrator { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [Required]
        [StringLength(50)]
        public string Remark { get; set; }
    
    }
}
