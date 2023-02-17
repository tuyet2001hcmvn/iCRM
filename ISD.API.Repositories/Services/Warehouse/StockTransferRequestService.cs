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
    public interface IStockTransferRequestService
    {
        StockTransferRequestViewModel Create(StockTransferRequestCreateViewModel obj);
        StockTransferRequestViewModel GetById(Guid id);
        void Update(Guid id, StockTransferRequestUpdateViewModel obj);

        void DeleteDetail(Guid id);
        IEnumerable<StockTransferRequestViewModel> Search(Guid? companyId, Guid? storeId, int? stockTransferRequestCode, string salesEmployeeCode, Guid? fromStock, Guid? toStock, DateTime? fromDate, DateTime? toDate, bool? actived, int pageIndex, int pageSize, out int totalRow);
    }

    public class StockTransferRequestService : IStockTransferRequestService
    {
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;
        public StockTransferRequestService(IMapper mapper, UnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public StockTransferRequestViewModel Create(StockTransferRequestCreateViewModel obj)
        {
            StockTransferRequestModel newRequest = _mapper.Map<StockTransferRequestModel>(obj);
            List<StockTransferRequestDetailModel> newRequestDetail = _mapper.Map<List<StockTransferRequestDetailModel>>(obj.TransferRequestDetails);
            _unitOfWork.StockTransferRequestRepository.Add(newRequest);
            foreach (var detail in newRequestDetail)
            {
                detail.StockTransferRequestId = newRequest.Id;
                _unitOfWork.StockTransferRequestDetailRepository.Add(detail);
            }
            _unitOfWork.Save();
            var res = GetById(newRequest.Id);
            return res;
        }

        public StockTransferRequestViewModel GetById(Guid id)
        {
            var transferRequest = _unitOfWork.StockTransferRequestRepository.GetBy(s => s.Id == id, s => s.CreateByNavigation, s => s.LastEditByNavigation, s => s.ToStockNavigation, s => s.FromStockNavigation, s => s.Company, s => s.Store);
            var listTransferRequestDetail = _unitOfWork.StockTransferRequestDetailRepository.GetTransferRequestDetailByTransferRequestId(id);
            var res = _mapper.Map<StockTransferRequestViewModel>(transferRequest);
            res.TransferRequestDetails = _mapper.Map<List<StockTransferRequestDetailViewModel>>(listTransferRequestDetail);
            foreach (var item in res.TransferRequestDetails)
            {
                item.RequestQuantity ??= 0;
                item.TransferredQuantity = _unitOfWork.StockTransferRequestRepository.GetTransferredQuantityByPlanDate(res.Id,item.ProductId);
                item.TransferredQuantity = _unitOfWork.StockTransferRequestRepository.GetTransferredQuantityByPlanDate(res.Id,item.ProductId);
                item.RemainingQuantity = item.RequestQuantity - item.TransferredQuantity;
            }
            return res;
        }

       
        public void Update(Guid id, StockTransferRequestUpdateViewModel obj)
        {
            var update = _unitOfWork.StockTransferRequestRepository.GetById(id);
            if(update != null)
            {
                _mapper.Map<StockTransferRequestUpdateViewModel, StockTransferRequestModel>(obj, update);
                _unitOfWork.StockTransferRequestRepository.Update(update); //update master

                //Xoá danh sách cũ
                _unitOfWork.StockTransferRequestDetailRepository.DeleteTransferRequestDetailByTransferRequestId(id);
                //Cập nhật danh sách
                foreach (var item in obj.TransferRequestDetails)
                {
                    StockTransferRequestDetailModel RequestDetail = new StockTransferRequestDetailModel();
                    RequestDetail.Id = Guid.NewGuid();

                    RequestDetail.StockTransferRequestId = id;
                    RequestDetail.ProductId = item.ProductId;
                    RequestDetail.RequestQuantity = item.RequestQuantity;
                    RequestDetail.OfferQuantity = item.OfferQuantity;
                    RequestDetail.TransferredQuantity = item.TransferredQuantity;
                    _unitOfWork.StockTransferRequestDetailRepository.Add(RequestDetail);
                }
            }
            _unitOfWork.Save();
        }
        public IEnumerable<StockTransferRequestViewModel> Search(Guid? companyId,Guid? storeId, int? stockTransferRequestCode, string salesEmployeeCode,  Guid? fromStock, Guid? toStock, DateTime? fromDate,  DateTime? toDate, bool? actived, int pageIndex,  int pageSize, out int totalRow)
        {
            var list = _unitOfWork.StockTransferRequestRepository.Search(companyId, storeId, stockTransferRequestCode, salesEmployeeCode, fromStock, toStock, fromDate, toDate, actived, out totalRow);
            if (list.Count() != 0)
            {
                var pagging = list.Skip(pageIndex * pageSize - pageSize)
                         .Take(pageSize)
                         .ToList();
                return _mapper.Map<List<StockTransferRequestViewModel>>(pagging);
            }
            return _mapper.Map<List<StockTransferRequestViewModel>>(list);
        }

        public void DeleteDetail(Guid id)
        {
            var delete = _unitOfWork.StockTransferRequestDetailRepository.GetById(id);
            if (delete != null)
            {
                _unitOfWork.StockTransferRequestDetailRepository.Delete(delete.Id);
                _unitOfWork.Save();
            }

            
        }
    }
}
