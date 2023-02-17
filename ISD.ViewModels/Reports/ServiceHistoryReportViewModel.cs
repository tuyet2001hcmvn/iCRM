using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.Reports
{
    public class ServiceHistoryReportViewModel
    {
        //Biển số xe
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ServiceOrder_LicensePlate")]
        //[Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string PlateNumber { get; set; }
    }

    public class ServiceHistoryReportResultViewModel
    {
        public int STT { get; set; }
        public string BienSo { get; set; }
        public string TenXe { get; set; }
        public DateTime NgayMuaXe { get; set; }
        public int SoLanSuaChua { get; set; }
        public string TenKH { get; set; }
        public DateTime NgaySuaChua { get; set; }
        public string MaDonHang { get; set; }
        public string LoaiDonHang { get; set; }
        public string TenSanPham { get; set; }
        public decimal SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal GiamGia { get; set; }
        public decimal ThanhTien { get; set; }
        public string CuaHang { get; set; }
    }
}
