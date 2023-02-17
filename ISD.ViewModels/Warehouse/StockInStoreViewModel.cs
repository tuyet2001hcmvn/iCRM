using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class StockInStoreViewModel
    {
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Warehouse_Stock")]
        public Guid StockId { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "MasterData_Store")]
        public Guid StoreId { get; set; }
    }
}
