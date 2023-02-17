using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.API.ViewModels
{
    public class StockTransferRequestViewModel
    {
        public Guid Id { get; set; }
        public int? StockTransferRequestCode { get; set; }
        public Guid? FromStock { get; set; }
        public string FromStockName { get; set; }
        public string FromStockCode { get; set; }
        public Guid? ToStock { get; set; }
        public string ToStockName { get; set; }
        public string ToStockCode { get; set; }
        public Guid? CompanyId { get; set; }
        public string CompanyName { get; set; }
        public Guid? StoreId { get; set; }
        public string StoreName { get; set; }
        public string Note { get; set; }
        public bool? IsDelete { get; set; }
        public bool? Actived { get; set; }
        public Guid? CreateBy { get; set; }
        public string CreateByName { get; set; }
        public DateTime? CreateTime { get; set; }
        public Guid? LastEditBy { get; set; }
        public string LastEditByName { get; set; }
        public DateTime? LastEditTime { get; set; }     
        public Guid? DeletedBy { get; set; }
        public DateTime? DeletedTime { get; set; }
        public string SalesEmployeeCode { get; set; }
        public string DocumentDate { get; set; }
        public string FromPlanDate { get; set; }
        public string ToPlanDate { get; set; }
        public string SenderName { get; set; }
        public string SenderPhone { get; set; }
        public string RecipientName { get; set; }
        public string RecipientPhone { get; set; }
        public string RecipientAddress { get; set; }
        public string RecipientCompany { get; set; }
        public string SenderAddress { get; set; }
        public string DeletedReason { get; set; }
        public List<StockTransferRequestDetailViewModel> TransferRequestDetails { get; set; }
    }
}
