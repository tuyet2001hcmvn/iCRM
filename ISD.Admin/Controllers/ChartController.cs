using ISD.Constant;
using ISD.Core;
using ISD.ViewModels;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ISD.Admin.Controllers
{
    public class ChartController : BaseController
    {
        // 1. Tỉ lệ Khách hàng theo Phân loại KH (doanh nghiệp - tiêu dùng)
        public ActionResult GetChartCustomerClassification()
        {
            var labels = new List<String>();

            labels.Add("Doanh Nghiệp");
            labels.Add("Tiêu dùng");

            int sl_kh_tieuDung = getNumberCustomerByTypeAndCompanyCode("C", CurrentUser.CompanyCode);
            int sl_kh_doanhNghiep = getNumberCustomerByTypeAndCompanyCode("B", CurrentUser.CompanyCode);

            int tong = sl_kh_tieuDung + sl_kh_doanhNghiep;
            float ti_le_khTieuDung = (sl_kh_tieuDung * 100) / (float)tong;
            float ti_le_khDoanhNghiep = (sl_kh_doanhNghiep * 100) / (float)tong;

            var dataSet = new List<double>();

            dataSet.Add(Math.Round(ti_le_khDoanhNghiep, 2));
            dataSet.Add(Math.Round(ti_le_khTieuDung, 2));

            return Json(new { success = true, dataSet, labels }, JsonRequestBehavior.AllowGet);
        }

        // 2. Khách hàng theo nhóm KH - tỉ lệ nhóm khách hàng
        public ActionResult GetPieChartCustomerGroup()
        {
            var lstProfileGroup = getChartCustomerGroupData();
            lstProfileGroup = lstProfileGroup.OrderByDescending(item => item.PercentOfProfiles).ToList();

            var labels = new List<String>();
            var dataSet = new List<decimal>();
            var totalGroupPercent = new Decimal();

            foreach (var item in lstProfileGroup)
            {
                labels.Add(item.ProfileGroupName);
                dataSet.Add((decimal)item.PercentOfProfiles);
                totalGroupPercent += (decimal)item.PercentOfProfiles;
            }

            labels.Add("Nhóm Khác");
            dataSet.Add(100 - totalGroupPercent);

            return Json(new { success = true, dataSet, labels }, JsonRequestBehavior.AllowGet);
        }

        // Biểu đồ cột - số lượng khách hàng theo nhóm
        public ActionResult GetBarChartCustomerGroup()
        {
            var lstProfileGroup = getChartCustomerGroupData();
            lstProfileGroup = lstProfileGroup.OrderByDescending(item => item.PercentOfProfiles).ToList();

            var labels = new List<String>();
            var dataSet = new List<decimal>();
            var totalGroupProfile = new int();

            foreach (var item in lstProfileGroup)
            {
                labels.Add(item.ProfileGroupName);
                dataSet.Add((int)item.NumberOfProfiles);
                totalGroupProfile += (int)item.NumberOfProfiles;
            }

            string totalQuery = "SELECT COUNT(*) FROM Customer.ProfileGroupModel AS gr JOIN Customer.ProfileModel AS pr ON pr.ProfileId = gr.ProfileId " +
                                                "WHERE gr.CompanyCode ='" + CurrentUser.CompanyCode + "' AND pr.Actived = 1";

            var totalProfile = getTotalActiveProfileByCurrCompanyCode(totalQuery);

            labels.Add("Nhóm Khác");
            dataSet.Add(totalProfile - totalGroupProfile);

            return Json(new { success = true, dataSet, labels }, JsonRequestBehavior.AllowGet);
        }

        // 3. Phân loại Khách hàng theo khu vực - tỉ lệ khách hàng
        public ActionResult GetPieChartPercentCustomerByArea()
        {
            var lstData = getChartCustomerByAreaData();

            var labels = new List<String>();
            var dataSet = new List<decimal>();
            var totalAreaPercent = new decimal();
            string totalQuery = "select count(*) FROM[Customer].[ProfileModel] as a join[Customer].ProfileTypeModel as b on a.ProfileId = b.ProfileId" +
                                                 " where b.CompanyCode ='" + CurrentUser.CompanyCode + "' and a.CustomerTypeCode = 'Account' and a.Actived = 1";

            var finalTotalProfile = getTotalActiveProfileByCurrCompanyCode(totalQuery);

            foreach (var item in lstData)
            {
                labels.Add(item.AreaName);
                if (finalTotalProfile != 0)
                {
                    item.PercentOfProfiles = (decimal)(item.NumberOfProfiles * 100) / finalTotalProfile;
                }
                else
                {
                    item.PercentOfProfiles = 0;
                }
                dataSet.Add(item.PercentOfProfiles);
                totalAreaPercent += item.PercentOfProfiles;
            }

            labels.Add("Nước Ngoài");
            dataSet.Add(100 - totalAreaPercent);

            return Json(new { success = true, dataSet, labels }, JsonRequestBehavior.AllowGet);
        }

        // số lượng khách hàng
        public ActionResult GetBarChartCustomerByArea()
        {
            var lstData = getChartCustomerByAreaData();

            var totalProfile = new int();
            var dataSet = new List<int>();
            var labels = new List<String>();

            foreach (var item in lstData)
            {
                labels.Add(item.AreaName);
                dataSet.Add(item.NumberOfProfiles);
                totalProfile += item.NumberOfProfiles;
            }

            string totalQuery = "select count(*) FROM[Customer].[ProfileModel] as a join[Customer].ProfileTypeModel as b on a.ProfileId = b.ProfileId" +
                                                 " where b.CompanyCode ='" + CurrentUser.CompanyCode + "' and a.CustomerTypeCode = 'Account' and a.Actived = 1";

            var finalTotalProfile = getTotalActiveProfileByCurrCompanyCode(totalQuery);

            labels.Add("Nước Ngoài");
            dataSet.Add(finalTotalProfile - totalProfile);

            return Json(new { success = true, dataSet, labels }, JsonRequestBehavior.AllowGet);
        }

        // 4. Báo cáo số lượng khách hàng theo top 10 tỉnh thành
        public ActionResult GetChartCustomerByTop10Province()
        {
            var lstData = getChartCustomerByTop10ProvinceData();

            var labels = new List<String>();
            var dataSet = new List<int>();

            foreach (var item in lstData)
            {
                labels.Add(item.ProvinceName);
                dataSet.Add(item.NumberOfProfiles);
            }

            return Json(new { success = true, dataSet, labels }, JsonRequestBehavior.AllowGet);
        }

        // 5.1 SL Khách hàng ghé thăm SR kì này, kì trước đến showroom tháng này và tháng trước NPP
        public ActionResult GetChartNumberCustomerVisitSRNPP()
        {
            var labels = new List<String>();

            var dataSet = new List<int>();
            var dataSet2 = new List<int>();

            DateTime? fromDate, toDate, fromPreviousDay, toPreviousDay;

            _unitOfWork.CommonDateRepository.GetDateBy("ThisMonth", out fromDate, out toDate, out fromPreviousDay, out toPreviousDay);

            var storeList = _context.StoreModel.Where(x => x.DefaultCustomerSource == "NPP" && x.CompanyId == CurrentUser.CompanyId && x.Actived == true).OrderBy(x => x.OrderIndex);

            var currentMonth = getChartNumberCustomerVisitSRData(fromDate, toDate);
            var previousMonth = getChartNumberCustomerVisitSRData(fromPreviousDay, toPreviousDay);

            var data = new List<ProfileQuantityAppointmentWithShowRoomReportViewModel>();

            foreach (var item in storeList)
            {
                var current = currentMonth.Where(x => x.StoreId == item.StoreId).FirstOrDefault()?.ProfileCount;
                var previous = previousMonth.Where(x => x.StoreId == item.StoreId).FirstOrDefault()?.ProfileCount;
                data.Add(new ProfileQuantityAppointmentWithShowRoomReportViewModel
                {
                    StoreName = item.StoreName,
                    ProfileCount = current ?? 0,
                    ProfileCountPrevious = previous
                });
            }

            foreach (var item in data.OrderByDescending(x => x.ProfileCountPrevious))
            {
                labels.Add(item.StoreName);
                dataSet.Add(item.ProfileCount);
                dataSet2.Add((int)(item.ProfileCountPrevious ?? 0));
            }

            return Json(new { success = true, dataSet, dataSet2, labels }, JsonRequestBehavior.AllowGet);
        }
        // 5.2 SL Khách hàng ghé thăm SR kì này, kì trước đến showroom tháng này và tháng trước SR
        public ActionResult GetChartNumberCustomerVisitSR()
        {
            var labels = new List<String>();

            var dataSet = new List<int>();
            var dataSet2 = new List<int>();

            DateTime? fromDate, toDate, fromPreviousDay, toPreviousDay;

            _unitOfWork.CommonDateRepository.GetDateBy("ThisMonth", out fromDate, out toDate, out fromPreviousDay, out toPreviousDay);

            var storeList = _context.StoreModel.Where(x => x.DefaultCustomerSource == "SHOWROOM" && x.CompanyId == CurrentUser.CompanyId && x.Actived == true).OrderBy(x => x.OrderIndex);

            var currentMonth = getChartNumberCustomerVisitSRData(fromDate, toDate);
            var previousMonth = getChartNumberCustomerVisitSRData(fromPreviousDay, toPreviousDay);

            var data = new List<ProfileQuantityAppointmentWithShowRoomReportViewModel>();

            foreach (var item in storeList)
            {
                var current = currentMonth.Where(x => x.StoreId == item.StoreId).FirstOrDefault()?.ProfileCount;
                var previous = previousMonth.Where(x => x.StoreId == item.StoreId).FirstOrDefault()?.ProfileCount;
                data.Add(new ProfileQuantityAppointmentWithShowRoomReportViewModel
                {
                    StoreName = item.StoreName,
                    ProfileCount = current ?? 0,
                    ProfileCountPrevious = previous
                });
            }

            foreach (var item in data.OrderByDescending(x => x.ProfileCountPrevious))
            {
                labels.Add(item.StoreName);
                dataSet.Add(item.ProfileCount);
                dataSet2.Add((int)(item.ProfileCountPrevious ?? 0));
            }

            return Json(new { success = true, dataSet, dataSet2, labels }, JsonRequestBehavior.AllowGet);
        }

        // 6A. Báo cáo thị hiếu khách hàng đến showroom - số lượt like sp trên crm 
        public ActionResult GetChartCustomerTastes()
        {
            var labels = new List<String>();
            var dataSet = new List<int>();

            DateTime? fromDate, toDate;

            _unitOfWork.CommonDateRepository.GetDateBy("ThisMonth", out fromDate, out toDate);


            var searchModel = new CustomerTastesSearchViewModel();
            searchModel.FromDate = fromDate;
            searchModel.ToDate = toDate;
            if (searchModel.StoreId == null || searchModel.StoreId.Count == 0)
            {
                var storeList = _unitOfWork.StoreRepository.GetAllStore();
                if (storeList != null && storeList.Count > 0)
                {
                    searchModel.StoreId = storeList.Select(p => p.StoreId).ToList();
                }
            }

            var lstData = getChartCustomerTastesData(searchModel);

            foreach (var item in lstData)
            {
                labels.Add(item.MaSP);
                dataSet.Add((int)(item.SoLuotLiked ?? 0));
            }

            return Json(new { success = true, dataSet, labels }, JsonRequestBehavior.AllowGet);
        }

        // 6. Báo cáo tổng hợp thị hiếu sản phẩm - tổng số lượt like và view (Top15)
        public ActionResult GetChartStatisticLikeViewProduct()
        {
            var labels = new List<String>();
            var dataSet = new List<int>();
            DateTime? fromDate, toDate;
            _unitOfWork.CommonDateRepository.GetDateBy("ThisMonth", out fromDate, out toDate);

            var searchViewModel = new StatisticLikeViewSearchViewModel();
            searchViewModel.CommonDate = "ThisMonth";
            searchViewModel.FromDate = fromDate;
            searchViewModel.ToDate = toDate;
            searchViewModel.FieldTOP = "Total";
            searchViewModel.TOP = 15;

            var lstData = getChartProductCommentData(searchViewModel);
            foreach (var item in lstData)
            {
                labels.Add(item.MaSP);
                dataSet.Add((int)(item.SoLuotLikedAC ?? 0) + (item.SoLuotViewedAC ?? 0) + (item.SoLuotLikedCRM ?? 0) + (item.SoLuotLikedCRM ?? 0) + (item.SoLuotLikedCRMNV ?? 0));
            }

            return Json(new { success = true, dataSet, labels }, JsonRequestBehavior.AllowGet);
        }

        // 7. Báo cáo tổng hợp khiếu nại theo lỗi sản phẩm - theo nhóm hàng
        public ActionResult chartProductIssue()
        {
            var labels = new List<String>();

            var dataSet = new List<decimal>();
            var dataSetLine = new List<int>();

            //DateTime? fromDate, toDate;

            //_unitOfWork.CommonDateRepository.GetDateBy("ThisMonth", out fromDate, out toDate);

            //var lstProductCategoryCode = new List<string>() { "ACR_FOIL", "ACR_MDFCA", "ACR_NOLINE", "MEL_MFC" }; //Ancuong
            var lstProductCategoryCode = new List<string>() { "ACR_FOIL", "ACR_MDFCA", "ACR_NOLINE", "MEL_MFC" }; //db dev

            var lstRawData = getChartProductIssueData(lstProductCategoryCode, null, null, null);

            // === Demo Data === //
            //var lstRawData = tempProductDataIssue();

            var lstData = lstRawData.GroupBy(item => item.ProductCategoryName)
                        .Select(group => new { ErrorName = group.Key, Items = group.ToList() })
                        .ToList();

            foreach (var dataItem in lstData)
            {
                labels.Add(dataItem.ErrorName);
                dataSet.Add((decimal)dataItem.Items.Select(i => i.WarrantyValue).Sum());
                dataSetLine.Add(dataItem.Items.Count);
            }

            return Json(new { success = true, dataSet, dataSetLine, labels }, JsonRequestBehavior.AllowGet);
        }

        // 7. Báo cáo tổng hợp khiếu nại theo lỗi sản phẩm - theo lỗi thường gặp
        public ActionResult chartProductIssue2()
        {
            var labels = new List<String>();

            var dataSet = new List<decimal>();
            var dataSetLine = new List<int>();
            //DateTime? fromDate, toDate;

            //_unitOfWork.CommonDateRepository.GetDateBy("ThisMonth", out fromDate, out toDate);

            var lstUsualErrorCode = new List<string>() { "BH_CongVenh", "BH_Tray", "BH_SanLechMau", "BH_Moi" };

            var lstRawData = getChartProductIssueData(null, lstUsualErrorCode, null, null);

            //var lstRawData = tempProductDataIssue();

            var lstData = lstRawData.GroupBy(item => item.UsualErrorName)
                        .Select(group => new { ErrorName = group.Key, Items = group.ToList() })
                        .ToList();

            foreach (var dataItem in lstData)
            {
                labels.Add(dataItem.ErrorName);
                dataSet.Add((decimal)dataItem.Items.Select(i => i.WarrantyValue).Sum());
                dataSetLine.Add(dataItem.Items.Count);
            }

            return Json(new { success = true, dataSet, dataSetLine, labels }, JsonRequestBehavior.AllowGet);
        }

        // 8. Báo cáo tổng hợp điểm trưng bày
        public ActionResult chartShowroom()
        {
            var labels = new List<String>();

            var dataSet = new List<int>();
            var dataSetLine = new List<decimal>();

            //DateTime? fromDate, toDate;

            //_unitOfWork.CommonDateRepository.GetDateBy("Custom", out fromDate, out toDate);

            var lstArea = new List<string>() { "1100", "1200", "1300", "2101" }; // Khu vực 'Khu vực Miền Bắc','Khu vực Miền Trung','Khu vực Nam', 'Khu Vực Châu á'

            var searchModel = new ShowroomReportSearchViewModel();
            //searchModel.WorkFlowId = new List<Guid>() { new Guid("dfd830ef-4db4-420c-b9d9-9e094bb07760") }; // loại "Góc Vật Liệu"
            searchModel.TaskStatusCode = new List<string>() { "DL" }; // trạng thái "Đã lắp"
            searchModel.CompanyId = CurrentUser.CompanyId; // fd69542b-2626-4010-b5e5-a7d06c490772
            //searchModel.StartFromDate = fromDate;
            //searchModel.StartToDate = toDate;

            var lstData = getChartShowroomData(searchModel, lstArea).Select(x => new { x.Area, x.NumberOfShowroom, x.ValueOfShowroom }).GroupBy(x => x.Area);
            foreach (var dataItem in lstData)
            {
                labels.Add(dataItem?.Key);
                dataSet.Add((int)dataItem?.Sum(x => x.NumberOfShowroom));
                dataSetLine.Add((decimal)dataItem?.Sum(x => x.ValueOfShowroom));
            }

            return Json(new { success = true, dataSet, dataSetLine, labels }, JsonRequestBehavior.AllowGet);
        }

        // 9. Báo cáo tổng hợp xuất nhập tồn Catalog
        public async Task<ActionResult> chartCatalog()
        {
            var labels = new List<String>();

            var dataSet = new List<decimal>();
            var dataSetLine = new List<int>();

            //var lstProductId = new List<string>()
            //{
            //    "1000041253", //Chỉ nhựa PVC/ABS
            //    "1000041252", //Arilux
            //    "1000041255", //Formica
            //    "1000056032", //Eco Veneer Ver 1.2020
            //};

            //foreach (var dataItem in lstProductId)
            //{
            var data = new List<StockOnHandViewModel>();
            //var storeList = _unitOfWork.StoreRepository.GetAllStore().Where(x=>x.DefaultCustomerSource != "NPP" );
            //foreach (var item in storeList)
            //{
                data.AddRange(_unitOfWork.StockRepository.GetStockOnHandBySaleOrgCatalogue());
            //}
            if (data != null)
            {
                data = (from lst in data
                        group lst by lst.ProductGroups into g
                        select new StockOnHandViewModel
                        {
                            ProductGroups = g.Key?? "Khác",
                            Qty = g.Sum(x => x.Qty)
                        }).ToList();
                data = data.OrderByDescending(x => x.Qty).ToList();
            }

            foreach (var item in data)
            {
                labels.Add(item.ProductGroups);
                dataSet.Add(item.Qty);
            }

            //}

            return Json(new { success = true, dataSet, dataSetLine, labels }, JsonRequestBehavior.AllowGet);
        }


        #region GetChartData

        private List<ProfileQuantityAppointmentWithShowRoomReportViewModel> getChartNumberCustomerVisitSRData(DateTime? fromDate, DateTime? toDate)
        {
            var tableCreateAtSaleOrgSchema = new List<SqlMetaData>(1)
            {
                    new SqlMetaData("Code", SqlDbType.NVarChar, 100)
            }.ToArray();
            var tableCreateAtSaleOrg = new List<SqlDataRecord>();
            var tableRow = new SqlDataRecord(tableCreateAtSaleOrgSchema);
            tableCreateAtSaleOrg.Add(tableRow);
            string sqlQuery = "EXEC [Report].[usp_ProfileQuantityAppointmentWithShowRoomReport] @CreateAtSaleOrg, @FromDate, @ToDate, @CurrentCompanyCode";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "CreateAtSaleOrg",
                    TypeName = "[dbo].[StringList]",
                    Value = tableCreateAtSaleOrg
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "FromDate",
                    Value = fromDate ?? (object)DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "ToDate",
                    Value = toDate ?? (object)DBNull.Value,
                },
                //Nguồn KH
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "CurrentCompanyCode",
                    Value = CurrentUser.CompanyCode,
                },
            };
            var lstData = _context.Database.SqlQuery<ProfileQuantityAppointmentWithShowRoomReportViewModel>(sqlQuery, parameters.ToArray()).ToList();

            return lstData;
        }

        private List<CustomerTastesReportViewModel> getChartCustomerTastesData(CustomerTastesSearchViewModel searchViewModel)
        {
            string sqlQuery = "EXEC [Customer].[usp_CustomerTastesReport] @SaleOrgCode, @FromDate, @ToDate, @CustomerSourceCode, @CustomerGroupCode, @SaleEmployeeCode, @isViewByStore, @TOP";

            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "SaleOrgCode",
                    Value = searchViewModel.SaleOrgCode ?? (object)DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "FromDate",
                    Value = searchViewModel.FromDate ?? (object)DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "ToDate",
                    Value = searchViewModel.ToDate ?? (object)DBNull.Value,
                },
                //Nguồn KH
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "CustomerSourceCode",
                    Value = searchViewModel.CustomerSourceCode ?? (object)DBNull.Value,
                },
                //Nhóm KH
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "CustomerGroupCode",
                    Value = searchViewModel.CustomerGroupCode ?? (object)DBNull.Value,
                },
                //User
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "SaleEmployeeCode",
                    Value = searchViewModel.SaleEmployeeCode ?? (object)DBNull.Value,
                },
                //isViewByStore
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "isViewByStore",
                    Value = searchViewModel.isViewByStore,
                },
                //isViewByStore
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input,
                    ParameterName = "TOP",
                    Value = 15,
                },
            };

            #region RolesId parameter
            //Build your record
            var tableSchema = new List<SqlMetaData>(1)
                {
                    new SqlMetaData("Id", SqlDbType.UniqueIdentifier)
                }.ToArray();

            //And a table as a list of those records
            var table = new List<SqlDataRecord>();
            if (searchViewModel.StoreId != null && searchViewModel.StoreId.Count > 0)
            {
                foreach (var r in searchViewModel.StoreId)
                {
                    var tableRow = new SqlDataRecord(tableSchema);
                    tableRow.SetGuid(0, r);
                    table.Add(tableRow);
                }
                parameters.Add(
                    new SqlParameter
                    {
                        SqlDbType = SqlDbType.Structured,
                        Direction = ParameterDirection.Input,
                        ParameterName = "StoreId",
                        TypeName = "[dbo].[GuidList]", //Don't forget this one!
                        Value = table
                    });
                sqlQuery += ", @StoreId";
            }
            #endregion

            var result = _context.Database.SqlQuery<CustomerTastesReportViewModel>(sqlQuery, parameters.ToArray()).ToList();
            return result;
        }

        private List<InventoryViewModel> getChartCatalogInventoryData(Guid lstProductId, Guid? companyId)
        {
            string sqlQuery = "EXEC [dbo].[Warehouse_Search_Inventory] @CompanyId, @StoreId, @ProductId";

            #region Parameters
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter
                {
                    SqlDbType = SqlDbType.UniqueIdentifier,
                    Direction = ParameterDirection.Input,
                    ParameterName = "ProductId",
                    //TypeName = "[dbo].[GuidList]", //Don't forget this one!
                    Value = lstProductId
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.UniqueIdentifier,
                    Direction = ParameterDirection.Input,
                    ParameterName = "CompanyId",
                    Value =companyId ?? (object)DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.UniqueIdentifier,
                    Direction = ParameterDirection.Input,
                    ParameterName = "StoreId",
                    Value =(object)DBNull.Value,
                },
            };
            #endregion

            var result = _context.Database.SqlQuery<InventoryViewModel>(sqlQuery, parameters.ToArray()).ToList();

            return result;
        }

        private List<ShowroomReportViewModel> getChartShowroomData(ShowroomReportSearchViewModel searchModel, List<string> lstArea)
        {
            #region List Area Code
            //Build your record
            var tblAreaSchema = new List<SqlMetaData>(1)
            {
                new SqlMetaData("Code", SqlDbType.NVarChar, 50)
            }.ToArray();

            //And a table as a list of those records
            var tableArea = new List<SqlDataRecord>();
            if (lstArea != null && lstArea.Count > 0)
            {
                foreach (var areaCode in lstArea)
                {
                    var tableRow = new SqlDataRecord(tblAreaSchema);
                    tableRow.SetString(0, areaCode);
                    tableArea.Add(tableRow);
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tblAreaSchema);
                tableArea.Add(tableRow);
            }
            #endregion

            #region WorkFlowId
            //Build your record
            var tableWorkFlowSchema = new List<SqlMetaData>(1)
            {
                new SqlMetaData("Id", SqlDbType.UniqueIdentifier)
            }.ToArray();

            //And a table as a list of those records
            var tableWorkFlow = new List<SqlDataRecord>();
            if (searchModel.WorkFlowId != null && searchModel.WorkFlowId.Count > 0)
            {
                foreach (var r in searchModel.WorkFlowId)
                {
                    var tableRow = new SqlDataRecord(tableWorkFlowSchema);
                    tableRow.SetSqlGuid(0, r);
                    tableWorkFlow.Add(tableRow);
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tableWorkFlowSchema);
                tableWorkFlow.Add(tableRow);
            }
            #endregion

            #region TaskStatusCode
            //Build your record
            var tableStatusSchema = new List<SqlMetaData>(1)
            {
                new SqlMetaData("Code", SqlDbType.NVarChar, 50)
            }.ToArray();

            //And a table as a list of those records
            var tableStatus = new List<SqlDataRecord>();
            if (searchModel.TaskStatusCode != null && searchModel.TaskStatusCode.Count > 0)
            {
                foreach (var r in searchModel.TaskStatusCode)
                {
                    var tableRow = new SqlDataRecord(tableStatusSchema);
                    tableRow.SetString(0, r);
                    tableStatus.Add(tableRow);
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tableStatusSchema);
                tableStatus.Add(tableRow);
            }
            #endregion

            #region CategoryId
            //Build your record
            var tableCategorySchema = new List<SqlMetaData>(1)
            {
                new SqlMetaData("Id", SqlDbType.UniqueIdentifier)
            }.ToArray();

            //And a table as a list of those records
            var tableCategory = new List<SqlDataRecord>();
            var tableCategoryRow = new SqlDataRecord(tableCategorySchema);
            tableCategory.Add(tableCategoryRow);
            #endregion

            string sqlQuery = "EXEC [Report].[usp_ShowroomReport] @WorkFlowId, @TaskStatusCode, @Area, @CompanyId, @StartFromDate,@StartToDate, @CategoryId";

            #region Parameters
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "WorkFlowId",
                    TypeName = "[dbo].[GuidList]", //Don't forget this one!
                    Value = tableWorkFlow
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "TaskStatusCode",
                    TypeName = "[dbo].[StringList]", //Don't forget this one!
                    Value = tableStatus
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "Area",
                    TypeName = "[dbo].[StringList]", //Don't forget this one!
                    Value = tableArea
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.UniqueIdentifier,
                    Direction = ParameterDirection.Input,
                    ParameterName = "CompanyId",
                    Value = searchModel.CompanyId ?? (object)DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "StartFromDate",
                    Value = searchModel.StartFromDate?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "StartToDate",
                    Value = searchModel.StartToDate?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "CategoryId",
                    TypeName = "[dbo].[GuidList]", //Don't forget this one!
                    Value = tableCategory
                },
            };
            #endregion

            var result = _context.Database.SqlQuery<ShowroomReportViewModel>(sqlQuery, parameters.ToArray()).ToList();
            return result;
        }

        private List<TicketUsualErrorViewModel> getChartProductIssueData(List<string> ProductCategoryCodes, List<string> UsualErrorCodes, DateTime? FromDate, DateTime? ToDate)
        {
            if (ToDate.HasValue)
            {
                ToDate = ToDate.Value.AddDays(1).AddSeconds(-1);
            }

            #region UsualErrorCodeList
            //Build your record
            var tableErrorSchema = new List<SqlMetaData>(1)
            {
                new SqlMetaData("Code", SqlDbType.NVarChar, 50)
            }.ToArray();
            //And a table as a list of those records
            var tblUsualErrorCode = new List<SqlDataRecord>();
            if (UsualErrorCodes != null && UsualErrorCodes.Count > 0)
            {
                foreach (var code in UsualErrorCodes)
                {
                    var tblRow = new SqlDataRecord(tableErrorSchema);
                    tblRow.SetString(0, code);
                    tblUsualErrorCode.Add(tblRow);
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tableErrorSchema);
                tblUsualErrorCode.Add(tableRow);
            }
            #endregion

            #region ProductColorCodeList
            //Build your record
            var tableColorSchema = new List<SqlMetaData>(1)
            {
                new SqlMetaData("Code", SqlDbType.NVarChar, 50)
            }.ToArray();
            //And a table as a list of those records
            var tblColor = new List<SqlDataRecord>();
            var tblColorRow = new SqlDataRecord(tableColorSchema);
            tblColor.Add(tblColorRow);
            #endregion

            #region ProductCategoryCode
            //ProductCategoryCodes
            //Build your record
            var tableCategorySchema = new List<SqlMetaData>(1)
            {
                new SqlMetaData("Code", SqlDbType.NVarChar, 50)
            }.ToArray();

            //And a table as a list of those records
            var tblCategory = new List<SqlDataRecord>();
            if (ProductCategoryCodes != null && ProductCategoryCodes.Count > 0)
            {
                foreach (var code in ProductCategoryCodes)
                {
                    var tableRow = new SqlDataRecord(tableCategorySchema);
                    tableRow.SetString(0, code);
                    tblCategory.Add(tableRow);
                }
            }
            else
            {
                var tblCategoryRow = new SqlDataRecord(tableCategorySchema);
                tblCategory.Add(tblCategoryRow);
            }

            #endregion

            string sqlQuery = "EXEC [Report].[usp_TicketUsualErrorReport] @ProductCategoryCode, @ProductColorCode, @UsualErrorCode, @CurrentCompanyCode,@FromDate,@ToDate";

            #region Parameters
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "ProductCategoryCode",
                    TypeName = "[dbo].[StringList]", //Don't forget this one!
                    Value = tblCategory
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "ProductColorCode",
                    TypeName = "[dbo].[StringList]", //Don't forget this one!
                    Value = tblColor
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "UsualErrorCode",
                    TypeName = "[dbo].[StringList]", //Don't forget this one!
                    Value = tblUsualErrorCode
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "CurrentCompanyCode",
                    Value = CurrentUser.CompanyCode
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "FromDate",
                    Value = FromDate ?? (object)DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "ToDate",
                    Value = ToDate ?? (object)DBNull.Value
                }
            };
            #endregion

            var result = _context.Database.SqlQuery<TicketUsualErrorViewModel>(sqlQuery, parameters.ToArray()).ToList();

            return result;
        }

        private int getNumberCustomerByTypeAndCompanyCode(string customerTypeCode, string currentCompanyCode)
        {
            var data = (from pr in _context.ProfileModel
                        join prt in _context.ProfileTypeModel on pr.ProfileId equals prt.ProfileId
                        where prt.CustomerTypeCode == customerTypeCode && prt.CompanyCode == currentCompanyCode
                        && pr.CustomerTypeCode == "Account"
                        select pr).Count();
            //var returnNumb = new SqlParameter();
            //returnNumb.ParameterName = "@return_value";
            //returnNumb.SqlDbType = SqlDbType.Int;
            //returnNumb.Direction = ParameterDirection.Output;

            //var customerType = new SqlParameter("@CustomerType", customerTypeCode);
            //var companyCode = new SqlParameter("@CompanyCode", currentCompanyCode);
            //_context.Database.ExecuteSqlCommand("EXEC [Customer].[GetNumberCustomerByTypeAndCompanyCode] @CustomerType ,@CompanyCode, @return_value OUT", customerType, companyCode, returnNumb);

            return (int)data;
        }

        private List<ProfileGroupReportViewModel> getChartCustomerGroupData()
        {
            var searchModel = new ProfileGroupSearchModel();

            var lstCustomerGroupSapCode = new List<string>();
            lstCustomerGroupSapCode.AddRange(new List<string>() { "23", "16", "35", "11", "10", "12", "28", "24", "34", "18", "33", "17" });

            searchModel.ProfileGroupCode = lstCustomerGroupSapCode;

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
                    Value = CurrentUser.CompanyCode,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "FromDate",
                    Value = (object)DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "ToDate",
                    Value = (object)DBNull.Value,
                },
            };
            var result = _context.Database.SqlQuery<ProfileGroupReportViewModel>(sqlQuery, parameters.ToArray()).ToList();
            return result;
        }

        private int getTotalActiveProfileByCurrCompanyCode(string sqlQuery)
        {
            var totalProfile = _context.Database.SqlQuery<int>(sqlQuery).Single();
            return totalProfile;
        }

        private List<StatisticLikeViewProductViewModel> getChartProductCommentData(StatisticLikeViewSearchViewModel searchViewModel)
        {
            if (searchViewModel.StoreId == null || searchViewModel.StoreId.Count == 0)
            {
                var storeList = _unitOfWork.StoreRepository.GetStoreByPermission(CurrentUser.AccountId);
                if (storeList != null && storeList.Count > 0)
                {
                    searchViewModel.StoreId = storeList.Select(p => p.StoreId).ToList();
                }
            }

            #region list StoreId parameter
            //Build your record
            var tableSchema = new List<SqlMetaData>(1)
                {
                    new SqlMetaData("Id", SqlDbType.UniqueIdentifier)
                }.ToArray();

            //And a table as a list of those records
            var table = new List<SqlDataRecord>();
            if (searchViewModel.StoreId != null && searchViewModel.StoreId.Count > 0)
            {
                foreach (var r in searchViewModel.StoreId)
                {
                    var tableRow = new SqlDataRecord(tableSchema);
                    tableRow.SetGuid(0, r);
                    table.Add(tableRow);
                }
            }
            #endregion

            string sqlQuery = "EXEC [Report].[usp_StatisticLikeViewProduct] @FromDate, @ToDate, @SaleEmployeeCode, @FieldTOP, @TOP, @StoreId";

            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "FromDate",
                    Value = searchViewModel.FromDate ?? (object)DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "ToDate",
                    Value = searchViewModel.ToDate ?? (object)DBNull.Value,
                }, //User
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "SaleEmployeeCode",
                    Value = searchViewModel.SaleEmployeeCode ?? (object)DBNull.Value,
                },
                //FieldTOP
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "FieldTOP",
                    Value = searchViewModel.FieldTOP ?? (object)DBNull.Value,
                },
                //TOP màu like
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input,
                    ParameterName = "TOP",
                    Value = searchViewModel.TOP ?? (object)DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "StoreId",
                    TypeName = "[dbo].[GuidList]", //Don't forget this one!
                    Value = table
                }
            };

            var result = _context.Database.SqlQuery<StatisticLikeViewProductViewModel>(sqlQuery, parameters.ToArray()).ToList();

            return result;
        }

        private List<SummaryProfileByAreaVm> getChartCustomerByAreaData()
        {
            var lstAreaCode = new List<string>() { "1100", "1200", "1300" }; // khu vực miền bắc, trung, nam

            var data = (from pr in _context.ProfileModel
                        join c in _context.CatalogModel on new { CatalogCode = pr.SaleOfficeCode, CatalogTypeCode = "SaleOffice" } equals new { c.CatalogCode, c.CatalogTypeCode }
                        join s in _context.ProfileGroupModel on pr.ProfileId equals s.ProfileId
                        join a in lstAreaCode on pr.SaleOfficeCode equals a
                        where pr.Actived == true && pr.CustomerTypeCode == "Account" && s.CompanyCode == CurrentUser.CompanyCode
                        group new { pr, a, c } by new
                        {
                            pr.SaleOfficeCode,
                            c.CatalogText_vi
                        } into g
                        select new SummaryProfileByAreaVm
                        {
                            AreaCode = g.Key.SaleOfficeCode,
                            AreaName = g.Key.CatalogText_vi,
                            NumberOfProfiles = g.Count(),
                        }).OrderByDescending(x => x.NumberOfProfiles).ToList();

            //var tableAreaCodeSchema = new List<SqlMetaData>(1)
            //{
            //    new SqlMetaData("StringList", SqlDbType.NVarChar, 50)
            //}.ToArray();

            //var tableAreaCode = new List<SqlDataRecord>();
            //foreach (var areaCode in lstAreaCode)
            //{
            //    var tableRow = new SqlDataRecord(tableAreaCodeSchema);
            //    tableRow.SetString(0, areaCode);
            //    tableAreaCode.Add(tableRow);
            //}

            //string sqlQuery = "EXEC [Customer].[GetNumberCustomerPercentByCompanyCodeAndArea] @AreasCode , @CurrentCompanyCode";

            //List<SqlParameter> parameters = new List<SqlParameter>()
            //{
            //    new SqlParameter
            //    {
            //        SqlDbType = SqlDbType.Structured,
            //        Direction = ParameterDirection.Input,
            //        ParameterName = "AreasCode",
            //        TypeName = "[dbo].[StringList]", //Don't forget this one!
            //        Value = tableAreaCode
            //    },
            //    new SqlParameter
            //    {
            //        SqlDbType = SqlDbType.NVarChar,
            //        Direction = ParameterDirection.Input,
            //        ParameterName = "CurrentCompanyCode",
            //        Value = CurrentUser.CompanyCode,
            //    },
            //};

            //var result = _context.Database.SqlQuery<SummaryProfileByAreaVm>(sqlQuery, parameters.ToArray()).ToList();
            return data;
        }

        private List<SummaryProfileByProvinceVm> getChartCustomerByTop10ProvinceData()
        {
            var data = (from pr in _context.ProfileModel
                        join p in _context.ProvinceModel on pr.ProvinceId equals p.ProvinceId
                        join s in _context.ProfileGroupModel on pr.ProfileId equals s.ProfileId
                        where pr.Actived == true && pr.CustomerTypeCode == "Account" && s.CompanyCode == CurrentUser.CompanyCode
                        group new { pr, p, s } by new
                        {
                            p.ProvinceId,
                            p.ProvinceName,
                            p.Area
                        } into g
                        select new SummaryProfileByProvinceVm
                        {
                            Area = g.Key.Area,
                            ProvinceName = g.Key.ProvinceName,
                            NumberOfProfiles = g.Count(),
                        }).OrderByDescending(x => x.NumberOfProfiles).Take(10).OrderBy(x => x.Area).ToList();


            //string sqlQuery = "EXEC [Customer].[GetTopTenProvinceWithHighestCustomerCount] @CurrentCompanyCode = '" + CurrentUser.CompanyCode + "'";

            //var result = _context.Database.SqlQuery<SummaryProfileByProvinceVm>(sqlQuery).ToList();

            return data;
        }

        #endregion
    }

    class SummaryProfileByAreaVm
    {
        public string AreaCode { get; set; }
        public string AreaName { get; set; }
        public int NumberOfProfiles { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public decimal PercentOfProfiles { get; set; }
    }

    class SummaryProfileByProvinceVm
    {
        public Guid ProvinceId { get; set; }
        public string ProvinceName { get; set; }
        public int NumberOfProfiles { get; set; }
        public string Area { get; set; }
    }

    class SummaryProfileVisitShowroomVm
    {
        public string SaleOrgCode { get; set; }
        public string StoreName { get; set; }
        public int? CurrMonthProfile { get; set; }
        public int? PrevMonthProfile { get; set; }
    }

    class ShowroomReportViewModel2
    {
        public string WorkFlowName { get; set; }
        public string TaskStatusName { get; set; }
        public string Area { get; set; }
        public int? NumberOfShowroom { get; set; }
        public decimal? ValueOfShowroom { get; set; }
    }
}