using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("APIModel", Schema = "tMasterData")]
    public partial class Apimodel
    {
        [Key]
        [StringLength(100)]
        public string Token { get; set; }
        [Required]
        [StringLength(100)]
        public string Key { get; set; }
        [Column("isAllowedToBooking")]
        public bool? IsAllowedToBooking { get; set; }
        [Column("isRequiredLogin")]
        public bool? IsRequiredLogin { get; set; }
        [Column("isReceiveInCurrentDay")]
        public bool? IsReceiveInCurrentDay { get; set; }
    }
}
