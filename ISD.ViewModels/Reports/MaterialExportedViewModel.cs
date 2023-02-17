using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class MaterialExportedViewModel
    {
        [Display(Name = "STT")]
        public int STT { get; set; }

        [Display(Name = "Khu vực")]
        public string SaleOfficeName { get; set; }

        [Display(Name = "Phòng ban")]
        public string RolesName { get; set; }

        [Display(Name = "Số GVL")]
        public decimal? QtyGVL { get; set; }

        [Display(Name = "Kệ CTL")]
        public decimal? QtyKeCTL { get; set; }

        [Display(Name = "Kệ Mẫu")]
        public decimal? QtyKeMau { get; set; }

        [Display(Name = "Bas Inox")]
        public decimal? QtyBasInox { get; set; }

        [Display(Name = "Bảng Hiệu")]
        public decimal? QtyBangHieu { get; set; }

        [Display(Name = "Khay A5")]
        public decimal? QtyKhayA5 { get; set; }

        [Display(Name = "Mẫu A5")]
        public decimal? QtyMauA5 { get; set; }

        [Display(Name = "Mẫu 250x370")]
        public decimal? QtyMau250x370 { get; set; }

    }
    public class MaterialExportedSearchModel
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CommonReceiveDate")]
        public string CommonDate { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FromDate", Description = "Appointment_VisitDate")]
        public DateTime? FromDate { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ToDate", Description = "Appointment_VisitDate")]
        public DateTime? ToDate { get; set; }
        public bool IsView { get; set; }
    }
}
