using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.Service
{
    public class ClaimAccessoryLogViewModel
    {
        public Guid ClaimAccessoryLogId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public int? StatusId { get; set; }

        public string StatusName { get; set; }

        public DateTime? CreatedTime { get; set; }

        public string CreatedUser { get; set; }
    }
}
