using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("ProfileGroupModel", Schema = "Customer")]
    public partial class ProfileGroupModel
    {
        [Key]
        public Guid ProfileGroupId { get; set; }
        public Guid? ProfileId { get; set; }
        [StringLength(50)]
        public string CompanyCode { get; set; }
        [StringLength(50)]
        public string ProfileGroupCode { get; set; }
        public Guid? CreateBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateTime { get; set; }
        public Guid? LastEditBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LastEditTime { get; set; }

        [ForeignKey(nameof(ProfileId))]
        [InverseProperty(nameof(ProfileModel.ProfileGroupModels))]
        public virtual ProfileModel Profile { get; set; }
    }
}
