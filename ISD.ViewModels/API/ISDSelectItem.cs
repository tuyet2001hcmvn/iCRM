using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class ISDSelectItem
    {
        public Guid? value { get; set; }

        public string text { get; set; }

        public int? area { get; set; }
    }

    public class ISDSelectItem2
    {
        public string value { get; set; }

        public string text { get; set; }

        public int? orderIndex { get; set; }
    }

    public class ISDSelectStringItem
    {
        public string id { get; set; }

        public string name { get; set; }

        public string additional { get; set; }

        public string additional2 { get; set; }
    }

    public class ISDSelectStringItemWithGuid
    {
        public string id { get; set; }

        public string name { get; set; }

        public Guid value { get; set; }
    }

    public class ISDSelectStringItem_BaoHiem
    {
        public string id { get; set; }

        public string name { get; set; }

        public string warehousecode { get; set; }

        public string unit { get; set; }
    }

    public class ISDSelectGuidItem
    {
        public Guid id { get; set; }

        public string name { get; set; }

        public Guid? additionalGuid { get; set; }
    }

    public class ISDSelectGuidItemWithCode
    {
        public Guid id { get; set; }

        public string code { get; set; }
        public string name { get; set; }
    }

    public class ISDSelectGuidItemWithColor
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string color { get; set; }
        public string bgColor { get; set; }
        public string iconName { get; set; }
        public string iconType { get; set; }
    }

    public class ISDSelectStringItemWithColor
    {
        public string id { get; set; }
        public string name { get; set; }
        public string color { get; set; }
        public string bgColor { get; set; }
        public string iconName { get; set; }
        public string iconType { get; set; }
    }

    public class ISDSelectStringItemWithIcon
    {
        public string id { get; set; }
        public string name { get; set; }
        public string icon { get; set; }
        public string description { get; set; }
        public string additional { get; set; }
    }

    public class ISDSelectIntItem
    {
        public int id { get; set; }

        public string name { get; set; }
    }

    public class ISDRadioStringItem
    {
        public string label { get; set; }

        public string value { get; set; }
    }

    public class ISDSelectBoolItem
    {
        public bool? id { get; set; }

        public string name { get; set; }
    }
}
