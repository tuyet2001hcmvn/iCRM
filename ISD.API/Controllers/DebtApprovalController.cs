using ISD.Constant;
using ISD.EntityModels;
using ISD.Extensions;
using ISD.Repositories;
using ISD.Resources;
using ISD.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using ISD.Core;
using System.Data;
using System.Globalization;

namespace ISD.API.Controllers
{
    public class DebtApprovalController : BaseController
    {
        // GET: DebtApproval
        #region Helper
        //Các danh mục cơ bản để duyệt hạn mức
        public ActionResult GetCreditList(string CompanyCode, string Type, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                if (string.IsNullOrEmpty(CompanyCode))
                {
                    CompanyCode = "1000";
                }
                if (string.IsNullOrEmpty(Type))
                {
                    Type = "%";
                }
                //Trả về các danh mục cơ bản để duyệt hạn mức
                //Cấp duyệt
                /*
                    SALE
                    CONGNO
                    MANAGER
                    HEAD
                 */

                var result = new List<ISDSelectStringItem>();
                var dt = _unitOfWork.TaskRepository.GetCreditList(CompanyCode, Type);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        result.Add(new ISDSelectStringItem()
                        {
                            id = item["CODENO"].ToString(),
                            name = item["DATANM"].ToString(),
                        });
                    }
                }
                return _APISuccess(result);
            });
        }

        //Danh sách user login
        public ActionResult GetUserList(string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                //Trả về danh sách user login
                var result = new List<ISDSelectStringItem>();
                var dt = _unitOfWork.TaskRepository.GetUserList();
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        result.Add(new ISDSelectStringItem()
                        {
                            id = item["USERNO"].ToString(),
                            name = item["USERNO"].ToString() + " | " + item["USERNAME"].ToString(),
                        });
                    }
                }
                return _APISuccess(result);
            });
        }

        //Danh sách trạng thái duyệt
        public ActionResult GetApprovalList(string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                //Trả về danh sách trạng thái duyệt
                var result = new List<ISDSelectStringItem>();

                result.Add(new ISDSelectStringItem()
                {
                    id = string.Empty,
                    name = "Chưa duyệt",
                });

                result.Add(new ISDSelectStringItem()
                {
                    id = "X",
                    name = "Đã duyệt",
                });
                return _APISuccess(result);
            });
        }
        #endregion

        #region Check login (authenticate)
        public ActionResult CheckLogin(CreditAuthenticateViewModel auth, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                //Trả về thông tin user login
                var result = new CreditAuthenticateInfoViewModel();

                var dt = _unitOfWork.TaskRepository.CheckLogin(auth);
                if (dt != null && dt.Rows.Count > 0)
                {
                    result.UserNo = dt.Rows[0]["USERNO"].ToString();
                    result.UserName = dt.Rows[0]["USERNAME"].ToString();
                    result.FromEmail = dt.Rows[0]["FR_EMAIL"].ToString();
                    result.ToEmail = dt.Rows[0]["TO_EMAIL"].ToString();
                    result.MessageNo = Convert.ToInt32(dt.Rows[0]["MSGNO"].ToString());
                    result.MessageName = dt.Rows[0]["MSGNAME"].ToString();
                }
                return _APISuccess(result);
            });
        }
        #endregion

        #region Search
        public ActionResult GetCreditLimit(CreditLimitSearchViewModel searchViewModel, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                //searchViewModel.CompanyCode = "1000";
                //searchViewModel.NamTruoc = "2020";
                //searchViewModel.NamHienTai = "2021";
                //searchViewModel.CapDuyet = "SALE";
                //searchViewModel.Duyet = "";
                //searchViewModel.PhongBan = "";
                //searchViewModel.MaSAPKH = "";
                //searchViewModel.NVSale = "";
                //searchViewModel.NVCongNo = "";
                searchViewModel.GetRow = 5;

                //Trả về thông tin hạn mức công nợ khách hàng
                var result = new List<CreditLimitSearchResultViewModel>();
                var dt = _unitOfWork.TaskRepository.GetCreditLimit(searchViewModel);
                if (dt != null && dt.Rows.Count > 0)
                {
                    //Năm hiện tại
                    int currentYear = Convert.ToInt32(searchViewModel.NamHienTai);
                    //Một năm trước
                    var lastYear = currentYear - 1;
                    //Hai năm trước
                    var twoYearsAgo = currentYear - 2;
                    foreach (DataRow item in dt.Rows)
                    {
                        result.Add(new CreditLimitSearchResultViewModel()
                        {
                            /*
                            MaCongTy = item["WERKS"].ToString(),
                            MaKhachHang = item["KUNNR"].ToString(),
                            TenKhachHang = item["CUSTNM"].ToString(),
                            PhongBan = item["DEPTNM"].ToString(),
                            //------------------------------------------
                            DuDauKy1 = Convert.ToDecimal(item["DUDAUKY1"]).ToString("n0"),
                            InKyNo1 = Convert.ToDecimal(item["INKYNO1"]).ToString("n0"),
                            InKyCo1 = Convert.ToDecimal(item["INKYCO1"]).ToString("n0"),
                            DuCuoiKy1 = Convert.ToDecimal(item["DUCUOIKY1"]).ToString("n0"),
                            //------------------------------------------
                            DuDauKy2 = Convert.ToDecimal(item["DUDAUKY2"]).ToString("n0"),
                            InKyNo2 = Convert.ToDecimal(item["INKYNO2"]).ToString("n0"),
                            InKyCo2 = Convert.ToDecimal(item["INKYCO2"]).ToString("n0"),
                            DuCuoiKy2 = Convert.ToDecimal(item["DUCUOIKY2"]).ToString("n0"),
                            //------------------------------------------
                            DoanhSoBan1 = Convert.ToDecimal(item["DOANHSOSAP1"]).ToString("n0"),
                            DoanhSoBan2 = Convert.ToDecimal(item["DOANHSOSAP2"]).ToString("n0"),
                            //------------------------------------------
                            MaDKTT1 = item["ZTERM1"].ToString(),
                            TenDKTT1 = item["ZTERMDESC1"].ToString(),
                            HanMucNam1 = Convert.ToDecimal(item["CREDITLIMIT1"]).ToString("n0"),
                            HanMucBoSungNam1 = Convert.ToDecimal(item["CREDITHIDE1"]).ToString("n0"),
                            TongHanMucNam1 = Convert.ToDecimal(item["CREDITTOL1"]).ToString("n0"),
                            //------------------------------------------
                            MaDKTT2 = item["ZTERM_DX"].ToString(),
                            TenDKTT2 = item["ZTERMDESC_DX"].ToString(),
                            HanMucNam2 = Convert.ToDecimal(item["CREDITLIMIT_DX"]).ToString("n0"),
                            HanMucBoSungNam2 = Convert.ToDecimal(item["CREDITHIDE_DX"]).ToString("n0"),
                            TongHanMucNam2 = Convert.ToDecimal(item["CREDITTOL_DX"]).ToString("n0"),
                            //------------------------------------------
                            MaCDT = item["MACDT"].ToString(),
                            TenCDT = item["TENCDT"].ToString(),
                            //------------------------------------------
                            SaleDuyet = item["SALEDUYET"].ToString() == "X" ? "Duyệt" : string.Empty,
                            SaleDate = !string.IsNullOrEmpty(item["SALEDATE"].ToString()) && !item["SALEDATE"].ToString().Contains("00000000") ?
                                                            DateTime.ParseExact(item["SALEDATE"].ToString(),
                                                              "yyyyMMdd",
                                                               CultureInfo.InvariantCulture).ToString("dd/MM/yyyy") : string.Empty,
                            TenSaleDuyet = item["TENSALEDUYET"].ToString(),
                            SaleNote = item["SALENOTE"].ToString(),
                            //------------------------------------------
                            CongNoDuyet = item["CNODUYET"].ToString() == "X" ? "Duyệt" : string.Empty,
                            CongNoDate = !string.IsNullOrEmpty(item["CNODATE"].ToString()) && !item["CNODATE"].ToString().Contains("00000000") ?
                                                            DateTime.ParseExact(item["CNODATE"].ToString(),
                                                              "yyyyMMdd",
                                                               CultureInfo.InvariantCulture).ToString("dd/MM/yyyy") : string.Empty,
                            TenCongNoDuyet = item["TENCNODUYET"].ToString(),
                            CongNoNote = item["CNONOTE"].ToString(),
                            //------------------------------------------
                            QuanLyDuyet = item["MANDUYET"].ToString() == "X" ? "Duyệt" : string.Empty,
                            QuanLyDate = !string.IsNullOrEmpty(item["MANDATE"].ToString()) && !item["MANDATE"].ToString().Contains("00000000") ?
                                                            DateTime.ParseExact(item["MANDATE"].ToString(),
                                                              "yyyyMMdd",
                                                               CultureInfo.InvariantCulture).ToString("dd/MM/yyyy") : string.Empty,
                            TenQuanLyDuyet = item["TENMANDUYET"].ToString(),
                            QuanLyNote = item["NOTEDUYET"].ToString(),
                            //------------------------------------------
                            HeadDuyet = item["HEADDUYET"].ToString() == "X" ? "Duyệt" : string.Empty,
                            HeadDate = !string.IsNullOrEmpty(item["HEADDATE"].ToString()) && !item["HEADDATE"].ToString().Contains("00000000") ?
                                                            DateTime.ParseExact(item["HEADDATE"].ToString(),
                                                              "yyyyMMdd",
                                                               CultureInfo.InvariantCulture).ToString("dd/MM/yyyy") : string.Empty,
                            TenHeadDuyet = item["TENHEADDUYET"].ToString(),
                            HeadNote = item["HEADNOTE"].ToString(),
                            //------------------------------------------
                            EmailList = item["EMAIL_LIST"].ToString(),
                            EmailNext = item["EMAIL_NEXT"].ToString(),
                            NaturalNumber = Convert.ToInt32(item["IDNO"].ToString()),
                            NamHanMuc = item["NAM"].ToString(),
                            KyHanMuc = item["PERIOD"].ToString(),
                            SaleName = item["SALENAME"].ToString(),
                            //------------------------------------------
                            EmailPrior = item["EMAIL_PRIOR"].ToString(),
                            */
                            Nam0 = twoYearsAgo.ToString(),
                            Nam1 = lastYear.ToString(),
                            Nam2 = currentYear.ToString(),
                            NaturalNumber = Convert.ToInt32(item["IDNO"].ToString()),
                            EmailList = item["EMAIL_LIST"].ToString(),
                            EmailNext = item["EMAIL_NEXT"].ToString(),
                            NamHanMuc = item["NAM"].ToString(),
                            KyHanMuc = item["PERIOD"].ToString(),
                            SaleName = item["SALENAME"].ToString(),
                            EmailPrior = item["EMAIL_PRIOR"].ToString(),
                            //Thông tin khách hàng
                            MaKhachHang = item["KUNNR"].ToString(),
                            TenKhachHang = item["CUSTNM"].ToString(),
                            MaCDT = item["MACDT"].ToString(),
                            TenCDT = item["TENCDT"].ToString(),
                            NhomKhachHang = item["KTEXT"].ToString(),
                            CNOXepLoai = item["CNOXEPLOAI"].ToString(),
                            SALEXepLoai = item["XEPLOAI"].ToString(),
                            DanhGiaKH = item["DANHGIAKH"].ToString(),
                            //------------------------------------------
                            //Thông tin công ty
                            MaCongTy = item["WERKS"].ToString(),
                            CongTyBanHang = item["WERKS"].ToString(),
                            PhongBan = item["DEPTNM"].ToString(),
                            SALEPhuTrach = item["SALENAME"].ToString(),
                            CongNo = item["CNONAME"].ToString(),
                            CapQuanLy = item["QLYNAME"].ToString(),
                            NgayBaoCao = DateTime.Now.ToString("dd.MM.yyyy"), //lấy ngày hiện tại
                            //------------------------------------------
                            NoDauKyNam1 = !string.IsNullOrEmpty(item["DUDAUKY1"].ToString()) ? Convert.ToDecimal(item["DUDAUKY1"]).ToString("n0") : string.Empty,
                            PhatSinhBanNam1 = !string.IsNullOrEmpty(item["INKYNO1"].ToString()) ? Convert.ToDecimal(item["INKYNO1"]).ToString("n0") : string.Empty,
                            PhatSinhThuNam1 = !string.IsNullOrEmpty(item["INKYCO1"].ToString()) ? Convert.ToDecimal(item["INKYCO1"]).ToString("n0") : string.Empty,
                            DuNoCuoiKyNam1 = !string.IsNullOrEmpty(item["DUCUOIKY1"].ToString()) ? Convert.ToDecimal(item["DUCUOIKY1"]).ToString("n0") : string.Empty,
                            //------------------------------------------
                            NoDauKyNam2 = !string.IsNullOrEmpty(item["DUDAUKY2"].ToString()) ? Convert.ToDecimal(item["DUDAUKY2"]).ToString("n0") : string.Empty,
                            PhatSinhBanNam2 = !string.IsNullOrEmpty(item["INKYNO2"].ToString()) ? Convert.ToDecimal(item["INKYNO2"]).ToString("n0") : string.Empty,
                            PhatSinhThuNam2 = !string.IsNullOrEmpty(item["INKYCO2"].ToString()) ? Convert.ToDecimal(item["INKYCO2"]).ToString("n0") : string.Empty,
                            DuNoCuoiKyNam2 = !string.IsNullOrEmpty(item["DUCUOIKY2"].ToString()) ? Convert.ToDecimal(item["DUCUOIKY2"]).ToString("n0") : string.Empty,
                            //------------------------------------------
                            DoanhSoBanNam0 = !string.IsNullOrEmpty(item["DOANHSOSAP0"].ToString()) ? Convert.ToDecimal(item["DOANHSOSAP0"]).ToString("n0") : string.Empty,
                            DoanhSoBanNam1 = !string.IsNullOrEmpty(item["DOANHSOSAP1"].ToString()) ? Convert.ToDecimal(item["DOANHSOSAP1"]).ToString("n0") : string.Empty,
                            DoanhSoBanNam2 = !string.IsNullOrEmpty(item["DOANHSOSAP2"].ToString()) ? Convert.ToDecimal(item["DOANHSOSAP2"]).ToString("n0") : string.Empty,
                            //------------------------------------------
                            ThoiHanNoNam1 = item["ZTERMDESC1"].ToString(),
                            HanMucNoNam1 = !string.IsNullOrEmpty(item["CREDITLIMIT1"].ToString()) ? Convert.ToDecimal(item["CREDITLIMIT1"]).ToString("n0") : string.Empty,
                            HanMucThemNam1 = !string.IsNullOrEmpty(item["CREDITHIDE1"].ToString()) ? Convert.ToDecimal(item["CREDITHIDE1"]).ToString("n0") : string.Empty,
                            TongHanMucNam1 = !string.IsNullOrEmpty(item["CREDITTOL1"].ToString()) ? Convert.ToDecimal(item["CREDITTOL1"]).ToString("n0") : string.Empty,
                            GhiChuThoiHan1 = item["NOTETHOIHAN1"].ToString(),
                            //Chính sách công nợ đang áp dụng năm (YHT)
                            ThoiHanNoDangAp = item["ZTERMDESC"].ToString(),
                            HanMucNoDangAp = !string.IsNullOrEmpty(item["CREDITLIMIT"].ToString()) ? Convert.ToDecimal(item["CREDITLIMIT"]).ToString("n0") : string.Empty,
                            HanMucThemDangAp = !string.IsNullOrEmpty(item["CREDITHIDE"].ToString()) ? Convert.ToDecimal(item["CREDITHIDE"]).ToString("n0") : string.Empty,
                            TongHanMucDangAp = !string.IsNullOrEmpty(item["CREDITTOL"].ToString()) ? Convert.ToDecimal(item["CREDITTOL"]).ToString("n0") : string.Empty,
                            //Đề xuất chính sách công nợ mới
                            NgayDeXuat = !string.IsNullOrEmpty(item["NGAYDXHM"].ToString()) && !item["NGAYDXHM"].ToString().Contains("00000000") ?
                                                            DateTime.ParseExact(item["NGAYDXHM"].ToString(),
                                                              "yyyyMMdd",
                                                               CultureInfo.InvariantCulture).ToString("dd.MM.yyyy") : string.Empty,
                            ThoiHanNoNam2 = item["ZTERMDESC_DX"].ToString(),
                            HanMucNoNam2 = !string.IsNullOrEmpty(item["CREDITLIMIT_DX"].ToString()) ? Convert.ToDecimal(item["CREDITLIMIT_DX"]).ToString("n0") : string.Empty,
                            HanMucThemNam2 = !string.IsNullOrEmpty(item["CREDITHIDE_DX"].ToString()) ? Convert.ToDecimal(item["CREDITHIDE_DX"]).ToString("n0") : string.Empty,
                            TongHanMucNam2 = !string.IsNullOrEmpty(item["CREDITTOL_DX"].ToString()) ? Convert.ToDecimal(item["CREDITTOL_DX"]).ToString("n0") : string.Empty,
                            GhiChuThoiHan2 = item["NOTETHOIHAN"].ToString(),
                            //Thông tin Sale đề nghị
                            SALETrangThaiDeNghi = !string.IsNullOrEmpty(item["SALEDUYET"].ToString()) && item["SALEDUYET"].ToString().ToUpper() == "X" ? "Đã duyệt" : string.Empty,
                            SALENgayDeNghi = !string.IsNullOrEmpty(item["SALEDATE"].ToString()) && !item["SALEDATE"].ToString().Contains("00000000") ?
                                                            DateTime.ParseExact(item["SALEDATE"].ToString(),
                                                              "yyyyMMdd",
                                                               CultureInfo.InvariantCulture).ToString("dd.MM.yyyy") : string.Empty,
                            SALENhanVienDeNghi = item["TENSALEDUYET"].ToString(),
                            SALENote = item["SALENOTE"].ToString(),
                            SALEEmail = item["SALEEMAIL"].ToString(),
                            //Thông tin phòng công nợ
                            CNOKyNhay = item["CNONHAY"].ToString(),
                            CNONgayKyNhay = !string.IsNullOrEmpty(item["CNONHAYDATE"].ToString()) && !item["CNONHAYDATE"].ToString().Contains("00000000") ?
                                                            DateTime.ParseExact(item["CNONHAYDATE"].ToString(),
                                                              "yyyyMMdd",
                                                               CultureInfo.InvariantCulture).ToString("dd.MM.yyyy") : string.Empty,
                            CNONVKyNhay = item["CNONHAYUSERNM"].ToString(),
                            CNOTrangThaiDuyet = !string.IsNullOrEmpty(item["CNODUYET"].ToString()) && item["CNODUYET"].ToString().ToUpper() == "X" ? "Đã duyệt" : string.Empty,
                            CNONgayDuyet = !string.IsNullOrEmpty(item["CNODATE"].ToString()) && !item["CNODATE"].ToString().Contains("00000000") ?
                                                            DateTime.ParseExact(item["CNODATE"].ToString(),
                                                              "yyyyMMdd",
                                                               CultureInfo.InvariantCulture).ToString("dd.MM.yyyy") : string.Empty,
                            CNOCanBoDuyet = item["TENCNODUYET"].ToString(),
                            CNONote = item["CNONOTE"].ToString(),
                            //Thông tin quản lý kiểm tra và duyệt
                            MANKyNhay = item["MANNHAY"].ToString(),
                            MANNgayKyNhay = !string.IsNullOrEmpty(item["MANNHAYDATE"].ToString()) && !item["MANNHAYDATE"].ToString().Contains("00000000") ?
                                                            DateTime.ParseExact(item["MANNHAYDATE"].ToString(),
                                                              "yyyyMMdd",
                                                               CultureInfo.InvariantCulture).ToString("dd.MM.yyyy") : string.Empty,
                            MANCanBoKyNhay = item["MANNHAYUSERNM"].ToString(),
                            MANTrangThaiDuyet = !string.IsNullOrEmpty(item["MANDUYET"].ToString()) && item["MANDUYET"].ToString().ToUpper() == "X" ? "Đã duyệt" : string.Empty,
                            MANNgayDuyet = !string.IsNullOrEmpty(item["MANDATE"].ToString()) && !item["MANDATE"].ToString().Contains("00000000") ?
                                                            DateTime.ParseExact(item["MANDATE"].ToString(),
                                                              "yyyyMMdd",
                                                               CultureInfo.InvariantCulture).ToString("dd.MM.yyyy") : string.Empty,
                            MANDuyet = item["TENMANDUYET"].ToString(),
                            MANNote = item["NOTEDUYET"].ToString(),
                            //Thông tin TGĐ/PTGĐ kiểm tra và duyệt
                            HEADTrangThaiDuyet = !string.IsNullOrEmpty(item["HEADDUYET"].ToString()) && item["HEADDUYET"].ToString().ToUpper() == "X" ? "Đã duyệt" : string.Empty,
                            HEADNgayDuyet = !string.IsNullOrEmpty(item["HEADDATE"].ToString()) && !item["HEADDATE"].ToString().Contains("00000000") ?
                                                            DateTime.ParseExact(item["HEADDATE"].ToString(),
                                                              "yyyyMMdd",
                                                               CultureInfo.InvariantCulture).ToString("dd.MM.yyyy") : string.Empty,
                            HEADDuyet = item["TENHEADDUYET"].ToString(),
                            HEADNote = item["HEADNOTE"].ToString(),
                        });
                    }
                }
                if (result != null && result.Count > 0)
                {
                    return _APISuccess(result);
                }
                else
                {
                    return _APIError("Không có dữ liệu. Xin kiểm tra lại tham số !");
                }
            });
        }
        #endregion

        #region Review
        public ActionResult GetDanhGia(string CompanyCode, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                //Trả về thông tin đánh giá
                var result = new List<ReviewResultViewModel>();
                var dt = _unitOfWork.TaskRepository.GetDanhGia(CompanyCode);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        result.Add(new ReviewResultViewModel()
                        {
                            TIEUCHI = item["TIEUCHI"].ToString(),
                            DIEM = item["DIEM"].ToString(),
                            STT = Convert.ToInt32(item["STT"].ToString()),
                        });
                    }
                }
                if (result != null && result.Count > 0)
                {
                    return _APISuccess(result.OrderBy(p => p.STT).ToList());
                }
                else
                {
                    return _APIError("Không có dữ liệu. Xin kiểm tra lại tham số !");
                }
            });
        }
        #endregion

        #region Set credit limit
        public ActionResult SetCreditLimitUpdate(CreditLimitFormViewModel viewModel, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                //Check field bắt buộc khi lưu note
                if (viewModel.FormType == "SAVE")
                {
                    //Chech theo cấp duyệt
                    /*
                     Tùy theo cấp đang thao tác mà lấy đúng field truyền vào:
                    - Cấp SALE lấy field: SALENOTE
                    - Cấp CONGNO lấy field: CNONOTE
                    - Cấp MANAGER lấy field: NOTEDUYET
                    - Cấp HEAD lấy field: HEADNOTE
                     */
                    if (viewModel.CapDuyet == "SALE" && string.IsNullOrEmpty(viewModel.SALENote))
                    {
                        return _APIError("Vui lòng nhập thông tin Note ở mục Thông tin Sale đề nghị!");
                    }
                    if (viewModel.CapDuyet == "CONGNO" && string.IsNullOrEmpty(viewModel.CNONote))
                    {
                        return _APIError("Vui lòng nhập thông tin Note ở mục Thông tin phòng công nợ!");
                    }
                    if (viewModel.CapDuyet == "MANAGER" && string.IsNullOrEmpty(viewModel.MANNote))
                    {
                        return _APIError("Vui lòng nhập thông tin Note ở mục Thông tin quản lý kiểm tra và duyệt!");
                    }
                    if (viewModel.CapDuyet == "HEAD" && string.IsNullOrEmpty(viewModel.HEADNote))
                    {
                        return _APIError("Vui lòng nhập thông tin Note ở mục Thông tin TGĐ/PTGĐ kiểm tra và duyệt!");
                    }
                }
                #region Lấy lại các thông tin từ SAP để gửi mail
                var searchViewModel = new CreditLimitSearchViewModel();
                searchViewModel.CompanyCode = viewModel.MaCongTy;
                if (!string.IsNullOrEmpty(viewModel.NamHanMuc))
                {
                    int x = 0;

                    if (Int32.TryParse(viewModel.NamHanMuc, out x))
                    {
                        searchViewModel.NamTruoc = x.ToString();
                    }
                }
                searchViewModel.NamHienTai = viewModel.NamHanMuc;
                searchViewModel.CapDuyet = viewModel.CapDuyet;
                //searchViewModel.Duyet = "";
                //searchViewModel.PhongBan = "";
                searchViewModel.MaSAPKH = viewModel.MaKhachHang;
                //searchViewModel.NVSale = "";
                //searchViewModel.NVCongNo = "";
                searchViewModel.GetRow = 5;

                //Trả về thông tin hạn mức công nợ khách hàng
                var result = new List<CreditLimitSearchResultViewModel>();
                var dtResult = _unitOfWork.TaskRepository.GetCreditLimit(searchViewModel);
                if (dtResult != null && dtResult.Rows.Count > 0)
                {
                    //Năm hiện tại
                    int currentYear = Convert.ToInt32(searchViewModel.NamHienTai);
                    //Một năm trước
                    var lastYear = currentYear - 1;
                    //Hai năm trước
                    var twoYearsAgo = currentYear - 2;
                    foreach (DataRow item in dtResult.Rows)
                    {
                        result.Add(new CreditLimitSearchResultViewModel()
                        {
                            Nam0 = twoYearsAgo.ToString(),
                            Nam1 = lastYear.ToString(),
                            Nam2 = currentYear.ToString(),
                            NaturalNumber = Convert.ToInt32(item["IDNO"].ToString()),
                            EmailList = item["EMAIL_LIST"].ToString(),
                            EmailNext = item["EMAIL_NEXT"].ToString(),
                            NamHanMuc = item["NAM"].ToString(),
                            KyHanMuc = item["PERIOD"].ToString(),
                            SaleName = item["SALENAME"].ToString(),
                            EmailPrior = item["EMAIL_PRIOR"].ToString(),
                            //Thông tin khách hàng
                            MaKhachHang = item["KUNNR"].ToString(),
                            TenKhachHang = item["CUSTNM"].ToString(),
                            MaCDT = item["MACDT"].ToString(),
                            TenCDT = item["TENCDT"].ToString(),
                            NhomKhachHang = item["KTEXT"].ToString(),
                            CNOXepLoai = item["CNOXEPLOAI"].ToString(),
                            SALEXepLoai = item["XEPLOAI"].ToString(),
                            DanhGiaKH = item["DANHGIAKH"].ToString(),
                            //------------------------------------------
                            //Thông tin công ty
                            MaCongTy = item["WERKS"].ToString(),
                            CongTyBanHang = item["WERKS"].ToString(),
                            PhongBan = item["DEPTNM"].ToString(),
                            SALEPhuTrach = item["SALENAME"].ToString(),
                            CongNo = item["CNONAME"].ToString(),
                            CapQuanLy = item["QLYNAME"].ToString(),
                            NgayBaoCao = DateTime.Now.ToString("dd.MM.yyyy"), //lấy ngày hiện tại
                            //------------------------------------------
                            NoDauKyNam1 = !string.IsNullOrEmpty(item["DUDAUKY1"].ToString()) ? Convert.ToDecimal(item["DUDAUKY1"]).ToString("n0") : string.Empty,
                            PhatSinhBanNam1 = !string.IsNullOrEmpty(item["INKYNO1"].ToString()) ? Convert.ToDecimal(item["INKYNO1"]).ToString("n0") : string.Empty,
                            PhatSinhThuNam1 = !string.IsNullOrEmpty(item["INKYCO1"].ToString()) ? Convert.ToDecimal(item["INKYCO1"]).ToString("n0") : string.Empty,
                            DuNoCuoiKyNam1 = !string.IsNullOrEmpty(item["DUCUOIKY1"].ToString()) ? Convert.ToDecimal(item["DUCUOIKY1"]).ToString("n0") : string.Empty,
                            //------------------------------------------
                            NoDauKyNam2 = !string.IsNullOrEmpty(item["DUDAUKY2"].ToString()) ? Convert.ToDecimal(item["DUDAUKY2"]).ToString("n0") : string.Empty,
                            PhatSinhBanNam2 = !string.IsNullOrEmpty(item["INKYNO2"].ToString()) ? Convert.ToDecimal(item["INKYNO2"]).ToString("n0") : string.Empty,
                            PhatSinhThuNam2 = !string.IsNullOrEmpty(item["INKYCO2"].ToString()) ? Convert.ToDecimal(item["INKYCO2"]).ToString("n0") : string.Empty,
                            DuNoCuoiKyNam2 = !string.IsNullOrEmpty(item["DUCUOIKY2"].ToString()) ? Convert.ToDecimal(item["DUCUOIKY2"]).ToString("n0") : string.Empty,
                            //------------------------------------------
                            DoanhSoBanNam0 = !string.IsNullOrEmpty(item["DOANHSOSAP0"].ToString()) ? Convert.ToDecimal(item["DOANHSOSAP0"]).ToString("n0") : string.Empty,
                            DoanhSoBanNam1 = !string.IsNullOrEmpty(item["DOANHSOSAP1"].ToString()) ? Convert.ToDecimal(item["DOANHSOSAP1"]).ToString("n0") : string.Empty,
                            DoanhSoBanNam2 = !string.IsNullOrEmpty(item["DOANHSOSAP2"].ToString()) ? Convert.ToDecimal(item["DOANHSOSAP2"]).ToString("n0") : string.Empty,
                            //------------------------------------------
                            ThoiHanNoNam1 = item["ZTERMDESC1"].ToString(),
                            HanMucNoNam1 = !string.IsNullOrEmpty(item["CREDITLIMIT1"].ToString()) ? Convert.ToDecimal(item["CREDITLIMIT1"]).ToString("n0") : string.Empty,
                            HanMucThemNam1 = !string.IsNullOrEmpty(item["CREDITHIDE1"].ToString()) ? Convert.ToDecimal(item["CREDITHIDE1"]).ToString("n0") : string.Empty,
                            TongHanMucNam1 = !string.IsNullOrEmpty(item["CREDITTOL1"].ToString()) ? Convert.ToDecimal(item["CREDITTOL1"]).ToString("n0") : string.Empty,
                            GhiChuThoiHan1 = item["NOTETHOIHAN1"].ToString(),
                            //Chính sách công nợ đang áp dụng năm (YHT)
                            ThoiHanNoDangAp = item["ZTERMDESC"].ToString(),
                            HanMucNoDangAp = !string.IsNullOrEmpty(item["CREDITLIMIT"].ToString()) ? Convert.ToDecimal(item["CREDITLIMIT"]).ToString("n0") : string.Empty,
                            HanMucThemDangAp = !string.IsNullOrEmpty(item["CREDITHIDE"].ToString()) ? Convert.ToDecimal(item["CREDITHIDE"]).ToString("n0") : string.Empty,
                            TongHanMucDangAp = !string.IsNullOrEmpty(item["CREDITTOL"].ToString()) ? Convert.ToDecimal(item["CREDITTOL"]).ToString("n0") : string.Empty,
                            //Đề xuất chính sách công nợ mới
                            NgayDeXuat = !string.IsNullOrEmpty(item["NGAYDXHM"].ToString()) && !item["NGAYDXHM"].ToString().Contains("00000000") ?
                                                            DateTime.ParseExact(item["NGAYDXHM"].ToString(),
                                                              "yyyyMMdd",
                                                               CultureInfo.InvariantCulture).ToString("dd.MM.yyyy") : string.Empty,
                            ThoiHanNoNam2 = item["ZTERMDESC_DX"].ToString(),
                            HanMucNoNam2 = !string.IsNullOrEmpty(item["CREDITLIMIT_DX"].ToString()) ? Convert.ToDecimal(item["CREDITLIMIT_DX"]).ToString("n0") : string.Empty,
                            HanMucThemNam2 = !string.IsNullOrEmpty(item["CREDITHIDE_DX"].ToString()) ? Convert.ToDecimal(item["CREDITHIDE_DX"]).ToString("n0") : string.Empty,
                            TongHanMucNam2 = !string.IsNullOrEmpty(item["CREDITTOL_DX"].ToString()) ? Convert.ToDecimal(item["CREDITTOL_DX"]).ToString("n0") : string.Empty,
                            GhiChuThoiHan2 = item["NOTETHOIHAN"].ToString(),
                            //Thông tin Sale đề nghị
                            SALETrangThaiDeNghi = !string.IsNullOrEmpty(item["SALEDUYET"].ToString()) && item["SALEDUYET"].ToString().ToUpper() == "X" ? "Đã duyệt" : string.Empty,
                            SALENgayDeNghi = !string.IsNullOrEmpty(item["SALEDATE"].ToString()) && !item["SALEDATE"].ToString().Contains("00000000") ?
                                                            DateTime.ParseExact(item["SALEDATE"].ToString(),
                                                              "yyyyMMdd",
                                                               CultureInfo.InvariantCulture).ToString("dd.MM.yyyy") : string.Empty,
                            SALENhanVienDeNghi = item["TENSALEDUYET"].ToString(),
                            SALENote = item["SALENOTE"].ToString(),
                            SALEEmail = item["SALEEMAIL"].ToString(),
                            //Thông tin phòng công nợ
                            CNOKyNhay = item["CNONHAY"].ToString(),
                            CNONgayKyNhay = !string.IsNullOrEmpty(item["CNONHAYDATE"].ToString()) && !item["CNONHAYDATE"].ToString().Contains("00000000") ?
                                                            DateTime.ParseExact(item["CNONHAYDATE"].ToString(),
                                                              "yyyyMMdd",
                                                               CultureInfo.InvariantCulture).ToString("dd.MM.yyyy") : string.Empty,
                            CNONVKyNhay = item["CNONHAYUSERNM"].ToString(),
                            CNOTrangThaiDuyet = !string.IsNullOrEmpty(item["CNODUYET"].ToString()) && item["CNODUYET"].ToString().ToUpper() == "X" ? "Đã duyệt" : string.Empty,
                            CNONgayDuyet = !string.IsNullOrEmpty(item["CNODATE"].ToString()) && !item["CNODATE"].ToString().Contains("00000000") ?
                                                            DateTime.ParseExact(item["CNODATE"].ToString(),
                                                              "yyyyMMdd",
                                                               CultureInfo.InvariantCulture).ToString("dd.MM.yyyy") : string.Empty,
                            CNOCanBoDuyet = item["TENCNODUYET"].ToString(),
                            CNONote = item["CNONOTE"].ToString(),
                            //Thông tin quản lý kiểm tra và duyệt
                            MANKyNhay = item["MANNHAY"].ToString(),
                            MANNgayKyNhay = !string.IsNullOrEmpty(item["MANNHAYDATE"].ToString()) && !item["MANNHAYDATE"].ToString().Contains("00000000") ?
                                                            DateTime.ParseExact(item["MANNHAYDATE"].ToString(),
                                                              "yyyyMMdd",
                                                               CultureInfo.InvariantCulture).ToString("dd.MM.yyyy") : string.Empty,
                            MANCanBoKyNhay = item["MANNHAYUSERNM"].ToString(),
                            MANTrangThaiDuyet = !string.IsNullOrEmpty(item["MANDUYET"].ToString()) && item["MANDUYET"].ToString().ToUpper() == "X" ? "Đã duyệt" : string.Empty,
                            MANNgayDuyet = !string.IsNullOrEmpty(item["MANDATE"].ToString()) && !item["MANDATE"].ToString().Contains("00000000") ?
                                                            DateTime.ParseExact(item["MANDATE"].ToString(),
                                                              "yyyyMMdd",
                                                               CultureInfo.InvariantCulture).ToString("dd.MM.yyyy") : string.Empty,
                            MANDuyet = item["TENMANDUYET"].ToString(),
                            MANNote = item["NOTEDUYET"].ToString(),
                            //Thông tin TGĐ/PTGĐ kiểm tra và duyệt
                            HEADTrangThaiDuyet = !string.IsNullOrEmpty(item["HEADDUYET"].ToString()) && item["HEADDUYET"].ToString().ToUpper() == "X" ? "Đã duyệt" : string.Empty,
                            HEADNgayDuyet = !string.IsNullOrEmpty(item["HEADDATE"].ToString()) && !item["HEADDATE"].ToString().Contains("00000000") ?
                                                            DateTime.ParseExact(item["HEADDATE"].ToString(),
                                                              "yyyyMMdd",
                                                               CultureInfo.InvariantCulture).ToString("dd.MM.yyyy") : string.Empty,
                            HEADDuyet = item["TENHEADDUYET"].ToString(),
                            HEADNote = item["HEADNOTE"].ToString(),
                        });
                    }
                }
                if (result != null && result.Count > 0)
                {
                    var info = result.FirstOrDefault();
                    viewModel.ZTERMDESC = info.ThoiHanNoDangAp;
                    viewModel.CREDITLIMIT = info.HanMucNoDangAp;
                    viewModel.CREDITHIDE = info.HanMucThemDangAp;
                    viewModel.CREDITTOL = info.TongHanMucDangAp;
                }
                #endregion

                //Set trạng thái Duyệt/Bỏ Duyệt hạn mức khách hàng vào SAP Table
                string message = string.Empty;
                int number = 0;
                //Nếu không duyệt thì chỉ gửi mail không cần gửi lên SAP
                if (viewModel.FormType == "NOTAPPROVED")
                {
                    SendEmail(viewModel);
                    number = 1;
                    message = "Đã gửi mail Không duyệt thành công!";
                }
                else
                {
                    //gán note theo cấp duyệt
                    if (viewModel.CapDuyet == "SALE")
                    {
                        viewModel.Note = viewModel.SALENote;
                    }
                    else if (viewModel.CapDuyet == "CONGNO")
                    {
                        viewModel.Note = viewModel.CNONote;
                    }
                    else if (viewModel.CapDuyet == "MANAGER")
                    {
                        viewModel.Note = viewModel.MANNote;
                    }
                    else if (viewModel.CapDuyet == "HEAD")
                    {
                        viewModel.Note = viewModel.HEADNote;
                    }
                    var dt = _unitOfWork.TaskRepository.SetCreditLimit(viewModel);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow item in dt.Rows)
                        {
                            if (viewModel.FormType == "UPDATE" && Convert.ToInt32(item["MSGNO"].ToString()) == 1)
                            {
                                SendEmail(viewModel);
                            }
                            number = Convert.ToInt32(item["MSGNO"].ToString());
                            message = item["MSGNAME"].ToString();
                        }
                    }
                }

                return _APISuccess(new { MessageNo = number, MessageName = message });
            });
        }

        private void SendEmail(CreditLimitFormViewModel viewModel)
        {
            //Lấy thông tin từ cấu hình Email
            //Loại: Duyệt hạn mức công nợ
            var config = _unitOfWork.EmailTemplateConfigRepository.GetByType(ConstEmailTemplateConfig.DuyetHanCongNo);
            //string FromEmail = viewModel.FromEmail;
            string FromEmail = config.FromEmail;
            //string ToEmail = string.Format("{0};{1}", viewModel.ToEmail, viewModel.EmailList);
            string ToEmail = string.Format("{0};{1};{2}", viewModel.EmailList, viewModel.EmailNext, config.FromEmail);
            if (viewModel.FormType == "NOTAPPROVED")
            {
                ToEmail = viewModel.EmailPrior;
            }
            //string Subject = "[Không duyệt]/[Duyệt]/[Bỏ duyệt] hạn mức công nợ KH: [Mã KH] - [Tên khách hàng]";
            string Subject = config.Subject;

            string EmailContent = string.Empty;
            EmailContent += config.Content;
            //EmailContent += "Gửi Anh/Chị/Em," + "<br />";
            //EmailContent += "<br />";
            ////1.
            //EmailContent += "<b>1/ Thông tin khách hàng đề xuất duyệt hạn mức công nợ: </b>" + "<br />";

            ////EmailContent += "- Khách hàng sau đã được [Không duyệt]/[Duyệt]/[Bỏ duyệt] hạn mức công nợ: ";
            ////EmailContent += "<br />";

            //EmailContent += "- Mã KH: [Mã KH]";
            //EmailContent += "<br />";

            //EmailContent += "- Tên KH: [Tên khách hàng]";
            //EmailContent += "<br />";

            //EmailContent += "- Mã CĐT: [Mã CĐT]";
            //EmailContent += "<br />";

            //EmailContent += "- Tên CĐT: [Tên CĐT]";
            //EmailContent += "<br />";

            //EmailContent += "- Nhóm KH: [Nhóm KH]";
            //EmailContent += "<br />";

            //EmailContent += "- Khối/Phòng ban: [Phòng ban]";
            //EmailContent += "<br />";

            //EmailContent += "- Sale phụ trách: [SALEName]";
            //EmailContent += "<br />";

            //EmailContent += "- Email Sale phụ trách: [SALEEmail]";
            //EmailContent += "<br />";

            //EmailContent += "- Công nợ: [CNOName]";
            //EmailContent += "<br />";

            //EmailContent += "- Cấp quản lý: [MANName]";
            //EmailContent += "<br />";

            //EmailContent += "<br />";

            //EmailContent += "- Ngày đề xuất: [NgayDeXuat]";
            //EmailContent += "<br />";

            //EmailContent += "- Thời hạn nợ năm [NamHanMuc]: [ThoiHanNoNam2]";
            //EmailContent += "<br />";

            //EmailContent += "- Hạn mức nợ năm [NamHanMuc]: [HanMucNoNam2]";
            //EmailContent += "<br />";

            //EmailContent += "- Hạn mức thêm năm [NamHanMuc]: [HanMucThemNam2]";
            //EmailContent += "<br />";

            //EmailContent += "- Tổng hạn mức năm [NamHanMuc]: [TongHanMucNam2]";
            //EmailContent += "<br />";

            //EmailContent += "<br />";

            ////2.
            //EmailContent += "<b>2/ Cấp xét duyệt hiện tại: [CẤP DUYỆT]</b>" + "<br />";

            //EmailContent += "- Trạng thái: [Không duyệt]/[Duyệt]/[Bỏ duyệt]";
            //EmailContent += "<br />";

            //EmailContent += "- Ghi chú: [Note]";
            //EmailContent += "<br />";

            //EmailContent += "- UserNo: [UserName]";
            //EmailContent += "<br />";

            //EmailContent += "- Email: [FromEmail]";
            //EmailContent += "<br />";

            //EmailContent += "<br />";
            ////3.
            //EmailContent += "<b>3/ Cấp xét duyệt tiếp theo: </b>" + "<br />";
            //EmailContent += "- Có Email: [EmailNext]" + "<br />";
            //EmailContent += "- Vui lòng vào App CRM xét duyệt tiếp hạn mức công nợ cho khách hàng trên." + "<br />";
            //EmailContent += "<br />";

            //EmailContent += "* Những Anh/Chị/Em có liên quan vui lòng kiểm tra lại thông tin!";
            //EmailContent += "<br />";
            //EmailContent += "* Email được gửi từ App duyệt hạn mức công nợ khách hàng.";
            //EmailContent += "<br />";

            string CapDuyet = viewModel.CapDuyet;
            if (CapDuyet == "CONGNO")
            {
                CapDuyet = "CÔNG NỢ";
            }
            else if (CapDuyet == "MANAGER")
            {
                CapDuyet = "QUẢN LÝ";
            }

            string Note = string.Empty;
            if (viewModel.CapDuyet == "SALE")
            {
                Note = viewModel.SALENote; 
            }
            else if (viewModel.CapDuyet == "CONGNO")
            {
                Note = viewModel.CNONote;
            }
            else if (viewModel.CapDuyet == "MANAGER")
            {
                Note = viewModel.MANNote;
            }
            else if (viewModel.CapDuyet == "HEAD")
            {
                Note = viewModel.HEADNote;
            }
            EmailContent = EmailContent.Replace("[CẤP DUYỆT]", CapDuyet)
                                       .Replace("[Không duyệt]/[Duyệt]/[Bỏ duyệt]", viewModel.FormType == "NOTAPPROVED" ? "Không duyệt" : (viewModel.Status == "X" ? "Duyệt" : "Bỏ Duyệt"))
                                       .Replace("[Kh&ocirc;ng duyệt]/[Duyệt]/[Bỏ duyệt]", viewModel.FormType == "NOTAPPROVED" ? "Không duyệt" : (viewModel.Status == "X" ? "Duyệt" : "Bỏ Duyệt"))
                                       .Replace("[Mã KH]", viewModel.MaKhachHang)
                                       .Replace("[M&atilde; KH]", viewModel.MaKhachHang)
                                       .Replace("[Tên khách hàng]", viewModel.TenKhachHang)
                                       .Replace("[T&ecirc;n kh&aacute;ch h&agrave;ng]", viewModel.TenKhachHang)
                                       .Replace("[UserNo]", viewModel.UserNo)
                                       .Replace("[UserName]", viewModel.UserName)
                                       .Replace("[FromEmail]", viewModel.FromEmail)
                                       .Replace("[EmailNext]", viewModel.FormType == "NOTAPPROVED" ? viewModel.EmailPrior : viewModel.EmailNext)
                                       .Replace("[SaleName]", viewModel.SaleName)
                                       .Replace("[EmailList]", viewModel.EmailList)
                                       .Replace("[Note]", Note)
                                       .Replace("[Xem lại]/[Duyệt]/[Bỏ duyệt]", viewModel.FormType == "NOTAPPROVED" ? "Xem lại" : (viewModel.Status == "X" ? "Duyệt" : "Bỏ Duyệt"))
                                       .Replace("[Mã CĐT]", viewModel.MaCDT)
                                       .Replace("[M&atilde; CĐT]", viewModel.MaCDT)
                                       .Replace("[Tên CĐT]", viewModel.TenCDT)
                                       .Replace("[T&ecirc;n CĐT]", viewModel.TenCDT)
                                       .Replace("[Nhóm KH]", viewModel.NhomKhachHang)
                                       .Replace("[Nh&oacute;m KH]", viewModel.NhomKhachHang)
                                       .Replace("[Phòng ban]", viewModel.PhongBan)
                                       .Replace("[Ph&ograve;ng ban]", viewModel.PhongBan)
                                       .Replace("[SALEName]", viewModel.SaleName)
                                       .Replace("[SALEEmail]", viewModel.SALEEmail)
                                       .Replace("[CNOName]", viewModel.CNOName)
                                       .Replace("[MANName]", viewModel.MANName)
                                       .Replace("[NgayDeXuat]", viewModel.NgayDeXuat)
                                       .Replace("[ThoiHanNoNam2]", viewModel.ThoiHanNoNam2)
                                       .Replace("[HanMucNoNam2]", viewModel.HanMucNoNam2)
                                       .Replace("[HanMucThemNam2]", viewModel.HanMucThemNam2)
                                       .Replace("[TongHanMucNam2]", viewModel.TongHanMucNam2)
                                       .Replace("[NamHanMuc]", viewModel.NamHanMuc)
                                       .Replace("[ZTERMDESC]", viewModel.ZTERMDESC)
                                       .Replace("[CREDITLIMIT]", viewModel.CREDITLIMIT)
                                       .Replace("[CREDITHIDE]", viewModel.CREDITHIDE)
                                       .Replace("[CREDITTOL]", viewModel.CREDITTOL)
            ;
            Subject = Subject.Replace("[Không duyệt]/[Duyệt]/[Bỏ duyệt]", viewModel.FormType == "NOTAPPROVED" ? "Không duyệt" : (viewModel.Status == "X" ? "Duyệt" : "Bỏ Duyệt"))
                             .Replace("[Không Duyệt]/[Duyệt]/[Bỏ duyệt]", viewModel.FormType == "NOTAPPROVED" ? "Không duyệt" : (viewModel.Status == "X" ? "Duyệt" : "Bỏ Duyệt"))
                             .Replace("[Mã KH]", viewModel.MaKhachHang)
                             .Replace("[Tên khách hàng]", viewModel.TenKhachHang);


            _unitOfWork.TaskRepository.SendMailCreditLimit(EmailContent, Subject, FromEmail, ToEmail, config.EmailTemplateType);
        }
        #endregion
    }
}