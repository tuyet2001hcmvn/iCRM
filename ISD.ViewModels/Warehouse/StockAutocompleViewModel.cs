using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class StockAutocompleViewModel
    {
        public Guid? StockId { get; set; }
        public string StockName { get; set; }
        public string StockCodeText { get; set; }
        public string StockCode { get; set; }
    }
}
