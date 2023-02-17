using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.API.ViewModels
{
    public class TemplateAndGiftMemberAddressViewModel
    {
        public Guid Id { get; set; }
        public Guid? TempalteAndGiftMemberId { get; set; }
        public string Address { get; set; }
        public string ProductName { get; set; }
        public Guid? ProductId { get; set; }
        public decimal? Quantity { get; set; }
        public Guid CreateBy { get; set; }
    }
}
