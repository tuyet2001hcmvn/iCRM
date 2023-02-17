using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ISD.ViewModels
{
    public class WarehouseViewModel
    {
        public System.Guid WarehouseId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Company")]
        public System.Guid CompanyId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Company")]
        public string CompanyName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Store")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public System.Guid StoreId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Store")]
        public string StoreName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Warehouse_WarehouseCode")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        [Remote("CheckExistingWarehouseCode", "Warehouse", AdditionalFields = "WarehouseCodeValid", HttpMethod = "POST", ErrorMessageResourceName = "Validation_Already_Exists", ErrorMessageResourceType = typeof(Resources.LanguageResource))]
        public string WarehouseCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Warehouse_WarehouseName")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string WarehouseName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Warehouse_WarehouseShortName")]
        public string WarehouseShortName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "OrderIndex")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Validation_OrderIndex")]
        public Nullable<int> OrderIndex { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public Nullable<bool> Actived { get; set; }

        //Warehouse
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_Product")]
        public string ProductName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_Color")]
        public string MainColorProductCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_Color")]
        public string MainColorProductName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_Style")]
        public string StyleWarehouseName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_Quantity")]
        public decimal? Quantity { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_ProductPostDate")]
        public DateTime? ProductWarehousePostDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_ProductUserPost")]
        public string ProductWarehouseUserPost { get; set; }
    }
}
