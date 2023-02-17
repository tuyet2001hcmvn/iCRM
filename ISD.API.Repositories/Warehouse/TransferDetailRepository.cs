using ISD.API.EntityModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.API.Repositories
{
    public interface ITransferDetailRepository
    {
    }
    public class TransferDetailRepository : GenericRepository<TransferDetailModel>, ITransferDetailRepository
    {
        public TransferDetailRepository(ICRMDbContext context) : base(context)
        {
        }
    }
}
