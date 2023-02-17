using System;
using System.ComponentModel.DataAnnotations;

namespace ISD.API.ViewModels
{
    public class TargetGroupCreateViewModel
    {
        [Required]
        [MaxLength(50)]
        public string TargetGroupName { get; set; }
        [Required]
        public string Type { get; set; }
        public Guid CreateBy { get; set; }
    }
}
