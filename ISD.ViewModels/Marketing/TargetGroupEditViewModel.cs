using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.Marketing
{
    public class TargetGroupEditViewModel
    {
        public Guid Id { get; set; }
        [Required]
        [MaxLength(50)]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TargetGroupName")]
        public string TargetGroupName { get; set; }
        [Required]
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TargetGroupCode")]
        public int TargetGroupCode { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public bool Actived { get; set; }
        [Required]
        public string Type { get; set; }
    }
}
