using ISD.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ISD.ViewModels.Service
{
    public class ServiceDetailDTO
    {
        public Guid ServiceOrderId { get; set; }
        public string AccessoryCode { get; set; }
        public string AccessoryCodeReference { get; set; }
        public Guid? AccessoryIdReference { get; set; }
        public string AccessoryName { get; set; }
        public Decimal? AccessoryPrice { get; set; }

        public string WarehouseCode { get; set; }
        public int Quantity { get; set; }
        //public string ServiceTypeCode { get; set; }
        public Guid? FixingTypeId { get; set; }
        public string Unit { get; set; }
        public string Location { get; set; }

        public Guid? ServiceOrderDetailAccessoryId { get; set; }
        public Guid? ServiceOrderDetailServiceId { get; set; }
        
        public string Note { get; set; }
        public string WarehouseName { get; set; }
        public string FixingType { get; set; }
    }
}
