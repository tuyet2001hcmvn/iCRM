using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Keyless]
    [Table("ProfileContactTempModel", Schema = "Customer")]
    public partial class ProfileContactTempModel
    {
        [StringLength(50)]
        public string ContactCode { get; set; }
        [StringLength(50)]
        public string ProfileCode { get; set; }
        [StringLength(4000)]
        public string ContactName { get; set; }
        [StringLength(1000)]
        public string Position { get; set; }
        [StringLength(1000)]
        public string Department { get; set; }
        [StringLength(1000)]
        public string Phone { get; set; }
        [StringLength(1000)]
        public string Mobile { get; set; }
        [StringLength(1000)]
        public string Email { get; set; }
        [StringLength(50)]
        public string Status { get; set; }
        public bool? Actived { get; set; }
    }
}
