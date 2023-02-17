using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class ImageListViewModel
    {
        public Guid? ColorId { get; set; }
        public Guid? StyleId { get; set; }
        public string ImageUrl { get; set; }
    }
}
