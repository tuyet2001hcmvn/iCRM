using System;
using System.Collections.Generic;

#nullable disable

namespace ISD.API.EntityModels.Test
{
    public partial class AccountModel
    {
        public AccountModel()
        {
            TemplateAndGiftCampaignModelCreateByNavigations = new HashSet<TemplateAndGiftCampaignModel>();
            TemplateAndGiftCampaignModelLastEditByNavigations = new HashSet<TemplateAndGiftCampaignModel>();
            TemplateAndGiftMemberAddressModelCreateByNavigations = new HashSet<TemplateAndGiftMemberAddressModel>();
            TemplateAndGiftMemberAddressModelLastEditByNavigations = new HashSet<TemplateAndGiftMemberAddressModel>();
            TemplateAndGiftTargetGroupModelCreateByNavigations = new HashSet<TemplateAndGiftTargetGroupModel>();
            TemplateAndGiftTargetGroupModelLastEditByNavigations = new HashSet<TemplateAndGiftTargetGroupModel>();
        }

        public Guid AccountId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public string EmployeeCode { get; set; }
        public bool? IsShowChoseModule { get; set; }
        public bool? IsViewTotal { get; set; }
        public bool? IsViewByWarehouse { get; set; }
        public bool? IsReceiveNotification { get; set; }
        public string TaskFilterCode { get; set; }
        public string DeviceId { get; set; }
        public DateTime? LastLogin { get; set; }
        public string OneSignalWebId { get; set; }
        public bool? Actived { get; set; }
        public bool? IsShowDashboard { get; set; }
        public bool? IsViewByStore { get; set; }
        public bool? IsCreatePrivateTask { get; set; }
        public bool? IsViewRevenue { get; set; }
        public string ViewPermission { get; set; }

        public virtual ICollection<TemplateAndGiftCampaignModel> TemplateAndGiftCampaignModelCreateByNavigations { get; set; }
        public virtual ICollection<TemplateAndGiftCampaignModel> TemplateAndGiftCampaignModelLastEditByNavigations { get; set; }
        public virtual ICollection<TemplateAndGiftMemberAddressModel> TemplateAndGiftMemberAddressModelCreateByNavigations { get; set; }
        public virtual ICollection<TemplateAndGiftMemberAddressModel> TemplateAndGiftMemberAddressModelLastEditByNavigations { get; set; }
        public virtual ICollection<TemplateAndGiftTargetGroupModel> TemplateAndGiftTargetGroupModelCreateByNavigations { get; set; }
        public virtual ICollection<TemplateAndGiftTargetGroupModel> TemplateAndGiftTargetGroupModelLastEditByNavigations { get; set; }
    }
}
