using ISD.EntityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ISD.ViewModels
{
    public class ModuleViewModel
    {
        public System.Guid ModuleId { get; set; }
        public string ModuleName { get; set; }
        public string Icon { get; set; }
        public bool? isSystemModule { get; set; }
        public int? OrderIndex { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public string Details { get; set; }
        public string Url { get; set; }

        //Excel
        public int RowIndex { get; set; }
        public string Error { get; set; }
        public bool isNullValueId { get; set; }

        //Permission Menu
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Permission_MenuModel")]
        public Guid MenuId { get; set; }

        public string MenuName { get; set; }

        public List<PageModel> ActivedMenuList { get; set; }

        //Permission Page
        public List<PageModel> PageList { get; set; }

        public List<PageModel> ActivedPageList { get; set; }

        public Guid PageId { get; set; }

        public string PageName { get; set; }
    }
}
