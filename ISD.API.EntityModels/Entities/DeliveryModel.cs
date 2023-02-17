using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("DeliveryModel", Schema = "Warehouse")]
    public partial class DeliveryModel
    {
        public DeliveryModel()
        {
            DeliveryDetailModels = new HashSet<DeliveryDetailModel>();
        }

        [Key]
        public Guid DeliveryId { get; set; }
        public int DeliveryCode { get; set; }
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
        [StringLength(500)]
        public string RecipientCompany { get; set; }
        [StringLength(500)]
        public string RecipientName { get; set; }
        [StringLength(4000)]
        public string RecipientAddress { get; set; }
        [StringLength(200)]
        public string RecipientPhone { get; set; }
        [StringLength(500)]
        public string SenderName { get; set; }
        [StringLength(4000)]
        public string SenderAddress { get; set; }
        [StringLength(200)]
        public string SenderPhone { get; set; }
        public Guid? LastEditBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LastEditTime { get; set; }
        public Guid? TaskId { get; set; }
        [StringLength(4000)]
        public string DeletedReason { get; set; }
        [StringLength(200)]
        public string DeliveryType { get; set; }
        [StringLength(50)]
        public string ShippingTypeCode { get; set; }

        [ForeignKey(nameof(CompanyId))]
        [InverseProperty(nameof(CompanyModel.DeliveryModels))]
        public virtual CompanyModel Company { get; set; }
        [ForeignKey(nameof(ProfileId))]
        [InverseProperty(nameof(ProfileModel.DeliveryModels))]
        public virtual ProfileModel Profile { get; set; }
        [ForeignKey(nameof(SalesEmployeeCode))]
        [InverseProperty(nameof(SalesEmployeeModel.DeliveryModels))]
        public virtual SalesEmployeeModel SalesEmployeeCodeNavigation { get; set; }
        [ForeignKey(nameof(StoreId))]
        [InverseProperty(nameof(StoreModel.DeliveryModels))]
        public virtual StoreModel Store { get; set; }
        [InverseProperty(nameof(DeliveryDetailModel.Delivery))]
        public virtual ICollection<DeliveryDetailModel> DeliveryDetailModels { get; set; }
    }
}
