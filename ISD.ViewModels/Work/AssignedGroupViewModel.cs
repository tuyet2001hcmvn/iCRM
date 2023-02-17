using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class AssignedGroupViewModel
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "AssignedGroupName")]
        [Required]
        public string GroupName { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "AssignedGroupMember")]
        [Required]
        public List<SalesEmployeeViewModel> AccountIdList { get; set; }

        public string Type { get; set; }
    }
}
