using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("AccountModel", Schema = "pms")]
    [Index(nameof(UserName), Name = "UserName_UNIQUE", IsUnique = true)]
    public partial class AccountModel
    {
        public AccountModel()
        {
            AccountInRoleModels = new HashSet<AccountInRoleModel>();
            AccountInStoreModels = new HashSet<AccountInStoreModel>();
            ApplicationLogs = new HashSet<ApplicationLog>();
            ContentModelCreateByNavigations = new HashSet<ContentModel>();
            ContentModelLastEditByNavigations = new HashSet<ContentModel>();
            FavoriteReportModels = new HashSet<FavoriteReportModel>();
            NotificationAccountMappingModels = new HashSet<NotificationAccountMappingModel>();
            QuestionBankModelCreateByNavigations = new HashSet<QuestionBankModel>();
            QuestionBankModelLastEditByNavigations = new HashSet<QuestionBankModel>();
            StockTransferRequestModelCreateByNavigations = new HashSet<StockTransferRequestModel>();
            StockTransferRequestModelDeletedByNavigations = new HashSet<StockTransferRequestModel>();
            StockTransferRequestModelLastEditByNavigations = new HashSet<StockTransferRequestModel>();
            TargetGroupModelCreateByNavigations = new HashSet<TargetGroupModel>();
            TargetGroupModelLastEditByNavigations = new HashSet<TargetGroupModel>();
            TemplateAndGiftCampaignModelCreateByNavigations = new HashSet<TemplateAndGiftCampaignModel>();
            TemplateAndGiftCampaignModelLastEditByNavigations = new HashSet<TemplateAndGiftCampaignModel>();
            TemplateAndGiftMemberAddressModelCreateByNavigations = new HashSet<TemplateAndGiftMemberAddressModel>();
            TemplateAndGiftMemberAddressModelLastEditByNavigations = new HashSet<TemplateAndGiftMemberAddressModel>();
            TemplateAndGiftTargetGroupModelCreateByNavigations = new HashSet<TemplateAndGiftTargetGroupModel>();
            TemplateAndGiftTargetGroupModelLastEditByNavigations = new HashSet<TemplateAndGiftTargetGroupModel>();
        }

        [Key]
        public Guid AccountId { get; set; }
        [StringLength(100)]
        public string UserName { get; set; }
        [StringLength(200)]
        public string FullName { get; set; }
        [StringLength(100)]
        public string Password { get; set; }
        [StringLength(100)]
        public string EmployeeCode { get; set; }
        [Column("isShowChoseModule")]
        public bool? IsShowChoseModule { get; set; }
        [Column("isViewTotal")]
        public bool? IsViewTotal { get; set; }
        [Column("isViewByWarehouse")]
        public bool? IsViewByWarehouse { get; set; }
        [Column("isReceiveNotification")]
        public bool? IsReceiveNotification { get; set; }
        [StringLength(100)]
        public string TaskFilterCode { get; set; }
        [StringLength(100)]
        public string DeviceId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LastLogin { get; set; }
        [StringLength(50)]
        public string OneSignalWebId { get; set; }
        public bool? Actived { get; set; }
        [Column("isShowDashboard")]
        public bool? IsShowDashboard { get; set; }
        [Column("isViewByStore")]
        public bool? IsViewByStore { get; set; }
        [Column("isCreatePrivateTask")]
        public bool? IsCreatePrivateTask { get; set; }
        [Column("isViewRevenue")]
        public bool? IsViewRevenue { get; set; }
        [StringLength(50)]
        public string ViewPermission { get; set; }
        [Column("isViewOpportunity")]
        public bool? IsViewOpportunity { get; set; }
        [StringLength(500)]
        public string GoogleCalendarId { get; set; }

        [InverseProperty(nameof(AccountInRoleModel.Account))]
        public virtual ICollection<AccountInRoleModel> AccountInRoleModels { get; set; }
        [InverseProperty(nameof(AccountInStoreModel.Account))]
        public virtual ICollection<AccountInStoreModel> AccountInStoreModels { get; set; }
        [InverseProperty(nameof(ApplicationLog.PerformedByAccount))]
        public virtual ICollection<ApplicationLog> ApplicationLogs { get; set; }
        [InverseProperty(nameof(ContentModel.CreateByNavigation))]
        public virtual ICollection<ContentModel> ContentModelCreateByNavigations { get; set; }
        [InverseProperty(nameof(ContentModel.LastEditByNavigation))]
        public virtual ICollection<ContentModel> ContentModelLastEditByNavigations { get; set; }
        [InverseProperty(nameof(FavoriteReportModel.Account))]
        public virtual ICollection<FavoriteReportModel> FavoriteReportModels { get; set; }
        [InverseProperty(nameof(NotificationAccountMappingModel.Account))]
        public virtual ICollection<NotificationAccountMappingModel> NotificationAccountMappingModels { get; set; }
        [InverseProperty(nameof(QuestionBankModel.CreateByNavigation))]
        public virtual ICollection<QuestionBankModel> QuestionBankModelCreateByNavigations { get; set; }
        [InverseProperty(nameof(QuestionBankModel.LastEditByNavigation))]
        public virtual ICollection<QuestionBankModel> QuestionBankModelLastEditByNavigations { get; set; }
        [InverseProperty(nameof(StockTransferRequestModel.CreateByNavigation))]
        public virtual ICollection<StockTransferRequestModel> StockTransferRequestModelCreateByNavigations { get; set; }
        [InverseProperty(nameof(StockTransferRequestModel.DeletedByNavigation))]
        public virtual ICollection<StockTransferRequestModel> StockTransferRequestModelDeletedByNavigations { get; set; }
        [InverseProperty(nameof(StockTransferRequestModel.LastEditByNavigation))]
        public virtual ICollection<StockTransferRequestModel> StockTransferRequestModelLastEditByNavigations { get; set; }
        [InverseProperty(nameof(TargetGroupModel.CreateByNavigation))]
        public virtual ICollection<TargetGroupModel> TargetGroupModelCreateByNavigations { get; set; }
        [InverseProperty(nameof(TargetGroupModel.LastEditByNavigation))]
        public virtual ICollection<TargetGroupModel> TargetGroupModelLastEditByNavigations { get; set; }
        [InverseProperty(nameof(TemplateAndGiftCampaignModel.CreateByNavigation))]
        public virtual ICollection<TemplateAndGiftCampaignModel> TemplateAndGiftCampaignModelCreateByNavigations { get; set; }
        [InverseProperty(nameof(TemplateAndGiftCampaignModel.LastEditByNavigation))]
        public virtual ICollection<TemplateAndGiftCampaignModel> TemplateAndGiftCampaignModelLastEditByNavigations { get; set; }
        [InverseProperty(nameof(TemplateAndGiftMemberAddressModel.CreateByNavigation))]
        public virtual ICollection<TemplateAndGiftMemberAddressModel> TemplateAndGiftMemberAddressModelCreateByNavigations { get; set; }
        [InverseProperty(nameof(TemplateAndGiftMemberAddressModel.LastEditByNavigation))]
        public virtual ICollection<TemplateAndGiftMemberAddressModel> TemplateAndGiftMemberAddressModelLastEditByNavigations { get; set; }
        [InverseProperty(nameof(TemplateAndGiftTargetGroupModel.CreateByNavigation))]
        public virtual ICollection<TemplateAndGiftTargetGroupModel> TemplateAndGiftTargetGroupModelCreateByNavigations { get; set; }
        [InverseProperty(nameof(TemplateAndGiftTargetGroupModel.LastEditByNavigation))]
        public virtual ICollection<TemplateAndGiftTargetGroupModel> TemplateAndGiftTargetGroupModelLastEditByNavigations { get; set; }
    }
}
