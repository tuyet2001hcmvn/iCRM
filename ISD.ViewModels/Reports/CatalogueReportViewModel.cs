using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class CatalogueReportViewModel
    {
        public int? STT { get; set; }
        
        public string MaCatalogue { get; set; }
        
        public string TenCTL { get; set; }

        public string DVT { get; set; }
        
        public int? SoLuong { get; set; }

        public Guid? StoreId { get; set; }
    }

    public class CatalogueReportSearchViewModel
    {
        public Guid? CompanyId { get; set; }
        public string CompanyCode { get; set; }
        public List<Guid> StoreId { get; set; }
        public string SaleOrgCode { get; set; }
        public string CustomerSourceCode { get; set; }
        public string SaleEmployeeCode { get; set; }
        public string CustomerTypeCode { get; set; }
        public string CommonDate { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string CustomerCareerCode { get; set; }
        
        public string CommonReceiveDate { get; set; }
        public DateTime? ReceiveFromDate { get; set; }
        public DateTime? ReceiveToDate { get; set; }
        //Ngày kết thúc
        public string CommonEndDate { get; set; }
        public DateTime? EndFromDate { get; set; }
        public DateTime? EndToDate { get; set; }
        //Ngày tạo
        public string CommonCreateDate { get; set; }
        public DateTime? CreateFromDate { get; set; }
        public DateTime? CreateToDate { get; set; }

        //Mobile additional field
        public string CustomerGroupCode { get; set; }
        public bool? isViewByStore { get; set; }

        public string CustomerCategoryCode { get; set; }
        public Guid? CategoryId { get; set; }
    }
}
