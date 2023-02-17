using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("TransferModel", Schema = "Warehouse")]
    public partial class TransferModel
    {
        [Key]
        public Guid TransferId { get; set; }
        public int TransferCode { get; set; }
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
        public Guid? StockTransferRequestId { get; set; }

        [ForeignKey(nameof(CompanyId))]
        [InverseProperty(nameof(CompanyModel.TransferModels))]
        public virtual CompanyModel Company { get; set; }
        [ForeignKey(nameof(SalesEmployeeCode))]
        [InverseProperty(nameof(SalesEmployeeModel.TransferModels))]
        public virtual SalesEmployeeModel SalesEmployeeCodeNavigation { get; set; }
        [ForeignKey(nameof(StoreId))]
        [InverseProperty(nameof(StoreModel.TransferModels))]
        public virtual StoreModel Store { get; set; }
    }
}
