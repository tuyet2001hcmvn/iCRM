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
    
    public partial class TaskCommentModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TaskCommentModel()
        {
            this.Comment_File_Mapping = new HashSet<Comment_File_Mapping>();
        }
    
        public System.Guid TaskCommentId { get; set; }
        public Nullable<System.Guid> TaskId { get; set; }
        public string Comment { get; set; }
        public string ProcessUser { get; set; }
        public Nullable<System.Guid> FromStatusId { get; set; }
        public Nullable<System.Guid> ToStatusId { get; set; }
        public Nullable<System.Guid> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public Nullable<System.Guid> LastEditBy { get; set; }
        public Nullable<System.DateTime> LastEditTime { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comment_File_Mapping> Comment_File_Mapping { get; set; }
    }
}