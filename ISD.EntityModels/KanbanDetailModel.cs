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
    
    public partial class KanbanDetailModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public KanbanDetailModel()
        {
            this.Kanban_TaskStatus_Mapping = new HashSet<Kanban_TaskStatus_Mapping>();
        }
    
        public System.Guid KanbanDetailId { get; set; }
        public Nullable<System.Guid> KanbanId { get; set; }
        public string ColumnName { get; set; }
        public Nullable<int> OrderIndex { get; set; }
        public string Note { get; set; }
        public Nullable<System.Guid> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public Nullable<System.Guid> LastEditBy { get; set; }
        public Nullable<System.DateTime> LastEditTime { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Kanban_TaskStatus_Mapping> Kanban_TaskStatus_Mapping { get; set; }
        public virtual KanbanModel KanbanModel { get; set; }
    }
}
