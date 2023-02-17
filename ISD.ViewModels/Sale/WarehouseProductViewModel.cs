using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ISD.ViewModels
{
    public class WarehouseProductViewModel
    {
        public string ProductCode { get; set; }

        public string WarehouseCode { get; set; }

        public string StyleCode { get; set; }

        public string MainColorCode { get; set; }

        public decimal? Quantity { get; set; }

        public DateTime? PostDate { get; set; }

        public TimeSpan? PostTime { get; set; }

        public string UserPost { get; set; }
    }
}
