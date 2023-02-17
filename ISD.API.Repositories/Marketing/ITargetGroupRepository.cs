using ISD.API.EntityModels.Entities;
using System.Collections.Generic;
using System.Linq;

namespace ISD.API.Repositories.Marketing
{
    public interface ITargetGroupRepository 
    {
        TargetGroupModel GetByCode(int code);
        //void UpdateTargetGroup(TargetGroupModel model);
        IEnumerable<TargetGroupModel> Search(int? targetGroupCode, string targetGroupName, bool? actived, int page, int pageSize, string type);
        int GetTotalRow(int? targetGroupCode, string targetGroupName, bool? actived, string type);
        IQueryable<TargetGroupModel> GetAll(string Type);
    }
}
