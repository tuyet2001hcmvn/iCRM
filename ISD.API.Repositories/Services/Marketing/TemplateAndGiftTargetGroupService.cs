using AutoMapper;
using ISD.API.Core;
using ISD.API.EntityModels.Entities;
using ISD.API.ViewModels;
using ISD.API.ViewModels.CustomExceptions;
using ISD.API.ViewModels.MarketingViewModels.TargetGroupViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.API.Repositories.Services
{
    public interface ITemplateAndGiftTargetGroupService
    {
        TemplateAndGiftTargetGroupViewModels Create(TargetGroupCreateViewModel obj);
        List<TemplateAndGiftTargetGroupViewModels> Search(int? targetGroupCode, string targetGroupName, bool? actived, int pageIndex, int pageSize, out int totalRow);
        TemplateAndGiftTargetGroupViewModels GetById(Guid id);
        List<TemplateAndGiftMemberViewModel> GetMember(Guid id, int pageIndex, int pageSize, out int totalRow);
        void Import(IFormFile file, Guid targetGroupId);
        void Update(TargetGroupEditViewModel obj, Guid id);
        List<TemplateAndGiftTargetGroupViewModels> GetAll();
    }
    public class TemplateAndGiftTargetGroupService : ITemplateAndGiftTargetGroupService
    {
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;
        public TemplateAndGiftTargetGroupService(IMapper mapper, UnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;

        }
        public TemplateAndGiftTargetGroupViewModels Create(TargetGroupCreateViewModel obj)
        {
            TemplateAndGiftTargetGroupModel newTargetGroup = _mapper.Map<TemplateAndGiftTargetGroupModel>(obj);
            _unitOfWork.TemplateAndGiftTargetGroupRepository.Add(newTargetGroup);
            _unitOfWork.Save();
            return _mapper.Map<TemplateAndGiftTargetGroupViewModels>(newTargetGroup);
        }
        public List<TemplateAndGiftTargetGroupViewModels> Search(int? targetGroupCode, string targetGroupName, bool? actived, int pageIndex, int pageSize, out int totalRow)
        {
            var list = _unitOfWork.TemplateAndGiftTargetGroupRepository.Search(targetGroupCode, targetGroupName, actived, out totalRow);
            if (list.Count() != 0)
            {
                var pagging = list.Skip(pageIndex * pageSize - pageSize)
                         .Take(pageSize)
                         .ToList();
                return _mapper.Map<List<TemplateAndGiftTargetGroupViewModels>>(pagging);
            }
            return _mapper.Map<List<TemplateAndGiftTargetGroupViewModels>>(list);
        }
        public TemplateAndGiftTargetGroupViewModels GetById(Guid id)
        {
            var res = _unitOfWork.TemplateAndGiftTargetGroupRepository.GetBy(s=>s.Id == id, s=>s.CreateByNavigation, s =>s.LastEditByNavigation);
            if(res !=null)
            {
                return _mapper.Map<TemplateAndGiftTargetGroupViewModels>(res);
            }
            return null;
        }
        public List<TemplateAndGiftTargetGroupViewModels> GetAll()
        {
            var list = _unitOfWork.TemplateAndGiftTargetGroupRepository.Get(s => s.Actived != null);
            if (list != null)
            {
                return _mapper.Map<List<TemplateAndGiftTargetGroupViewModels>>(list);
            }
            return null;
        }
        public List<TemplateAndGiftMemberViewModel> GetMember(Guid id, int pageIndex, int pageSize, out int totalRow)
        {
            var list = _unitOfWork.TemplateAndGiftMemberRepository.GetMember(id, out totalRow);
            if (list.Count() != 0)
            {
                var pagging = list.Skip(pageIndex * pageSize - pageSize)
                         .Take(pageSize)
                         .ToList();
                return _mapper.Map<List<TemplateAndGiftMemberViewModel>>(pagging);
            }
            return _mapper.Map<List<TemplateAndGiftMemberViewModel>>(list);
        }
        public void Import(IFormFile file, Guid targetGroupId)
        {
            ExcelHelper excelHelper = new ExcelHelper(file);
            DataSet dataSet = excelHelper.GetDataSet();
            DataTable dt = new DataTable();
            dt = dataSet.Tables[0].Columns[1].Table;
            List<TemplateAndGiftMemberModel> list = new List<TemplateAndGiftMemberModel>();
            for (int i = 5; i < dt.Rows.Count; i++)
            {
                var profileId = Guid.Parse(dt.Rows[i][1].ToString());
                var existing = _unitOfWork.TemplateAndGiftMemberRepository.GetBy(s=>s.ProfileId == profileId && s.TemplateAndGiftTargetGroupId == targetGroupId);
                if (existing == null)
                {
                    CreateNewMember(profileId, targetGroupId);
                }               
            }
            _unitOfWork.Save();          
        }
        private void CreateNewMember(Guid profileId,Guid targetGroupId)
        {
            TemplateAndGiftMemberModel newMember = new TemplateAndGiftMemberModel
            {
                Id = Guid.NewGuid(),
                ProfileId = profileId,
                TemplateAndGiftTargetGroupId = targetGroupId
            };
            _unitOfWork.TemplateAndGiftMemberRepository.Add(newMember);
        }

        public void Update(TargetGroupEditViewModel obj, Guid id)
        {
            TemplateAndGiftTargetGroupModel targetGroup = _unitOfWork.TemplateAndGiftTargetGroupRepository.GetById(id);
            if (targetGroup == null)
            {
                throw new ResourceNotFoundException("Target group {0} was not found", id);
            }
            _mapper.Map(obj, targetGroup);
            _unitOfWork.TemplateAndGiftTargetGroupRepository.Update(targetGroup);
            _unitOfWork.Save();
        }
    }
}
