using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ISD.API2.Middlewares
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        IWebHostEnvironment _env;

        public RequestResponseLoggingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory, IWebHostEnvironment env)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<RequestResponseLoggingMiddleware>();
            _env = env;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            finally
            {
                string mess =string.Format("From {3} Request {0} {1} => {2}", context.Request?.Method,context.Request?.Path.Value,context.Response?.StatusCode,context.Request?.Headers["Referer"]);
                WriteLogFile(Path.Combine(_env.ContentRootPath, "log.txt"), mess);
            }
        }
        public static void WriteLogFile(string filePath, string message)
        {
            if (System.IO.File.Exists(filePath))
            {
                if (!System.IO.File.Exists(filePath))
                    System.IO.File.Create(filePath);
            }
            using (FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
            {
                fileStream.Flush();
                fileStream.Close();
            }

            using (StreamWriter sw = new StreamWriter(filePath, true))
            {
                string lastRecordText = "# " + System.DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + " # " + Environment.NewLine + "#" + message + " #" + Environment.NewLine;
                sw.WriteLine(lastRecordText);
                sw.Close();
            }
        }
    }
}

