using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class MobileAddressBookViewModel
    {
        public Guid ProfileId { get; set; }
        public Guid AccountId { get; set; }
        //Loại địa chỉ
        public string AddressTypeCode { get; set; }
        //Đối tượng
        public string CountryCode { get; set; }
        //Tỉnh/Thành
        public Guid? ProvinceId { get; set; }
        //Quận/Huyện
        public Guid? DistrictId { get; set; }
        //Phường/Xã
        public Guid? WardId { get; set; }
        //Địa chỉ
        public string Address { get; set; }
        //Ghi chú
        public string Note { get; set; }
        //Địa chỉ chính
        public bool? isMain { get; set; }

    }
}
