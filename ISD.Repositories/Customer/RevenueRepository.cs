using ISD.Constant;
using ISD.EntityModels;
using ISD.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;

namespace ISD.Repositories
{
    public class RevenueRepository
    {
        private EntityDataContext _context;

        public RevenueRepository(EntityDataContext db)
        {
            _context = db;
        }

        /// <summary>
        /// Dannh sách doanh số KH
        /// </summary>
        /// <param name="CompanyId">Công ty</param>
        /// <param name="CustomerCode">Mã khách hàng</param>
        /// <param name="PhoneNumber">SĐT</param>
        /// <param name="FromLimit">Hạn mức từ</param>
        /// <param name="ToLimit">Hạn mức đến</param>
        /// <param name="Row">Số dòng hiển thị</param>
        /// <returns>Danh sách doah số KH</returns>
        public List<RevenueViewModel> Search(Guid? CompanyId = null, string CustomerCode = "", string PhoneNumber = "", decimal? FromLimit = null, decimal? ToLimit = null, int? Row = 100)
        {
            List<RevenueViewModel> lst = new List<RevenueViewModel>();
            var company = _context.CompanyModel.Where(p => p.CompanyId == CompanyId).FirstOrDefault();
            var datatable = GetCustomerData(company.CompanyCode, PhoneNumber, CustomerCode, FromLimit, ToLimit, Row, 2);

            if (datatable != null && datatable.Rows.Count > 0)
            {
                foreach (DataRow item in datatable.Rows)
                {
                    RevenueViewModel model = new RevenueViewModel();
                    model.WERKS = item["WERKS"].ToString();
                    model.BUKRS = item["BUKRS"].ToString();
                    model.KUNNR = item["KUNNR"].ToString();
                    model.CUSTNAME = item["CUSTNAME"].ToString();
                    model.CUSTPHONE = item["CUSTPHONE"].ToString();
                    model.CUSTADDR = item["CUSTADDR"].ToString();
                    model.DOANHSO = Convert.ToDecimal(item["DOANHSO"].ToString());
                    model.STCEG = item["STCEG"].ToString();
                    model.CUSTGROUP = item["CUSTGROUP"].ToString();
                    model.SALEDISTRICT = item["SALEDISTRICT"].ToString();

                    //Hạn mức khách hàng
                    var currentLevel = _context.ProfileLevelModel
                                               .Where(p => p.LineOfLevel <= model.DOANHSO && p.CompanyId == company.CompanyId && p.Actived == true)
                                               .OrderByDescending(p => p.LineOfLevel).FirstOrDefault();
                    if (currentLevel != null)
                    {
                        model.CustomerLevel = currentLevel.CustomerLevelCode + " | " + currentLevel.CustomerLevelName;
                        model.CurrentLimit = currentLevel.LineOfLevel;
                        model.Point = model.DOANHSO / currentLevel.ExchangeValue;
                        model.ExchangeValue = currentLevel.ExchangeValue;

                        var nextLevel = _context.ProfileLevelModel
                                            .Where(p => p.LineOfLevel > currentLevel.LineOfLevel && p.CompanyId == company.CompanyId && p.Actived == true)
                                            .OrderBy(p => p.LineOfLevel)
                                            .FirstOrDefault();
                        if (nextLevel != null)
                        {
                            model.NextLimit = nextLevel.LineOfLevel;
                            model.MissingValue = model.NextLimit - model.DOANHSO;
                        }
                    }
                    else
                    {
                        var nextLimit = _context.ProfileLevelModel.Where(p => p.CompanyId == company.CompanyId)
                                                .OrderBy(p => p.LineOfLevel).FirstOrDefault();
                        if (nextLimit != null)
                        {
                            model.NextLimit = nextLimit.LineOfLevel;
                            model.MissingValue = model.NextLimit - model.DOANHSO;
                        }
                    }
                    lst.Add(model);
                }
            }
            lst = lst.OrderByDescending(p => p.DOANHSO).ToList();
            return lst;
        }

