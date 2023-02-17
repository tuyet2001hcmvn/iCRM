using AutoMapper;
using ISD.API.Core;
using ISD.API.EntityModels.Entities;
using ISD.API.ViewModels.CustomExceptions;
using ISD.API.ViewModels.MarketingViewModels.CampaignViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ISD.API.Repositories.Services.Marketing
{
    public class CampaignService : ICampaignService
    {
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;
        public CampaignService(IMapper mapper, UnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public CampaignViewViewModel Create(CampaignCreateViewModel obj)
        {
            ValidateContenAndTargetGroup(obj.ContentId, obj.ContentName, obj.TargetGroupId, obj.TargetGroupName);
            CampaignModel newCampaign = _mapper.Map<CampaignModel>(obj);
            newCampaign.Status = _unitOfWork.CatalogRepository.GetBy(c => c.CatalogCode == Constant.CampaignStatus_Planned).CatalogId;
            _unitOfWork.CampaignRepository.Add(newCampaign);
            _unitOfWork.Save();
            _unitOfWork.CampaignRepository.CreateSendMailCalendarForNewCampaign(newCampaign.Id);          
            return _mapper.Map<CampaignViewViewModel>(newCampaign);
        }

        public void EmailIsOpened(Guid id)
        {
            SendMailCalendarModel email = _unitOfWork.SendMailCalendarRepository.GetById(id);
            if(email !=null)
            {
                email.IsOpened = true;
                _unitOfWork.Save();
            }
        }

        public CampaignViewViewModel GetById(Guid id)
        {
            CampaignModel model = _unitOfWork.CampaignRepository.GetBy(m => m.Id == id,m=>m.TargetGroup,m=>m.Content,m=>m.StatusNavigation);
            if (model == null)
            {
                throw new ResourceNotFoundException("Campaign {0} was not found", id);
            }
            CampaignViewViewModel view = _mapper.Map<CampaignViewViewModel>(model);
            var CreateBy = _unitOfWork.AccountRepository.GetById(Guid.Parse(view.CreateBy))?.EmployeeCode;
            var LastEditBy = _unitOfWork.AccountRepository.GetById(Guid.Parse(view.LastEditBy))?.EmployeeCode;
            view.CreateBy = _unitOfWork.AccountRepository.GetEmployee(CreateBy).SalesEmployeeShortName;
            if (LastEditBy != null)
            {
                view.LastEditBy = _unitOfWork.AccountRepository.GetEmployee(LastEditBy).SalesEmployeeShortName;

            }
            return view;
        }

        public List<CampaignStatusViewModel> GetCampaignStatus()
        {
            var listModel = _unitOfWork.CatalogRepository.GetCampaignStatus();
            return _mapper.Map<List<CampaignStatusViewModel>>(listModel);
        }

        public CampaignReportViewModel GetReportForCampiagn(Guid id)
        {
            var report = _unitOfWork.CampaignRepository.GetReportById(id);
            return report;
        }

        public int GetTotalRowSearch(int? campaignCode, string campaignName, string status, string type)
        {
            return _unitOfWork.CampaignRepository.Search(campaignCode, campaignName, status, type).Count();
        }

        public List<CampaignViewViewModel> Search(int? campaignCode, string campaignName, string status, int pageIndex, int pageSize, string type)
        {
            var models = _unitOfWork.CampaignRepository.Search(campaignCode, campaignName, status, type);
            if(models.Count()!=0)
            {
                var list = models.Skip(pageIndex * pageSize - pageSize)
                         .Take(pageSize)
                         .ToList();
                var campaign = _mapper.Map<List<CampaignModel>, List<CampaignViewViewModel>>(list);
                foreach (var item in campaign)
                {
                    var EmployeeCode = _unitOfWork.AccountRepository.GetById(Guid.Parse(item.CreateBy))?.EmployeeCode;
                    item.CreateBy = _unitOfWork.AccountRepository.GetEmployee(EmployeeCode).SalesEmployeeShortName;
                }
                return campaign;
            }
                        
            List<CampaignViewViewModel> res = _mapper.Map<List<CampaignModel>, List<CampaignViewViewModel>>(models.ToList());
            return res;
        }

        public void Update(Guid id, CampaignEditViewModel obj)
        {
            bool targetGroupIsChange = false;
            ValidateContenAndTargetGroup(obj.ContentId, obj.ContentName, obj.TargetGroupId, obj.TargetGroupName);
             CampaignModel model = _unitOfWork.CampaignRepository.GetById(id);
            if (model == null)
            {
                throw new ResourceNotFoundException("Campaign {0} was not found", id);
            }
            if (model.TargetGroupId != obj.TargetGroupId)
            {
                targetGroupIsChange = true;
            }
            if (obj.IsImmediately == true && obj.ScheduledToStart == null)
            {
                model.IsImmediately = true;
                model.ScheduledToStart = DateTime.Now;
                _mapper.Map<CampaignEditViewModel, CampaignModel>(obj, model);
            }
            else
            {
                if(obj.IsImmediately==false && obj.ScheduledToStart!=null )
                {
                    model.IsImmediately = false;
                    model.ScheduledToStart =obj.ScheduledToStart;
                    _mapper.Map<CampaignEditViewModel, CampaignModel>(obj, model);
                }
                else
                {
                    _mapper.Map<CampaignEditViewModel, CampaignModel>(obj, model);
                }
            }
            _mapper.Map<CampaignEditViewModel, CampaignModel>(obj, model);
            
            _unitOfWork.CampaignRepository.Update(model);
            _unitOfWork.Save();
            if (targetGroupIsChange)
            {
                _unitOfWork.CampaignRepository.UpdateSendMailCalendarByCampaign(id);
            }
        }
        private void ValidateContenAndTargetGroup(Guid contentId,string contentName, Guid targetGroupId,string targetGroupName)
        {
            ContentModel content = _unitOfWork.ContentRepository.GetById(contentId);
            if (content == null)
            {
                throw new ResourceNotFoundException("Content {0} was not found", contentName);
            }
            TargetGroupModel targetGroup = _unitOfWork.TargetGroupRepository.GetById(targetGroupId);
            if (targetGroup == null)
            {
                throw new ResourceNotFoundException("TargetGroup {0} was not found", targetGroupName);
            }
        }
    }
}
