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
    public class EmailAccountService : IEmailAccountService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public EmailAccountService(UnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public List<EmailAccountViewModel> GetEmails()
        {
            var listEntity = _unitOfWork.EmailAccountRepository.GetAll().ToList();
            var listView = _mapper.Map<List<EmailAccountModel>, List<EmailAccountViewModel>>(listEntity);
            return listView;
        }
    }
}
