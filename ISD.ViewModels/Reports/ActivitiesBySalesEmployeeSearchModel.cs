using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class ActivitiesBySalesEmployeeSearchModel
    {
        public List<string> SalesEmployeeCode { get; set; }
        public List<Guid> WorkFlowIdList { get; set; }
        public DateTime? StartFromDate { get; set; }
        public DateTime? StartToDate { get; set; }
        public DateTime? CreateFromDate { get; set; }
        public DateTime? CreateToDate { get; set; }


    }
}
