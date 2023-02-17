using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("PageFunctionModel", Schema = "pms")]
    public partial class PageFunctionModel
    {
        [Key]
        public Guid PageId { get; set; }
        [Key]
        [StringLength(50)]
        public string FunctionId { get; set; }

        [ForeignKey(nameof(FunctionId))]
        [InverseProperty(nameof(FunctionModel.PageFunctionModels))]
        public virtual FunctionModel Function { get; set; }
        [ForeignKey(nameof(PageId))]
        [InverseProperty(nameof(PageModel.PageFunctionModels))]
        public virtual PageModel Page { get; set; }
    }
}
