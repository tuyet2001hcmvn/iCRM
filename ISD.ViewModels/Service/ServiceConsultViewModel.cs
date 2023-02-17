using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.Service
{
    public class ServiceConsultViewModel
    {
        public string FullName { get; set; }
        public string RequestMessage { get; set; }
        public bool? IsThayThe { get; set; }
        public DateTime? CreateDateS { get; set; }
        public string CreateDate
        {
            get
            {
                if (CreateDateS == null) return "";
                return CreateDateS.Value.ToString("HH:mm - dd/MM/yyyy");
            }
        }
    }
}
