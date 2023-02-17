using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.API
{
    public class PriceListViewModel
    {
        public Guid? ColorId { get; set; }
        public Guid? StyleId { get; set; }
        public decimal? Price { protected get; set; }
        public string PriceWithFormat
        {
            get { return string.Format("{0:n0}đ", Price); }
        }
        public DateTime? PostDate { protected get; set; }
        public TimeSpan? PostTime { protected get; set; }
        public string ApplyTime
        {            
            get {
                if (PostTime.HasValue)
                {
                    return string.Format("{0:dd/MM/yyyy} {1:00}:{2:00}:{3:00}", PostDate, PostTime.Value.Hours, PostTime.Value.Minutes, PostTime.Value.Seconds);
                }
                else
                {
                    return string.Format("{0:dd/MM/yyyy}", PostDate);
                }
            }
        }
    }
}
