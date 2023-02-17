using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.Sale
{
    public class ProductExcelViewModel
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProductId")]
        public Guid ProductId { get; set; }

        //Mã Công ty
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Company_CompanyCode")]
        [Required]
        public string CompanyCode { get; set; }

        //Mã ERP
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_ERPProductCode")]
        [Required]
        public string ERPProductCode { get; set; }

        //Mã SP
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_ProductCode")]
        [Required]
        public string ProductCode { get; set; }

        //Tên nhóm ( Dùng gom ver trong báo cáo )
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_ProductGroups")]
        [Required]
        public string ProductGroups { get; set; }

        //Tên SP
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_ProductName")]
        [Required]
        public string ProductName { get; set; }

        //Hãng xe
        //public Guid? BrandId { get; set; }
        public Guid? ParentCategoryId { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_Brand")]
        [Required]
        //public string BrandName { get; set; }
        public string ParentCategoryName { get; set; }


        //Loại xe
        public Guid? CategoryId { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_Category")]
        public string CategoryName { get; set; }

        //Dung tích xi lanh
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CylinderCapacity")]
        public decimal? CylinderCapacity { get; set; }

        //Đời xe
        public Guid? ConfigurationId { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_Configuration")]
        [Required]
        public string ConfigurationName { get; set; }

        //Mã màu
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Color_ColorName")]
        public string Color { get; set; }

        //Thời hạn bảo hành
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_GuaranteePeriod")]
        public string GuaranteePeriod { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_Price")]
        public decimal? Price { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_ProcessingValue")]
        public decimal? ProcessingValue { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_SampleValue")]
        public decimal? SampleValue { get; set; }

        //Thứ tự hiển thị
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "OrderIndex")]
        public int? OrderIndex { get; set; }

        //Là sản phẩm nổi bật
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_isHot")]
        public bool? isHot { get; set; }

        //Trạng thái
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public bool Actived { get; set; }

        //Import Excel
        public int RowIndex { get; set; }
        public string Error { get; set; }
        public bool isNullValueId { get; set; }
    }

    public class ColorStyleProductExcelViewModel
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ColorProductId")]
        public Guid ColorProductId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_ProductName")]
        [Required]
        public string ProductName { get; set; }
        public Guid ProductId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Color_ColorName")]
        [Required]
        public string ColorName { get; set; }
        public Guid? ColorId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Style_StyleName")]
        public string StyleName { get; set; }
        public Guid? StyleId { get; set; }

        //Import Excel
        public int RowIndex { get; set; }
        public string Error { get; set; }
        public bool isNullValueId { get; set; }
    }

    public class SpecificationProductExcelViewModel
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SpecificationsProductId")]
        public System.Guid SpecificationsProductId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_ProductName")]
        [Required]
        public string ProductName { get; set; }
        public Guid ProductId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Specifications_SpecificationsName")]
        [Required]
        public string SpecificationsName { get; set; }
        public System.Guid SpecificationsId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Description")]
        [Required]
        public string Description { get; set; }

        //Import Excel
        public int RowIndex { get; set; }
        public string Error { get; set; }
        public bool isNullValueId { get; set; }
    }

    public class AccessoryProductExcelViewModel
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "AccessoryProductId")]
        public System.Guid AccessoryProductId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_ProductName")]
        [Required]
        public string ProductName { get; set; }
        public Guid ProductId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Accessory_AccessoryName")]
        [Required]
        public string AccessoryName { get; set; }
        public System.Guid AccessoryId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Price")]
        [Required]
        public decimal? Price { get; set; }

        //[Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_ProcessingValue")]
        //public decimal? ProcessingValue { get; set; }

        //[Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_SampleValue")]
        //public decimal? SampleValue { get; set; }

        //Import Excel
        public int RowIndex { get; set; }
        public string Error { get; set; }
        public bool isNullValueId { get; set; }
    }
}
