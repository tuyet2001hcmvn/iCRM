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
    
    public partial class TaskAssignModel
    {
        public System.Guid TaskAssignId { get; set; }
        public Nullable<System.Guid> TaskId { get; set; }
        public string SalesEmployeeCode { get; set; }
        public string TaskAssignTypeCode { get; set; }
        public Nullable<System.Guid> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public string RolesCode { get; set; }
        public Nullable<System.Guid> GroupId { get; set; }
    }
}
