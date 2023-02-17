using ISD.API.EntityModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.API.Repositories
{
    public interface IMemberOfExternalProfileTargetGroupRepository
    {
    }
    public class MemberOfExternalProfileTargetGroupRepository : GenericRepository<MemberOfExternalProfileTargetGroupModel>, IMemberOfExternalProfileTargetGroupRepository
    {
        public MemberOfExternalProfileTargetGroupRepository(ICRMDbContext context) : base(context)
        {
        }
    }
}
