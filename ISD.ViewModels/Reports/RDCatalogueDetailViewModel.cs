using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class RDCatalogueDetailViewModel
    {
        [Display(Name = "STT")]
        public int? STT { get; set; }
        [Display(Name = "Loại chứng từ")]
        public string DocumentType { get; set; }

        [Display(Name = "Số chứng từ")]
        public int? DocumentCode { get; set; }
        [Display(Name = "Ngày chứng từ")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? DocumentDate { get; set; }
     
        [Display(Name = "Diễn giải")]
        public string Note { get; set; }

        [Display(Name = "Tồn đầu kì")]
        [DisplayFormat(DataFormatString = "{0:n0}")]
        public decimal? TonDauKi { get; set; }

        [Display(Name = "Nhập")]
        [DisplayFormat(DataFormatString = "{0:n0}")]
        public decimal? ReceiveQty { get; set; }

        [Display(Name = "Xuất")]
        [DisplayFormat(DataFormatString = "{0:n0}")]
        public decimal? DeliveryQty { get; set; }

        [Display(Name = "Tồn cuối kì")]
        [DisplayFormat(DataFormatString = "{0:n0}")]
        public decimal? TonCuoiKi { get; set; }
    }
    public class RDCatalogueDetailSearchViewModel
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_Warehouse")]
        public Guid? StockId { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product")]
        public Guid? ProductId { get; set; }
        //13. Ngày chứng từ
        public string CommonDate { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FromDate")]
        public DateTime? FromDate { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ToDate")]
        public DateTime? ToDate { get; set; }
        public long TonDauKi { get; set; }
        public long TonCuoiKi { get; set; }
        public bool IsView { get; set; }
    }
}
