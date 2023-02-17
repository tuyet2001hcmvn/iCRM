using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("DimDateModel", Schema = "Warehouse")]
    public partial class DimDateModel
    {
        public DimDateModel()
        {
            DeliveryDetailModels = new HashSet<DeliveryDetailModel>();
            StockReceivingDetailModels = new HashSet<StockReceivingDetailModel>();
            TransferDetailModels = new HashSet<TransferDetailModel>();
        }

        [Key]
        public int DateKey { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Date { get; set; }
        [Column("FullDateUK")]
        [StringLength(10)]
        public string FullDateUk { get; set; }
        [Column("FullDateUSA")]
        [StringLength(10)]
        public string FullDateUsa { get; set; }
        [StringLength(2)]
        public string DayOfMonth { get; set; }
        [StringLength(4)]
        public string DaySuffix { get; set; }
        [StringLength(9)]
        public string DayName { get; set; }
        [Column("DayOfWeekUSA")]
        [StringLength(1)]
        public string DayOfWeekUsa { get; set; }
        [Column("DayOfWeekUK")]
        [StringLength(1)]
        public string DayOfWeekUk { get; set; }
        [StringLength(2)]
        public string DayOfWeekInMonth { get; set; }
        [StringLength(2)]
        public string DayOfWeekInYear { get; set; }
        [StringLength(3)]
        public string DayOfQuarter { get; set; }
        [StringLength(3)]
        public string DayOfYear { get; set; }
        [StringLength(1)]
        public string WeekOfMonth { get; set; }
        [StringLength(2)]
        public string WeekOfQuarter { get; set; }
        [StringLength(2)]
        public string WeekOfYear { get; set; }
        [StringLength(2)]
        public string Month { get; set; }
        [StringLength(9)]
        public string MonthName { get; set; }
        [StringLength(2)]
        public string MonthOfQuarter { get; set; }
        [StringLength(1)]
        public string Quarter { get; set; }
        [StringLength(9)]
        public string QuarterName { get; set; }
        [StringLength(4)]
        public string Year { get; set; }
        [StringLength(7)]
        public string YearName { get; set; }
        [StringLength(10)]
        public string MonthYear { get; set; }
        [Column("MMYYYY")]
        [StringLength(6)]
        public string Mmyyyy { get; set; }
        [Column(TypeName = "date")]
        public DateTime? FirstDayOfMonth { get; set; }
        [Column(TypeName = "date")]
        public DateTime? LastDayOfMonth { get; set; }
        [Column(TypeName = "date")]
        public DateTime? FirstDayOfQuarter { get; set; }
        [Column(TypeName = "date")]
        public DateTime? LastDayOfQuarter { get; set; }
        [Column(TypeName = "date")]
        public DateTime? FirstDayOfYear { get; set; }
        [Column(TypeName = "date")]
        public DateTime? LastDayOfYear { get; set; }
        [Column("IsHolidayUSA")]
        public bool? IsHolidayUsa { get; set; }
        public bool? IsWeekday { get; set; }
        [Column("HolidayUSA")]
        [StringLength(50)]
        public string HolidayUsa { get; set; }
        [Column("IsHolidayUK")]
        public bool? IsHolidayUk { get; set; }
        [Column("HolidayUK")]
        [StringLength(50)]
        public string HolidayUk { get; set; }
        [StringLength(3)]
        public string FiscalDayOfYear { get; set; }
        [StringLength(3)]
        public string FiscalWeekOfYear { get; set; }
        [StringLength(2)]
        public string FiscalMonth { get; set; }
        [StringLength(1)]
        public string FiscalQuarter { get; set; }
        [StringLength(9)]
        public string FiscalQuarterName { get; set; }
        [StringLength(4)]
        public string FiscalYear { get; set; }
        [StringLength(7)]
        public string FiscalYearName { get; set; }
        [StringLength(10)]
        public string FiscalMonthYear { get; set; }
        [Column("FiscalMMYYYY")]
        [StringLength(6)]
        public string FiscalMmyyyy { get; set; }
        [Column(TypeName = "date")]
        public DateTime? FiscalFirstDayOfMonth { get; set; }
        [Column(TypeName = "date")]
        public DateTime? FiscalLastDayOfMonth { get; set; }
        [Column(TypeName = "date")]
        public DateTime? FiscalFirstDayOfQuarter { get; set; }
        [Column(TypeName = "date")]
        public DateTime? FiscalLastDayOfQuarter { get; set; }
        [Column(TypeName = "date")]
        public DateTime? FiscalFirstDayOfYear { get; set; }
        [Column(TypeName = "date")]
        public DateTime? FiscalLastDayOfYear { get; set; }

        [InverseProperty(nameof(DeliveryDetailModel.DateKeyNavigation))]
        public virtual ICollection<DeliveryDetailModel> DeliveryDetailModels { get; set; }
        [InverseProperty(nameof(StockReceivingDetailModel.DateKeyNavigation))]
        public virtual ICollection<StockReceivingDetailModel> StockReceivingDetailModels { get; set; }
        [InverseProperty(nameof(TransferDetailModel.DateKeyNavigation))]
        public virtual ICollection<TransferDetailModel> TransferDetailModels { get; set; }
    }
}
