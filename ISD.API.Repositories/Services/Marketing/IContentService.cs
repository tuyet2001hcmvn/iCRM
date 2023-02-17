using ISD.API.ViewModels.MarketingViewModels.ContentViewModels;
using System;
using System.Collections.Generic;

namespace ISD.API.Repositories.Services.Marketing
{
    public interface IContentService
    {
        ContenViewViewModel Create(ContentCreateViewModel content);
        ContenViewViewModel GetById(Guid id);
        void Update(Guid id, ContentEditViewModel obj);
        List<ContenViewViewModel> Search(int? contentCode, string contentName, bool? actived, int pageIndex, int pageSize, string type);
        int GetTotalRow(int? contentCode, string contentName, bool? actived, string type);
        List<ContenViewViewModel> GetAll(string Type);
    }
}
