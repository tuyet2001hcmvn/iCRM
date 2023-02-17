
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class MenuViewModel
    {
        public System.Guid MenuId { get; set; }
        public string MenuName { get; set; }
        public Nullable<System.Guid> ModuleId { get; set; }
        
        public string Icon { get; set; }
        public int? OrderIndex { get; set; }

        public List<PageViewModel> PageViewModels { get; set; }
        public List<MobileScreenViewModel> MobileScreenViewModels { get; set; }

        //Excel
        public int RowIndex { get; set; }
        public string Error { get; set; }
        public bool isNullValueId { get; set; }

        //Choose all functions in pages in menu
        public bool? isChooseAll { get; set; }
    }
}
