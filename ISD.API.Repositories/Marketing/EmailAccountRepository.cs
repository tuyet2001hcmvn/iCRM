using ISD.API.EntityModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.API.Repositories.Marketing
{
    public class EmailAccountRepository : GenericRepository<EmailAccountModel>, IEmailAccountRepository
    {
        public EmailAccountRepository(ICRMDbContext context) : base(context)
        {
        }

        public IQueryable<EmailAccountModel> GetAll()
        {
            return context.EmailAccountModels.Where(s=>s.IsSender==true);
        }

        public EmailAccountModel GetByAddress(string address)
        {
            return context.EmailAccountModels.Where(e => e.Address == address).FirstOrDefault();
        }
    }
}
