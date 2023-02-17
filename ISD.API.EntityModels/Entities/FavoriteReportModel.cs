using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("FavoriteReportModel", Schema = "tMasterData")]
    public partial class FavoriteReportModel
    {
        [Key]
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public Guid PageId { get; set; }

        [ForeignKey(nameof(AccountId))]
        [InverseProperty(nameof(AccountModel.FavoriteReportModels))]
        public virtual AccountModel Account { get; set; }
        [ForeignKey(nameof(PageId))]
        [InverseProperty(nameof(PageModel.FavoriteReportModels))]
        public virtual PageModel Page { get; set; }
    }
}
