using System;

namespace ISD.API.ViewModels.MarketingViewModels.CampaignViewModels
{
    public class CampaignStatusViewModel
    {
        public Guid CatalogId { get; set; }
        public string CatalogTypeCode { get; set; }
        public string CatalogCode { get; set; }
        public string CatalogTextEn { get; set; }
        public string CatalogTextVi { get; set; }
        public int? OrderIndex { get; set; }
        public bool? Actived { get; set; }
    }
}
