using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.Service
{
    public class LoadVehicleDetailsViewModel
    {
        //public FindVehicleViewModel SelectedCustomer { get; set; }
        public ServiceCustomerViewModel SelectedCustomer { get; set; }
        
        public VehicleInfoViewModel SelectedVehicle { get; set; }
        public List<ServiceHistoryViewModel> HistoryList { get; set; }
    }
}
