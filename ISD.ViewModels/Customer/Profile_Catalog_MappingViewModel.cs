using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class Profile_Catalog_MappingViewModel
    {
        public Guid ProfileCatalogId { get; set; }
        public Guid? ProfileId { get; set; }
        public Guid? CompetitorId { get; set; }
        public string CompetitorName { get; set; }
        public string CatalogCode { get; set; }
        public string MaterialCode { get; set; }
        public string MappingType { get; set; }

        public bool? IsChecked { get; set; }
    }
}
