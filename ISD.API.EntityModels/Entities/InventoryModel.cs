using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("InventoryModel", Schema = "Warehouse")]
    public partial class InventoryModel
    {
        public InventoryModel()
        {
            DeliveryDetailModels = new HashSet<DeliveryDetailModel>();
            StockReceivingDetailModels = new HashSet<StockReceivingDetailModel>();
        }

        [Key]
        public Guid InventoryId { get; set; }
        public int InventoryCode { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DocumentDate { get; set; }
        public Guid? CompanyId { get; set; }
        public Guid? StoreId { get; set; }
        [StringLength(50)]
        public string SalesEmployeeCode { get; set; }
        public string Note { get; set; }
        public Guid? CreateBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateTime { get; set; }
        public Guid? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedTime { get; set; }
        [Column("isDeleted")]
        public bool? IsDeleted { get; set; }
        [StringLength(4000)]
        public string DeletedReason { get; set; }

        [ForeignKey(nameof(CompanyId))]
        [InverseProperty(nameof(CompanyModel.InventoryModels))]
        public virtual CompanyModel Company { get; set; }
        [ForeignKey(nameof(SalesEmployeeCode))]
        [InverseProperty(nameof(SalesEmployeeModel.InventoryModels))]
        public virtual SalesEmployeeModel SalesEmployeeCodeNavigation { get; set; }
        [ForeignKey(nameof(StoreId))]
        [InverseProperty(nameof(StoreModel.InventoryModels))]
        public virtual StoreModel Store { get; set; }
        [InverseProperty(nameof(DeliveryDetailModel.Inventory))]
        public virtual ICollection<DeliveryDetailModel> DeliveryDetailModels { get; set; }
        [InverseProperty(nameof(StockReceivingDetailModel.Inventory))]
        public virtual ICollection<StockReceivingDetailModel> StockReceivingDetailModels { get; set; }
    }
}
