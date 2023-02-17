
using ISD.Extensions;
using ISD.Repositories;
using ISD.ViewModels;
using ISD.Core;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ISD.Repositories.Excel;
using ISD.Constant;
using System.Linq;
using DevExpress.Web.Mvc;

namespace Customer.Controllers
{
    public class TastesController : BaseController
    {
        // Thị hiếu khách hàng
        // GET: Tastes
        #region Index
        [ISDAuthorizationAttribute]
        public ActionResult Index()
        {
            CreateViewBag();
            return View();
        }

        //public ActionResult _Search(CustomerTastesSearchViewModel searchViewModel, Guid? ChooseStoreId = null)
        //{
        //    return ExecuteSearch(() =>
        //    {
        //        List<CustomerTastesReportViewModel> result = GetTastes(searchViewModel, ChooseStoreId);
        //        return PartialView(result);
        //    });
        //}

        private List<CustomerTastesReportViewModel> GetTastes(CustomerTastesSearchViewModel searchViewModel, Guid? ChooseStoreId)
        {
            if (searchViewModel.ToDate.HasValue)
            {
                searchViewModel.ToDate = searchViewModel.ToDate.Value.Date.AddDays(1).AddSeconds(-1);
            }
            //isViewByStore
            searchViewModel.isViewByStore = CurrentUser.isViewByStore;
            //SaleOrgCode
            if (ChooseStoreId != null)
            {
                var store = _unitOfWork.StoreRepository.GetBy(ChooseStoreId);
                if (store != null)
                {
                    searchViewModel.SaleOrgCode = store.SaleOrgCode;
                }
            }
            //StoreId
            var storeList = _unitOfWork.StoreRepository.GetStoreByPermission(CurrentUser.AccountId);
            if (storeList != null && storeList.Count > 0)
            {
                searchViewModel.StoreId = storeList.Select(p => p.StoreId).ToList();
            }
            var result = _unitOfWork.CustomerTasteRepository.GetCustomerTastesReport(searchViewModel);
            return result;
        }
        #endregion Index

