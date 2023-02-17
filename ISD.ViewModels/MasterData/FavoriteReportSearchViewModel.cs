using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.MasterData
{
    public class FavoriteReportSearchViewModel
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ReportName")]
        public string ReportName { get; set; }
        public Guid PageId { get; set; }
        public string PageUrl { get; set; }
    }
}
