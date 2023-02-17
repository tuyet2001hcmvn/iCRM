using System.ComponentModel.DataAnnotations;

namespace ISD.ViewModels
{
    public class TaskProductWarrantyReportExcelModel
    {
        [Display(Name = "Loại")]
        public string WorkFlowName { get; set; }

        [Display(Name = "Mã SAP sản phẩm")]
        public string ERPProductCode { get; set; }

        [Display(Name = "Tên sản phẩm")]
        public string ProductName { get; set; }

        [Display(Name = "Phân loại sản phẩm")]
        public string ProductCategoryName { get; set; }

        [Display(Name = "Phương thức xử lý")]
        public string ErrorName { get; set; }

        [Display(Name = "Số lượng")]
        public int? ProductQuantity { get; set; }
    }
}