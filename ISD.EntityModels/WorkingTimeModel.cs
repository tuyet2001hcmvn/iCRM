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
    
    public partial class WorkingTimeModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public WorkingTimeModel()
        {
            this.WorkingTimeDetailModel = new HashSet<WorkingTimeDetailModel>();
        }
    
        public System.Guid WorkingTimeId { get; set; }
        public Nullable<int> DayOfWeek { get; set; }
        public Nullable<System.TimeSpan> FromTime { get; set; }
        public Nullable<System.TimeSpan> ToTime { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WorkingTimeDetailModel> WorkingTimeDetailModel { get; set; }
    }
}
