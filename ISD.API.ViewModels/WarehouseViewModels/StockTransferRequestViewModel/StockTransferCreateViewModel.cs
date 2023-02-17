using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.API.ViewModels
{
    public class StockTransferCreateViewModel
    {
        public Guid? StockTransferRequestId { get; set; }     
        public string Note { get; set; }
        // public bool? IsDelete { get; set; }
        //  public bool? Actived { get; set; }
        public Guid? CreateBy { get; set; }
        public string SalesEmployeeCode { get; set; }
        public string SenderName { get; set; }
        public string SenderPhone { get; set; }
        public string RecipientName { get; set; }
        public string RecipientPhone { get; set; }
        public string RecipientAddress { get; set; }
        public string RecipientCompany { get; set; }
        public string SenderAddress { get; set; }
        public List<StockTransferDetailViewModel> TransferDetails { get; set; }
    }
}
