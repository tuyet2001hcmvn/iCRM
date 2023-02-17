using AutoMapper;
using ISD.API.EntityModels.Entities;
using ISD.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.API.Repositories.Services
{
    public interface ITemplateAndGiftCampaignService
    {
        TemplateAndGiftCampaignViewModel Create(TemplateAndGiftCampaignCreateModel obj);
        List<TemplateAndGiftCampaignViewModel> Search(int? templateAndGiftCampaignCode, string templateAndGiftCampaignName, int pageIndex, int pageSize, out int totalRow);
        TemplateAndGiftCampaignViewModel GetById(Guid id);
        List<TemplateAndGiftMemberViewModel> GetMemberByCampaign(Guid campaignId, string profileCode, string profileName, int pageIndex, int pageSize, out int totalRow);
        TemplateAndGiftMemberViewModel GetMember(Guid campaignId, Guid profileId);
        void Update(TemplateAndGiftCampaignEditModel obj, Guid id);
    }
    public class TemplateAndGiftCampaignService : ITemplateAndGiftCampaignService
    {
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;
        public TemplateAndGiftCampaignService(IMapper mapper, UnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public TemplateAndGiftCampaignViewModel Create(TemplateAndGiftCampaignCreateModel obj)
        {
            TemplateAndGiftCampaignModel newCampaign = _mapper.Map<TemplateAndGiftCampaignModel>(obj);         
            _unitOfWork.TemplateAndGiftCampaignRepository.Add(newCampaign);
            _unitOfWork.Save();          
            return _mapper.Map<TemplateAndGiftCampaignViewModel>(newCampaign);
        }

        public List<TemplateAndGiftCampaignViewModel> Search(int? templateAndGiftCampaignCode, string templateAndGiftCampaignName, int pageIndex, int pageSize, out int totalRow)
        {
            var list = _unitOfWork.TemplateAndGiftCampaignRepository.Search(templateAndGiftCampaignCode, templateAndGiftCampaignName, out totalRow);
            if (list.Count() != 0)
            {
                var pagging = list.Skip(pageIndex * pageSize - pageSize)
                         .Take(pageSize)
                         .ToList();
                return _mapper.Map<List<TemplateAndGiftCampaignViewModel>>(pagging);
            }
            return _mapper.Map<List<TemplateAndGiftCampaignViewModel>>(list);
        }
        public List<TemplateAndGiftMemberViewModel> GetMemberByCampaign(Guid campaignId, string profileCode,  string profileName, int pageIndex, int pageSize, out int totalRow)
        {
            var list = _unitOfWork.TemplateAndGiftMemberRepository.GetMemberByCampaign(campaignId, profileCode, profileName, out totalRow);
            if (list.Count() != 0)
            {
                var pagging = list.Skip(pageIndex * pageSize - pageSize)
                         .Take(pageSize)
                         .ToList();
                return _mapper.Map<List<TemplateAndGiftMemberViewModel>>(pagging);
            }
            return _mapper.Map<List<TemplateAndGiftMemberViewModel>>(list);
        }
         public TemplateAndGiftMemberViewModel GetMember(Guid campaignId, Guid profileId)
        {
            var res = _unitOfWork.TemplateAndGiftMemberRepository.GetMember(campaignId, profileId);
            return res;
        }
        public TemplateAndGiftCampaignViewModel GetById(Guid id)
        {
            var res = _unitOfWork.TemplateAndGiftCampaignRepository.GetBy(s=>s.Id==id, s=>s.CreateByNavigation,s=>s.LastEditByNavigation);
            return _mapper.Map<TemplateAndGiftCampaignViewModel>(res);
        }
        public void Update(TemplateAndGiftCampaignEditModel obj, Guid id)
        {
            TemplateAndGiftCampaignModel campaign = _unitOfWork.TemplateAndGiftCampaignRepository.GetById(id);
            if(campaign !=null)
            {
                _mapper.Map(obj, campaign);
                _unitOfWork.TemplateAndGiftCampaignRepository.Update(campaign);
                _unitOfWork.Save();
            }
           
        }
    }
}
