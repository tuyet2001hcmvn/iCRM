using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("CatalogTypeModel", Schema = "tMasterData")]
    public partial class CatalogTypeModel
    {
        [Key]
        [StringLength(100)]
        public string CatalogTypeCode { get; set; }
        [StringLength(100)]
        public string CatalogTypeName { get; set; }
        public bool? Actived { get; set; }
        [StringLength(20)]
        public string CategoryType { get; set; }
        [StringLength(200)]
        public string Note { get; set; }
    }
}
