using AutoMapper;
using ISD.API.EntityModels.Entities;
using ISD.API.ViewModels.CustomExceptions;
using ISD.API.ViewModels.UtilitiesViewModel.SearchTemplateViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.API.Repositories.Services.Utilities
{
    public interface ISearchTemplateService
    {
        SearchTemplateViewModel Create(SearchTemplateCreateViewModel obj);
        void Update(Guid id, SearchTemplateEditViewModel obj);
        void Delete(Guid id);
        SearchTemplateViewModel GetById(Guid id);
        List<SearchTemplateViewModel> GetByAccount(Guid AccountId, Guid PageId);

    }
    public class SearchTemplateService : ISearchTemplateService
    {
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;
        public SearchTemplateService(IMapper mapper, UnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public SearchTemplateViewModel Create(SearchTemplateCreateViewModel obj)
        {
            var newSearchTemplate = _mapper.Map<SearchTemplateModel>(obj);

            _unitOfWork.SearchTemplateRepository.Add(newSearchTemplate);
            _unitOfWork.Save();
            return _mapper.Map<SearchTemplateViewModel>(newSearchTemplate);
        }

        public void Delete(Guid id)
        {
            var exist = _unitOfWork.SearchTemplateRepository.GetById(id);
            if (exist != null)
            {
                _unitOfWork.SearchTemplateRepository.Delete(exist.SearchTemplateId);
                _unitOfWork.Save();
            }
        }

        public List<SearchTemplateViewModel> GetByAccount(Guid AccountId, Guid PageId)
        {
            var searchTempList = _unitOfWork.SearchTemplateRepository.GetByAccount(AccountId, PageId).ToList();
            return _mapper.Map<List<SearchTemplateModel>, List<SearchTemplateViewModel>>(searchTempList);
        }

        public SearchTemplateViewModel GetById(Guid id)
        {
            SearchTemplateModel model = _unitOfWork.SearchTemplateRepository.GetBy(m => m.SearchTemplateId == id);
            if (model == null)
            {
                throw new ResourceNotFoundException("Template {0} was not found", id);
            }
            return _mapper.Map<SearchTemplateViewModel>(model);
        }

        public void Update(Guid id, SearchTemplateEditViewModel obj)
        {
            SearchTemplateModel model = _unitOfWork.SearchTemplateRepository.GetById(id);
            if (model == null)
            {
                throw new ResourceNotFoundException("Template {0} was not found", id);
            }
            _mapper.Map<SearchTemplateEditViewModel, SearchTemplateModel>(obj, model);

            _unitOfWork.SearchTemplateRepository.Update(model);
            _unitOfWork.Save();
        }
    }
}
