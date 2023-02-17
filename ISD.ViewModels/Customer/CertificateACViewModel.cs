using ISD.EntityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class CertificateACViewModel : BaseClassViewModel
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_Certificate")]
        public System.Guid CertificateId { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Customer")]
        public Nullable<System.Guid> ProfileId { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Content")]
        public string Content { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Certificate_StartDate")]
        public Nullable<System.DateTime> StartDate { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Certificate_EndDate")]
        public Nullable<System.DateTime> EndDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateBy")]
        public string CreateByName { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "LastEditBy")]
        public string LastEditByName { get; set; }
    }
}
