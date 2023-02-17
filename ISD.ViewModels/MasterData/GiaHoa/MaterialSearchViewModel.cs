using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.Sale
{
    public class MaterialSearchViewModel
    {
        //Mã sản phẩm
        //[Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_ProductCode", Description = "MinCode_Hint")]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_ProductCode")]
        public string SearchMaterialCode { get; set; }

        //Tên sản phẩm (tiếng Việt)
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_ProductName")]
        public string SearchMaterialName { get; set; }

        //Dòng xe
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_MaterialGroup")]
        public string SearchMaterialGroupCode { get; set; }

        //Nhãn hiệu
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_ProfitCenter")]
        public string SearchProfitCenterCode { get; set; }

        //Loại xe
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_ProductHierarchy")]
        public string SearchProductHierarchyCode { get; set; }

        //Phiên bản
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_Labor")]
        public string SearchLaborCode { get; set; }

        //Màu sắc
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_MaterialFreightGroup")]
        public string SearchMaterialFreightGroupCode { get; set; }

        //Đời xe
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_ExternalMaterialGroup")]
        public string SearchExternalMaterialGroupCode { get; set; }

        //Kiểu xe
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_TemperatureCondition")]
        public string SearchTemperatureConditionCode { get; set; }

        //Option
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_ContainerRequirement")]
        public string SearchContainerRequirementCode { get; set; }

        public string SaleOrg { get; set; }
    }
}
