using ISD.Constant;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class CustomerTasteViewModel : BaseClassViewModel
    {
        public Guid CustomerTasteId { get; set; }
        //Mã chi nhánh
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Store_StoreCode")]
        public string SaleOrgCode { get; set; }

        //Tên chi nhánh
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Store_StoreName")]
        public string SaleOrgName { get; set; }

        //Mã SAP
        public Guid? ProductId { get; set; }

        //Mã SAP
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_ERPProductCode")]
        public string ERPProductCode { get; set; }

        //Mã sản phẩm
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_ProductCode")]
        public string ProductCode { get; set; }

        //Tên sản phẩm
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_ProductName")]
        public string ProductName { get; set; }

        //Tháng
        [Display(Name = "Tháng")]
        public string CreatedMonth { get; set; }

        //Số lượng like
        [Display(Name = "Số lượng like")]
        public int Liked { get; set; }

        [Display(Name = "Ngày like")]
        [DisplayFormat(DataFormatString = SystemConfig.DateTimeFormat)]
        public DateTime? CreateDate { get; set; }
    }
}
