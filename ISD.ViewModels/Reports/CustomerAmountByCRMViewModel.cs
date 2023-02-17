using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class CustomerAmountByCRMViewModel
    {
        [Display(Name = "Phân loại KH")]
        public string CustomerTypeName { get; set; }
        [Display(Name = "Nhóm khách hàng")]
        public string CustomerGroupCode { get; set; }
        [Display(Name = "Nhóm khách hàng")]
        public string CustomerGroupName { get; set; }
        //Doanh nghiệp
        public decimal? QtyECCB { get; set; }
        public decimal? QtyCRMB { get; set; }
        public decimal? QtyTotalB { get; set; }
        //Tiêu dùng
        public decimal? QtyECCC { get; set; }
        public decimal? QtyCRMC { get; set; }
        public decimal? QtyTotalC { get; set; }
        //Tổng
        public decimal? QtyECC{ get; set; }
        public decimal? QtyCRM { get; set; }
        public decimal? QtyTotal { get; set; }
    }

    public class CustomerAmountByCRMSearchViewModel
    {
        [Display(Name = "Phân loại KH")]
        public string CustomerTypeCode { get; set; }
        [Display(Name = "Nhóm khách hàng")]
        public string CustomerGroupCode { get; set; }
        //Ngày tạo
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CommonCreateDate")]
        public string CommonDate { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FromDate", Description = "CreateTime")]
        public DateTime? FromDate { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ToDate", Description = "CreateTime")]
        public DateTime? ToDate { get; set; }
        public bool IsView { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Company")]
        public string CompanyCode { get; set; }
    }
}
