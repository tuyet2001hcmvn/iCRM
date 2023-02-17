using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.Service
{
    public class ListPushNotificationViewModel
    {
        public Guid? ServiceOrderId { get; set; }
        public string SaleOrg { get; set; }
        public string LicensePlate { get; set; }
        public string Message
        {
            get
            {
                return string.Format("Xe có biển số {0} vừa được tiếp nhận và cần phân công sửa chữa", LicensePlate);
            }
        }
    }
}
