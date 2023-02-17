using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.Sale
{
    public class MaterialGroupsViewModel
    {
        //Mã dòng xe
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaterialGroup_MaterialGroupCode")]
        public string MaterialGroupsCode { get; set; }
        //Tên dòng xe
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaterialGroup_MaterialGroupName")]
        public string MaterialGroupsName { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaterialGroup_IconUrl")]
        public string IconUrl { get; set; }

        //Phí dịch vụ
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ServiceFee")]
        public decimal? ServiceFee { get; set; }
    }
}
