using ISD.Constant;
using ISD.EntityModels;
using ISD.Extensions;
using ISD.Resources;
using ISD.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ISD.Core;

namespace MasterData.Controllers
{
    public class CustomerPromotionController : BaseController
    {
        // GET: CustomerPromotion
        #region Index
        [ISDAuthorizationAttribute]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult _Search(string PromotionName = "", DateTime? EffectFromDate = null, DateTime? EffectToDate = null)
        {
            return ExecuteSearch(() =>
            {
                if (EffectFromDate == null)
                {
                    EffectFromDate = new DateTime(2018, 01, 01);
                }
                if (EffectToDate == null)
                {
                    EffectToDate = DateTime.Now;
                }
                var promotionList = (from p in _context.CustomerPromotionModel
                                     orderby p.EffectToDate descending
                                     where
                                     //search by PromotionName
                                     (PromotionName == "" || p.PromotionName.Contains(PromotionName))
                                     //search by EffectDate
                                     && ((EffectFromDate <= p.EffectFromDate && p.EffectFromDate <= EffectToDate)
                                     || (EffectFromDate <= p.EffectToDate && p.EffectToDate <= EffectToDate))
                                     select p).ToList();

                return PartialView(promotionList);
            });
        }
        #endregion

        //GET: /CustomerPromotion/Create
        #region Create
        [ISDAuthorizationAttribute]
        public ActionResult Create()
        {
            return View();
        }
        //POST: Create
        [HttpPost]
        [ValidateAjax]
        [ValidateInput(false)] //need when using ckeditor, do not delete
        [ISDAuthorizationAttribute]
        public JsonResult Create(CustomerPromotionModel model, HttpPostedFileBase ImageUrl, List<PromotionViewModel> productList)
        {
            return ExecuteContainer(() =>
            {
                model.PromotionId = Guid.NewGuid();
                if (productList != null && productList.Count > 0)
                {
                    var ProductId = productList.Select(p => p.ProductId).ToList();
                    ManyToMany(model, ProductId);
                }
                if (ImageUrl != null)
                {
                    model.ImageUrl = Upload(ImageUrl, "CustomerPromotion");
                }
                _context.Entry(model).State = EntityState.Added;
                _context.SaveChanges();

                // Push notification
                //PushNotification(model);

                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.MasterData_Promotion.ToLower())
                });
            });
        }
        #endregion

        //GET: /CustomerPromotion/View
        #region View
        [ISDAuthorizationAttribute]
        public ActionResult View(Guid id)
        {
            var promotion = (from p in _context.CustomerPromotionModel.AsEnumerable()
                             where p.PromotionId == id
                             select new PromotionViewModel()
                             {
                                 PromotionId = p.PromotionId,
                                 PromotionCode = p.PromotionCode,
                                 PromotionName = p.PromotionName,
                                 EffectFromDate = p.EffectFromDate,
                                 EffectToDate = p.EffectToDate,
                                 Description = p.Description,
                                 ImageUrl = p.ImageUrl,
                                 Notes = p.Notes
                             }).FirstOrDefault();

            if (promotion == null)
            {
                return RedirectToAction("ErrorText", "Error", new { area = "", statusCode = 404, exception = string.Format(LanguageResource.Error_NotExist, LanguageResource.MasterData_Promotion.ToLower()) });
            }
            return View(promotion);
        }
        #endregion View

        //GET: /CustomerPromotion/Edit
        #region Edit
        [ISDAuthorizationAttribute]
        public ActionResult Edit(Guid id)
        {
            var promotion = (from p in _context.CustomerPromotionModel.AsEnumerable()
                             where p.PromotionId == id
                             select new PromotionViewModel()
                             {
                                 PromotionId = p.PromotionId,
                                 PromotionCode = p.PromotionCode,
                                 PromotionName = p.PromotionName,
                                 EffectFromDate = p.EffectFromDate,
                                 EffectToDate = p.EffectToDate,
                                 Description = p.Description,
                                 ImageUrl = p.ImageUrl,
                                 Notes = p.Notes
                             }).FirstOrDefault();

            if (promotion == null)
            {
                return RedirectToAction("ErrorText", "Error", new { area = "", statusCode = 404, exception = string.Format(LanguageResource.Error_NotExist, LanguageResource.MasterData_Promotion.ToLower()) });
            }
            CreateViewBag();
            return View(promotion);
        }
        [HttpPost]
        [ValidateAjax]
        [ValidateInput(false)] //need when using ckeditor, do not delete
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CustomerPromotionModel model, HttpPostedFileBase ImageUrl, List<PromotionViewModel> productList)
        {
            return ExecuteContainer(() =>
            {
                var promotion = _context.CustomerPromotionModel.FirstOrDefault(p => p.PromotionId == model.PromotionId);
                if (promotion != null)
                {
                    promotion.PromotionCode = model.PromotionCode;
                    promotion.PromotionName = model.PromotionName;
                    promotion.EffectFromDate = model.EffectFromDate;
                    promotion.EffectToDate = model.EffectToDate;
                    promotion.Description = model.Description;
                    promotion.Notes = model.Notes;

                    if (productList != null && productList.Count > 0)
                    {
                        var ProductId = productList.Select(p => p.ProductId).ToList();
                        ManyToMany(promotion, ProductId);
                    }
                    else
                    {
                        if (promotion.ProductModel != null)
                        {
                            promotion.ProductModel.Clear();
                        }
                    }

                    //Image
                    if (ImageUrl != null)
                    {
                        promotion.ImageUrl = Upload(ImageUrl, "CustomerPromotion");
                    }
                    _context.Entry(promotion).State = EntityState.Modified;
                    _context.SaveChanges();
                }

                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.MasterData_Promotion.ToLower())
                });
            });
        }
        #endregion Edit

        //GET: /CustomerPromotion/Delete
        #region Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ISDAuthorizationAttribute]
        public ActionResult Delete(Guid id)
        {
            return ExecuteDelete(() =>
            {
                var promotion = _context.CustomerPromotionModel.FirstOrDefault(p => p.PromotionId == id);
                if (promotion != null)
                {
                    if (promotion.ProductModel != null)
                    {
                        promotion.ProductModel.Clear();
                    }
                    _context.Entry(promotion).State = EntityState.Deleted;
                    _context.SaveChanges();

                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.Created,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.MasterData_Promotion.ToLower())
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = string.Format(LanguageResource.Alert_NotExist_Delete, LanguageResource.MasterData_Promotion.ToLower())
                    });
                }
            });
        }
        #endregion

        #region Detail Partial
        public ActionResult _ProductPromotion(Guid? id, int mode)
        {
            ViewBag.Mode = mode;
            var productAllList = _context.ProductModel.Where(p => p.Actived == true)
                                             .Select(p =>
                                             new
                                             {
                                                 Id = p.ProductId,
                                                 Name = p.ERPProductCode + " | " + p.ProductName
                                             }).ToList();
            ViewBag.ProductId = new SelectList(productAllList, "Id", "Name");
            //Edit
            if (mode != 1)
            {
                List<PromotionViewModel> productList = new List<PromotionViewModel>();
                if (id != null)
                {
                    productList = (from p in _context.CustomerPromotionModel
                                   from m in p.ProductModel
                                   where p.PromotionId == id
                                   select new PromotionViewModel()
                                   {
                                       PromotionId = p.PromotionId,
                                       ProductId = m.ProductId,
                                       ProductName = m.ProductName,
                                       ERPProductCode = m.ERPProductCode,
                                       PromotionCode = p.PromotionCode,
                                       PromotionName = p.PromotionName
                                   }).ToList();
                }
                return PartialView(productList);
            }
            //Create
            else
            {
                return PartialView();
            }
        }

        //list product to show on table
        public ActionResult _ProductPromotionInner(List<PromotionViewModel> productList = null)
        {
            if (productList == null)
            {
                productList = new List<PromotionViewModel>();
            }
            return PartialView(productList);
        }

        //delete row detail
        public ActionResult DeleteProduct(List<PromotionViewModel> productList, int STT)
        {
            var List = productList.Where(p => p.STT != STT).ToList();
            return PartialView("_ProductPromotionInner", List);
        }

        //insert row detail
        public ActionResult InsertProduct(List<PromotionViewModel> productList, Guid ProductId, string PromotionCode, string PromotionName)
        {
            if (productList == null)
            {
                productList = new List<PromotionViewModel>();
            }
            PromotionViewModel viewModel = new PromotionViewModel();
            viewModel.ProductId = ProductId;
            viewModel.PromotionCode = PromotionCode;
            viewModel.PromotionName = PromotionName;

            //if product exist in list not insert
            var ProductIdList = productList.Select(p => p.ProductId).ToList();
            if (!ProductIdList.Contains(ProductId))
            {
                //product
                var product = _context.ProductModel.FirstOrDefault(p => p.ProductId == ProductId);
                viewModel.ERPProductCode = product.ERPProductCode;
                viewModel.ProductName = product.ProductName;
                productList.Add(viewModel);
            }
            return PartialView("_ProductPromotionInner", productList);
        }
        #endregion Detail Partial

        #region Helper
        private void ManyToMany(CustomerPromotionModel model, List<Guid> ProductId)
        {
            if (model.ProductModel != null)
            {
                model.ProductModel.Clear();
            }
            foreach (var item in ProductId)
            {
                var product = _context.ProductModel.Find(item);
                if (product != null)
                {
                    model.ProductModel.Add(product);
                }
            }
        }

        public void CreateViewBag()
        {
            var missionList = _context.CatalogModel.Where(p => p.CatalogTypeCode == ConstCatalogType.Mission).OrderBy(p => p.OrderIndex).ToList();
            ViewBag.CatalogCode = new SelectList(missionList, "CatalogCode", "CatalogText_vi");
        }

        private void PushNotification(CustomerPromotionModel model)
        {
            string detail = string.Empty;
            

            var request = WebRequest.Create(ConstPushNotification.PushNotificationUrl) as HttpWebRequest;

            request.KeepAlive = true;
            request.Method = "POST";
            request.ContentType = "application/json; charset=utf-8";

            request.Headers.Add("authorization", "Basic " + ConstPushNotification.RESTAPIKey);

            var serializer = new JavaScriptSerializer();
            var obj = new
            {
                app_id = ConstPushNotification.AppId,
                // OneSignal uses English as the default language so the field must be filled. 
                // However if you only want to send your message in one language you can place it under "en"
                headings = new { en = LanguageResource.PushNotification_CustomerPromotion },
                contents = new { en = model.PromotionName },
                included_segments = new string[] { "All" },
            };
            var param = serializer.Serialize(obj);
            byte[] byteArray = Encoding.UTF8.GetBytes(param);

            string responseContent = null;
            string message = null;

            try
            {
                using (var writer = request.GetRequestStream())
                {
                    writer.Write(byteArray, 0, byteArray.Length);
                }

                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        responseContent = reader.ReadToEnd();
                    }
                }
            }
            catch (WebException ex)
            {
                message = ex.Message;
            }
        }
        #endregion

        #region Remote Validation
        private bool IsExists(string PromotionCode)
        {
            return (_context.CustomerPromotionModel.FirstOrDefault(p => p.PromotionCode == PromotionCode) != null);
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult CheckExistingPromotionCode(string PromotionCode, string PromotionCodeValid)
        {
            try
            {
                if (PromotionCodeValid != PromotionCode)
                {
                    return Json(!IsExists(PromotionCode));
                }
                else
                {
                    return Json(true);
                }
            }
            catch //(Exception ex)
            {
                return Json(false);
            }
        }
        #endregion
    }
}