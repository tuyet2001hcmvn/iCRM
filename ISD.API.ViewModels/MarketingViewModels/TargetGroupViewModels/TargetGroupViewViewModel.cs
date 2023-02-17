using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.API.ViewModels.MarketingViewModels.TargetGroupViewModels
{
    public class TargetGroupViewViewModel
    {
        public Guid Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string TargetGroupName { get; set; }
        [Required]
        public int TargetGroupCode { get; set; }
        [Required]
        public string Type { get; set; }
        public bool Actived { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateTime { get; set; }
        public string LastEditBy { get; set; }
        public DateTime? LastEditTime { get; set; }
        public int InternalCustomerQuantity { get; set; }
        public int ExternalCustomerQuantity { get; set; }
    }
}
