using ISD.Constant;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class BaseClassViewModel
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateBy")]
        public Nullable<System.Guid> CreateBy { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateTime")]
        [DisplayFormat(DataFormatString = SystemConfig.DateTimeFormat)]
        public Nullable<System.DateTime> CreateTime { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "LastEditBy")]
        public Nullable<System.Guid> LastEditBy { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "LastEditTime")]
        [DisplayFormat(DataFormatString = SystemConfig.DateTimeFormat)]
        public Nullable<System.DateTime> LastEditTime { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreateBy")]
        public string CreateUser { get; set; }

    }
}
