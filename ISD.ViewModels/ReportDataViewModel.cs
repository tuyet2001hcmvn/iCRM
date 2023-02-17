using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class ReportDataViewModel
    {
        public string FullName { get; set; }
        public string FullNameLabel { get; set; }

        public string Phone { get; set; }
        public string PhoneLabel { get; set; }

        public string Gender { get; set; }
        public string GenderLabel { get; set; }
    }

    #region Báo cáo danh sách khách hàng tiềm năng
    public class ProspectReportDataViewModel
    {
        public Guid ProspectId { get; set; }
        public string ProspectIdLabel { get; set; }

        //Họ và tên
        public string FullName { get; set; }
        public string FullNameLabel { get; set; }

        //Số điện thoại
        public string Phone { get; set; }
        public string PhoneLabel { get; set; }

        //Giới tính
        public string Gender { get; set; }
        public string GenderLabel { get; set; }

        //Ngày sinh
        public string DateOfBirth { get; set; }
        public string DateOfBirthLabel { get; set; }

        //Xe dự định mua
        public string Subject { get; set; }
        public string SubjectLabel { get; set; }

        //Thời gian dự mua
        public string PurchasedTime { get; set; }
        public string PurchasedTimeLabel { get; set; }

        //Khả năng mua
        public string SuccessPercent { get; set; }
        public string SuccessPercentLabel { get; set; }

        //Chi nhánh
        public string StoreName { get; set; }
        public string StoreNameLabel { get; set; }

        //Nhân viên tạo
        public string CreateUser { get; set; }
        public string CreateUserLabel { get; set; }

        //Ngày tạo
        public string CreatedDate { get; set; }
        public string CreatedDateLabel { get; set; }
    }
    #endregion Báo cáo danh sách khách hàng tiềm năng

    #region Báo cáo xe bán trong ngày
    public class XeBanTrongNgayReportDataViewModel
    {
        //STT
        public string NumberLabel { get; set; }
        public string Number { get; set; }

        //Chi nhánh
        public string StoreName { get; set; }
        public string StoreNameLabel { get; set; }

        //Nhân viên bán hàng
        public string SaleEmployeeName { get; set; }
        public string SaleEmployeeNameLabel { get; set; }

        //Loại xe
        public string MaterialDescription { get; set; }
        public string MaterialDescriptionLabel { get; set; }

        //Giá xuất giao
        public string SalePrice { get; set; }
        public string SalePriceLabel { get; set; }

        //Phụ kiện
        public string AccessoryTotalPrice { get; set; }
        public string AccessoryTotalPriceLabel { get; set; }

        //Tổng lệ phí
        public string FeeTotal { get; set; }
        public string FeeTotalLabel { get; set; }

        //Tổng cộng
        public string TotalPrice { get; set; }
        public string TotalPriceLabel { get; set; }

        //Khuyến mãi
        public string Promotion { get; set; }
        public string PromotionLabel { get; set; }

        //Thời gian
        public string CreatedDate { get; set; }
        public string CreatedDateLabel { get; set; }
    }
    #endregion Báo cáo xe bán trong ngày

    #region Báo cáo doanh thu sửa chữa phụ tùng
    public class DoanhThuSuaChuaPhuTungReportDataViewModel
    {
        //STT
        //public string NumberLabel { get; set; }
        //public string Number { get; set; }

        //Chi nhánh
        public string StoreName { get; set; }
        public string StoreNameLabel { get; set; }

        //Nhân viên sửa chữa
        public string ServiceEmployeeName { get; set; }
        public string ServiceEmployeeNameLabel { get; set; }

        //Số lượng phiếu sửa chữa
        public string ServiceQuantity { get; set; }
        public string ServiceQuantityLabel { get; set; }

        //Tiền công
        public string ServiceTotalPrice { get; set; }
        public string ServiceTotalPriceLabel { get; set; }

        //Tiền phụ tùng
        public string AccessoryTotalPrice { get; set; }
        public string AccessoryTotalPriceLabel { get; set; }

        //VAT
        //public string VATPrice { get; set; }
        //public string VATPriceLabel { get; set; }

        //Tổng cộng
        public string TotalPrice { get; set; }
        public string TotalPriceLabel { get; set; }
    }
    #endregion Báo cáo doanh thu sửa chữa phụ tùng

    #region Báo cáo thời gian sửa chữa ráp xe
    public class ThoiGianSuaChuaRapXeReportDataViewModel
    {
        //Thông tin khách hàng
        public string CustomerName { get; set; }
        public string CustomerNameLabel { get; set; }

        public string CustomerAddress { get; set; }
        public string CustomerAddressLabel { get; set; }

        public string CustomerPhone { get; set; }
        public string CustomerPhoneLabel { get; set; }

        public string CustomerEmail { get; set; }
        public string CustomerEmailLabel { get; set; }

        //Thông tin KTV 
        public string ServiceEmployeeName { get; set; }
        public string ServiceEmployeeNameLabel { get; set; }

        public string ServiceDate { get; set; }
        public string ServiceDateLabel { get; set; }

        public string ServiceTime { get; set; }
        public string ServiceTimeLabel { get; set; }

        //Thông tin xe
        public string ProfitCenter { get; set; }
        public string ProfitCenterLabel { get; set; }

        public string ProductHierarchy { get; set; }
        public string ProductHierarchyLabel { get; set; }
    }
    #endregion Báo cáo thời gian sửa chữa ráp xe
}
