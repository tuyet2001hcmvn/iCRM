using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.Service
{
    public class CreateServiceOrderDTO
    {
        public Guid CustomerId { get; set; }
        public Guid VehicleId { get; set; }
        public bool IsNewCustomer { get; set; }
        public bool IsNewVehicle { get; set; }
        public string KmDaDi { get; set; }
        public string CustomerRequest { get; set; }
        public int WashRequest { get; set; }
        public Guid AccountId { get; set; }
        public string SaleOrg { get; set; }

        // For create new
        public string selectedNhanHieu { get; set; } // Nhãn hiệu: ProfitCenterModel
        public string selectedLoaiXe { get; set; } // Đời xe: ProductHierarchyModel

        public string LicensePlate { get; set; }
        public DateTime? BuyDate { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }

        public Guid? ProvinceId { get; set; }
        public string ProvinceName { get; set; }
        //Quận/huyện
        public Guid? DistrictId { get; set; }
        public string DistrictName { get; set; }
        //Phường/xã
        public Guid? WardId { get; set; }
        public string WardName { get; set; }

        public string CustomerAddress { get; set; }

    }
}
