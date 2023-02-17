using ISD.API.Core;
using ISD.API.EntityModels.Entities;
using ISD.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.API.Repositories.Marketing
{
    public class CatalogRepository : GenericRepository<CatalogModel>,ICatalogRepository
    {
        public CatalogRepository(ICRMDbContext context) : base(context)
        {
        }

        public IQueryable<CatalogModel> GetCampaignStatus()
        {
            return context.CatalogModels.Where(c => c.CatalogTypeCode == Constant.CampaignCatalogTypeCode && c.Actived == true);
        }

        /// <summary>
        /// Lấy danh sách Catalog theo CatalogType
        /// </summary>
        /// <param name="CataLogType">string: CatalogType</param>
        /// <returns>Danh sách catalog</returns>
        public IQueryable<CatalogViewModel> GetBy(string CataLogType)
        {
            var ret = (from p in context.CatalogModels
                       where CataLogType == p.CatalogTypeCode && p.Actived == true
                       orderby p.OrderIndex, p.CatalogCode
                       select new CatalogViewModel()
                       {
                           CatalogId = p.CatalogId,
                           CatalogCode = p.CatalogCode,
                           CatalogTextVi = p.CatalogTextVi,
                           CatalogTextEn = p.CatalogTextEn,
                           OrderIndex = p.OrderIndex,
                       });
            return ret;
        }

    }
}
