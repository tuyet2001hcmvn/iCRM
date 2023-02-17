using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("DepartmentModel", Schema = "tMasterData")]
    public partial class DepartmentModel
    {
        public DepartmentModel()
        {
            SalesEmployeeModels = new HashSet<SalesEmployeeModel>();
        }

        [Key]
        public Guid DepartmentId { get; set; }
        public Guid? CompanyId { get; set; }
        public Guid? StoreId { get; set; }
        [StringLength(100)]
        public string DepartmentCode { get; set; }
        [StringLength(100)]
        public string DepartmentName { get; set; }
        public int? OrderIndex { get; set; }
        public Guid? CreateBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateTime { get; set; }
        public Guid? LastEditBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LastEditTime { get; set; }
        public bool? Actived { get; set; }

        [ForeignKey(nameof(StoreId))]
        [InverseProperty(nameof(StoreModel.DepartmentModels))]
        public virtual StoreModel Store { get; set; }
        [InverseProperty(nameof(SalesEmployeeModel.Department))]
        public virtual ICollection<SalesEmployeeModel> SalesEmployeeModels { get; set; }
    }
}
