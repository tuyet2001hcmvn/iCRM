using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.API.ViewModels
{
    public class StockOnHandViewModel
    {
        public Guid ProductId { get; set; }
        public string ERPProductCode { get; set; }
        public decimal? Qty { get; set; }
    }
}
