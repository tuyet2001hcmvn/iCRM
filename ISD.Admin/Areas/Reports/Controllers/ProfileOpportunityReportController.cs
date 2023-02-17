using ISD.Constant;
using ISD.Core;
using ISD.Extensions;
using ISD.Repositories;
using ISD.Repositories.Excel;
using ISD.Resources;
using ISD.ViewModels;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace Reports.Controllers
{
    public class ProfileOpportunityReportController : BaseController
    {
        // GET: ProfileOpportunityReport
        #region Index
        [ISDAuthorizationAttribute]
        public ActionResult Index()
        {
            ViewBag.Title = LanguageResource.Reports_ProfileOpportunityReport;
            var parameter = "?Type=" + ConstProfileType.Opportunity;
            ViewBag.PageId = GetPageId("/Reports/ProfileReport", parameter);

            #region CommonDate
            var SelectedCommonDate = "Custom";
            //Common Date
            var commonDateList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CommonDate);
            ViewBag.CommonDate = new SelectList(commonDateList, "CatalogCode", "CatalogText_vi", SelectedCommonDate);
            #endregion

            CreateViewBagSearch(ProfileType: ConstProfileType.Opportunity);
            return View();
        }

        [HttpPost]
        public ActionResult _PaggingServerSide(DatatableViewModel model, ProfileSearchViewModel searchViewModel)
        {
            return ExecuteSearch(() =>
            {
                int filteredResultsCount;
                //10
                int totalResultsCount = model.length;
                //Page Size 
                searchViewModel.PageSize = model.length;
                //Page Number
                searchViewModel.PageNumber = model.start / model.length + 1;

                searchViewModel.ProfileForeignCode = searchViewModel.SearchProfileForeignCode;
                #region //Create Date
                if (searchViewModel.CreateCommonDate != "Custom")
                {
                    DateTime? fromDate;
                    DateTime? toDate;
                    _unitOfWork.CommonDateRepository.GetDateBy(searchViewModel.CreateCommonDate, out fromDate, out toDate);
                    //Tìm kiếm kỳ hiện tại
                    searchViewModel.CreateFromDate = fromDate;
                    searchViewModel.CreateToDate = toDate;
                }
                #endregion

                //Trạng thái
                searchViewModel.Actived = true;
                searchViewModel.isUsedStoredDynamic = true;

                ProfileRepository repo = new ProfileRepository(_context);
                var profiles = repo.SearchQueryProfile(searchViewModel, CurrentUser.AccountId, CurrentUser.CompanyCode, out filteredResultsCount);
                if (profiles != null && profiles.Count() > 0)
                {
                    int i = 0;
                    foreach (var item in profiles)
                    {
                        i++;
                        item.STT = i;
                        if (!string.IsNullOrEmpty(item.Address) && item.Address.StartsWith(","))
                        {
                            item.Address = item.Address.Remove(0, 1).Trim();
                        }
                        //Lấy thông tin NV phụ trách
                        var employeeList = _context.PersonInChargeModel.Where(p => p.ProfileId == item.ProfileId).ToList();
                        if (employeeList != null && employeeList.Count > 0)
                        {
                            //1. NV kinh doanh
                            var sales = employeeList.Where(p => p.SalesEmployeeType == ConstSalesEmployeeType.NVKD).Select(p => p.SalesEmployeeCode).ToList();
                            if (sales != null && sales.Count > 0)
                            {
                                var salesName = _context.SalesEmployeeModel.Where(p => sales.Contains(p.SalesEmployeeCode)).Select(p => p.SalesEmployeeName).ToList();
                                item.PersonInChargeSales = string.Join(", ", salesName);
                            }
                            //2. NV sales admin
                            var salesAdmin = employeeList.Where(p => p.SalesEmployeeType == ConstSalesEmployeeType.NVSalesAdmin).Select(p => p.SalesEmployeeCode).ToList();
                            if (salesAdmin != null && salesAdmin.Count > 0)
                            {
                                var salesAdminName = _context.SalesEmployeeModel.Where(p => salesAdmin.Contains(p.SalesEmployeeCode)).Select(p => p.SalesEmployeeName).ToList();
                                item.PersonInChargeSalesAdmin = string.Join(", ", salesAdminName);
                            }
                            //2. NV spec
                            var salesSpec = employeeList.Where(p => p.SalesEmployeeType == ConstSalesEmployeeType.NVSpec).Select(p => p.SalesEmployeeCode).ToList();
                            if (salesSpec != null && salesSpec.Count > 0)
                            {
                                var salesSpecName = _context.SalesEmployeeModel.Where(p => salesSpec.Contains(p.SalesEmployeeCode)).Select(p => p.SalesEmployeeName).ToList();
                                item.PersonInChargeSpec = string.Join(", ", salesSpecName);
                            }
                        }
                    }
                }
                return Json(new
                {
                    draw = model.draw,
                    recordsTotal = totalResultsCount,
                    recordsFiltered = filteredResultsCount,
                    data = profiles
                });
            });
        }
        #endregion

        #region CreateViewBag
        public void CreateViewBagSearch(string ProfileType = null)
        {
            var _catalogRepository = new CatalogRepository(_context);

            ViewBag.Type = ProfileType;

            #region //Company and Store
            var companyList = _unitOfWork.CompanyRepository.GetAll(CurrentUser.isViewByStore, CurrentUser.AccountId);
            ViewBag.CompanyId = new SelectList(companyList, "CompanyId", "CompanyName");

            var storeList = _unitOfWork.StoreRepository.GetAllStore(CurrentUser.isViewByStore, CurrentUser.AccountId);
            ViewBag.StoreId = new SelectList(storeList, "StoreId", "StoreName");
            #endregion

            #region //Get list SalesEmployee (NV phụ trách)
            var empList = _unitOfWork.PersonInChargeRepository.GetListEmployee();
            ViewBag.SalesEmployeeCode = new SelectList(empList, "SalesEmployeeCode", "SalesEmployeeName");

            ViewBag.EmployeeList1 = _unitOfWork.PersonInChargeRepository.GetListEmployeeByRoles(ConstSalesEmployeeType.NVKD_Code);
            ViewBag.EmployeeList2 = _unitOfWork.PersonInChargeRepository.GetListEmployeeByRoles(ConstSalesEmployeeType.NVSalesAdmin_Code);
            ViewBag.EmployeeList3 = _unitOfWork.PersonInChargeRepository.GetListEmployeeByRoles(ConstSalesEmployeeType.NVSpec_Code);
            #endregion

            #region //Get list Roles (Phòng ban)
            var rolesList = _context.RolesModel.Where(p => p.Actived == true && p.isEmployeeGroup == true).ToList();
            ViewBag.RolesCode = new SelectList(rolesList, "RolesCode", "RolesName");
            #endregion

            #region //Get list SaleOffice (Khu vực)
            var saleOfficeList = _catalogRepository.GetSaleOffice(false).Where(p => p.CatalogCode != "9999").ToList();
            ViewBag.SaleOfficeCode = new SelectList(saleOfficeList, "CatalogCode", "CatalogText_vi");
            #endregion

            #region Filters
            var filterLst = new List<DropdownlistFilter>();
            filterLst.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.Address, FilterName = LanguageResource.ProjectLocation });
            filterLst.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.SalesEmployeeCode, FilterName = LanguageResource.PersonInCharge });
            filterLst.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.RolesCode, FilterName = LanguageResource.Profile_Department });
            filterLst.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.Create, FilterName = LanguageResource.CommonEditDate });
            filterLst.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.SaleOfficeCode, FilterName = LanguageResource.Profile_SaleOfficeCode });
            ViewBag.Filters = filterLst;
            #endregion
        }
        #endregion

        //Export
        #region Export to excel
        const int startIndex = 8;

        public ActionResult ExportExcel(ProfileSearchViewModel searchViewModel)
        {
            var data = new List<ProfileOpportunityReportViewModel>();
            #region //Create Date
            if (searchViewModel.CreateCommonDate != "Custom")
            {
                DateTime? fromDate;
                DateTime? toDate;
                _unitOfWork.CommonDateRepository.GetDateBy(searchViewModel.CreateCommonDate, out fromDate, out toDate);
                //Tìm kiếm kỳ hiện tại
                searchViewModel.CreateFromDate = fromDate;
                searchViewModel.CreateToDate = toDate;
            }
            #endregion

            ProfileRepository repo = new ProfileRepository(_context);

            data = repo.SearchQueryProfileOpportunityExport(searchViewModel, CurrentUser.AccountId, CurrentUser.CompanyCode);

            return Export(data.OrderByDescending(p => p.LastEditTime).ToList(), searchViewModel.Type);
        }

        [ISDAuthorizationAttribute]
        public FileContentResult Export(List<ProfileOpportunityReportViewModel> viewModel, string Type)
        {
            List<ExcelTemplate> columns = new List<ExcelTemplate>();
            //Header
            string fileheader = string.Empty;

            fileheader = "DANH SÁCH DỰ ÁN";
            #region Columns
            columns.Add(new ExcelTemplate { ColumnName = "PersonInCharge", isAllowedToEdit = false, isWraptext = true, CustomWidth = 14 });
            //columns.Add(new ExcelTemplate { ColumnName = "SalesAdmin", isAllowedToEdit = false, isWraptext = true, CustomWidth = 14 });
            //columns.Add(new ExcelTemplate { ColumnName = "PersonSpec", isAllowedToEdit = false, isWraptext = true, CustomWidth = 14 });
            //columns.Add(new ExcelTemplate { ColumnName = "LastEditTime", isAllowedToEdit = false, isDateTime = true, });
            columns.Add(new ExcelTemplate { ColumnName = "ProfileName", isAllowedToEdit = false, CustomWidth = 14, isWraptext = true, });
            columns.Add(new ExcelTemplate { ColumnName = "HandoverFurniture", isWraptext = true, CustomWidth = 14, isAllowedToEdit = false, });
            columns.Add(new ExcelTemplate { ColumnName = "ProvinceName", isAllowedToEdit = false, CustomWidth = 14, isWraptext = true, });
            //columns.Add(new ExcelTemplate { ColumnName = "SaleOfficeName", isAllowedToEdit = false, });
            columns.Add(new ExcelTemplate { ColumnName = "OpportunityType", isAllowedToEdit = false, CustomWidth = 14, isWraptext = true, });
            columns.Add(new ExcelTemplate { ColumnName = "ProjectGabarit", isAllowedToEdit = false, isNumber = true , CustomWidth = 14, });
            //columns.Add(new ExcelTemplate { ColumnName = "OpportunityUnit", isAllowedToEdit = false, });
            columns.Add(new ExcelTemplate { ColumnName = "Investor", isAllowedToEdit = false, isWraptext = true, CustomWidth = 14 });
            columns.Add(new ExcelTemplate { ColumnName = "ConsultingAndDesign", isAllowedToEdit = false, isWraptext = true, CustomWidth = 14 });
            //columns.Add(new ExcelTemplate { ColumnName = "GeneralContractor", isAllowedToEdit = false, isWraptext = true, CustomWidth = 30 });
            //columns.Add(new ExcelTemplate { ColumnName = "Internal", isAllowedToEdit = false, isWraptext = true, CustomWidth = 60, MergeHeaderTitle = "Thi công" });
            //columns.Add(new ExcelTemplate { ColumnName = "Competitor", isAllowedToEdit = false, isWraptext = true, CustomWidth = 60, MergeHeaderTitle = "Thi công" });

            //columns.Add(new ExcelTemplate { ColumnName = "Laminate", isAllowedToEdit = false, isWraptext = true, CustomWidth = 20, MergeHeaderTitle = "Spec" });
            //columns.Add(new ExcelTemplate { ColumnName = "MFC", isAllowedToEdit = false, isWraptext = true, CustomWidth = 20, MergeHeaderTitle = "Spec" });
            //columns.Add(new ExcelTemplate { ColumnName = "Flooring", isAllowedToEdit = false, isWraptext = true, CustomWidth = 20, MergeHeaderTitle = "Spec" });
            //columns.Add(new ExcelTemplate { ColumnName = "Accessories", isAllowedToEdit = false, isWraptext = true, CustomWidth = 20, MergeHeaderTitle = "Spec" });
            //columns.Add(new ExcelTemplate { ColumnName = "KitchenEquipment", isAllowedToEdit = false, isWraptext = true, CustomWidth = 20, MergeHeaderTitle = "Spec" });
            //columns.Add(new ExcelTemplate { ColumnName = "Veneer", isAllowedToEdit = false, isWraptext = true, CustomWidth = 20, MergeHeaderTitle = "Spec" });

            //Danh sách ngành hàng 
            //spec
            var specLst = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.Opportunity_Industry).Where(p => p.CatalogText_en.Contains(ConstCatalogType.Spec)).ToList();
            if (specLst != null && specLst.Count > 0)
            {
                foreach (var element in specLst)
                {
                    columns.Add(new ExcelTemplate()
                    {
                        ColumnName = element.CatalogCode.Replace("OpportunityIndustryContent", "OpportunityIndustrySpecContent"),
                        isAllowedToEdit = false,
                        isWraptext = true,
                        CustomWidth = 14,
                        MergeHeaderTitle = "Spec",
                        hasAnotherName = true,
                        AnotherName = element.CatalogText_vi,
                    });
                }
            }
            //thi công
            var constructionLst = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.Opportunity_Industry).Where(p => p.CatalogText_en.Contains(ConstCatalogType.Construction)).ToList();
            if (constructionLst != null && constructionLst.Count > 0)
            {
                foreach (var element in constructionLst)
                {
                    columns.Add(new ExcelTemplate()
                    {
                        ColumnName = element.CatalogCode.Replace("OpportunityIndustryContent", "OpportunityIndustryConstructionContent"),
                        isAllowedToEdit = false,
                        isWraptext = true,
                        CustomWidth = 10,
                        MergeHeaderTitle = "Thi công",
                        hasAnotherName = true,
                        AnotherName = element.CatalogText_vi,
                        isBoolean = true,
                    });
                }
            }

            //columns.Add(new ExcelTemplate { ColumnName = "Text6", isAllowedToEdit = false, isWraptext = true, CustomWidth = 20, MergeHeaderTitle = "Thi công" });
            //columns.Add(new ExcelTemplate { ColumnName = "Text7", isAllowedToEdit = false, isWraptext = true, CustomWidth = 20, MergeHeaderTitle = "Thi công" });
            //columns.Add(new ExcelTemplate { ColumnName = "Text8", isAllowedToEdit = false, isWraptext = true, CustomWidth = 20, MergeHeaderTitle = "Thi công" });
            //columns.Add(new ExcelTemplate { ColumnName = "Text9", isAllowedToEdit = false, isWraptext = true, CustomWidth = 20, MergeHeaderTitle = "Thi công" });

            columns.Add(new ExcelTemplate { ColumnName = "OtherBrand", isWraptext = true, CustomWidth = 14, isAllowedToEdit = false, });
            columns.Add(new ExcelTemplate { ColumnName = "OppCompetitor", isWraptext = true, CustomWidth = 15, isAllowedToEdit = false, });
            columns.Add(new ExcelTemplate { ColumnName = "ProjectStatus", isWraptext = true, CustomWidth = 15, isAllowedToEdit = false, });
            columns.Add(new ExcelTemplate { ColumnName = "OpportunityPercentage", isWraptext = true, CustomWidth = 29, isAllowedToEdit = false, });
            columns.Add(new ExcelTemplate { ColumnName = "ProjectComplete", isAllowedToEdit = false, isWraptext = true, CustomWidth = 12 });
            #endregion Columns
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

            //Body
            byte[] filecontent = ClassExportExcel.ExportExcel(viewModel, columns, heading, true, HasExtraSheet: false, headerRowMergeCount: 1,PageSize:ePaperSize.A3,Scale:70, Orientation: eOrientation.Landscape,headerFontSize: 22,bodyFontSize: 15);
            string fileNameWithFormat = string.Format("{0}.xlsx", _unitOfWork.UtilitiesRepository.RemoveSign4VietnameseString(fileheader.ToUpper()).Replace(" ", "_"));

            return File(filecontent, ClassExportExcel.ExcelContentType, fileNameWithFormat);
        }
        #endregion Export to excel
    }
}