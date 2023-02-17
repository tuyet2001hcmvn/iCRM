using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("StockReceivingMasterModel", Schema = "Warehouse")]
    public partial class StockReceivingMasterModel
    {
        public StockReceivingMasterModel()
        {
            StockReceivingDetailModels = new HashSet<StockReceivingDetailModel>();
        }

        [Key]
        public Guid StockReceivingId { get; set; }
        public int StockReceivingCode { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DocumentDate { get; set; }
        public Guid? CompanyId { get; set; }
        public Guid? StoreId { get; set; }
        [StringLength(50)]
        public string SalesEmployeeCode { get; set; }
        public Guid? ProfileId { get; set; }
        public string Note { get; set; }
        public Guid? CreateBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateTime { get; set; }
        public Guid? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedTime { get; set; }
        [Column("isDeleted")]
        public bool? IsDeleted { get; set; }
        [Column("isFirst")]
        public bool? IsFirst { get; set; }
        [StringLength(4000)]
        public string DeletedReason { get; set; }

        [ForeignKey(nameof(CompanyId))]
        [InverseProperty(nameof(CompanyModel.StockReceivingMasterModels))]
        public virtual CompanyModel Company { get; set; }
        [ForeignKey(nameof(ProfileId))]
        [InverseProperty(nameof(ProfileModel.StockReceivingMasterModels))]
        public virtual ProfileModel Profile { get; set; }
        [ForeignKey(nameof(SalesEmployeeCode))]
        [InverseProperty(nameof(SalesEmployeeModel.StockReceivingMasterModels))]
        public virtual SalesEmployeeModel SalesEmployeeCodeNavigation { get; set; }
        [ForeignKey(nameof(StoreId))]
        [InverseProperty(nameof(StoreModel.StockReceivingMasterModels))]
        public virtual StoreModel Store { get; set; }
        [InverseProperty(nameof(StockReceivingDetailModel.StockReceiving))]
        public virtual ICollection<StockReceivingDetailModel> StockReceivingDetailModels { get; set; }
    }
}
