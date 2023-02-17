using System.ComponentModel.DataAnnotations;

namespace ISD.ViewModels
{
    public class TicketUsualErrorViewModel
    {
        [Display(Name = "STT")]
        public long? NumberIndex { get; set; }

        [Display(Name = "Phân cấp SP")]
        public string ProductLevelName { get; set; }

        [Display(Name = "Nhóm vật tư")]
        public string ProductCategoryName { get; set; }

        [Display(Name = "Mã màu")]
        public string ProductColorCode { get; set; }

        [Display(Name = "Các lỗi BH thường gặp")]
        public string UsualErrorName { get; set; }


        [Display(Name = "Khu vực")]
        public string SaleOfficeName { get; set; }

        [Display(Name = "Nhân viên được phân công")]
        public string SaleEmployeeName { get; set; }

        [Display(Name = "Số lượng")]
        public int? CountOfTaskProduct { get; set; }

        [Display(Name = "Giá trị bảo hành")]
        public decimal? WarrantyValue { get; set; }

        [Display(Name = "Giá trị đơn hàng")]
        public decimal? OrderValue { get; set; }

        [Display(Name = "Tỷ lệ bảo hành")]
        public string WarrantyRate { 
            get {
                decimal? ret = 0;
                if (OrderValue != null && OrderValue != 0 )
                {
                    ret =  (WarrantyValue * 100) / OrderValue;
                }
                return string.Format("{0:n1} {1}",ret, "%");
            }
        } 
    }
}
