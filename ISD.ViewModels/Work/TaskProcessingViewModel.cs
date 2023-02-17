using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class TaskProcessingViewModel
    {
        public Guid? TaskStatusId { get; set; }
        public string TaskStatusName { get; set; }
        public int? OrderIndex { get; set; }
        public Guid? LastEditBy { get; set; }
        public string LastEditByName { get; set; }
        public DateTime? LastEditTime { get; set; }
        public string ProcessCode { get; set; }
    }
}
