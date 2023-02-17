using ISD.Constant;
using ISD.EntityModels;
using ISD.Repositories.Infrastructure.Extensions;
using ISD.Resources;
using ISD.ViewModels;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

namespace ISD.Repositories
{
    public class CostOfUsingCatalogueComparedToApprovalRepository
    {
        private EntityDataContext _context;
        public CostOfUsingCatalogueComparedToApprovalRepository(EntityDataContext dataContext)
        {
            _context = dataContext;
        }

        
        public List<CostOfUsingCatalogueComparedToApprovalViewModel> GetAllForReport(CostOfUsingCatalogueComparedToApprovalSearchViewModel searchViewModel, string CurrentCompanyCode)
        {

            #region Nhân viên kinh doanh
            var tableSalesEmployeeCodeSchema = new List<SqlMetaData>(1)
            {
                new SqlMetaData("Code", SqlDbType.NVarChar, 50)
            }.ToArray();
            //And a table as a list of those records
            var tableSalesEmployeeCode = new List<SqlDataRecord>();
            if (searchViewModel.SalesEmployeeCode != null && searchViewModel.SalesEmployeeCode.Count > 0)
            {
                foreach (var a in searchViewModel.SalesEmployeeCode)
                {
                    var tableRow = new SqlDataRecord(tableSalesEmployeeCodeSchema);
                    tableRow.SetString(0, a);
                    tableSalesEmployeeCode.Add(tableRow);
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tableSalesEmployeeCodeSchema);
                tableSalesEmployeeCode.Add(tableRow);
            }
            #endregion
            #region Khu vực
            var saleOfficeCodeSchema = new List<SqlMetaData>(1)
            {
                new SqlMetaData("Code", SqlDbType.NVarChar, 50)
            }.ToArray();
            //And a table as a list of those records
            var tableSaleOfficeCode = new List<SqlDataRecord>();
            if (searchViewModel.SaleOfficeCode != null && searchViewModel.SaleOfficeCode.Count > 0)
            {
                foreach (var a in searchViewModel.SaleOfficeCode)
                {
                    var tableRow = new SqlDataRecord(saleOfficeCodeSchema);
                    tableRow.SetString(0, a);
                    tableSaleOfficeCode.Add(tableRow);
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(saleOfficeCodeSchema);
                tableSaleOfficeCode.Add(tableRow);
            }
            #endregion
            #region Phòng ban
            var rolesCodeSchema = new List<SqlMetaData>(1)
            {
                new SqlMetaData("Code", SqlDbType.NVarChar, 50)
            }.ToArray();
            //And a table as a list of those records
            var tableRolesCode = new List<SqlDataRecord>();
            if (searchViewModel.RolesCode != null && searchViewModel.RolesCode.Count > 0)
            {
                foreach (var a in searchViewModel.RolesCode)
                {
                    var tableRow = new SqlDataRecord(rolesCodeSchema);
                    tableRow.SetString(0, a);
                    tableRolesCode.Add(tableRow);
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(rolesCodeSchema);
                tableRolesCode.Add(tableRow);
            }
            #endregion
            
            #region Catalogue
            var productIdSchema = new List<SqlMetaData>(1)
            {
                new SqlMetaData("Id", SqlDbType.UniqueIdentifier)
            }.ToArray();
            //And a table as a list of those records
            var tableProductId = new List<SqlDataRecord>();
            List<Guid> productIdLst = new List<Guid>();
            if (searchViewModel.ProductId != null && searchViewModel.ProductId.Count > 0)
            {
                foreach (var r in searchViewModel.ProductId)
                {
                    var tableRow = new SqlDataRecord(productIdSchema);
                    tableRow.SetGuid(0, r);
                    if (!productIdLst.Contains(r))
                    {
                        productIdLst.Add(r);
                        tableProductId.Add(tableRow);
                    }
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(productIdSchema);
                tableProductId.Add(tableRow);
            }
            
            #endregion

            string sqlQuery = "EXEC [Report].[usp_CostOfUsingCatalogueComparedToApprovalReport] @CompanyId, @StoreId, @SaleOfficeCode, @SalesEmployeeCode,  @RolesCode, @ProductId, @PriceType, @FromDate, @ToDate";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter
                {
                    SqlDbType = SqlDbType.UniqueIdentifier,
                    Direction = ParameterDirection.Input,
                    ParameterName = "CompanyId",
                    Value = searchViewModel.CompanyId ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.UniqueIdentifier,
                    Direction = ParameterDirection.Input,
                    ParameterName = "StoreId",
                    Value = searchViewModel.StoreId?? (object)DBNull.Value
                },
               
                 new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "SaleOfficeCode",
                    TypeName = "[dbo].[StringList]", //Don't forget this one!
                    Value = tableSaleOfficeCode
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "SalesEmployeeCode",
                    TypeName = "[dbo].[StringList]", //Don't forget this one!
                    Value = tableSalesEmployeeCode
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "RolesCode",
                    TypeName = "[dbo].[StringList]", //Don't forget this one!
                    Value = tableRolesCode
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "ProductId",
                    TypeName = "[dbo].[GuidList]", //Don't forget this one!
                    Value = tableProductId
                }, 
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "PriceType",
                    Value = searchViewModel.PriceType?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "FromDate",
                    Value = searchViewModel.FromDate?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "ToDate",
                    Value = searchViewModel.ToDate?? (object)DBNull.Value
                },
            };

            var data = _context.Database.SqlQuery<CostOfUsingCatalogueComparedToApprovalViewModel>(sqlQuery, parameters.ToArray()).ToList();

            return data;
        }

    }
}
