using ISD.EntityModels;
using ISD.ViewModels.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.Sale
{
    /// <summary>
    /// Phiếu thu tiền
    /// </summary>
    public class ReceiptVoucherViewModel
    {
        public Guid? ReceiptVoucherId { get; set; }

        //SO_WEB_GUID
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder")]
        public Guid? SaleOrderId { get; set; }

        //SO_WEB
        //[Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder")]
        [Display(Name = "Mã đơn hàng")]
        public string SaleOrderCode { get; set; }

        //NGAYTHUTIEN
        //[Display(ResourceType = typeof(Resources.LanguageResource), Name = "ReceiptVoucherDate")]
        [Display(Name = "Ngày thu tiền")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime? CreatedDate { get; set; }

        //COMPANYCODE
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Company")]
        public string CompanyCode { get; set; }

        //SOHDTRAGOP
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ContractNumber")]
        public string ContractNumber { get; set; }

        //TAIKHOAN
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Permission_AccountModel")]
        public string Account { get; set; }

        //BUSSINESS_AREA
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Store_SaleOrgCode_KetToan")]
        public string StoreCode { get; set; }

        //DIENGIAI
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ReceiptVoucherDescription")]
        public string Description { get; set; }

        //AMOUNT
        //[Display(ResourceType = typeof(Resources.LanguageResource), Name = "ReceiptVoucherAmount")]
        [Display(Name = "Thực thu/chi")]
        [DisplayFormat(DataFormatString = "{0:n0}")]
        public decimal? Amount { get; set; }

        [Display(Name = "Họ và tên KH")]
        public string FullName { get; set; }

        [Display(Name = "Loại đơn hàng")]
        public string OrderType { get; set; }

        //[Display(Name = "Thời gian")]
        //public string ThoiGian { get; set; }

        [Display(Name = "Nhân viên BH/KTT")]
        public Guid? Step3AccountId { get; set; }
        [Display(Name = "Nhân viên BH/KTT")]
        public string Step3AccountIdName { get; set; }


        [Display(Name = "Thu ngân")]
        public Guid? AccountIdThuNgan { get; set; }
        [Display(Name = "Thu ngân")]
        public string AccountIdThuNganName { get; set; }

        [Display(Name = "Tổng tiền")]
        [DisplayFormat(DataFormatString = "{0:n0}")]
        public decimal? Total { get; set; }
        public int? PaymentType { get; set; }
        public Guid? DescriptionId { get; set; }


        //public decimal ThucThu { get; set; }
        public int PaymentMethod { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PaymentMethod")]
        public string PaymentMethodName { get; set; }

        public bool? IsCanceled { get; set; }

        public int? Type { get; set; }
    }

    public class ReceiptVoucherReportViewModel
    {
        public Guid? ReceiptVoucherId { get; set; }

        //SO_WEB_GUID
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder")]
        public Guid? SaleOrderId { get; set; }

        //SO_WEB
        //[Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder")]
        [Display(Name = "Mã đơn hàng")]
        public string SaleOrderCode { get; set; }

        [Display(Name = "Loại đơn hàng")]
        public string OrderType
        {
            get
            {
                if (SaleOrderCode.IndexOf("XE") != -1)
                {
                    return "Bán xe mới";
                }

                if (SaleOrderCode.IndexOf("PT") != -1)
                {
                    return "Bán phụ tùng";
                }

                if (SaleOrderCode.IndexOf("SC") != -1)
                {
                    return "Dịch vụ";
                }

                return "";
            }
        }

        public int? PaymentType { get; set; }
        [Display(Name = "Loại thu chi")]
        public string PaymentTypeName
        {
            get
            {
                if (PaymentType == 0 || PaymentType == null)
                {
                    return "THU";
                }
                return "CHI";
            }
        }

        [Display(Name = "Thu ngân")]
        public Guid? AccountIdThuNgan { get; set; }
        [Display(Name = "Thu ngân")]
        public string AccountIdThuNganName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PaymentMethod")]
        public string PaymentMethodName { get; set; }

        //NGAYTHUTIEN
        //[Display(ResourceType = typeof(Resources.LanguageResource), Name = "ReceiptVoucherDate")]
        [Display(Name = "Ngày thu tiền")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime? CreatedDate { get; set; }

        [Display(Name = "Nhân viên BH/KTT")]
        public Guid? Step3AccountId { get; set; }
        [Display(Name = "Nhân viên BH/KTT")]
        public string Step3AccountIdName { get; set; }

        //DIENGIAI
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ReceiptVoucherDescription")]
        public string Description { get; set; }

        //COMPANYCODE
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Company")]
        public string CompanyCode { get; set; }

        //SOHDTRAGOP
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ContractNumber")]
        public string ContractNumber { get; set; }

        //TAIKHOAN
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Permission_AccountModel")]
        public string Account { get; set; }

        //BUSSINESS_AREA
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Store_SaleOrgCode_KetToan")]
        public string StoreCode { get; set; }

        [Display(Name = "Họ và tên KH")]
        public string FullName { get; set; }

        [Display(Name = "Tổng tiền")]
        [DisplayFormat(DataFormatString = "{0:n0}")]
        public decimal? Total { get; set; }


        public Guid? DescriptionId { get; set; }

        //public decimal ThucThu { get; set; }
        public int PaymentMethod { get; set; }

        public bool? IsCanceled { get; set; }

        public int? Type { get; set; }

        //AMOUNT
        //[Display(ResourceType = typeof(Resources.LanguageResource), Name = "ReceiptVoucherAmount")]
        [Display(Name = "Thực thu/chi")]
        [DisplayFormat(DataFormatString = "{0:n0}")]
        public decimal? Amount { get; set; }

    }

    #region Bán hàng + Dịch vụ
    public class ReceiptVoucherResultViewModel
    {

        [Display(Name = "Lịch sử thu tiền")]
        public List<ReceiptVoucherViewModel> ListLichSuThuTien { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder")]
        public Guid SaleOrderId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder")]
        public string SaleOrderCode { get; set; }

        public int Type { get; set; }


        //Trừ lệ phí biển số
        //true: có trừ
        //false: không trừ
        public bool? IsHasLicensePrice { get; set; }

        #region Bộ phận bán hàng
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_SaleOrderType")]
        public Nullable<int> SaleOrderType { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_SaleOrderType")]
        public string SaleOrderTypeName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Store_StoreCode")]
        public string SaleOrg { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrgReceive")]
        public string SaleOrgReceive { get; set; }
        public string SaleOrgReceiveName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Store")]
        public string SaleOrgName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_CustomerCode")]
        public string CustomerCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_CustomerName")]
        public string CustomerName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_CustomerCode2")]
        public string CustomerCode2 { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_CustomerName2")]
        public string CustomerName2 { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProvinceId")]
        public Nullable<System.Guid> ProvinceId { get; set; }
        public string ProvinceName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DistrictId")]
        public Nullable<System.Guid> DistrictId { get; set; }
        public string DistrictName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "WardId")]
        public Nullable<System.Guid> WardId { get; set; }
        public string WardName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_CustomerAddress")]
        public string CustomerAddress { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_IdentityNumber")]
        public string IdentityNumber { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TaxCode")]
        public string TaxCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_ProductCode")]
        public string MaterialCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Unit")]
        public string Unit { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_ProfitCenter")]
        public string ProfitCenter { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_Category")]
        public string ProductHierarchy { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_MaterialGroup")]
        public string MaterialGroup { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_Labor")]
        public string Labor { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_MaterialFreightGroup")]
        public string MaterialFreightGroup { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_ExternalMaterialGroup")]
        public string ExternalMaterialGroup { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_TemperatureCondition")]
        public string TemperatureCondition { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_SerialNumber")]
        public string SerialNumber { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_EngineNumber")]
        public string EngineNumber { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "GiaBanLe")]
        [DisplayFormat(DataFormatString = "{0:n0}")]
        public Nullable<decimal> SalePrice { get; set; }
        public string SalePriceText
        {
            get
            {
                if (SalePrice != null)
                {
                    return string.Format("{0:n0}", SalePrice);
                }
                return string.Empty;
            }
        }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "VATPrice")]
        [DisplayFormat(DataFormatString = "{0:n0}")]
        public Nullable<decimal> VATPrice { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "RegisterFee")]
        public Nullable<decimal> RegisterFee { get; set; }
        public string RegisterFeeText
        {
            get
            {
                if (RegisterFee != null)
                {
                    return string.Format("{0:n0}%", RegisterFee * 100);
                }
                return string.Empty;
            }
        }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "RegisterFeeTotal")]
        [DisplayFormat(DataFormatString = "{0:n0}")]
        public Nullable<decimal> RegisterFeeTotal { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "LicensePrice")]
        [DisplayFormat(DataFormatString = "{0:n0}")]
        public Nullable<decimal> LicensePrice { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "BHTNDS")]
        [DisplayFormat(DataFormatString = "{0:n0}")]
        public Nullable<decimal> BHTNDS { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ServiceFee")]
        [DisplayFormat(DataFormatString = "{0:n0}")]
        public Nullable<decimal> ServiceFee { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FeeTotal")]
        [DisplayFormat(DataFormatString = "{0:n0}")]
        public Nullable<decimal> FeeTotal { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "BHCHXM")]
        [DisplayFormat(DataFormatString = "{0:n0}")]
        public Nullable<decimal> BHCHXM { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TotalPrice")]
        public Nullable<decimal> TotalPrice { get; set; }
        public string TotalPriceText
        {
            get
            {
                if (TotalPrice != null)
                {
                    return string.Format("{0:n0}", TotalPrice);
                }
                return string.Empty;
            }
        }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "AccountCode")]
        public string AccountCode { get; set; }
        public string AccountName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Organization")]
        public string Organization { get; set; }
        public string OrganizationName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ContractNumber")]
        public string ContractNumber { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Cash")]
        public decimal? DownPaymentCash { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Transfer")]
        public decimal? DownPaymentTransfer { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DownPayment")]
        public Nullable<decimal> DownPayment { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "BalanceDue")]
        public Nullable<decimal> BalanceDue { get; set; }

        [DisplayFormat(DataFormatString = "{0:n0}")]
        public Nullable<decimal> DownPaymentThucTe { get; set; }
        [DisplayFormat(DataFormatString = "{0:n0}")]
        public Nullable<decimal> BalanceDueThucTe { get; set; }

        [Display(Name = "Số tiền phải trả")]
        [DisplayFormat(DataFormatString = "{0:n0}")]
        public Nullable<decimal> DownPaymentThucTeBanDau { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "EmployeeCode")]
        public string EmployeeCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Note")]
        public string Note { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "IsGiuXe")]
        public Nullable<bool> IsGiuXe { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "IsChayHoSo")]
        public Nullable<bool> IsChayHoSo { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public Nullable<int> Status { get; set; }

        public Nullable<bool> IsPaid { get; set; }
        public Nullable<bool> IsChecked { get; set; }
        public Nullable<bool> IsXacNhan { get; set; }
        public Nullable<bool> IsCanceled { get; set; }

        //Tổng tiền phụ kiện
        //[Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_Accessory")]
        [Display(Name = "Phụ kiện")]
        public decimal? AccessoryTotalPrice { get; set; }

        //Tổng tiền công việc
        //[Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_Accessory")]
        public decimal? ServiceTotalPrice { get; set; }

        //Số tiền
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DownPaymentAmount")]
        public decimal? DownPaymentAmount { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Installment")]
        public Nullable<decimal> Installment { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "GiaBanLe")]
        public decimal? SalePriceEstimated { get; set; }

        public Guid? ProvinceIdValue { get; set; }
        public Guid? DistrictIdValue { get; set; }

        public int? CustomerType { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "RegisterFeeTotalText")]
        public string RegisterFeeTotalText { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Ngày tạo")]
        public DateTime? CreatedDate { get; set; }
        public string CreatedDateText
        {
            get
            {
                return string.Format("{0:dd/MM/yyyy}", CreatedDate);
            }
        }

        public string FeeTotalText
        {
            get
            {
                if (AccessoryTotalPrice == null)
                {
                    AccessoryTotalPrice = 0;
                }
                return string.Format("{0:n0}", FeeTotal + BHCHXM + AccessoryTotalPrice);
            }
        }

        public string CustomerFullAddress
        {
            get
            {
                if (!string.IsNullOrEmpty(CustomerAddress))
                {
                    if (WardId != null)
                    {
                        return string.Format("{0}, {1}, {2}, {3}", CustomerAddress, WardName, DistrictName, ProvinceName);
                    }
                    else
                    {
                        return string.Format("{0}, {1}, {2}", CustomerAddress, DistrictName, ProvinceName);
                    }
                }
                else
                {
                    if (WardId != null)
                    {
                        return string.Format("{0}, {1}, {2}", WardName, DistrictName, ProvinceName);
                    }
                    else
                    {
                        return string.Format("{0}, {1}", DistrictName, ProvinceName);
                    }
                }
            }
        }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Configuration_Phone")]
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ReceiptVoucherDescription")]
        public string Description { get; set; }

        //Số tiền thu
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ReceiptVoucherAmount")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public decimal? Amount { get; set; }

        public int PaymentMethod { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "PaymentMethod")]
        public string PaymentMethodName { get; set; }

        [Display(Name = "Nợ tối đa")]
        public decimal? BalanceDueMax { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_Gender")]
        public bool? Gender { get; set; }
        public string GenderText
        {
            get
            {
                string gender = string.Empty;
                if (Gender.HasValue)
                {
                    if (Gender == Constant.ConstGender.Male)
                    {
                        gender = "Nam";
                    }
                    else
                    {
                        gender = "Nữ";
                    }
                }
                return gender;
            }
        }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_DateOfBirth")]
        public DateTime? DateOfBirth { get; set; }
        public string DateOfBirthText
        {
            get
            {
                string dateOfBirth = string.Empty;
                if (DateOfBirth.HasValue)
                {
                    dateOfBirth = string.Format("{0:dd/MM/yyyy}", DateOfBirth);
                }
                return dateOfBirth;
            }
        }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_Career")]
        public string CareerName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SoBinhDien")]
        public string SoBinhDien { get; set; }

        [Display(Name = "Chiết khấu")]
        public decimal? Discount { get; set; }

        [Display(Name = "Ghi chú chiết khấu")]
        public string DiscountNote { get; set; }

        [Display(Name = "Loại chiết khấu")]
        public int DiscountType { get; set; }

        //Tổng tiền trước chiết khấu
        public decimal? Total { get; set; }

        #endregion Bộ phận bán hàng

        public List<SaleOrderAccessoryDetailViewModel> accessoryDetail { get; set; }
        public List<SaleOrderAccessoryDetailViewModel> promotionDetail { get; set; }
        public List<ServiceOrderDetailServiceViewModel> serviceDetail { get; set; }
        public List<SaleOrderAccessoryDetailViewModel> insuranceDetail { get; set; }

        [Display(Name = "Tiền đặt cọc")]
        public decimal? DepositPrice { get; set; }
    }
    #endregion Bán hàng + Dịch vụ

    #region Filter
    public class ReceiptVoucherSearchViewModel
    {
        [Display(Name = "Công ty")]
        public Guid? CompanyId { get; set; }
        [Display(Name = "Chi nhánh")]
        public string SaleOrgCode { get; set; }
        [Display(Name = "Loại đơn hàng")]
        public string Type { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_FromDate")]
        public DateTime? FromDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_ToDate")]
        public DateTime? ToDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_SaleOrderCode")]
        public string SaleOrderCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_CustomerCode")]
        public string CustomerCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_CustomerName")]
        public string CustomerName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_Phone")]
        public string Phone { get; set; }

        public int? PaymentStatus { get; set; }

        [Display(Name = "Thu ngân")]
        public string Cashier { get; set; }

        [Display(Name = "Loại thu")]
        public string PaymentMethodType { get; set; }

        [Display(Name = "Loại thanh toán")]
        public string PaymentType { get; set; }
    }
    #endregion Filter

    #region Thu tiền
    public class ReceiptVoucherSaveViewModel
    {
        public Guid SaleOrderId { get; set; }
        public string SaleOrderCode { get; set; }
        public string SaleOrg { get; set; }
        public string ContractNumber { get; set; }
        public string Organization { get; set; }
        public string AccountCode { get; set; }
        public string Description { get; set; }
        public decimal? Amount { get; set; }
        public int PaymentMethod { get; set; }
        public int PaymentType { get; set; }


        public string EmployeeCode { get; set; } // NV Bán hàng g
        //public string AccountId { get; set; } 
        public Guid? AccountId1TiepNhan { get; set; } // Kỹ thuật trưởng
        public Guid? AccountIdKeToan { get; set; } // Thu ngân
        public string FullName { get; set; } // Họ tên KH
        public string OrderType { get; set; } // Loại đơn hàng
        public Guid? DescriptionId { get; set; } //Diễn giải thu/chi

        public Guid? ReceiptVoucherId { get; set; }
        public bool? isCoc { get; set; }
    }
    #endregion Thu tiền 
}
