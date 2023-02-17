using ISD.API.ViewModels.MasterDataViewModels.FavoriteReportViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.API.Repositories.Services.MasterData
{
    public interface IFavoriteReportService
    {
        IEnumerable<FavoriteReportViewModel> Search(Guid accountId,string reportName, int pageIndex, int pageSize, out int totalRow);
        IEnumerable<FavoriteReportViewModel> GetFavoriteReport(Guid accountId);
        void Create(FavoriteReportCreateViewModel obj);
        void Delete(FavoriteReportDeleteViewModel obj);

    }
}
