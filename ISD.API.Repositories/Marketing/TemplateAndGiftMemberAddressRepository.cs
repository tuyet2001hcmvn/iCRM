using ISD.API.EntityModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.API.Repositories
{
    public interface ITemplateAndGiftMemberAddressRepository
    {
    }
    public class TemplateAndGiftMemberAddressRepository : GenericRepository<TemplateAndGiftMemberAddressModel>, ITemplateAndGiftMemberAddressRepository
    {
        public TemplateAndGiftMemberAddressRepository(ICRMDbContext context) : base(context)
        {

        }
    }
}
