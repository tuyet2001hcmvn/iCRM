using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ISD.ViewModels
{
    public class SearchSaleOrderDetailViewModel
    {
        public string CustomerCode { get; set; }
        
        public string SaleOrderCode { get; set; }
        
        public string CreatedDate { get; set; }

        public string Quantity { get; set; }

        public string Product { get; set; }

        public string Description { get; set; }

        //Số lượng
        public string TransDate
        {
            get
            {

                if (TransDateTmp == null)
                {
                    return "";
                }
                else
                {
                    return string.Format("{0:n0}", Convert.ToDecimal(TransDateTmp));
                }
            }
        }
        public string TransDateTmp { protected get; set; }

        //Thành tiền
        public string AmountMST
        {
            get
            {

                if (AmountMSTTmp == null)
                {
                    return "";
                }
                else
                {
                    return string.Format("{0:n0} đ", Convert.ToDecimal(AmountMSTTmp));
                }
            }
        }
        public string AmountMSTTmp { protected get; set; }

        public string StoreName { get; set; }

        public string TransType { get; set; }

        public string SaleOrderType { get; set; }
    }
}
