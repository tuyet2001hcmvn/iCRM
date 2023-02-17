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

namespace MasterData.Controllers
{
    public class CustomerGiftController : BaseController
    {
        public const string LevelCode = "THUONG";

        // GET: CustomerGift
        #region Index
        [ISDAuthorizationAttribute]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult _Search(string GiftName = "", DateTime? EffectFromDate = null, DateTime? EffectToDate = null)
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
                var giftList = (from p in _context.CustomerGiftModel
                                orderby p.EffectToDate descending
                                where
                                //search by GiftName
                                (GiftName == "" || p.GiftName.Contains(GiftName))
                                //search by EffectDate
                                && ((EffectFromDate <= p.EffectFromDate && p.EffectFromDate <= EffectToDate)
                                || (EffectFromDate <= p.EffectToDate && p.EffectToDate <= EffectToDate))
                                select p).ToList();

                return PartialView(giftList);
            });
        }
        #endregion

        //GET: /CustomerGift/Create
        #region Create
        [ISDAuthorizationAttribute]
        public ActionResult Create()
        {
            var customerLevelList = _context.CustomerLevelModel.Where(p => p.CustomerLevelCode != LevelCode)
                                                               .OrderBy(p => p.OrderIndex).ToList();
            ViewBag.SearchCustomerLevelId = new SelectList(customerLevelList, "CustomerLevelId", "CustomerLevelName");

            return View();
        }
        //POST: Create
        [HttpPost]
        [ValidateAjax]
        [ValidateInput(false)] //need when using ckeditor, do not delete
        [ISDAuthorizationAttribute]
        public JsonResult Create(CustomerGiftModel model, HttpPostedFileBase ImageUrl, List<Detail_CustomerGiftViewModel> chosenList)
        {
            return ExecuteContainer(() =>
            {
                model.GiftId = Guid.NewGuid();
                if (chosenList != null)
                {
                    var CustomerId = chosenList.Select(p => p.CustomerId).ToList();
                    ManyToMany(model, CustomerId);

                    if (ImageUrl != null)
                    {
                        model.ImageUrl = Upload(ImageUrl, "Gift");
                    }
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = LanguageResource.CustomerGift_ErrorNotExist
                    });
                }
                _context.Entry(model).State = EntityState.Added;
                _context.SaveChanges();

                //Push Notification
                PushNotification(model);

                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.MasterData_CustomerGift.ToLower())
                });
            });
        }
        #endregion

        //GET: /CustomerGift/View
        #region View
        [ISDAuthorizationAttribute]
        public ActionResult View(Guid id)
        {
            var gift = (from p in _context.CustomerGiftModel.AsEnumerable()
                        where p.GiftId == id
                        select new CustomerGiftViewModel()
                        {
                            GiftId = p.GiftId,
                            GiftCode = p.GiftCode,
                            GiftName = p.GiftName,
                            EffectFromDate = p.EffectFromDate,
                            EffectToDate = p.EffectToDate,
                            Description = p.Description,
                            ImageUrl = p.ImageUrl,
                            Notes = p.Notes
                        }).FirstOrDefault();

            if (gift == null)
            {
                return RedirectToAction("ErrorText", "Error", new { area = "", statusCode = 404, exception = string.Format(LanguageResource.Error_NotExist, LanguageResource.MasterData_CustomerGift.ToLower()) });
            }

            var chosenList = (from g in _context.CustomerGiftModel
                              join detail in _context.CustomerGiftDetailModel on gift.GiftId equals detail.GiftId
                              join p in _context.CustomerModel on detail.CustomerId equals p.CustomerId
                              where g.GiftId == id
                              select new Detail_CustomerGiftViewModel()
                              {
                                  CustomerId = p.CustomerId,
                                  CustomerCode = p.CustomerCode,
                                  FullName = p.LastName + " " + p.MiddleName + " " + p.FirstName,
                                  Phone = p.Phone,
                              }).ToList();

            ViewBag.chosenList = chosenList;
            return View(gift);
        }
        #endregion View

        //GET: /CustomerGift/Edit
        #region Edit
        [ISDAuthorizationAttribute]
        public ActionResult Edit(Guid id)
        {
            var gift = (from p in _context.CustomerGiftModel
                        where p.GiftId == id
                        select new CustomerGiftViewModel()
                        {
                            GiftId = p.GiftId,
                            GiftCode = p.GiftCode,
                            GiftName = p.GiftName,
                            EffectFromDate = p.EffectFromDate,
                            EffectToDate = p.EffectToDate,
                            Description = p.Description,
                            ImageUrl = p.ImageUrl,
                            Notes = p.Notes
                        }).FirstOrDefault();

            if (gift == null)
            {
                return RedirectToAction("ErrorText", "Error", new { area = "", statusCode = 404, exception = string.Format(LanguageResource.Error_NotExist, LanguageResource.MasterData_CustomerGift.ToLower()) });
            }

            var chosenList = (from g in _context.CustomerGiftModel
                              join detail in _context.CustomerGiftDetailModel on gift.GiftId equals detail.GiftId
                              join p in _context.CustomerModel on detail.CustomerId equals p.CustomerId
                              where g.GiftId == id
                              select new Detail_CustomerGiftViewModel()
                              {
                                  CustomerId = p.CustomerId,
                                  CustomerCode = p.CustomerCode,
                                  FullName = p.LastName + " " + p.MiddleName + " " + p.FirstName,
                                  Phone = p.Phone,
                              }).ToList();

            ViewBag.chosenList = chosenList;
            var customerLevelList = _context.CustomerLevelModel.Where(p => p.CustomerLevelCode != LevelCode)
                                                               .OrderBy(p => p.OrderIndex).ToList();
            ViewBag.SearchCustomerLevelId = new SelectList(customerLevelList, "CustomerLevelId", "CustomerLevelName");

            return View(gift);
        }
        [HttpPost]
        [ValidateAjax]
        [ValidateInput(false)] //need when using ckeditor, do not delete
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CustomerGiftModel model, HttpPostedFileBase ImageUrl, List<Detail_CustomerGiftViewModel> chosenList)
        {
            return ExecuteContainer(() =>
            {
                var gift = _context.CustomerGiftModel.FirstOrDefault(p => p.GiftId == model.GiftId);
                if (gift != null)
                {
                    gift.GiftCode = model.GiftCode;
                    gift.GiftName = model.GiftName;
                    gift.EffectFromDate = model.EffectFromDate;
                    gift.EffectToDate = model.EffectToDate;
                    gift.Description = model.Description;
                    gift.Notes = model.Notes;

                    if (chosenList != null)
                    {
                        var CustomerId = chosenList.Select(p => p.CustomerId).ToList();
                        ManyToMany(gift, CustomerId);

                        if (ImageUrl != null)
                        {
                            gift.ImageUrl = Upload(ImageUrl, "Gift");
                        }
                    }
                    else
                    {
                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.NotModified,
                            Success = false,
                            Data = LanguageResource.CustomerGift_ErrorNotExist
                        });
                    }
                    _context.Entry(gift).State = EntityState.Modified;
                    _context.SaveChanges();
                }

                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.MasterData_CustomerGift.ToLower())
                });
            });
        }
        #endregion Edit

        //GET: /CustomerGift/Delete
        #region Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ISDAuthorizationAttribute]
        public ActionResult Delete(Guid id)
        {
            return ExecuteDelete(() =>
            {
                var gift = _context.CustomerGiftModel.FirstOrDefault(p => p.GiftId == id);
                if (gift != null)
                {
                    _context.Entry(gift).State = EntityState.Deleted;
                    _context.SaveChanges();

                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.Created,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.MasterData_CustomerGift.ToLower())
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = string.Format(LanguageResource.Alert_NotExist_Delete, LanguageResource.MasterData_CustomerGift.ToLower())
                    });
                }
            });
        }
        #endregion

        #region Detail Partial
        public ActionResult _CustomerGift(List<Detail_CustomerGiftViewModel> chosenList)
        {
            if (chosenList != null)
            {
                return PartialView(chosenList);
            }
            return PartialView();
        }

        //list product to show on table
        public ActionResult _CustomerGiftInner(List<Detail_CustomerGiftViewModel> chosenList = null)
        {
            if (chosenList == null)
            {
                chosenList = new List<Detail_CustomerGiftViewModel>();
            }
            return PartialView(chosenList);
        }
        //Popup Search Customer
        public ActionResult _CustomerSearch()
        {
            CustomerSearchViewModel model = new CustomerSearchViewModel();
            var customerLevelList = _context.CustomerLevelModel.Where(p => p.CustomerLevelCode != LevelCode)
                                                               .OrderBy(p => p.OrderIndex).ToList();
            ViewBag.SearchCustomerLevelId = new SelectList(customerLevelList, "CustomerLevelId", "CustomerLevelName");
            return PartialView(model);
        }
        //Popup Search Customer Result
        public ActionResult _CustomerSearchResult(CustomerSearchViewModel model, List<Guid> chosenList)
        {
            return ExecuteSearch(() =>
            {
                var customerList = (from p in _context.CustomerModel
                                    where
                                    //search by SearchFullName
                                    (string.IsNullOrEmpty(model.SearchFullName) || (p.LastName + " " + p.MiddleName + " " + p.FirstName).Contains(model.SearchFullName))
                                    //search by SearchPhone
                                    && (string.IsNullOrEmpty(model.SearchPhone) || p.Phone.Contains(model.SearchPhone))
                                    select new Detail_CustomerGiftViewModel()
                                    {
                                        CustomerId = p.CustomerId,
                                        CustomerCode = p.CustomerCode,
                                        FullName = p.LastName + " " + p.MiddleName + " " + p.FirstName,
                                        Phone = p.Phone,
                                    }).ToList();

                //select list search without products in productTable (product detail table)
                if (chosenList != null)
                {
                    customerList = customerList.Where(p => !chosenList.Contains(p.CustomerId)).ToList();
                }

                return PartialView(customerList);
            });
        }
        //insert row detail
        public ActionResult InsertCustomer(List<Guid> customerList, List<Guid> existList)
        {
            List<Detail_CustomerGiftViewModel> chosenList = new List<Detail_CustomerGiftViewModel>();
            if (existList != null && existList.Count > 0)
            {
                foreach (var item in existList)
                {
                    Detail_CustomerGiftViewModel model = new Detail_CustomerGiftViewModel();
                    var customer = (from p in _context.CustomerModel
                                    where p.CustomerId == item
                                    select new Detail_CustomerGiftViewModel()
                                    {
                                        CustomerId = p.CustomerId,
                                        CustomerCode = p.CustomerCode,
                                        FullName = p.LastName + " " + p.MiddleName + " " + p.FirstName,
                                        Phone = p.Phone,
                                    }).FirstOrDefault();

                    if (customer != null)
                    {
                        chosenList.Add(customer);
                    }
                }
            }
            if (customerList != null && customerList.Count > 0)
            {
                foreach (var item in customerList)
                {
                    Detail_CustomerGiftViewModel model = new Detail_CustomerGiftViewModel();
                    var customer = (from p in _context.CustomerModel
                                    where p.CustomerId == item
                                    select new Detail_CustomerGiftViewModel()
                                    {
                                        CustomerId = p.CustomerId,
                                        CustomerCode = p.CustomerCode,
                                        FullName = p.LastName + " " + p.MiddleName + " " + p.FirstName,
                                        Phone = p.Phone,
                                    }).FirstOrDefault();

                    if (customer != null)
                    {
                        chosenList.Add(customer);
                    }
                }
            }
            return PartialView("_CustomerGiftInner", chosenList);
        }
        //delete row detail
        public ActionResult DeleteCustomer(List<Guid> customerList, Guid DeleteCustomerId)
        {
            var List = customerList.Where(p => p != DeleteCustomerId).ToList();

            List<Detail_CustomerGiftViewModel> chosenList = new List<Detail_CustomerGiftViewModel>();
            foreach (var item in List)
            {
                Detail_CustomerGiftViewModel model = new Detail_CustomerGiftViewModel();
                var customer = (from p in _context.CustomerModel
                                where p.CustomerId == item
                                select new Detail_CustomerGiftViewModel()
                                {
                                    CustomerId = p.CustomerId,
                                    CustomerCode = p.CustomerCode,
                                    FullName = p.LastName + " " + p.MiddleName + " " + p.FirstName,
                                    Phone = p.Phone,
                                }).FirstOrDefault();

                if (customer != null)
                {
                    chosenList.Add(customer);
                }
            }
            return PartialView("_CustomerGiftInner", chosenList);
        }
        #endregion Detail Partial

        #region Helper
        private void ManyToMany(CustomerGiftModel model, List<Guid> CustomerId)
        {
            // Nhieu nhieu voi bang trung gian
            var detail = _context.CustomerGiftDetailModel.Where(p => p.GiftId == model.GiftId).ToList();
            if (detail != null && detail.Count > 0)
            {
                _context.CustomerGiftDetailModel.RemoveRange(detail);
            }
            foreach (var item in CustomerId)
            {
                CustomerGiftDetailModel refModel = new CustomerGiftDetailModel()
                {
                    GiftId = model.GiftId,
                    CustomerId = item,
                    isRead = false
                };
                _context.Entry(refModel).State = EntityState.Added;
            }

        }
        //GetCustomerByLevel
        //public ActionResult GetCustomerByLevel(Guid? CustomerLevelId)
        //{
        //    var customer = (from p in _context.CustomerModel
        //                    join level in _context.CustomerLevelModel on p.CustomerLevelId equals level.CustomerLevelId
        //                    where (CustomerLevelId == null || p.CustomerLevelId == CustomerLevelId)
        //                    && (level.CustomerLevelCode != LevelCode)
        //                    select new
        //                    {
        //                        Id = p.CustomerId,
        //                        Name = p.CustomerCode + " | " + p.CustomerLoyaltyCard + " | " +
        //                                                            p.LastName + " " + p.MiddleName + " " + p.FirstName
        //                    }).ToList();
        //    var customerList = new SelectList(customer, "Id", "Name");

        //    return Json(customerList, JsonRequestBehavior.AllowGet);
        //}

        private void PushNotification(CustomerGiftModel model)
        {
            //Gửi thông báo của chương trình quà tặng cho từng khách hàng
            List<string> playerIdList = new List<string>();
            var customerList = (from d in _context.CustomerGiftDetailModel
                                join cus in _context.CustomerModel on d.CustomerId equals cus.CustomerId
                                where d.GiftId == model.GiftId
                                select cus
                                ).ToList();
            if (customerList != null && customerList.Count > 0)
            {
                foreach (var customer in customerList)
                {
                    //var deviceId = _context.AccountModel.Where(p => p.AccountId == customer.AccountId).Select(p => p.DeviceId).FirstOrDefault();
                    //if (deviceId != null)
                    //{
                    //    playerIdList.Add(deviceId);
                    //}
                }
                //Có deviceId mới tiến hành gửi thông báo
                if (playerIdList != null && playerIdList.Count > 0)
                {
                    var playerIds = playerIdList.ToArray();

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
                        contents = new { en = model.GiftName },
                        data = new { openAction = ConstPushNotification.UuDaiCuaBan },
                        include_player_ids = playerIds
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

            }
        }
        #endregion

        #region Remote Validation
        private bool IsExists(string GiftCode)
        {
            return (_context.CustomerGiftModel.FirstOrDefault(p => p.GiftCode == GiftCode) != null);
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult CheckExistingGiftCode(string GiftCode, string GiftCodeValid)
        {
            try
            {
                if (GiftCodeValid != GiftCode)
                {
                    return Json(!IsExists(GiftCode));
                }
                else
                {
                    return Json(true);
                }
            }
            catch (Exception ex)
            {
                return Json(false);
            }
        }
        #endregion
    }
}