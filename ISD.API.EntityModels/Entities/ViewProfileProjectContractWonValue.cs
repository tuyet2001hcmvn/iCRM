using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Keyless]
    public partial class ViewProfileProjectContractWonValue
    {
        public Guid? ProfileId { get; set; }
        [Column(TypeName = "decimal(38, 2)")]
        public decimal? ContractWonValue { get; set; }
    }
}
