using ISD.EntityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.Service
{
    public class ServiceOrderDetailsViewModel
    {
        public StoreViewModel Store { get; set; }
        public ServiceOrderViewModel ServiceOrder { get; set; }
        public VehicleInfoViewModel VehicleInfo { get; set; }

        public decimal? DepositPrice { get; set; }

        public ServiceCustomerViewModel Customer { get; set; }
        //public ISD.ViewModels.Sale.SaleOrderMasterViewModel Customer { get; set; }

        public List<ServiceHistoryViewModel> History { get; set; }
        public List<ServiceConsultViewModel> Consults { get; set; }
        public List<ServiceOrderDetailAccessoryViewModel> Accessories { get; set; }
        public List<ServiceOrderDetailAccessoryViewModel> AccessoriesUrgent { get; set; }
        public List<ServiceOrderDetailServiceViewModel> Services { get; set; }
        public List<ServiceTypeViewModel> ServiceTypes { get; set; }
        public List<FixingTypeViewModel> FixingTypes { get; set; }
        public List<FixingTypeViewModel> ServiceFixingTypes { get; set; }
    }
}
