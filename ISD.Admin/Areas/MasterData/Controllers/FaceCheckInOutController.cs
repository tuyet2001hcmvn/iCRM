using ISD.Core;
using ISD.Repositories;
using ISD.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MasterData.Controllers
{
    public class FaceCheckInOutController : BaseController
    {
        #region Index
        // GET: FaceCheckInOut
        public ActionResult Index()
        {
            var currentDate = DateTime.Now;
            ViewBag.FromDate = currentDate;
            ViewBag.ToDate = currentDate;
            return View();
        }
        public ActionResult _Search(FaceCheckInOutSearchViewModel searchViewModel)
        {
            //Session["frmSearchDistrict"] = searchViewModel;

            return ExecuteSearch(() =>
            {
                FaceCheckInOutRepository _faceRepository = new FaceCheckInOutRepository(_context);
                var listFaceCheckIntOut = _faceRepository.GetListFaceCheckInByDate(searchViewModel.FromDate, searchViewModel.ToDate,searchViewModel.FaceType);

                return PartialView(listFaceCheckIntOut);
            });

        }
        #endregion End Index
        #region Create
        public ActionResult Create()
        {
            return View();
        }
        #endregion End Create

        #region Edit
        public ActionResult Edit(string id)
        {
            //using (var client = new HttpClient())
            //{
            //    //client.BaseAddress = new Uri("https://partner.hanet.ai/");
            //    var url = new Uri("https://partner.hanet.ai/person/getUserInfoByPersonID");
            //    //HTTP GET
            //    var param = new
            //    {
            //         token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJwYXJ0bmVySUQiOjI2NywicGFydG5lck5hbWUiOiJpY3JtIiwiaWQiOiIxODcwNzkzMTUyNTI1MDAwNzA0IiwiZW1haWwiOiJuYW0udmhAaXNkY29ycC52biIsInRzIjoxNjE4Mjk4MTMwMTMyLCJjbGllbnRfaWQiOiI1OTdlNzZjOTMzM2IzNGFkZGVmYjZlOTZlNjYzZWI0ZSIsImNvZGUiOiIiLCJjb2RlX2NoYWxsZW5nZSI6IiIsInR5cGUiOiJhdXRob3JpemF0aW9uX2NvZGUiLCJpYXQiOjE2MTgyOTgxMzAsImV4cCI6MTY0OTgzNDEzMH0.VTScZ-F0Z6rRr6ajnx9BxcuFA5dbW_SiWia0niyyfUw",
            //        personID= "1870914709813485568"
            //    };
            //    string  json = JsonConvert.SerializeObject(param);
            //    StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            //    var responseTask = client.PostAsync(url, httpContent);
            //    responseTask.Wait();

            //    var result = responseTask.Result;
            //    if (result.IsSuccessStatusCode)
            //    {
            //        var readTask = result.Content.ReadAsStringAsync();
            //        readTask.Wait();

            //        //students = readTask.Result;
            //    }
            //    else //web api sent error response 
            //    {
            //        //log response status here..

            //        //students = Enumerable.Empty<StudentViewModel>();

            //        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            //    }
            //}
            //var client = new RestClient("https://partner.hanet.ai/person/getUserInfoByPersonID");
            //var request = new RestRequest(Method.POST);
            //request.AddHeader("content-type", "application/json");
            //request.AddParameter("application/json", "{\n  \"name\":\"camera.listFiles\"\n}\n", ParameterType.RequestBody);
            //IRestResponse response = client.Execute(request);
            return View();
        }
        #endregion
    }
}