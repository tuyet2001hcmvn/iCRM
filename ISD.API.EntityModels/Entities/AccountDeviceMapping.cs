using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("Account_Device_Mapping", Schema = "pms")]
    public partial class AccountDeviceMapping
    {
        [Key]
        public Guid AccountId { get; set; }
        [Key]
        [StringLength(100)]
        public string DeviceId { get; set; }
        [StringLength(200)]
        public string DeviceName { get; set; }
    }
}
