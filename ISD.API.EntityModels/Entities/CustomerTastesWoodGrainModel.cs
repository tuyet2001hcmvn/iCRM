using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("CustomerTastes_WoodGrainModel", Schema = "Customer")]
    public partial class CustomerTastesWoodGrainModel
    {
        [Key]
        public Guid WoodGrainId { get; set; }
        [Required]
        [StringLength(50)]
        public string WoodGrainCode { get; set; }
        [StringLength(200)]
        public string WoodGrainName { get; set; }
        public Guid? ProfileId { get; set; }
        public Guid? CompanyId { get; set; }
        public Guid? StoreId { get; set; }
        public Guid? AppointmentId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
    }
}
