using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.API.ViewModels
{
    public class StockTransferDetailViewModel
    {
        public Guid Id { get; set; }
        // public Guid? StockTransferRequestId { get; set; }
        public Guid? ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public decimal? RequestQuantity { get; set; }
        public decimal? Price { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? TransferredQuantity { get; set; }
        public decimal? TransferQuantity { get; set; }
        public decimal? RemainingQuantity { get; set; }
    }
}
