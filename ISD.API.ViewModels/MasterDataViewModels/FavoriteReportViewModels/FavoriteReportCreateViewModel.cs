using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.API.ViewModels.MasterDataViewModels.FavoriteReportViewModels
{
    public class FavoriteReportCreateViewModel
    {
        public Guid AccountId { get; set; }
        public Guid PageId { get; set; }
    }
}
