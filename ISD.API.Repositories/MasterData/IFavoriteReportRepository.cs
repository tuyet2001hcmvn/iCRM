using ISD.API.EntityModels.Entities;
using ISD.API.ViewModels.MasterDataViewModels.FavoriteReportViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.API.Repositories.MasterData
{
    public interface IFavoriteReportRepository
    {
        IQueryable<PageModel> GetAllReportWithPermission(Guid accountId, string reportName, out int totalRow);
        IQueryable<PageModel> GetFavoriteReport(Guid accountId);
    }
}
