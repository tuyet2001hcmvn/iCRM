using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class TotalRDCatalogueReportViewModel
    {
        [Display(Name = "STT")]
        public int NumberIndex { get; set; }

        [Display(Name = "Mã chi nhánh")]
        public string SaleOrgCode { get; set; }

        [Display(Name = "Tên chi nhánh")]
        public string StoreName { get; set; }

        [Display(Name = "Mã Catalogue")]
        public string ERPProductCode { get; set; }

        [Display(Name = "Tên Catalogue")]
        public string ProductName { get; set; }

        [Display(Name = "Đơn vị tính")]
        public string Unit { get; set; }

        [Display(Name = "Tồn đầu kỳ")]
        public int TonDauKi { get; set; }

        [Display(Name = "Nhập trong kỳ")]
        public int ReceiveQty { get; set; }

        [Display(Name = "Xuất trong kỳ")]
        public int DeliveryQty { get; set; }

        [Display(Name = "Tồn cuối kỳ")]
        public int TonCuoiKi { get; set; }

    }
    public class TotalRDCatalogueReportSearchViewModel
    {
        [Display(Name = "Chi nhánh")]
        public List<Guid> StoreId { get; set; }
        [Display(Name = "Catalogue")]
        public List<Guid> ProductId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CommonDate")]
        public string CommonDate { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FromDate", Description = "Appointment_VisitDate")]
        public DateTime? FromDate { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ToDate", Description = "Appointment_VisitDate")]
        public DateTime? ToDate { get; set; }
        public bool IsView { get; set; }
    }
}
