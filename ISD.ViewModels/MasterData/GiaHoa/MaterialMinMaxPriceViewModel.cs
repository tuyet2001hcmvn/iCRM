using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.Sale
{
    public class MaterialMinMaxPriceViewModel
    {
        public System.Guid MaterialMinMaxPriceId { get; set; }

        public System.Guid StoreId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Store")]
        public string StoreName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_ProductCode")]
        public string MaterialCode { get; set; }

        [Display(Name ="Tên xe")]
        public string MaterialName { get; set; }

        //Giá min
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MinPrice")]
        [DisplayFormat(DataFormatString = "{0:n0}")]
        public decimal? MinPrice { get; set; }

        //Giá max: giá đề xuất
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MaxPrice")]
        [DisplayFormat(DataFormatString = "{0:n0}")]
        public decimal? MaxPrice { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "EffectedFromDate")]
        public DateTime? EffectedFromDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "EffectedToDate")]
        public DateTime? EffectedToDate { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreatedUser")]
        public string CreatedUser { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "CreatedTime")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> CreatedTime { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "LastModifiedUser")]
        public string LastModifiedUser { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "LastModifiedTime")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> LastModifiedTime { get; set; }

        public List<Guid> ActivedStoreList { get; set; }

        //Excel
        public int RowIndex { get; set; }
        public string Error { get; set; }
        public bool isNullValueId { get; set; }
    }
}
