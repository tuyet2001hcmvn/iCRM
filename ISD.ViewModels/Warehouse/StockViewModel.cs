using ISD.EntityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class StockViewModel : StockModel
    {
        public string CreateByName { get; set; }
        public List<StoreModel> StoreList { get; set; }
        
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "StockInStore")]
        public Guid StoreId { get; set; }
    }
}
