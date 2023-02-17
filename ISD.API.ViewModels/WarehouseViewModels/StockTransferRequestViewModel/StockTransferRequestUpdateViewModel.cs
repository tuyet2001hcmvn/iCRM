using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.API.ViewModels
{
    public class StockTransferRequestUpdateViewModel
    {
        //  public Guid Id { get; set; }
        //  public int? StockTransferRequestCode { get; set; }
        //public Guid? FromStock { get; set; }
        //public Guid? ToStock { get; set; }
        //public Guid? CompanyId { get; set; }
        //public Guid? StoreId { get; set; }
        public string Note { get; set; }
        // public bool? IsDelete { get; set; }
        //  public bool? Actived { get; set; }
        //public Guid? CreateBy { get; set; }
        public string SalesEmployeeCode { get; set; }
        public DateTime DocumentDate { get; set; }
        public DateTime? FromPlanDate { get; set; }
        public DateTime? ToPlanDate { get; set; }
        public string SenderName { get; set; }
        public string SenderPhone { get; set; }
        public string RecipientName { get; set; }
        public string RecipientPhone { get; set; }
        public string RecipientAddress { get; set; }
        public string RecipientCompany { get; set; }
        public string SenderAddress { get; set; }
        //public string DeletedReason { get; set; }
        //  public DateTime? CreateTime { get; set; }
        //  public Guid? LastEditBy { get; set; }
        //   public DateTime? LastEditTime { get; set; }
        //public List<StockTransferRequestDetailViewModel> TransferRequestDetails { get; set; }
        public List<StockTransferRequestDetailViewModel> TransferRequestDetails { get; set; }
    }
}
