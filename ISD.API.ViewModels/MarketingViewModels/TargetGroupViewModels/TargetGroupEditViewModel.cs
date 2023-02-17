using System;
using System.ComponentModel.DataAnnotations;

namespace ISD.API.ViewModels
{
    public class TargetGroupEditViewModel
    {
        [Required]
        [MaxLength(50)]
        public string TargetGroupName { get; set; }
        public bool Actived { get; set; }
        [Required]
        public string Type { get; set; }
        public Guid LastEditBy { get; set; }
    }
}
