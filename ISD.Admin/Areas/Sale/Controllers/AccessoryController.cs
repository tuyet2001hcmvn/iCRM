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

namespace Sale.Controllers
{
    public class AccessoryController : BaseController
    {
        public const string MBH = "bddec2cb-9c3e-4cb9-9824-587479caa492";   

        // GET: Accessory
        #region Index
        [ISDAuthorizationAttribute]
        public ActionResult Index()
        {
            //Get list AccessoryCategory
            var accCategoryList = _context.AccessoryCategoryModel.Where(p => p.Actived == true)
                                                                 .OrderBy(p => p.OrderIndex).ToList();
            ViewBag.AccessoryCategoryId = new SelectList(accCategoryList, "AccessoryCategoryId", "AccessoryCategoryName");

            return View();
        }

        public ActionResult _Search(Guid? AccessoryCategoryId = null, string AccessoryName = "", bool? Actived = null)
        {
            return ExecuteSearch(() =>
            {
                var accessory = (from p in _context.AccessoryModel.AsEnumerable()
                                 //join to get AccessoryCategory
                                 join br in _context.AccessoryCategoryModel on p.AccessoryCategoryId equals br.AccessoryCategoryId
                                 orderby p.AccessoryCategoryId, p.OrderIndex
                                 where
                                 //search by AccessoryCategoryId
                                 (AccessoryCategoryId == null || p.AccessoryCategoryId == AccessoryCategoryId)
                                 //search by AccessoryName
                                 && (AccessoryName == "" || p.AccessoryName.Contains(AccessoryName))
                                 //search by Actived
                                 && (Actived == null || p.Actived == Actived)
                                 select new AccessoryViewModel()
                                 {
                                     AccessoryId = p.AccessoryId,
                                     AccessoryCode = p.AccessoryCode,
                                     AccessoryName = p.AccessoryName,
                                     AccessoryCategoryId = p.AccessoryCategoryId,
                                     AccessoryCategoryName = br.AccessoryCategoryName,
                                     OrderIndex = p.OrderIndex,
                                     ImageUrl = p.ImageUrl,
                                     //NumberOfImage = p.AccessoryDetailModel.Where(p1 => p1.AccessoryId == p.AccessoryId).Count(),
                                     Actived = p.Actived
                                 })
                                 .ToList();

                return PartialView(accessory);
            });
        }
        #endregion

