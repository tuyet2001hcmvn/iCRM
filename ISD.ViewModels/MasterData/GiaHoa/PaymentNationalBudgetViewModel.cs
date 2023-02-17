using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.Sale
{
    #region Tài khoản nộp NSNN
    public class PaymentNationalBudgetViewModel
    {
        public Guid PaymentNationalId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PaymentNationalCode")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string PaymentNationalCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "BankName")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string BankName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "AccountId")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string Account { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public bool? Actived { get; set; }
    }
    #endregion

    #region Tài khoản Kho bạc Nhà nước
    public class StateTreasuryViewModel
    {
        public Guid StateTreasuryId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PaymentNationalCode")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string StateTreasuryCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "BankName")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string BankName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "AccountId")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string Account { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public bool? Actived { get; set; }
    }
    #endregion

    #region Cơ quan quản lý thu NSNN
    public class CollectingAuthorityViewModel
    {
        public Guid CollectingAuthorityId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CollectingAuthorityCode")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string CollectingAuthorityCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CollectingAuthorityName")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string CollectingAuthorityName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public bool? Actived { get; set; }
    }
    #endregion
}
