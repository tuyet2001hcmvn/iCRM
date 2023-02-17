using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ISD.ViewModels
{
    public class ServiceOrderView_ViewModel
    {
        public Guid ServiceOrderId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_SaleOrderCode")]
        public string ServiceOrderCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_CustomerCode")]
        public string CustomerCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_CustomerName")]
        public string CustomerName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Store")]
        public string StoreName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreatedDate")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? CreatedDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_FromDate")]
        public DateTime? FromDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_ToDate")]
        public DateTime? ToDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_Category")]
        public Guid? CategoryId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_Category")]
        public string Category { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ServiceOrder_ServicePool")]
        public string ServicePool { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_SerialNumber")]
        public string SerialNumber { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_EngineNumber")]
        public string EngineNumber { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ServiceOrder_LicensePlate")]
        public string LicensePlate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_CurrentKm")]
        public decimal? CurrentKilometers { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_PersonnalNumberId")]
        public string PersonnalNumberId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ServiceOrder_Quantity")]
        public int Quantity { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_Total")]
        public decimal? Total { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_Note")]
        public string Note { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_SystemNote")]
        public string SystemNote { get; set; }

        [Display(Name = "Công ty")]
        public Guid? CompanyId { get; set; }
        [Display(Name = "Chi nhánh")]
        public string SaleOrgCode { get; set; }
        [Display(Name = "Số thứ tự trong ngày")]
        public int? Number { get; set; }
        [Display(Name = "Trạng thái")]
        public int? Step { get; set; }

        public bool? IsDeleted { get; set; }

        public bool? isShowCancelButton { get; set; }

        public bool? isShowCancelButton_AfterComplete { get; set; }

        [Display(Name = "Giới tính")]
        public bool? Gender { get; set; }

        [Display(Name = "Địa chỉ")]
        public string CustomerAddress { get; set; }

        [Display(Name = "Số điện thoại")]
        public string CustomerPhone { get; set; }

        [Display(Name = "Loại xe")]
        public string ProductHierarchy { get; set; }

        [Display(Name = "Yêu cầu KH")]
        public string CustomerRequest { get; set; }

        [Display(Name = "Nội dung sửa chữa")]
        public string ServiceOrderType { get; set; }

        [Display(Name = "Tiền công")]
        public decimal? ServicePrice { get; set; }

        [Display(Name = "Tiền phụ tùng")]
        public decimal? AccessoryPrice { get; set; }

        [Display(Name = "Tiếp nhận")]
        public string TiepNhan { get; set; }

        [Display(Name = "Kỹ thuật")]
        public string KyThuat { get; set; }

        public List<ServiceOrderDetailViewModel> detailList { get; set; }
        [Display(Name = "USER hủy")]
        public string CanceledUser { get; set; }

        [Display(Name = "Lý do hủy")]
        public string DeletedNote { get; set; }

        [Display(Name = "Tiền cọc đã thu")]
        public decimal? TienCocDaThu { get; set; }

        [Display(Name = "Ngày xác nhận")]
        public DateTime? NgayXacNhan { get; set; }

        //Detail
        public class ServiceOrderDetailViewModel
        {
            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_Description")]
            public string Description { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_Price")]
            public Nullable<decimal> Price { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_Quantity")]
            public Nullable<decimal> Quantity { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_DiscountType")]
            public Nullable<bool> DiscountType { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_Discount")]
            public Nullable<decimal> Discount { get; set; }

            [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_UnitPrice")]
            public Nullable<decimal> UnitPrice { get; set; }
        }
    }

    public class ExportDonHangKhan
    {
        public Guid ServiceOrderId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_SaleOrderCode")]
        public string ServiceOrderCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_CustomerCode")]
        public string CustomerCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_CustomerName")]
        public string CustomerName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Store")]
        public string StoreName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreatedDate")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? CreatedDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_FromDate")]
        public DateTime? FromDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_ToDate")]
        public DateTime? ToDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_Category")]
        public Guid? CategoryId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_Category")]
        public string Category { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ServiceOrder_ServicePool")]
        public string ServicePool { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_SerialNumber")]
        public string SerialNumber { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_EngineNumber")]
        public string EngineNumber { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ServiceOrder_LicensePlate")]
        public string LicensePlate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_CurrentKm")]
        public decimal? CurrentKilometers { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_PersonnalNumberId")]
        public string PersonnalNumberId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_Note")]
        public string Note { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_SystemNote")]
        public string SystemNote { get; set; }

        [Display(Name = "Công ty")]
        public Guid? CompanyId { get; set; }
        [Display(Name = "Chi nhánh")]
        public string SaleOrgCode { get; set; }
        [Display(Name = "Số thứ tự trong ngày")]
        public int? Number { get; set; }
        [Display(Name = "Trạng thái")]
        public int? Step { get; set; }

        public bool? IsDeleted { get; set; }

        public bool? isShowCancelButton { get; set; }

        public bool? isShowCancelButton_AfterComplete { get; set; }

        [Display(Name = "Giới tính")]
        public bool? Gender { get; set; }

        [Display(Name = "Địa chỉ")]
        public string CustomerAddress { get; set; }

        [Display(Name = "Số điện thoại")]
        public string CustomerPhone { get; set; }

        [Display(Name = "Loại xe")]
        public string ProductHierarchy { get; set; }

        [Display(Name = "Yêu cầu KH")]
        public string CustomerRequest { get; set; }

        [Display(Name = "Nội dung sửa chữa")]
        public string ServiceOrderType { get; set; }

        [Display(Name = "Tiền công")]
        public decimal? ServicePrice { get; set; }

        //[Display(Name = "Tiền phụ tùng")]
        //public decimal? AccessoryPrice { get; set; }

        [Display(Name = "Tiếp nhận")]
        public string TiepNhan { get; set; }

        [Display(Name = "Kỹ thuật")]
        public string KyThuat { get; set; }

        [Display(Name = "STT detail")]
        public Int64 RowNum { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Accessory_Code")]
        public string AccessoryCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Accessory_Name")]
        public string AccessoryName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Unit")]
        public string Unit { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_Quantity")]
        public int? Quantity { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_DetailUnitPrice")]
        [DisplayFormat(DataFormatString = "{0:n0}")]
        public decimal? AccessoryPrice { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_Total")]
        [DisplayFormat(DataFormatString = "{0:n0}")]
        public decimal? Total { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "StockPosition")]
        public string Location { get; set; }

        public string Plant { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Warehouse_WarehouseCode")]
        public string WarehouseCode { get; set; }

        [Display(Name = "Tên kho")]
        public string WarehouseName { get; set; }

        public int? SumOfQuantity { get; set; }

    }
}
