using System;
using System.ComponentModel.DataAnnotations;

namespace ISD.ViewModels.Service
{
    public class ServiceOrderDetailServiceViewModel
    {
        public System.Guid ServiceOrderDetailServiceId { get; set; }
        public System.Guid ServiceOrderId { get; set; }

        [Display(Name = "Loại dịch vụ")]
        //public string ServiceTypeCode { get; set; }
        public Guid? FixingTypeId { get; set; }

        public string MaterialNumber { get; set; }
        public string ShortText { get; set; }
        public string UOM { get; set; }
        public Nullable<decimal> HourPrice { get; set; }
        public Nullable<int> Discount { get; set; }
        public Nullable<decimal> Total { get; set; }
        public string ServiceTypeCode { get; set; }

        public string AccessoryCode { get; set; }
        public string AccessoryCodeReference { get; set; }
        public string AccessoryName { get; set; }
        public Nullable<decimal> AccessoryPrice { get; set; }
        public string Note { get; set; }
        public string FixingTypeName { get; set; }
        public Guid? AccessoryIdReference { get; set; }
        public int? Number { get; set; }
    }
}