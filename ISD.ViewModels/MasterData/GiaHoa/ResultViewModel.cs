using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.Sale
{
    public class ResultViewModel
    {
        public int ProfitCenter { get; set; }
        public int ProductHierarchy { get; set; }
        public int MaterialGroup { get; set; }
        public int Labor { get; set; }
        public int MaterialFreight { get; set; }
        public int ExternalMaterial { get; set; }
        public int TemperatureCondition { get; set; }
        public int ContainerRequirement { get; set; }
        public int Material { get; set; }
        //Phụ kiện
        public int AccessoryCategory { get; set; }
        public int Accessory { get; set; }
        public int AccessoryPrice { get; set; }
        //Nhân viên
        public int SalesEmployee { get; set; }
        //Nhóm công việc
        public int ServiceType { get; set; }
    }

    #region Update MasterData
    public class UpdateLogViewModel
    {
        //public System.Guid UpdateId { get; set; }
        public string ID { get; set; }
        public string ZKEY { get; set; }
        public string FunctionName { get; set; }
        public string TimeSend { get; set; }
        public string TimeReceive { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public string Note { get; set; }
    }
    #endregion
}
