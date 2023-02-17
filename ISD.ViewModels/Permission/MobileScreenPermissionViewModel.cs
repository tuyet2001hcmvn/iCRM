using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class MobileScreenPermissionViewModel
    {
        public System.Guid RolesId { get; set; }
        public System.Guid MobileScreenId { get; set; }
        public string FunctionId { get; set; }
    }
}
