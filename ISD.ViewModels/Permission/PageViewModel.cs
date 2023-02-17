using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class PageViewModel
    {
        public System.Guid PageId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PageName")]
        public string PageName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PageUrl")]
        public string PageUrl { get; set; }
        public string Icon { get; set; }
        public string Color { get; set; }

        public string Parameter { get; set; }
        public Nullable<System.Guid> ModuleId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Permission_MenuModel")]
        public System.Guid MenuId { get; set; }

        public int? OrderIndex { get; set; }

        public List<FunctionViewModel> FunctionViewModels { get; set; }

        //Choose all functions in page
        public bool? isChooseAll { get; set; }
    }
}
