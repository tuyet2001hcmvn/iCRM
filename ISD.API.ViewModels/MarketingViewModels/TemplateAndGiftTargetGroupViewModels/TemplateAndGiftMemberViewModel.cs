using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.API.ViewModels
{
    public class TemplateAndGiftMemberViewModel
    {
        public Guid Id { get; set; }
        public Guid ProfileId { get; set; }
        public string ProfileCode { get; set; }
        public string ProfileForeignCode { get; set; }
        public string ProfileName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public List<TemplateAndGiftMemberAddressViewModel> ListDetail { get; set; }
    }
}
