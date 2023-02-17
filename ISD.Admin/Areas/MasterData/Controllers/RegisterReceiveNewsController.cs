using ISD.Core;
using ISD.Repositories.Excel;
using ISD.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MasterData.Controllers
{
    public class RegisterReceiveNewsController : BaseController
    {
        // GET: RegisterReceiveNews
        public ActionResult Index()
        {
            ViewBag.PageId = GetPageId("/MasterData/RegisterReceiveNews");
            CreateViewBag();
            return View();
        }
        public ActionResult _Search(DateTime? FromDate, DateTime? ToDate, string PhoneNumber, string CompanyCode)
        {
            return ExecuteSearch(()=> {
                var result = (from news in _context.RegisterReceiveNewsModel
                              join com in _context.CompanyModel on news.CompanyCode equals com.CompanyCode
                              where (PhoneNumber == "" || news.Phone == PhoneNumber)
                              && (CompanyCode == "" || news.CompanyCode == CompanyCode)
                              && (FromDate == null || FromDate <= news.CreateDate)
                              && (ToDate == null || news.CreateDate <= ToDate)
                              orderby news.CreateDate descending
                              select new RegisterReceiveNewsViewModel
                              {
                                  RegisterReceiveNewsId = news.RegisterReceiveNewsId,
                                  Companyname = com.CompanyName,
                                  FullName = news.FullName,
                                  Address = news.Address,
                                  DistrictName = news.DistrictName,
                                  ProvinceName = news.ProvinceName,
                                  Phone = news.Phone,
                                  Email = news.Email,
                                  Note = news.Note
                              }).ToList();
                return PartialView(result);
            });
        }

        private void CreateViewBag(string CompanyCode = "")
        {
            var comList = _unitOfWork.CompanyRepository.GetAll();
            ViewBag.CompanyCode = new SelectList(comList, "CompanyCode", "CompanyName", CompanyCode);
        }
        public ActionResult ExportExcel(ExternalCustomerSearchModel searchModel)
        {
            List<ExternalCustomerExportModel> result = new List<ExternalCustomerExportModel>();
            result = (from customer in _context.RegisterReceiveNewsModel
                      
                      where (searchModel.CompanyCode == null || customer.CompanyCode == searchModel.CompanyCode) &&
                             (searchModel.PhoneNumber == null || customer.Phone.Contains(searchModel.PhoneNumber)) &&
                              (searchModel.FromDate == null || customer.CreateDate >= searchModel.FromDate) &&
                              (searchModel.ToDate == null || customer.CreateDate <= searchModel.ToDate) &&
                              (customer.Email!=null) &&
                              (customer.Email!="")
                      orderby customer.FullName
                      select new ExternalCustomerExportModel()
                      {                        
                          Name = customer.FullName,
                          Phone = customer.Phone,
                          Email = customer.Email
                      }).ToList();
         
            return Export(result);
        }


        public ActionResult Export(List<ExternalCustomerExportModel> viewModel)
        {
            List<ExcelTemplate> columns = new List<ExcelTemplate>();
            //Header
            string fileheader = string.Empty;
            fileheader = "THÔNG TIN KHÁCH HÀNG ĐĂNG KÝ NHẬN TIN";

            #region Master
            columns.Add(new ExcelTemplate { ColumnName = "Name", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "Phone", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "Email", isAllowedToEdit = false });
           
            // columns.Add(new ExcelTemplate { ColumnName = "CreateTime", isAllowedToEdit = false });
            #endregion

            heading.Add(new ExcelHeadingTemplate()
            {
                Content = "",//controllerCode,
                RowsToIgnore = 1,
                isWarning = false,
                isCode = true
            });
            heading.Add(new ExcelHeadingTemplate()
            {
                Content = fileheader.ToUpper(),
                RowsToIgnore = 1,
                isWarning = false,
                isCode = false
            });

            //Body
            byte[] filecontent = ClassExportExcel.ExportExcel(viewModel, columns, heading, true, HasExtraSheet: false);
            string fileNameWithFormat = string.Format("{0}.xlsx", _unitOfWork.UtilitiesRepository.RemoveSign4VietnameseString(fileheader.ToUpper()).Replace(" ", "_"));

            return File(filecontent, ClassExportExcel.ExcelContentType, fileNameWithFormat);
        }
    }
}