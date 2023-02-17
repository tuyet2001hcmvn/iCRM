using ISD.Constant;
using ISD.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.Repositories
{
    public class RequestEccEmailConfigRepository
    {
        EntityDataContext _context;
        public RequestEccEmailConfigRepository(EntityDataContext db)
        {
            _context = db;
        }
        public RequestEccEmailConfigModel GetEmailConfig()
        {
            var config = _context.RequestEccEmailConfigModel.FirstOrDefault();
            config.ToEmail = _context.CatalogModel.FirstOrDefault(s => s.CatalogCode == ConstCatalogType.RequestEccConfig_ToEmail).CatalogText_vi;
            config.Subject = _context.CatalogModel.FirstOrDefault(s => s.CatalogCode == ConstCatalogType.RequestEccConfig_Subject).CatalogText_vi;
            return config;
        }
    }
}
