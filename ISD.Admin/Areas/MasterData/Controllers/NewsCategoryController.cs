using ISD.EntityModels;
using ISD.Extensions;
using ISD.Resources;
using ISD.Core;
using ISD.ViewModels.MasterData;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MasterData.Controllers
{
    public class NewsCategoryController : BaseController
    {
        // GET: NewsCategory
        [ISDAuthorizationAttribute]
        public ActionResult Index()
        {
            CreateViewBag();
            return View();
        }
        public ActionResult _Search(bool? Actived = null)
        {
            return ExecuteSearch(() =>
            {
                var news = (from p in _context.NewsCategoryModel
                            join acc in _context.AccountModel on p.CreateBy equals acc.AccountId
                            orderby p.OrderIndex descending
                            where
                            //Search by Actived
                            (Actived == null || p.Actived == Actived)
                            select new NewsCategoryViewModel()
                            {
                                NewsCategoryId = p.NewsCategoryId,
                                NewsCategoryCode = p.NewsCategoryCode,
                                NewsCategoryName = p.NewsCategoryName,
                                Description = p.Description,
                                OrderIndex = p.OrderIndex,
                                CreateBy = p.CreateBy,
                                CreateByName = acc.UserName,
                                CreateTime = p.CreateTime,
                                ImageUrl = p.ImageUrl,
                                Actived = p.Actived
                            }).ToList();
                return PartialView(news);
            });
        }

        //GET: /NewsCategory/Create
        #region Create
        [ISDAuthorizationAttribute]
        public ActionResult Create()
        {
            CreateViewBag();
            return View();
        }
        //POST: Create
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAjax]
        [ISDAuthorizationAttribute]
        public JsonResult Create(NewsCategoryModel model, HttpPostedFileBase FileUpload)
        {
            return ExecuteContainer(() =>
            {
                if (FileUpload != null)
                {
                    model.ImageUrl = Upload(FileUpload, "NewsCategory");
                }
                else
                {
                    model.ImageUrl = "noimage.jpg";
                }
                model.NewsCategoryId = Guid.NewGuid();
                model.CreateBy = CurrentUser.AccountId;
                model.CreateTime = DateTime.Now;
                _context.Entry(model).State = EntityState.Added;
                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.MasterData_NewsCategory.ToLower())
                });
            });
        }
        #endregion

        //GET: /NewsCategory/Edit
        #region Edit
        [ISDAuthorizationAttribute]
        public ActionResult Edit(Guid id)
        {
            var news = _context.NewsCategoryModel.FirstOrDefault(p => p.NewsCategoryId == id);
            if (news == null)
            {
                return RedirectToAction("ErrorText", "Error", new { area = "", statusCode = 404, exception = string.Format(LanguageResource.Error_NotExist, LanguageResource.MasterData_Company.ToLower()) });
            }
            return View(news);
        }
        //POST: Edit
        [HttpPost]
        [ValidateInput(false)]
        [ISDAuthorizationAttribute]
        public JsonResult Edit(NewsCategoryModel model, HttpPostedFileBase FileUpload)
        {
            return ExecuteContainer(() =>
            {
                var news = _context.NewsCategoryModel.FirstOrDefault(p => p.NewsCategoryId == model.NewsCategoryId);
                if (FileUpload != null)
                {
                    news.ImageUrl = Upload(FileUpload, "NewsCategory");
                }
                news.NewsCategoryName = model.NewsCategoryName;
                news.Description = model.Description;
                news.OrderIndex = model.OrderIndex;
                news.Actived = model.Actived;
                news.LastEditBy = CurrentUser.AccountId;
                news.LastEditTime = DateTime.Now;
                //Lưu danh mục bảng tin
                _context.Entry(news).State = EntityState.Modified;
                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.MasterData_News.ToLower())
                });
            });
        }
        #endregion

        //GET: /NewsCategory/Delete
        #region Delete
        [HttpPost]
        [ISDAuthorizationAttribute]
        public ActionResult Delete(Guid id)
        {
            return ExecuteDelete(() =>
            {
                var news = _context.NewsCategoryModel.FirstOrDefault(p => p.NewsCategoryId == id);
                if (news != null)
                {
                    _context.Entry(news).State = EntityState.Deleted;
                    _context.SaveChanges();

                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.Created,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.MasterData_NewsCategory.ToLower())
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

        #region CreateViewBag, Helper
        public void CreateViewBag(string CatalogTypeCode = null)
        {
            //Get list CatalogType
            var catalogTypeList = _context.CatalogTypeModel.Where(p => p.Actived == true)
                                                           .OrderBy(p => p.CatalogTypeName).ToList();

            ViewBag.CatalogTypeCode = new SelectList(catalogTypeList, "CatalogTypeCode", "CatalogTypeName", CatalogTypeCode);
        }
        #endregion

        public string getFileName(HttpPostedFileBase file)
        {
            var fileName = Path.GetFileName(file.FileName);

            //Create dynamically folder to save file
            var existPath = Server.MapPath("~/Upload/NewsCategory");
            Directory.CreateDirectory(existPath);
            var path = Path.Combine(existPath, fileName);

            file.SaveAs(path);

            return fileName;
        }
    }
}