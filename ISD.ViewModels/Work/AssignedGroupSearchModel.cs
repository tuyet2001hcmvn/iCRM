using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class AssignedGroupSearchModel
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "AssignedGroupName")]
        public string GroupName { get; set; }
    }
}
