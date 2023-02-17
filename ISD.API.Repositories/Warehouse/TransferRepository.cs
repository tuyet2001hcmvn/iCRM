using ISD.API.EntityModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.API.Repositories
{
    public interface ITransferRepository
    {
    }
    public class TransferRepository : GenericRepository<TransferModel>, ITransferRepository
    {
        public TransferRepository(ICRMDbContext context) : base(context)
        {
        }
    }
}
