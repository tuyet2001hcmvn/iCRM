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
    
    public partial class ConfigurationModel
    {
        public System.Guid ConfigurationId { get; set; }
        public string ConfigurationCode { get; set; }
        public string ConfigurationName { get; set; }
        public Nullable<int> OrderIndex { get; set; }
        public bool Actived { get; set; }
    }
}
