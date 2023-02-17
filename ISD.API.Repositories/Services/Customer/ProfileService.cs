using AutoMapper;
using ISD.API.Core;
using ISD.API.EntityModels.Entities;
using ISD.API.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ISD.API.Repositories.Services
{
    public interface IProfileService
    {
        //void RequestCreateEcc(IFormFile file);
        void RequestCreateEcc(EmailViewModel emailViewModel);
        RequestEccEmailConfigModel GetEmailConfig(string senderName);
    }
    public class ProfileService : IProfileService
    {
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;
        private readonly IEmailHelper _emailHelper;
        public ProfileService(IMapper mapper, UnitOfWork unitOfWork, IEmailHelper emailHelper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _emailHelper = emailHelper;
        }

        //public void RequestCreateEcc(IFormFile attachment)
        //{
        //    var config = _unitOfWork.RequestEccEmailConfigRepository.GetEmailConfig();
        //    _emailHelper.Send(config.Host, config.Port.Value, config.FromEmail, config.FromEmailPassword);
        //}

        public void RequestCreateEcc(EmailViewModel emailViewModel)
        {
            var config = _unitOfWork.RequestEccEmailConfigRepository.GetEmailConfig();
            
            MailMessage email = new MailMessage();
            email.From = new MailAddress(config.FromEmail);
            email.Sender = new MailAddress(config.FromEmail);
            email.To.Add(new MailAddress(config.ToEmail));
            email.Body = emailViewModel.EmailContent;
            email.IsBodyHtml = true;
            email.BodyEncoding = Encoding.UTF8;
            email.Subject = emailViewModel.Subject;
            using (var ms = new MemoryStream())
            {
                emailViewModel.Attachments[0].CopyTo(ms);
                var fileBytes = ms.ToArray();
                Attachment att = new Attachment(new MemoryStream(fileBytes), emailViewModel.Attachments[0].FileName);
                email.Attachments.Add(att);
            }

            _emailHelper.Send(config.Host, config.Port.Value, config.FromEmail, config.FromEmailPassword, email);
        }

        public RequestEccEmailConfigModel GetEmailConfig(string senderName)
        {
            var res = _unitOfWork.RequestEccEmailConfigRepository.GetEmailConfig();
            
            res.EmailContent =res.EmailContent.Replace("##SenderName##", senderName).Replace("##RequestDate##",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            res.Subject = res.Subject.Replace("##SenderName##", senderName).Replace("##RequestDate##", DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
            return res;
        }
    }
}
