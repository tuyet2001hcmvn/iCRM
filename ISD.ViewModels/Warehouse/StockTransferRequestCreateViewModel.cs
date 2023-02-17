using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class StockTransferRequestCreateViewModel
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "StockTransferRequest_FromStock")]
        [Required]
        public Guid FromStock { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "StockTransferRequest_ToStock")]
        [Required]
        public Guid ToStock { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_Company")]
        public Guid? CompanyId { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Store")]
        public Guid? StoreId { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Note")]
        public string Note { get; set; }
    }
}