        /// <summary>
        /// Lấy doanh số cá nhân
        /// </summary>
        /// <param name="ProfileId"></param>
        /// <returns></returns>
        public RevenueViewModel GetProfileRevenue(Guid? ProfileId)
        {
            try
            {
                RevenueViewModel model = new RevenueViewModel();
                var profile = _context.ProfileModel.Where(p => p.ProfileId == ProfileId).FirstOrDefault();
                if (profile != null)
                {
                    if (string.IsNullOrEmpty(profile.ProfileForeignCode))
                    {
                        return new RevenueViewModel();
                    }
                    var datatable = GetCustomerData(profile.CreateAtCompany, null, profile.ProfileForeignCode, null, null, 100, 1);
                    if (datatable != null && datatable.Rows.Count > 0)
                    {
                        var item = datatable.Rows[0];
                        model.WERKS = item["WERKS"].ToString();
                        model.BUKRS = item["BUKRS"].ToString();
                        model.KUNNR = item["KUNNR"].ToString();
                        model.CUSTNAME = item["CUSTNAME"].ToString();
                        model.CUSTPHONE = item["CUSTPHONE"].ToString();
                        model.CUSTADDR = item["CUSTADDR"].ToString();
                        model.DOANHSO = Convert.ToDecimal(item["DOANHSO"].ToString());
                        model.STCEG = item["STCEG"].ToString();
                        model.CUSTGROUP = item["CUSTGROUP"].ToString();
                        model.SALEDISTRICT = item["SALEDISTRICT"].ToString();

                        //Hạn mức khách hàng
                        var company = _context.CompanyModel.Where(p => p.CompanyCode == profile.CreateAtCompany).FirstOrDefault();
                        var currentLevel = _context.ProfileLevelModel
                                                   .Where(p => p.LineOfLevel <= model.DOANHSO && p.CompanyId == company.CompanyId && p.Actived == true)
                                                   .OrderByDescending(p => p.LineOfLevel).FirstOrDefault();
                        if (currentLevel != null)
                        {
                            model.CustomerLevel = currentLevel.CustomerLevelCode + " | " + currentLevel.CustomerLevelName;
                            model.CurrentLimit = currentLevel.LineOfLevel;
                            model.Point = model.DOANHSO / currentLevel.ExchangeValue;
                            model.ExchangeValue = currentLevel.ExchangeValue;

                            var nextLevel = _context.ProfileLevelModel
                                                .Where(p => p.LineOfLevel > currentLevel.LineOfLevel && p.CompanyId == company.CompanyId && p.Actived == true)
                                                .OrderBy(p => p.LineOfLevel)
                                                .FirstOrDefault();
                            if (nextLevel != null)
                            {
                                model.NextLimit = nextLevel.LineOfLevel;
                                model.MissingValue = model.NextLimit - model.DOANHSO;
                            }
                        }
                        else
                        {
                            var nextLimit = _context.ProfileLevelModel.Where(p => p.CompanyId == company.CompanyId)
                                                    .OrderBy(p => p.LineOfLevel).FirstOrDefault();
                            if (nextLimit != null)
                            {
                                model.NextLimit = nextLimit.LineOfLevel;
                                model.MissingValue = model.NextLimit - model.DOANHSO;
                            }
                        }
                    }
                }
                return model;
            }
            catch// (Exception ex)
            {
                return new RevenueViewModel();
            }
            
        }

