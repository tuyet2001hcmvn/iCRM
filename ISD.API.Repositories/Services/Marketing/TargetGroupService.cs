using AutoMapper;
using ISD.API.EntityModels.Entities;
using ISD.API.ViewModels;
using ISD.API.ViewModels.CustomExceptions;
using ISD.API.ViewModels.MarketingViewModels.TargetGroupViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ISD.API.Repositories.Services.Marketing
{
    public class TargetGroupService : ITargetGroupService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public TargetGroupService(UnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
     
        public TargetGroupViewViewModel Create(TargetGroupCreateViewModel obj)
        {
            TargetGroupModel newTargetGroup = _mapper.Map<TargetGroupModel>(obj);
            _unitOfWork.TargetGroupRepository.Add(newTargetGroup);
            _unitOfWork.Save();
            return _mapper.Map<TargetGroupViewViewModel>(newTargetGroup);
        }

        public List<TargetGroupViewModel> GetAll(string Type)
        {
            var list = _unitOfWork.TargetGroupRepository.GetAll(Type).OrderByDescending(C => C.TargetGroupCode).ToList();
            return _mapper.Map<List<TargetGroupModel>, List<TargetGroupViewModel>>(list);
        }

        public TargetGroupViewViewModel GetById(Guid id)
        {         
            TargetGroupModel exists = _unitOfWork.TargetGroupRepository.GetBy(m=>m.Id==id,m=>m.CreateByNavigation,m=>m.LastEditByNavigation);
            if (exists != null)
            {
                TargetGroupViewViewModel res = _mapper.Map<TargetGroupViewViewModel>(exists);
                return res;
            }
            else
            {             
                throw new ResourceNotFoundException("Target group {0} was not found", id);          
            }
            
        }

        public int GetTotalRow(int? targetGroupCode, string targetGroupName, bool? actived, string type)
        {
            return _unitOfWork.TargetGroupRepository.GetTotalRow(targetGroupCode, targetGroupName, actived, type);
        }

        public List<TargetGroupViewViewModel> Search(int? targetGroupCode, string targetGroupName, bool? actived, int page, int pageSize, string type)
        {
            var targetGroupList = _unitOfWork.TargetGroupRepository.Search(targetGroupCode, targetGroupName, actived, page,pageSize, type).Select(t => new TargetGroupViewViewModel
            {
                Id = t.Id,
                TargetGroupCode = t.TargetGroupCode,
                TargetGroupName = t.TargetGroupName,
                Actived = t.Actived,
                CreateTime=t.CreateTime,
                CreateBy=t.CreateByNavigation.UserName
            }).ToList();
            foreach(var group in targetGroupList)
            {
                group.InternalCustomerQuantity = _unitOfWork.MemberOfTargetGroupRepository.GetAll().Where(s => s.TargetGroupId == group.Id).Count();
                group.ExternalCustomerQuantity = _unitOfWork.MemberOfExternalProfileTargetGroupRepository.Get(s => s.TargetGroupId == group.Id).Count();
            }    
            return targetGroupList;
        }
        public void Update(TargetGroupEditViewModel obj, Guid id)
        {
            TargetGroupModel targetGroup = _unitOfWork.TargetGroupRepository.GetById(id);
            if (targetGroup == null)
            {
                throw new ResourceNotFoundException("Target group {0} was not found", id);
            }
            _mapper.Map<TargetGroupEditViewModel, TargetGroupModel>(obj, targetGroup);
            _unitOfWork.TargetGroupRepository.Update(targetGroup);
            _unitOfWork.Save();
        }     
    }
}
