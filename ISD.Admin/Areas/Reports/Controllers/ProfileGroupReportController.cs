using DevExpress.Web.Mvc;
using ISD.Constant;
using ISD.Core;
using ISD.Extensions;
using ISD.Repositories;
using ISD.Repositories.Excel;
using ISD.ViewModels;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace Reports.Controllers
{
    public class ProfileGroupReportController : BaseController
    {
        // GET: ProfileGroupReport
        #region Index
        [ISDAuthorizationAttribute]
        public ActionResult Index()
        {
            var searchModel = (ProfileGroupSearchModel)TempData[CurrentUser.AccountId + "ProfileGroupSearchData"];
            var tempalteIdString = TempData[CurrentUser.AccountId + "ProfileGroupTemplateId"];
            var modeSearch = TempData[CurrentUser.AccountId + "ProfileGroupModeSearch"];
            var pageId = GetPageId("/Reports/ProfileGroupReport");

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
            var SelectedCommonDate = "ThisMonth";
            if (searchModel == null)
            {
                DateTime? FromDate; DateTime? ToDate;
                _unitOfWork.CommonDateRepository.GetDateBy(SelectedCommonDate, out FromDate, out ToDate);
                searchModel = new ProfileGroupSearchModel()
                {
                    CommonDate = SelectedCommonDate,
                    FromDate = FromDate,
                    ToDate = ToDate
                };
            }
            else SelectedCommonDate = searchModel.CommonDate;
            var CurrentDateList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CommonDate);
            ViewBag.CommonDate = new SelectList(CurrentDateList, "CatalogCode", "CatalogText_vi", SelectedCommonDate);
            CreateViewBag();
            return View(searchModel);
        }
        public void CreateViewBag()
        {
            var _catalogRepository = new CatalogRepository(_context);
            #region //Get list CustomerGroup (Nhóm khách hàng doanh nghiệp)
            var customerGroupList = _catalogRepository.GetCustomerCategory(CurrentUser.CompanyCode);
            ViewBag.ProfileGroupCode = new SelectList(customerGroupList, "CatalogCode", "CatalogText_vi");
            #endregion

            #region Get select Company
            //Dropdown Company
            var companyList = _unitOfWork.CompanyRepository.GetAll();
            ViewBag.CompanyCode = new SelectList(companyList, "CompanyCode", "CompanyName");
            #endregion
        }
        #endregion

        #region Export Excel
        const int startIndex = 8;

        public ActionResult ExportExcel(ProfileGroupSearchModel searchModel)
        {
            //get search value to display in file
            var filterDisplayList = GetSearchInfoToDisplay(searchModel);
            var data = GetData(searchModel);
            var modelTotal = new ProfileGroupReportViewModel
            {
                ProfileGroupName = "Tổng",
                NumberOfProfiles = data.Sum(p => p.NumberOfProfiles),
                PercentOfProfiles = data.Sum(p => p.PercentOfProfiles)
            };
            data.Add(modelTotal);
           
            return Export(data, filterDisplayList);
        }
        private List<ProfileGroupReportViewModel> GetData(ProfileGroupSearchModel searchModel)
        {
            if (searchModel.ProfileGroupCode != null && searchModel.ProfileGroupCode.Count > 0)
            {
                var firstProfileGroupCode = searchModel.ProfileGroupCode[0];
                if (string.IsNullOrEmpty(firstProfileGroupCode))
                {
                    searchModel.ProfileGroupCode = new List<string>();
                    searchModel.ProfileGroupCode = _unitOfWork.CatalogRepository.GetCustomerCategory(CurrentUser.CompanyCode).Select(x => x.CatalogCode).ToList();
                }
            }
            else
            {
                searchModel.ProfileGroupCode = new List<string>();
                searchModel.ProfileGroupCode = _unitOfWork.CatalogRepository.GetCustomerCategory(CurrentUser.CompanyCode).Select(x=>x.CatalogCode).ToList();
            }

            #region ProfileGroupCode
            //Build your record
            var tableProfileGroupSchema = new List<SqlMetaData>(1)
            {
                new SqlMetaData("StringList", SqlDbType.NVarChar, 50)
            }.ToArray();

            //And a table as a list of those records
            var tableProfileGroup = new List<SqlDataRecord>();
            if (searchModel.ProfileGroupCode != null && searchModel.ProfileGroupCode.Count > 0)
            {
                foreach (var r in searchModel.ProfileGroupCode)
                {
                    var tableRow = new SqlDataRecord(tableProfileGroupSchema);
                    tableRow.SetString(0, r);
                    tableProfileGroup.Add(tableRow);
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tableProfileGroupSchema);
                tableProfileGroup.Add(tableRow);
            }
            #endregion

            
            string sqlQuery = "EXEC [Report].[usp_ProfileGroupReport] @CustomerGroupCode, @CurrentCompanyCode, @FromDate, @ToDate";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "CustomerGroupCode",
                    TypeName = "[dbo].[StringList]", //Don't forget this one!
                    Value = tableProfileGroup
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "CurrentCompanyCode",
                    Value = searchModel.CompanyCode ?? (object)DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "FromDate",
                    Value = searchModel.FromDate ?? (object)DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "ToDate",
                    Value = searchModel.ToDate ?? (object)DBNull.Value,
                },
            };
            var result = _context.Database.SqlQuery<ProfileGroupReportViewModel>(sqlQuery, parameters.ToArray()).ToList();

            
            return result;
        }
        public FileContentResult Export(List<ProfileGroupReportViewModel> viewModel, List<SearchDisplayModel> filters)
        {
            List<ExcelTemplate> columns = new List<ExcelTemplate>();
            //Header
            string fileheader = string.Empty;
            fileheader = "BÁO CÁO TỶ LỆ NHÓM KHÁCH HÀNG";
            columns.Add(new ExcelTemplate { ColumnName = "ProfileGroupName", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "NumberOfProfiles", isAllowedToEdit = false, isCurrency = true });
            columns.Add(new ExcelTemplate { ColumnName = "PercentOfProfiles", isAllowedToEdit = false });
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
            byte[] filecontent = ClassExportExcel.ExportExcel(viewModel, columns, heading, true, HasExtraSheet: false, IsMergeCellHeader: false);
            string fileNameWithFormat = string.Format("{0}.xlsx", _unitOfWork.UtilitiesRepository.RemoveSign4VietnameseString(fileheader.ToUpper()).Replace(" ", "_"));

            return File(filecontent, ClassExportExcel.ExcelContentType, fileNameWithFormat);
        }
        #endregion
        public ActionResult ViewDetail(ProfileGroupSearchModel searchModel, Guid pivotTemplate, string modeSearch)
        {
            TempData[CurrentUser.AccountId + "ProfileGroupSearchData"] = searchModel;
            TempData[CurrentUser.AccountId + "ProfileGroupTemplateId"] = pivotTemplate;
            TempData[CurrentUser.AccountId + "ProfileGroupModeSearch"] = modeSearch;
            return RedirectToAction("Index");
        }

        public ActionResult ChangeTemplate(Guid pivotTemplate, ProfileGroupSearchModel searchModel, string modeSearch)
        {
            TempData[CurrentUser.AccountId + "ProfileGroupSearchData"] = searchModel;
            TempData[CurrentUser.AccountId + "ProfileGroupTemplateId"] = pivotTemplate;
            TempData[CurrentUser.AccountId + "ProfileGroupModeSearch"] = modeSearch;
            return RedirectToAction("Index");
        }


        [ValidateInput(false)]
        public ActionResult ProfileGroupPivotGridPartial(Guid? templateId = null, ProfileGroupSearchModel searchViewModel = null, string jsonReq = null)
        {
            var pageId = GetPageId("/Reports/ProfileGroupReport");
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
                return PartialView("_ProfileGroupPivotGridPartial", null);
            }
            else
            {
                if (!string.IsNullOrEmpty(jsonReq))
                {
                    searchViewModel = JsonConvert.DeserializeObject<ProfileGroupSearchModel>(jsonReq);
                }
                var model = GetData(searchViewModel);
                ViewBag.Search = searchViewModel;
                return PartialView("_ProfileGroupPivotGridPartial", model);
            }
        }
        [HttpPost]
        public ActionResult ExportPivot(ProfileGroupSearchModel searchViewModel, Guid? templateId)
        {
            PivotGridExportOptions options = new PivotGridExportOptions();
            options.ExportType = PivotGridExportFormats.Excel;
            options.WYSIWYG.PrintRowHeaders = true;
            options.WYSIWYG.PrintFilterHeaders = false;
            options.WYSIWYG.PrintDataHeaders = false;
            options.WYSIWYG.PrintColumnHeaders = false;
            //get search value to display in file
            var filterDisplayList = GetSearchInfoToDisplay(searchViewModel);
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
            string fileName = "BAO_CAO_TY_LE_NHOM_KHACH_HANG";
            return PivotGridExportExcel.GetExportActionResult(fileName, options, pivotSetting, model, filterDisplayList, headerText, layoutConfigs);
        }
        #region Lấy điều kiện lọc
        private List<SearchDisplayModel> GetSearchInfoToDisplay(ProfileGroupSearchModel searchViewModel)
        {
            List<SearchDisplayModel> filterList = new List<SearchDisplayModel>();
            if (searchViewModel.FromDate != null)
            {
                filterList.Add(new SearchDisplayModel() { DisplayName = "Tạo từ ngày", DisplayValue = searchViewModel.FromDate.Value.ToString("dd-MM-yyyy") });
            }
            if (searchViewModel.ToDate != null)
            {
                filterList.Add(new SearchDisplayModel() { DisplayName = "Tạo đến ngày", DisplayValue = searchViewModel.ToDate.Value.ToString("dd-MM-yyyy") });
            }

            if (!string.IsNullOrEmpty(searchViewModel.CompanyCode))
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Công Ty";
                foreach (var code in searchViewModel.CompanyCode)
                {
                    var company = _context.CompanyModel.FirstOrDefault(s => s.CompanyCode == searchViewModel.CompanyCode);
                    var value = company.CompanyName;
                    filter.DisplayValue = value;
                }

                filterList.Add(filter);
            }
            if (searchViewModel.ProfileGroupCode != null && searchViewModel.ProfileGroupCode.Count > 0)
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Nhóm khách hàng";
                foreach (var code in searchViewModel.ProfileGroupCode)
                {
                    var group = _context.CatalogModel.FirstOrDefault(s => s.CatalogCode == code && s.CatalogTypeCode == ConstCatalogType.CustomerGroup);
                    filter.DisplayValue += group.CatalogText_vi;
                    filter.DisplayValue += ", ";
                }
                filterList.Add(filter);
            }

            
            return filterList;
        }
        #endregion
    }
}