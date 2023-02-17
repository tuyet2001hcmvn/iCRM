using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Keyless]
    public partial class ViewProfileProfilePhone
    {
        public Guid? ProfileId { get; set; }
        [StringLength(50)]
        public string Phone { get; set; }
    }
}
