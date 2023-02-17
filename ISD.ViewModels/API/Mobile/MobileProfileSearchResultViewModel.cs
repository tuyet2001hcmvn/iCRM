using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class MobileProfileSearchResultViewModel
    {
        public Guid ProfileId { get; set; }
        public string ProfileName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string CustomerTypeCode { get; set; }
        public string CustomerTypeName { get; set; }
        public string CustomerSourceCode { get; set; }
        public string CustomerGroupCode { get; set; }
        public string CustomerCareerCode { get; set; }
        public string SaleOfficeCode { get; set; }
        public Guid? ProvinceId { get; set; }
        public Guid? DistrictId { get; set; }
        public Guid? WardId { get; set; }
        public string Title { get; set; }
        public string ProfileShortName { get; set; }
        public string TaxNo { get; set; }
        public string Website { get; set; }
        public string ProvinceName { get; set; }
        public string DistrictName { get; set; }
        public string WardName { get; set; }
        public string AddressTypeCode { get; set; }
        public string ProfileForeignCode { get; set; }
        public string SalesEmployeeName { get; set; }
        public string SalesSupervisorCode { get; set; }
        public string SalesSupervisorName { get; set; }
        public string DepartmentName { get; set; }
        public List<string> AddressList { get; set; }

        //Contact
        public Guid? ProfileContactId { get; set; }
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPosition { get; set; }
        public string ContactDepartment { get; set; }
    }
}
