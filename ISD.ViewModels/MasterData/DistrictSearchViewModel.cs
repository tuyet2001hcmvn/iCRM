using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.MasterData
{
    public class DistrictSearchViewModel
    {
        public Guid? ProvinceId { get; set; }
        public string DistrictName { get; set; }
        public bool? Actived { get; set; }
    }
}
