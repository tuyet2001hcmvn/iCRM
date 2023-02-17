using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class CreditLimitSearchViewModel
    {
        public string CompanyCode { get; set; }
        public string NamTruoc { get; set; }
        public string NamHienTai { get; set; }
        public string CapDuyet { get; set; }
        public string Duyet { get; set; }
        public string PhongBan { get; set; }
        public string MaSAPKH { get; set; }
        public string NVSale { get; set; }
        public string NVCongNo { get; set; }
        public string UserNo { get; set; }
        public int GetRow { get; set; }
    }

    public class CreditLimitSearchResultViewModel
    {
        /*
        public string MaCongTy { get; set; }
        public string MaKhachHang { get; set; }
        public string TenKhachHang { get; set; }
        public string PhongBan { get; set; }
        //------------------------------------------
        public string DuDauKy1 { get; set; }
        public string InKyNo1 { get; set; }
        public string InKyCo1 { get; set; }
        public string DuCuoiKy1 { get; set; }
        //------------------------------------------
        public string DuDauKy2 { get; set; }
        public string InKyNo2 { get; set; }
        public string InKyCo2 { get; set; }
        public string DuCuoiKy2 { get; set; }
        //------------------------------------------
        public string DoanhSoBan1 { get; set; }
        public string DoanhSoBan2 { get; set; }
        //------------------------------------------
        public string MaDKTT1 { get; set; }
        public string TenDKTT1 { get; set; }
        public string HanMucNam1 { get; set; }
        public string HanMucBoSungNam1 { get; set; }
        public string TongHanMucNam1 { get; set; }
        //------------------------------------------
        public string MaDKTT2 { get; set; }
        public string TenDKTT2 { get; set; }
        public string HanMucNam2 { get; set; }
        public string HanMucBoSungNam2 { get; set; }
        public string TongHanMucNam2 { get; set; }
        //------------------------------------------
        public string MaCDT { get; set; }
        public string TenCDT { get; set; }
        //------------------------------------------
        public string SaleDuyet { get; set; }
        public string SaleDate { get; set; }
        public string TenSaleDuyet { get; set; }
        public string SaleNote { get; set; }
        //------------------------------------------
        public string CongNoDuyet { get; set; }
        public string CongNoDate { get; set; }
        public string TenCongNoDuyet { get; set; }
        public string CongNoNote { get; set; }
        //------------------------------------------
        public string QuanLyDuyet { get; set; }
        public string QuanLyDate { get; set; }
        public string TenQuanLyDuyet { get; set; }
        public string QuanLyNote { get; set; }
        //------------------------------------------
        public string HeadDuyet { get; set; }
        public string HeadDate { get; set; }
        public string TenHeadDuyet { get; set; }
        public string HeadNote { get; set; }
        //------------------------------------------
        public string EmailList { get; set; }
        public string EmailNext { get; set; }
        public int? NaturalNumber { get; set; }
        public string NamHanMuc { get; set; }
        public string KyHanMuc { get; set; }
        public string SaleName { get; set; }
        //------------------------------------------
        public string EmailPrior { get; set; }
        */
        public string Nam0 { get; set; }
        public string Nam1 { get; set; }
        public string Nam2 { get; set; }
        public int? NaturalNumber { get; set; }
        //------------------------------------------
        public string EmailList { get; set; }
        public string EmailNext { get; set; }
        public string NamHanMuc { get; set; }
        public string KyHanMuc { get; set; }
        public string SaleName { get; set; }
        //------------------------------------------
        public string EmailPrior { get; set; }
        #region Thông tin khách hàng
        public string MaKhachHang { get; set; }
        public string TenKhachHang { get; set; }
        public string MaCDT { get; set; }
        public string TenCDT { get; set; }
        public string NhomKhachHang { get; set; }
        public string CNOXepLoai { get; set; }
        public string SALEXepLoai { get; set; }
        public string DanhGiaKH { get; set; }
        #endregion
        //------------------------------------------
        #region Thông tin công ty
        public string MaCongTy { get; set; }
        public string CongTyBanHang { get; set; }
        public string PhongBan { get; set; }
        public string SALEPhuTrach { get; set; }
        public string CongNo { get; set; }
        public string CapQuanLy { get; set; }
        public string NgayBaoCao { get; set; }
        //------------------------------------------
        public string NoDauKyNam1 { get; set; }
        public string PhatSinhBanNam1 { get; set; }
        public string PhatSinhThuNam1 { get; set; }
        public string DuNoCuoiKyNam1 { get; set; }
        //------------------------------------------
        public string NoDauKyNam2 { get; set; }
        public string PhatSinhBanNam2 { get; set; }
        public string PhatSinhThuNam2 { get; set; }
        public string DuNoCuoiKyNam2 { get; set; }
        //------------------------------------------
        public string DoanhSoBanNam0 { get; set; }
        public string DoanhSoBanNam1 { get; set; }
        public string DoanhSoBanNam2 { get; set; }
        //------------------------------------------
        public string ThoiHanNoNam1 { get; set; }
        public string HanMucNoNam1 { get; set; }
        public string HanMucThemNam1 { get; set; }
        public string TongHanMucNam1 { get; set; }
        public string GhiChuThoiHan1 { get; set; }
        #endregion
        //------------------------------------------
        #region Chính sách công nợ đang áp dụng năm (YHT)
        public string ThoiHanNoDangAp { get; set; }
        public string HanMucNoDangAp { get; set; }
        public string HanMucThemDangAp { get; set; }
        public string TongHanMucDangAp { get; set; }
        #endregion
        //------------------------------------------
        #region Đề xuất chính sách công nợ mới
        public string NgayDeXuat { get; set; }
        public string ThoiHanNoNam2 { get; set; }
        public string HanMucNoNam2 { get; set; }
        public string HanMucThemNam2 { get; set; }
        public string TongHanMucNam2 { get; set; }
        public string GhiChuThoiHan2 { get; set; }
        #endregion
        //------------------------------------------
        #region Thông tin SALE đề nghị
        public string SALETrangThaiDeNghi { get; set; }
        public string SALENgayDeNghi { get; set; }
        public string SALENhanVienDeNghi { get; set; }
        public string SALENote { get; set; }
        public string SALEEmail { get; set; }
        #endregion
        //------------------------------------------
        #region Thông tin phòng CÔNG NỢ
        public string CNOKyNhay { get; set; }
        public string CNONgayKyNhay { get; set; }
        public string CNONVKyNhay { get; set; }
        public string CNOTrangThaiDuyet { get; set; }
        public string CNONgayDuyet { get; set; }
        public string CNOCanBoDuyet { get; set; }
        public string CNONote { get; set; }
        #endregion
        //------------------------------------------
        #region Thông tin QUẢN LÝ kiểm tra và duyệt
        public string MANKyNhay { get; set; }
        public string MANNgayKyNhay { get; set; }
        public string MANCanBoKyNhay { get; set; }
        public string MANTrangThaiDuyet { get; set; }
        public string MANNgayDuyet { get; set; }
        public string MANDuyet { get; set; }
        public string MANNote { get; set; }
        #endregion
        //------------------------------------------
        #region Thông tin TGĐ/PTGĐ kiểm tra và duyệt
        public string HEADTrangThaiDuyet { get; set; }
        public string HEADNgayDuyet { get; set; }
        public string HEADDuyet { get; set; }
        public string HEADNote { get; set; }
        #endregion
    }

    public class CreditLimitFormViewModel
    {
        //Gửi thông tin lên SAP
        public string CapDuyet { get; set; }
        public string MaCongTy { get; set; }
        public string MaKhachHang { get; set; }
        public string NamHanMuc { get; set; }
        public string KyHanMuc { get; set; }
        public int? NaturalNumber { get; set; }
        public string FormType { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
        public string XepLoai { get; set; }
        public string UserNo { get; set; }
        public string UserName { get; set; }
        //Gửi email
        public string FromEmail { get; set; }
        public string ToEmail { get; set; }
        public string EmailList { get; set; }
        public string EmailNext { get; set; }
        public string TenKhachHang { get; set; }
        public string SaleName { get; set; }
        public string EmailPrior { get; set; }
        //Các fields bổ sung
        public string DanhGiaKH { get; set; }
        public string GhiChuThoiHan2 { get; set; }
        public string MaCDT { get; set; }
        public string TenCDT { get; set; }
        public string NhomKhachHang { get; set; }
        public string PhongBan { get; set; }
        public string SALEEmail { get; set; }
        public string CNOName { get; set; }
        public string MANName { get; set; }
        public string NgayDeXuat { get; set; }
        public string ThoiHanNoNam2 { get; set; }
        public string HanMucNoNam2 { get; set; }
        public string HanMucThemNam2 { get; set; }
        public string TongHanMucNam2 { get; set; }
        public string SALENote { get; set; }
        public string CNONote { get; set; }
        public string MANNote { get; set; }
        public string HEADNote { get; set; }
        public string CNOXepLoai { get; set; }
        public string SALEXepLoai { get; set; }
        public string ZTERMDESC { get; set; }
        public string CREDITLIMIT { get; set; }
        public string CREDITHIDE { get; set; }
        public string CREDITTOL { get; set; }
    }
}
