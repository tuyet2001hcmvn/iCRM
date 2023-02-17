using ISD.EntityModels;
using ISD.Extensions;
using ISD.Resources;
using ISD.Core;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ISD.Constant;
using ISD.ViewModels;
using ISD.Repositories;

namespace MasterData.Controllers
{
    public class NewsController : BaseController
    {
        // GET: News
        #region Index
        [ISDAuthorizationAttribute]
        public ActionResult Index(int? Type = null)
        {
            CreateViewBag(Type: Type);
            return View();
        }
        public ActionResult _Search(bool? Actived = null, int? Type = null)
        {
            return ExecuteSearch(() =>
            {
                var listNews = new List<Guid>();
                if (Type == ConstNewsCategoryCode.BangTin)
                {
                    listNews = (from  a in _context.NewsModel 
                                join c in _context.NewsCategoryModel on a.NewsCategoryId equals c.NewsCategoryId
                                where a.CreateBy == CurrentUser.AccountId
                                select a.NewsId).Distinct().ToList();
                }
                else
                {
                    listNews = (from a in _context.NewsModel
                                join c in _context.NewsCategoryModel on a.NewsCategoryId equals c.NewsCategoryId
                                where c.NewsCategoryCode != ConstNewsCategoryCode.BangTin
                                select a.NewsId).Distinct().ToList();
                }
                
                var news = (from p in _context.NewsModel
                            join acc in _context.AccountModel on p.CreateBy equals acc.AccountId
                            join c in _context.NewsCategoryModel on p.NewsCategoryId equals c.NewsCategoryId
                            join l in listNews on p.NewsId equals l
                            join sTemp in _context.CatalogModel on new { CatalogCode = p.Summary, CatalogTypeCode = ConstCatalogType.BANGTIN_SUMMARY } equals new { sTemp.CatalogCode, sTemp.CatalogTypeCode } into sList
                            from s in sList.DefaultIfEmpty()
                            join dTemp in _context.CatalogModel on new { CatalogCode = p.Detail, CatalogTypeCode = ConstCatalogType.BANGTIN_DETAIL } equals new { dTemp.CatalogCode, dTemp.CatalogTypeCode } into dList
                            from d in dList.DefaultIfEmpty()
                            join tTemp in _context.CatalogModel on new { CatalogCode = p.TypeNews, CatalogTypeCode = ConstCatalogType.News_Type } equals new { tTemp.CatalogCode, tTemp.CatalogTypeCode } into tList
                            from t in tList.DefaultIfEmpty()
                            orderby p.NewsCode descending
                            where
                            //Search by Actived
                            (Actived == null || p.Actived == Actived)
                            && (Type == null || c.NewsCategoryCode == Type)
                            select new NewsViewModel()
                            {
                                NewsId = p.NewsId,
                                NewsCategoryName = c.NewsCategoryName,
                                NewsCategoryCode = c.NewsCategoryCode,
                                Title = p.Title,
                                ScheduleTime = p.ScheduleTime,
                                ImageUrl = p.ImageUrl,
                                CreateTime = p.CreateTime,
                                Actived = p.Actived,
                                CreateByName = acc.UserName,
                                Description = p.Description,
                                SummaryName = s.CatalogText_vi,
                                DetailName = d.CatalogText_vi,
                                EndTime = p.EndTime,
                                TypeNews = t.CatalogText_vi,
                            }).ToList();
                CreateViewBag(Type: Type);
                return PartialView(news);
            });
        }
        public ActionResult _SearchRelatedNews(NewsViewModel viewModel)
        {
            return ExecuteSearch(() =>
            {
                var listNews = new List<Guid>();

                listNews = (from p in _context.TaskGroupDetailModel
                            join a in _context.NewsModel on p.GroupId equals a.GroupEmployeeId
                            join c in _context.NewsCategoryModel on a.NewsCategoryId equals c.NewsCategoryId
                            where p.AccountId == CurrentUser.AccountId || a.CreateBy == CurrentUser.AccountId
                            select a.NewsId).Distinct().ToList();
                var news = (from p in _context.NewsModel
                            join empg in _context.TaskGroupDetailModel on p.GroupEmployeeId equals empg.GroupId
                            join acc in _context.AccountModel on p.CreateBy equals acc.AccountId
                            join l in listNews on p.NewsId equals l
                            join c in _context.NewsCategoryModel on p.NewsCategoryId equals c.NewsCategoryId into cg
                            from c1 in cg.DefaultIfEmpty()
                            join sTemp in _context.CatalogModel on new { CatalogCode = p.Summary, CatalogTypeCode = ConstCatalogType.BANGTIN_SUMMARY } equals new { sTemp.CatalogCode, sTemp.CatalogTypeCode } into sList
                            from s in sList.DefaultIfEmpty()
                            join dTemp in _context.CatalogModel on new { CatalogCode = p.Detail, CatalogTypeCode = ConstCatalogType.BANGTIN_DETAIL } equals new { dTemp.CatalogCode, dTemp.CatalogTypeCode } into dList
                            from d in dList.DefaultIfEmpty()
                            join tTemp in _context.CatalogModel on new { CatalogCode = p.TypeNews, CatalogTypeCode = ConstCatalogType.News_Type } equals new { tTemp.CatalogCode, tTemp.CatalogTypeCode } into tList
                            from t in tList.DefaultIfEmpty()
                            orderby p.NewsCode descending
                            where p.Actived == true && p.NewsId != viewModel.NewsId
                            && (viewModel.Title == null || p.Title.Contains(viewModel.Title))
                            && (viewModel.NewsCategoryId == null || p.NewsCategoryId == viewModel.NewsCategoryId)
                            && (viewModel.Summary == null || p.Summary == viewModel.Summary)
                            && (viewModel.Detail == null || p.Detail == viewModel.Detail)
                            && (empg.AccountId == CurrentUser.AccountId || p.CreateBy == CurrentUser.AccountId)
                            select new NewsViewModel()
                            {
                                NewsId = p.NewsId,
                                NewsCategoryName = c1 != null ? c1.NewsCategoryName : null,
                                Title = p.Title,
                                ScheduleTime = p.ScheduleTime,
                                ImageUrl = p.ImageUrl,
                                CreateTime = p.CreateTime,
                                Actived = p.Actived,
                                CreateByName = acc.UserName,
                                Description = p.Description,
                                SummaryName = s.CatalogText_vi,
                                DetailName = d.CatalogText_vi,
                                TypeNews = t.CatalogText_vi
                            }).ToList();
                return PartialView(news);
            });
        }
        #endregion

