using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class ConstructionViewModel : BaseClassViewModel
    {
        public Guid? OpportunityConstructionId { get; set; }
        public Guid? ProfileId { get; set; }
        public string CatalogCode { get; set; }
        public string MaterialCode { get; set; }
        public decimal? ProjectValue { get; set; }
        public bool? IsWon { get; set; }
        public bool? IsChecked { get; set; }

        public List<Guid> MaterialId { get; set; }
        public List<Guid> CompetitorId { get; set; }
    }
}
