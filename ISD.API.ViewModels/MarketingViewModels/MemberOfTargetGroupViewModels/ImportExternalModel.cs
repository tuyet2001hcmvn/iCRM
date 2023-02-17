using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.API.ViewModels
{
    public class ImportExternalModel
    {
        public Guid? TargetGroupId { get; set; }
        public Guid? Name { get; set; }
        public Guid? Email { get; set; }
    }
}
