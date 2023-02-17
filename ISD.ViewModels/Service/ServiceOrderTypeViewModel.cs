using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.Service
{
    public class ServiceOrderTypeViewModel 
    {
        public string ServiceOrderTypeCode { get; set; }
        public string ServiceOrderTypeName { get; set; }
        public Nullable<int> OrderIndex { get; set; }
    }

}
