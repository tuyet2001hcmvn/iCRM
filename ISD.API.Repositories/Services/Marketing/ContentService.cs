using AutoMapper;
using ISD.API.Core;
using ISD.API.EntityModels.Entities;
using ISD.API.ViewModels.CustomExceptions;
using ISD.API.ViewModels.MarketingViewModels.ContentViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ISD.API.Repositories.Services.Marketing
{
    public class ContentService : IContentService
    {
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;
        public ContentService(IMapper mapper,UnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public ContenViewViewModel Create(ContentCreateViewModel content)
        {
            if (content.CatalogCode != Constant.SMS)
            {
                var fromEmailAccount = _unitOfWork.EmailAccountRepository.GetById(content.FromEmailAccountId);
                if (fromEmailAccount == null)
                {
                    throw new ResourceNotFoundException("Email Account {0} was not found", content.FromEmailAccount);
                }
            }
            
            ContentModel newContent = _mapper.Map<ContentModel>(content);
            // newContent.FromEmailAccountId = fromEmailAccount.Id;
            //newContent.Id = Guid.NewGuid();
            //newContent.Actived = true;
            //newContent.CreateTime = DateTime.Now;
            _unitOfWork.ContentRepository.Add(newContent);
            _unitOfWork.Save();
            ContenViewViewModel res = _mapper.Map<ContenViewViewModel>(newContent);
            return res;
        }

        public List<ContenViewViewModel> GetAll(string Type)
        {
            var list = _unitOfWork.ContentRepository.GetAll(Type).OrderByDescending(C=>C.ContentCode).ToList();
            return _mapper.Map<List<ContentModel>, List<ContenViewViewModel>>(list);
        }

        public ContenViewViewModel GetById(Guid id)
        {
            ContentModel content = _unitOfWork.ContentRepository.GetBy(m => m.Id == id, m => m.CreateByNavigation, m => m.LastEditByNavigation);
            if (content == null)
            {
                throw new ResourceNotFoundException("Content {0} was not found", id);
            }
            ContenViewViewModel view = _mapper.Map<ContenViewViewModel>(content);
            if (view.CatalogCode != Constant.SMS)
            {
                var fromEmailAccount = _unitOfWork.EmailAccountRepository.GetById(content.FromEmailAccountId);
                view.FromEmailAccount = fromEmailAccount.Address;
                view.Domain = fromEmailAccount.Domain;
            }
            return view;
        }

        public int GetTotalRow(int? contentCode, string contentName, bool? actived, string Type)
        {
            return _unitOfWork.ContentRepository.Search(contentCode, contentName, actived, Type).Count();
        }

        public List<ContenViewViewModel> Search(int? contentCode, string contentName, bool? actived, int pageIndex, int pageSize, string Type)
        {
            var contents = _unitOfWork.ContentRepository.Search(contentCode, contentName, actived, Type)
                 .Skip(pageIndex * pageSize - pageSize)
                 .Take(pageSize)
                 .ToList();
            List<ContenViewViewModel> res = _mapper.Map<List<ContentModel>, List<ContenViewViewModel>>(contents);
            return res;
        }

        public void Update(Guid id, ContentEditViewModel obj)
        {
            var content = _unitOfWork.ContentRepository.GetById(id);
            if (content == null)
            {
                throw new ResourceNotFoundException("Content {0} was not found", id);
            }
            if (content.CatalogCode != Constant.SMS)
            {
                var fromEmailAccount = _unitOfWork.EmailAccountRepository.GetById(obj.FromEmailAccountId);
                if (fromEmailAccount == null)
                {
                    throw new ResourceNotFoundException("Email Account {0} was not found", obj.FromEmailAccount);
                }
            }
            _mapper.Map<ContentEditViewModel, ContentModel>(obj,content);
            //content.FromEmailAccountId = fromEmailAccount.Id;
            //content.LastEditTime = DateTime.Now;       
            _unitOfWork.ContentRepository.Update(content);
            _unitOfWork.Save();
        }
    }
}
