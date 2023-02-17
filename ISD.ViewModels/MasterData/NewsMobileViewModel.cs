using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class NewsMobileViewModel
    {
        public Guid NotificationId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedDateWithFormat { get { return string.Format("{0:dd/MM/yyyy HH:mm:ss}", CreatedDate); } }
        public string CreatedUser { get; set; }
        public string ImageUrl { get; set; }
        public string Summary { get; set; }
        public string SummaryName { get; set; }
        public string Detail { get; set; }
        public string DetailName { get; set; }
        public string TypeNews { get; set; }
        public string CreateByName { get; set; }
        public DateTime? ScheduleTimeTemp { get; set; }
        public string ScheduleTime
        {
            get
            {
                if (ScheduleTimeTemp.HasValue)
                {
                    return string.Format("{0:dd/MM/yyyy HH:mm}", ScheduleTimeTemp.Value);
                }
                return string.Empty;
            }
        }
    }
}
