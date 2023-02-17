using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.Service
{
    public class NewVehicleInfoDTO
    {
        public string LicensePlate { get; set; }
        public string doiXeSelected { get; set; } // Đời xe: ExternalMaterialGroupCode

        public string selectedNhanHieu { get; set; } // Nhãn hiệu: ProfitCenterModel
        public string selectedLoaiXe { get; set; } // Đời xe: ProductHierarchyModel
        
        public string SerialNumber { get; set; }
        public string EngineNumber { get; set; }
        public DateTime? BuyDate { get; set; }

        public bool IsNewCustomer { get; set; }
        public Guid? CustomerId { get; set; }
         
        public string FullName { get; set; }
        public string Phone { get; set; }
        public Guid? AccountId { get; set; }

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
