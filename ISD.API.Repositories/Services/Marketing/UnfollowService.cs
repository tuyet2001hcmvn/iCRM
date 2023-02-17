using AutoMapper;
using ISD.API.EntityModels.Entities;
using ISD.API.ViewModels.MarketingViewModels.EmailAccountViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.API.Repositories.Services.Marketing
{
    public class UnfollowService : IUnfollowService
    {
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;
        public UnfollowService(IMapper mapper, UnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public void Create(UnfollowCreateViewModel obj)
        {
            if (obj.Email != null)
            {
                Unfollow isExist = _unitOfWork.UnfollowRepository.GetBy(S => S.Email == obj.Email);
                if (isExist == null)
                {
                    Unfollow entity = _mapper.Map<Unfollow>(obj);
                    _unitOfWork.UnfollowRepository.Add(entity);                
                    _unitOfWork.UnfollowRepository.Add(entity);
                    _unitOfWork.Save();
                }
            }
           

        }

        public void Delete(Guid id)
        {
            _unitOfWork.UnfollowRepository.Delete(id);
            _unitOfWork.Save();
        }

        public int GetTotalRow(string email,string companyCode)
        {
            return _unitOfWork.UnfollowRepository.Search(email, companyCode).Count();
        }

        public List<UnfollowViewModel> Search(string email,string companyCode, int pageIndex, int pageSize)
        {
            var list = _unitOfWork.UnfollowRepository.Search(email, companyCode);
            if (list.Count() != 0)
            {
                var res =list.Skip(pageIndex * pageSize - pageSize).Take(pageSize).ToList();
                return _mapper.Map<List<Unfollow>, List<UnfollowViewModel>>(res);
            }
            return _mapper.Map<List<Unfollow>, List<UnfollowViewModel>>(list.ToList());
        }
    }
}
