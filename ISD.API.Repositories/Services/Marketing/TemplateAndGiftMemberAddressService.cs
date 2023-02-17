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
    public interface ITemplateAndGiftMemberAddressService
    {
        void Create(TemplateAndGiftMemberAddressCreateModel obi);
    }
    public class TemplateAndGiftMemberAddressService : ITemplateAndGiftMemberAddressService
    {
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;
        public TemplateAndGiftMemberAddressService(IMapper mapper, UnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public void Create(TemplateAndGiftMemberAddressCreateModel obi)
        {
            foreach(var item in obi.ListDetail)
            {
                if(item.Id ==Guid.Empty)
                {
                    TemplateAndGiftMemberAddressModel newAddress = _mapper.Map<TemplateAndGiftMemberAddressModel>(item);
                    _unitOfWork.TemplateAndGiftMemberAddressRepository.Add(newAddress);
                }
            }
            _unitOfWork.Save();
        }
    }
}
