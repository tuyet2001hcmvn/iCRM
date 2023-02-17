using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("SalesEmployeeModel", Schema = "tMasterData")]
    public partial class SalesEmployeeModel
    {
        public SalesEmployeeModel()
        {
            DeliveryModels = new HashSet<DeliveryModel>();
            InventoryModels = new HashSet<InventoryModel>();
            PersonInChargeModels = new HashSet<PersonInChargeModel>();
            StockReceivingMasterModels = new HashSet<StockReceivingMasterModel>();
            TransferModels = new HashSet<TransferModel>();
        }

        [Key]
        [StringLength(50)]
        public string SalesEmployeeCode { get; set; }
        public Guid? CompanyId { get; set; }
        public Guid? StoreId { get; set; }
        public Guid? DepartmentId { get; set; }
        [StringLength(1000)]
        public string SalesEmployeeName { get; set; }
        [StringLength(200)]
        public string SalesEmployeeShortName { get; set; }
        [StringLength(100)]
        public string AbbreviatedName { get; set; }
        [StringLength(100)]
        public string Email { get; set; }
        [StringLength(100)]
        public string Phone { get; set; }
        public Guid? CreateBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateTime { get; set; }
        public Guid? LastEditBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LastEditTime { get; set; }
        public bool? Actived { get; set; }

        [ForeignKey(nameof(CompanyId))]
        [InverseProperty(nameof(CompanyModel.SalesEmployeeModels))]
        public virtual CompanyModel Company { get; set; }
        [ForeignKey(nameof(DepartmentId))]
        [InverseProperty(nameof(DepartmentModel.SalesEmployeeModels))]
        public virtual DepartmentModel Department { get; set; }
        [ForeignKey(nameof(StoreId))]
        [InverseProperty(nameof(StoreModel.SalesEmployeeModels))]
        public virtual StoreModel Store { get; set; }
        [InverseProperty(nameof(DeliveryModel.SalesEmployeeCodeNavigation))]
        public virtual ICollection<DeliveryModel> DeliveryModels { get; set; }
        [InverseProperty(nameof(InventoryModel.SalesEmployeeCodeNavigation))]
        public virtual ICollection<InventoryModel> InventoryModels { get; set; }
        [InverseProperty(nameof(PersonInChargeModel.SalesEmployeeCodeNavigation))]
        public virtual ICollection<PersonInChargeModel> PersonInChargeModels { get; set; }
        [InverseProperty(nameof(StockReceivingMasterModel.SalesEmployeeCodeNavigation))]
        public virtual ICollection<StockReceivingMasterModel> StockReceivingMasterModels { get; set; }
        [InverseProperty(nameof(TransferModel.SalesEmployeeCodeNavigation))]
        public virtual ICollection<TransferModel> TransferModels { get; set; }
    }
}
