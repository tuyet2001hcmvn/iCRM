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
    public class CertificateACRepository
    {
        private EntityDataContext _context;
        public CertificateACRepository(EntityDataContext dataContext)
        {
            _context = dataContext;
        }

        /// <summary>
        /// Danh sách Địa chỉ của khách hàng
        /// Địa chỉ chính: Address trong ProfileModel
        /// Địa chỉ còn lại: CertificateACModel
        /// </summary>
        /// <param name="profileId"></param>
        /// <returns></returns>
        public List<CertificateACViewModel> GetAll(Guid? profileId)
        {
            var certificateACList = (from a in _context.CertificateACModel
                                         //Create User
                                     join acc in _context.AccountModel on a.CreateBy equals acc.AccountId
                                     join s in _context.SalesEmployeeModel on acc.EmployeeCode equals s.SalesEmployeeCode
                                     //LastEditBy
                                     join eTemp in _context.AccountModel on a.LastEditBy equals eTemp.AccountId into eList
                                     from e in eList.DefaultIfEmpty()
                                     join esTemp in _context.SalesEmployeeModel on e.EmployeeCode equals esTemp.SalesEmployeeCode into esList
                                     from es in esList.DefaultIfEmpty()
                                     where a.ProfileId == profileId
                                     orderby a.EndDate descending, a.StartDate descending
                                     select new CertificateACViewModel
                                     {
                                         CertificateId = a.CertificateId,
                                         Content = a.Content,
                                         ProfileId = a.ProfileId,
                                         StartDate = a.StartDate,
                                         EndDate = a.EndDate,
                                         CreateTime = a.CreateTime,
                                         LastEditTime = a.LastEditTime,
                                         CreateByName = s.SalesEmployeeName,
                                         LastEditByName = es.SalesEmployeeName
                                     }).ToList();

            return certificateACList;
        } 
        
       
        public List<CustomerCertificateACReportViewModel> GetAllForReport(CustomerCertificateACReportSearchViewModel searchViewModel, string CurrentCompanyCode)
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

            string sqlQuery = "EXEC [Report].[usp_CustomerCertificateACReport] @ProfileId, @SalesEmployeeCode,  @StartFromDate, @StartToDate,  @EndFromDate, @EndToDate, @CurrentCompanyCode, @Content";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter
                {
                    SqlDbType = SqlDbType.UniqueIdentifier,
                    Direction = ParameterDirection.Input,
                    ParameterName = "ProfileId",
                    Value = searchViewModel.ProfileId ?? (object)DBNull.Value
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
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "StartFromDate",
                    Value = searchViewModel.StartFromDate ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "StartToDate",
                    Value = searchViewModel.StartToDate ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "EndFromDate",
                    Value = searchViewModel.EndFromDate ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "EndToDate",
                    Value = searchViewModel.EndToDate ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "CurrentCompanyCode",
                    Value = CurrentCompanyCode ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "Content",
                    Value = searchViewModel.Content ?? (object)DBNull.Value
                }
            };

            var data = _context.Database.SqlQuery<CustomerCertificateACReportViewModel>(sqlQuery, parameters.ToArray()).ToList();

            return data;
        }

        public CertificateACViewModel GetById(Guid? certificateId)
        {
            var certificateAC = (from a in _context.CertificateACModel
                                         //Create User
                                     join acc in _context.AccountModel on a.CreateBy equals acc.AccountId
                                     join s in _context.SalesEmployeeModel on acc.EmployeeCode equals s.SalesEmployeeCode
                                     //LastEditBy
                                     join eTemp in _context.AccountModel on a.LastEditBy equals eTemp.AccountId into eList
                                     from e in eList.DefaultIfEmpty()
                                     join esTemp in _context.SalesEmployeeModel on e.EmployeeCode equals esTemp.SalesEmployeeCode into esList
                                     from es in esList.DefaultIfEmpty()
                                     where a.CertificateId == certificateId
                                     orderby a.EndDate descending, a.StartDate descending
                                     select new CertificateACViewModel
                                     {
                                         CertificateId = a.CertificateId,
                                         Content = a.Content,
                                         ProfileId = a.ProfileId,
                                         StartDate = a.StartDate,
                                         EndDate = a.EndDate,
                                         CreateTime = a.CreateTime,
                                         LastEditTime = a.LastEditTime,
                                         CreateByName = s.SalesEmployeeName,
                                         LastEditByName = es.SalesEmployeeName
                                     }).FirstOrDefault();
            return certificateAC;
        }

        /// <summary>
        /// Thêm mới địa chỉ
        /// 1. Nếu       tick "Địa chỉ chính" (isMain) thì update Address trong ProfileModel, lấy Address hiện tại trong ProfileModel lưu vào CertificateACModel
        /// 2. Nếu không tick "Địa chỉ chính" (isMain) thì create CertificateACModel
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public CertificateACModel Create(CertificateACModel viewModel)
        {
            viewModel.CertificateId = Guid.NewGuid();
            _context.Entry(viewModel).State = EntityState.Added;
            return viewModel;
        }

        /// <summary>
        /// Cập nhật địa chỉ
        /// 1. Nếu cập nhật địa chỉ thường thành địa chỉ chính => update Address trong ProfileModel, xóa địa chỉ chính (nếu có) trong CertificateACModel
        /// 2. Nếu cập nhật địa chỉ thường (không cập nhật isMain) => cập nhật CertificateACModel
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public bool Update(CertificateACModel viewModel)
        {
            var certificateInDB = _context.CertificateACModel.FirstOrDefault(p => p.CertificateId == viewModel.CertificateId);
            if (certificateInDB != null)
            {
                certificateInDB.Content = viewModel.Content;
                certificateInDB.StartDate = viewModel.StartDate;
                certificateInDB.EndDate = viewModel.EndDate;
                certificateInDB.LastEditBy = viewModel.LastEditBy;
                certificateInDB.LastEditTime = viewModel.LastEditTime;
                _context.Entry(certificateInDB).State = EntityState.Modified;
                return true;
            }
            return false;
        }
    }
}
