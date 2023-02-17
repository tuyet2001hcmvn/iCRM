using ISD.API.Core;
using ISD.API.EntityModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.API.Repositories
{
    public interface IRequestEccEmailConfigRepository
    {
        RequestEccEmailConfigModel GetEmailConfig();
    }
    public class RequestEccEmailConfigRepository : IRequestEccEmailConfigRepository
    {
        private ICRMDbContext _context;
        public RequestEccEmailConfigRepository(ICRMDbContext context)
        {
            _context = context;
        }

        public RequestEccEmailConfigModel GetEmailConfig()
        {
            var config = _context.RequestEccEmailConfigModels.FirstOrDefault();
            config.ToEmail = _context.CatalogModels.FirstOrDefault(s => s.CatalogCode == Constant.RequestEccConfig_ToEmail).CatalogTextVi;
            config.Subject = _context.CatalogModels.FirstOrDefault(s => s.CatalogCode == Constant.RequestEccConfig_Subject).CatalogTextVi;
            return config;
        }
    }
}
