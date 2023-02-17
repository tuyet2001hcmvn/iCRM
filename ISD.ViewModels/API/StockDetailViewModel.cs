using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.API
{
    public class StockDetailViewModel
    {
        public Int64 RowNumber { get; set; }
        public string WarehouseName { get; set; }
        public string ColorName { get; set; }
        public string StyleName { get; set; }
        public int Quantity { get; set; }
    }
}
