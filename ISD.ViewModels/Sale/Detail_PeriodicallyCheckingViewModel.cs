using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ISD.ViewModels
{
    public class Detail_PeriodicallyCheckingViewModel
    {
        public int STT { get; set; }

        public System.Guid ProductId { get; set; }
        
        //Product details
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_ProductCode")]
        public string ProductCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_ERPProductCode")]
        public string ERPProductCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Product_ProductName")]
        public string ProductName { get; set; }

        //Category
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_Category")]
        public System.Guid? CategoryId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_Category")]
        public string CategoryName { get; set; }

        //Configuration
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_Configuration")]
        public System.Guid ConfigurationId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Sale_Configuration")]
        public string ConfigurationName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public Nullable<bool> Actived { get; set; }
    }
}
