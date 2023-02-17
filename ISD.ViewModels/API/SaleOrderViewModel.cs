using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ISD.ViewModels
{
    public class SaleOrderViewModel
    {
        /// <summary>
        /// SO_WEB_GUID Mã đơn hàng dạng Guid trên web
        /// </summary>
        public System.Guid SaleOrderMasterId { get; set; }

        public string SaleOrderMasterId_Text
        {
            get
            {
                return SaleOrderMasterId.ToString().ToUpper();
            }
        }

        /// <summary>
        /// SO_WEB Mã đơn hàng (dạng string)
        /// </summary>
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_SaleOrderCode")]
        public string SaleOrderMasterCode { get; set; }

        /// <summary>
        /// NGAYTAO Ngày tại đơn hàng
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        //Mã plant
        public string Plant { get; set; }

        /// <summary>
        /// SALEORG Mã chi nhánh
        /// </summary>
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Store_StoreCode")]
        public string SaleOrg { get; set; }

        /// <summary>
        /// ORDERTYPE Loại đơn hàng (ConstSaleOrder: BanXe|BanDichVu|BanPT)
        /// </summary>
        public string OrderType { get; set; }

        /// <summary>
        /// FULLNAME_CUS Tên đầy đủ khách hàng
        /// </summary>
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_CustomerName")]
        public string CustomerName { get; set; }

        /// <summary>
        /// SEX Giới tính
        /// </summary>
        public bool? Gender { get; set; }

        /// <summary>
        /// PROVINCE Tên tỉnh thành
        /// </summary>
        public Guid? ProvinceId { get; set; }
        //Mã code
        /// <summary>
        /// SALEOFFICE  Mã tỉnh thành (Mã code)
        /// </summary>
        public string SaleOffice { get; set; }
        //Tên
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProvinceId")]
        public string ProvinceName { get; set; }

        /// <summary>
        /// DISTRICT
        /// </summary>
        public Guid? DistrictId { get; set; }
        //Mã code
        /// <summary>
        /// SALEGROUP
        /// </summary>
        public string SaleGroup { get; set; }
        //Tên
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DistrictId")]
        public string DistrictName { get; set; }

        /// <summary>
        /// WARD
        /// </summary>
        public Guid? WardId { get; set; }
        //Mã code
        /// <summary>
        /// SALEDISTRICT
        /// </summary>
        public string SaleDistrict { get; set; }
        //Tên
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "WardId")]
        public string WardName { get; set; }

        /// <summary>
        /// ADDRESS
        /// </summary>
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Customer_CustomerAddress")]
        public string CustomerAddress { get; set; }

        /// <summary>
        /// SALEEMPLOYEE Mã nhân viên
        /// </summary>
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "EmployeeCode")]
        public string EmployeeCode { get; set; }

        public string EmployeeCode2 { get; set; }

        /// <summary>
        /// RECONC_ACC Tài khoản thu tiền trả góp
        /// </summary>
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "AccountCode")]
        public string AccountCode { get; set; }

        public string MaterialName { get; set; }

        //Giảm giá
        public decimal? DiscountAmount { get; set; }
        public decimal? DiscountPercent { get; set; }

        //Địa chỉ tạm trú
        public Guid? ProvinceId2 { get; set; }
        public Guid? DistrictId2 { get; set; }
        public Guid? WardId2 { get; set; }
        public string CustomerAddress2 { get; set; }
    }

    public class SaleOrderDetailViewModel
    {
        public Guid? SaleOrderDetailId { get; set; }

        public string SaleOrderDetailId_Text
        {
            get
            {
                return SaleOrderDetailId.ToString().ToUpper();
            }
        }

        //MATERIAL Mã xe/Mã PTPK
        public string MaterialCode { get; set; }

        //BATCH Batch
        public string Batch { get; set; }

        //LGORT Mã kho
        public string WarehouseCode { get; set; }

        //SOLUONG Số lượng
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_Quantity")]
        public int? Quantity { get; set; }

        //UNIT Đơn vị tính (3 ký tự)
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Unit")]
        public string Unit { get; set; }

        //GIA Đơn giá
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleOrder_DetailUnitPrice")]
        public decimal? UnitPrice { get; set; }

        //FLAG Loại material (Xe/Biển số/Dịch vụ) (ConstSaleOrder: MAXE|BIENSO|PHIDICHVU|NULL)
        public string Flag { get; set; }

        //STATUS kiểu cập nhật (Create/Update/Delete) (ConstantUpdateType: C|U|D)
        public string Status { get; set; }

        //Loại phụ tùng/phụ kiện
        public string AccessoryTypeCode { get; set; }

        //Chiết khấu theo line: chỉ đối với đơn hàng bán xe
        public decimal? DiscountAmount { get; set; }
        public decimal? DiscountPercent { get; set; }

        public decimal? MinPrice { get; set; }
        public string SoBinhDien { get; set; }
    }
}