        //GET: /Accessory/Create
        #region Create
        [ISDAuthorizationAttribute]
        public ActionResult Create()
        {
            //Get list AccessoryCategory
            var accCategoryList = _context.AccessoryCategoryModel.Where(p => p.Actived == true)
                                                                 .OrderBy(p => p.OrderIndex).ToList();
            ViewBag.AccessoryCategoryId = new SelectList(accCategoryList, "AccessoryCategoryId", "AccessoryCategoryName");

            AccessoryViewModel viewModel = new AccessoryViewModel();
            return View(viewModel);
        }
        //POST: Create
        [HttpPost]
        [ValidateAjax]
        [ISDAuthorizationAttribute]
        public JsonResult Create(AccessoryModel model, HttpPostedFileBase ImageUrl)
        {
            return ExecuteContainer(() =>
            {
                model.AccessoryId = Guid.NewGuid();
                if (model.AccessoryCategoryId == new Guid(MBH))
                {
                    model.isHelmet = true;
                }
                //save image
                if (ImageUrl != null)
                {
                    model.ImageUrl = Upload(ImageUrl, "Accessory");
                }
                _context.Entry(model).State = EntityState.Added;
                _context.SaveChanges();

                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.Sale_Accessory.ToLower())
                });
            });
        }
        #endregion

        //GET: /Accessory/Edit
        #region Edit
        [ISDAuthorizationAttribute]
        public ActionResult Edit(Guid id)
        {
            var accessory = (from p in _context.AccessoryModel
                             //join to get AccessoryCategory
                             join br in _context.AccessoryCategoryModel on p.AccessoryCategoryId equals br.AccessoryCategoryId
                             orderby p.AccessoryCategoryId, p.OrderIndex
                             where p.AccessoryId == id
                             select new AccessoryViewModel()
                             {
                                 AccessoryId = p.AccessoryId,
                                 AccessoryCode = p.AccessoryCode,
                                 AccessoryName = p.AccessoryName,
                                 AccessoryCategoryId = p.AccessoryCategoryId,
                                 AccessoryCategoryName = br.AccessoryCategoryName,
                                 OrderIndex = p.OrderIndex,
                                 isHelmet = p.isHelmet,
                                 isHelmetAdult = p.isHelmetAdult,
                                 ImageUrl = p.ImageUrl,
                                 Size = p.Size,
                                 Actived = p.Actived
                             })
                             .FirstOrDefault();
            if (accessory == null)
            {
                return RedirectToAction("ErrorText", "Error", new { area = "", statusCode = 404, exception = string.Format(LanguageResource.Error_NotExist, LanguageResource.Sale_Accessory.ToLower()) });
            }
            return View(accessory);
        }
        //POST: Edit
        [HttpPost]
        [ISDAuthorizationAttribute]
        public JsonResult Edit(AccessoryModel model, HttpPostedFileBase ImageUrl)
        {
            return ExecuteContainer(() =>
            {
                if (model.AccessoryCategoryId == new Guid(MBH))
                {
                    model.isHelmet = true;
                }
                else
                {
                    model.isHelmet = false;
                }
                //save image
                if (ImageUrl != null)
                {
                    model.ImageUrl = Upload(ImageUrl, "Accessory");
                }
                _context.Entry(model).State = EntityState.Modified;
                _context.SaveChanges();

                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.Sale_Accessory.ToLower())
                });
            });
        }
        #endregion

        //GET: /Accessory/Delete
        #region Delete
        [HttpPost]
        [ISDAuthorizationAttribute]
        public ActionResult Delete(Guid id)
        {
            return ExecuteDelete(() =>
            {
                var accessory = _context.AccessoryModel.FirstOrDefault(p => p.AccessoryId == id);
                if (accessory != null)
                {
                    //delete price
                    var deletepriceList = _context.AccessoryProductModel.Where(p => p.AccessoryId == id).ToList();
                    _context.AccessoryProductModel.RemoveRange(deletepriceList);
                    _context.SaveChanges();

                    //delete image
                    var deleteImageList = _context.AccessoryDetailModel.Where(p => p.AccessoryId == id).ToList();
                    _context.AccessoryDetailModel.RemoveRange(deleteImageList);
                    _context.SaveChanges();

                    _context.Entry(accessory).State = EntityState.Deleted;
                    _context.SaveChanges();

                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.Created,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.Sale_Accessory.ToLower())
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

        #region Detail Partial Color
        public ActionResult _AccessoryColor(Guid? id, int mode)
        {
            ViewBag.Mode = mode;
            //Get list Color
            var colorList = _context.ColorModel.Where(p => p.Actived == true)
                                                  .Select(p =>
                                                  new
                                                  {
                                                      Id = p.ColorCode,
                                                      Name = p.ColorName,
                                                      OrderIndex = p.OrderIndex
                                                  }).OrderBy(p => p.OrderIndex).ToList();
            ViewBag.HelmetColorCode = new SelectList(colorList, "Id", "Name");
            //Edit
            if (mode != 1)
            {
                List<AccessoryViewModel> imageList = new List<AccessoryViewModel>();
                if (id != null)
                {
                    imageList = (from p in _context.AccessoryDetailModel
                                 join m in _context.AccessoryModel on p.AccessoryId equals m.AccessoryId
                                 join c in _context.ColorModel on p.HelmetColorId equals c.ColorId into cs
                                 from c1 in cs.DefaultIfEmpty()
                                 where p.AccessoryId == id
                                 select new AccessoryViewModel()
                                 {
                                     AccessoryId = p.AccessoryId,
                                     AccessoryCode = m.AccessoryCode,
                                     AccessoryName = m.AccessoryName,
                                     HelmetColorId = p.HelmetColorId,
                                     HelmetColorCode = c1.ColorCode,
                                     HelmetColorName = c1.ColorName,
                                     ImageUrl = p.ImageUrl
                                 }).ToList();
                }
                return PartialView(imageList);
            }
            //Create
            else
            {
                return PartialView();
            }
        }

        //list product to show on table
        public ActionResult _AccessoryColorInner(List<AccessoryViewModel> imageList = null)
        {
            if (imageList == null)
            {
                imageList = new List<AccessoryViewModel>();
            }
            return PartialView(imageList);
        }

        //delete row detail
        public ActionResult DeleteAccessoryColor(List<AccessoryViewModel> imageList, int STT)
        {
            var List = imageList.Where(p => p.STT != STT).ToList();
            return PartialView("_AccessoryColorInner", List);
        }

        //insert row detail
        public ActionResult InsertAccessoryColor(List<AccessoryViewModel> imageList, string HelmetColorCode, HttpPostedFileBase ImageUrl, string AccessoryCode, string AccessoryName)
        {
            if (imageList == null)
            {
                imageList = new List<AccessoryViewModel>();
            }
            AccessoryViewModel viewModel = new AccessoryViewModel();
            //HelmetColor
            if (!String.IsNullOrEmpty(HelmetColorCode))
            {
                viewModel.HelmetColorCode = HelmetColorCode;
                var color = _context.ColorModel.FirstOrDefault(p => p.ColorCode == HelmetColorCode);
                viewModel.HelmetColorId = color.ColorId;
                viewModel.HelmetColorName = color.ColorName;
            }
            //Image
            viewModel.ImageUrl = Upload(ImageUrl, "Accessory");

            viewModel.AccessoryCode = AccessoryCode;
            viewModel.AccessoryName = AccessoryName;

            imageList.Add(viewModel);
            return PartialView("_AccessoryColorInner", imageList);
        }
        #endregion Detail Partial Color

        #region Remote Validation
        private bool IsExists(string AccessoryCode)
        {
            return (_context.AccessoryModel.FirstOrDefault(p => p.AccessoryCode == AccessoryCode) != null);
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult CheckExistingAccessoryCode(string AccessoryCode, string AccessoryCodeValid)
        {
            try
            {
                if (AccessoryCodeValid != AccessoryCode)
                {
                    return Json(!IsExists(AccessoryCode));
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