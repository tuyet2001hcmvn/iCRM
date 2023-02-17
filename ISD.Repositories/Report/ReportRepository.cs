using ISD.EntityModels;
using ISD.Extensions;
using ISD.Repositories.Infrastructure.Extensions;
using ISD.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.Repositories
{
    public class ReportRepository
    {
        EntityDataContext _context;
        RepositoryLibrary _repositoryLibrary;
        /// <summary>
        /// Khởi tạo repository
        /// </summary>
        /// <param name="dataContext">EntityDataContext</param>
        public ReportRepository(EntityDataContext dataContext)
        {
            _context = dataContext;
            _repositoryLibrary = new RepositoryLibrary();
        }

        /// <summary>
        /// Báo cáo danh sách khách hàng
        /// </summary>
        /// <returns>ProfileReportResultViewModel</returns>
        public List<ProfileReportResultViewModel> ProfileListReport(ProfileReportSearchViewModel searchViewModel)
        {
            List<ProfileReportResultViewModel> result = new List<ProfileReportResultViewModel>();

            result = _context.Database.SqlQuery<ProfileReportResultViewModel>("EXEC [dbo].[rpt_ProfileListReport] @FromDate, @ToDate, @UserName, @CustomerSourceCode, @CustomerTypeCode, @CustomerGroupCode, @CustomerCareerCode, @TaskStatusId, @CurrentCompanyCode, @CurrentSaleOrg, @CurrentUserName, @isAgency",
                        new SqlParameter("@FromDate", _repositoryLibrary.VNStringToDateTime(searchViewModel.FromDate) ?? (object)DBNull.Value),
                        new SqlParameter("@ToDate", _repositoryLibrary.VNStringToDateTime(searchViewModel.ToDate) ?? (object)DBNull.Value),
                        new SqlParameter("@UserName", searchViewModel.UserName ?? (object)DBNull.Value),
                        new SqlParameter("@CustomerSourceCode", searchViewModel.CustomerSourceCode ?? (object)DBNull.Value),
                        new SqlParameter("@CustomerTypeCode", searchViewModel.CustomerTypeCode ?? (object)DBNull.Value),
                        new SqlParameter("@CustomerGroupCode", searchViewModel.CustomerGroupCode ?? (object)DBNull.Value),
                        new SqlParameter("@CustomerCareerCode", searchViewModel.CustomerCareerCode ?? (object)DBNull.Value),
                        new SqlParameter("@TaskStatusId", searchViewModel.TaskStatusId ?? (object)DBNull.Value),
                        new SqlParameter("@CurrentCompanyCode", searchViewModel.CurrentCompanyCode ?? (object)DBNull.Value),
                        new SqlParameter("@CurrentSaleOrg", searchViewModel.CurrentSaleOrg ?? (object)DBNull.Value),
                        new SqlParameter("@CurrentUserName", searchViewModel.CurrentUserName ?? (object)DBNull.Value),
                        new SqlParameter("@isAgency", searchViewModel.isAgency ?? (object)DBNull.Value)
                    ).ToList();
            return result;
        }
    }
}
