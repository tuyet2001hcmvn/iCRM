using ISD.API.ViewModels.MarketingViewModels.MemberOfTargetGroupViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace ISD.API.Repositories.Services.Marketing
{
    public interface IMemberOfTargetGroupService
    {
        void Import(IFormFile files, Guid id, string type = null);
        List<MemberOfTargetGroupViewViewModel> GetMemberOfTargetGroupById(Guid id, bool hasEmail, bool distinctEmail, int pageIndex, int pageSize, out int totalRow);
        List<MemberOfTargetGroupViewViewModel> GetExternalMemberOfTargetGroupById(Guid id, int pageIndex, int pageSize, out int totalRow);
        void ImportExternal(IFormFile file, Guid targetGroupId);
    }
}
