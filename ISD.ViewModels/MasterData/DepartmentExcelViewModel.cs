using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.MasterData
{
    public class DepartmentExcelViewModel
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Company")]
        public string CompanyCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Store_StoreCode")]
        public string StoreCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Department_DepartmentCode")]     
        public string DepartmentCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Department_DepartmentName")]
        [Required]
        public string DepartmentName { get; set; }
        public Nullable<Guid> CompanyId { get; set; }
        public Nullable<Guid> StoreId { get; set; }



        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public bool? Actived { get; set; }

        //Import Excel
        public int RowIndex { get; set; }
        public string Error { get; set; }
        public bool isNullValueId { get; set; }
    }
}
