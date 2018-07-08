using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace X.Entity
{
    [DataContract]
    [Table("Log")]
    public class Log : BaseEntity
    {
        [DataMember]
        [Required]
        [StringLength(50)]
        public string Tenant { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [Required]
        [StringLength(50)]
        public string Remark { get; set; }
   

    }
}
