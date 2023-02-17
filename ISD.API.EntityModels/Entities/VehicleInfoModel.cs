using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("VehicleInfoModel", Schema = "ghService")]
    public partial class VehicleInfoModel
    {
        public VehicleInfoModel()
        {
            AccessorySaleOrderModels = new HashSet<AccessorySaleOrderModel>();
            ServiceOrderModels = new HashSet<ServiceOrderModel>();
        }

        [Key]
        public Guid VehicleId { get; set; }
        public Guid? CustomerId { get; set; }
        [StringLength(200)]
        public string LicensePlate { get; set; }
        [StringLength(200)]
        public string SerialNumber { get; set; }
        [StringLength(200)]
        public string EngineNumber { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? CurrentKilometers { get; set; }
        [StringLength(200)]
        public string WarrantyNumber { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? BuyDate { get; set; }
        [StringLength(50)]
        public string SaleOrg { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        [StringLength(100)]
        public string ProfitCenterCode { get; set; }
        [StringLength(100)]
        public string ProductHierarchyCode { get; set; }
        [StringLength(200)]
        public string SoBinhDien { get; set; }

        [InverseProperty(nameof(AccessorySaleOrderModel.Vehicle))]
        public virtual ICollection<AccessorySaleOrderModel> AccessorySaleOrderModels { get; set; }
        [InverseProperty(nameof(ServiceOrderModel.Vehicle))]
        public virtual ICollection<ServiceOrderModel> ServiceOrderModels { get; set; }
    }
}
