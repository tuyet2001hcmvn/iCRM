using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("MemberOfTargetGroupModel", Schema = "Marketing")]
    public partial class MemberOfTargetGroupModel
    {
        [Key]
        public Guid TargetGroupId { get; set; }
        [Key]
        public Guid ProfileId { get; set; }

        [ForeignKey(nameof(TargetGroupId))]
        [InverseProperty(nameof(TargetGroupModel.MemberOfTargetGroupModels))]
        public virtual TargetGroupModel TargetGroup { get; set; }
    }
}
