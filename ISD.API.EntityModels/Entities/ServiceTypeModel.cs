using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("ServiceTypeModel", Schema = "ghMasterData")]
    public partial class ServiceTypeModel
    {
        public ServiceTypeModel()
        {
            ServiceOrderDetailServiceModels = new HashSet<ServiceOrderDetailServiceModel>();
        }

        [Key]
        [StringLength(50)]
        public string ServiceTypeCode { get; set; }
        [StringLength(50)]
        public string ServiceTypeName { get; set; }
        public bool? Actived { get; set; }

        [InverseProperty(nameof(ServiceOrderDetailServiceModel.ServiceTypeCodeNavigation))]
        public virtual ICollection<ServiceOrderDetailServiceModel> ServiceOrderDetailServiceModels { get; set; }
    }
}
