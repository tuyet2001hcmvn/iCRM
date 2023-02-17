using ISD.Constant;
using ISD.EntityModels;
using ISD.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.Repositories
{
    public class NewsRepository
    {
        EntityDataContext _context;
        public NewsRepository(EntityDataContext db)
        {
            _context = db;
        }

        public List<NewsMobileViewModel> GetNewsBy(Guid? AccountId, string CompanyCode)
        {
            //Ngày hiện tại
            var currentDate = DateTime.Now;
            var ret = new List<NewsMobileViewModel>();
            ret = (from n in _context.NewsModel
                       //join m in _context.News_Company_Mapping on n.NewsId equals m.NewsId
                       //join c in _context.CompanyModel on m.CompanyId equals c.CompanyId
                   join nc in _context.NewsCategoryModel on n.NewsCategoryId equals nc.NewsCategoryId
                   join empg in _context.TaskGroupDetailModel on n.GroupEmployeeId equals empg.GroupId into tgTemp
                   from tg in tgTemp.DefaultIfEmpty()
                   join cr in _context.AccountModel on n.CreateBy equals cr.AccountId
                   join empTemp in _context.SalesEmployeeModel on cr.EmployeeCode equals empTemp.SalesEmployeeCode into empList
                   from emp in empList.DefaultIfEmpty()
                   join sTemp in _context.CatalogModel on new { CatalogCode = n.Summary, CatalogTypeCode = ConstCatalogType.BANGTIN_SUMMARY } equals new { sTemp.CatalogCode, sTemp.CatalogTypeCode } into sList
                   from s in sList.DefaultIfEmpty()
                   join dTemp in _context.CatalogModel on new { CatalogCode = n.Detail, CatalogTypeCode = ConstCatalogType.BANGTIN_DETAIL } equals new { dTemp.CatalogCode, dTemp.CatalogTypeCode } into dList
                   from d in dList.DefaultIfEmpty()
                   join tTemp in _context.CatalogModel on new { CatalogCode = n.TypeNews, CatalogTypeCode = ConstCatalogType.News_Type } equals new { tTemp.CatalogCode, tTemp.CatalogTypeCode } into tList
                   from t in tList.DefaultIfEmpty()
                   where //c.CompanyCode == CompanyCode
                    nc.NewsCategoryCode == ConstNewsCategoryCode.BangTin
                   && n.isShowOnMobile == true
                   && ((tg != null && tg.AccountId == AccountId) || n.CreateBy == AccountId)
                   && (n.EndTime == null || currentDate >= n.EndTime)
                   select new NewsMobileViewModel()
                   {
                       NotificationId = n.NewsId,
                       Title = n.Title,
                       CreatedDate = n.ScheduleTime != null ? n.ScheduleTime : n.CreateTime,
                       //Nếu tin tức không có hình ảnh thì lấy ảnh của loại tin tức
                       ImageUrl = (n.ImageUrl != null && n.ImageUrl != ConstImageUrl.noImage) ? ConstDomain.Domain + "/Upload/News/" + n.ImageUrl : ConstDomain.Domain + "/Upload/NewsCategory/" + nc.ImageUrl,
                       SummaryName = s.CatalogText_vi,
                       Summary = s.CatalogCode,
                       DetailName = d.CatalogText_vi,
                       Detail = d.CatalogCode,
                       TypeNews = t.CatalogText_vi,
                       CreateByName = emp.SalesEmployeeName,
                       ScheduleTimeTemp = n.ScheduleTime,
                   }).ToList();
            return ret.GroupBy(g => new { g.NotificationId, g.Title, g.CreatedDate, g.ImageUrl, g.SummaryName, g.Summary, g.DetailName, g.Detail, g.TypeNews, g.CreateByName, g.ScheduleTimeTemp })
                        .Select(p => new NewsMobileViewModel()
                        {
                            NotificationId = p.Key.NotificationId,
                            Title = p.Key.Title,
                            CreatedDate = p.Key.CreatedDate,
                            ImageUrl = p.Key.ImageUrl,
                            SummaryName = p.Key.SummaryName,
                            Summary = p.Key.Summary,
                            DetailName = p.Key.DetailName,
                            Detail = p.Key.Detail,
                            TypeNews = p.Key.TypeNews,
                            CreateByName = p.Key.CreateByName,
                            ScheduleTimeTemp = p.Key.ScheduleTimeTemp,
                        }).ToList();
        }

        public NewsMobileViewModel GetNewsDetails(Guid NewsId)
        {
            var ret = new NewsMobileViewModel();
            ret = (from n in _context.NewsModel
                   join sTemp in _context.CatalogModel on new { CatalogCode = n.Summary, CatalogTypeCode = ConstCatalogType.BANGTIN_SUMMARY } equals new { sTemp.CatalogCode, sTemp.CatalogTypeCode } into sList
                   from s in sList.DefaultIfEmpty()
                   join dTemp in _context.CatalogModel on new { CatalogCode = n.Detail, CatalogTypeCode = ConstCatalogType.BANGTIN_DETAIL } equals new { dTemp.CatalogCode, dTemp.CatalogTypeCode } into dList
                   from d in dList.DefaultIfEmpty()
                   join tTemp in _context.CatalogModel on new { CatalogCode = n.TypeNews, CatalogTypeCode = ConstCatalogType.News_Type } equals new { tTemp.CatalogCode, tTemp.CatalogTypeCode } into tList
                   from t in tList.DefaultIfEmpty()
                   join cr in _context.AccountModel on n.CreateBy equals cr.AccountId
                   join empTemp in _context.SalesEmployeeModel on cr.EmployeeCode equals empTemp.SalesEmployeeCode into empList
                   from emp in empList.DefaultIfEmpty()
                   where n.NewsId == NewsId
                   select new NewsMobileViewModel()
                   {
                       NotificationId = n.NewsId,
                       Title = n.Title,
                       Description = n.Description,
                       SummaryName = s.CatalogText_vi,
                       Summary = s.CatalogCode,
                       DetailName = d.CatalogText_vi,
                       Detail = d.CatalogCode,
                       TypeNews = t.CatalogText_vi,
                       CreateByName = emp.SalesEmployeeName,
                       ScheduleTimeTemp = n.ScheduleTime,
                   }).FirstOrDefault();
            return ret;
        }
    }
}
