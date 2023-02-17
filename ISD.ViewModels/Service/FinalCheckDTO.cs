using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.Service
{
    public class FinalCheckDTO
    {
        public Guid ServiceOrderId { get; set; }
        public string selectedLoaiDichVu { get; set; }
        public string Step2HangMuc { get; set; }
        public DateTime? Step2NextDateTime { get; set; }
        public bool? Step2GetOldAccessory { get; set; }
        
        public string Step2Km { get; set; }
        public Guid? AccountId { get; set; }
        public Guid? Step2HangMucId { get; set; }
    }
}
