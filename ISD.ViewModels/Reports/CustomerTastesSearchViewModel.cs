using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class CustomerTastesSearchViewModel
    {
        public string CompanyCode { get; set; }
        [Display(Name = "Chi nhánh")]
        public string SaleOrgCode { get; set; }
        public string FromDate_String { get; set; }
        [Display(Name = "Từ ngày")]
        public DateTime? FromDate { get; set; }
        public string ToDate_String { get; set; }
        [Display(Name = "Đến ngày")]
        public DateTime? ToDate { get; set; }

        [Display(Name = "Nhân viên")]
        public string SaleEmployeeCode { get; set; }
        [Display(Name = "Nhóm khách hàng")]
        public string CustomerGroupCode { get; set; }
        [Display(Name = "Nguồn khách hàng")]
        public string CustomerSourceCode { get; set; }
        public bool isViewByStore { get; set; }
        [Display(Name = "Top sản phẩm yêu thích")]
        public int? TOP { get; set; }
        public List<Guid> StoreId { get; set; }
    }
}
