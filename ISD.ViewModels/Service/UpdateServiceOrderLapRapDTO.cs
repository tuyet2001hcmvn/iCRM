using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.Service
{
    public class UpdateServiceOrderLapRapDTO
    {
        public Guid ServiceOrderId { get; set; }
        public Guid selectedKTV1 { get; set; }
        public Guid AccountId { get; set; }
    }
}
