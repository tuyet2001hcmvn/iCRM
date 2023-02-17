using ISD.EntityModels;
using ISD.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.Repositories.Infrastructure.Extensions
{
    public static class WareHouseExtension
    {
        /// <summary>
        /// Map Stock Receiving Master từ View Model qua Entity Model
        /// </summary>
        /// <param name="model">Stock Receiving Master Model</param>
        /// <param name="viewModel">Stock Receiving View Model</param>
        public static void MapStockReceivingMaster(this StockReceivingMasterModel model, StockReceivingViewModel viewModel)
        {
            //1. GUID
            model.StockReceivingId = viewModel.StockReceivingId;
            //2. Mã phiếu nhập kho
            model.StockReceivingCode = viewModel.StockReceivingCode;
            //3. Ngày chứng từ
            model.DocumentDate = viewModel.DocumentDate;
            //4. Công ty
            model.CompanyId = viewModel.CompanyId;
            //5. Chi nhánh
            model.StoreId = viewModel.StoreId;
            //6. Nhân viên
            model.SalesEmployeeCode = viewModel.SalesEmployeeCode;
            //7. Nhà cung cấp
            model.ProfileId = viewModel.ProfileId;
            //8. Ghi chú
            model.Note = viewModel.Note;
        }
        public static void MapStockReceivingMaster(this StockReceivingMasterModel model, StockReceivingMasterViewModel viewModel)
        {
            //1. GUID
            model.StockReceivingId = viewModel.StockReceivingId;
            //2. Mã phiếu nhập kho
            model.StockReceivingCode = viewModel.StockReceivingCode;
            //3. Ngày chứng từ
            model.DocumentDate = viewModel.DocumentDate;
            //4. Công ty
            model.CompanyId = viewModel.CompanyId;
            //5. Chi nhánh
            model.StoreId = viewModel.StoreId;
            //6. Nhân viên
            model.SalesEmployeeCode = viewModel.SalesEmployeeCode;
            //7. Nhà cung cấp
            model.ProfileId = viewModel.ProfileId;
            //8. Ghi chú
            model.Note = viewModel.Note;
        }
        /// <summary>
        /// Ánh xạ Stock Receiving từ ViewModel qua EntityModel
        /// </summary>
        /// <param name="viewModel">Stock Receving Detail View Model</param>
        /// <param name="model">Stock Receving Detail Model</param>
        public static void MapStockRecevingDetail(this StockReceivingDetailModel model, StockReceivingDetailViewModel viewModel)
        {
            //1. Mã chi tiết
            model.StockReceivingDetailId = viewModel.StockReceivingDetailId;
            //2. Mã phiếu nhập kho
            model.StockReceivingId = viewModel.StockReceivingId;
            //3. Sản phẩm
            model.ProductId = viewModel.ProductId;
            //4. Kho
            model.StockId = viewModel.StockId;
            //5. Ngày chứng từ
            model.DateKey = viewModel.DateKey;
            //6. Số lượng
            model.Quantity = viewModel.Quantity;
            //7. Giá
            model.Price = viewModel.Price;
            //8. Thành tiền
            model.UnitPrice = viewModel.UnitPrice;
            //9. Ghi chú
            model.Note = viewModel.DetailNote;

        }

        public static void MapTransfer(this TransferModel model, TransferViewModel viewModel)
        {
            model.TransferId = viewModel.TransferId;
            model.TransferCode = viewModel.TransferCode;
            model.DocumentDate = viewModel.DocumentDate;
            model.CompanyId = viewModel.CompanyId;
            model.StoreId = viewModel.StoreId;
            model.SalesEmployeeCode = viewModel.SalesEmployeeCode;
            model.Note = viewModel.Note;
            //Thông tin người gửi
            model.SenderName = viewModel.SenderName;
            model.SenderPhone = viewModel.SenderPhone;
            model.SenderAddress = viewModel.SenderAddress;
            //Thông tin người nhận
            model.RecipientName = viewModel.RecipientName;
            model.RecipientPhone = viewModel.RecipientPhone;
            model.RecipientAddress = viewModel.RecipientAddress;
            model.RecipientCompany = viewModel.RecipientCompany;
        }

        public static void MapTranferDetail(this TransferDetailModel model, TransferDetailViewModel viewModel)
        {
            model.TransferDetailId = viewModel.TransferDetailId;
            model.TransferId = viewModel.TransferId;
            model.ProductId = viewModel.ProductId;
            model.FromStockId = viewModel.FromStockId;
            model.ToStockId = viewModel.ToStockId;
            model.DateKey = viewModel.DateKey;
            model.Quantity = viewModel.Quantity;
            model.Price = viewModel.Price;
            model.UnitPrice = viewModel.UnitPrice;
            model.Note = viewModel.DetailNote;
        }

        public static void MapDelivery(this DeliveryModel model, DeliveryViewModel viewModel)
        {
            model.DeliveryId = viewModel.DeliveryId;
            model.DeliveryCode = viewModel.DeliveryCode;
            model.DocumentDate = viewModel.DocumentDate;
            model.CompanyId = viewModel.CompanyId;
            model.StoreId = viewModel.StoreId;
            model.SalesEmployeeCode = viewModel.SalesEmployeeCode;
            model.ProfileId = viewModel.ProfileId;
            model.Note = viewModel.Note;

            model.SenderName = viewModel.SenderName;
            model.SenderAddress = viewModel.SenderAddress;
            model.SenderPhone = viewModel.SenderPhone;
            model.RecipientName = viewModel.RecipientName;
            model.RecipientAddress = viewModel.RecipientAddress;
            model.RecipientPhone = viewModel.RecipientPhone;
            model.RecipientCompany = viewModel.RecipientCompany;
            model.ShippingTypeCode = viewModel.ShippingTypeCode;

            model.TaskId = viewModel.TaskId;
            model.DeliveryType = viewModel.DeliveryType;
        }

        public static void MapDeliveryDetail(this DeliveryDetailModel model, DeliveryDetailViewModel viewModel)
        {
            model.DeliveryDetailId = viewModel.DeliveryDetailId;
            model.DeliveryId = viewModel.DeliveryId;
            model.StockId = viewModel.StockId;
            model.ProductId = viewModel.ProductId;
            model.DateKey = viewModel.DateKey;
            model.Quantity = viewModel.Quantity;
            model.Price = viewModel.Price;
            model.UnitPrice = viewModel.UnitPrice;
            model.Note = viewModel.DetailNote;
        }

    }
}

