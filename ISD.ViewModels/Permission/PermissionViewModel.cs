using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class PermissionViewModel
    {
        public List<PageViewModel> PageModel { get; set; }

        public List<MenuViewModel> MenuModel { get; set; }

        public List<PagePermissionViewModel> PagePermissionModel { get; set; }
        public List<ModuleViewModel> ModuleModel { get; set; }

        
    }
}
