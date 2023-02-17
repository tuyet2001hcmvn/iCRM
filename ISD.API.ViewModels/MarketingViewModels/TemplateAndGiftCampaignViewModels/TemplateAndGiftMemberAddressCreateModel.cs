using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.API.ViewModels
{
    public class TemplateAndGiftMemberAddressCreateModel
    {
        public Guid TempalteAndGiftMemberId { get; set; }
        public Guid CreateBy { get; set; }
        public List<TemplateAndGiftMemberAddressViewModel> ListDetail { get; set; }
    }
}
