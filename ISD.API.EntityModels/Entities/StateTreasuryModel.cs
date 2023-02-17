using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("StateTreasuryModel", Schema = "ghMasterData")]
    public partial class StateTreasuryModel
    {
        [Key]
        public Guid StateTreasuryId { get; set; }
        [StringLength(50)]
        public string StateTreasuryCode { get; set; }
        [StringLength(200)]
        public string BankName { get; set; }
        [StringLength(100)]
        public string Account { get; set; }
        public bool? Actived { get; set; }
    }
}
