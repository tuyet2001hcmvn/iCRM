using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ISD.ViewModels.Marketing
{
    public class TargetGroupCreateViewModel
    {
        [Required]
        [MaxLength(50)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TargetGroupName")]
        public string TargetGroupName { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TargetGroupName")]
        public List<string> ProfileIdList { get; set; }
    }
}
