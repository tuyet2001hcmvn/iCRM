using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("News_Company_Mapping", Schema = "tMasterData")]
    public partial class NewsCompanyMapping
    {
        [Key]
        public Guid NewsId { get; set; }
        [Key]
        public Guid CompanyId { get; set; }
    }
}
