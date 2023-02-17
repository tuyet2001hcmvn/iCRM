using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class KanbanMenuViewModel
    {
        public Guid MenuId { get; set; }
        public string MenuName { get; set; }
        public string MenuCode { get; set; }
        public string IconType { get; set; }
        public string IconName { get; set; }
        public int? OrderIndex { get; set; }
        public bool? isHasCreateTaskPermission { get; set; }
        public bool? isCreated { get; set; }
        public bool? isReporter { get; set; }
        public bool? isAssignee { get; set; }
        public bool? Visible { get; set; }
    }
}
