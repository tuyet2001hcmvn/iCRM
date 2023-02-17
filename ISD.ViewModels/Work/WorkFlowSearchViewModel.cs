using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class WorkFlowSearchViewModel
    {
        public string WorkFlowName { get; set; }
        //public string TaskStatusName { get; set; }
        public Nullable<bool> Actived { get; set; }
    }
}
