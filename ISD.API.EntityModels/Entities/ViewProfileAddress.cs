using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Keyless]
    public partial class ViewProfileAddress
    {
        public Guid? ProfileId { get; set; }
        [StringLength(4000)]
        public string Address { get; set; }
        public Guid? ProvinceId { get; set; }
        public Guid AreaId { get; set; }
    }
}
