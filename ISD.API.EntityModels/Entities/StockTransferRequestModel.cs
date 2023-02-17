using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("StockTransferRequestModel", Schema = "Warehouse")]
    public partial class StockTransferRequestModel
    {
        public StockTransferRequestModel()
        {
            StockTransferRequestDetailModels = new HashSet<StockTransferRequestDetailModel>();
        }

        [Key]
        public Guid Id { get; set; }
        public int StockTransferRequestCode { get; set; }
        public Guid? FromStock { get; set; }
        public Guid? ToStock { get; set; }
        public Guid? CompanyId { get; set; }
        public Guid? StoreId { get; set; }
        [StringLength(500)]
        public string Note { get; set; }
        public bool? IsDelete { get; set; }
        public bool? Actived { get; set; }
        public Guid? CreateBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateTime { get; set; }
        public Guid? LastEditBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LastEditTime { get; set; }
        [StringLength(50)]
        public string SalesEmployeeCode { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? FromPlanDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ToPlanDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DocumentDate { get; set; }
        public Guid? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedTime { get; set; }
        [StringLength(500)]
        public string SenderName { get; set; }
        [StringLength(20)]
        public string SenderPhone { get; set; }
        [StringLength(500)]
        public string RecipientName { get; set; }
        [StringLength(20)]
        public string RecipientPhone { get; set; }
        [StringLength(4000)]
        public string RecipientAddress { get; set; }
        [StringLength(500)]
        public string RecipientCompany { get; set; }
        [StringLength(4000)]
        public string SenderAddress { get; set; }
        [StringLength(4000)]
        public string DeletedReason { get; set; }

        [ForeignKey(nameof(CompanyId))]
        [InverseProperty(nameof(CompanyModel.StockTransferRequestModels))]
        public virtual CompanyModel Company { get; set; }
        [ForeignKey(nameof(CreateBy))]
        [InverseProperty(nameof(AccountModel.StockTransferRequestModelCreateByNavigations))]
        public virtual AccountModel CreateByNavigation { get; set; }
        [ForeignKey(nameof(DeletedBy))]
        [InverseProperty(nameof(AccountModel.StockTransferRequestModelDeletedByNavigations))]
        public virtual AccountModel DeletedByNavigation { get; set; }
        [ForeignKey(nameof(FromStock))]
        [InverseProperty(nameof(StockModel.StockTransferRequestModelFromStockNavigations))]
        public virtual StockModel FromStockNavigation { get; set; }
        [ForeignKey(nameof(LastEditBy))]
        [InverseProperty(nameof(AccountModel.StockTransferRequestModelLastEditByNavigations))]
        public virtual AccountModel LastEditByNavigation { get; set; }
        [ForeignKey(nameof(StoreId))]
        [InverseProperty(nameof(StoreModel.StockTransferRequestModels))]
        public virtual StoreModel Store { get; set; }
        [ForeignKey(nameof(ToStock))]
        [InverseProperty(nameof(StockModel.StockTransferRequestModelToStockNavigations))]
        public virtual StockModel ToStockNavigation { get; set; }
        [InverseProperty(nameof(StockTransferRequestDetailModel.StockTransferRequest))]
        public virtual ICollection<StockTransferRequestDetailModel> StockTransferRequestDetailModels { get; set; }
    }
}
