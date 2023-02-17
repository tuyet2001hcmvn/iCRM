using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class NotificationMobileViewModel
    {
        public Guid NotificationId { get; set; }
        public bool? IsRead { get; set; }
        public Guid? TaskId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Detail { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedDateWithFormat
        {
            get
            {
                if (CreatedDate.HasValue)
                {
                    return string.Format("{0:dd/MM/yyyy HH:mm}", CreatedDate.Value);
                }
                return string.Empty;
            }
        }
    }
}
