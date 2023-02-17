using ISD.API.EntityModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.API.Repositories.Marketing
{
    public class SendMailCalendarRepository : GenericRepository<SendMailCalendarModel>, ISendMailCalendarRepository
    {
        public SendMailCalendarRepository(ICRMDbContext context) : base(context)
        {
        }
    }
}
