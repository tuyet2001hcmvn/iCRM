using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class ContactDetailViewModel
    {
        public Guid ContactDetailId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_Subject")]
        public string Title { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Configuration_DisplayPhone")]
        public string DisplayPhone { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Configuration_Phone")]
        public string Phone { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "OrderIndex")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Validation_OrderIndex")]
        public Nullable<int> OrderIndex { get; set; }
    }
}
