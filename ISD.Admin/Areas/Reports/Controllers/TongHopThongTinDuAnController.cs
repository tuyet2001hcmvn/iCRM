
using DevExpress.Web.Mvc;
using ISD.Constant;
using ISD.Core;
using ISD.Extensions;
using ISD.Repositories.Excel;
using ISD.ViewModels;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Reports.Controllers
{
    public class TongHopThongTinDuAnController : BaseController
    {
        // GET: TongHopThongTinDuAn
        public ActionResult Index()
        {
            #region Remove sesion
            Session.Remove(CurrentUser.AccountId + "Layout");
            Session.Remove(CurrentUser.AccountId + "LayoutConfigs");
            #endregion

            var searchModel = (OpportunityReportSearchViewModel)TempData[CurrentUser.AccountId + "OpportunityReportSearchData"];
            var tempalteIdString = TempData[CurrentUser.AccountId + "OpportunityReportTemplateId"];
            var modeSearch = TempData[CurrentUser.AccountId + "OpportunityReportModeSearch"];
            var pageId = GetPageId("/Reports/TongHopThongTinDuAn");

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

            #region Chủ đầu tư
            var investors = _context.ProfileModel.Where(p => p.IsInvestor == true).Select(p => new { p.ProfileId, name = p.ProfileShortName??p.ProfileCode.ToString() + "(Chưa đặt tên ngắn)"  }).ToList();
            ViewBag.Investor = new MultiSelectList(investors, "ProfileId", "name");
            #endregion

            #region Thiết kế
            var designers = _context.ProfileModel.Where(p => p.IsDesigner == true).Select(p => new { p.ProfileId, name = p.ProfileShortName ?? p.ProfileCode.ToString() + "(Chưa đặt tên ngắn)" }).ToList();
            ViewBag.Designer = new MultiSelectList(designers, "ProfileId", "name");
            #endregion

            #region Tổng thầu
            var contractors = _context.ProfileModel.Where(p => p.IsContractor == true).Select(p => new { p.ProfileId, name = p.ProfileShortName ?? p.ProfileCode.ToString() + "(Chưa đặt tên ngắn)" }).ToList();
            ViewBag.Contractor = new MultiSelectList(contractors, "ProfileId", "name");
            #endregion
            #region Đối thủ
            var competitors = (from a in _context.ProfileModel
                               join b in _context.Profile_ProfileType_Mapping on a.ProfileId equals b.ProfileId into bTemp
                               from map in bTemp.DefaultIfEmpty()
                               where (a.CustomerTypeCode == ConstProfileType.Competitor || map.ProfileType == ConstProfileType.Competitor)
                               orderby a.ProfileName
                               select new { a.ProfileId, name = a.ProfileShortName ?? a.ProfileCode.ToString() + "(Chưa đặt tên ngắn)" }).ToList();
            ViewBag.Competitor = new MultiSelectList(competitors, "ProfileId", "name");
            #endregion

            #region //Get list Sale Office (Khu vực)
            var saleOfficeList = _unitOfWork.CatalogRepository.GetOpportunityRegion(false).Where(p => p.CatalogCode != "9999").ToList();
            ViewBag.SaleOfficeCode = new SelectList(saleOfficeList, "CatalogCode", "CatalogText_vi");
            #endregion

            #region //Get list Province (Tỉnh/Thành phố)

            //Load Tỉnh thành theo Khu vực (sắp xếp theo thứ tự các tỉnh thuộc khu vực chọn sẽ được xếp trước)
            var saleOfficeCodeList = saleOfficeList.Select(p => p.CatalogCode).ToList();
            var provinceAreaList = _context.ProvinceModel.Where(p => p.Actived == true && saleOfficeCodeList.Contains(p.Area))
                                           .Select(p => new ProvinceViewModel()
                                           {
                                               ProvinceId = p.ProvinceId,
                                               ProvinceCode = p.ProvinceCode,
                                               ProvinceName = p.ProvinceName,
                                               Area = p.Area,
                                               OrderIndex = p.OrderIndex
                                           }).ToList();

            provinceAreaList = provinceAreaList.OrderBy(p => p.ProvinceCode).OrderByDescending(p => p.ProvinceName == "Hồ Chí Minh").ThenByDescending(p => p.ProvinceName == "Hà Nội").ToList();
            ViewBag.ProvinceId = new SelectList(provinceAreaList, "ProvinceId", "ProvinceName", searchModel != null ? searchModel.ProvinceId : null);
            #endregion

            #region District (Quận/Huyện)
            ViewBag.DistrictId = new SelectList(new List<DistrictViewModel>(), "DistrictId", "DistrictName");
            if (searchModel != null && searchModel.DistrictId != null && searchModel.DistrictId.Count() > 0)
            {
                var districtLst = _unitOfWork.DistrictRepository.GetBy(searchModel.ProvinceId);
                ViewBag.DistrictId = new MultiSelectList(districtLst, "DistrictId", "DistrictName", searchModel.DistrictId);
            }
            #endregion

            #region Xác suất
            var workFlowId = _context.WorkFlowModel.Where(p => p.WorkFlowCode == ConstWorkFlow.PROJECT).Select(p => p.WorkFlowId).FirstOrDefault();
            var opportunityPercentageList = _context.TaskStatusModel.Where(p => p.WorkFlowId == workFlowId).OrderBy(p => p.OrderIndex).ToList();
            ViewBag.OpportunityPercentage = new MultiSelectList(opportunityPercentageList, "TaskStatusId", "TaskStatusName");
            #endregion


            #region Dự án
            var opportunityList = (from a in _context.ProfileModel
                              join b in _context.Profile_ProfileType_Mapping on a.ProfileId equals b.ProfileId into bTemp
                              from map in bTemp.DefaultIfEmpty()
                              where (a.CustomerTypeCode == ConstProfileType.Opportunity || map.ProfileType == ConstProfileType.Opportunity)
                              //trạng thái đang hoạt động
                              && a.Actived == true
                              orderby a.ProfileCode
                              select new {
                                  a.ProfileId,
                                  ProfileName = !string.IsNullOrEmpty(a.ProfileShortName) ? a.ProfileShortName : a.ProfileName
                              });
            ViewBag.Opportunity = new MultiSelectList(opportunityList, "ProfileId", "ProfileName");
            #endregion


            #region sale/spec
            var empLst = _unitOfWork.SalesEmployeeRepository.GetAllForDropdownlist();
            ViewBag.SaleSpecCode = new MultiSelectList(empLst, "SalesEmployeeCode", "SalesEmployeeName");
            #endregion

            #region Năm hoàn thiện
            var completeYear = new List<ISDSelectIntItem>();
            int numberOfYear = DateTime.Now.Year - 2021;
            if (numberOfYear > 0)
            {
                for (int i = numberOfYear; i >= 0; i--)
                {
                    completeYear.Add(new ISDSelectIntItem() { id = DateTime.Now.AddYears(-i).Year, name = DateTime.Now.AddYears(-i).Year.ToString() });
                }
            }
            var currentYear = DateTime.Now.Year;
            for (int i = 0; i < 10; i++)
            {
                completeYear.Add(new ISDSelectIntItem()
                {
                    id = currentYear,
                    name = currentYear.ToString(),
                });
                currentYear++;
            }
            ViewBag.FromCompleteYear = new SelectList(completeYear, "id", "name");
            ViewBag.ToCompleteYear = new SelectList(completeYear, "id", "name");
            #endregion

            #region //Get list SalesEmployee (NV phụ trách)
            //ViewBag.EmployeeList = _unitOfWork.PersonInChargeRepository.GetListEmployee();
            ViewBag.EmployeeList1 = _unitOfWork.PersonInChargeRepository.GetListEmployeeByRoles(ConstSalesEmployeeType.NVKD_Code);
            ViewBag.EmployeeList2 = _unitOfWork.PersonInChargeRepository.GetListEmployeeByRoles(ConstSalesEmployeeType.NVSalesAdmin_Code);
            ViewBag.EmployeeList3 = _unitOfWork.PersonInChargeRepository.GetListEmployeeByRoles(ConstSalesEmployeeType.NVSpec_Code);
            #endregion

            #region //Get list Roles (Phòng ban)
            var rolesList = _context.RolesModel.Where(p => p.Actived == true && p.isEmployeeGroup == true).ToList();
            ViewBag.RolesCode = new MultiSelectList(rolesList, "RolesCode", "RolesName");
            #endregion
            #region //Get list OpportunityStatusType (Tình trạng dự án)
            var opportunityStatusTypeList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.Opportunity_Status);
            ViewBag.OpportunityStatusType = new MultiSelectList(opportunityStatusTypeList, "CatalogCode", "CatalogText_vi");

            #endregion

            //xử lý gán dữ liệu search cho autocomplete
            if (searchModel != null)
            {
                #region Chủ đầu tư
                if (searchModel.Investor != null && searchModel.Investor.Count() > 0)
                {
                    var investor = (from a in _context.ProfileModel
                                    join i in searchModel.Investor on a.ProfileId equals i
                                    orderby a.ProfileName
                                    select new ISDSelectItem()
                                    {
                                        value = a.ProfileId,
                                        text = a.ProfileShortName
                                    }).FirstOrDefault();
                    ViewBag.InvestorId = investor.value;
                    ViewBag.InvestorName = investor.text;

                }
                #endregion

                #region Thiết kế
                if (searchModel.Designer != null && searchModel.Designer.Count() > 0)
                {
                    var designer = (from a in _context.ProfileModel
                                    join i in searchModel.Designer on a.ProfileId equals i
                                    orderby a.ProfileName
                                    select new ISDSelectItem()
                                    {
                                        value = a.ProfileId,
                                        text = a.ProfileShortName
                                    }).FirstOrDefault();
                    ViewBag.DesignerId = designer.value;
                    ViewBag.DesignerName = designer.text;
                }
                #endregion

                #region Tổng thầu
                if (searchModel.Contractor != null && searchModel.Contractor.Count() > 0)
                {
                    var contractor = (from a in _context.ProfileModel
                                      join i in searchModel.Contractor on a.ProfileId equals i
                                      orderby a.ProfileName
                                      select new ISDSelectItem()
                                      {
                                          value = a.ProfileId,
                                          text = a.ProfileShortName
                                      }).FirstOrDefault();
                    ViewBag.ContractorId = contractor.value;
                    ViewBag.ContractorName = contractor.text;
                }
                #endregion

                #region Đối thủ
                if (searchModel.Competitor != null && searchModel.Competitor.Count() > 0)
                {
                    var competitor = (from a in _context.ProfileModel
                                      join i in searchModel.Competitor on a.ProfileId equals i
                                      orderby a.ProfileName
                                      select new ISDSelectItem()
                                      {
                                          value = a.ProfileId,
                                          text = a.ProfileShortName
                                      }).FirstOrDefault();
                    ViewBag.CompetitorId = competitor.value;
                    ViewBag.CompetitorName = competitor.text;
                }
                #endregion

                #region Dự án
                if (searchModel.Opportunity != null && searchModel.Opportunity.Count() > 0)
                {
                    var competitor = (from a in _context.ProfileModel
                                      join i in searchModel.Opportunity on a.ProfileId equals i
                                      orderby a.ProfileName
                                      select new ISDSelectItem()
                                      {
                                          value = a.ProfileId,
                                          text = a.ProfileShortName
                                      }).FirstOrDefault();
                    ViewBag.OpportunityId = competitor.value;
                    ViewBag.OpportunityName = competitor.text;
                }
                #endregion

            }
            return View(searchModel);
        }

        public ActionResult ViewDetail(OpportunityReportSearchViewModel searchModel, Guid pivotTemplate, string modeSearch)
        {
            TempData[CurrentUser.AccountId + "OpportunityReportSearchData"] = searchModel;
            TempData[CurrentUser.AccountId + "OpportunityReportTemplateId"] = pivotTemplate;
            TempData[CurrentUser.AccountId + "OpportunityReportModeSearch"] = modeSearch;
            return RedirectToAction("Index");
        }

        public ActionResult ChangeTemplate(Guid pivotTemplate, OpportunityReportSearchViewModel searchModel, string modeSearch)
        {
            TempData[CurrentUser.AccountId + "OpportunityReportSearchData"] = searchModel;
            TempData[CurrentUser.AccountId + "OpportunityReportTemplateId"] = pivotTemplate;
            TempData[CurrentUser.AccountId + "OpportunityReportModeSearch"] = modeSearch;
            return RedirectToAction("Index");
        }

        [ValidateInput(false)]
        public ActionResult OpportunityPivotGridPartial(Guid? templateId = null, OpportunityReportSearchViewModel searchViewModel = null, string jsonReq = null)
        {
            var pageId = GetPageId("/Reports/TongHopThongTinDuAn");
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
                    if (searchViewModel == null)
                    {
                        searchViewModel = new OpportunityReportSearchViewModel();
                    }
                    searchViewModel.OrderBy = Template.OrderBy;
                    searchViewModel.TypeSort = Template.TypeSort;
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
                return PartialView("_OpportunityPivotGridPartial", null);
            }
            else
            {
                if (!string.IsNullOrEmpty(jsonReq))
                {
                    searchViewModel = JsonConvert.DeserializeObject<OpportunityReportSearchViewModel>(jsonReq);
                }

                if (templateId.HasValue)
                {
                    var pivotTemplate = _unitOfWork.PivotGridTemplateRepository.GetTemplateById(templateId.Value);
                    if (pivotTemplate != null)
                    {
                        searchViewModel.ReportType = pivotTemplate.TemplateName;
                    }
                }


                var model = GetData(searchViewModel);
                ViewBag.Search = searchViewModel;
                return PartialView("_OpportunityPivotGridPartial", model);
            }
        }

        #region GetData
        private List<OpportunityReportViewModel> GetData(OpportunityReportSearchViewModel searchModel)
        {
            List<OpportunityReportViewModel> result = new List<OpportunityReportViewModel>();

            #region Xác suất
            //Build your record
            var tableOpportunityPercentageSchema = new List<SqlMetaData>(1)
                {
                    new SqlMetaData("Id", SqlDbType.UniqueIdentifier)
                }.ToArray();

            //And a table as a list of those records
            var tableOpportunityPercentage = new List<SqlDataRecord>();
            List<Guid> opportunityPercentageLst = new List<Guid>();
            if (searchModel.OpportunityPercentage != null && searchModel.OpportunityPercentage.Count > 0)
            {
                foreach (var r in searchModel.OpportunityPercentage)
                {
                    var tableRow = new SqlDataRecord(tableOpportunityPercentageSchema);
                    tableRow.SetGuid(0, r);
                    if (!opportunityPercentageLst.Contains(r))
                    {
                        opportunityPercentageLst.Add(r);
                        tableOpportunityPercentage.Add(tableRow);
                    }
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tableOpportunityPercentageSchema);
                tableOpportunityPercentage.Add(tableRow);
            }
            #endregion
            #region Chủ đầu tư
            //Build your record
            var tableInvestorSchema = new List<SqlMetaData>(1)
                {
                    new SqlMetaData("Id", SqlDbType.UniqueIdentifier)
                }.ToArray();

            //And a table as a list of those records
            var tableInvestor = new List<SqlDataRecord>();
            List<Guid> investorLst = new List<Guid>();
            if (searchModel.Investor != null && searchModel.Investor.Count > 0)
            {
                foreach (var r in searchModel.Investor)
                {
                    var tableRow = new SqlDataRecord(tableInvestorSchema);
                    tableRow.SetGuid(0, r);
                    if (!investorLst.Contains(r))
                    {
                        investorLst.Add(r);
                        tableInvestor.Add(tableRow);
                    }
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tableInvestorSchema);
                tableInvestor.Add(tableRow);
            }
            #endregion
            #region Thiết kế
            //Build your record
            var tableDesignerSchema = new List<SqlMetaData>(1)
                {
                    new SqlMetaData("Id", SqlDbType.UniqueIdentifier)
                }.ToArray();

            //And a table as a list of those records
            var tableDesigner = new List<SqlDataRecord>();
            List<Guid> designerLst = new List<Guid>();
            if (searchModel.Designer != null && searchModel.Designer.Count > 0)
            {
                foreach (var r in searchModel.Designer)
                {
                    var tableRow = new SqlDataRecord(tableDesignerSchema);
                    tableRow.SetGuid(0, r);
                    if (!designerLst.Contains(r))
                    {
                        designerLst.Add(r);
                        tableDesigner.Add(tableRow);
                    }
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tableDesignerSchema);
                tableDesigner.Add(tableRow);
            }
            #endregion
            #region Đối thủ
            //Build your record
            var tableCompetitorSchema = new List<SqlMetaData>(1)
                {
                    new SqlMetaData("Id", SqlDbType.UniqueIdentifier)
                }.ToArray();

            //And a table as a list of those records
            var tableCompetitor = new List<SqlDataRecord>();
            List<Guid> competitorLst = new List<Guid>();
            if (searchModel.Competitor != null && searchModel.Competitor.Count > 0)
            {
                foreach (var r in searchModel.Competitor)
                {
                    var tableRow = new SqlDataRecord(tableCompetitorSchema);
                    tableRow.SetGuid(0, r);
                    if (!competitorLst.Contains(r))
                    {
                        competitorLst.Add(r);
                        tableCompetitor.Add(tableRow);
                    }
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tableCompetitorSchema);
                tableCompetitor.Add(tableRow);
            }
            #endregion
            #region Tổng thầu
            //Build your record
            var tableContractorSchema = new List<SqlMetaData>(1)
                {
                    new SqlMetaData("Id", SqlDbType.UniqueIdentifier)
                }.ToArray();

            //And a table as a list of those records
            var tableContractor = new List<SqlDataRecord>();
            List<Guid> contractorLst = new List<Guid>();
            if (searchModel.Contractor != null && searchModel.Contractor.Count > 0)
            {
                foreach (var r in searchModel.Contractor)
                {
                    var tableRow = new SqlDataRecord(tableContractorSchema);
                    tableRow.SetGuid(0, r);
                    if (!contractorLst.Contains(r))
                    {
                        contractorLst.Add(r);
                        tableContractor.Add(tableRow);
                    }
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tableContractorSchema);
                tableContractor.Add(tableRow);
            }
            #endregion
            #region Quận huyện
            //Build your record
            var tableDistrictIdSchema = new List<SqlMetaData>(1)
                {
                    new SqlMetaData("Id", SqlDbType.UniqueIdentifier)
                }.ToArray();

            //And a table as a list of those records
            var tableDistrictId = new List<SqlDataRecord>();
            List<Guid> districtIdLst = new List<Guid>();
            if (searchModel.DistrictId != null && searchModel.DistrictId.Count > 0)
            {
                foreach (var r in searchModel.DistrictId)
                {
                    var tableRow = new SqlDataRecord(tableDistrictIdSchema);
                    tableRow.SetGuid(0, r);
                    if (!districtIdLst.Contains(r))
                    {
                        districtIdLst.Add(r);
                        tableDistrictId.Add(tableRow);
                    }
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tableDistrictIdSchema);
                tableDistrictId.Add(tableRow);
            }
            #endregion
            #region Dự án
            //Build your record
            var tableOpportunitySchema = new List<SqlMetaData>(1)
                {
                    new SqlMetaData("Id", SqlDbType.UniqueIdentifier)
                }.ToArray();

            //And a table as a list of those records
            var tableOpportunity = new List<SqlDataRecord>();
            List<Guid> opportunityLst = new List<Guid>();
            if (searchModel.Opportunity != null && searchModel.Opportunity.Count > 0)
            {
                foreach (var r in searchModel.Opportunity)
                {
                    var tableRow = new SqlDataRecord(tableOpportunitySchema);
                    tableRow.SetGuid(0, r);
                    if (!opportunityLst.Contains(r))
                    {
                        opportunityLst.Add(r);
                        tableOpportunity.Add(tableRow);
                    }
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tableOpportunitySchema);
                tableOpportunity.Add(tableRow);
            }
            #endregion
            #region NV kinh doanh
            //Build your record
            var tablePersonInChargeSalesSchema = new List<SqlMetaData>(1)
                {
                    new SqlMetaData("Code", SqlDbType.NVarChar, 100)
                }.ToArray();

            //And a table as a list of those records
            var tablePersonInChargeSales = new List<SqlDataRecord>();
            List<string> personInChargeSalesLst = new List<string>();
            if (searchModel.PersonInChargeSales != null && searchModel.PersonInChargeSales.Count > 0)
            {
                foreach (var r in searchModel.PersonInChargeSales)
                {
                    var tableRow = new SqlDataRecord(tablePersonInChargeSalesSchema);
                    tableRow.SetString(0, r);
                    if (!personInChargeSalesLst.Contains(r))
                    {
                        personInChargeSalesLst.Add(r);
                        tablePersonInChargeSales.Add(tableRow);
                    }
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tablePersonInChargeSalesSchema);
                tablePersonInChargeSales.Add(tableRow);
            }
            #endregion
            #region NV Sales
            //Build your record
            var tablePersonInChargeSalesAdminSchema = new List<SqlMetaData>(1)
                {
                    new SqlMetaData("Code", SqlDbType.NVarChar, 100)
                }.ToArray();

            //And a table as a list of those records
            var tablePersonInChargeSalesAdmin = new List<SqlDataRecord>();
            List<string> personInChargeSalesAdminLst = new List<string>();
            if (searchModel.PersonInChargeSalesAdmin != null && searchModel.PersonInChargeSalesAdmin.Count > 0)
            {
                foreach (var r in searchModel.PersonInChargeSalesAdmin)
                {
                    var tableRow = new SqlDataRecord(tablePersonInChargeSalesAdminSchema);
                    tableRow.SetString(0, r);
                    if (!personInChargeSalesAdminLst.Contains(r))
                    {
                        personInChargeSalesAdminLst.Add(r);
                        tablePersonInChargeSalesAdmin.Add(tableRow);
                    }
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tablePersonInChargeSalesAdminSchema);
                tablePersonInChargeSalesAdmin.Add(tableRow);
            }
            #endregion
            #region NV Spec
            //Build your record
            var tablePersonInChargeSpecSchema = new List<SqlMetaData>(1)
                {
                    new SqlMetaData("Code", SqlDbType.NVarChar, 100)
                }.ToArray();

            //And a table as a list of those records
            var tablePersonInChargeSpec = new List<SqlDataRecord>();
            List<string> personInChargeSpecLst = new List<string>();
            if (searchModel.PersonInChargeSpec != null && searchModel.PersonInChargeSpec.Count > 0)
            {
                foreach (var r in searchModel.PersonInChargeSpec)
                {
                    var tableRow = new SqlDataRecord(tablePersonInChargeSpecSchema);
                    tableRow.SetString(0, r);
                    if (!personInChargeSpecLst.Contains(r))
                    {
                        personInChargeSpecLst.Add(r);
                        tablePersonInChargeSpec.Add(tableRow);
                    }
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tablePersonInChargeSpecSchema);
                tablePersonInChargeSpec.Add(tableRow);
            }
            #endregion
            #region Phòng ban
            //Build your record
            var tableRolesCodeSchema = new List<SqlMetaData>(1)
                {
                    new SqlMetaData("Code", SqlDbType.NVarChar, 100)
                }.ToArray();

            //And a table as a list of those records
            var tableRolesCode = new List<SqlDataRecord>();
            List<string> rolesCodeLst = new List<string>();
            if (searchModel.RolesCode != null && searchModel.RolesCode.Count > 0)
            {
                foreach (var r in searchModel.RolesCode)
                {
                    var tableRow = new SqlDataRecord(tableRolesCodeSchema);
                    tableRow.SetString(0, r);
                    if (!rolesCodeLst.Contains(r))
                    {
                        rolesCodeLst.Add(r);
                        tableRolesCode.Add(tableRow);
                    }
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tableRolesCodeSchema);
                tableRolesCode.Add(tableRow);
            }
            #endregion
           
            #region Khu vực
            //Build your record
            var tableSaleOfficeCodeSchema = new List<SqlMetaData>(1)
                {
                    new SqlMetaData("Code", SqlDbType.NVarChar, 100)
                }.ToArray();

            //And a table as a list of those records
            var tableSaleOfficeCode = new List<SqlDataRecord>();
            List<string> saleOfficeCodeLst = new List<string>();
            if (searchModel.SaleOfficeCode != null && searchModel.SaleOfficeCode.Count > 0)
            {
                foreach (var r in searchModel.SaleOfficeCode)
                {
                    var tableRow = new SqlDataRecord(tableSaleOfficeCodeSchema);
                    tableRow.SetString(0, r);
                    if (!saleOfficeCodeLst.Contains(r))
                    {
                        saleOfficeCodeLst.Add(r);
                        tableSaleOfficeCode.Add(tableRow);
                    }
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tableSaleOfficeCodeSchema);
                tableSaleOfficeCode.Add(tableRow);
            }
            #endregion
            #region Tình trạng dự án
            //Build your record
            var tableOpportunityStatusTypeSchema = new List<SqlMetaData>(1)
                {
                    new SqlMetaData("Code", SqlDbType.NVarChar, 100)
                }.ToArray();

            //And a table as a list of those records
            var tableOpportunityStatusType = new List<SqlDataRecord>();
            List<string> opportunityStatusTypeLst = new List<string>();
            if (searchModel.OpportunityStatusType != null && searchModel.OpportunityStatusType.Count > 0)
            {
                foreach (var r in searchModel.OpportunityStatusType)
                {
                    var tableRow = new SqlDataRecord(tableOpportunityStatusTypeSchema);
                    tableRow.SetString(0, r);
                    if (!opportunityStatusTypeLst.Contains(r))
                    {
                        opportunityStatusTypeLst.Add(r);
                        tableOpportunityStatusType.Add(tableRow);
                    }
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tableOpportunityStatusTypeSchema);
                tableOpportunityStatusType.Add(tableRow);
            }
            #endregion
            string sqlQuery = "EXEC [Report].[usp_TongHopThongTinDuAn] @Investor, @Designer, @SaleOfficeCode, @SaleSpecCode, @ProjectLocation, @OpportunityName, @Opportunity, @SearchProjectMode, @ReportType, @FromCompleteYear, @ToCompleteYear, @PersonInChargeSales, @PersonInChargeSalesAdmin, @FromValue, @ToValue, @OpportunityPercentage, @RolesCode, @CurrentCompanyCode, @Contractor, @ProvinceId, @DistrictId, @PersonInChargeSpec, @Competitor,@IsTopInvestor,@AccountId,@OrderBy,@TypeSort,@OpportunityStatusType";
            //string sqlQuery = "EXEC [Report].[usp_TongHopThongTinDuAn] @Investor, @Designer, @SaleOfficeCode, @SaleSpecCode, @ProjectLocation, @OpportunityName, @SearchProjectMode, @ReportType, @FromCompleteYear, @ToCompleteYear, @PersonInChargeSales, @PersonInChargeSalesAdmin, @FromValue, @ToValue, @OpportunityPercentage, @RolesCode, @CurrentCompanyCode, @Contractor, @ProvinceId, @DistrictId";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter
                {
                    //SqlDbType = SqlDbType.UniqueIdentifier,
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "Investor",
                    TypeName = "[dbo].[GuidList]",
                    Value = tableInvestor
                    //Value = searchModel.Investor ?? (object)DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "Designer",
                    //Value = searchModel.Designer ?? (object)DBNull.Value,
                    TypeName = "[dbo].[GuidList]",
                    Value = tableDesigner
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "SaleOfficeCode",
                    TypeName = "[dbo].[StringList]",
                    Value = tableSaleOfficeCode

                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "SaleSpecCode",
                    Value = searchModel.SaleSpecCode ?? (object)DBNull.Value,

                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "ProjectLocation",
                    Value = searchModel.ProjectLocation ?? (object)DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "OpportunityName",
                    Value = searchModel.OpportunityName ?? (object)DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "Opportunity",
                    //Value = searchModel.OpportunityName ?? (object)DBNull.Value,
                    TypeName = "[dbo].[GuidList]",
                    Value = tableOpportunity
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "SearchProjectMode",
                    Value = searchModel.SearchProjectMode ?? (object)DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "ReportType",
                    Value = searchModel.ReportType ?? (object)DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "FromCompleteYear",
                    Value = searchModel.FromCompleteYear ?? (object)DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "ToCompleteYear",
                    Value = searchModel.ToCompleteYear ?? (object)DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "PersonInChargeSales",
                    TypeName = "[dbo].[StringList]",
                    Value = tablePersonInChargeSales
                    //Value = searchModel.PersonInChargeSales ?? (object)DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "PersonInChargeSalesAdmin",
                    TypeName = "[dbo].[StringList]",
                    Value = tablePersonInChargeSalesAdmin
                    //Value = searchModel.PersonInChargeSalesAdmin ?? (object)DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Decimal,
                    Direction = ParameterDirection.Input,
                    ParameterName = "FromValue",
                    Value = searchModel.FromValue ?? (object)DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Decimal,
                    Direction = ParameterDirection.Input,
                    ParameterName = "ToValue",
                    Value = searchModel.ToValue ?? (object)DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "OpportunityPercentage",
                    TypeName = "[dbo].[GuidList]",
                    Value = tableOpportunityPercentage
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "RolesCode",
                    TypeName = "[dbo].[StringList]",
                    Value = tableRolesCode
                    //Value = searchModel.RolesCode ?? (object)DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "CurrentCompanyCode",
                    Value = CurrentUser.CompanyCode,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "Contractor",
                    TypeName = "[dbo].[GuidList]",
                    Value = tableContractor
                    //Value = searchModel.Contractor ?? (object)DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.UniqueIdentifier,
                    Direction = ParameterDirection.Input,
                    ParameterName = "ProvinceId",
                    Value = searchModel.ProvinceId ?? (object)DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "DistrictId",
                    TypeName = "[dbo].[GuidList]",
                    Value = tableDistrictId
                    //Value = searchModel.DistrictId ?? (object)DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "PersonInChargeSpec", 
                    TypeName = "[dbo].[StringList]",
                    Value = tablePersonInChargeSpec
                    //Value = searchModel.PersonInChargeSpec ?? (object)DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "Competitor",
                    TypeName = "[dbo].[GuidList]",
                    Value = tableCompetitor
                    //Value = searchModel.Competitor ?? (object)DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Bit,
                    Direction = ParameterDirection.Input,
                    ParameterName = "IsTopInvestor",
                    Value = searchModel.IsTopInvestor ?? (object)DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.UniqueIdentifier,
                    Direction = ParameterDirection.Input,
                    ParameterName = "AccountId",
                    Value = CurrentUser.AccountId ?? (object)DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "OrderBy",
                    Value = searchModel.OrderBy ?? (object)DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "TypeSort",
                    Value = searchModel.TypeSort ?? (object)DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "OpportunityStatusType",
                    TypeName = "[dbo].[StringList]",
                    Value = tableOpportunityStatusType
                },
            };

            result = _context.Database.SqlQuery<OpportunityReportViewModel>(sqlQuery, parameters.ToArray()).ToList();
            return result;
        }
        #endregion

        [HttpPost]
        public ActionResult ExportPivot(OpportunityReportSearchViewModel searchViewModel, Guid? templateId)
        {
            PivotGridExportOptions options = new PivotGridExportOptions();
            options.ExportType = PivotGridExportFormats.Excel;
            options.WYSIWYG.PrintRowHeaders = true;
            options.WYSIWYG.PrintFilterHeaders = false;
            options.WYSIWYG.PrintDataHeaders = false;
            options.WYSIWYG.PrintColumnHeaders = false;

            var pivotTemplate = _unitOfWork.PivotGridTemplateRepository.GetTemplateById(templateId.Value);
            if (pivotTemplate != null)
            {
                searchViewModel.ReportType = pivotTemplate.TemplateName;
            }
            var model = GetData(searchViewModel);
            //get search value to display in file
            var filterDisplayList = GetSearchInfoToDisplay(searchViewModel);
            //var pivotTemplate = _unitOfWork.PivotGridTemplateRepository.GetTemplateById(templateId.Value);
            //get pivot config 
            //Lấy cột - thứ tự cột ... từ bảng SearchResultDetailTemplateModel
            var pivotSetting = _unitOfWork.PivotGridTemplateRepository.GetSettingByTemplate(templateId.Value).ToList();
            //Lấy thông tin config các thông số người dùng lưu template từ SearchResultTemplateModel.LayoutConfigs
            var template = _unitOfWork.PivotGridTemplateRepository.GetTemplateById(templateId.Value);
            var layoutConfigs = (string)Session[CurrentUser.AccountId + "LayoutConfigs"];

            try
            {
                string fileName = (pivotTemplate.TemplateName.Contains(".") ? pivotTemplate.TemplateName.Split('.')[1].ToLower() : pivotTemplate.TemplateName.ToLower());
                string fileNameWithFormat = string.Format("{0}", _unitOfWork.UtilitiesRepository.RemoveSign4VietnameseString(fileName.ToUpper()).Replace(" ", "_"));
                return PivotGridExportExcel.GetExportActionResult(fileNameWithFormat, options, pivotSetting, model, filterDisplayList, fileName, layoutConfigs, PagerSize: ePaperSize.A4, Scale: 70, Orientation: eOrientation.Landscape);

            }
            catch (Exception)
            {

                throw;
            }

        }
        public ActionResult ExportExcel(OpportunityReportSearchViewModel searchModel, Guid? templateId)
        {
            var pivotTemplate = _unitOfWork.PivotGridTemplateRepository.GetTemplateById(templateId.Value);
            if (pivotTemplate != null)
            {
                searchModel.ReportType = pivotTemplate.TemplateName;
            }
            var data = GetData(searchModel);
            //get search value to display in file
            var filterDisplayList = GetSearchInfoToDisplay(searchModel);
            return Export(data, filterDisplayList, templateId);
        }

        private FileContentResult Export(List<OpportunityReportViewModel> data, List<SearchDisplayModel> filters, Guid? templateId)
        {
            var pivotTemplate = _unitOfWork.PivotGridTemplateRepository.GetTemplateById(templateId.Value);
            List<ExcelTemplate> columns = new List<ExcelTemplate>();
            //Header
            string fileheader = "Danh sách dự án";
            if (pivotTemplate != null)
            {
                #region Header
                fileheader = fileheader + (pivotTemplate.TemplateName.Contains(".") ? pivotTemplate.TemplateName.Split('.')[1].ToLower() : pivotTemplate.TemplateName.ToLower());
                #endregion

                #region Column
                if (pivotTemplate.TemplateName.ToLower().Contains("chủ đầu tư"))
                {
                    data = data.GroupBy(p => 
                                    new { 
                                        p.InvestorName, 
                                        p.OpportunityName,
                                        p.OpportunityPercentage, 
                                        p.ProjectGabarit,
                                        p.OpportunityUnit, 
                                        p.ProvinceName,   
                                        p.ProjectValue, 
                                        p.CompleteYear 
                                    })
                        .Select(p => new OpportunityReportViewModel()
                                {
                                    InvestorName = p.Key.InvestorName,
                                    OpportunityName = p.Key.OpportunityName,
                                    OpportunityPercentage = p.Key.OpportunityPercentage,
                                    ProjectGabarit = p.Key.ProjectGabarit,
                                    OpportunityUnit = p.Key.OpportunityUnit,
                                    ProvinceName = p.Key.ProvinceName,
                                    ProjectValue = p.Key.ProjectValue,
                                    CompleteYear = p.Key.CompleteYear,
                                }).OrderBy(s => s.InvestorName).ToList();
                    columns.Add(new ExcelTemplate { ColumnName = "CDT_InvestorName", isAllowedToEdit = false });
                    columns.Add(new ExcelTemplate { ColumnName = "CDT_OpportunityName", isAllowedToEdit = false });
                    columns.Add(new ExcelTemplate { ColumnName = "CDT_OpportunityPercentage", isAllowedToEdit = false });
                    columns.Add(new ExcelTemplate { ColumnName = "CDT_ProjectGabarit", isAllowedToEdit = false });
                    columns.Add(new ExcelTemplate { ColumnName = "CDT_OpportunityUnit", isAllowedToEdit = false });
                    columns.Add(new ExcelTemplate { ColumnName = "CDT_ProvinceName", isAllowedToEdit = false });
                    columns.Add(new ExcelTemplate { ColumnName = "CDT_ProjectValue", isAllowedToEdit = false, isCurrency = true });
                    columns.Add(new ExcelTemplate { ColumnName = "CDT_CompleteYear", isAllowedToEdit = false });
                }
                else if (pivotTemplate.TemplateName.ToLower().Contains("thiết kế"))
                {
                    data = data.GroupBy(p => 
                                    new { 
                                        p.DesignName, 
                                        p.OpportunityName, 
                                        p.OpportunityPercentage, 
                                        p.SalesEmployeeName1,
                                        p.SalesEmployeeName2,
                                        p.ProjectGabarit,
                                        p.OpportunityUnit,
                                        p.ProvinceName,
                                        p.Spec,
                                    })
                        .Select(p => new OpportunityReportViewModel()
                                {
                                    DesignName = p.Key.DesignName,
                                    OpportunityName = p.Key.OpportunityName,
                                    OpportunityPercentage = p.Key.OpportunityPercentage,
                                    SalesEmployeeName1 = p.Key.SalesEmployeeName1,
                                    SalesEmployeeName2 = p.Key.SalesEmployeeName2,
                                    ProjectGabarit = p.Key.ProjectGabarit,
                                    OpportunityUnit = p.Key.OpportunityUnit,
                                    ProvinceName = p.Key.ProvinceName,
                                    Spec = p.Key.Spec
                                 }).OrderBy(s => s.DesignName).ToList();
                    columns.Add(new ExcelTemplate { ColumnName = "DS_DesignName", isAllowedToEdit = false });
                    columns.Add(new ExcelTemplate { ColumnName = "DS_OpportunityName", isAllowedToEdit = false });
                    columns.Add(new ExcelTemplate { ColumnName = "DS_OpportunityPercentage", isAllowedToEdit = false });
                    columns.Add(new ExcelTemplate { ColumnName = "DS_SalesEmployeeName1", isAllowedToEdit = false });
                    columns.Add(new ExcelTemplate { ColumnName = "DS_SaleSpec", isAllowedToEdit = false });
                    columns.Add(new ExcelTemplate { ColumnName = "DS_ProjectGabarit", isAllowedToEdit = false });
                    columns.Add(new ExcelTemplate { ColumnName = "DS_OpportunityUnit", isAllowedToEdit = false });
                    columns.Add(new ExcelTemplate { ColumnName = "DS_ProvinceName", isAllowedToEdit = false });
                    columns.Add(new ExcelTemplate { ColumnName = "DS_GTSpec", isAllowedToEdit = false });
                }
                else if (pivotTemplate.TemplateName.ToLower().Contains("tổng thầu"))
                {
                    data = data.GroupBy(p => new {
                                    p.Contractor,
                                    p.OpportunityName,
                                    p.OpportunityPercentage,
                                    p.SalesEmployeeName1,
                                    p.SalesEmployeeName2,
                                    p.ProjectGabarit,
                                    p.OpportunityUnit,
                                    p.Construction,
                                    p.CompleteYear
                    })
                                .Select(p => new OpportunityReportViewModel()
                                {
                                    Contractor = p.Key.Contractor,
                                    OpportunityName = p.Key.OpportunityName,
                                    OpportunityPercentage = p.Key.OpportunityPercentage,
                                    SalesEmployeeName1 = p.Key.SalesEmployeeName1,
                                    ProjectGabarit = p.Key.ProjectGabarit,
                                    OpportunityUnit = p.Key.OpportunityUnit,
                                    Construction = p.Key.Construction,
                                    CompleteYear = p.Key.CompleteYear
                                }).OrderBy(s => s.DesignName).ToList();
                    columns.Add(new ExcelTemplate { ColumnName = "CR_ContractorName", isAllowedToEdit = false });
                    columns.Add(new ExcelTemplate { ColumnName = "CR_OpportunityName", isAllowedToEdit = false });
                    columns.Add(new ExcelTemplate { ColumnName = "CR_OpportunityPercentage", isAllowedToEdit = false });
                    columns.Add(new ExcelTemplate { ColumnName = "CR_SalesEmployeeName1", isAllowedToEdit = false });
                    columns.Add(new ExcelTemplate { ColumnName = "CR_ProjectGabarit", isAllowedToEdit = false });
                    columns.Add(new ExcelTemplate { ColumnName = "CR_OpportunityUnit", isAllowedToEdit = false });
                    columns.Add(new ExcelTemplate { ColumnName = "CR_Construction", isAllowedToEdit = false });
                    columns.Add(new ExcelTemplate { ColumnName = "CR_CompleteYear", isAllowedToEdit = false });
                }
                else if (pivotTemplate.TemplateName.ToLower().Contains("đối thủ"))
                {
                    data = data.GroupBy(p => new {
                        p.CompetitorName,
                        p.Address,
                        p.Phone,
                        p.Website,
                        p.Category,
                        p.Number1,
                        p.Number2,
                    })
                                .Select(p => new OpportunityReportViewModel()
                                {
                                    CompetitorName = p.Key.CompetitorName,
                                    Address = p.Key.Address,
                                    Phone = p.Key.Phone,
                                    Website = p.Key.Website,
                                    Category = p.Key.Category,
                                    Number1 = p.Key.Number1,
                                    Number2 = p.Key.Number2,
                                }).OrderBy(s => s.DesignName).ToList();
                    columns.Add(new ExcelTemplate { ColumnName = "CP_CompetitorName", isAllowedToEdit = false });
                    columns.Add(new ExcelTemplate { ColumnName = "CP_Address", isAllowedToEdit = false });
                    columns.Add(new ExcelTemplate { ColumnName = "CP_Phone", isAllowedToEdit = false });
                    columns.Add(new ExcelTemplate { ColumnName = "CP_Website", isAllowedToEdit = false });
                    columns.Add(new ExcelTemplate { ColumnName = "CP_Category", isAllowedToEdit = false });
                    columns.Add(new ExcelTemplate { ColumnName = "CP_Number1", isAllowedToEdit = false });
                    columns.Add(new ExcelTemplate { ColumnName = "CP_Number2", isAllowedToEdit = false });
                }
                else if (pivotTemplate.TemplateName.ToLower().Contains("khu vực"))
                {
                    data = data.GroupBy(p => new { p.DesignName, p.OpportunityName, p.InvestorName, p.ProvinceName, p.ProjectValue })
                                .Select(p => new OpportunityReportViewModel()
                                {
                                    DesignName = p.Key.DesignName,
                                    OpportunityName = p.Key.OpportunityName,
                                    InvestorName = p.Key.InvestorName,
                                    ProvinceName = p.Key.ProvinceName,
                                    ProjectValue = p.Key.ProjectValue
                                }).OrderBy(s => s.ProvinceName).ToList();
                    columns.Add(new ExcelTemplate { ColumnName = "P_ProvinceName", isAllowedToEdit = false });
                    columns.Add(new ExcelTemplate { ColumnName = "P_OpportunityName", isAllowedToEdit = false });
                    columns.Add(new ExcelTemplate { ColumnName = "P_InvestorName", isAllowedToEdit = false });
                    columns.Add(new ExcelTemplate { ColumnName = "P_DesignName", isAllowedToEdit = false });
                    columns.Add(new ExcelTemplate { ColumnName = "P_ProjectValue", isAllowedToEdit = false, isCurrency = true });
                }
                else if (pivotTemplate.TemplateName.ToLower().Contains("sale") || pivotTemplate.TemplateName.Contains("spec"))
                {
                    data = data.GroupBy(p => new { p.DesignName, p.OpportunityName, p.InvestorName, p.SalesEmployeeName1, p.SalesEmployeeName2, p.SpecDescription, p.OpportunityStatus, p.Spec, p.Construction })
                                .Select(p => new OpportunityReportViewModel()
                                {
                                    DesignName = p.Key.DesignName,
                                    OpportunityName = p.Key.OpportunityName,
                                    InvestorName = p.Key.InvestorName,
                                    SalesEmployeeName1 = p.Key.SalesEmployeeName1,
                                    SalesEmployeeName2 = p.Key.SalesEmployeeName2,
                                    SpecDescription = p.Key.SpecDescription,
                                    OpportunityStatus = p.Key.OpportunityStatus,
                                    Spec = p.Key.Spec,
                                    Construction = p.Key.Construction,
                                }).OrderBy(s => s.SaleSpec).ToList();
                    columns.Add(new ExcelTemplate { ColumnName = "S_SaleSpec", isAllowedToEdit = false });
                    columns.Add(new ExcelTemplate { ColumnName = "S_OpportunityName", isAllowedToEdit = false });
                    columns.Add(new ExcelTemplate { ColumnName = "S_InvestorName", isAllowedToEdit = false });
                    columns.Add(new ExcelTemplate { ColumnName = "S_DesignName", isAllowedToEdit = false });
                    columns.Add(new ExcelTemplate { ColumnName = "S_Spec", isAllowedToEdit = false, isWraptext = true });
                    //columns.Add(new ExcelTemplate { ColumnName = "S_Construction", isAllowedToEdit = false, isWraptext = true });
                    columns.Add(new ExcelTemplate { ColumnName = "S_OpportunityStatus", isAllowedToEdit = false });
                    columns.Add(new ExcelTemplate { ColumnName = "S_GTSpec", isAllowedToEdit = false, isCurrency = true });
                    columns.Add(new ExcelTemplate { ColumnName = "S_Construction", isAllowedToEdit = false, isCurrency = true });
                }
                else if (pivotTemplate.TemplateName.ToLower().Contains("xác suất"))
                {
                    data = data.GroupBy(p => new { p.OpportunityName, p.InvestorName, p.OpportunityPercentage, p.ProjectValue })
                               .Select(p => new OpportunityReportViewModel()
                               {
                                   OpportunityName = p.Key.OpportunityName,
                                   InvestorName = p.Key.InvestorName,
                                   OpportunityPercentage = p.Key.OpportunityPercentage,
                                   ProjectValue = p.Key.ProjectValue,
                               }).OrderBy(s => s.OpportunityPercentage).ToList();
                    columns.Add(new ExcelTemplate { ColumnName = "OP_OpportunityPercentage", isAllowedToEdit = false });
                    columns.Add(new ExcelTemplate { ColumnName = "OP_OpportunityName", isAllowedToEdit = false });
                    columns.Add(new ExcelTemplate { ColumnName = "OP_InvestorName", isAllowedToEdit = false });
                    columns.Add(new ExcelTemplate { ColumnName = "OP_ProjectValue", isAllowedToEdit = false, isCurrency = true });
                }
                else if (pivotTemplate.TemplateName.ToLower().Contains("phụ kiện"))
                {
                    //data = data.OrderBy(s => s.AccessoryList).ToList();
                    columns.Add(new ExcelTemplate { ColumnName = "A_AccessoryList", isAllowedToEdit = false });
                    columns.Add(new ExcelTemplate { ColumnName = "A_OpportunityName", isAllowedToEdit = false });
                    columns.Add(new ExcelTemplate { ColumnName = "A_InvestorName", isAllowedToEdit = false });
                    //columns.Add(new ExcelTemplate { ColumnName = "A_DesignName", isAllowedToEdit = false });
                    columns.Add(new ExcelTemplate { ColumnName = "A_AccessoryValue", isAllowedToEdit = false, isCurrency = true });
                }
                else if (pivotTemplate.TemplateName.ToLower().Contains("giá trị"))
                {
                    data = data.GroupBy(p => new { p.OpportunityName, p.InvestorName, p.ProjectValue })
                               .Select(p => new OpportunityReportViewModel()
                               {
                                   OpportunityName = p.Key.OpportunityName,
                                   InvestorName = p.Key.InvestorName,
                                   ProjectValue = p.Key.ProjectValue,
                               }).OrderBy(s => s.InvestorName).ToList();
                    columns.Add(new ExcelTemplate { ColumnName = "GT_OpportunityName", isAllowedToEdit = false });
                    columns.Add(new ExcelTemplate { ColumnName = "GT_InvestorName", isAllowedToEdit = false });
                    columns.Add(new ExcelTemplate { ColumnName = "GT_ProjectValue", isAllowedToEdit = false, isCurrency = true });
                }
                else if (pivotTemplate.TemplateName.ToLower().Contains("quy mô"))
                {
                    data = data.GroupBy(p => new { p.OpportunityName, p.InvestorName, p.ProjectGabarit })
                               .Select(p => new OpportunityReportViewModel()
                               {
                                   OpportunityName = p.Key.OpportunityName,
                                   InvestorName = p.Key.InvestorName,
                                   ProjectGabarit = p.Key.ProjectGabarit,
                               }).OrderBy(s => s.ProjectGabarit).ToList();
                    columns.Add(new ExcelTemplate { ColumnName = "QM_ProjectGabarit", isAllowedToEdit = false });
                    columns.Add(new ExcelTemplate { ColumnName = "QM_OpportunityName", isAllowedToEdit = false });
                    columns.Add(new ExcelTemplate { ColumnName = "QM_InvestorName", isAllowedToEdit = false });
                }
                else if (pivotTemplate.TemplateName.ToLower().Contains("nhóm hàng"))
                {
                    //data = data.OrderBy(s => s.AccessoryList).ToList();
                    columns.Add(new ExcelTemplate { ColumnName = "NH_Category", isAllowedToEdit = false });
                    columns.Add(new ExcelTemplate { ColumnName = "NH_OpportunityName", isAllowedToEdit = false });
                    columns.Add(new ExcelTemplate { ColumnName = "NH_InvestorName", isAllowedToEdit = false });
                    columns.Add(new ExcelTemplate { ColumnName = "NH_PercentProjectValue", isAllowedToEdit = false});
                    columns.Add(new ExcelTemplate { ColumnName = "NH_AccessoryValue", isAllowedToEdit = false, isCurrency = true });
                }
                else if (pivotTemplate.TemplateName.Contains("hoàn thiện"))
                {
                    data = data.GroupBy(p => new { p.CompleteYear, p.OpportunityName, p.InvestorName, p.ProjectValue })
                               .Select(p => new OpportunityReportViewModel()
                               {
                                   CompleteYear = p.Key.CompleteYear,
                                   OpportunityName = p.Key.OpportunityName,
                                   InvestorName = p.Key.InvestorName,
                                   ProjectValue = p.Key.ProjectValue,
                               }).OrderBy(s => s.ProjectGabarit).ToList();
                    columns.Add(new ExcelTemplate { ColumnName = "CY_CompleteYear", isAllowedToEdit = false });
                    columns.Add(new ExcelTemplate { ColumnName = "CY_OpportunityName", isAllowedToEdit = false });
                    columns.Add(new ExcelTemplate { ColumnName = "CY_InvestorName", isAllowedToEdit = false });
                    columns.Add(new ExcelTemplate { ColumnName = "CY_ProjectValue", isAllowedToEdit = false });
                }
                #endregion
            }

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
            byte[] filecontent = ClassExportExcel.ExportExcel(data, columns, heading, true, HasExtraSheet: false);
            string fileNameWithFormat = string.Format("{0}.xlsx", _unitOfWork.UtilitiesRepository.RemoveSign4VietnameseString(fileheader.ToUpper()).Replace(" ", "_"));

            return File(filecontent, ClassExportExcel.ExcelContentType, fileNameWithFormat);
        }

        #region Lấy điều kiện lọc
        private List<SearchDisplayModel> GetSearchInfoToDisplay(OpportunityReportSearchViewModel searchViewModel)
        {
            List<SearchDisplayModel> filterList = new List<SearchDisplayModel>();
            if (searchViewModel.Investor != null && searchViewModel.Investor.Count() > 0)
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Chủ đầu tư";
                foreach (var item in searchViewModel.Investor)
                {
                    var value = _context.ProfileModel.FirstOrDefault(s => s.ProfileId == item).ProfileName;
                    filter.DisplayValue = value;
                    filterList.Add(filter);
                }
             
            }
            if (searchViewModel.Designer != null && searchViewModel.Designer.Count() > 0)
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Thiết kế";
                foreach (var item in searchViewModel.Designer)
                {
                    var value = _context.ProfileModel.FirstOrDefault(s => s.ProfileId == item)?.ProfileName;
                    filter.DisplayValue = value;
                    filterList.Add(filter);
                }
            }
            if (searchViewModel.SaleOfficeCode != null && searchViewModel.SaleOfficeCode.Count() > 0)
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Khu vực";
                foreach (var item in searchViewModel.SaleOfficeCode)
                {
                    var value = _context.CatalogModel.FirstOrDefault(s => s.CatalogCode == item && s.CatalogTypeCode == ConstCatalogType.SaleOffice)?.CatalogText_vi;
                    filter.DisplayValue = value;
                    filterList.Add(filter);
                }
            }
            if (!string.IsNullOrEmpty(searchViewModel.ProjectLocation))
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Khu vực";

                filter.DisplayValue = searchViewModel.ProjectLocation;
                filterList.Add(filter);
            }
            if (!string.IsNullOrEmpty(searchViewModel.SaleSpecCode))
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Tên Sale/spec";
                var sale = _context.SalesEmployeeModel.FirstOrDefault(s => s.SalesEmployeeCode == searchViewModel.SaleSpecCode);
                var value = sale?.SalesEmployeeCode + " | " + sale?.SalesEmployeeName;
                filter.DisplayValue = value;
                filterList.Add(filter);
            }
            if (searchViewModel.Opportunity != null && searchViewModel.Opportunity.Count() > 0)
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Tên dự án";
                foreach (var item in searchViewModel.Opportunity)
                {
                    var value = _context.ProfileModel.FirstOrDefault(s => s.ProfileId == item).ProfileName;
                    filter.DisplayValue = value;
                    filterList.Add(filter);
                }
            }
            return filterList;
        }
        #endregion

        #region In báo cáo
        [HttpPost]
        public ActionResult Print(OpportunityReportSearchViewModel searchViewModel, Guid? templateId)
        {
            PivotGridExportOptions options = new PivotGridExportOptions();
            options.ExportType = PivotGridExportFormats.Pdf;

            var pivotTemplate = _unitOfWork.PivotGridTemplateRepository.GetTemplateById(templateId.Value);
            if (pivotTemplate != null)
            {
                searchViewModel.ReportType = pivotTemplate.TemplateName;
            }
            var model = GetData(searchViewModel);
            //get search value to display in file
            var filterDisplayList = GetSearchInfoToDisplay(searchViewModel);
            var pivotSetting = _unitOfWork.PivotGridTemplateRepository.GetSettingByTemplate(templateId.Value).ToList();
            var layoutConfigs = (string)Session[CurrentUser.AccountId + "LayoutConfigs"];
            try
            {
                string fileName = (pivotTemplate.TemplateName.Contains(".") ? pivotTemplate.TemplateName.Split('.')[1].ToLower() : pivotTemplate.TemplateName.ToLower());
                string fileNameWithFormat = string.Format("{0}", _unitOfWork.UtilitiesRepository.RemoveSign4VietnameseString(fileName.ToUpper()).Replace(" ", "_"));
                return PivotGridExportExcel.GetExportActionResult(fileNameWithFormat, options, pivotSetting, model, filterDisplayList, fileName, layoutConfigs);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}