        #region Export excel
        [ISDAuthorizationAttribute]
        public ActionResult Export(CustomerTastesSearchViewModel searchViewModel, Guid? ChooseStoreId = null)
        {
            try
            {
                if (searchViewModel.ToDate.HasValue)
                {
                    searchViewModel.ToDate = searchViewModel.ToDate.Value.Date.AddDays(1).AddSeconds(-1);
                }
                //isViewByStore
                searchViewModel.isViewByStore = CurrentUser.isViewByStore;
                //SaleOrgCode
                if (ChooseStoreId != null)
                {
                    var store = _unitOfWork.StoreRepository.GetBy(ChooseStoreId);
                    if (store != null)
                    {
                        searchViewModel.SaleOrgCode = store.SaleOrgCode;
                    }
                }
                //StoreId
                var storeList = _unitOfWork.StoreRepository.GetStoreByPermission(CurrentUser.AccountId);
                if (storeList != null && storeList.Count > 0)
                {
                    searchViewModel.StoreId = storeList.Select(p => p.StoreId).ToList();
                }

                List<CustomerTastesReportViewModel> lst = _unitOfWork.CustomerTasteRepository.GetCustomerTastesReport(searchViewModel);

                #region Columns
                //Mẫu excel báo cáo thị hiếu khách hàng
                //1. Mã SAP
                //2. Mã sản phẩm
                //3. Tên sản phẩm
                //4. Phân loại VT
                //5. Nhóm VT
                //6. Số lượt like
                List<ExcelTemplate> columns = new List<ExcelTemplate>();

                columns.Add(new ExcelTemplate() { ColumnName = "MaSAP", isAllowedToEdit = false, });
                columns.Add(new ExcelTemplate() { ColumnName = "MaSP", isAllowedToEdit = false, });
                columns.Add(new ExcelTemplate() { ColumnName = "TenSP", isAllowedToEdit = false, });
                columns.Add(new ExcelTemplate() { ColumnName = "PLoaiVT", isAllowedToEdit = false, });
                columns.Add(new ExcelTemplate() { ColumnName = "NhomVT", isAllowedToEdit = false, });
                columns.Add(new ExcelTemplate() { ColumnName = "SoLuotLiked", isAllowedToEdit = false, });
                #endregion Columns

                //Header
                string fileheader = "BC THỊ HIẾU KHÁCH HÀNG";
                //List<ExcelHeadingTemplate> heading initialize in BaseController
                //Default:
                //          1. heading[0] is controller code
                //          2. heading[1] is file name
                //          3. headinf[2] is warning (edit)
                heading.Add(new ExcelHeadingTemplate()
                {
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
                heading.Add(new ExcelHeadingTemplate()
                {
                    Content = string.Format("Từ ngày: {0:dd/MM/yyyy}", searchViewModel.FromDate),
                    RowsToIgnore = 0,
                    isHeadingDetail = true,
                });
                heading.Add(new ExcelHeadingTemplate()
                {
                    Content = string.Format("Đến ngày: {0:dd/MM/yyyy}", searchViewModel.ToDate),
                    RowsToIgnore = 1,
                    isHeadingDetail = true,
                });

                //Body
                byte[] filecontent = ClassExportExcel.ExportExcel(lst, columns, heading, true);
                string fileHeaderWithFormat = _unitOfWork.UtilitiesRepository.RemoveSign4VietnameseString(fileheader.ToUpper()).Replace(" ", "_");
                fileHeaderWithFormat = fileHeaderWithFormat + DateTime.Now.ToString("'_'yyyy'-'MM'-'dd'T'HH'-'mm'-'ss");
                string fileNameWithFormat = string.Format("{0}.xlsx", fileHeaderWithFormat);

                return File(filecontent, ClassExportExcel.ExcelContentType, fileNameWithFormat);
            }
            catch (Exception ex)
            {

                string Message = ex.Message;
                return Content(Message);
            }
        }
        #endregion Export excel

        #region Helper
        public void CreateViewBag()
        {
            //FromDate, ToDate
            //ViewBag.FromDate = DateTime.Now;
            //ViewBag.ToDate = DateTime.Now;

            //Dropdown Company
            var companyList = _unitOfWork.CompanyRepository.GetAll();
            ViewBag.CompanyId = new SelectList(companyList, "CompanyId", "CompanyName");

            //Dropdown Store
            var storeList = _unitOfWork.StoreRepository.GetAllStore();
            ViewBag.ChooseStoreId = new SelectList(storeList, "StoreId", "StoreName");

            //Địa điểm khách ghé
            var CustomerSourceList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CustomerSource);
            ViewBag.CustomerSourceCode = new SelectList(CustomerSourceList, "CatalogCode", "CatalogText_vi");

            //Nhóm khách hàng
            var customerGroupList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CustomerCategory);
            ViewBag.CustomerGroupCode = new SelectList(customerGroupList, "CatalogCode", "CatalogText_vi");

            //Nhân viên tiếp khách
            var _salesEmployeeRepository = new SalesEmployeeRepository(_context);
            var saleEmployeeList = _salesEmployeeRepository.GetAllForDropdownlist();
            ViewBag.SaleEmployeeCode = new SelectList(saleEmployeeList, "SalesEmployeeCode", "SalesEmployeeName");
        }
        #endregion Helper


        [ValidateInput(false)]
        public ActionResult PivotGridPartial(CustomerTastesSearchViewModel searchViewModel, Guid? ChooseStoreId = null)
        {
            //var searchView = new CustomerTastesSearchViewModel();
            //var model = _unitOfWork.CustomerTasteRepository.GetCustomerTastesReport(searchView);
            var model = GetTastes(searchViewModel, ChooseStoreId);
            return PartialView("_PivotGridPartial", model);
        }
        //public ActionResult ExportToXLSX_DataAware(CustomerTastesSearchViewModel searchViewModel, Guid? ChooseStoreId = null)
        //{
        //    //var searchView = new CustomerTastesSearchViewModel();
        //    //var model = _unitOfWork.CustomerTasteRepository.GetCustomerTastesReport(searchView);
        //    var model = GetTastes(searchViewModel, ChooseStoreId);
        //    return PivotGridExtension.ExportToXlsx(TastesPivotGridHelper.Settings, model, TastesPivotGridHelper.XlsxOptions);
        //}

        [HttpPost]
        public ActionResult ExportPivot(PivotGridExportOptions options, CustomerTastesSearchViewModel searchViewModel, Guid? ChooseStoreId = null)
        {
            var model = GetTastes(searchViewModel, ChooseStoreId);
            if (Request.Params["ExportTo"] == null)
            { // Theme changing
               // ViewBag.DemoOptions = options;
                return View("Export", model);
            }
            return TastesPivotGridDataOutputHelper.GetExportActionResult(options, model);
        }
    }
}