using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.MasterData
{
    public class CareerSearchViewModel
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Career_CareerName")]
        public string CareerName { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public bool? Actived { get; set; }
    }
}
