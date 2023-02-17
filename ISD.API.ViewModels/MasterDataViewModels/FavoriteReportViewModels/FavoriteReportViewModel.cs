using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.API.ViewModels.MasterDataViewModels.FavoriteReportViewModels
{
    public class FavoriteReportViewModel
    {
        public string ReportName { get; set; }
        public Guid PageId { get; set; }
        public string PageUrl { get; set; }
        public bool IsFavorite { get; set; }
    }
}
