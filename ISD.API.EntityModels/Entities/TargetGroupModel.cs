using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("TargetGroupModel", Schema = "Marketing")]
    public partial class TargetGroupModel
    {
        public TargetGroupModel()
        {
            MemberOfExternalProfileTargetGroupModels = new HashSet<MemberOfExternalProfileTargetGroupModel>();
            MemberOfTargetGroupModels = new HashSet<MemberOfTargetGroupModel>();
        }

        [Key]
        public Guid Id { get; set; }
        public int TargetGroupCode { get; set; }
        [StringLength(50)]
        public string Type { get; set; }
        [Required]
        [StringLength(50)]
        public string TargetGroupName { get; set; }
        public Guid CreateBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreateTime { get; set; }
        public Guid? LastEditBy { get; set; }
        public bool Actived { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LastEditTime { get; set; }

        [ForeignKey(nameof(CreateBy))]
        [InverseProperty(nameof(AccountModel.TargetGroupModelCreateByNavigations))]
        public virtual AccountModel CreateByNavigation { get; set; }
        [ForeignKey(nameof(LastEditBy))]
        [InverseProperty(nameof(AccountModel.TargetGroupModelLastEditByNavigations))]
        public virtual AccountModel LastEditByNavigation { get; set; }
        [InverseProperty(nameof(MemberOfExternalProfileTargetGroupModel.TargetGroup))]
        public virtual ICollection<MemberOfExternalProfileTargetGroupModel> MemberOfExternalProfileTargetGroupModels { get; set; }
        [InverseProperty(nameof(MemberOfTargetGroupModel.TargetGroup))]
        public virtual ICollection<MemberOfTargetGroupModel> MemberOfTargetGroupModels { get; set; }
    }
}
