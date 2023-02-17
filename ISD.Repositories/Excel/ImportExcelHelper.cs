using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.Repositories.Excel
{
    public class ImportExcelHelper
    {
        private string _filepath;      
        public ImportExcelHelper(string filepath)
        {
          
            _filepath = filepath;
        }
        //private string CreateFilePath()
        //{
        //    string filePath = "";
        //    if (_file != null && _file.Length > 0)
        //    {
        //        //Check file is excel
        //        if (_file.FileName.Contains("xls") || _file.FileName.Contains("xlsx"))
        //        {
        //            var fileName = Path.GetFileName(_file.FileName);
        //            //if (!Directory.Exists(mapPath))
        //            //{
        //            //    Directory.CreateDirectory(mapPath);
        //            //}
        //            //var path = Path.Combine(mapPath, fileName);
        //            // var mapPath = Server.MapPath("~/Upload/ImportExcel/");
        //            // var path = HostingEnvironment.MapPath("~/Upload/ImportExcel/");
        //            var rootFolder = Directory.GetCurrentDirectory() + "\\Upload\\ImportExcel";
        //            filePath = Path.Combine(rootFolder, fileName);
        //        }
        //        using (var fileStream = new FileStream(filePath, FileMode.Create))
        //        {
        //            _file.CopyTo(fileStream);
        //        }
        //    }
        //    return filePath;
        //}
       
        public DataSet GetDataSet(bool? isUseHeaderRow = null)
        {
            //Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            using (var stream = System.IO.File.Open(_filepath, FileMode.Open, FileAccess.Read))
            {

                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {

                    var conf = new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = a => new ExcelDataTableConfiguration
                        {
                            UseHeaderRow = isUseHeaderRow == true ? true : false
                        }
                    };
                    DataSet dataSet = reader.AsDataSet(conf);
                    return dataSet;
                }
            }
        }
    }
}
