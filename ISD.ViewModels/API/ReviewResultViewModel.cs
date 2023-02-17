using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class ReviewResultViewModel
    {
        //Nội dung đánh giá
        public string TIEUCHI { get; set; }
        //Điểm đánh giá
        public string DIEM { get; set; }
        //Thứ tự hiển thị
        public int? STT { get; set; }
    }
}
