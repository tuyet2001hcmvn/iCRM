using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Keyless]
    public partial class ViewProfileCompanyMapping
    {
        public Guid? ProfileId { get; set; }
        [StringLength(50)]
        public string CompanyCode { get; set; }
    }
}
