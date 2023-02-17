using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class FunctionViewModel
    {
        public Guid PageId { get; set; }
        public Guid MobileScreenId { get; set; }
        public string FunctionId { get; set; }
        public string FunctionName { get; set; }
        public int? OrderIndex { get; set; }
        public bool Selected { get; set; }
    }
}
