//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ISD.EntityModels
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class EntityDataContext : DbContext
    {
        public EntityDataContext()
            : base("name=EntityDataContext")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AddressBookModel> AddressBookModel { get; set; }
        public virtual DbSet<CertificateACModel> CertificateACModel { get; set; }
        public virtual DbSet<Competitor_Industry_Mapping> Competitor_Industry_Mapping { get; set; }
        public virtual DbSet<CreditLimitModel> CreditLimitModel { get; set; }
        public virtual DbSet<CustomerTastes_CollectionModel> CustomerTastes_CollectionModel { get; set; }
        public virtual DbSet<CustomerTastes_ColorToneModel> CustomerTastes_ColorToneModel { get; set; }
        public virtual DbSet<CustomerTastes_ProductGroupModel> CustomerTastes_ProductGroupModel { get; set; }
        public virtual DbSet<CustomerTastes_WoodGrainModel> CustomerTastes_WoodGrainModel { get; set; }
        public virtual DbSet<CustomerTastesModel> CustomerTastesModel { get; set; }
        public virtual DbSet<ExistProfileModel> ExistProfileModel { get; set; }
        public virtual DbSet<FileAttachmentModel> FileAttachmentModel { get; set; }
        public virtual DbSet<PartnerModel> PartnerModel { get; set; }
        public virtual DbSet<PersonInChargeDeletedModel> PersonInChargeDeletedModel { get; set; }
        public virtual DbSet<PersonInChargeModel> PersonInChargeModel { get; set; }
        public virtual DbSet<Profile_Catalog_Mapping> Profile_Catalog_Mapping { get; set; }
        public virtual DbSet<Profile_File_Mapping> Profile_File_Mapping { get; set; }
        public virtual DbSet<Profile_Opportunity_CompetitorDetailModel> Profile_Opportunity_CompetitorDetailModel { get; set; }
        public virtual DbSet<Profile_Opportunity_CompetitorModel> Profile_Opportunity_CompetitorModel { get; set; }
        public virtual DbSet<Profile_Opportunity_InternalModel> Profile_Opportunity_InternalModel { get; set; }
        public virtual DbSet<Profile_Opportunity_MaterialModel> Profile_Opportunity_MaterialModel { get; set; }
        public virtual DbSet<Profile_Opportunity_PartnerModel> Profile_Opportunity_PartnerModel { get; set; }
        public virtual DbSet<Profile_OpportunityStatus_Mapping> Profile_OpportunityStatus_Mapping { get; set; }
        public virtual DbSet<Profile_ProfileType_Mapping> Profile_ProfileType_Mapping { get; set; }
        public virtual DbSet<ProfileBAttributeModel> ProfileBAttributeModel { get; set; }
        public virtual DbSet<ProfileCareerModel> ProfileCareerModel { get; set; }
        public virtual DbSet<ProfileCategoryModel> ProfileCategoryModel { get; set; }
        public virtual DbSet<ProfileCAttributeModel> ProfileCAttributeModel { get; set; }
        public virtual DbSet<ProfileConfigModel> ProfileConfigModel { get; set; }
        public virtual DbSet<ProfileContactAttributeDeletedModel> ProfileContactAttributeDeletedModel { get; set; }
        public virtual DbSet<ProfileContactAttributeModel> ProfileContactAttributeModel { get; set; }
        public virtual DbSet<ProfileDeletedModel> ProfileDeletedModel { get; set; }
        public virtual DbSet<ProfileEmailDeletedModel> ProfileEmailDeletedModel { get; set; }
        public virtual DbSet<ProfileEmailModel> ProfileEmailModel { get; set; }
        public virtual DbSet<ProfileFieldModel> ProfileFieldModel { get; set; }
        public virtual DbSet<ProfileGroupModel> ProfileGroupModel { get; set; }
        public virtual DbSet<ProfileLevelModel> ProfileLevelModel { get; set; }
        public virtual DbSet<ProfileModel> ProfileModel { get; set; }
        public virtual DbSet<ProfilePhoneDeletedModel> ProfilePhoneDeletedModel { get; set; }
        public virtual DbSet<ProfilePhoneModel> ProfilePhoneModel { get; set; }
        public virtual DbSet<ProfileTypeModel> ProfileTypeModel { get; set; }
        public virtual DbSet<RegisterReceiveNewsModel> RegisterReceiveNewsModel { get; set; }
        public virtual DbSet<RoleInChargeDeletedModel> RoleInChargeDeletedModel { get; set; }
        public virtual DbSet<RoleInChargeModel> RoleInChargeModel { get; set; }
        public virtual DbSet<SMSModel> SMSModel { get; set; }
        public virtual DbSet<SponsModel> SponsModel { get; set; }
        public virtual DbSet<ApplicationConfig> ApplicationConfig { get; set; }
        public virtual DbSet<ApplicationLog> ApplicationLog { get; set; }
        public virtual DbSet<DAS_EMAILTEMPLATE> DAS_EMAILTEMPLATE { get; set; }
        public virtual DbSet<FaceCheckInOutModel> FaceCheckInOutModel { get; set; }
        public virtual DbSet<LastRunCheckInOutModel> LastRunCheckInOutModel { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<AccessoryPriceModel> AccessoryPriceModel { get; set; }
        public virtual DbSet<CareerModel> CareerModel { get; set; }
        public virtual DbSet<CollectingAuthorityModel> CollectingAuthorityModel { get; set; }
        public virtual DbSet<ContainerRequirementModel> ContainerRequirementModel { get; set; }
        public virtual DbSet<CustomerModel> CustomerModel { get; set; }
        public virtual DbSet<ExternalMaterialGroupModel> ExternalMaterialGroupModel { get; set; }
        public virtual DbSet<GH_NotificationModel> GH_NotificationModel { get; set; }
        public virtual DbSet<LaborModel> LaborModel { get; set; }
        public virtual DbSet<MaterialAccessoryMappingModel> MaterialAccessoryMappingModel { get; set; }
        public virtual DbSet<MaterialFreightGroupModel> MaterialFreightGroupModel { get; set; }
        public virtual DbSet<MaterialGroupModel> MaterialGroupModel { get; set; }
        public virtual DbSet<MaterialInvoicePriceModel> MaterialInvoicePriceModel { get; set; }
        public virtual DbSet<MaterialMinMaxPriceModel> MaterialMinMaxPriceModel { get; set; }
        public virtual DbSet<MaterialModel> MaterialModel { get; set; }
        public virtual DbSet<MaterialPriceModel> MaterialPriceModel { get; set; }
        public virtual DbSet<MaterialPropertiesModel> MaterialPropertiesModel { get; set; }
        public virtual DbSet<MaterialRegistrationFeePriceModel> MaterialRegistrationFeePriceModel { get; set; }
        public virtual DbSet<MaterialServiceModel> MaterialServiceModel { get; set; }
        public virtual DbSet<MaterialSpecificationsModel> MaterialSpecificationsModel { get; set; }
        public virtual DbSet<MobileScreenModel> MobileScreenModel { get; set; }
        public virtual DbSet<NotificationAccountMappingModel> NotificationAccountMappingModel { get; set; }
        public virtual DbSet<NotificationModel> NotificationModel { get; set; }
        public virtual DbSet<PaymentMethodModel> PaymentMethodModel { get; set; }
        public virtual DbSet<PaymentNationalBudgetModel> PaymentNationalBudgetModel { get; set; }
        public virtual DbSet<ProductHierarchyModel> ProductHierarchyModel { get; set; }
        public virtual DbSet<ProfitCenterModel> ProfitCenterModel { get; set; }
        public virtual DbSet<PromotionBuyMaterialModel> PromotionBuyMaterialModel { get; set; }
        public virtual DbSet<PromotionByStoreModel> PromotionByStoreModel { get; set; }
        public virtual DbSet<PromotionGiftAccessoryModel> PromotionGiftAccessoryModel { get; set; }
        public virtual DbSet<ProspectModel> ProspectModel { get; set; }
        public virtual DbSet<ServiceTypeModel> ServiceTypeModel { get; set; }
        public virtual DbSet<StateTreasuryModel> StateTreasuryModel { get; set; }
        public virtual DbSet<TaxConfigModel> TaxConfigModel { get; set; }
        public virtual DbSet<TemperatureConditionModel> TemperatureConditionModel { get; set; }
        public virtual DbSet<AccessorySaleOrderDetailModel> AccessorySaleOrderDetailModel { get; set; }
        public virtual DbSet<AccessorySaleOrderModel> AccessorySaleOrderModel { get; set; }
        public virtual DbSet<AccessorySellTypeModel> AccessorySellTypeModel { get; set; }
        public virtual DbSet<SaleOrderDetailModel> SaleOrderDetailModel { get; set; }
        public virtual DbSet<SaleOrderMasterModel> SaleOrderMasterModel { get; set; }
        public virtual DbSet<BookingModel> BookingModel { get; set; }
        public virtual DbSet<CheckingTimesNotificationModel> CheckingTimesNotificationModel { get; set; }
        public virtual DbSet<ClaimAccessoryLogModel> ClaimAccessoryLogModel { get; set; }
        public virtual DbSet<ClaimAccessoryModel> ClaimAccessoryModel { get; set; }
        public virtual DbSet<ClaimAccessoryStatusModel> ClaimAccessoryStatusModel { get; set; }
        public virtual DbSet<FixingTypeModel> FixingTypeModel { get; set; }
        public virtual DbSet<ServiceAppointmentModel> ServiceAppointmentModel { get; set; }
        public virtual DbSet<ServiceFlagModel> ServiceFlagModel { get; set; }
        public virtual DbSet<ServiceOrderConsultModel> ServiceOrderConsultModel { get; set; }
        public virtual DbSet<ServiceOrderDetailAccessoryModel> ServiceOrderDetailAccessoryModel { get; set; }
        public virtual DbSet<ServiceOrderDetailModel> ServiceOrderDetailModel { get; set; }
        public virtual DbSet<ServiceOrderDetailServiceModel> ServiceOrderDetailServiceModel { get; set; }
        public virtual DbSet<ServiceOrderModel> ServiceOrderModel { get; set; }
        public virtual DbSet<ServiceOrderPoolModel> ServiceOrderPoolModel { get; set; }
        public virtual DbSet<ServiceOrderTypeModel> ServiceOrderTypeModel { get; set; }
        public virtual DbSet<VehicleInfoModel> VehicleInfoModel { get; set; }
        public virtual DbSet<WorkingDateModel> WorkingDateModel { get; set; }
        public virtual DbSet<WorkingTimeConfigModel> WorkingTimeConfigModel { get; set; }
        public virtual DbSet<WorkingTimeDetailModel> WorkingTimeDetailModel { get; set; }
        public virtual DbSet<WorkingTimeModel> WorkingTimeModel { get; set; }
        public virtual DbSet<ProductWarrantyModel> ProductWarrantyModel { get; set; }
        public virtual DbSet<WarrantyModel> WarrantyModel { get; set; }
        public virtual DbSet<CampaignModel> CampaignModel { get; set; }
        public virtual DbSet<ContentModel> ContentModel { get; set; }
        public virtual DbSet<EmailAccountModel> EmailAccountModel { get; set; }
        public virtual DbSet<MailServerProviderModel> MailServerProviderModel { get; set; }
        public virtual DbSet<MemberOfExternalProfileTargetGroupModel> MemberOfExternalProfileTargetGroupModel { get; set; }
        public virtual DbSet<MemberOfTargetGroupModel> MemberOfTargetGroupModel { get; set; }
        public virtual DbSet<ProductPromotionContactModel> ProductPromotionContactModel { get; set; }
        public virtual DbSet<ProductPromotionDetailModel> ProductPromotionDetailModel { get; set; }
        public virtual DbSet<ProductPromotionModel> ProductPromotionModel { get; set; }
        public virtual DbSet<SendMailCalendarModel> SendMailCalendarModel { get; set; }
        public virtual DbSet<TargetGroupModel> TargetGroupModel { get; set; }
        public virtual DbSet<TemplateAndGiftCampaignModel> TemplateAndGiftCampaignModel { get; set; }
        public virtual DbSet<TemplateAndGiftMemberAddressModel> TemplateAndGiftMemberAddressModel { get; set; }
        public virtual DbSet<TemplateAndGiftMemberModel> TemplateAndGiftMemberModel { get; set; }
        public virtual DbSet<TemplateAndGiftTargetGroupModel> TemplateAndGiftTargetGroupModel { get; set; }
        public virtual DbSet<Unfollow> Unfollow { get; set; }
        public virtual DbSet<Account_Device_Mapping> Account_Device_Mapping { get; set; }
        public virtual DbSet<AccountModel> AccountModel { get; set; }
        public virtual DbSet<FunctionModel> FunctionModel { get; set; }
        public virtual DbSet<MenuModel> MenuModel { get; set; }
        public virtual DbSet<MobileScreenPermissionModel> MobileScreenPermissionModel { get; set; }
        public virtual DbSet<ModuleModel> ModuleModel { get; set; }
        public virtual DbSet<Page_Module_Mapping> Page_Module_Mapping { get; set; }
        public virtual DbSet<PageModel> PageModel { get; set; }
        public virtual DbSet<PagePermissionModel> PagePermissionModel { get; set; }
        public virtual DbSet<ResourceModel> ResourceModel { get; set; }
        public virtual DbSet<RolesModel> RolesModel { get; set; }
        public virtual DbSet<AppointmentModel> AppointmentModel { get; set; }
        public virtual DbSet<AutoConditionModel> AutoConditionModel { get; set; }
        public virtual DbSet<CheckInOutModel> CheckInOutModel { get; set; }
        public virtual DbSet<Comment_File_Mapping> Comment_File_Mapping { get; set; }
        public virtual DbSet<RatingModel> RatingModel { get; set; }
        public virtual DbSet<RemindTaskModel> RemindTaskModel { get; set; }
        public virtual DbSet<StatusTransition_Task_Mapping> StatusTransition_Task_Mapping { get; set; }
        public virtual DbSet<StatusTransitionModel> StatusTransitionModel { get; set; }
        public virtual DbSet<Survey_Mapping> Survey_Mapping { get; set; }
        public virtual DbSet<Task_Event_Mapping> Task_Event_Mapping { get; set; }
        public virtual DbSet<Task_File_Mapping> Task_File_Mapping { get; set; }
        public virtual DbSet<TaskAssignModel> TaskAssignModel { get; set; }
        public virtual DbSet<TaskCommentModel> TaskCommentModel { get; set; }
        public virtual DbSet<TaskContactModel> TaskContactModel { get; set; }
        public virtual DbSet<TaskGroupDetailModel> TaskGroupDetailModel { get; set; }
        public virtual DbSet<TaskGroupModel> TaskGroupModel { get; set; }
        public virtual DbSet<TaskModel> TaskModel { get; set; }
        public virtual DbSet<TaskProductAccessoryModel> TaskProductAccessoryModel { get; set; }
        public virtual DbSet<TaskProductModel> TaskProductModel { get; set; }
        public virtual DbSet<TaskProductUsualErrorModel> TaskProductUsualErrorModel { get; set; }
        public virtual DbSet<TaskReferenceModel> TaskReferenceModel { get; set; }
        public virtual DbSet<TaskReporterModel> TaskReporterModel { get; set; }
        public virtual DbSet<TaskRoleInChargeModel> TaskRoleInChargeModel { get; set; }
        public virtual DbSet<TaskStatusModel> TaskStatusModel { get; set; }
        public virtual DbSet<TaskSurveyAnswerModel> TaskSurveyAnswerModel { get; set; }
        public virtual DbSet<TaskSurveyModel> TaskSurveyModel { get; set; }
        public virtual DbSet<WorkFlowCategoryModel> WorkFlowCategoryModel { get; set; }
        public virtual DbSet<WorkFlowConfigModel> WorkFlowConfigModel { get; set; }
        public virtual DbSet<WorkFlowFieldModel> WorkFlowFieldModel { get; set; }
        public virtual DbSet<WorkFlowModel> WorkFlowModel { get; set; }
        public virtual DbSet<AboutModel> AboutModel { get; set; }
        public virtual DbSet<APIModel> APIModel { get; set; }
        public virtual DbSet<BangTinModel> BangTinModel { get; set; }
        public virtual DbSet<BannerModel> BannerModel { get; set; }
        public virtual DbSet<CatalogModel> CatalogModel { get; set; }
        public virtual DbSet<CatalogTypeModel> CatalogTypeModel { get; set; }
        public virtual DbSet<CompanyModel> CompanyModel { get; set; }
        public virtual DbSet<ContactDetailModel> ContactDetailModel { get; set; }
        public virtual DbSet<ContactModel> ContactModel { get; set; }
        public virtual DbSet<CustomerGiftDetailModel> CustomerGiftDetailModel { get; set; }
        public virtual DbSet<CustomerGiftModel> CustomerGiftModel { get; set; }
        public virtual DbSet<CustomerLevelModel> CustomerLevelModel { get; set; }
        public virtual DbSet<CustomerPromotionModel> CustomerPromotionModel { get; set; }
        public virtual DbSet<DepartmentModel> DepartmentModel { get; set; }
        public virtual DbSet<DistrictModel> DistrictModel { get; set; }
        public virtual DbSet<EmailConfig> EmailConfig { get; set; }
        public virtual DbSet<EmailTemplateConfigModel> EmailTemplateConfigModel { get; set; }
        public virtual DbSet<FavoriteReportModel> FavoriteReportModel { get; set; }
        public virtual DbSet<Kanban_TaskStatus_Mapping> Kanban_TaskStatus_Mapping { get; set; }
        public virtual DbSet<KanbanDetailModel> KanbanDetailModel { get; set; }
        public virtual DbSet<KanbanModel> KanbanModel { get; set; }
        public virtual DbSet<News_Company_Mapping> News_Company_Mapping { get; set; }
        public virtual DbSet<NewsCategoryModel> NewsCategoryModel { get; set; }
        public virtual DbSet<NewsModel> NewsModel { get; set; }
        public virtual DbSet<PrognosisModel> PrognosisModel { get; set; }
        public virtual DbSet<PromotionModel> PromotionModel { get; set; }
        public virtual DbSet<ProvinceModel> ProvinceModel { get; set; }
        public virtual DbSet<QuestionBankModel> QuestionBankModel { get; set; }
        public virtual DbSet<RelatedNews_Mapping> RelatedNews_Mapping { get; set; }
        public virtual DbSet<RequestEccEmailConfigModel> RequestEccEmailConfigModel { get; set; }
        public virtual DbSet<SaleProcessModel> SaleProcessModel { get; set; }
        public virtual DbSet<SalesEmployeeModel> SalesEmployeeModel { get; set; }
        public virtual DbSet<SaleUnitModel> SaleUnitModel { get; set; }
        public virtual DbSet<SourceModel> SourceModel { get; set; }
        public virtual DbSet<StoreModel> StoreModel { get; set; }
        public virtual DbSet<StoreTypeModel> StoreTypeModel { get; set; }
        public virtual DbSet<SurveyDetailModel> SurveyDetailModel { get; set; }
        public virtual DbSet<SurveyModel> SurveyModel { get; set; }
        public virtual DbSet<WardModel> WardModel { get; set; }
        public virtual DbSet<AccessoryCategoryModel> AccessoryCategoryModel { get; set; }
        public virtual DbSet<AccessoryDetailModel> AccessoryDetailModel { get; set; }
        public virtual DbSet<AccessoryModel> AccessoryModel { get; set; }
        public virtual DbSet<AccessoryProductModel> AccessoryProductModel { get; set; }
        public virtual DbSet<CategoryModel> CategoryModel { get; set; }
        public virtual DbSet<ColorModel> ColorModel { get; set; }
        public virtual DbSet<ColorProductModel> ColorProductModel { get; set; }
        public virtual DbSet<ConfigurationModel> ConfigurationModel { get; set; }
        public virtual DbSet<ImageProductModel> ImageProductModel { get; set; }
        public virtual DbSet<PeriodicallyCheckingModel> PeriodicallyCheckingModel { get; set; }
        public virtual DbSet<PlateFeeDetailModel> PlateFeeDetailModel { get; set; }
        public virtual DbSet<PlateFeeModel> PlateFeeModel { get; set; }
        public virtual DbSet<PriceProductModel> PriceProductModel { get; set; }
        public virtual DbSet<ProductAttributeModel> ProductAttributeModel { get; set; }
        public virtual DbSet<ProductModel> ProductModel { get; set; }
        public virtual DbSet<ProductTypeModel> ProductTypeModel { get; set; }
        public virtual DbSet<PropertiesProductModel> PropertiesProductModel { get; set; }
        public virtual DbSet<ShowroomCategoryModel> ShowroomCategoryModel { get; set; }
        public virtual DbSet<SpecificationsModel> SpecificationsModel { get; set; }
        public virtual DbSet<SpecificationsProductModel> SpecificationsProductModel { get; set; }
        public virtual DbSet<StyleModel> StyleModel { get; set; }
        public virtual DbSet<WarehouseModel> WarehouseModel { get; set; }
        public virtual DbSet<WarehouseProductModel> WarehouseProductModel { get; set; }
        public virtual DbSet<ChangeDataLogModel> ChangeDataLogModel { get; set; }
        public virtual DbSet<HistoryModel> HistoryModel { get; set; }
        public virtual DbSet<SearchResultDetailTemplateModel> SearchResultDetailTemplateModel { get; set; }
        public virtual DbSet<SearchResultTemplateModel> SearchResultTemplateModel { get; set; }
        public virtual DbSet<SearchTemplateModel> SearchTemplateModel { get; set; }
        public virtual DbSet<DeliveryDetailModel> DeliveryDetailModel { get; set; }
        public virtual DbSet<DeliveryModel> DeliveryModel { get; set; }
        public virtual DbSet<DimDateModel> DimDateModel { get; set; }
        public virtual DbSet<InventoryModel> InventoryModel { get; set; }
        public virtual DbSet<Stock_Store_Mapping> Stock_Store_Mapping { get; set; }
        public virtual DbSet<StockModel> StockModel { get; set; }
        public virtual DbSet<StockOnHand> StockOnHand { get; set; }
        public virtual DbSet<StockReceivingDetailModel> StockReceivingDetailModel { get; set; }
        public virtual DbSet<StockReceivingMasterModel> StockReceivingMasterModel { get; set; }
        public virtual DbSet<StockTransferRequestDetailModel> StockTransferRequestDetailModel { get; set; }
        public virtual DbSet<StockTransferRequestModel> StockTransferRequestModel { get; set; }
        public virtual DbSet<TransferDetailModel> TransferDetailModel { get; set; }
        public virtual DbSet<TransferModel> TransferModel { get; set; }
        public virtual DbSet<Profile_ExtendInfo> Profile_ExtendInfo { get; set; }
        public virtual DbSet<DeliveryTempModel> DeliveryTempModel { get; set; }
        public virtual DbSet<View_Profile_Address> View_Profile_Address { get; set; }
        public virtual DbSet<View_Profile_ContactPhone> View_Profile_ContactPhone { get; set; }
        public virtual DbSet<View_Profile_ExtendInfo> View_Profile_ExtendInfo { get; set; }
        public virtual DbSet<View_Profile_MainContact> View_Profile_MainContact { get; set; }
        public virtual DbSet<View_ProfileDeleted_ContactPhone> View_ProfileDeleted_ContactPhone { get; set; }
        public virtual DbSet<View_Task_Area> View_Task_Area { get; set; }
        public virtual DbSet<View_Catalog_Category> View_Catalog_Category { get; set; }
        public virtual DbSet<View_Task_GTB> View_Task_GTB { get; set; }
        public virtual DbSet<View_Product_ProductInfo> View_Product_ProductInfo { get; set; }
        public virtual DbSet<View_Stock_Delivery> View_Stock_Delivery { get; set; }
        public virtual DbSet<View_Stock_Receive> View_Stock_Receive { get; set; }
        public virtual DbSet<View_Stock_TransferFrom_Delivery> View_Stock_TransferFrom_Delivery { get; set; }
        public virtual DbSet<View_Stock_TransferTo_Receive> View_Stock_TransferTo_Receive { get; set; }
    }
}
