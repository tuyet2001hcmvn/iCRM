using ISD.API.ViewModels;
using ISD.API.ViewModels.MarketingViewModels.TargetGroupViewModels;
using System;
using System.Collections.Generic;

namespace ISD.API.Repositories.Services.Marketing
{
    public interface ITargetGroupService
    {
        TargetGroupViewViewModel Create(TargetGroupCreateViewModel obj);
        void Update(TargetGroupEditViewModel obj, Guid id);
        TargetGroupViewViewModel GetById(Guid id);
        List<TargetGroupViewViewModel> Search(int? targetGroupCode, string targetGroupName, bool? actived, int page, int pageSize, string type);
        int GetTotalRow(int? targetGroupCode, string targetGroupName, bool? actived, string type);
        List<TargetGroupViewModel> GetAll(string Type);
    }
}
