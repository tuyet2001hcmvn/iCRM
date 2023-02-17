using ISD.API.EntityModels.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISD.API.Repositories.Marketing
{
    public interface IProfileRepository
    {
        List<ProfileModel> GetInfoMemberOfTagetGroup(List<MemberOfTargetGroupModel> members);
    }
}
