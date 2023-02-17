using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Keyless]
    public partial class ViewPriorityModel
    {
        [StringLength(100)]
        public string PriorityCode { get; set; }
        [StringLength(1000)]
        public string PriorityName { get; set; }
    }
}
