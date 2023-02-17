using ISD.EntityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class SponsViewModel : BaseClassViewModel
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_Spons")]
        public System.Guid SponsId { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Customer")]
        public Nullable<System.Guid> ProfileId { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Descriptions")]
        public string Descriptions { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Spons_Value")]
        [DisplayFormat(DataFormatString = "{0:n0}")]
        public Nullable<System.Decimal> Value { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Spons_Time")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> Time { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Spons_Type")]
        public string Type { get; set; }
    }
}
