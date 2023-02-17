using ISD.Constant;
using ISD.EntityModels;
using ISD.ViewModels;
using ISD.ViewModels.Marketing;
using ISD.ViewModels.Sale;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace ISD.Repositories
{
    public class ProductPromotionRepository
    {
        private EntityDataContext _context;
        /// <summary>
        /// Khởi tạo ProductRepository truyển vào DataContext
        /// </summary>
        /// <param name="db">EntityDataContext</param>
        public ProductPromotionRepository(EntityDataContext db)
        {
            _context = db;
        }
        /// <summary>
        /// Lấy thông tin Campaign
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ProductPromotionViewModel GetBy(Guid? Id)
        {
            var data = (from p in _context.ProductPromotionModel
                        join c in _context.CatalogModel on new { CatalogCode = p.SendTypeCode, CatalogTypeCode = ConstCatalogType.ProductPromotionSendType } equals new { c.CatalogCode, c.CatalogTypeCode }
                        //CreateBy
                        //join create in _context.AccountModel on p.CreateBy equals create.AccountId
                        //join cr in _context.SalesEmployeeModel on create.EmployeeCode equals cr.SalesEmployeeCode into crg
                        //from cr1 in crg.DefaultIfEmpty()
                            //EditBy
                        //join modify in _context.AccountModel on p.LastEditBy equals modify.AccountId into mg
                        //from m in mg.DefaultIfEmpty()
                        //join md in _context.SalesEmployeeModel on m.EmployeeCode equals md.SalesEmployeeCode into mdg
                        //from md1 in mdg.DefaultIfEmpty()
                        where p.ProductPromotionId == Id
                        select new ProductPromotionViewModel
                        {
                            ProductPromotionId = p.ProductPromotionId,
                            ProductPromotionTitle = p.ProductPromotionTitle,
                            Type = p.Type,
                            StartTime = p.StartTime,
                            EndTime = p.EndTime,
                            SendTypeCode = c.CatalogCode,
                            SendTypeName = c.CatalogText_vi,
                            CreateTime = p.CreateTime,
                            //CreateByName = cr1.SalesEmployeeName,
                            LastEditTime = p.LastEditTime,
                            //LastEditByName = md1.SalesEmployeeName,
                            CreateBy = p.CreateBy,
                            LastEditBy = p.LastEditBy,
                            TargetGroupId = p.TargetGroupId,
                            IsSendCatalogue = p.IsSendCatalogue,
                            Actived = p.Actived
                        }).FirstOrDefault();

            return data;
        }
        /// <summary>
        /// Lấy thông tin chi tiết chiến dịch
        /// </summary>
        /// <param name="Id">ProductPromotionId</param>
        /// <returns></returns>
        public IQueryable<ProductPromotionDetailViewModel> GetDetailBy(ProductPromotionViewModel viewModel)
        {
            var ppDetail = (from p in _context.ProductPromotionDetailModel
                                //Thông tin khách hàng
                            join pro in _context.ProfileModel on new { ProfileId = p.ProfileId.Value, CustomerTypeCode = ConstCustomerType.Account } equals new { pro.ProfileId, pro.CustomerTypeCode }
                            join account in _context.AccountModel on p.Checker equals account.AccountId into joinAcc
                            from st1 in joinAcc.DefaultIfEmpty()
                            join emp in _context.SalesEmployeeModel on st1.EmployeeCode equals emp.SalesEmployeeCode into final
                            from st2 in final.DefaultIfEmpty()
                            where p.ProductPromotionId == viewModel.ProductPromotionId 
                            && (viewModel.ListProfileCode == null || viewModel.ListProfileCode.Contains(pro.ProfileCode.ToString()))
                            && (viewModel.CustomerAccountGroup == null || pro.CustomerAccountGroupCode == viewModel.CustomerAccountGroup)
                            && (viewModel.ProfileName == null || pro.ProfileName.Contains(viewModel.ProfileName))
                            && (viewModel.isHasSAPCode == null || (viewModel.isHasSAPCode == true && pro.ProfileForeignCode != null) || (viewModel.isHasSAPCode == false && pro.ProfileForeignCode == null))
                            && (viewModel.SalesEmployeeCode == null || st2.SalesEmployeeCode == viewModel.SalesEmployeeCode)
                            orderby pro.ProfileCode
                            select new ProductPromotionDetailViewModel
                            {
                                ProductPromotionDetailId = p.ProductPromotionDetailId,
                                ProfileId = pro.ProfileId,
                                ProfileCode = pro.ProfileCode,
                                ProfileName = pro.ProfileName,
                                ProfileShortName = pro.ProfileShortName,
                                Status = p.Status,
                                ProductPromotionId = p.ProductPromotionId,
                            });
            //if (viewModel.SalesEmployeeCode != null || viewModel.RolesCode != null)
            //{
            //    ppDetail = (from p in ppDetail
            //                join account in _context.AccountModel on p.Checker equals account.AccountId into joinAcc
            //                from st1 in joinAcc.DefaultIfEmpty()
            //                join emp in _context.SalesEmployeeModel on st1.EmployeeCode equals emp.SalesEmployeeCode into final
            //                from st2 in final.DefaultIfEmpty()
            //                where st2.SalesEmployeeCode == viewModelSalesEmployeeCode
            //                orderby p.ProfileCode
            //                select new ProductPromotionDetailViewModel
            //                {
            //                    ProductPromotionDetailId = p.ProductPromotionDetailId,
            //                    ProfileId = p.ProfileId,
            //                    ProfileCode = p.ProfileCode,
            //                    ProfileName = p.ProfileName,
            //                    ProfileShortName = p.ProfileShortName,
            //                    Status = p.Status,
            //                    ProductPromotionId = p.ProductPromotionId
            //                });
            //}
            return ppDetail;
        }
        /// <summary>
        /// Lấy thông tin chi tiết chiến dịch theo khách hàng
        /// </summary>
        /// <param name="Id">ProfileId</param>
        /// <returns></returns>
        public List<ProductPromotionViewModel> GetByProfile(Guid Id)
        {
            var ppDetail = (from pm in _context.ProductPromotionModel 
                            join p in _context.ProductPromotionDetailModel on pm.ProductPromotionId equals p.ProductPromotionId
                            join c in _context.CatalogModel on new { CatalogCode = pm.SendTypeCode, CatalogTypeCode = ConstCatalogType.ProductPromotionSendType } equals new { c.CatalogCode, c.CatalogTypeCode }
                            //Thông tin khách hàng
                            join pro in _context.ProfileModel on new { ProfileId = p.ProfileId.Value, CustomerTypeCode = ConstCustomerType.Account } equals new { pro.ProfileId, pro.CustomerTypeCode }
                            where pro.ProfileId == Id
                            orderby pro.ProfileCode
                            select new ProductPromotionViewModel
                            {
                                ProductPromotionId = pm.ProductPromotionId,
                                ProductPromotionTitle = pm.ProductPromotionTitle,
                                SendTypeName = c.CatalogText_vi,
                                StartTime = pm.StartTime,
                                EndTime = pm.EndTime,
                                IsSendCatalogue = pm.IsSendCatalogue,
                                Status = p.Status
                            });
            return ppDetail.ToList();
        }

        /// <summary>
        /// Danh sách Địa chỉ của khách hàng
        /// Địa chỉ chính: Address trong ProfileModel
        /// Địa chỉ còn lại: AddressBookModel
        /// </summary>
        /// <param name="profileId"></param>
        /// <returns></returns>
        public List<ProfileAddressProductPromotionViewModel> GetAddressByProfile(Guid? profileId)
        {
            var addressBookList = (from a in _context.AddressBookModel
                                   join cTemp in _context.CatalogModel on new { AddressTypeCode = a.AddressTypeCode, Type = ConstCatalogType.AddressType } equals new { AddressTypeCode = cTemp.CatalogCode, Type = cTemp.CatalogTypeCode } into list0
                                   from c in list0.DefaultIfEmpty()
                                   join prTemp in _context.ProvinceModel on a.ProvinceId equals prTemp.ProvinceId into prList
                                   from pr in prList.DefaultIfEmpty()
                                   join dTemp in _context.DistrictModel on a.DistrictId equals dTemp.DistrictId into dList
                                   from d in dList.DefaultIfEmpty()
                                   join wTemp in _context.WardModel on a.WardId equals wTemp.WardId into wList
                                   from w in wList.DefaultIfEmpty()
                                   where a.ProfileId == profileId
                                   //Type of Address type
                                   && c.CatalogTypeCode == ConstCatalogType.AddressType
                                   orderby a.isMain descending, a.CreateTime descending
                                   select new ProfileAddressProductPromotionViewModel
                                   {
                                       ProfileId = a.ProfileId,
                                       Address = a.Address + (w != null ? ", " + w.Appellation + " " + w.WardName : null) +(d != null ? ", " + d.Appellation + " " + d.DistrictName : null) + (pr != null ?  ", " +  pr.ProvinceName : null),
                                   }).ToList();

            //Địa chỉ chính
            var mainAddress = (from a in _context.ProfileModel
                               join prTemp in _context.ProvinceModel on a.ProvinceId equals prTemp.ProvinceId into prList
                               from pr in prList.DefaultIfEmpty()
                               join dTemp in _context.DistrictModel on a.DistrictId equals dTemp.DistrictId into dList
                               from d in dList.DefaultIfEmpty()
                               join wTemp in _context.WardModel on a.WardId equals wTemp.WardId into wList
                               from w in wList.DefaultIfEmpty()
                               where a.ProfileId == profileId
                               //Type of Address type
                               //&& co.CatalogTypeCode == ConstCatalogType.AddressType
                               select new ProfileAddressProductPromotionViewModel()
                               {
                                   ProfileId = a.ProfileId,
                                   Address = a.Address + (w != null ? ", " + w.Appellation + " " + w.WardName : null) + (d != null ? ", " + d.Appellation + " " + d.DistrictName : null) + (pr != null ? ", " + pr.ProvinceName : null),

                               }).FirstOrDefault();

            addressBookList.Insert(0, mainAddress);
            if (addressBookList != null && addressBookList.Count > 0)
            {
                if (addressBookList[0] != null)
                {
                    addressBookList = addressBookList.ToList();
                }
                else
                {
                    addressBookList = new List<ProfileAddressProductPromotionViewModel>();
                }
            }
            return addressBookList;
        }


        public IQueryable<ProductPromotionViewModel> GetBy(ProductPromotionSearchViewModel searchViewModel)
        {
            IQueryable<ProductPromotionViewModel> data = (from p in _context.ProductPromotionModel
                                                          
                                                          join ctemp in _context.CatalogModel on new { CatalogCode = p.SendTypeCode, CatalogTypeCode = ConstCatalogType.ProductPromotionSendType } equals new { ctemp.CatalogCode, ctemp.CatalogTypeCode } into cList
                                                          from c in cList.DefaultIfEmpty()
                                                          //CreateBy
                                                          join create in _context.AccountModel on p.CreateBy equals create.AccountId
                                                          join cr in _context.SalesEmployeeModel on create.EmployeeCode equals cr.SalesEmployeeCode into crg
                                                          from cr1 in crg.DefaultIfEmpty()
                                                              //EditBy
                                                          join modify in _context.AccountModel on p.LastEditBy equals modify.AccountId into mg
                                                          from m in mg.DefaultIfEmpty()
                                                          join md in _context.SalesEmployeeModel on m.EmployeeCode equals md.SalesEmployeeCode into mdg
                                                          from md1 in mdg.DefaultIfEmpty()
                                                          where (searchViewModel.ProductPromotionTitle == null || p.ProductPromotionTitle.Contains(searchViewModel.ProductPromotionTitle))
                                                          && (searchViewModel.FromStartTime == null || searchViewModel.FromStartTime <= p.StartTime)
                                                          && (searchViewModel.ToStartTime == null || p.StartTime <= searchViewModel.ToStartTime)
                                                          && (searchViewModel.FromEndTime == null || searchViewModel.FromEndTime <= p.EndTime)
                                                          && (searchViewModel.ToEndTime == null || p.EndTime <= searchViewModel.ToEndTime)
                                                          && (searchViewModel.Actived == null || p.Actived == searchViewModel.Actived)
                                                          orderby  p.EndTime descending,p.CreateTime descending
                                                          select new ProductPromotionViewModel
                                                          {
                                                              ProductPromotionId = p.ProductPromotionId,
                                                              ProductPromotionTitle = p.ProductPromotionTitle,
                                                              Type = p.Type,
                                                              StartTime = p.StartTime,
                                                              EndTime = p.EndTime,
                                                              SendTypeCode = c.CatalogCode,
                                                              SendTypeName = c.CatalogText_vi,
                                                              CreateTime = p.CreateTime,
                                                              CreateByName = cr1.SalesEmployeeName,
                                                              LastEditTime = p.LastEditTime,
                                                              LastEditByName = md1.SalesEmployeeName,
                                                              CreateBy = p.CreateBy,
                                                              LastEditBy = p.LastEditBy,
                                                              Actived = p.Actived
                                                          });
            return data;

        }
        public List<ProductPromotionReportViewModel> GetAllForReport(ProductPromotionReportSearchViewModel searchModel)
        {
            #region SalesEmployeeCode
            //Build your record
            var tableSalesEmployeeCodeSchema = new List<SqlMetaData>(1)
                {
                    new SqlMetaData("Code", SqlDbType.NVarChar, 100)
                }.ToArray();

            //And a table as a list of those records
            var tableSalesEmployeeCode = new List<SqlDataRecord>();
            List<string> salesEmployeeCodeLst = new List<string>();
            if (searchModel.CheckerCode != null && searchModel.CheckerCode.Count > 0)
            {
                foreach (var r in searchModel.CheckerCode)
                {
                    var tableRow = new SqlDataRecord(tableSalesEmployeeCodeSchema);

                    tableRow.SetString(0, r);
                    if (!salesEmployeeCodeLst.Contains(r))
                    {
                        salesEmployeeCodeLst.Add(r);
                        tableSalesEmployeeCode.Add(tableRow);
                    }
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tableSalesEmployeeCodeSchema);
                tableSalesEmployeeCode.Add(tableRow);
            }
            #endregion
            #region RolesCode
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

            string sqlQuery = "EXEC [Report].[usp_ProductPromotionReport] @ProfileId, @CheckerCode, @RolesCode,@FromDate, @ToDate";

            #region Parameters
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter
                {
                    SqlDbType = SqlDbType.UniqueIdentifier,
                    Direction = ParameterDirection.Input,
                    ParameterName = "ProfileId",
                    Value = searchModel.ProfileId ?? (object)DBNull.Value,
                },new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "CheckerCode",
                    TypeName = "[dbo].[StringList]", //Don't forget this one!
                    Value = tableSalesEmployeeCode,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "RolesCode",
                    TypeName = "[dbo].[StringList]", //Don't forget this one!
                    Value = tableRolesCode,
                },new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "FromDate",
                    Value = searchModel.FromDate ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "ToDate",
                    Value = searchModel.ToDate ?? (object)DBNull.Value
                },
            };
            #endregion

            var result = _context.Database.SqlQuery<ProductPromotionReportViewModel>(sqlQuery, parameters.ToArray()).ToList();

            return result;
        }
        
        public List<ProductPromotionDetailReportViewModel> GetAllForDetailReport(ProductPromotionDetailReportSearchViewModel searchModel)
        {
            #region SalesEmployeeCode
            //Build your record
            var tableSalesEmployeeCodeSchema = new List<SqlMetaData>(1)
                {
                    new SqlMetaData("Code", SqlDbType.NVarChar, 100)
                }.ToArray();

            //And a table as a list of those records
            var tableSalesEmployeeCode = new List<SqlDataRecord>();
            List<string> salesEmployeeCodeLst = new List<string>();
            if (searchModel.SalesEmployeeCode != null && searchModel.SalesEmployeeCode.Count > 0)
            {
                foreach (var r in searchModel.SalesEmployeeCode)
                {
                    var tableRow = new SqlDataRecord(tableSalesEmployeeCodeSchema);

                    tableRow.SetString(0, r);
                    if (!salesEmployeeCodeLst.Contains(r))
                    {
                        salesEmployeeCodeLst.Add(r);
                        tableSalesEmployeeCode.Add(tableRow);
                    }
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tableSalesEmployeeCodeSchema);
                tableSalesEmployeeCode.Add(tableRow);
            }
            #endregion
            #region Checked
            //Build your record
            var tableCheckerCodeSchema = new List<SqlMetaData>(1)
                {
                    new SqlMetaData("Code", SqlDbType.NVarChar, 100)
                }.ToArray();

            //And a table as a list of those records
            var tableCheckerCode = new List<SqlDataRecord>();
            List<string> checkerCodeLst = new List<string>();
            if (searchModel.CheckerCode != null && searchModel.CheckerCode.Count > 0)
            {
                foreach (var r in searchModel.CheckerCode)
                {
                    var tableRow = new SqlDataRecord(tableCheckerCodeSchema);

                    tableRow.SetString(0, r);
                    if (!checkerCodeLst.Contains(r))
                    {
                        checkerCodeLst.Add(r);
                        tableCheckerCode.Add(tableRow);
                    }
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tableCheckerCodeSchema);
                tableCheckerCode.Add(tableRow);
            }
            #endregion
            #region RolesCode
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

            string sqlQuery = "EXEC [Report].[usp_ProductPromotionDetailReport] @ProfileId, @SalesEmployeeCode, @CheckerCode, @RolesCode,@FromDate, @ToDate";

            #region Parameters
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter
                {
                    SqlDbType = SqlDbType.UniqueIdentifier,
                    Direction = ParameterDirection.Input,
                    ParameterName = "ProfileId",
                    Value = searchModel.ProfileId ?? (object)DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "CheckerCode",
                    TypeName = "[dbo].[StringList]", //Don't forget this one!
                    Value = tableCheckerCode,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "SalesEmployeeCode",
                    TypeName = "[dbo].[StringList]", //Don't forget this one!
                    Value = tableSalesEmployeeCode,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "RolesCode",
                    TypeName = "[dbo].[StringList]", //Don't forget this one!
                    Value = tableRolesCode,
                },new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "FromDate",
                    Value = searchModel.FromDate ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "ToDate",
                    Value = searchModel.ToDate ?? (object)DBNull.Value
                },
            };
            #endregion

            var result = _context.Database.SqlQuery<ProductPromotionDetailReportViewModel>(sqlQuery, parameters.ToArray()).ToList();

            return result;
        }
    }
}