using ISD.Constant;
using ISD.Core;
using ISD.Extensions;
using ISD.Repositories.Excel;
using ISD.ViewModels;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Reports.Controllers
{
    public class ProductWarrantyReportController : BaseController
    {
        // GET: ProductWarrantyReport
        [ISDAuthorizationAttribute]
        public ActionResult Index()
        {
            var searchModel = (ProductWarrantyReportSearchModel)TempData[CurrentUser.AccountId + "ProductWarrantySearchData"];
            var tempalteIdString = TempData[CurrentUser.AccountId + "ProductWarrantyTemplateId"];
            var modeSearch = TempData[CurrentUser.AccountId + "ProductWarrantyModeSearch"];
            //mode search
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
            var pageId = GetPageId("/Reports/ProductWarrantyReport");
            // search data
            if (searchModel == null || searchModel.IsView != true)
            {
                ViewBag.Search = null;
            }
            else
            {
                ViewBag.Search = searchModel;
            }
            //get list template
            var listSystemTemplate = _unitOfWork.PivotGridTemplateRepository.GetSystemTemplate(pageId);
            var listUserTemplate = _unitOfWork.PivotGridTemplateRepository.GetUserTemplate(pageId, CurrentUser.AccountId.Value);
            //get pivot setting
            List<FieldSettingModel> pivotSetting = new List<FieldSettingModel>();
            //nếu đang có template đang xem
            if (templateId != Guid.Empty && templateId != null)
            {

                pivotSetting = _unitOfWork.PivotGridTemplateRepository.GetSettingByTemplate(templateId);
                ViewBag.PivotSetting = pivotSetting;
                ViewBag.TemplateId = templateId;
            }
            else
            {
                var userDefaultTemplate = listUserTemplate.FirstOrDefault(s => s.IsDefault == true);
                //nếu ko có template đang xem thì lấy default của user
                if (userDefaultTemplate != null)
                {
                    pivotSetting = _unitOfWork.PivotGridTemplateRepository.GetSettingByTemplate(userDefaultTemplate.SearchResultTemplateId);
                    ViewBag.PivotSetting = pivotSetting;
                    ViewBag.TemplateId = userDefaultTemplate.SearchResultTemplateId;
                }
                else
                {
                    var sysDefaultTemplate = listSystemTemplate.FirstOrDefault(s => s.IsDefault == true);
                    //nếu user không có template thì lấy default của hệ thống
                    if (sysDefaultTemplate != null)
                    {
                        pivotSetting = _unitOfWork.PivotGridTemplateRepository.GetSettingByTemplate(sysDefaultTemplate.SearchResultTemplateId);
                        ViewBag.PivotSetting = pivotSetting;
                        ViewBag.TemplateId = sysDefaultTemplate.SearchResultTemplateId;
                    }
                    else // nếu tất cả đều không có thì render default partial view
                    {
                        ViewBag.PivotSetting = null;
                        ViewBag.TemplateId = templateId;
                    }
                }
            }
            #region //Get list ServiceTechnicalTeamCode (Trung tâm bảo hành)
            var serviceTechnicalTeamCodeList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.ServiceTechnicalTeam);
            ViewBag.ServiceTechnicalTeamCode = new SelectList(serviceTechnicalTeamCodeList, "CatalogCode", "CatalogText_vi");
            #endregion
            #region //Get list DepartmentCode (Phòng ban)
            var TaskRolesList = _unitOfWork.AccountRepository.GetRolesList(isEmployeeGroup: true).Where(x => x.RolesCode.Contains(CurrentUser.CompanyCode)).ToList();
            ViewBag.DepartmentCode = new SelectList(TaskRolesList, "RolesCode", "RolesName");
            #endregion

            ViewBag.PageId = pageId;
            ViewBag.SystemTemplate = listSystemTemplate;
            ViewBag.UserTemplate = listUserTemplate;
            CreateViewBag(searchModel);
            return View();
        }

        private void CreateViewBag(ProductWarrantyReportSearchModel searchModel)
        {

            if (searchModel == null)
            {
                searchModel = new ProductWarrantyReportSearchModel();
            }

            #region CommonDate
            //Common Date
            var commonDateList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CommonDate);
            ViewBag.FromCommonDate = new SelectList(commonDateList, "CatalogCode", "CatalogText_vi", searchModel?.FromCommonDate ?? "ThisMonth");

            //Common Date 2
            ViewBag.ToCommonDate = new SelectList(commonDateList, "CatalogCode", "CatalogText_vi", searchModel?.ToCommonDate);
            #endregion


            ViewBag.ERPProductCodeList = searchModel.ERPProductCode;
            ViewBag.SerialNumberList = searchModel.SerialNumber;
            ViewBag.SaleOrderList = searchModel.SaleOrder;
            ViewBag.OrderDeliveryList = searchModel.OrderDelivery;
            ViewBag.ProfileCodeList = searchModel.ProfileCode;

            #region CompanyId
            var companyList = _unitOfWork.CompanyRepository.GetAll();
            ViewBag.CompanyId = new SelectList(companyList, "CompanyId", "CompanyName", CurrentUser.CompanyId);
            #endregion

            #region Province (Tỉnh/Thành phố)
            //Load Tỉnh thành theo Khu vực (sắp xếp theo thứ tự các tỉnh thuộc khu vực chọn sẽ được xếp trước)
            var provinceAreaList = _context.ProvinceModel.Where(p => p.Actived == true)
                                           .Select(p => new ProvinceViewModel()
                                           {
                                               ProvinceId = p.ProvinceId,
                                               ProvinceCode = p.ProvinceCode,
                                               ProvinceName = p.ProvinceName,
                                               Area = p.Area,
                                               OrderIndex = p.OrderIndex
                                           }).ToList();

            provinceAreaList = provinceAreaList.OrderBy(p => p.ProvinceCode).OrderByDescending(p => p.ProvinceName == "Hồ Chí Minh").ThenByDescending(p => p.ProvinceName == "Hà Nội").ToList();
            ViewBag.ProvinceId = new SelectList(provinceAreaList, "ProvinceId", "ProvinceName");
            #endregion

            #region District (Quận/Huyện)
            ViewBag.DistrictId = new SelectList(new List<DistrictViewModel>(), "DistrictId", "DistrictName");
            if (searchModel != null && searchModel.ProvinceId != null && searchModel.ProvinceId.Count() > 0)
            {
                var districtLst = (from p in _context.DistrictModel
                                   join c in _context.ProvinceModel on p.ProvinceId equals c.ProvinceId
                                   join pr in searchModel.ProvinceId on c.ProvinceId equals pr
                                   where p.Actived == true
                                   && p.ProvinceId != null
                                   orderby c.OrderIndex, p.OrderIndex
                                   select new DistrictViewModel()
                                   {
                                       DistrictId = p.DistrictId,
                                       DistrictName = c.ProvinceName + " | " + p.Appellation + " " + p.DistrictName
                                   }).ToList();
                ViewBag.DistrictId = new MultiSelectList(districtLst, "DistrictId", "DistrictName");
            }
            #endregion

            #region Ward (Phường/Xã)
            ViewBag.WardId = new SelectList(new List<WardViewModel>(), "WardId", "WardName");
            if (searchModel != null && searchModel.DistrictId != null && searchModel.DistrictId.Count() > 0)
            {
                var WardLst = (from p in _context.WardModel
                               join c in _context.DistrictModel on p.DistrictId equals c.DistrictId
                               join pr in searchModel.DistrictId on c.DistrictId equals pr
                               where p.DistrictId != null
                               orderby c.OrderIndex, p.OrderIndex
                               select new WardViewModel()
                               {
                                   WardId = p.WardId,
                                   WardName = c.DistrictName + " | " + p.Appellation + " " + p.WardName
                               }).ToList();
                ViewBag.WardId = new MultiSelectList(WardLst, "WardId", "WardName");
            }
            #endregion

            #region WarrantyCode
            var warrantyCodeList = _context.WarrantyModel.OrderBy(x => x.WarrantyCode).Select(x => new { x.WarrantyCode, x.WarrantyName }).ToList();
            ViewBag.WarrantyCode = new MultiSelectList(warrantyCodeList, "WarrantyCode", "WarrantyName");
            #endregion

        }

        public ActionResult ExportExcel(ProductWarrantyReportSearchModel searchModel)
        {

            var data = GetData(searchModel);
            //get search value to display in file
            var filterDisplayList = GetSearchInfoToDisplay(searchModel);
            return Export(data, filterDisplayList);
        }

        private FileContentResult Export(List<ProductWarrantyReportExcelModel> viewModel, List<SearchDisplayModel> filters)
        {
            List<ExcelTemplate> columns = new List<ExcelTemplate>();

            columns.Add(new ExcelTemplate { ColumnName = "WorkFlowName", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "ERPProductCode", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "ProductName", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "ProductQuantity", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "ProductCategoryName", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "ErrorName", isAllowedToEdit = false });

            //Header
            string fileheader = "Báo cáo Sản phẩm bảo hành";

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

        private List<ProductWarrantyReportExcelModel> GetData(ProductWarrantyReportSearchModel searchModel)
        {
            #region ProvinceId
            //Build your record
            var tableProvinceIdSchema = new List<SqlMetaData>(1)
            {
                new SqlMetaData("Id", SqlDbType.UniqueIdentifier)
            }.ToArray();

            //And a table as a list of those records
            var tableProvinceId = new List<SqlDataRecord>();
            if (searchModel.ProvinceId != null && searchModel.ProvinceId.Count > 0)
            {
                foreach (var r in searchModel.ProvinceId)
                {
                    var tableRow = new SqlDataRecord(tableProvinceIdSchema);
                    tableRow.SetSqlGuid(0, r);
                    tableProvinceId.Add(tableRow);
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tableProvinceIdSchema);
                tableProvinceId.Add(tableRow);
            }
            #endregion

            #region DistrictId
            //Build your record
            var tableDistrictIdSchema = new List<SqlMetaData>(1)
            {
                new SqlMetaData("Id", SqlDbType.UniqueIdentifier)
            }.ToArray();

            //And a table as a list of those records
            var tableDistrictId = new List<SqlDataRecord>();
            if (searchModel.DistrictId != null && searchModel.DistrictId.Count > 0)
            {
                foreach (var r in searchModel.DistrictId)
                {
                    var tableRow = new SqlDataRecord(tableDistrictIdSchema);
                    tableRow.SetSqlGuid(0, r);
                    tableDistrictId.Add(tableRow);
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tableDistrictIdSchema);
                tableDistrictId.Add(tableRow);
            }
            #endregion

            #region WardId
            //Build your record
            var tableWardIdSchema = new List<SqlMetaData>(1)
            {
                new SqlMetaData("Id", SqlDbType.UniqueIdentifier)
            }.ToArray();

            //And a table as a list of those records
            var tableWardId = new List<SqlDataRecord>();
            if (searchModel.WardId != null && searchModel.WardId.Count > 0)
            {
                foreach (var r in searchModel.WardId)
                {
                    var tableRow = new SqlDataRecord(tableWardIdSchema);
                    tableRow.SetSqlGuid(0, r);
                    tableWardId.Add(tableRow);
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tableWardIdSchema);
                tableWardId.Add(tableRow);
            }
            #endregion

            #region ERPProductCode
            var tableERPProductCodeSchema = new List<SqlMetaData>(1)
                {
                    new SqlMetaData("Code", SqlDbType.NVarChar, 100)
                }.ToArray();

            //And a table as a list of those records
            var tableERPProductCode = new List<SqlDataRecord>();
            List<string> RPProductCodeLst = new List<string>();
            if (searchModel.ERPProductCode != null && searchModel.ERPProductCode.Count > 0)
            {
                foreach (var r in searchModel.ERPProductCode)
                {
                    var tableRow = new SqlDataRecord(tableERPProductCodeSchema);
                    tableRow.SetString(0, r);
                    if (!RPProductCodeLst.Contains(r))
                    {
                        RPProductCodeLst.Add(r);
                        tableERPProductCode.Add(tableRow);
                    }
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tableERPProductCodeSchema);
                tableERPProductCode.Add(tableRow);
            }
            #endregion

            #region SerialNumber
            var tableSerialNumberSchema = new List<SqlMetaData>(1)
                {
                    new SqlMetaData("Code", SqlDbType.NVarChar, 100)
                }.ToArray();

            //And a table as a list of those records
            var tableSerialNumber = new List<SqlDataRecord>();
            List<string> SerialNumberLst = new List<string>();
            if (searchModel.SerialNumber != null && searchModel.SerialNumber.Count > 0)
            {
                foreach (var r in searchModel.SerialNumber)
                {
                    var tableRow = new SqlDataRecord(tableSerialNumberSchema);
                    tableRow.SetString(0, r);
                    if (!SerialNumberLst.Contains(r))
                    {
                        SerialNumberLst.Add(r);
                        tableSerialNumber.Add(tableRow);
                    }
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tableSerialNumberSchema);
                tableSerialNumber.Add(tableRow);
            }
            #endregion

            #region SaleOrder
            var tableSaleOrderSchema = new List<SqlMetaData>(1)
                {
                    new SqlMetaData("Code", SqlDbType.NVarChar, 100)
                }.ToArray();

            //And a table as a list of those records
            var tableSaleOrder = new List<SqlDataRecord>();
            List<string> SaleOrderLst = new List<string>();
            if (searchModel.SaleOrder != null && searchModel.SaleOrder.Count > 0)
            {
                foreach (var r in searchModel.SaleOrder)
                {
                    var tableRow = new SqlDataRecord(tableSaleOrderSchema);
                    tableRow.SetString(0, r);
                    if (!SaleOrderLst.Contains(r))
                    {
                        SaleOrderLst.Add(r);
                        tableSaleOrder.Add(tableRow);
                    }
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tableSaleOrderSchema);
                tableSaleOrder.Add(tableRow);
            }
            #endregion

            #region OrderDelivery
            var tableOrderDeliverySchema = new List<SqlMetaData>(1)
                {
                    new SqlMetaData("Code", SqlDbType.NVarChar, 100)
                }.ToArray();

            //And a table as a list of those records
            var tableOrderDelivery = new List<SqlDataRecord>();
            List<string> OrderDeliveryLst = new List<string>();
            if (searchModel.OrderDelivery != null && searchModel.OrderDelivery.Count > 0)
            {
                foreach (var r in searchModel.OrderDelivery)
                {
                    var tableRow = new SqlDataRecord(tableOrderDeliverySchema);
                    tableRow.SetString(0, r);
                    if (!OrderDeliveryLst.Contains(r))
                    {
                        OrderDeliveryLst.Add(r);
                        tableOrderDelivery.Add(tableRow);
                    }
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tableOrderDeliverySchema);
                tableOrderDelivery.Add(tableRow);
            }
            #endregion

            #region ProfileCode
            var tableProfileCodeSchema = new List<SqlMetaData>(1)
                {
                    new SqlMetaData("Code", SqlDbType.NVarChar, 100)
                }.ToArray();

            //And a table as a list of those records
            var tableProfileCode = new List<SqlDataRecord>();
            List<string> ProfileCodeLst = new List<string>();
            if (searchModel.ProfileCode != null && searchModel.ProfileCode.Count > 0)
            {
                foreach (var r in searchModel.ProfileCode)
                {
                    var tableRow = new SqlDataRecord(tableProfileCodeSchema);
                    tableRow.SetString(0, r);
                    if (!ProfileCodeLst.Contains(r))
                    {
                        ProfileCodeLst.Add(r);
                        tableProfileCode.Add(tableRow);
                    }
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tableProfileCodeSchema);
                tableProfileCode.Add(tableRow);
            }
            #endregion

            #region WarrantyCode
            var tableWarrantyCodeSchema = new List<SqlMetaData>(1)
                {
                    new SqlMetaData("Code", SqlDbType.NVarChar, 100)
                }.ToArray();

            //And a table as a list of those records
            var tableWarrantyCode = new List<SqlDataRecord>();
            List<string> WarrantyCodeLst = new List<string>();
            if (searchModel.WarrantyCode != null && searchModel.WarrantyCode.Count > 0)
            {
                foreach (var r in searchModel.WarrantyCode)
                {
                    var tableRow = new SqlDataRecord(tableWarrantyCodeSchema);
                    tableRow.SetString(0, r);
                    if (!WarrantyCodeLst.Contains(r))
                    {
                        WarrantyCodeLst.Add(r);
                        tableWarrantyCode.Add(tableRow);
                    }
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tableWarrantyCodeSchema);
                tableWarrantyCode.Add(tableRow);
            }
            #endregion

            string sqlQuery = "EXEC [Report].[usp_ProductWarrantyReport] @CompanyId, @Actived, @ProfileName, @ProfileCode,@Address,@Phone ,@ProvinceId,@DistrictId,@WardId,@ProductName,@ERPProductCode,@SerialNumber,@SaleOrder,@OrderDelivery,@WarrantyCode,@StartFromDate,@EndFromDate ,@StartToDate ,@EndToDate ";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {

                new SqlParameter
                {
                    SqlDbType = SqlDbType.UniqueIdentifier,
                    Direction = ParameterDirection.Input,
                    ParameterName = "CompanyId",
                    Value = searchModel.CompanyId ?? (object)DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Bit,
                    Direction = ParameterDirection.Input,
                    ParameterName = "Actived",
                    Value = searchModel.Actived ?? (object)DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "ProfileName",
                    Value = searchModel.ProfileName ?? (object)DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "ProfileCode",
                    TypeName = "[dbo].[StringList]", //Don't forget this one!
                    Value = tableProfileCode,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "Address",
                    Value = searchModel.Address ?? (object)DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "Phone",
                    Value = searchModel.Phone ?? (object)DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "ProvinceId",
                    TypeName = "[dbo].[GuidList]", //Don't forget this one!
                    Value = tableProvinceId
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "DistrictId",
                    TypeName = "[dbo].[GuidList]", //Don't forget this one!
                    Value = tableDistrictId
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "WardId",
                    TypeName = "[dbo].[GuidList]", //Don't forget this one!
                    Value = tableWardId
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "ProductName",
                    Value = searchModel.ProductName ?? (object)DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "ERPProductCode",
                    TypeName = "[dbo].[StringList]", //Don't forget this one!
                    Value = tableERPProductCode
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "SerialNumber",
                    TypeName = "[dbo].[StringList]", //Don't forget this one!
                    Value = tableSerialNumber
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "SaleOrder",
                    TypeName = "[dbo].[StringList]", //Don't forget this one!
                    Value = tableSaleOrder
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "OrderDelivery",
                    TypeName = "[dbo].[StringList]", //Don't forget this one!
                    Value = tableOrderDelivery
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "WarrantyCode",
                    TypeName = "[dbo].[StringList]", //Don't forget this one!
                    Value = tableWarrantyCode
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "StartFromDate",
                    Value = searchModel.StartFromDate ?? (object)DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "EndFromDate",
                    Value = searchModel.EndFromDate ?? (object)DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "StartToDate",
                    Value = searchModel.StartToDate ?? (object)DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "EndToDate",
                    Value = searchModel.EndToDate ?? (object)DBNull.Value,
                },
            };
            var result = _context.Database.SqlQuery<ProductWarrantyReportExcelModel>(sqlQuery, parameters.ToArray()).ToList();
            return result;

        }

        public ActionResult ViewDetail(ProductWarrantyReportSearchModel searchModel, Guid pivotTemplate, string modeSearch)
        {
            TempData[CurrentUser.AccountId + "ProductWarrantySearchData"] = searchModel;
            TempData[CurrentUser.AccountId + "ProductWarrantyTemplateId"] = pivotTemplate;
            TempData[CurrentUser.AccountId + "ProductWarrantyModeSearch"] = modeSearch;
            return RedirectToAction("Index");
        }

        public ActionResult ChangeTemplate(Guid pivotTemplate, ProductWarrantyReportSearchModel searchModel, string modeSearch)
        {
            TempData[CurrentUser.AccountId + "ProductWarrantySearchData"] = searchModel;
            TempData[CurrentUser.AccountId + "ProductWarrantyTemplateId"] = pivotTemplate;
            TempData[CurrentUser.AccountId + "ProductWarrantyModeSearch"] = modeSearch;
            return RedirectToAction("Index");
        }


        [ValidateInput(false)]
        public ActionResult ProductWarrantyPivotGridPartial(Guid? templateId = null, ProductWarrantyReportSearchModel searchViewModel = null, string jsonReq = null)
        {
            var pageId = GetPageId("/Reports/ProductWarrantyReport");
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
                return PartialView("_ProductWarrantyPivotGridPartial", null);
            }
            else
            {
                if (!string.IsNullOrEmpty(jsonReq))
                {
                    searchViewModel = JsonConvert.DeserializeObject<ProductWarrantyReportSearchModel>(jsonReq);
                }
                var model = GetData(searchViewModel);
                ViewBag.Search = searchViewModel;
                return PartialView("_ProductWarrantyPivotGridPartial", model);
            }
        }
        [HttpPost]
        public ActionResult ExportPivot(ProductWarrantyReportSearchModel searchViewModel, Guid? templateId)
        {
            PivotGridExportOptions options = new PivotGridExportOptions();
            options.ExportType = PivotGridExportFormats.Excel;
            options.WYSIWYG.PrintRowHeaders = true;
            options.WYSIWYG.PrintFilterHeaders = false;
            options.WYSIWYG.PrintDataHeaders = false;
            options.WYSIWYG.PrintColumnHeaders = false;


            var model = GetData(searchViewModel);
            //get search value to display in file
            var filterDisplayList = GetSearchInfoToDisplay(searchViewModel);
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
            string fileName = "BAO_CAO_DANG_KY_BAO_HANH_MOI";
            return PivotGridExportExcel.GetExportActionResult(fileName, options, pivotSetting, model, filterDisplayList, headerText, layoutConfigs);
        }

        #region Lấy điều kiện lọc
        private List<SearchDisplayModel> GetSearchInfoToDisplay(ProductWarrantyReportSearchModel searchViewModel)
        {
            List<SearchDisplayModel> filterList = new List<SearchDisplayModel>();
            if (searchViewModel.StartFromDate != null)
            {
                filterList.Add(new SearchDisplayModel() { DisplayName = "Ngày bắt đầu" });
                filterList.Add(new SearchDisplayModel() { DisplayName = "Từ ngày", DisplayValue = searchViewModel.EndFromDate?.ToString("dd-MM-yyyy") });
            }
            if (searchViewModel.EndFromDate != null)
            {
                filterList.Add(new SearchDisplayModel() { DisplayName = "Đến ngày", DisplayValue = searchViewModel.EndToDate?.ToString("dd-MM-yyyy") });
            }
            if (searchViewModel.StartToDate != null)
            {
                filterList.Add(new SearchDisplayModel() { DisplayName = "Bảo hành đến hạn" });
                filterList.Add(new SearchDisplayModel() { DisplayName = "Từ ngày", DisplayValue = searchViewModel.EndFromDate?.ToString("dd-MM-yyyy") });
            }
            if (searchViewModel.EndToDate != null)
            {
                filterList.Add(new SearchDisplayModel() { DisplayName = "Đến ngày", DisplayValue = searchViewModel.EndToDate?.ToString("dd-MM-yyyy") });
            }

            if (searchViewModel.ProfileCode != null && searchViewModel.ProfileCode.Count() > 0)
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Mã CRM";
                var value = searchViewModel.ProfileCode;
                filter.DisplayValue = value != null && value.Count > 0 ? string.Join(", ", value) : string.Empty;
                filterList.Add(filter);
            }

            if (!string.IsNullOrEmpty(searchViewModel.ProfileName))
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Mã CRM";
                var value = searchViewModel.ProfileName;
                filter.DisplayValue = value;
                filterList.Add(filter);
            }

            if (searchViewModel.ERPProductCode != null && searchViewModel.ERPProductCode.Count() > 0)
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Mã SAP Sản phẩm";
                var value = searchViewModel.ERPProductCode;
                filter.DisplayValue = value != null && value.Count > 0 ? string.Join(", ", value) : string.Empty;
                filterList.Add(filter);
            }

            if (!string.IsNullOrEmpty(searchViewModel.ProductName))
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Tên sản phẩm";
                var value = searchViewModel.ProductName;
                filter.DisplayValue = value;
                filterList.Add(filter);
            }
           
            if (searchViewModel.SerialNumber != null && searchViewModel.SerialNumber.Count() > 0)
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Số Serial";
                var value = searchViewModel.SerialNumber;
                filter.DisplayValue = value != null && value.Count > 0 ? string.Join(", ", value) : string.Empty;
                filterList.Add(filter);
            }
            if (searchViewModel.SaleOrder != null && searchViewModel.SaleOrder.Count() > 0)
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Đơn hàng";
                var value = searchViewModel.SaleOrder;
                filter.DisplayValue = value != null && value.Count > 0 ? string.Join(", ", value) : string.Empty;
                filterList.Add(filter);
            }
            if (searchViewModel.OrderDelivery != null && searchViewModel.OrderDelivery.Count() > 0)
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Phiếu giao hàng";
                var value = searchViewModel.OrderDelivery;
                filter.DisplayValue = value != null && value.Count > 0 ? string.Join(", ", value) : string.Empty;
                filterList.Add(filter);
            }
            if (searchViewModel.WarrantyCode != null && searchViewModel.WarrantyCode.Count() > 0)
            {
                SearchDisplayModel filter = new SearchDisplayModel();
                filter.DisplayName = "Tên bảo hành";
                var warrantylist = (from a in _context.WarrantyModel
                                   join p in searchViewModel.WarrantyCode on a.WarrantyCode equals p
                                   select a.WarrantyName).ToList();
                var value = warrantylist;
                filter.DisplayValue = value != null && value.Count > 0 ? string.Join(", ", value) : string.Empty;
                filterList.Add(filter);
            }

            return filterList;
        }
        #endregion
    }
}