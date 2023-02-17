using System;
using System.ComponentModel.DataAnnotations;

namespace ISD.ViewModels.Marketing
{
    public class ContentEditViewModel
    {
        public Guid Id { get; set; }
        [Required]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ContentCode")]
        public int ContentCode { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Marketing_Content_CatalogCode")]
        public string CatalogCode { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Marketing_Content_Email_Type")]
        public string EmailType { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SentFrom")]
        public string SentFrom { get; set; }
        [Required]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ContentName")]
        public string ContentName { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FromEmailAccount")]
        public string FromEmailAccount { get; set; }
        [Required]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FromEmailAccount")]
        public string FromEmailAccountId { get; set; }
        public string Domain { get; set; }
        [Required]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MailSenderName")]
        public string SenderName { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Company")]
        [Required]
        public string CompanyCode { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrg")]
        public string SaleOrg { get; set; }
        [Required]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Subject")]
        public string Subject { get; set; }
        [Required]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MailContent")]
        public string Content { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public bool? Actived { get; set; }
        //public Guid? CreateBy { get; set; }
        //public DateTime? CreateTime { get; set; }
        //public Guid? LastEditBy { get; set; }
        //public DateTime? LastEditTime { get; set; }
    }
}
