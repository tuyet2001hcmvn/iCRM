using ISD.API.EntityModels.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISD.API.Repositories.Marketing
{
    public class ProfileRepository : GenericRepository<ProfileModel>, IProfileRepository
    {
        public ProfileRepository(ICRMDbContext context) : base(context)
        {
        }

        public List<ProfileModel> GetInfoMemberOfTagetGroup(List<MemberOfTargetGroupModel> members)
        {
            List<ProfileModel> listInfo = new List<ProfileModel>();
            foreach(var mem in members)
            {
                ProfileModel info = context.ProfileModels.FirstOrDefault(p => p.ProfileId == mem.ProfileId);
                listInfo.Add(info);
            }
            return listInfo;
        }
    }
}
