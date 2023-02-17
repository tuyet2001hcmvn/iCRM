using AutoMapper;
using ISD.API.EntityModels.Entities;
using ISD.API.ViewModels.MasterDataViewModels.FavoriteReportViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.API.Repositories.Services.MasterData
{
    public class FavoriteReportService : IFavoriteReportService
    {
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;
        public FavoriteReportService(IMapper mapper, UnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public void Create(FavoriteReportCreateViewModel obj)
        {
            var exist = _unitOfWork.FavoriteReportRepository.Get(s => s.PageId == obj.PageId && s.AccountId == obj.AccountId).FirstOrDefault();
            if(exist==null)
            {
                FavoriteReportModel newFavoriteReport = _mapper.Map<FavoriteReportModel>(obj);
                _unitOfWork.FavoriteReportRepository.Add(newFavoriteReport);
                _unitOfWork.Save();
            }            
        }

        public void Delete(FavoriteReportDeleteViewModel obj)
        {
            var exist = _unitOfWork.FavoriteReportRepository.Get(s => s.PageId == obj.PageId && s.AccountId == obj.AccountId).FirstOrDefault();
            if (exist != null)
            {
                _unitOfWork.FavoriteReportRepository.Delete(exist.Id);
                _unitOfWork.Save();
            }
        }

        public IEnumerable<FavoriteReportViewModel> GetFavoriteReport(Guid accountId)
        {
            var listFavorite = _mapper.Map<List<PageModel>, List<FavoriteReportViewModel>>(_unitOfWork.FavoriteReportRepository.GetFavoriteReport(accountId).ToList());
           foreach(var report in listFavorite)
            {
                report.IsFavorite = true;
            }
            return listFavorite;
        }

        public IEnumerable<FavoriteReportViewModel> Search(Guid accountId, string reportName, int pageIndex, int pageSize, out int totalRow)
        {
            var listAll = _unitOfWork.FavoriteReportRepository.GetAllReportWithPermission(accountId, reportName, out totalRow).Skip(pageIndex * pageSize - pageSize).Take(pageSize).ToList();
            var listFavorite = _unitOfWork.FavoriteReportRepository.GetFavoriteReport(accountId).ToList();           
            var listAllMap= _mapper.Map<List<PageModel>,List<FavoriteReportViewModel>>(listAll);
            foreach(var f in listFavorite)
            {
                foreach(var a in listAllMap)
                {
                    if (a.PageId == f.PageId)
                    {
                        a.IsFavorite = true;
                        break;
                    }
                }
            }
            return listAllMap;
        }
    }
}
