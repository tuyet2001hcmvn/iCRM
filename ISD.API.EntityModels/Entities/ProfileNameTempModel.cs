using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Keyless]
    [Table("ProfileNameTempModel", Schema = "Customer")]
    public partial class ProfileNameTempModel
    {
        [StringLength(50)]
        public string ProfileCode { get; set; }
        [StringLength(4000)]
        public string FullName { get; set; }
        [StringLength(1000)]
        public string ShortName { get; set; }
    }
}
