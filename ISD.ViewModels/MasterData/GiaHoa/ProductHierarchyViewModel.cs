using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.Sale
{
    public class ProductHierarchyViewModel
    {
        //Mã loại xe
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProductHierarchy_ProductHierarchyCode")]
        public string ProductHierarchyCode { get; set; }
        //Tên loại xe
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProductHierarchy_ProductHierarchyName")]
        public string ProductHierarchyName { get; set; }
        //Level 
        public int LevelNo { get; set; }
        //Mã nhãn hiệu
        public string ProfitCenterCode { get; set; }
        //Hình ảnh
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ImageUrl")]
        public string ImageUrl { get; set; }

        public string MaterialGroupCode { get; set; }
        public string MaterialGroupName { get; set; }

        [Display(Name = "Số tiền nợ tối đa")]
        [DisplayFormat(DataFormatString = "{0:n0}")]
        public decimal BalanceDueMax { get; set; }
    }
}
