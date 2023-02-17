using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class ProductAutocompleViewModel
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public Guid? ProductId { get; set; }
        public string ProductSearchName { get; set; }
        public decimal? Price { get; set; }
    }

    public class ProductAutocompleteViewModel
    {
        public Guid? value { get; set; }
        public string text { get; set; }
    }
}
