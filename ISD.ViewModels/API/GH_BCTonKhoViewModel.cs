using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.API
{
    public class GH_BCTonKhoViewModel
    {
        public string StoreName { get; set; }
        public List<GH_BCTonKhoDetailViewModel> detail { get; set; }
        public int TotalQuantity { get; set; }
        public string Message { get; set; }
    }

    public class GH_BCTonKhoDetailViewModel
    {
        public string SaleOrg { get; set; }
        public string StoreName { get; set; }
        public string ProductHierarchy { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialFreightGroup { get; set; }
        public int Quantity { get; set; }
    }

    public class GH_BCTonKhoSearchViewModel
    {
        public Guid AccountId { get; set; }
        public string SaleOrg { get; set; }
        public string ProfitCenterCode { get; set; }
        public string ProductHierarchyCode { get; set; }
        public string MaterialFreightGroupCode { get; set; }
    }
}