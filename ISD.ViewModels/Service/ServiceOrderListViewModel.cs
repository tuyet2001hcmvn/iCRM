
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.Service
{
    public class ServiceOrderListViewModel
    {
        public Guid? ServiceOrderId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string SerialNumber { get; set; }
        public string LicensePlate { get; set; }
        public string KmDaDi { get; set; }
        public int? Step { get; set; }

        public Guid? TienNhanId { get; set; }
        public string TienNhanFullName { get; set; }
        public DateTime? TiepNhanTime { get; set; }

        public Guid? KTV1Id { get; set; }
        public string KTV1FullName { get; set; }
        public Guid? KTV2Id { get; set; }
        public string KTV2FullName { get; set; }

        public Guid? KiemTraCuoiId { get; set; }
        public string KiemTraCuoiFullName { get; set; }
        public DateTime? KiemTraCuoiTime { get; set; }
        public int? Number { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedDateText
        {
            get
            {
                string text = string.Empty;
                if (CreatedDate.HasValue)
                {
                    text = string.Format("{0:dd/MM/yyyy HH:mm:ss}", CreatedDate);
                }
                return text;
            }
        }
        public string ServiceOrderCode { get; set; }

        public string SaleOrderMasterCode { get; set; }
        public string Batch { get; set; }
        public string ProductHierarchyName { get; set; }
        public string ExternalMaterialGroup { get; set; }
        public string MaterialFreightGroup { get; set; }
        public string MaterialDescription
        {
            get
            {
                string result = string.Empty;
                result = string.Format("{0} - {1} - {2}", ProductHierarchyName, ExternalMaterialGroup, MaterialFreightGroup);

                return result;
            }
        }
    }
}
