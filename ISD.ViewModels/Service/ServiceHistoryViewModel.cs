using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.Service
{
    public class ServiceHistoryViewModel
    {
        public Guid ServiceOrderId { get; set; }
        public DateTime? CreateDateData { get; set; }
        public string CreateDate
        {
            get
            {
                if (CreateDateData == null)
                    return "";

                return CreateDateData.Value.ToString("dd/MM/yyyy");
            }

        }
        public string ServiceTypeName { get; set; }
        public string SaleOrgName { get; set; }
        public string CompanyName { get; set; }
        public string ServiceOrderName { get; set; }
    }
}
