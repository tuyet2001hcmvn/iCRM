using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class MaterialExportedDetailReportViewModel
    {
        [Display(Name = "STT")]
        public int NumberIndex { get; set; }
        [Display(Name = "Khu vực")]
        public string SaleOfficeName { get; set; }

        [Display(Name = "Phòng ban")]
        public string RolesName { get; set; }

        [Display(Name = "Loại ĐTB")]
        public string WorkFlowName { get; set; }

        [Display(Name = "Số lượng ĐTB")]
        public decimal? Quantity { get; set; }

        [Display(Name = "Kệ CTL")]
        public decimal? QtyKeCTL { get; set; }

        [Display(Name = "Kệ Mẫu")]
        public decimal? QtyKeMau { get; set; }

        [Display(Name = "Bas Inox")]
        public decimal? QtyBasInox { get; set; }

        [Display(Name = "Bảng hiệu")]
        public decimal? QtyBangHieu { get; set; }

        [Display(Name = "Khay A5")]
        public decimal? QtyKhayA5 { get; set; }

        [Display(Name = "Mẫu A5")]
        public decimal? QtyMauA5 { get; set; }

        [Display(Name = "Mẫu 250x370")]
        public decimal? QtyMau250x370 { get; set; }


        public Guid? ProfileId { get; set; }

    }
    public class MaterialExportedDetailReportSearchViewModel
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_SaleOfficeCode")]
        public List<string> SaleOfficeCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaterialExportedDetail_WorkFlowCode")]
        public List<Guid> WorkFlowId { get; set; }


        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaterialExportedDetail_RolesCode")]
        public List<string> RolesCode { get; set; }

        //Ngày ghé thăm
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CommonReceiveDate")]
        public string CommonDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FromDate", Description = "Appointment_VisitDate")]
        public DateTime? FromDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ToDate", Description = "Appointment_VisitDate")]
        public DateTime? ToDate { get; set; }

        public bool IsView { get; set; }

        public string ReportType { get; set; }
    }
}
