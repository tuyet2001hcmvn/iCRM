//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ISD.EntityModels
{
    using System;
    using System.Collections.Generic;
    
    public partial class ColorModel
    {
        public System.Guid ColorId { get; set; }
        public string ColorCode { get; set; }
        public string ColorShortName { get; set; }
        public string ColorName { get; set; }
        public Nullable<int> OrderIndex { get; set; }
        public bool Actived { get; set; }
    }
}
