using ISD.Constant;
using ISD.Core;
using ISD.Extensions;
using ISD.Repositories;
using ISD.Repositories.Excel;
using ISD.Resources;
using ISD.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Reports.Controllers
{
    public class CustomerAppointmentController : BaseController
    {
        // GET: CustomerAppointment
        [ISDAuthorizationAttribute]
        public ActionResult Index()
        {
            var searchModel = (AppointmentSearchViewModel)TempData[CurrentUser.AccountId + "CustomerAppointmentSearchData"];
            if (searchModel == null)
            {
                searchModel = new AppointmentSearchViewModel();
            }
            if (searchModel.CommonDate == null)
            {
                searchModel.CommonDate = "ThisMonth";
            }
            var tempalteIdString = TempData[CurrentUser.AccountId + "CustomerAppointmentTemplateId"];
            var modeSearch = TempData[CurrentUser.AccountId + "CustomerAppointmentModeSearch"];
            var pageId = GetPageId("/Reports/CustomerAppointment");
            if (modeSearch == null || modeSearch.ToString() == "Default")
            {
                ViewBag.ModeSearch = "Default";
            }
            else
            {
                ViewBag.ModeSearch = "Recently";
            }
            Guid templateId = Guid.Empty;
            if (tempalteIdString != null)
            {
                templateId = Guid.Parse(tempalteIdString.ToString());
            }

            if (searchModel == null || searchModel.IsView != true)
            {
                ViewBag.Search = null;
            }
            else
            {
                ViewBag.Search = searchModel;            
            }
            var listSystemTemplate = _unitOfWork.PivotGridTemplateRepository.GetSystemTemplate(pageId);
            var listUserTemplate = _unitOfWork.PivotGridTemplateRepository.GetUserTemplate(pageId, CurrentUser.AccountId.Value);
            List<FieldSettingModel> pivotSetting = new List<FieldSettingModel>();
            if (templateId != Guid.Empty && templateId != null)
            {

                pivotSetting = _unitOfWork.PivotGridTemplateRepository.GetSettingByTemplate(templateId);
                ViewBag.PivotSetting = pivotSetting;
                ViewBag.TemplateId = templateId;
            }
            else
            {
                var userDefaultTemplate = listUserTemplate.FirstOrDefault(s => s.IsDefault == true);
                if (userDefaultTemplate != null)
                {
                    pivotSetting = _unitOfWork.PivotGridTemplateRepository.GetSettingByTemplate(userDefaultTemplate.SearchResultTemplateId);
                    ViewBag.PivotSetting = pivotSetting;
                    ViewBag.TemplateId = userDefaultTemplate.SearchResultTemplateId;
                }
                else
                {
                    var sysDefaultTemplate = listSystemTemplate.FirstOrDefault(s => s.IsDefault == true);
                    if (sysDefaultTemplate != null)
                    {
                        pivotSetting = _unitOfWork.PivotGridTemplateRepository.GetSettingByTemplate(sysDefaultTemplate.SearchResultTemplateId);
                        ViewBag.PivotSetting = pivotSetting;
                        ViewBag.TemplateId = sysDefaultTemplate.SearchResultTemplateId;
                    }
                    else
                    {
                        ViewBag.PivotSetting = null;
                        ViewBag.TemplateId = templateId;
                    }
                }
            }
            ViewBag.PageId = pageId;
            ViewBag.SystemTemplate = listSystemTemplate;
            ViewBag.UserTemplate = listUserTemplate;
            CreateSearchViewBag(searchModel);
            return View();
        }

        #region CreateSearchViewBag
        private void CreateSearchViewBag(AppointmentSearchViewModel searchViewModel)
        {
            //Dropdown Company
            var companyList = _unitOfWork.CompanyRepository.GetAll(CurrentUser.isViewByStore, CurrentUser.AccountId);
            ViewBag.CompanyId = new SelectList(companyList, "CompanyId", "CompanyName", searchViewModel.CompanyId);

            //Dropdown Store
            var storeList = _unitOfWork.StoreRepository.GetAllStore(CurrentUser.isViewByStore, CurrentUser.AccountId);
            ViewBag.StoreId = new SelectList(storeList, "StoreId", "StoreName", searchViewModel.StoreId);

            //Địa điểm khách ghé
            var CustomerSourceList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CustomerSource);
            ViewBag.CustomerSourceCode = new SelectList(CustomerSourceList, "CatalogCode", "CatalogText_vi");

            //Nhân viên tiếp khách
            var _salesEmployeeRepository = new SalesEmployeeRepository(_context);
            var saleEmployeeList = _salesEmployeeRepository.GetAllForDropdownlist();
            ViewBag.SalesEmployeeCode = new SelectList(saleEmployeeList, "SalesEmployeeCode", "SalesEmployeeName", searchViewModel.SalesEmployeeCode);

            //Danh mục
            var categoryList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.Appoitment_Category);
            ViewBag.CategoryCode = new SelectList(categoryList, "CatalogCode", "CatalogText_vi");

            //Phân loại khách hàng
            var customerClassList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CustomerClass);
            ViewBag.CustomerClassCode = new SelectList(customerClassList, "CatalogCode", "CatalogText_vi");

            //Get list Age (Độ tuổi)
            var ageList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.Age);
            ViewBag.Age = new SelectList(ageList, "CatalogCode", "CatalogText_vi");

            //Get list CustomerType (Tiêu dùng, Doanh nghiệp || Liên hệ)
            var catalogList = _context.CatalogModel.Where(
                p => p.CatalogTypeCode == ConstCatalogType.CustomerType
                && p.Actived == true && p.CatalogCode != ConstCustomerType.Contact).OrderBy(p => p.OrderIndex).ToList();

            ViewBag.CustomerTypeCode = new SelectList(catalogList, "CatalogCode", "CatalogText_vi");

            //Trạng thái xử lý yêu cầu
            var WorkFlowId = _unitOfWork.WorkFlowRepository.FindWorkFlowIdByCode(ConstWorkFlow.GT);
            var taskStatusList = _unitOfWork.TaskStatusRepository.GetTaskStatusByWorkFlow(WorkFlowId);
            ViewBag.TaskStatusId = new SelectList(taskStatusList, "TaskStatusId", "TaskStatusName");

            //CommonDate
            var commonDateList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CommonDate);
            ViewBag.CommonDate = new SelectList(commonDateList, "CatalogCode", "CatalogText_vi", searchViewModel.CommonDate);
            ViewBag.CommonCreateDate = new SelectList(commonDateList, "CatalogCode", "CatalogText_vi", searchViewModel.CommonCreateDate);
            //ViewBag.CommonReceiveDate = new SelectList(commonDateList, "CatalogCode", "CatalogText_vi", searchViewModel.CommonReceiveDate);
            //ViewBag.CommonEndDate = new SelectList(commonDateList, "CatalogCode", "CatalogText_vi", searchViewModel.CommonEndDate);

            //Nhóm khách hàng
            var customerGroupList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CustomerCategory);
            ViewBag.CustomerGroupCode = new SelectList(customerGroupList, "CatalogCode", "CatalogText_vi");

            //Ngành nghề
            var customerCareerList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CustomerCareer);
            ViewBag.CustomerCareerCode = new SelectList(customerCareerList, "CatalogCode", "CatalogText_vi");

            //Ngày nhập thông tin

            //Bắc trung Nam
            var SaleOfficeList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.SaleOffice);
            ViewBag.SaleOfficeCode = new SelectList(SaleOfficeList, "CatalogCode", "CatalogText_vi");

            var filterList = new List<DropdownlistFilter>();
            filterList.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.SaleOfficeCode, FilterName = LanguageResource.Profile_SaleOfficeCode });
            filterList.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.CustomerTypeCode, FilterName = LanguageResource.Profile_CustomerTypeCode });
            //filterList.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.CustomerSourceCode, FilterName = LanguageResource.Profile_CustomerSourceCode });
            // filterList.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.StoreId, FilterName = LanguageResource.MasterData_Store });
            //filterList.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.SalesEmployeeCode, FilterName = LanguageResource.Appointment_SaleEmployeeCode });
            filterList.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.Age, FilterName = LanguageResource.Profile_Age });
            filterList.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.TaskStatusId, FilterName = LanguageResource.TaskStatus });
            filterList.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.Phone, FilterName = LanguageResource.Profile_Phone });
            filterList.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.TaxNo, FilterName = LanguageResource.Profile_TaxNo });
            filterList.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.CustomerGroupCode, FilterName = LanguageResource.Profile_CustomerCategoryCode });
            filterList.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.CustomerCareerCode, FilterName = LanguageResource.Profile_CustomerCareerCode });
            // filterList.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.Create, FilterName = LanguageResource.CreatedDate });
            ViewBag.Filters = filterList;
        }
        #endregion


        //Export
        #region Export to excel
        //const string controllerCode = ConstExcelController.Appointment;
        const int startIndex = 8;

        public ActionResult ExportExcel(AppointmentSearchViewModel searchViewModel)
        {
            if (searchViewModel.CommonDate != "Custom")
            {
                DateTime? fromDate;
                DateTime? toDate;
                DateTime? fromPreviousDay;
                DateTime? toPreviousDay;


                _unitOfWork.CommonDateRepository.GetDateBy(searchViewModel.CommonDate, out fromDate, out toDate, out fromPreviousDay, out toPreviousDay);
                //Tìm kiếm kỳ hiện tại
                searchViewModel.FromDate = fromDate;
                searchViewModel.ToDate = toDate;
            }
            if (searchViewModel.CommonCreateDate != "Custom")
            {
                DateTime? fromDate;
                DateTime? toDate;
                DateTime? fromPreviousDay;
                DateTime? toPreviousDay;


                _unitOfWork.CommonDateRepository.GetDateBy(searchViewModel.CommonCreateDate, out fromDate, out toDate, out fromPreviousDay, out toPreviousDay);
                //Tìm kiếm kỳ hiện tại
                searchViewModel.CreateFromDate = fromDate;
                searchViewModel.CreateToDate = toDate;
            }
            //get search value to display in file
            var filterDisplayList = GetSearchInfoToDisplay(searchViewModel);
            //Get data filter
            //Get data from server
            int resultCount = 0;
            var appointments = _unitOfWork.AppointmentRepository.QueryReportAppointment(searchViewModel, CurrentUser.CompanyCode, out resultCount);

            if (appointments != null && appointments.Count > 0)
            {
                foreach (var item in appointments)
                {
                    var contact = _unitOfWork.AppointmentRepository.GetMainContact(item.ProfileId,CurrentUser.CompanyCode);
                    item.ContactName = contact.MainContactName;
                    item.ContactPhone = contact.MainContactPhone;
                    item.ContactEmail = contact.MainContactEmail;
                    item.SalesSupervisorName = contact.SalesSupervisorName;
                    item.DepartmentName = contact.DepartmentName;
                }
            }
            return Export(appointments, filterDisplayList);
        }

        [ISDAuthorizationAttribute]
        public FileContentResult Export(List<AppointmentReportViewModel> viewModel, List<SearchDisplayModel> filters)
        {
            #region Master
            List<ExcelTemplate> columns = new List<ExcelTemplate>();

            columns.Add(new ExcelTemplate { ColumnName = "VisitDate", isAllowedToEdit = false, isDateTime = true });//1. Ngày ghé thăm
            columns.Add(new ExcelTemplate { ColumnName = "Summary", isAllowedToEdit = false });//2. Tiêu đề
            columns.Add(new ExcelTemplate { ColumnName = "ProfileCode", isAllowedToEdit = false });//3. Mã khách
            columns.Add(new ExcelTemplate { ColumnName = "ProfileName", isAllowedToEdit = false });//4. Tên Khách
            columns.Add(new ExcelTemplate { ColumnName = "Address", isAllowedToEdit = false });//5. Địa chỉ
            columns.Add(new ExcelTemplate { ColumnName = "Phone", isAllowedToEdit = false });//6. SDT liên hệ
            columns.Add(new ExcelTemplate { ColumnName = "Email", isAllowedToEdit = false });//7. Email
            columns.Add(new ExcelTemplate { ColumnName = "AgeText", isAllowedToEdit = false });//8. Độ tuổi KH
            columns.Add(new ExcelTemplate { ColumnName = "CustomerClassName", isAllowedToEdit = false });//9. Phân loại KH (old/new)
            columns.Add(new ExcelTemplate { ColumnName = "ForeignCustomerName", isAllowedToEdit = false });//10. Đối tượng
            columns.Add(new ExcelTemplate { ColumnName = "ShowroomName", isAllowedToEdit = false });//11. Nguồn KH
            columns.Add(new ExcelTemplate { ColumnName = "StoreName", isAllowedToEdit = false });//12. Chi Nhánh
            columns.Add(new ExcelTemplate { ColumnName = "CustomerTypeName", isAllowedToEdit = false });//13. Phân loại KH (B|C)
            columns.Add(new ExcelTemplate { ColumnName = "CustomerGroupName", isAllowedToEdit = false });//14. Nhóm KH
            columns.Add(new ExcelTemplate { ColumnName = "ContactName", isAllowedToEdit = false });//15. Liên hệ chính
            columns.Add(new ExcelTemplate { ColumnName = "ContactPhone", isAllowedToEdit = false });//16. SDT liên hệ
            columns.Add(new ExcelTemplate { ColumnName = "ContactEmail", isAllowedToEdit = false });//17. Email liên hệ
            columns.Add(new ExcelTemplate { ColumnName = "customerTasteLst", isAllowedToEdit = false });//18. Thị hiếu KH
            columns.Add(new ExcelTemplate { ColumnName = "customerCatalogueLst", isAllowedToEdit = false });//19. CTL đã xuất
            columns.Add(new ExcelTemplate { ColumnName = "ChannelName", isAllowedToEdit = false });//20. Khách biết đến AC qua
            columns.Add(new ExcelTemplate { ColumnName = "Requirement", isAllowedToEdit = false });//21. Yêu cầu
            columns.Add(new ExcelTemplate { ColumnName = "Description", isAllowedToEdit = false });//22. Ghi chú	
            columns.Add(new ExcelTemplate { ColumnName = "SalesSupervisorName", isAllowedToEdit = false });//23. NV kinh doanh
            columns.Add(new ExcelTemplate { ColumnName = "SaleEmployeeName", isAllowedToEdit = false });//24. NV tiếp khách
            columns.Add(new ExcelTemplate { ColumnName = "DepartmentName", isAllowedToEdit = false });//25. Phòng ban
            columns.Add(new ExcelTemplate { ColumnName = "CreateTime", isAllowedToEdit = false, isDateTimeTime = true });//26. Ngày tạo

            #endregion Master

            //Header
            string fileheader = "BÁO CÁO TỔNG HỢP KHÁCH GHÉ THĂM";
            //List<ExcelHeadingTemplate> heading initialize in BaseController
            //Default:
            //          1. heading[0] is controller code
            //          2. heading[1] is file name
            //          3. headinf[2] is warning (edit)
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
            #region search info
            foreach (var search in filters)
            {
                heading.Add(new ExcelHeadingTemplate()
                {
                    Content = search.DisplayName + ": " + search.DisplayValue,
                    RowsToIgnore = 0,
                    isWarning = false,
                    isCode = true
                });
            }
            #endregion

            //Body
            byte[] filecontent = ClassExportExcel.ExportExcel(viewModel, columns, heading, true, HasExtraSheet: false);
            string fileNameWithFormat = string.Format("{0}.xlsx", _unitOfWork.UtilitiesRepository.RemoveSign4VietnameseString(fileheader.ToUpper()).Replace(" ", "_"));

            return File(filecontent, ClassExportExcel.ExcelContentType, fileNameWithFormat);
        }
        #endregion Export to excel

        private List<AppointmentReportViewModel> GetData(AppointmentSearchViewModel searchViewModel)
        {
            if (searchViewModel.CommonDate != "Custom")
            {
                DateTime? fromDate;
                DateTime? toDate;
                DateTime? fromPreviousDay;
                DateTime? toPreviousDay;


                _unitOfWork.CommonDateRepository.GetDateBy(searchViewModel.CommonDate, out fromDate, out toDate, out fromPreviousDay, out toPreviousDay);
                //Tìm kiếm kỳ hiện tại
                searchViewModel.FromDate = fromDate;
                searchViewModel.ToDate = toDate;
            }
            //Get data filter
            //Get data from server
            int resultCount = 0;
            var data = _unitOfWork.AppointmentRepository.QueryReportAppointment(searchViewModel, CurrentUser.CompanyCode, out resultCount);

            if (data != null && data.Count > 0)
            {
                foreach (var item in data)
                {
                    var contact = _unitOfWork.AppointmentRepository.GetMainContact(item.ProfileId, CurrentUser.CompanyCode);
                    item.ContactName = contact.MainContactName;
                    item.ContactPhone = contact.MainContactPhone;
                    item.ContactEmail = contact.MainContactEmail;
                    item.SalesSupervisorName = contact.SalesSupervisorName;
                    item.DepartmentName = contact.DepartmentName;
                }
            }
            return data;
        }
        public ActionResult ViewDetail(AppointmentSearchViewModel searchModel, Guid pivotTemplate, string modeSearch)
        {
            TempData[CurrentUser.AccountId + "CustomerAppointmentSearchData"] = searchModel;
            TempData[CurrentUser.AccountId + "CustomerAppointmentTemplateId"] = pivotTemplate;
            TempData[CurrentUser.AccountId + "CustomerAppointmentModeSearch"] = modeSearch;
            return RedirectToAction("Index");
        }

        public ActionResult ChangeTemplate(Guid pivotTemplate, AppointmentSearchViewModel searchModel, string modeSearch)
        {
            TempData[CurrentUser.AccountId + "CustomerAppointmentSearchData"] = searchModel;
            TempData[CurrentUser.AccountId + "CustomerAppointmentTemplateId"] = pivotTemplate;
            TempData[CurrentUser.AccountId + "CustomerAppointmentModeSearch"] = modeSearch;
            return RedirectToAction("Index");
        }


        [ValidateInput(false)]
        public ActionResult CustomerAppointmentGridPartial(Guid? templateId = null, AppointmentSearchViewModel searchViewModel = null, string jsonReq = null)
        {
            var pageId = GetPageId("/Reports/CustomerAppointment");
            var listSystemTemplate = _unitOfWork.PivotGridTemplateRepository.GetSystemTemplate(pageId);
            var listUserTemplate = _unitOfWork.PivotGridTemplateRepository.GetUserTemplate(pageId, CurrentUser.AccountId.Value);
            List<FieldSettingModel> pivotSetting = new List<FieldSettingModel>();
            if (templateId != Guid.Empty && templateId != null)
            {

                pivotSetting = _unitOfWork.PivotGridTemplateRepository.GetSettingByTemplate(templateId.Value);
                ViewBag.PivotSetting = pivotSetting;
                ViewBag.TemplateId = templateId;

                var Template = _unitOfWork.PivotGridTemplateRepository.GetTemplateById(templateId.Value);
                if (Template != null)
                {
                    ViewBag.LayoutConfigs = Template.LayoutConfigs;
                }
            }
            else
            {
                var userDefaultTemplate = listUserTemplate.FirstOrDefault(s => s.IsDefault == true);
                if (userDefaultTemplate != null)
                {
                    pivotSetting = _unitOfWork.PivotGridTemplateRepository.GetSettingByTemplate(userDefaultTemplate.SearchResultTemplateId);
                    ViewBag.PivotSetting = pivotSetting;
                    ViewBag.TemplateId = userDefaultTemplate.SearchResultTemplateId;
                }
                else
                {
                    var sysDefaultTemplate = listSystemTemplate.FirstOrDefault(s => s.IsDefault == true);
                    if (sysDefaultTemplate != null)
                    {
                        pivotSetting = _unitOfWork.PivotGridTemplateRepository.GetSettingByTemplate(sysDefaultTemplate.SearchResultTemplateId);
                        ViewBag.PivotSetting = pivotSetting;
                        ViewBag.TemplateId = sysDefaultTemplate.SearchResultTemplateId;
                    }
                    else
                    {
                        ViewBag.PivotSetting = null;
                        ViewBag.TemplateId = templateId;
                    }
                }
            }

            if ((string.IsNullOrEmpty(jsonReq) || jsonReq == "null") && (searchViewModel == null || searchViewModel.IsView != true))
            {
                ViewBag.Search = null;
                return PartialView("_CustomerAppointmentPivotGridPartial", null);
            }
            else
            {
                if (!string.IsNullOrEmpty(jsonReq))
                {
                    searchViewModel = JsonConvert.DeserializeObject<AppointmentSearchViewModel>(jsonReq);
                }
                var model = GetData(searchViewModel);
                ViewBag.Search = searchViewModel;
                return PartialView("_CustomerAppointmentPivotGridPartial", model);
            }
        }
        [HttpPost]
        public ActionResult ExportPivot(AppointmentSearchViewModel searchViewModel, Guid? templateId)
        {
            PivotGridExportOptions options = new PivotGridExportOptions();
            options.ExportType = PivotGridExportFormats.Excel;
            options.WYSIWYG.PrintRowHeaders = true;
            options.WYSIWYG.PrintFilterHeaders = false;
            options.WYSIWYG.PrintDataHeaders = false;
            options.WYSIWYG.PrintColumnHeaders = false;
            var model = GetData(searchViewModel);
            //get pivot config 
            //Lấy cột - thứ tự cột ... từ bảng SearchResultDetailTemplateModel
            var pivotSetting = _unitOfWork.PivotGridTemplateRepository.GetSettingByTemplate(templateId.Value).ToList();
            //Lấy thông tin config các thông số người dùng lưu template từ SearchResultTemplateModel.LayoutConfigs
            var template = _unitOfWork.PivotGridTemplateRepository.GetTemplateById(templateId.Value);
            var layoutConfigs = (string)Session[CurrentUser.AccountId + "LayoutConfigs"];
            var headerText = string.Empty;

            if (template != null)
            {
                headerText = template.TemplateName;
            }
            string fileName = "BAO_CAO_KHACH_GHE_THAM";
            //get search value to display in file
            var filterDisplayList = GetSearchInfoToDisplay(searchViewModel);
            return PivotGridExportExcel.GetExportActionResult(fileName, options, pivotSetting, model, filterDisplayList, headerText, layoutConfigs);
        }

        #region Lấy điều kiện lọc
        private List<SearchDisplayModel> GetSearchInfoToDisplay(AppointmentSearchViewModel searchViewModel)
        {
            List<SearchDisplayModel> filterList = new List<SearchDisplayModel>();
            if (searchViewModel.FromDate != null)
            {
                filterList.Add(new SearchDisplayModel() { DisplayName = "Ghé thăm Từ ngày", DisplayValue = searchViewModel.FromDate.Value.ToString("dd-MM-yyyy") });
            }
            if (searchViewModel.ToDate != null)
            {
                filterList.Add(new SearchDisplayModel() { DisplayName = "Ghé thăm Đến ngày", DisplayValue = searchViewModel.ToDate.Value.ToString("dd-MM-yyyy") });
            }
            if (searchViewModel.CreateFromDate != null)
            {
                filterList.Add(new SearchDisplayModel() { DisplayName = "Tạo từ ngày", DisplayValue = searchViewModel.CreateFromDate.Value.ToString("dd-MM-yyyy") });
            }
            if (searchViewModel.CreateToDate != null)
            {
                filterList.Add(new SearchDisplayModel() { DisplayName = "Tạo đến ngày", DisplayValue = searchViewModel.CreateToDate.Value.ToString("dd-MM-yyyy") });
            }
            if (searchViewModel.ProfileId != null && searchViewModel.ProfileId != Guid.Empty)
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Khách hàng";
                var value = _context.ProfileModel.FirstOrDefault(s => s.ProfileId == searchViewModel.ProfileId).ProfileName;
                filter.DisplayValue = value;
                filterList.Add(filter);
            }
            if (searchViewModel.CompanyId != null && searchViewModel.CompanyId != Guid.Empty)
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Công ty";
                var value = _context.CompanyModel.FirstOrDefault(s => s.CompanyId == searchViewModel.CompanyId).CompanyName;
                filter.DisplayValue = value;
                filterList.Add(filter);
            }
            if (searchViewModel.StoreId != null && searchViewModel.StoreId.Count > 0 && searchViewModel.StoreId[0] != Guid.Empty)
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Chi nhánh";
                foreach (var id in searchViewModel.StoreId)
                {
                    var store = _context.StoreModel.FirstOrDefault(s => s.StoreId == id);
                    filter.DisplayValue += store.SaleOrgCode + " | " + store.StoreName;
                    filter.DisplayValue += ", ";
                }
                filterList.Add(filter);
            }
            if (!string.IsNullOrEmpty(searchViewModel.SalesEmployeeCode))
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Nhân viên tiếp khách";
                var sale = _context.SalesEmployeeModel.FirstOrDefault(s => s.SalesEmployeeCode == searchViewModel.SalesEmployeeCode);
                var value = sale.SalesEmployeeCode + " | " + sale.SalesEmployeeName;
                filter.DisplayValue = value;
                filterList.Add(filter);
            }
            if (searchViewModel.CustomerSourceCode != null && searchViewModel.CustomerSourceCode.Count > 0)
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Nguồn khách hàng";
                foreach (var id in searchViewModel.CustomerSourceCode)
                {
                    var model = _context.CatalogModel.FirstOrDefault(s => s.CatalogCode == id && s.CatalogTypeCode == ConstCatalogType.CustomerSource).CatalogText_vi;
                    filter.DisplayValue += model;
                    filter.DisplayValue += ", ";
                }
                filterList.Add(filter);
            }
            if (searchViewModel.CustomerGroupCode != null && searchViewModel.CustomerGroupCode.Count() > 0)
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Nhóm khách hàng";
                var value = _context.CatalogModel.Where(s => searchViewModel.CustomerGroupCode.Contains(s.CatalogCode) && s.CatalogTypeCode == ConstCatalogType.CustomerGroup).Select(x=>x.CatalogText_vi);
                filter.DisplayValue = string.Join(", ",value);
                filterList.Add(filter);
            }
            if (searchViewModel.SaleOfficeCode != null && searchViewModel.SaleOfficeCode.Count() > 0)
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Khu vực";
                var value = _context.CatalogModel.Where(s => searchViewModel.SaleOfficeCode.Contains(s.CatalogCode) && s.CatalogTypeCode == ConstCatalogType.SaleOffice).Select(x => x.CatalogText_vi);
                filter.DisplayValue = string.Join(", ", value);
                filterList.Add(filter);
            }
            if (!string.IsNullOrEmpty(searchViewModel.CustomerTypeCode))
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Phân loại khách hàng";
                var value = _context.CatalogModel.FirstOrDefault(s => s.CatalogCode == searchViewModel.CustomerTypeCode).CatalogText_vi;
                filter.DisplayValue = value;
                filterList.Add(filter);
            }
            if (!string.IsNullOrEmpty(searchViewModel.Age))
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Độ tuổi";
                var value = _context.CatalogModel.FirstOrDefault(s => s.CatalogCode == searchViewModel.Age && s.CatalogTypeCode == ConstCatalogType.Age).CatalogText_vi;
                filter.DisplayValue = value;
                filterList.Add(filter);
            }
            if (searchViewModel.TaskStatusId != null && searchViewModel.TaskStatusId != Guid.Empty)
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Trạng thái";
                var WorkFlowId = _unitOfWork.WorkFlowRepository.FindWorkFlowIdByCode(ConstWorkFlow.GT);
                var taskStatusList = _unitOfWork.TaskStatusRepository.GetTaskStatusByWorkFlow(WorkFlowId);
                var value = taskStatusList.FirstOrDefault(s => s.TaskStatusId == searchViewModel.TaskStatusId).TaskStatusName;
                filter.DisplayValue = value;
                filterList.Add(filter);
            }
            if (!string.IsNullOrEmpty(searchViewModel.Phone))
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "SĐT liên hệ";

                filter.DisplayValue = searchViewModel.Phone;
                filterList.Add(filter);
            }
            if (!string.IsNullOrEmpty(searchViewModel.TaxNo))
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Mã số thuế";

                filter.DisplayValue = searchViewModel.TaxNo;
                filterList.Add(filter);
            }
            if (!string.IsNullOrEmpty(searchViewModel.CustomerCareerCode))
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Ngành nghề";
                var value = _context.CatalogModel.FirstOrDefault(s => s.CatalogCode == searchViewModel.CustomerCareerCode && s.CatalogTypeCode == ConstCatalogType.CustomerCareer).CatalogText_vi;
                filter.DisplayValue = value;
                filterList.Add(filter);
            }
            return filterList;
        }
        #endregion
    }
}