        public DataTable GetCustomerData(string CompanyCode, string PhoneNumber, string CustomerCode, decimal? FromLimit, decimal? ToLimit, int? Row, int PageType)
        {
            //Khởi tạo thư viện và kết nối
            var _sap = new SAPRepository();
            var destination = _sap.GetRfcWithConfig();
            //Định nghĩa hàm cần gọi
            var function = destination.Repository.CreateFunction(ConstantFunctionName.YAC_FM_WEB_GET_DOANHSO);

            //Thông số truyền vào
            function.SetValue("IM_PAGETYPE", PageType);
            function.SetValue("IM_WERKS", CompanyCode);
            function.SetValue("IM_KUNNR", CustomerCode);
            function.SetValue("IM_PHONE", PhoneNumber);
            function.SetValue("IM_FRAMT", FromLimit);
            function.SetValue("IM_TOAMT", ToLimit);
            function.SetValue("IM_TOPROW", Row);

            //Thực thi
            function.Invoke(destination);

            var datatable = function.GetTable("AMOUNT_T").ToDataTable("AMOUNT_T");
            if (datatable != null && datatable.Rows.Count > 0)
            {
                datatable = datatable.AsEnumerable().Take((int)Row).CopyToDataTable();
            }
            return datatable;
        }

        /// <summary>
        /// Lấy doanh số khách hàng theo thời gian
        /// </summary>
        /// <param name="ProfileId"></param>
        /// <returns></returns>
        public List<RevenueViewModel> GetProfileRevenueBy(Guid? ProfileId, string Year, string CompanyCode)
        {
            try
            {
                List<RevenueViewModel> model = new List<RevenueViewModel>();
                var profile = _context.ProfileModel.Where(p => p.ProfileId == ProfileId).FirstOrDefault();
                if (profile != null)
                {
                    if (string.IsNullOrEmpty(profile.ProfileForeignCode))
                    {
                        return new List<RevenueViewModel>();
                    }

                    //lấy doanh số theo thời gian
                    //var datatable = GetDoanhSoTheoThoiGian(profile.CreateAtCompany, profile.ProfileForeignCode);
                    var datatable = GetDoanhSoTheoThoiGian(CompanyCode, profile.ProfileForeignCode);
                    if (datatable != null && datatable.Rows.Count > 0)
                    {
                        foreach (DataRow item in datatable.Rows)
                        {
                            string MaCongTy = item["WERKS"].ToString();
                            string MaSAPKH = item["KUNNR"].ToString();
                            decimal? DoanhSo = Convert.ToDecimal(item["DOANHSO"].ToString());
                            string year = item["YEARMONTH"].ToString();
                            if (!string.IsNullOrEmpty(year))
                            {
                                year = year.Substring(0, Math.Min(year.Length, 4));
                            }
                            model.Add(new RevenueViewModel()
                            {
                                WERKS = MaCongTy,
                                KUNNR = MaSAPKH,
                                DOANHSO = DoanhSo,
                                YEARMONTH = year
                            });
                        }
                    }
                    //group by theo năm và tính tổng theo từng năm
                    if (model != null && model.Count > 0)
                    {
                        model = model.Where(p => ((Year == null || Year == "") || p.YEARMONTH == Year)).GroupBy(l => l.YEARMONTH)
                                .Select(cl => new RevenueViewModel
                                {
                                    WERKS = cl.First().WERKS,
                                    KUNNR = cl.First().KUNNR,
                                    YEARMONTH = cl.First().YEARMONTH,
                                    DOANHSO = cl.Sum(c => c.DOANHSO),
                                }).ToList();
                    }
                }
                return model;
            }
            catch//(Exception ex)
            {
                return new List<RevenueViewModel>();
            }

        }

        public DataTable GetDoanhSoTheoThoiGian(string CompanyCode, string CustomerCode)
        {
            //Khởi tạo thư viện và kết nối
            var _sap = new SAPRepository();
            var destination = _sap.GetRfcWithConfig();
            //Định nghĩa hàm cần gọi
            var function = destination.Repository.CreateFunction(ConstantFunctionName.YAC_FM_CRM_GET_DOANHSO);

            //Thông số truyền vào
            function.SetValue("IM_WERKS", CompanyCode);
            function.SetValue("IM_KUNNR", CustomerCode);

            //Thực thi
            function.Invoke(destination);

            var datatable = function.GetTable("DOANHSO_T").ToDataTable("DOANHSO_T");
            return datatable;
        }
    }
}