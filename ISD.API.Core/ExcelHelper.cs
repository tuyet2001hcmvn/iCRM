using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using System;
using System.Data;
using System.IO;
using System.Text;

namespace ISD.API.Core
{
    public class ExcelHelper
    {
        private string _filepath;
        IFormFile _file;
        public ExcelHelper(IFormFile file)
        {
            _file = file;
            _filepath = CreateFilePath();
        }
        private string CreateFilePath()
        {
            string filePath = "";
            if (_file != null && _file.Length > 0)
            {
                //Check file is excel
                if (_file.FileName.Contains("xls") || _file.FileName.Contains("xlsx"))
                {
                    var fileName = Path.GetFileName(_file.FileName);
                    //if (!Directory.Exists(mapPath))
                    //{
                    //    Directory.CreateDirectory(mapPath);
                    //}
                    //var path = Path.Combine(mapPath, fileName);
                   // var mapPath = Server.MapPath("~/Upload/ImportExcel/");
                    // var path = HostingEnvironment.MapPath("~/Upload/ImportExcel/");
                    var rootFolder = Directory.GetCurrentDirectory() + "\\Upload\\ImportExcel";
                    filePath = Path.Combine(rootFolder, fileName);
                }
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    _file.CopyTo(fileStream);
                }
            }
            return filePath;
        }
        public DataSet GetDataSet()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            using (var stream = System.IO.File.Open(_filepath, FileMode.Open, FileAccess.Read))
            {

                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {

                    var conf = new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = a => new ExcelDataTableConfiguration
                        {
                            UseHeaderRow = false
                        }
                    };
                    DataSet dataSet = reader.AsDataSet(conf);
                    stream.Close();
                    File.Delete(_filepath);
                    return dataSet;
                }
            }
        }
    }

}

