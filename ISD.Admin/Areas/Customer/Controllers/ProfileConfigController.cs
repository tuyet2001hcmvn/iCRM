using ISD.Core;
using ISD.EntityModels;
using ISD.Extensions;
using ISD.Resources;
using ISD.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Customer.Controllers
{
    public class ProfileConfigController : BaseController
    {
        // GET: ProfileConfig
        #region Index
        [ISDAuthorizationAttribute]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult _Search(string ProfileCategoryName, bool? Actived)
        {
            return ExecuteSearch(() =>
            {
                var lst = (from p in _context.ProfileCategoryModel
                           where (ProfileCategoryName == null || p.ProfileCategoryName.Contains(ProfileCategoryName))
                           && (Actived == null || p.Actived == Actived)
                           orderby p.ProfileCategoryCode
                           select p).ToList();
                return PartialView(lst);
            });
        }
        #endregion

        // GET: ProfileConfig/Create
        #region Create
        [ISDAuthorizationAttribute]
        public ActionResult Create()
        {
            CreateViewBag();
            return View();
        }
        [HttpPost]
        public ActionResult Create(ProfileCategoryModel model, List<ProfileConfigViewModel> configList)
        {
            return ExecuteContainer(() =>
            {
                //ProfileCategory
                model.CreateBy = CurrentUser.AccountId;
                model.CreateTime = DateTime.Now;
                _context.Entry(model).State = EntityState.Added;

                //Config list
                if (configList != null && configList.Count > 0)
                {
                    foreach (var item in configList)
                    {
                        if (!string.IsNullOrEmpty(item.FieldCode))
                        {
                            ProfileConfigModel configModel = new ProfileConfigModel();
                            configModel.ProfileCategoryCode = model.ProfileCategoryCode;
                            configModel.FieldCode = item.FieldCode;
                            configModel.OrderIndex = item.OrderIndex;
                            configModel.IsRequired = item.IsRequired;
                            configModel.Parameters = item.Parameters;
                            configModel.Note = item.Note;
                            _context.Entry(configModel).State = EntityState.Added;
                        }
                    }
                }
                _context.SaveChanges();

                return Json(new
                {
                    Code = HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.CustomerType.ToLower()),
                });
            });
        }
        #endregion

        // GET: ProfileConfig/Edit
        #region Edit
        [ISDAuthorizationAttribute]
        public ActionResult Edit(string id)
        {
            var category = (from p in _context.ProfileCategoryModel
                            where p.ProfileCategoryCode == id
                            select new ProfileCategoryViewModel
                            {
                                ProfileCategoryCode = p.ProfileCategoryCode,
                                ProfileCategoryName = p.ProfileCategoryName,
                                Note = p.Note,
                                OrderIndex = p.OrderIndex,
                                Actived = p.Actived,
                                CreateBy = p.CreateBy,
                                CreateTime = p.CreateTime,
                                LastEditBy = p.LastEditBy,
                                LastEditTime = p.LastEditTime
                            }).FirstOrDefault();
            CreateViewBag(category.ProfileCategoryCode);
            return View(category);
        }
        [HttpPost]
        public ActionResult Edit(ProfileCategoryViewModel viewModel, List<ProfileConfigViewModel> configList)
        {
            return ExecuteContainer(() =>
            {
                var existCategory = _context.ProfileCategoryModel
                                            .Where(p => p.ProfileCategoryCode == viewModel.ProfileCategoryCode).FirstOrDefault();
                if (existCategory != null)
                {
                    existCategory.ProfileCategoryName = viewModel.ProfileCategoryName;
                    existCategory.Note = viewModel.Note;
                    existCategory.OrderIndex = viewModel.OrderIndex;
                    existCategory.Actived = viewModel.Actived;
                    existCategory.LastEditBy = CurrentUser.AccountId;
                    existCategory.LastEditTime = DateTime.Now;

                    #region Update config
                    //Delete all config list
                    var exitConfigList = _context.ProfileConfigModel
                                                 .Where(p => p.ProfileCategoryCode == viewModel.ProfileCategoryCode).ToList();
                    _context.ProfileConfigModel.RemoveRange(exitConfigList);
                    //Add config list
                    if (configList != null && configList.Count > 0)
                    {
                        foreach (var item in configList)
                        {
                            if (!string.IsNullOrEmpty(item.FieldCode))
                            {
                                ProfileConfigModel configModel = new ProfileConfigModel();
                                configModel.ProfileCategoryCode = viewModel.ProfileCategoryCode;
                                configModel.FieldCode = item.FieldCode;
                                configModel.OrderIndex = item.OrderIndex;
                                configModel.IsRequired = item.IsRequired;
                                configModel.Parameters = item.Parameters;
                                configModel.Note = item.Note;
                                configModel.IsReadOnly = item.IsReadOnly;
                                _context.Entry(configModel).State = EntityState.Added;
                            }
                        }
                    }
                    #endregion
                }
                _context.SaveChanges();

                return Json(new
                {
                    Code = HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.CustomerType.ToLower()),
                });
            });
        }
        #endregion

        #region ViewBag
        private void CreateViewBag(string ProfileCategoryCode = null)
        {
            List<ProfileFieldViewModel> resultList = new List<ProfileFieldViewModel>();
            List<string> existFieldCodeList = new List<string>();
            if (ProfileCategoryCode != null)
            {
                var configList = (from p in _context.ProfileConfigModel
                                  join f in _context.ProfileFieldModel on p.FieldCode equals f.FieldCode
                                  where p.ProfileCategoryCode == ProfileCategoryCode
                                  select new ProfileFieldViewModel
                                  {
                                      FieldCode = p.FieldCode,
                                      FieldName = f.FieldName,
                                      OrderIndex = f.OrderIndex,
                                      Description = f.Description,
                                      OrderIndex_Config = p.OrderIndex,
                                      IsChoose = true,
                                      IsRequired = p.IsRequired,
                                      Note = p.Note,
                                      Parameters = p.Parameters,
                                      IsReadOnly = p.IsReadOnly,
                                  }).ToList();

                resultList.AddRange(configList);
                existFieldCodeList = configList.Select(p => p.FieldCode).ToList();
            }
            var fieldList = (from p in _context.ProfileFieldModel
                             where !existFieldCodeList.Contains(p.FieldCode)
                             orderby p.OrderIndex
                             select new ProfileFieldViewModel()
                             {
                                 FieldCode = p.FieldCode,
                                 FieldName = p.FieldName,
                                 OrderIndex = p.OrderIndex,
                                 Description = p.Description
                             }).ToList();
            resultList.AddRange(fieldList);

            ViewBag.ProfileField = resultList;
        }
        #endregion
    }
}