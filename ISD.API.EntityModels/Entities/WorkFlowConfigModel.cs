﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("WorkFlowConfigModel", Schema = "Task")]
    public partial class WorkFlowConfigModel
    {
        [Key]
        public Guid WorkFlowId { get; set; }
        [Key]
        [StringLength(50)]
        public string FieldCode { get; set; }
        public bool? IsRequired { get; set; }
        public int? OrderIndex { get; set; }
        [StringLength(200)]
        public string Parameters { get; set; }
        [StringLength(4000)]
        public string Note { get; set; }
        public bool? HideWhenAdd { get; set; }
        [StringLength(500)]
        public string AddDefaultValue { get; set; }
        public bool? HideWhenEdit { get; set; }
        [StringLength(500)]
        public string EditDefaultValue { get; set; }
    }
}
