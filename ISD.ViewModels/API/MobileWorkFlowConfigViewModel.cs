using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class MobileWorkFlowConfigViewModel
    {
        public string FieldCode { get; set; }
        public string FieldName { get; set; }
        public int? OrderIndex { get; set; }
        public bool? IsRequired { get; set; }
        public Nullable<bool> HideWhenAdd { get; set; }
        public string AddDefaultValue { get; set; }
        public Nullable<bool> HideWhenEdit { get; set; }
        public string EditDefaultValue { get; set; }
    }

    public class MobileDynamicWorkFlowConfigViewModel
    {
        public string typeField { get; set; }
        public string textField { get; set; }
        public string placeHolder { get; set; }
        public string iconType { get; set; }
        public string iconName { get; set; }
        public string stateName { get; set; }
        public object stateValue { get; set; }
        public int? iconSize { get; set; }
        public List<ISDSelectStringItem> DropDownData { get; set; }
    }
}
