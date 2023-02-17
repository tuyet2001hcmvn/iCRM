using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class MobileScreenViewModel
    {
        public System.Guid MobileScreenId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ScreenName")]
        public string ScreenName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ScreenCode")]
        public string ScreenCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Permission_MenuModel")]
        public System.Guid MenuId { get; set; }

        public string IconType { get; set; }
        public string IconName { get; set; }

        public Nullable<int> OrderIndex { get; set; }

        public List<FunctionViewModel> FunctionViewModels { get; set; }
    }
}
