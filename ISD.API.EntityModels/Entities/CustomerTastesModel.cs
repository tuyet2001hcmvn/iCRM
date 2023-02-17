using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("CustomerTastesModel", Schema = "Customer")]
    public partial class CustomerTastesModel
    {
        [Key]
        public Guid CustomerTasteId { get; set; }
        [Required]
        [Column("ERPProductCode")]
        [StringLength(20)]
        public string ErpproductCode { get; set; }
        [StringLength(50)]
        public string ProductCode { get; set; }
        [StringLength(200)]
        public string ProductName { get; set; }
        public Guid? ProfileId { get; set; }
        public Guid? CompanyId { get; set; }
        public Guid? StoreId { get; set; }
        public Guid? AppointmentId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
    }
}
