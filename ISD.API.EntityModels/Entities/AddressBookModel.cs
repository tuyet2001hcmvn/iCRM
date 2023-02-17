using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("AddressBookModel", Schema = "Customer")]
    public partial class AddressBookModel
    {
        [Key]
        public Guid AddressBookId { get; set; }
        public Guid? ProfileId { get; set; }
        [StringLength(50)]
        public string AddressTypeCode { get; set; }
        [StringLength(500)]
        public string Address { get; set; }
        [StringLength(500)]
        public string Address2 { get; set; }
        public Guid? ProvinceId { get; set; }
        public Guid? DistrictId { get; set; }
        public Guid? WardId { get; set; }
        [StringLength(50)]
        public string CountryCode { get; set; }
        [StringLength(500)]
        public string Note { get; set; }
        public Guid? CreateBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateTime { get; set; }
        public Guid? LastEditBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LastEditTime { get; set; }
        [Column("isMain")]
        public bool? IsMain { get; set; }

        [ForeignKey(nameof(ProfileId))]
        [InverseProperty(nameof(ProfileModel.AddressBookModels))]
        public virtual ProfileModel Profile { get; set; }
    }
}
