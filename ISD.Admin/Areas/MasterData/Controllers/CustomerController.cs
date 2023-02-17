using ISD.EntityModels;
using ISD.Extensions;
using ISD.Resources;
using ISD.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MasterData.Controllers
{
    public class CustomerController : BaseController
    {
        // GET: Customer
        #region Index
        [ISDAuthorizationAttribute]
        public ActionResult Index()
        {
            CreateViewBag();
            return View();
        }

        public ActionResult _Search(string CustomerName = "", Guid? CustomerLevelId = null, string Phone = "", string CustomerLoyaltyCard = "")
        {
            return ExecuteSearch(() =>
            {
                var customerList = (from p in _context.CustomerModel
                                    where
                                    //search by CustomerName
                                    (CustomerName == "" || (p.LastName.Contains(CustomerName) ||
                                                            p.MiddleName.Contains(CustomerName) ||
                                                            p.FirstName.Contains(CustomerName)))
                                    //search by Phone
                                    && (Phone == "" || p.Phone.Contains(Phone))
                                    select new CustomerViewModel()
                                    {
                                        CustomerId = p.CustomerId,
                                        CustomerCode = p.CustomerCode,
                                        FullName = p.LastName + " " + p.MiddleName + " " + p.FirstName,
                                        Phone = p.Phone
                                    }).ToList();

                return PartialView(customerList);
            });
        }
        #endregion

        //GET: /Customer/Create
        #region Create
        [ISDAuthorizationAttribute]
        public ActionResult Create()
        {
            CreateViewBag();
            return View();
        }
        //POST: Create
        [HttpPost]
        [ValidateAjax]
        [ISDAuthorizationAttribute]
        public JsonResult Create(CustomerModel model)
        {
            return ExecuteContainer(() =>
            {
                model.CustomerId = Guid.NewGuid();
                _context.Entry(model).State = EntityState.Added;
                _context.SaveChanges();

                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.MasterData_Customer.ToLower())
                });
            });
        }
        #endregion

        //GET: /Customer/View
        #region View
        [ISDAuthorizationAttribute]
        public ActionResult View(Guid id)
        {
            var customer = (from p in _context.CustomerModel
                            join pr in _context.ProvinceModel on p.ProvinceId equals pr.ProvinceId into pg
                            from province in pg.DefaultIfEmpty()
                            join d in _context.DistrictModel on p.DistrictId equals d.DistrictId into dg
                            from dis in dg.DefaultIfEmpty()
                            where p.CustomerId == id
                            select new CustomerViewModel()
                            {
                                CustomerId = p.CustomerId,
                                CustomerCode = p.CustomerCode,
                                FullName = (p.LastName + " " + p.MiddleName + " " + p.FirstName).Trim(),
                                LastName = p.LastName,
                                MiddleName = p.MiddleName,
                                FirstName = p.FirstName,
                                IdentityNumber = p.IdentityNumber,
                                Gender = p.Gender,
                                DateOfBirth = p.DateOfBirth,
                                CustomerAddress = p.CustomerAddress,
                                ProvinceId = p.ProvinceId,
                                ProvinceName = province.ProvinceName,
                                DistrictId = p.DistrictId,
                                DistrictName = dis.Appellation + " " + dis.DistrictName,
                                Phone = p.Phone,
                                EmailAddress = p.EmailAddress,
                                Fax = p.Fax
                            }).FirstOrDefault();
            if (customer == null)
            {
                return RedirectToAction("ErrorText", "Error", new { area = "", statusCode = 404, exception = string.Format(LanguageResource.Error_NotExist, LanguageResource.MasterData_Customer.ToLower()) });
            }
            CreateViewBag(customer.CustomerLevelId, customer.ProvinceId, customer.DistrictId);
            return View(customer);
        }
        #endregion View

        //GET: /Customer/Edit
        #region Edit
        [ISDAuthorizationAttribute]
        public ActionResult Edit(Guid id)
        {
            var customer = (from p in _context.CustomerModel
                            where p.CustomerId == id
                            select new CustomerViewModel()
                            {
                                CustomerId = p.CustomerId,
                                CustomerCode = p.CustomerCode,
                                LastName = p.LastName,
                                MiddleName = p.MiddleName,
                                FirstName = p.FirstName,
                                IdentityNumber = p.IdentityNumber,
                                Gender = p.Gender,
                                DateOfBirth = p.DateOfBirth,
                                CustomerAddress = p.CustomerAddress,
                                ProvinceId = p.ProvinceId,
                                DistrictId = p.DistrictId,
                                Phone = p.Phone,
                                EmailAddress = p.EmailAddress,
                                Fax = p.Fax
                            }).FirstOrDefault();
            if (customer == null)
            {
                return RedirectToAction("ErrorText", "Error", new { area = "", statusCode = 404, exception = string.Format(LanguageResource.Error_NotExist, LanguageResource.MasterData_Customer.ToLower()) });
            }
            CreateViewBag(customer.CustomerLevelId, customer.ProvinceId, customer.DistrictId);
            return View(customer);
        }
        //POST: Edit
        [HttpPost]
        [ISDAuthorizationAttribute]
        public JsonResult Edit(CustomerModel model)
        {
            return ExecuteContainer(() =>
            {
                _context.Entry(model).State = EntityState.Modified;
                _context.SaveChanges();

                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.MasterData_Customer.ToLower())
                });
            });
        }
        #endregion

        //GET: /Customer/Delete
        #region Delete
        [HttpPost]
        [ISDAuthorizationAttribute]
        public ActionResult Delete(Guid id)
        {
            return ExecuteDelete(() =>
            {
                var customer = _context.CustomerModel.FirstOrDefault(p => p.CustomerId == id);
                if (customer != null)
                {
                    _context.Entry(customer).State = EntityState.Deleted;
                    _context.SaveChanges();

                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.Created,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.MasterData_Customer.ToLower())
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = ""
                    });
                }
            });
        }
        #endregion

        #region Remote Validation
        private bool IsExists(string CustomerCode)
        {
            return (_context.CustomerModel.FirstOrDefault(p => p.CustomerCode == CustomerCode) != null);
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult CheckExistingCustomerCode(string CustomerCode, string CustomerCodeValid)
        {
            try
            {
                if (CustomerCodeValid != CustomerCode)
                {
                    return Json(!IsExists(CustomerCode));
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

        #region CreateViewBag, Helper
        public void CreateViewBag(Guid? CustomerLevelId = null, Guid? ProvinceId = null, Guid? DistrictId = null)
        {
            //CustomerLevel
            var customerLevelList = _context.CustomerLevelModel.OrderBy(p => p.OrderIndex).ToList();
            ViewBag.CustomerLevelId = new SelectList(customerLevelList, "CustomerLevelId", "CustomerLevelName", CustomerLevelId);

            //Province
            var provinceList = _context.ProvinceModel.OrderBy(p => p.ProvinceName).ToList();
            ViewBag.ProvinceId = new SelectList(provinceList, "ProvinceId", "ProvinceName", ProvinceId);

            //District
            if (ProvinceId != null)
            {
                var districtList = _context.DistrictModel.Where(p => p.ProvinceId == ProvinceId)
                                                         .Select(p => new
                                                         {
                                                             Id = p.DistrictId,
                                                             Name = p.Appellation + " " + p.DistrictName,
                                                             ShortName = p.DistrictName
                                                         })
                                                         .OrderByDescending(p => p.Name).ThenBy(p => p.ShortName).ToList();
                ViewBag.DistrictId = new SelectList(districtList, "Id", "Name", DistrictId);
            }
        }
        //GetDistrictByProvince
        public ActionResult GetDistrictByProvince(Guid? ProvinceId)
        {
            var districtList = _context.DistrictModel.Where(p => p.ProvinceId == ProvinceId)
                                                         .Select(p => new
                                                         {
                                                             Id = p.DistrictId,
                                                             Name = p.Appellation + " " + p.DistrictName,
                                                             ShortName = p.DistrictName
                                                         })
                                                         .OrderByDescending(p => p.Name).ThenBy(p => p.ShortName).ToList();

            var districtIdList = new SelectList(districtList, "Id", "Name");
            return Json(districtIdList, JsonRequestBehavior.AllowGet);
        }
        #endregion CreateViewBag, Helper
    }
}