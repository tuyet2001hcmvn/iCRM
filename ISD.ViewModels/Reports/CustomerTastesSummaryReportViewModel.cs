using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class CustomerTastesSummaryReportViewModel
    {
        [Display(Name = "Nhóm sản phẩm")]
        public string NhomVT { get; set; }

        [Display(Name = "Mã thương mại")]
        public string MaSP { get; set; }

        [Display(Name = "Tên sản phẩm")]
        public string TenSP { get; set; }
        
        [Display(Name = "Số lượt liked")]
        public int? SoLuotLiked { get; set; }
    }

    public class CustomerTastesSummaryReportSearchViewModel
    {
        public CustomerTastesSummaryReportSearchViewModel()
        {
            IsCallFirst = true;
        }
        [Display(Name = "Chi nhánh")]
        public string SaleOrgCode { get; set; }
        [Display(Name = "Thời gian")]
        public string CommonDate { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FromDate", Description = "Appointment_CreateFromDate")]
        public DateTime? FromDate { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ToDate", Description = "Appointment_CreateToDate")]
        public DateTime? ToDate { get; set; }

        [Display(Name = "Nhân viên")]
        public string SaleEmployeeCode { get; set; }
        
        public List<Guid> StoreId { get; set; }
        public bool IsCallFirst { get; set; }
        [Display(Name = "Top sản phẩm yêu thích")]
        public int? TOP { get; set; }
        public bool IsView { get; set; }
    }
}
