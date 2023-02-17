using ISD.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.Repositories
{
    public class EmailTemplateConfigRepository
    {
        EntityDataContext _context;
        public EmailTemplateConfigRepository(EntityDataContext db)
        {
            _context = db;
        }

     
        public EmailTemplateConfigModel GetByType(string EmailTemplateType)
        {
            var ret = _context.EmailTemplateConfigModel.Where(p => p.EmailTemplateType == EmailTemplateType && p.Actived == true).FirstOrDefault();
            return ret;
        }
        
    }
}
