using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.API.ViewModels.UtilitiesViewModel.SearchTemplateViewModel
{
    public class SearchTemplateViewModel
    {
        public Guid SearchTemplateId { get; set; }
        public Guid AccountId { get; set; }
        public Guid PageId { get; set; }
        public string TemplateName { get; set; }
        public string SearchContent { get; set; }
    }
}
