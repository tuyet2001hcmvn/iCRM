using System;
using System.ComponentModel.DataAnnotations;

namespace ISD.API.ViewModels.MarketingViewModels.ContentViewModels
{
    public class ContentInfoViewModel
    {
        //Loại nội dung: EMAIL | SMS
        public string CatalogCode { get; set; }
        //Email cá nhân | Email có sẵn
        public string EmailType { get; set; }
        [Required]
        public string ContentName { get; set; }
        [Required]
        public string FromEmailAccount { get; set; }
        [Required]
        public string SenderName { get; set; }
        public string SaleOrg { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Content { get; set; }    
    }
}
