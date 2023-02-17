using ISD.Constant;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class CatalogueViewModel : BaseClassViewModel
    {
        public Guid ProductId { get; set; }
        public Guid DeliveryDetailId { get; set; }
        //Nguồn KH
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_CustomerSourceCode")]
        public string CustomerSourceName { get; set; }

        //Mã SAP
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_ERPProductCode")]
        public string ERPProductCode { get; set; }

        //Mã catalogue
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Catalogue_ProductCode")]
        public string ProductCode { get; set; }

        //Tên catalogue
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Catalogue_ProductName")]
        public string ProductName { get; set; }

        //Số lượng
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Catalogue_Quantity")]
        [DisplayFormat(DataFormatString = SystemConfig.QuantityFormat)]
        public int? Quantity { get; set; }

        //Ngày lấy catalogue
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Catalogue_CreatedDate")]
        [DisplayFormat(DataFormatString = SystemConfig.DateTimeFormat)]
        public DateTime? CreatedDate { get; set; }

        public string customerCatalogueLst { get; set; }

        public bool? isDeleted { get; set; }
    }
}
