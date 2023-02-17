using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class TaskGTBQuantityReportViewModel
    {
        [Display(Name = "Khu vực")]
        public string KhuVuc { get; set; }
        [Display(Name = "SL tổng")]
        public int SLTong { get; set; }
        [Display(Name = "SL theo thời gian")]
        public int SLTheoThoiDiem { get; set; }
    }

    public class TaskGTBQuantityReportSearchModel
    {
        public string StartCommonDate { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FromDate")]
        public DateTime? StartFromDate { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ToDate")]
        public DateTime? StartToDate { get; set; }
        public bool IsView { get; set; }
    }
}
