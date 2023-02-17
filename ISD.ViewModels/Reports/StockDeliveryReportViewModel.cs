using System;
using System.ComponentModel.DataAnnotations;

namespace ISD.ViewModels
{
    public class StockDeliveryReportViewModel
    {
        [Display(Name = "Mã catalogue")]
        public string ERPProductCode { get; set; }

        [Display(Name = "Nhóm Catalogue")]
        public string ProductGroups { get; set; }

        [Display(Name = "Tên catalogue")]
        public string ProductName { get; set; }

        [Display(Name = "Nhóm vật tư")]
        public string CategoryName { get; set; }

        [Display(Name = "Số lượng xuất")]
        public decimal? Quantity { get; set; }

        [Display(Name = "Mã khách hàng")]
        public string ProfileCode { get; set; }

        [Display(Name = "Tên khách hàng")]
        public string ProfileName { get; set; }

        [Display(Name = "Nhóm khách hàng")]
        public string ProfileGroup { get; set; }

        [Display(Name = "NV kinh doanh")]
        public string PersonInCharge { get; set; }

        [Display(Name = "Mã NV xuất")]
        public string SalesEmployeeCode { get; set; }

        [Display(Name = "NV xuất CTL")]
        public string SalesEmployeeName { get; set; }

        [Display(Name = "Phòng ban")]
        public string RolesName { get; set; }
        [Display(Name = "Phòng ban")]
        public string DepartmentName { get; set; }

        [Display(Name = "Người nhận")]
        public string RecipientName { get; set; }

        [Display(Name = "Địa chỉ người nhận")]
        public string RecipientAddress { get; set; }

        [Display(Name = "SĐT người nhận")]
        public string RecipientPhone { get; set; }

        [Display(Name = "Khu vực")]
        public string SaleOfficeName { get; set; }

        [Display(Name = "Tỉnh/Thành phố")]
        public string ProvinceName { get; set; }

        [Display(Name = "Quận/Huyện")]
        public string DistrictName { get; set; }

        [Display(Name = "Mã kho xuất")]
        public string StockCode { get; set; }

        [Display(Name = "Tên kho xuất")]
        public string StockName { get; set; }

        [Display(Name = "Ngày chứng từ")]
        public DateTime? DocumentDate { get; set; }

        [Display(Name = "Mã phiếu xuất")]
        public string DeliveryCode { get; set; }

        [Display(Name = "Ghi chú")]
        public string Note { get; set; }

        [Display(Name = "Phương thức gửi")]
        public string ShippingTypeName { get; set; }

        [Display(Name = "Đơn giá")]
        public decimal? Price { get; set; }

        [Display(Name = "Tổng giá trị")]
        public decimal? TotalPrice { get; set; }



        [Display(Name = "Người tạo")]
        public string CreateByName { get; set; }

        [Display(Name = "Thời gian tạo")]
        public DateTime? CreateTime { get; set; }
    }
}
