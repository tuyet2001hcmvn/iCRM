using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class TargetGroupSearchModel
    {
        public Guid Id { get; set; }
        public string Type { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TargetGroupName")]
        public string TargetGroupName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TargetGroupCode")]
        public int? TargetGroupCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public bool? Actived { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateBy")]
        public string CreateBy { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateTime")]
        public bool CreateTime { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "InternalCustomerQuantity")]
        public string InternalCustomerQuantity { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ExternalCustomerQuantity")]
        public string ExternalCustomerQuantity { get; set; }
    }
}
