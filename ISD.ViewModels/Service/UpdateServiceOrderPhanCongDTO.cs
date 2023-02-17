using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.Service
{
    public class UpdateServiceOrderPhanCongDTO
    {
        public Guid ServiceOrderId { get; set; }
        public string CustomerRequest { get; set; }
        public string KmDaDi { get; set; }
        public int WashRequest { get; set; }
        public Guid selectedKTV1 { get; set; }
        public Guid selectedKTV2 { get; set; }
        public string selectedLoaiDichVu { get; set; }

        public string Step1Note { get; set; }
        public string ConsultNote { get; set; }
        
        public Guid AccountId { get; set; }
        public string Step1DateTimeDuKien { get; set; }
        public string Step1TimeDuKien { get; set; }
        //Số phút hoàn thành sửa chữa dự kiến
        public double Step1DuKien { get; set; }
    }
}
