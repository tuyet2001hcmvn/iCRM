using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Keyless]
    public partial class ViewTaskArea
    {
        public Guid TaskId { get; set; }
        public Guid AreaId { get; set; }
    }
}
