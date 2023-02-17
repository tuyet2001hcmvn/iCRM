using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("MemberOfExternalProfileTargetGroupModel", Schema = "Marketing")]
    public partial class MemberOfExternalProfileTargetGroupModel
    {
        [Key]
        public Guid ExternalProfileTargetGroupId { get; set; }
        public Guid? TargetGroupId { get; set; }
        [StringLength(500)]
        public string FullName { get; set; }
        [StringLength(500)]
        public string Email { get; set; }
        [StringLength(500)]
        public string Phone { get; set; }

        [ForeignKey(nameof(TargetGroupId))]
        [InverseProperty(nameof(TargetGroupModel.MemberOfExternalProfileTargetGroupModels))]
        public virtual TargetGroupModel TargetGroup { get; set; }
    }
}
