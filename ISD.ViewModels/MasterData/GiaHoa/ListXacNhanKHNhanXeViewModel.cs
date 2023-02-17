using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ISD.ViewModels.Sale
{
    public class ListXacNhanKHNhanXeViewModel
    {
        public Guid ServiceOrderId { get; set; }

        public Guid SaleOrderMasterId { get; set; }

        public string SaleOrderMasterCode { get; set; }

        public string CustomerCode { get; set; }

        public string FullName { get; set; }

        public string LoaiXe { get; set; }

        public string SerialNumber { get; set; }

        public string EngineNumber { get; set; }
    }
}
