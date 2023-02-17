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
    public interface ITransferService
    {
        void CreateByRequest(StockTransferCreateViewModel obj);
    }
    public class TransferService : ITransferService
    {
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;
        public TransferService(IMapper mapper, UnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public void CreateByRequest(StockTransferCreateViewModel obj)
        {
            var request = _unitOfWork.StockTransferRequestRepository.GetById(obj.StockTransferRequestId);
            if(request!=null)
            {
                TransferModel newTransfer = _mapper.Map<TransferModel>(request);             
                _mapper.Map<StockTransferCreateViewModel, TransferModel>(obj, newTransfer);
                newTransfer.DocumentDate = DateTime.Today;
                List<TransferDetailModel> listDetail = _mapper.Map<List<TransferDetailModel>>(obj.TransferDetails.Where(s=>s.TransferQuantity>0));
                //add data
                foreach(var item in listDetail)
                {
                    item.TransferId = newTransfer.TransferId;
                    item.DateKey = Convert.ToInt32(string.Format("{0:yyyyMMdd}", newTransfer.DocumentDate));
                    item.FromStockId = request.FromStock;
                    item.ToStockId = request.ToStock;
                }
                var dataForCheckStock = from p in listDetail
                                        group p by new { p.FromStockId, p.ProductId } into tmpList
                                        select new
                                        {
                                            tmpList.Key.FromStockId,
                                            tmpList.Key.ProductId,
                                            Sum = tmpList.Sum(p => p.Quantity),
                                        };
                //check ton kho
                foreach (var item in dataForCheckStock)
                {
                    var stockId = (Guid)item.FromStockId;
                    var productId = (Guid)item.ProductId;
                    var product = _unitOfWork.StockRepository.GetStockOnHandBy(stockId, productId);
                    if (item.Sum > product.Qty)
                    {
                        throw new Exception("Đã xảy ra lỗi: (" + product.ERPProductCode + ") Số lượng chuyển không thể lớn hơn số lượng tồn kho!");
                    }
                }

                _unitOfWork.TransferRepository.Add(newTransfer);
                //add new transfer detail and update quantity of transfer request detail
                foreach (var item in listDetail)
                {              
                    _unitOfWork.TransferDetailRepository.Add(item);
                    var requestDetail = _unitOfWork.StockTransferRequestDetailRepository.GetBy(s => s.ProductId == item.ProductId && s.StockTransferRequestId == request.Id);
                    requestDetail.TransferredQuantity = requestDetail.TransferredQuantity + item.Quantity;
                    _unitOfWork.StockTransferRequestDetailRepository.Update(requestDetail);
                }
                _unitOfWork.Save();
                CheckStockTransferRequestStatus(request.Id);
            }
        }
        private void CheckStockTransferRequestStatus(Guid id)
        {
            var isClose = true;
            var listDetail = _unitOfWork.StockTransferRequestDetailRepository.Get(s => s.StockTransferRequestId == id);
            foreach(var item in listDetail)
            {
                if(item.TransferredQuantity != item.RequestQuantity)
                {
                    isClose = false;
                    break;
                }
            }
            if(isClose)
            {
               var request = _unitOfWork.StockTransferRequestRepository.GetById(id);
                request.Actived = false;
                _unitOfWork.StockTransferRequestRepository.Update(request);
                _unitOfWork.Save();
            }           
        }
    }
}
