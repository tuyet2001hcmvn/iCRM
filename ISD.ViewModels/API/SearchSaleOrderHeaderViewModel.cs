using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ISD.ViewModels
{
    public class SearchSaleOrderHeaderViewModel
    {
        public string CustomerCode { get; set; }
        
        public string FullName { get; set; }
        
        public string SaleOrderCode { get; set; }
        
        public string CreatedDate { get; set; }
        
        public string Total
        {
            get
            {

                if (TotalTmp == null)
                {
                    return "";
                }
                else
                {
                    //return string.Format("{0:n0} đ", TotalTmp);
                    return string.Format("{0:n0} đ", Convert.ToDecimal(TotalTmp));
                }
            }
        }
        public string TotalTmp { protected get; set; }

        public string StoreName { get; set; }

        public string SaleOrderType { get; set; }
    }
}
