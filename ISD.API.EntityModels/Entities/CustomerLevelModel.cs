using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("CustomerLevelModel", Schema = "tMasterData")]
    public partial class CustomerLevelModel
    {
        [Key]
        public Guid CustomerLevelId { get; set; }
        [Required]
        [StringLength(50)]
        public string CustomerLevelCode { get; set; }
        [Required]
        [StringLength(100)]
        public string CustomerLevelName { get; set; }
        public int? OrderIndex { get; set; }
        public bool Actived { get; set; }
    }
}
