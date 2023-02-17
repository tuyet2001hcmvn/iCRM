using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("ShowroomCategoryModel", Schema = "tSale")]
    public partial class ShowroomCategoryModel
    {
        [Key]
        public int ShowroomCategoryId { get; set; }
        [StringLength(50)]
        public string CompanyCode { get; set; }
        [StringLength(50)]
        public string SaleOrgCode { get; set; }
        [StringLength(50)]
        public string MaterialCode { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? StartDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? EndDate { get; set; }
        [Column("MaTB")]
        [StringLength(50)]
        public string MaTb { get; set; }
        public bool Actived { get; set; }
    }
}
