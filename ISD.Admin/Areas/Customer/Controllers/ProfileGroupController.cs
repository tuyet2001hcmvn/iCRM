using ISD.Constant;
using ISD.EntityModels;
using ISD.Core;
using ISD.Resources;
using ISD.ViewModels;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Customer.Controllers
{
    public class ProfileGroupController : BaseController
    {
        // GET: ProfileGroup
        public ActionResult _List(Guid? id, bool? isLoadContent = false)
        {
            var profileGroupList = (from pg in _context.ProfileGroupModel
                                    //
                                    join ca in _context.CatalogModel on pg.ProfileGroupCode equals ca.CatalogCode
                                    //Create User
                                    join acc in _context.AccountModel on pg.CreateBy equals acc.AccountId
                                    //
                                    where pg.ProfileId == id
                                    orderby pg.CreateTime descending
                                    select new ProfileGroupViewModel
                                    {
                                        ProfileGroupId = pg.ProfileGroupId,
                                        ProfileId = pg.ProfileId,
                                        ProfileGroupCode = pg.ProfileGroupCode,
                                        ProfileGroupName = ca.CatalogText_vi,
                                        CreateUser = acc.UserName,
                                        CreateTime = pg.CreateTime
                                    }).ToList();
            if (isLoadContent == true)
            {
                return PartialView("_ListContent", profileGroupList);
            }
            return PartialView(profileGroupList);
        }

        public ActionResult _Create(Guid id)
        {
            var profileGroupVM = new ProfileGroupViewModel
            {
                ProfileId = id
            };
            CreateGroupListViewBag(id);
            return PartialView(profileGroupVM);
        }

        [HttpPost]
        public JsonResult Create(ProfileGroupViewModel model)
        {
            return ExecuteContainer(() =>
            {
                var profileGroupNew = new ProfileGroupModel
                {
                    ProfileGroupId = Guid.NewGuid(),
                    ProfileId = model.ProfileId,
                    ProfileGroupCode = model.ProfileGroupCode,
                    CreateBy = CurrentUser.AccountId,
                    CreateTime = DateTime.Now,
                };
                _context.Entry(profileGroupNew).State = EntityState.Added;
                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.Profile_ProfileGroup.ToLower())
                });
            });
        }

        #region Delete
        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            return ExecuteDelete(() =>
            {
                var profileInDb = _context.ProfileGroupModel.FirstOrDefault(p => p.ProfileGroupId == id);
                if (profileInDb != null)
                {
                    _context.Entry(profileInDb).State = EntityState.Deleted;
                    _context.SaveChanges();

                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.Created,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.Profile_ProfileGroup.ToLower())
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

        private void CreateGroupListViewBag(Guid ProfileId)
        {
            //Chỉ load Profile chưa có trong danh mục
            //Danh sách group mà profile Đã có
            var CurrentProfileGroup = (from g in _context.ProfileGroupModel
                                       where g.ProfileId == ProfileId
                                       select g.ProfileGroupCode).ToList();
            var ProfileGroupList = (from cat in _context.CatalogModel
                                    where cat.CatalogTypeCode == ConstCatalogType.ProfileGroup &&
                                    !CurrentProfileGroup.Contains(cat.CatalogCode)
                                    select new
                                    {
                                        cat.CatalogCode,
                                        cat.CatalogText_vi
                                    }).ToList();
            //IsHasData: 
            //  + null: Chưa thiết lập dữ liệu master data
            //  + true: Có dữ liệu hiển thị dropdownlist
            //  + false: Đã thuộc tất cả các nhóm
            bool? IsHasData = null;
            if (ProfileGroupList != null && ProfileGroupList.Count > 0)
            {
                //Có dữ liệu
                IsHasData = true;
            }
            else if (CurrentProfileGroup != null && CurrentProfileGroup.Count > 0)
            {
                //Dữ liệu mapping có dư liệu => đã thiết lập master data
                IsHasData = false;
            }
            else
            {
                IsHasData = null;
            }
            ViewBag.IsHasData = IsHasData;
            ViewBag.ProfileGroupCode = new SelectList(ProfileGroupList, "CatalogCode", "CatalogText_vi");
        }
    }
}