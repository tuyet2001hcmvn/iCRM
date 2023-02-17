using AutoMapper;
using ISD.API.EntityModels.Entities;
using ISD.API.ViewModels;
using ISD.API.ViewModels.MarketingViewModels.CampaignViewModels;
using ISD.API.ViewModels.MarketingViewModels.ContentViewModels;
using ISD.API.ViewModels.MarketingViewModels.EmailAccountViewModels;
using ISD.API.ViewModels.MarketingViewModels.TargetGroupViewModels;
using ISD.API.ViewModels.MasterDataViewModels.FavoriteReportViewModels;
using ISD.API.ViewModels.MasterDataViewModels.QuestionBankViewModels;
using ISD.API.ViewModels.UtilitiesViewModel.SearchTemplateViewModel;
using System;
using System.Text.RegularExpressions;
using System.Web;

namespace ISD.API.Repositories
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region Content
            CreateMap<ContentCreateViewModel, ContentModel>()
                .ForMember(d => d.Id, m => m.MapFrom(d => Guid.NewGuid()))
                .ForMember(d => d.CreateTime, m => m.MapFrom(d => DateTime.Now))
                .ForMember(d => d.Actived, m => m.MapFrom(s => true));

            CreateMap<ContentInfoViewModel, ContentModel>();
            CreateMap<ContentModel, ContenViewViewModel>()
                .ForMember(d => d.CreateBy, m => m.MapFrom(s => s.CreateByNavigation.UserName))
                .ForMember(d => d.LastEditBy, m => m.MapFrom(s => s.LastEditByNavigation != null ? s.LastEditByNavigation.UserName : s.CreateByNavigation.UserName))
                .ForMember(d => d.LastEditTime, m => m.MapFrom(s => s.LastEditByNavigation != null ? s.LastEditTime : s.CreateTime));
            CreateMap<ContentModel, ContentEditViewModel>();
            CreateMap<ContentEditViewModel, ContentModel>()
                 .ForMember(d => d.LastEditTime, m => m.MapFrom(d => DateTime.Now));
            #endregion

            #region EmailAccount
            CreateMap<EmailAccountModel, EmailAccountViewModel>();
            #endregion

            #region TargetGroup
            CreateMap<TargetGroupModel, TargetGroupViewModel>();
            CreateMap<TargetGroupEditViewModel, TargetGroupModel>()
                .ForMember(d => d.LastEditTime, m => m.MapFrom(d => DateTime.Now));
            CreateMap<TargetGroupModel, TargetGroupViewViewModel>()
                 .ForMember(d => d.CreateBy, m => m.MapFrom(s=>s.CreateByNavigation.UserName))
                 .ForMember(d => d.LastEditBy, m => m.MapFrom(s => s.LastEditByNavigation!=null?s.LastEditByNavigation.UserName: s.CreateByNavigation.UserName))
                 .ForMember(d => d.LastEditTime, m => m.MapFrom(s => s.LastEditByNavigation != null ? s.LastEditTime : s.CreateTime));
            CreateMap<TargetGroupCreateViewModel, TargetGroupModel>()
                .ForMember(d => d.Id, m => m.MapFrom(d => Guid.NewGuid()))
                .ForMember(d => d.CreateTime, m => m.MapFrom(d => DateTime.Now))
                .ForMember(d => d.Actived, m => m.MapFrom(s => true));
            #endregion

            #region Campaign
            CreateMap<CampaignCreateViewModel, CampaignModel>()
                .ForMember(d=>d.CreateTime,m=>m.MapFrom(d=>DateTime.Now))
                .ForMember(d => d.Id, m => m.MapFrom(d => Guid.NewGuid()))
                .ForMember(d => d.IsImmediately, m => m.MapFrom(s => s.IsImmediately == true ? true : false))
                .ForMember(d => d.ScheduledToStart, m => m.MapFrom(s =>s.IsImmediately==true? DateTime.Now:s.ScheduledToStart));
            CreateMap<CampaignModel, CampaignViewViewModel>()
                .ForMember(d => d.ContentName, m => m.MapFrom(d => d.Content.ContentName))
                .ForMember(d => d.TargetGroupName, m => m.MapFrom(d => d.TargetGroup.TargetGroupName))
                .ForMember(d => d.CreateBy, m => m.MapFrom(s => s.CreateBy))
                .ForMember(d => d.LastEditBy, m => m.MapFrom(s => s.LastEditBy != null ? s.LastEditBy : s.CreateBy))
                .ForMember(d => d.LastEditTime, m => m.MapFrom(s => s.LastEditTime != null ? s.LastEditTime : s.CreateTime))
                .ForMember(d => d.StatusName, m => m.MapFrom(s => s.StatusNavigation.CatalogTextVi));
            CreateMap<CampaignEditViewModel, CampaignModel>()
                .ForMember(d => d.LastEditTime, m => m.MapFrom(d => DateTime.Now))
                .ForMember(d => d.IsImmediately, m => m.Ignore())
                .ForMember(d => d.ScheduledToStart, m => m.Ignore());
            CreateMap<CatalogModel, CampaignStatusViewModel>();
            #endregion

            #region Unfollow
            CreateMap<Unfollow, UnfollowViewModel>();
            CreateMap<UnfollowCreateViewModel, Unfollow>()
                .ForMember(d => d.Id, m => m.MapFrom(d => Guid.NewGuid()));
            #endregion


            #region QuestionBank
            CreateMap<CatalogModel, QuestionCategoryViewModel>()
               .ForMember(d => d.QuestionCategoryId, m => m.MapFrom(d => d.CatalogId))
               .ForMember(d => d.QuestionCategoryName, m => m.MapFrom(d => d.CatalogTextVi));
            CreateMap<CatalogModel, DepartmentViewModel>()
              .ForMember(d => d.DepartmentId, m => m.MapFrom(d => d.CatalogId))
              .ForMember(d => d.DepartmentName, m => m.MapFrom(d => d.CatalogTextVi));
            CreateMap<QuestionBankModel, QuestionViewModel>()
               .ForMember(d => d.QuestionCategoryName, m => m.MapFrom(d => d.QuestionCategory.CatalogTextVi))
               .ForMember(d => d.DepartmentName, m => m.MapFrom(d => d.Department.CatalogTextVi))
               .ForMember(d => d.CreateBy, m => m.MapFrom(s => s.CreateByNavigation.UserName))
               .ForMember(d => d.Actived, m => m.MapFrom(s => s.Actived == true ? "Đã trả lời" : "Chưa trả lời"))
               .ForMember(d => d.LastEditBy, m => m.MapFrom(s => s.LastEditByNavigation != null ? s.LastEditByNavigation.UserName : s.CreateByNavigation.UserName))
               .ForMember(d => d.LastEditTime, m => m.MapFrom(s => s.LastEditByNavigation != null ? s.LastEditTime : s.CreateTime));
            CreateMap<QuestionCreateViewModel, QuestionBankModel>()
                .ForMember(d => d.CreateTime, m => m.MapFrom(d => DateTime.Now))
                .ForMember(d => d.Id, m => m.MapFrom(d => Guid.NewGuid()));
            CreateMap<QuestionEditViewModel, QuestionBankModel>()
                .ForMember(d => d.LastEditTime, m => m.MapFrom(d => DateTime.Now));
            #endregion

            #region FavoriteReport
            CreateMap<PageModel, FavoriteReportViewModel>()
               .ForMember(d => d.ReportName, m => m.MapFrom(d => d.PageName))
               .ForMember(d => d.PageId, m => m.MapFrom(d => d.PageId))
               .ForMember(d => d.IsFavorite, m => m.MapFrom(d=>false))
               .ForMember(d => d.PageUrl, m => m.MapFrom(d=>d.Parameter!=null ? d.PageUrl+d.Parameter:d.PageUrl));
            CreateMap<FavoriteReportCreateViewModel, FavoriteReportModel>()
                .ForMember(d => d.Id, m => m.MapFrom(s => Guid.NewGuid()));
            #endregion

            #region SeachTemplate
           
            CreateMap<SearchTemplateCreateViewModel, SearchTemplateModel>()
                .ForMember(d => d.SearchTemplateId, m => m.MapFrom(d => Guid.NewGuid()));
            CreateMap<SearchTemplateModel, SearchTemplateViewModel>();
            CreateMap<SearchTemplateEditViewModel, SearchTemplateModel>();
            #endregion

            #region StockTransferRequest

            CreateMap<StockTransferRequestCreateViewModel, StockTransferRequestModel>()
                .ForMember(d => d.Id, m => m.MapFrom(s => Guid.NewGuid()))
                .ForMember(d => d.CreateTime, m => m.MapFrom(s => DateTime.Now))
                .ForMember(d => d.IsDelete, m => m.MapFrom(s => false))
                .ForMember(d => d.Actived, m => m.MapFrom(s => true));
            CreateMap<StockTransferRequestDetailViewModel, StockTransferRequestDetailModel>()
                .ForMember(d => d.Id, m => m.MapFrom(s => Guid.NewGuid()))
                .ForMember(d => d.TransferredQuantity, m => m.MapFrom(s => 0))
                .ForMember(d => d.Id, m => m.MapFrom(s => Guid.NewGuid()))
                .ForMember(d => d.IsDeleted, m => m.MapFrom(s => false));
            CreateMap<StockTransferRequestModel, StockTransferRequestViewModel>()
               .ForMember(d => d.FromStockName, m => m.MapFrom(s => s.FromStockNavigation.StockName))
                .ForMember(d => d.FromStockCode, m => m.MapFrom(s => s.FromStockNavigation.StockCode))
               .ForMember(d => d.ToStockName, m => m.MapFrom(s => s.ToStockNavigation.StockName))
               .ForMember(d => d.ToStockCode, m => m.MapFrom(s => s.ToStockNavigation.StockCode))
               .ForMember(d => d.CompanyName, m => m.MapFrom(s => s.Company.CompanyName))
               .ForMember(d => d.StoreName, m => m.MapFrom(s => s.Store.StoreName))
               .ForMember(d => d.CreateByName, m => m.MapFrom(s => s.CreateByNavigation.UserName))
               .ForMember(d => d.LastEditByName, m => m.MapFrom(s => s.LastEditByNavigation != null ? s.LastEditByNavigation.UserName : s.CreateByNavigation.UserName))
               .ForMember(d => d.LastEditTime, m => m.MapFrom(s => s.LastEditByNavigation != null ? s.LastEditTime : s.CreateTime))
               .ForMember(d => d.DocumentDate, m => m.MapFrom(s => s.DocumentDate.Value.ToString("yyyy-MM-dd")))
               .ForMember(d => d.FromPlanDate, m => m.MapFrom(s => s.FromPlanDate != null ? s.FromPlanDate.Value.ToString("yyyy-MM-dd") : null))
               .ForMember(d => d.ToPlanDate, m => m.MapFrom(s => s.ToPlanDate != null ? s.ToPlanDate.Value.ToString("yyyy-MM-dd") : null));
            CreateMap<StockTransferRequestDetailModel, StockTransferRequestDetailViewModel>()
               .ForMember(d => d.ProductName, m => m.MapFrom(s => s.Product.ProductName))
               .ForMember(d => d.RemainingQuantity, m => m.MapFrom(s => s.RequestQuantity-s.TransferredQuantity))
               .ForMember(d => d.ProductCode, m => m.MapFrom(s => s.Product.ProductCode));
            CreateMap<StockTransferRequestUpdateViewModel, StockTransferRequestModel>();
            #endregion

            #region StockTransfer
            CreateMap<StockTransferRequestModel, TransferModel>()
               .ForMember(d => d.CreateTime, m => m.MapFrom(s => DateTime.Now))
               .ForMember(d => d.IsDeleted, m => m.MapFrom(s => false))
               .ForMember(d => d.StockTransferRequestId, m => m.MapFrom(s => s.Id));
            CreateMap<StockTransferCreateViewModel, TransferModel>()
                .ForMember(d => d.TransferId, m => m.MapFrom(s => Guid.NewGuid()));
            CreateMap<StockTransferDetailViewModel, TransferDetailModel>()
                .ForMember(d => d.TransferDetailId, m => m.MapFrom(s => Guid.NewGuid()))
                .ForMember(d=>d.Quantity,m=>m.MapFrom(s=>s.TransferQuantity))
                .ForMember(d => d.IsDeleted, m => m.MapFrom(s => false));
            #endregion

            #region TemplateAndGiftTargetGroup
            CreateMap<TemplateAndGiftTargetGroupModel, TemplateAndGiftTargetGroupViewModels>()
                .ForMember(d => d.CreateBy, m => m.MapFrom(s => s.CreateByNavigation.UserName))
                 .ForMember(d => d.LastEditTime, m => m.MapFrom(s => s.LastEditByNavigation != null ? s.LastEditTime : s.CreateTime))
                .ForMember(d => d.LastEditBy, m => m.MapFrom(s => s.LastEditByNavigation != null ? s.LastEditByNavigation.UserName : s.CreateByNavigation.UserName));
            CreateMap<TargetGroupEditViewModel, TemplateAndGiftTargetGroupModel>()
                .ForMember(d => d.LastEditTime, m => m.MapFrom(d => DateTime.Now));
            CreateMap<TargetGroupCreateViewModel, TemplateAndGiftTargetGroupModel>()
                .ForMember(d => d.Id, m => m.MapFrom(d => Guid.NewGuid()))
                .ForMember(d => d.CreateTime, m => m.MapFrom(d => DateTime.Now))
                .ForMember(d => d.Actived, m => m.MapFrom(s => true));
            CreateMap<ProfileModel, TemplateAndGiftMemberViewModel>();

            #endregion
            #region TemplateAndGiftCampaign
            CreateMap<TemplateAndGiftCampaignCreateModel, TemplateAndGiftCampaignModel>()
              .ForMember(d => d.Id, m => m.MapFrom(d => Guid.NewGuid()))
              .ForMember(d => d.CreateTime, m => m.MapFrom(d => DateTime.Now));
            CreateMap<TemplateAndGiftCampaignEditModel, TemplateAndGiftCampaignModel>()
              .ForMember(d => d.LastEditTime, m => m.MapFrom(d => DateTime.Now));
            CreateMap<TemplateAndGiftCampaignModel, TemplateAndGiftCampaignViewModel>()
                 .ForMember(d => d.CreateBy, m => m.MapFrom(s => s.CreateByNavigation.UserName))
                 .ForMember(d => d.LastEditTime, m => m.MapFrom(s => s.LastEditByNavigation != null ? s.LastEditTime : s.CreateTime))
                .ForMember(d => d.LastEditBy, m => m.MapFrom(s => s.LastEditByNavigation != null ? s.LastEditByNavigation.UserName : s.CreateByNavigation.UserName));
            #endregion

            #region TemplateAndGiftMemberAddress
            CreateMap<TemplateAndGiftMemberAddressViewModel, TemplateAndGiftMemberAddressModel>()
              .ForMember(d => d.Id, m => m.MapFrom(d => Guid.NewGuid()))
              .ForMember(d => d.CreateTime, m => m.MapFrom(d => DateTime.Now));
            //CreateMap<TemplateAndGiftCampaignModel, TemplateAndGiftCampaignViewModel>()
            //     .ForMember(d => d.CreateBy, m => m.MapFrom(s => s.CreateByNavigation.UserName))
            //     .ForMember(d => d.LastEditTime, m => m.MapFrom(s => s.LastEditByNavigation != null ? s.LastEditTime : s.CreateTime))
            //    .ForMember(d => d.LastEditBy, m => m.MapFrom(s => s.LastEditByNavigation != null ? s.LastEditByNavigation.UserName : s.CreateByNavigation.UserName));
            #endregion
        }
    }
}
