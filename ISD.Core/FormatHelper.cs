using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.Core
{
    public static class FormatHelper
    {
        public static string FormatCurrency(this object str)
        {
            if (str != null && !string.IsNullOrEmpty(str.ToString()))
            {
                //CultureInfo cultureInfo = CultureInfo.GetCultureInfo("vi-VN");
                //return string.Format(cultureInfo, "{0:n0}", str);
                return string.Format("{0:n0}", str);
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
