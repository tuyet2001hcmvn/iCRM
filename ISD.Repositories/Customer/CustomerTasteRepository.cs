using ISD.Constant;
using ISD.EntityModels;
using ISD.ViewModels;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace ISD.Repositories
{
    public class CustomerTasteRepository
    {
        private EntityDataContext _context;
        public CustomerTasteRepository(EntityDataContext db)
        {
            _context = db;
        }

        public List<CustomerTastesReportViewModel> GetTastes(Guid? profileId)
        {
            string sqlQuery = "EXEC [Customer].[usp_CustomerTastes] @ProfileId";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter
                {
                    SqlDbType = SqlDbType.UniqueIdentifier,
                    Direction = ParameterDirection.Input,
                    ParameterName = "ProfileId",
                    Value = profileId ?? (object)DBNull.Value,
                },
            };

            var catalogueList = _context.Database.SqlQuery<CustomerTastesReportViewModel>(sqlQuery, parameters.ToArray()).ToList();
            return catalogueList;
        }

        public List<CustomerTasteViewModel> GetCustomerTastesBy(Guid appointmentId)
        {
            string sqlQuery = "EXEC [Customer].[usp_CustomerTastesAppointment] @AppointmentId";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter
                {
                    SqlDbType = SqlDbType.UniqueIdentifier,
                    Direction = ParameterDirection.Input,
                    ParameterName = "AppointmentId",
                    Value = appointmentId,
                }
            };

            var result = _context.Database.SqlQuery<CustomerTastesReportViewModel>(sqlQuery, parameters.ToArray())
                                 .ToList();

            var catalogueList = (from a in result
                                 group a by new { a.MaSAP, a.MaSP, a.TenSP } into g
                                 select new CustomerTasteViewModel
                                 {
                                     ERPProductCode = g.Key.MaSAP,
                                     ProductCode = g.Key.MaSP,
                                     ProductName = g.Key.TenSP,
                                     Liked = g.Count(),
                                 }).ToList();
            return catalogueList;
        }

        public List<CustomerTastesReportViewModel> GetCustomerTastesReport(CustomerTastesSearchViewModel searchViewModel)
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
                //TOP màu like
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input,
                    ParameterName = "TOP",
                    Value = searchViewModel.TOP ?? (object)DBNull.Value,
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

        public List<CustomerTasteViewModel> Search(CustomerTasteSearchViewModel searchViewModel)
        {
            if (searchViewModel.ToDate != null && searchViewModel.ToDate.HasValue)
            {
                searchViewModel.ToDate = searchViewModel.ToDate.Value.AddDays(1).AddSeconds(-1);
            }
            if (searchViewModel.StoreId != null)
            {
                searchViewModel.SaleOrgCode = _context.StoreModel.Where(p => p.StoreId == searchViewModel.StoreId)
                                                      .Select(p => p.SaleOrgCode).FirstOrDefault();
            }

            string sqlQuery = "EXEC [Customer].[usp_CustomerTastes] @ProfileId, @AppointmentId, @CompanyId, @SaleOrgCode, @FromDate, @ToDate, @ERPProductCode, @ProductCode, @ShowroomCode, @ProfileGroupCode, @SaleEmployeeCode";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter
                {
                    SqlDbType = SqlDbType.UniqueIdentifier,
                    Direction = ParameterDirection.Input,
                    ParameterName = "ProfileId",
                    Value = DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.UniqueIdentifier,
                    Direction = ParameterDirection.Input,
                    ParameterName = "AppointmentId",
                    Value = DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.UniqueIdentifier,
                    Direction = ParameterDirection.Input,
                    ParameterName = "CompanyId",
                    Value = searchViewModel.CompanyId ?? (object)DBNull.Value,
                },
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
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "ERPProductCode",
                    Value = searchViewModel.ERPProductCode ?? (object)DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "ProductCode",
                    Value = searchViewModel.ProductCode ?? (object)DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "ShowroomCode",
                    Value = DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "ProfileGroupCode",
                    Value = DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "SaleEmployeeCode",
                    Value = DBNull.Value,
                },
            };

            var result = _context.Database.SqlQuery<CustomerTasteViewModel>(sqlQuery, parameters.ToArray())
                                 .OrderByDescending(p => p.CreateDate).ToList();

            var customerTastesList = (from a in result
                                      group a by new { a.SaleOrgCode, a.SaleOrgName, a.ERPProductCode, a.ProductCode, a.ProductName } into g
                                      select new CustomerTasteViewModel
                                      {
                                          SaleOrgCode = g.Key.SaleOrgCode,
                                          SaleOrgName = g.Key.SaleOrgName,
                                          ERPProductCode = g.Key.ERPProductCode,
                                          ProductCode = g.Key.ProductCode,
                                          ProductName = g.Key.ProductName,
                                          Liked = g.Count(),
                                      }).ToList();
            return customerTastesList;
        }
    }
}
