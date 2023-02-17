using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Keyless]
    [Table("ProfileAddressTempModel", Schema = "Customer")]
    public partial class ProfileAddressTempModel
    {
        [StringLength(50)]
        public string ProfileCode { get; set; }
        [StringLength(50)]
        public string Status { get; set; }
        public bool? Actived { get; set; }
        [StringLength(4000)]
        public string Address { get; set; }
    }
}
