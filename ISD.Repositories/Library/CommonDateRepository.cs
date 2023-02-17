using ISD.EntityModels;
using ISD.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.Repositories
{
    public class CommonDateRepository
    {
        EntityDataContext _context;

        UtilitiesRepository _utilitiesRepository;
        public CommonDateRepository(EntityDataContext ct)
        {
            _context = ct;
            _utilitiesRepository = new UtilitiesRepository();
        }

        public bool GetDateBy(string CommonDate, out DateTime? FromDate, out DateTime? Todate)
        {
            try
            {
                var dimDate = _context.DimDateModel.Find(_utilitiesRepository.ConvertDateTimeToInt(DateTime.Now));
                if (dimDate != null)
                {
                    switch (CommonDate)
                    {
                        //Hôm qua
                        case "Yesterday":
                            FromDate = DateTime.Now.Date.AddDays(-1);
                            Todate = DateTime.Now.Date.AddSeconds(-1);
                            break;
                        //Hôm nay
                        case "Today":
                            FromDate = DateTime.Now.Date;
                            Todate = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
                            break;
                        //Ngày mai
                        case "Tomorrow":
                            FromDate = DateTime.Now.Date.AddDays(1);
                            Todate = DateTime.Now.Date.AddDays(2).AddSeconds(-1);
                            break;
                        //Tuần trước
                        case "LastWeek":
                            FromDate = DateTime.Now.StartOfWeek(DayOfWeek.Monday).AddDays(-7);
                            Todate = DateTime.Now.StartOfWeek(DayOfWeek.Monday).AddSeconds(-1);
                            break;
                        //Tuần này
                        case "ThisWeek":
                            FromDate = DateTime.Now.StartOfWeek(DayOfWeek.Monday);
                            Todate = FromDate.Value.AddDays(7).AddSeconds(-1);
                            break;
                        //Tuần sau
                        case "NextWeek":
                            FromDate = DateTime.Now.StartOfWeek(DayOfWeek.Monday).AddDays(7);
                            Todate = FromDate.Value.AddDays(7).AddSeconds(-1);
                            break;
                        //Tháng trước
                        case "LastMonth":
                            FromDate = dimDate.FirstDayOfMonth.Value.AddMonths(-1);
                            Todate = dimDate.FirstDayOfMonth.Value.AddSeconds(-1);
                            break;
                        //Tháng này
                        case "ThisMonth":
                            FromDate = dimDate.FirstDayOfMonth;
                            Todate = dimDate.LastDayOfMonth.Value.AddDays(1).AddSeconds(-1);
                            break;
                        //Tháng sau
                        case "NextMonth":
                            FromDate = dimDate.FirstDayOfMonth.Value.AddMonths(1);
                            Todate = dimDate.FirstDayOfMonth.Value.AddMonths(2).AddSeconds(-1);
                            break;
                        //Quý trước
                        case "LastQuarter":
                            FromDate = dimDate.FirstDayOfQuarter.Value.AddMonths(-3);
                            Todate = dimDate.FirstDayOfQuarter.Value.AddSeconds(-1);
                            break;
                        //Quý này
                        case "ThisQuarter":
                            FromDate = dimDate.FirstDayOfQuarter;
                            Todate = dimDate.LastDayOfQuarter.Value.AddDays(1).AddSeconds(-1);
                            break;
                        //Quý sau
                        case "NextQuater":
                            FromDate = dimDate.FirstDayOfQuarter.Value.AddMonths(3);
                            Todate = dimDate.FirstDayOfQuarter.Value.AddMonths(6).AddSeconds(-1);
                            break;
                        //Năm ngoái
                        case "LastYear":
                            FromDate = dimDate.FirstDayOfYear.Value.AddYears(-1);
                            Todate = dimDate.FirstDayOfYear.Value.AddSeconds(-1);
                            break;
                        //Năm nay
                        case "ThisYear":
                            FromDate = dimDate.FirstDayOfYear;
                            Todate = dimDate.LastDayOfYear.Value.AddDays(1).AddSeconds(-1);
                            break;
                        //Năm sau
                        case "NextYear":
                            FromDate = dimDate.FirstDayOfYear.Value.AddYears(1);
                            Todate = dimDate.FirstDayOfYear.Value.AddYears(2).AddSeconds(-1);
                            break;
                        //Tùy chỉnh
                        case "Custom":
                            FromDate = null;
                            Todate = null;
                            break;
                        default:
                            FromDate = null;
                            Todate = null;
                            break;
                    }
                    return true;
                }
                FromDate = null;
                Todate = null;
                return false;
            }
            catch (Exception)
            {
                FromDate = null;
                Todate = null;
                return false;
            }

        }
        public bool GetDateBy(string CommonDate, out DateTime? FromDate, out DateTime? Todate, out DateTime? FromPreviousDay, out DateTime? ToPreviousDay)
        {
            try
            {
                var dimDate = _context.DimDateModel.Find(_utilitiesRepository.ConvertDateTimeToInt(DateTime.Now));
                if (dimDate != null)
                {
                    switch (CommonDate)
                    {
                        //Hôm nay
                        case "Today":
                            FromDate = DateTime.Now.Date;
                            Todate = DateTime.Now.Date;
                            FromPreviousDay = DateTime.Now.Date.AddDays(-1);
                            ToPreviousDay = DateTime.Now.Date.AddDays(-1);
                            break;
                        //Hôm qua
                        case "Yesterday":
                            FromDate = DateTime.Now.Date.AddDays(-1);
                            Todate = DateTime.Now.Date.AddDays(-1);
                            FromPreviousDay = DateTime.Now.Date.AddDays(-2);
                            ToPreviousDay = DateTime.Now.Date.AddDays(-2);
                            break;
                        //Tuần này
                        case "ThisWeek":
                            FromDate = DateTime.Now.StartOfWeek(DayOfWeek.Monday);
                            Todate = FromDate.Value.AddDays(6);
                            FromPreviousDay = DateTime.Now.StartOfWeek(DayOfWeek.Monday).AddDays(-7);
                            ToPreviousDay = DateTime.Now.StartOfWeek(DayOfWeek.Monday).AddDays(-1);
                            break;
                        //Tuần trước
                        case "LastWeek":
                            FromDate = DateTime.Now.StartOfWeek(DayOfWeek.Monday).AddDays(-7);
                            Todate = DateTime.Now.StartOfWeek(DayOfWeek.Monday).AddDays(-1);
                            FromPreviousDay = FromDate.Value.AddDays(-7);
                            ToPreviousDay = FromDate.Value.AddDays(-1);
                            break;
                        //Tháng này
                        case "ThisMonth":
                            FromDate = dimDate.FirstDayOfMonth;
                            Todate = dimDate.LastDayOfMonth;
                            FromPreviousDay = dimDate.FirstDayOfMonth.Value.AddMonths(-1);
                            ToPreviousDay = dimDate.FirstDayOfMonth.Value.AddDays(-1);
                            break;
                        //Tháng trước
                        case "LastMonth":
                            FromDate = dimDate.FirstDayOfMonth.Value.AddMonths(-1);
                            Todate = dimDate.FirstDayOfMonth.Value.AddDays(-1);
                            FromPreviousDay = FromDate.Value.AddMonths(-1);
                            ToPreviousDay = FromDate.Value.AddDays(-1);
                            break;
                        //Quý này
                        case "ThisQuarter":
                            FromDate = dimDate.FirstDayOfQuarter;
                            Todate = dimDate.LastDayOfQuarter;
                            FromPreviousDay = dimDate.FirstDayOfQuarter.Value.AddMonths(-3);
                            ToPreviousDay = dimDate.FirstDayOfQuarter.Value.AddDays(-1);
                            break;
                        //Quý trước
                        case "LastQuarter":
                            FromDate = dimDate.FirstDayOfQuarter.Value.AddMonths(-3);
                            Todate = dimDate.FirstDayOfQuarter.Value.AddDays(-1);
                            FromPreviousDay = FromDate.Value.AddMonths(-3);
                            ToPreviousDay = FromDate.Value.AddDays(-1);
                            break;
                        //Năm nay
                        case "ThisYear":
                            FromDate = dimDate.FirstDayOfYear;
                            Todate = dimDate.LastDayOfYear;
                            FromPreviousDay = dimDate.FirstDayOfYear.Value.AddYears(-1);
                            ToPreviousDay = dimDate.FirstDayOfYear.Value.AddDays(-1);
                            break;
                        //Năm ngoái
                        case "LastYear":
                            FromDate = dimDate.FirstDayOfYear.Value.AddYears(-1);
                            Todate = dimDate.FirstDayOfYear.Value.AddDays(-1);
                            FromPreviousDay = FromDate.Value.AddYears(-1);
                            ToPreviousDay = FromDate.Value.AddDays(-1);
                            break;
                        //Tùy chỉnh
                        case "Custom":
                            FromDate = null;
                            Todate = null;
                            FromPreviousDay = null;
                            ToPreviousDay = null;
                            break;
                        default:
                            FromDate = null;
                            Todate = null;
                            FromPreviousDay = null;
                            ToPreviousDay = null;
                            break;
                    }
                    if (Todate.HasValue)
                    {
                        Todate = Todate.Value.Date.AddDays(1).AddSeconds(-1);
                    }
                    if (ToPreviousDay.HasValue)
                    {
                        ToPreviousDay = ToPreviousDay.Value.Date.AddDays(1).AddSeconds(-1);
                    }
                    return true;
                }
                FromDate = null;
                Todate = null;
                FromPreviousDay = null;
                ToPreviousDay = null;
                return false;
            }
            catch (Exception)
            {
                FromDate = null;
                Todate = null;
                FromPreviousDay = null;
                ToPreviousDay = null;
                return false;
            }

        }
    }
}
