using ISD.API.EntityModels.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ISD.API.Repositories.Marketing
{
    public interface IMemberOfTargetGroupRepository
    {
        IQueryable<ProfileModel> GetMemberOfTargetGroupById(Guid id, bool hasEmail, out int totalRow);
        IQueryable<ProfileModel> GetExternalMemberOfTargetGroupById(Guid id, out int totalRow);
        void Import(DataTable members, Guid targetGroupId);
        int GetTotalMemberOfGroup(Guid id);
    }
}
