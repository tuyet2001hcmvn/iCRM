using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.Sale
{
    public class PaymentMethodViewModel
    {
        public System.Guid PaymentMethodId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PaymentMethodCode")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string PaymentMethodCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PaymentMethodAccount")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string PaymentMethodAccount { get; set; }

        //Loại: chuyển khoản/trả góp
        public Nullable<int> PaymentMethodType { get; set; }

        //Chi nhánh
        public string SaleOrg { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "OrderIndex")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Validation_OrderIndex")]
        public Nullable<int> OrderIndex { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public Nullable<bool> Actived { get; set; }
    }
}
