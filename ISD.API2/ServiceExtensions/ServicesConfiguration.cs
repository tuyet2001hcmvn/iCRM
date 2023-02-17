using ISD.API.Core;
using ISD.API.EntityModels.Entities;
using ISD.API.Repositories;
using ISD.API.Repositories.Marketing;
using ISD.API.Repositories.Services;
using ISD.API.Repositories.Services.Marketing;
using ISD.API.Repositories.Services.MasterData;
using ISD.API.Repositories.Services.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace ISD.API2.ServiceExtensions
{
    public static class ServicesConfiguration
	{
		public static void AddCustomServices(this IServiceCollection services)
		{
			services.AddScoped<ITargetGroupService, TargetGroupService>();
			services.AddScoped<IMemberOfTargetGroupService, MemberOfTargetGroupService>();
			services.AddScoped<IContentService, ContentService>();		
			services.AddScoped<IProfileRepository, ProfileRepository>();
			services.AddScoped<IStoreService, StoreService>();
			services.AddScoped<IEmailAccountService, EmailAccountService>();
			services.AddScoped<ICampaignService, CampaignService>();
			services.AddScoped<IEmailService, EmailService>();
			services.AddScoped<IUnfollowService, UnfollowService>();
			services.AddScoped<IQuestionBankService, QuestionBankService>();
			services.AddScoped<IFavoriteReportService, FavoriteReportService>();
			services.AddScoped<ISearchTemplateService, SearchTemplateService>();
			services.AddScoped<IStockTransferRequestService, StockTransferRequestService>();
			services.AddScoped<ITransferService, TransferService>();
			services.AddScoped<ITemplateAndGiftTargetGroupService, TemplateAndGiftTargetGroupService>();
			services.AddScoped<ITemplateAndGiftCampaignService, TemplateAndGiftCampaignService>();
			services.AddScoped<ITemplateAndGiftMemberAddressService, TemplateAndGiftMemberAddressService>();
			services.AddScoped<IProfileService, ProfileService>();
			services.AddScoped<IEmailHelper, EmailHelper>();
		}	
		public static void AddRepositories(this IServiceCollection services)
		{
			services.AddScoped<ITargetGroupRepository, TargetGroupRepository>();
			services.AddScoped<IMemberOfTargetGroupRepository, MemberOfTargetGroupRepository>();
			services.AddScoped<GenericRepository<TargetGroupModel>, TargetGroupRepository>();
			services.AddScoped<GenericRepository<ProfileModel>, ProfileRepository>();
			services.AddScoped<GenericRepository<MemberOfTargetGroupModel>, MemberOfTargetGroupRepository>();
			services.AddScoped<GenericRepository<ContentModel>, ContentRepository>();
		}
		public static void AddUnitOfwork(this IServiceCollection services)
		{		
			services.AddScoped<UnitOfWork>();
		}
	}
}
