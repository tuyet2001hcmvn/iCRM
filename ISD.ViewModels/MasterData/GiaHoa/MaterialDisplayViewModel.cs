using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.Sale
{
    public class MaterialDisplayViewModel
    {
        //Mã sản phẩm
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_ProductCode")]
        public string MaterialCode { get; set; }

        //Tên sản phẩm (tiếng Việt)
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_ProductName")]
        public string DisplayMaterialName { get; set; }

        //Đơn vị tính
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Unit")]
        public string DisplayMaterialUnit { get; set; }

        //Dòng xe
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_MaterialGroup")]
        public string DisplayMaterialGroupName { get; set; }

        //Nhãn hiệu
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_ProfitCenter")]
        public string DisplayProfitCenterName { get; set; }

        //Loại xe
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_ProductHierarchy")]
        public string DisplayProductHierarchyName { get; set; }

        //Phiên bản
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_Labor")]
        public string DisplayLaborName { get; set; }

        //Màu sắc
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_MaterialFreightGroup")]
        public string DisplayMaterialFreightGroupName { get; set; }

        //Đời xe
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_ExternalMaterialGroup")]
        public string DisplayExternalMaterialGroupName { get; set; }

        //Kiểu xe
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_TemperatureCondition")]
        public string DisplayTemperatureConditionName { get; set; }

        //Option
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_ContainerRequirement")]
        public string DisplayContainerRequirementName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ImageUrl")]
        public string DisplayImageUrl { get; set; }
    }
}
