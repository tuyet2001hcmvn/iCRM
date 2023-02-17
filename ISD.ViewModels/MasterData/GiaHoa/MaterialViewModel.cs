using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.Sale
{
    public class MaterialViewModel
    {
        //Mã sản phẩm
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_ProductCode")]
        public string MaterialCode { get; set; }
        //Tên sản phẩm (tiếng Việt)
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_ProductName")]
        public string MaterialName { get; set; }
        //Đơn vị tính
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Unit")]
        public string MaterialUnit { get; set; }

        public string MaterialUnitName { get; set; }
        //Phân loại
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaterialType")]
        public string MaterialType { get; set; }

        //Dòng xe
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_MaterialGroup")]
        public string MaterialGroupCode { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_MaterialGroup")]
        public string MaterialGroupName { get; set; }

        //Nhãn hiệu
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_ProfitCenter")]
        public string ProfitCenterCode { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_ProfitCenter")]
        public string ProfitCenterName { get; set; }

        //Loại xe
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_ProductHierarchy")]
        public string ProductHierarchyCode { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_ProductHierarchy")]
        public string ProductHierarchyName { get; set; }

        //Phiên bản
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_Labor")]
        public string LaborCode { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_Labor")]
        public string LaborName { get; set; }

        //Màu sắc
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_MaterialFreightGroup")]
        public string MaterialFreightGroupCode { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_MaterialFreightGroup")]
        public string MaterialFreightGroupName { get; set; }
        public string MaterialFreightGroup { get {
                string color = string.Empty;
                if (!string.IsNullOrEmpty(MaterialFreightGroupName))
                {
                    color = MaterialFreightGroupName.Split(':').Last();
                }
                return color;
            } }

        //Đời xe
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_ExternalMaterialGroup")]
        public string ExternalMaterialGroupCode { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_ExternalMaterialGroup")]
        public string ExternalMaterialGroupName { get; set; }

        //Kiểu xe
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_TemperatureCondition")]
        public string TemperatureConditionCode { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_TemperatureCondition")]
        public string TemperatureConditionName { get; set; }

        //Option
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_ContainerRequirement")]
        public string ContainerRequirementCode { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_ContainerRequirement")]
        public string ContainerRequirementName { get; set; }

        //Năm sản xuất
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "InternalComment")]
        public string InternalComment { get; set; }
        //Tên hàng (in trên hóa đơn bán xe)
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SalesText")]
        public string SalesText { get; set; }
        //Tên sản phẩm bằng tiếng Anh
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_ProductNameEnglish")]
        public string BasicDataText { get; set; }
        //Mã cũ
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "OldMaterial")]
        public string OldMaterial { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ImageUrl")]
        public string ImageUrl { get; set; }

        [Display(Name ="Dung tích xi lanh")]
        public decimal? Capacity { get; set; }

        [Display(Name = "Đơn vị dung tích")]
        public string CapacityUnit { get; set; }

        [Display(Name = "Cho phép nợ tối đa")]
        public decimal? BalanceDueMax { get; set; }

        public bool? Actived { get; set; }

        //Modal Popup
        //Mã Plant
        public string Plant { get; set; }

        //Mã kho
        //LGORT
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_Warehouse")]
        public string WarehouseCode { get; set; }

        //Batch
        //CHARG
        public string Batch { get; set; }

        //Số lượng
        //MENGE
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_Quantity")]
        public decimal? Quantity { get; set; }

        //Đơn vị tính
        //MEINS
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Unit")]
        public string Unit { get; set; }

        //Số khung
        //SOKHUNG
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_SerialNumber")]
        public string ChassisNumber { get; set; }

        //Số máy
        //SOMAY
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_EngineNumber")]
        public string EngineNumber { get; set; }

        //Đăng kiểm
        //DANGKIEM
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Stock_IsGroup")]
        public string IsGroup { get; set; }

        //Giá xuất hóa đơn
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "InvoicePrice")]
        public decimal? Modal_InvoicePrice { get; set; }

        //Giá tính lệ phí trước bạ
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "RegistrationFeePrice")]
        public decimal? Modal_RegistrationFeePrice { get; set; }

        //Giá min
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MinPrice")]
        public decimal? MinPrice { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Dimension")]
        public string Dimension { get; set; }

        public string SaleOrg { get; set; }
    }
}
