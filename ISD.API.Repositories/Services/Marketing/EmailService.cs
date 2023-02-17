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
    public class EmailService : IEmailService
    {
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;
        public EmailService(IMapper mapper, UnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public void TrackingOpened(Guid sendMailCalendarId)
        {
            SendMailCalendarModel email = _unitOfWork.SendMailCalendarRepository.GetById(sendMailCalendarId);
            if (email != null)
            {
                email.IsOpened = true;
                email.LastOpenedTime = DateTime.Now;
                _unitOfWork.Save();
            }
        }
        public void Unsubscribe(Guid sendMailCalendarId)
        {
            SendMailCalendarModel email = _unitOfWork.SendMailCalendarRepository.GetById(sendMailCalendarId);      
            if (email != null)
            {
                var campaign = _unitOfWork.CampaignRepository.GetById(email.CampaignId);
                if(campaign!=null)
                {
                    var content = _unitOfWork.ContentRepository.GetById(campaign.ContentId);
                    if(content !=null)
                    {
                        Unfollow isExist = _unitOfWork.UnfollowRepository.GetBy(S => S.Email == email.ToEmail);
                        if (isExist == null)
                        {
                            Unfollow newUnsubscribe = new Unfollow
                            {
                                Id = Guid.NewGuid(),
                                Email = email.ToEmail,
                                CompanyCode = content.CompanyCode
                            };
                            _unitOfWork.UnfollowRepository.Add(newUnsubscribe);
                            _unitOfWork.Save();
                        }
                    }    
                }    
                          
            }
        }

        
    }
}
