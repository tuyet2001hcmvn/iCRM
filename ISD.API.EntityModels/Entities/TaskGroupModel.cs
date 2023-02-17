using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("TaskGroupModel", Schema = "Task")]
    public partial class TaskGroupModel
    {
        [Key]
        public Guid GroupId { get; set; }
        [StringLength(1000)]
        public string GroupName { get; set; }
        public Guid? CreatedAccountId { get; set; }
    }
}