        //GET: /News/Create
        #region Create
        [ISDAuthorizationAttribute]
        public ActionResult Create(int? Type)
        {
            GetRelatedNewsMapping(null);
            ViewBag.Company = _context.CompanyModel.OrderBy(p => p.OrderIndex).ToList();
            CreateViewBag(Type: Type);
            return View();
        }
        //POST: Create
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAjax]
        [ISDAuthorizationAttribute]
        public JsonResult Create(NewsViewModel model, HttpPostedFileBase FileUpload, List<ListCompanyViewModel> listCompany, List<Guid?> SelectNewsId)
        {
            return ExecuteContainer(() =>
            {
               
                if (model != null)
                {
                    if (model.NewsCategoryCode == ConstNewsCategoryCode.BangTin &&  model.EndTime == null)
                    {
                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.NotModified,
                            Success = false,
                            Data = string.Format("{0}", "Vui lòng nhập thông tin '"+LanguageResource.News_EndTime + "'!")
                        });
                    }
                    if (model.NewsCategoryCode == ConstNewsCategoryCode.BangTin && model.ScheduleTime == null)
                    {
                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.NotModified,
                            Success = false,
                            Data = string.Format("{0}", "Vui lòng nhập thông tin '"+LanguageResource.News_ScheduleTime + "'!")
                        });
                    }
                    if (model.NewsCategoryCode == ConstNewsCategoryCode.BangTin && model.GroupEmployeeId == null && model.GroupEmployeeId != Guid.Empty)
                    {
                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.NotModified,
                            Success = false,
                            Data = string.Format("{0}", "Vui lòng chọn '" + LanguageResource.News_GroupEmployee + "'!")
                        });
                    }
                    model.NewsCategoryId = _context.NewsCategoryModel.Where(x => x.NewsCategoryCode == model.NewsCategoryCode).Select(x=>x.NewsCategoryId).FirstOrDefault();
                }

                NewsModel result = new NewsModel();
                result.NewsId = Guid.NewGuid();
                result.CreateBy = CurrentUser.AccountId;
                result.CreateTime = DateTime.Now;
                if (FileUpload != null)
                {
                    result.ImageUrl = Upload(FileUpload, "News");
                }
                else
                {
                    result.ImageUrl = "noimage.jpg";
                }
                result.Title = model.Title;
                result.Description = model.Description;
                result.ScheduleTime = model.ScheduleTime;
                result.isShowOnMobile = model.isShowOnMobile;
                result.isShowOnWeb = model.isShowOnWeb;
                //result.LastEditBy = model.LastEditBy;
                //result.LastEditTime = model.LastEditTime;
                result.Actived = model.Actived;
                result.Summary = model.Summary;
                result.Detail = model.Detail;
                result.NewsCategoryId = model.NewsCategoryId;
                result.GroupEmployeeId = model.GroupEmployeeId;
                result.TypeNews = model.TypeNews;
                result.EndTime = model.EndTime;
                _context.Entry(result).State = EntityState.Added;
                //_context.SaveChanges();

                if (listCompany != null && listCompany.Count > 0)
                {
                    foreach (var item in listCompany)
                    {
                        if (item.isCheckComp == true)
                        {
                            News_Company_Mapping mapping = new News_Company_Mapping();
                            mapping.NewsId = result.NewsId;
                            mapping.CompanyId = item.CompanyId;
                            _context.Entry(mapping).State = EntityState.Added;
                        }
                    }
                }

                if (SelectNewsId != null && SelectNewsId.Count > 0)
                {
                    foreach (var item in SelectNewsId)
                    {
                        RelatedNews_Mapping mapping = new RelatedNews_Mapping();
                        mapping.RelatedNewsId = Guid.NewGuid();
                        mapping.News1 = result.NewsId;
                        mapping.News2 = item;
                        _context.Entry(mapping).State = EntityState.Added;
                    }
                }
                _context.SaveChanges();

                
                //Gửi thông báo  
                if (result.isShowOnMobile == true && result.GroupEmployeeId.HasValue)
                {
                    //Lấy danh sách account trong nhóm nhận bảng tin
                    var accountLst = (from acc in _context.TaskGroupDetailModel
                                      where acc.GroupId == result.GroupEmployeeId
                                      select acc.AccountId
                                    ).ToList();
                    if (accountLst != null && accountLst.Count > 0)
                    {
                        //Lấy danh sách device theo account
                        var deviceList = (from de in _context.Account_Device_Mapping
                                         where accountLst.Contains(de.AccountId)
                                         select de.DeviceId
                                    ).Distinct().ToList();
                        string[] deviceLst = deviceList.ToArray();
                        if (deviceLst != null && deviceLst.Length > 0)
                        {
                            PushNotification(result.NewsId, result.Title, deviceLst, "BẢNG TIN", accountLst, result.ScheduleTime);
                        }
                    }
                }
                
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.MasterData_News.ToLower())
                });
            });
        }
        #endregion

        #region Push Notification
        public void PushNotification(Guid? NewsId, string notificationMessage, string[] deviceLst, string header, List<Guid> accountLst, DateTime? ScheduleTime = null)
        {
            string message = string.Empty;

            var _utilitiesRepository = new UtilitiesRepository();
            Guid notificationId = Guid.NewGuid();
            if (!string.IsNullOrEmpty(notificationMessage) && notificationMessage.Length > 160)
            {
                notificationMessage = notificationMessage.Substring(0, 155) + "...";
            }
            _utilitiesRepository.PushNotification(deviceLst, out message, header, notificationMessage, new { newsId = NewsId }, ScheduleTime);

            //save notification in db
            if (string.IsNullOrEmpty(message))
            {
                NotificationModel notif = new NotificationModel();
                notif.NotificationId = notificationId;
                notif.TaskId = NewsId;
                notif.Title = header;
                notif.Description = notificationMessage;
                notif.Detail = notificationMessage;
                notif.CreatedDate = DateTime.Now;

                _context.Entry(notif).State = EntityState.Added;

                if (accountLst != null && accountLst.Count > 0)
                {
                    foreach (var acc in accountLst)
                    {
                        var existNotif = _context.NotificationAccountMappingModel
                                                 .Where(p => p.AccountId == acc && p.NotificationId == notif.NotificationId && p.IsRead == false)
                                                 .FirstOrDefault();
                        if (existNotif != null)
                        {
                            _context.Entry(existNotif).State = EntityState.Deleted;
                        }
                        NotificationAccountMappingModel mapping = new NotificationAccountMappingModel();
                        mapping.AccountId = acc;
                        mapping.NotificationId = notif.NotificationId;
                        mapping.IsRead = false;

                        _context.Entry(mapping).State = EntityState.Added;
                    }
                }
            }

            _context.SaveChanges();
        }
        #endregion Push Notification

        public ActionResult Detail(Guid id)
        {
            ViewBag.Company = _context.CompanyModel.OrderBy(p => p.OrderIndex).ToList();
            ViewBag.News_Company_Mapping = _context.News_Company_Mapping.Where(p => p.NewsId == id).ToList();
            var model = (from p in _context.NewsModel
                         join c in _context.NewsCategoryModel on p.NewsCategoryId equals c.NewsCategoryId
                         join empg in _context.TaskGroupDetailModel on p.GroupEmployeeId equals empg.GroupId into tgTemp
                         from tg in tgTemp.DefaultIfEmpty()
                         join cr in _context.AccountModel on p.CreateBy equals cr.AccountId
                         join empTemp in _context.SalesEmployeeModel on cr.EmployeeCode equals empTemp.SalesEmployeeCode into empList
                         from emp in empList.DefaultIfEmpty()
                         join sTemp in _context.CatalogModel on new { CatalogCode = p.Summary, CatalogTypeCode = ConstCatalogType.BANGTIN_SUMMARY } equals new { sTemp.CatalogCode, sTemp.CatalogTypeCode } into sList
                         from s in sList.DefaultIfEmpty()
                         join dTemp in _context.CatalogModel on new { CatalogCode = p.Detail, CatalogTypeCode = ConstCatalogType.BANGTIN_DETAIL } equals new { dTemp.CatalogCode, dTemp.CatalogTypeCode } into dList
                         from d in dList.DefaultIfEmpty()
                         join tTemp in _context.CatalogModel on new { CatalogCode = p.TypeNews, CatalogTypeCode = ConstCatalogType.News_Type } equals new { tTemp.CatalogCode, tTemp.CatalogTypeCode } into tList
                         from t in tList.DefaultIfEmpty()
                         where p.NewsId == id &&  (c.NewsCategoryCode.ToString() == ConstNewsType.HuongDan ||  (tg.AccountId == CurrentUser.AccountId || p.CreateBy == CurrentUser.AccountId))
                         select new NewsViewModel()
                         {
                             NewsId = p.NewsId,
                             NewsCode = p.NewsCode,
                             NewsCategoryId = p.NewsCategoryId,
                             Title = p.Title,
                             Description = p.Description,
                             ScheduleTime = p.ScheduleTime,
                             ImageUrl = p.ImageUrl,
                             isShowOnMobile = p.isShowOnMobile,
                             isShowOnWeb = p.isShowOnWeb,
                             CreateBy = p.CreateBy,
                             CreateTime = p.CreateTime,
                             LastEditBy = p.LastEditBy,
                             LastEditTime = p.LastEditTime,
                             Actived = p.Actived,
                             NewsCategoryCode = c.NewsCategoryCode,
                             NewsCategoryName = c.NewsCategoryName,
                             CreateByName = emp.SalesEmployeeName,
                             SummaryName = s.CatalogText_vi,
                             Summary = s.CatalogCode,
                             DetailName = d.CatalogText_vi,
                             Detail = d.CatalogCode,
                             TypeNews = t.CatalogText_vi
                         }).FirstOrDefault();
            if (model == null)
            {
                return RedirectToAction("ErrorText", "Error", new { area = "", statusCode = 404, exception = string.Format(LanguageResource.Error_NotExist, LanguageResource.MasterData_News.ToLower()) });
            }
            #region RelatedNews
            var listNews = new List<Guid>();
            if (model.NewsCategoryCode == ConstNewsCategoryCode.BangTin)
            {
                listNews = (from p in _context.TaskGroupDetailModel
                            join a in _context.NewsModel on p.GroupId equals a.GroupEmployeeId
                            join c in _context.NewsCategoryModel on a.NewsCategoryId equals c.NewsCategoryId
                            where p.AccountId == CurrentUser.AccountId || a.CreateBy == CurrentUser.AccountId
                            select a.NewsId).Distinct().ToList();
            }
            else
            {
                listNews = (from a in _context.NewsModel
                            join c in _context.NewsCategoryModel on a.NewsCategoryId equals c.NewsCategoryId
                            where c.NewsCategoryCode != ConstNewsCategoryCode.BangTin
                            select a.NewsId).Distinct().ToList();
            }

            //Liên quan có liên kết
            model.RelatedNews = new List<NewsViewModel>();
            var RelatedNews1 = (from a in _context.RelatedNews_Mapping
                                join r in _context.NewsModel on a.News2 equals r.NewsId
                                join l in listNews on r.NewsId equals l
                                join cr in _context.AccountModel on r.CreateBy equals cr.AccountId
                                join empTemp in _context.SalesEmployeeModel on cr.EmployeeCode equals empTemp.SalesEmployeeCode into empList
                                from emp in empList.DefaultIfEmpty()
                                join sTemp in _context.CatalogModel on new { CatalogCode = r.Summary, CatalogTypeCode = ConstCatalogType.BANGTIN_SUMMARY } equals new { sTemp.CatalogCode, sTemp.CatalogTypeCode } into sList
                                from s in sList.DefaultIfEmpty()
                                join dTemp in _context.CatalogModel on new { CatalogCode = r.Detail, CatalogTypeCode = ConstCatalogType.BANGTIN_DETAIL } equals new { dTemp.CatalogCode, dTemp.CatalogTypeCode } into dList
                                from d in dList.DefaultIfEmpty()
                                join tTemp in _context.CatalogModel on new { CatalogCode = r.TypeNews, CatalogTypeCode = ConstCatalogType.News_Type } equals new { tTemp.CatalogCode, tTemp.CatalogTypeCode } into tList
                                from t in tList.DefaultIfEmpty()
                                where a.News1 == model.NewsId && r.Actived == true 
                                select new NewsViewModel()
                                {
                                    NewsId = r.NewsId,
                                    Title = r.Title,
                                    CreateTime = r.CreateTime,
                                    CreateByName = emp.SalesEmployeeName,
                                    Description = r.Description,
                                    SummaryName = s.CatalogText_vi,
                                    Summary = s.CatalogCode,
                                    DetailName = d.CatalogText_vi,
                                    Detail = d.CatalogCode,
                                    TypeNews = t.CatalogText_vi
                                }).ToList();
            if (RelatedNews1 != null && RelatedNews1.Count() > 0)
            {
                model.RelatedNews.AddRange(RelatedNews1);
            }
            //Liên quan thông tin giống nhau
            var RelatedNews2 = (from p in _context.NewsModel
                                join cr in _context.AccountModel on p.CreateBy equals cr.AccountId
                                join l in listNews on p.NewsId equals l
                                join empTemp in _context.SalesEmployeeModel on cr.EmployeeCode equals empTemp.SalesEmployeeCode into empList
                                from emp in empList.DefaultIfEmpty()
                                join sTemp in _context.CatalogModel on new { CatalogCode = p.Summary, CatalogTypeCode = ConstCatalogType.BANGTIN_SUMMARY } equals new { sTemp.CatalogCode, sTemp.CatalogTypeCode } into sList
                                from s in sList.DefaultIfEmpty()
                                join dTemp in _context.CatalogModel on new { CatalogCode = p.Detail, CatalogTypeCode = ConstCatalogType.BANGTIN_DETAIL } equals new { dTemp.CatalogCode, dTemp.CatalogTypeCode } into dList
                                from d in dList.DefaultIfEmpty()
                                join tTemp in _context.CatalogModel on new { CatalogCode = p.TypeNews, CatalogTypeCode = ConstCatalogType.News_Type } equals new { tTemp.CatalogCode, tTemp.CatalogTypeCode } into tList
                                from t in tList.DefaultIfEmpty()
                                where (p.Summary != null && model.Summary == p.Summary
                                || p.Detail != null && model.Detail == p.Detail)
                                && model.NewsId != p.NewsId
                                && model.NewsCategoryId == p.NewsCategoryId
                                select new NewsViewModel()
                                {
                                    NewsId = p.NewsId,
                                    Title = p.Title,
                                    CreateTime = p.CreateTime,
                                    CreateByName = emp.SalesEmployeeName,
                                    Description = p.Description,
                                    SummaryName = s.CatalogText_vi,
                                    Summary = s.CatalogCode,
                                    DetailName = d.CatalogText_vi,
                                    Detail = d.CatalogCode,
                                    TypeNews = t.CatalogText_vi
                                }).ToList();
            if (RelatedNews2 != null && RelatedNews2.Count() > 0)
            {
                model.RelatedNews.AddRange(RelatedNews2);
            }
            model.RelatedNews.OrderByDescending(x=>x.CreateTime);
            #endregion
            GetRelatedNewsMapping(model.NewsId);
            CreateViewBag(model, model.NewsCategoryCode);
            return View(model);
        }

        //GET: /News/Edit
        #region Edit
        [ISDAuthorizationAttribute]
        public ActionResult Edit(Guid id)
        {
            ViewBag.Company = _context.CompanyModel.OrderBy(p => p.OrderIndex).ToList();
            ViewBag.News_Company_Mapping = _context.News_Company_Mapping.Where(p => p.NewsId == id).ToList();
            var model = (from p in _context.NewsModel
                         join c in _context.NewsCategoryModel on p.NewsCategoryId equals c.NewsCategoryId
                         join cr in _context.AccountModel on p.CreateBy equals cr.AccountId
                         join empTemp in _context.SalesEmployeeModel on cr.EmployeeCode equals empTemp.SalesEmployeeCode into empList
                         from emp in empList.DefaultIfEmpty()
                         join sTemp in _context.CatalogModel on new { CatalogCode = p.Summary, CatalogTypeCode = ConstCatalogType.BANGTIN_SUMMARY } equals new { sTemp.CatalogCode, sTemp.CatalogTypeCode } into sList
                         from s in sList.DefaultIfEmpty()
                         join dTemp in _context.CatalogModel on new { CatalogCode = p.Detail, CatalogTypeCode = ConstCatalogType.BANGTIN_DETAIL } equals new { dTemp.CatalogCode, dTemp.CatalogTypeCode } into dList
                         from d in dList.DefaultIfEmpty()
                         where p.NewsId == id
                         select new NewsViewModel()
                         {
                             NewsId = p.NewsId,
                             NewsCode = p.NewsCode,
                             NewsCategoryId = p.NewsCategoryId,
                             Title = p.Title,
                             Description = p.Description,
                             ScheduleTime = p.ScheduleTime,
                             ImageUrl = p.ImageUrl,
                             isShowOnMobile = p.isShowOnMobile,
                             isShowOnWeb = p.isShowOnWeb,
                             CreateBy = p.CreateBy,
                             CreateTime = p.CreateTime,
                             LastEditBy = p.LastEditBy,
                             LastEditTime = p.LastEditTime,
                             Actived = p.Actived,
                             NewsCategoryCode = c.NewsCategoryCode,
                             NewsCategoryName = c.NewsCategoryName,
                             CreateByName = emp.SalesEmployeeName,
                             SummaryName = s.CatalogText_vi,
                             Summary = s.CatalogCode,
                             DetailName = d.CatalogText_vi,
                             Detail = d.CatalogCode,
                             EndTime =  p.EndTime,
                             TypeNews = p.TypeNews,
                             GroupEmployeeId = p.GroupEmployeeId
                         }).FirstOrDefault();
            if (model == null)
            {
                return RedirectToAction("ErrorText", "Error", new { area = "", statusCode = 404, exception = string.Format(LanguageResource.Error_NotExist, LanguageResource.MasterData_Company.ToLower()) });
            }
            model.RelatedNews = (from p in _context.NewsModel
                                 join a in _context.RelatedNews_Mapping on p.NewsId equals a.News1
                                 join r in _context.NewsModel on a.News2 equals r.NewsId
                                 where p.NewsId == model.NewsId
                                 select new NewsViewModel()
                                 {
                                     NewsId = r.NewsId,
                                     Title = r.Title,
                                 }).ToList();
            GetRelatedNewsMapping(model.NewsId);
            CreateViewBag(model, model.NewsCategoryCode);
            return View(model);
        }
        //POST: Edit
        [HttpPost]
        [ValidateInput(false)]
        [ISDAuthorizationAttribute]
        public JsonResult Edit(NewsViewModel model, HttpPostedFileBase FileUpload, List<ListCompanyViewModel> listCompany, List<Guid?> SelectNewsId)
        {
            return ExecuteContainer(() =>
            {
                if (model.NewsCategoryCode == ConstNewsCategoryCode.BangTin && model.EndTime == null)
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = string.Format("{0}", "Vui lòng nhập thông tin '" + LanguageResource.News_EndTime + "'!")
                    });
                }
                if (model.NewsCategoryCode == ConstNewsCategoryCode.BangTin && model.ScheduleTime == null)
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = string.Format("{0}", "Vui lòng nhập thông tin '" + LanguageResource.News_ScheduleTime + "'!")
                    });
                }
                //Lưu bảng tin
                var news = _context.NewsModel.FirstOrDefault(p => p.NewsId == model.NewsId);
                news.Title = model.Title;
                news.Description = model.Description;
                news.ScheduleTime = model.ScheduleTime;
                news.isShowOnMobile = model.isShowOnMobile;
                news.isShowOnWeb = model.isShowOnWeb;
                news.LastEditBy = CurrentUser.AccountId;
                news.LastEditTime = DateTime.Now;
                news.Actived = model.Actived;
                news.Summary = model.Summary;
                news.Detail = model.Detail;
                news.EndTime = model.EndTime;
                news.TypeNews = model.TypeNews;
                news.GroupEmployeeId = model.GroupEmployeeId;

                if (FileUpload != null)
                {
                    news.ImageUrl = getFileName(FileUpload);
                }
                _context.Entry(news).State = EntityState.Modified;
                //_context.SaveChanges();

                //Lưu mapping công ty
                if (listCompany != null && listCompany.Count > 0)
                {
                    foreach (var item in listCompany)
                    {
                        var checkmapping = _context.News_Company_Mapping.FirstOrDefault(p => p.NewsId == model.NewsId && p.CompanyId == item.CompanyId);
                        //Chưa có trong csdl => thêm mới
                        if (checkmapping == null)
                        {
                            if (item.isCheckComp == true)
                            {
                                News_Company_Mapping mapping = new News_Company_Mapping();
                                mapping.NewsId = model.NewsId;
                                mapping.CompanyId = item.CompanyId;
                                _context.Entry(mapping).State = EntityState.Added;
                                //_context.SaveChanges();
                            }
                        }
                        //Đã có trong csdk => sửa
                        else
                        {
                            if (item.isCheckComp == false)
                            {
                                _context.Entry(checkmapping).State = EntityState.Deleted;
                                //_context.SaveChanges();
                            }
                        }
                    }
                }

                //Xóa data cũ
                var checkRelatedNewMapping = _context.RelatedNews_Mapping.Where(p => p.News1 == model.NewsId);
                if (checkRelatedNewMapping != null && checkRelatedNewMapping.Count() > 0)
                {
                    //Xóa data cũ
                    foreach (var data in checkRelatedNewMapping)
                    {
                        _context.Entry(data).State = EntityState.Deleted;
                    }
                }
                if (SelectNewsId != null && SelectNewsId.Count > 0)
                {
                    foreach (var item in SelectNewsId)
                    {

                        //Thêm data mới
                        RelatedNews_Mapping mapping = new RelatedNews_Mapping();
                        mapping.RelatedNewsId = Guid.NewGuid();
                        mapping.News1 = model.NewsId;
                        mapping.News2 = item;
                        _context.Entry(mapping).State = EntityState.Added;
                    }
                }
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

        //GET: /News/Delete
        #region Delete
        [HttpPost]
        [ISDAuthorizationAttribute]
        public ActionResult Delete(Guid id)
        {
            return ExecuteDelete(() =>
            {
                var mapping = _context.News_Company_Mapping.Where(p => p.NewsId == id).ToList();
                if (mapping != null)
                {
                    _context.News_Company_Mapping.RemoveRange(mapping);
                }
                var news = _context.NewsModel.FirstOrDefault(p => p.NewsId == id);
                if (news != null)
                {
                    _context.Entry(news).State = EntityState.Deleted;
                    _context.SaveChanges();

                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.Created,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.MasterData_News.ToLower())
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
        public void CreateViewBag(NewsViewModel viewModel = null, int? Type = null)
        {
            if (viewModel == null)
            {
                viewModel = new NewsViewModel();
            }
            ViewBag.Type = Type;
            string pageUrl = "/MasterData/News";
            var parameter = string.Empty;
            if (Type.ToString() == ConstNewsType.BangTin)
            {
                parameter = "?Type=" + ConstNewsType.BangTin;
            }
            else if (Type.ToString() == ConstNewsType.HuongDan)
            {
                parameter = "?Type=" + ConstNewsType.HuongDan;
            }
            var title = (from p in _context.PageModel
                         where p.PageUrl == pageUrl
                         && p.Parameter.Contains(Type.ToString())
                         select p.PageName).FirstOrDefault();
            ViewBag.Title = title;
            ViewBag.PageId = GetPageId(pageUrl, parameter);

            if (viewModel.NewsCategoryId == null)
            {
                viewModel.NewsCategoryId = _context.NewsCategoryModel.Where(p => p.Actived == true && p.NewsCategoryCode == Type).Select(x => x.NewsCategoryId).FirstOrDefault();
            }

            //Get list NewsCategory
            var newscategoryList = _context.NewsCategoryModel.Where(p => p.Actived == true)
                                                           .OrderBy(p => p.OrderIndex).ToList();
            ViewBag.NewsCategoryId = new SelectList(newscategoryList, "NewsCategoryId", "NewsCategoryName", viewModel.NewsCategoryId);

            //Chủ đề
            var summaryList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.BANGTIN_SUMMARY);
            ViewBag.Summary = new SelectList(summaryList, "CatalogCode", "CatalogText_vi", viewModel.Summary);

            var detailList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.BANGTIN_DETAIL);
            ViewBag.Detail = new SelectList(detailList, "CatalogCode", "CatalogText_vi", viewModel.Detail);

            var TypeNewsList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.News_Type);
            ViewBag.TypeNews = new SelectList(TypeNewsList, "CatalogCode", "CatalogText_vi", viewModel.TypeNews);

            var GroupEmployeeList = _context.TaskGroupModel.Where(x => x.GroupType == ConstTaskGroupType.News && x.CreatedAccountId == CurrentUser.AccountId ).ToList();
            ViewBag.GroupEmployeeId = new SelectList(GroupEmployeeList, "GroupId", "GroupName", viewModel.GroupEmployeeId);


        }

        public string getFileName(HttpPostedFileBase file)
        {
            var fileName = Path.GetFileName(file.FileName);

            //Create dynamically folder to save file
            var existPath = Server.MapPath("~/Upload/News");
            Directory.CreateDirectory(existPath);
            var path = Path.Combine(existPath, fileName);

            file.SaveAs(path);

            return fileName;
        }

        public void GetRelatedNewsMapping(Guid? Id)
        {
            var ActivedNewsList = (from p in _context.NewsModel
                                   join m in _context.RelatedNews_Mapping on p.NewsId equals m.News2
                                   join c in _context.NewsCategoryModel on p.NewsCategoryId equals c.NewsCategoryId
                                   join cr in _context.AccountModel on p.CreateBy equals cr.AccountId
                                   join empTemp in _context.SalesEmployeeModel on cr.EmployeeCode equals empTemp.SalesEmployeeCode into empList
                                   from emp in empList.DefaultIfEmpty()
                                   join sTemp in _context.CatalogModel on new { CatalogCode = p.Summary, CatalogTypeCode = ConstCatalogType.BANGTIN_SUMMARY } equals new { sTemp.CatalogCode, sTemp.CatalogTypeCode } into sList
                                   from s in sList.DefaultIfEmpty()
                                   join dTemp in _context.CatalogModel on new { CatalogCode = p.Detail, CatalogTypeCode = ConstCatalogType.BANGTIN_DETAIL } equals new { dTemp.CatalogCode, dTemp.CatalogTypeCode } into dList
                                   from d in dList.DefaultIfEmpty()
                                   where m.News1 == Id && p.Actived == true
                                   select new NewsViewModel
                                   {
                                       NewsId = p.NewsId,
                                       Title = p.Title,
                                       CreateTime = p.CreateTime,
                                       Description = p.Description,
                                       SummaryName = s.CatalogText_vi,
                                       DetailName = d.CatalogText_vi,
                                       ImageUrl = p.ImageUrl,
                                   }).ToList();
            ViewBag.ActivedNewsList = ActivedNewsList;
        }

        public ActionResult SearchNewSelect2(string type, string search)
        {
            var result = new List<ISDSelectItem>();
            result = (from a in _context.NewsModel
                      join p in _context.NewsCategoryModel on a.NewsCategoryId equals p.NewsCategoryId
                      where (type == null || p.NewsCategoryCode.ToString() == type)
                      && a.Actived == true
                      && (search == null || a.Title.Contains(search) || a.Description.Contains(search))
                      orderby a.NewsCode
                      select new ISDSelectItem()
                      {
                          value = a.NewsId,
                          text = a.Title
                      }).Take(10).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion


    }
}