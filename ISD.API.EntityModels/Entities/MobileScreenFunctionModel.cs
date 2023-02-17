using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("MobileScreenFunctionModel", Schema = "ghMasterData")]
    public partial class MobileScreenFunctionModel
    {
        [Key]
        public Guid MobileScreenId { get; set; }
        [Key]
        [StringLength(50)]
        public string FunctionId { get; set; }

        [ForeignKey(nameof(FunctionId))]
        [InverseProperty(nameof(FunctionModel.MobileScreenFunctionModels))]
        public virtual FunctionModel Function { get; set; }
        [ForeignKey(nameof(MobileScreenId))]
        [InverseProperty(nameof(MobileScreenModel.MobileScreenFunctionModels))]
        public virtual MobileScreenModel MobileScreen { get; set; }
    }
}
