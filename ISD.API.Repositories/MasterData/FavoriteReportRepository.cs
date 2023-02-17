using ISD.API.Core;
using ISD.API.EntityModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.API.Repositories.MasterData
{
    public class FavoriteReportRepository : GenericRepository<FavoriteReportModel>, IFavoriteReportRepository
    {
        public FavoriteReportRepository(ICRMDbContext context) : base(context)
        {
        }

        public IQueryable<PageModel> GetAllReportWithPermission(Guid accountId, string reportName, out int totalRow)
        {
            totalRow = 0;
            var list = (from ar in context.AccountInRoleModels
                       join pp in context.PagePermissionModels on ar.RolesId equals pp.RolesId into t1
                       from p1 in t1.DefaultIfEmpty()
                       join p in context.PageModels on p1.PageId equals p.PageId into t2
                       from p2 in t2.DefaultIfEmpty()
                       join m in context.MenuModels on p2.MenuId equals m.MenuId
                       where ar.AccountId == accountId &&
                             m.MenuName == Constant.ReportMenuName && 
                             p2.Actived == true &&
                             (reportName == null || reportName== "" || p2.PageName.Contains(reportName))
                             && p2.PageName!= Constant.FavoriteReportPage
                       orderby (p2.OrderIndex)
                       select p2).DistinctBy(t => t.PageId).AsQueryable();
          //  list = list.DistinctBy(t => t.PageId).AsQueryable();
            totalRow = list.Count();
            return list;
        }
    //    private static IEnumerable<TSource> DistinctByImpl<TSource, TKey>(IEnumerable<TSource> source,
    //Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
    //    {
    //        HashSet<TKey> knownKeys = new HashSet<TKey>(comparer);
    //        foreach (TSource element in source)
    //        {
    //            if (knownKeys.Add(keySelector(element)))
    //            {
    //                yield return element;
    //            }
    //        }
    //    }

        public IQueryable<PageModel> GetFavoriteReport(Guid accountId)
        {
            var list = from t1 in context.FavoriteReportModels
                       join t2 in context.PageModels on t1.PageId equals t2.PageId
                       where t1.AccountId == accountId
                       orderby (t2.OrderIndex)
                       select t2;
            return list;
        }
    }
}
