using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("StoreTypeModel", Schema = "tMasterData")]
    public partial class StoreTypeModel
    {
        [Key]
        public Guid StoreTypeId { get; set; }
        [StringLength(500)]
        public string StoreTypeName { get; set; }
        public int? OrderIndex { get; set; }
        public bool Actived { get; set; }
    }
}
