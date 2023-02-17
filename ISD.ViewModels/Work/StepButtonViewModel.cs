using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class StepButtonViewModel
    {
        public string TransitionName { get; set; }
        public Guid? TransitionStatusId { get; set; }
        public string Color { get; set; }
        public bool? isRequiredComment { get; set; }
    }
}
