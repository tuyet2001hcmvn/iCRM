using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.API.Repositories.Services
{
    public interface IStockTransferRequestDetailService
    {
    }
    public class StockTransferRequestDetailService : IStockTransferRequestDetailService
    {
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;
        public StockTransferRequestDetailService(IMapper mapper, UnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
    }
}
