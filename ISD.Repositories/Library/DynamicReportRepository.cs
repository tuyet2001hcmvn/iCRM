using ISD.EntityModels;
using ISD.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ISD.Repositories
{
    public class DynamicReportRepository
    {
        // Hướng dẫn sử dụng: 
        // Bỏ viewmodel report vào
        // Viewmodel khai báo ví dụ
        //   FullName = "Lê Xuân Tiến"
        //   FullNameLabel = "Họ Tên"
        public List<MobileReportMasterViewModel> DynamicMobileReport<T>(List<T> reportData)
        {
            // get all public static properties of MyClass type
            PropertyInfo[] propertyInfos;
            propertyInfos = typeof(T).GetProperties();
            // sort properties by name

            List<string> labelArray = new List<string>();
            // write property names
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                if (propertyInfo.Name.IndexOf("Label") == -1)
                {
                    labelArray.Add(propertyInfo.Name);
                }
            }

            //	reportData.Dump();

            //	JsonConvert.SerializeObject(reportData).Dump();

            List<MobileReportMasterViewModel> report = new List<MobileReportMasterViewModel>();
            foreach (T data in reportData)
            {
                var master = new MobileReportMasterViewModel();
                master.data = new List<MobileReportItemViewModel>();

                foreach (var label in labelArray)
                {
                    var value = ReflectionHelper.GetPropValue(data, label);
                    if (value != null)
                    {
                        string displayValue = value.ToString();
                        master.data.Add(new MobileReportItemViewModel()
                        {
                            //				Label = label,
                            label = ReflectionHelper.GetPropValue(data, label + "Label").ToString(),
                            value = displayValue,
                        });
                    }
                }

                report.Add(master);
            }

            return report;
        }

    }
}
