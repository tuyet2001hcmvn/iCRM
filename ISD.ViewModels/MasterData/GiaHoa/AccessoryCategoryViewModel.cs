using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.Sale
{
    public class AccessoryCategoryViewModel
    {
        public Guid AccessoryCategoryId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "AccessoryCategory_AccessoryCategoryCode")]
        public string AccessoryCategoryCode { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "AccessoryCategory_AccessoryCategoryName")]
        public string AccessoryCategoryName { get; set; }
    }
}
