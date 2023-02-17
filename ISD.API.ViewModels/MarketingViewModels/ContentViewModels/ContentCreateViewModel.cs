using System;
using System.ComponentModel.DataAnnotations;

namespace ISD.API.ViewModels.MarketingViewModels.ContentViewModels
{
    public class ContentCreateViewModel
    {        
        //Loại nội dung: EMAIL | SMS
        public string CatalogCode { get; set; }
        public string EmailType { get; set; }
        public string SentFrom { get; set; }
        public string Domain { get; set; }
        [Required]
        public string ContentName { get; set; }
        [Required]
        public string Type { get; set; }
        public string FromEmailAccount { get; set; }
        [Required]
        public Guid FromEmailAccountId { get; set; }
        [Required]
        public string SenderName { get; set; }
        public string CompanyCode { get; set; }
        public string SaleOrg { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Content { get; set; }
        public Guid CreateBy { get; set; }
    }
}
