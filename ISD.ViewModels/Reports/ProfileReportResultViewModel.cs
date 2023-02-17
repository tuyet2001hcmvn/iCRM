using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class ProfileReportResultViewModel
    {
        //Tên KH
        public string ProfileName { get; set; }
        //Địa chỉ
        public string Address { get; set; }
        //MST
        public string TaxNo { get; set; }
        //Email
        public string Email { get; set; }
        //Phân loại KH
        public string CustomerTypeName { get; set; }
        //Tên người liên hệ
        public string ContactName { get; set; }
        //SDT
        public string Phone { get; set; }
        //Nhóm KH
        public string CustomerGroupName { get; set; }
        //Ngành nghề
        public string CustomerCareerName { get; set; }
        //Ghi chú
        public string Note { get; set; }
        //Nhân viên tạo
        public string SalesEmployeeName { get; set; }
        //Nguồn KH
        public string CustomerSourceName { get; set; }
        //Ngày ghé thăm
        public string VisitDate { get; set; }
        //Thị hiếu KH
        public string CustomerTaste { get; set; }
        //Catalog
        public string Catalog { get; set; }
    }
}
