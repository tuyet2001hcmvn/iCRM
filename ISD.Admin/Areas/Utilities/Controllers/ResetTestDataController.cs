using ISD.Extensions;
using ISD.Resources;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using ISD.Core;

namespace Utilities.Controllers
{
    public class ResetTestDataController : BaseController
    {
        // GET: ResetTestData
        [ISDAuthorizationAttribute]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ResetData()
        {
            try
            {
                SqlConnection connectionString = new SqlConnection(ConfigurationManager.ConnectionStrings["cnStr"].ConnectionString);
                using (var conn = connectionString)
                using (var command = new SqlCommand("[dbo].[uspWebAdmin_ResetTestData]", conn)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    conn.Open();
                    command.ExecuteNonQuery();
                }
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = LanguageResource.ResetData_Success
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.NotModified,
                    Success = false,
                    Data = ex.Message
                });
            }
        }
    }
}