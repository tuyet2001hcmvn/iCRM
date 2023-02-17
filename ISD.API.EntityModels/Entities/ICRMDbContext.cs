using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    public partial class ICRMDbContext : DbContext
    {
        public ICRMDbContext()
        {
        }

        public ICRMDbContext(DbContextOptions<ICRMDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AboutModel> AboutModels { get; set; }
        public virtual DbSet<AccessoryCategoryModel> AccessoryCategoryModels { get; set; }
        public virtual DbSet<AccessoryDetailModel> AccessoryDetailModels { get; set; }
        public virtual DbSet<AccessoryModel> AccessoryModels { get; set; }
        public virtual DbSet<AccessoryPriceModel> AccessoryPriceModels { get; set; }
        public virtual DbSet<AccessoryProductModel> AccessoryProductModels { get; set; }
        public virtual DbSet<AccessorySaleOrderDetailModel> AccessorySaleOrderDetailModels { get; set; }
        public virtual DbSet<AccessorySaleOrderModel> AccessorySaleOrderModels { get; set; }
        public virtual DbSet<AccessorySellTypeModel> AccessorySellTypeModels { get; set; }
        public virtual DbSet<AccountDeviceMapping> AccountDeviceMappings { get; set; }
        public virtual DbSet<AccountInRoleModel> AccountInRoleModels { get; set; }
        public virtual DbSet<AccountInStoreModel> AccountInStoreModels { get; set; }
        public virtual DbSet<AccountModel> AccountModels { get; set; }
        public virtual DbSet<AddressBookModel> AddressBookModels { get; set; }
        public virtual DbSet<Apimodel> Apimodels { get; set; }
        public virtual DbSet<ApplicationConfig> ApplicationConfigs { get; set; }
        public virtual DbSet<ApplicationLog> ApplicationLogs { get; set; }
        public virtual DbSet<AppointmentModel> AppointmentModels { get; set; }
        public virtual DbSet<AutoConditionModel> AutoConditionModels { get; set; }
        public virtual DbSet<BangTinModel> BangTinModels { get; set; }
        public virtual DbSet<BannerModel> BannerModels { get; set; }
        public virtual DbSet<BookingModel> BookingModels { get; set; }
        public virtual DbSet<CampaignModel> CampaignModels { get; set; }
        public virtual DbSet<CareerModel> CareerModels { get; set; }
        public virtual DbSet<CatalogModel> CatalogModels { get; set; }
        public virtual DbSet<CatalogTypeModel> CatalogTypeModels { get; set; }
        public virtual DbSet<CategoryModel> CategoryModels { get; set; }
        public virtual DbSet<CertificateAcmodel> CertificateAcmodels { get; set; }
        public virtual DbSet<ChangeDataLogModel> ChangeDataLogModels { get; set; }
        public virtual DbSet<CheckInOutModel> CheckInOutModels { get; set; }
        public virtual DbSet<CheckingTimesNotificationModel> CheckingTimesNotificationModels { get; set; }
        public virtual DbSet<ClaimAccessoryLogModel> ClaimAccessoryLogModels { get; set; }
        public virtual DbSet<ClaimAccessoryModel> ClaimAccessoryModels { get; set; }
        public virtual DbSet<ClaimAccessoryStatusModel> ClaimAccessoryStatusModels { get; set; }
        public virtual DbSet<CollectingAuthorityModel> CollectingAuthorityModels { get; set; }
        public virtual DbSet<ColorModel> ColorModels { get; set; }
        public virtual DbSet<ColorProductModel> ColorProductModels { get; set; }
        public virtual DbSet<CommentFileMapping> CommentFileMappings { get; set; }
        public virtual DbSet<CompanyModel> CompanyModels { get; set; }
        public virtual DbSet<CompetitorIndustryMapping> CompetitorIndustryMappings { get; set; }
        public virtual DbSet<ConfigurationModel> ConfigurationModels { get; set; }
        public virtual DbSet<ContactDetailModel> ContactDetailModels { get; set; }
        public virtual DbSet<ContactModel> ContactModels { get; set; }
        public virtual DbSet<ContainerRequirementModel> ContainerRequirementModels { get; set; }
        public virtual DbSet<ContentModel> ContentModels { get; set; }
        public virtual DbSet<CreditLimitModel> CreditLimitModels { get; set; }
        public virtual DbSet<CustomerGiftDetailModel> CustomerGiftDetailModels { get; set; }
        public virtual DbSet<CustomerGiftModel> CustomerGiftModels { get; set; }
        public virtual DbSet<CustomerLevelModel> CustomerLevelModels { get; set; }
        public virtual DbSet<CustomerModel> CustomerModels { get; set; }
        public virtual DbSet<CustomerPromotionModel> CustomerPromotionModels { get; set; }
        public virtual DbSet<CustomerPromotionProductModel> CustomerPromotionProductModels { get; set; }
        public virtual DbSet<CustomerTastesCollectionModel> CustomerTastesCollectionModels { get; set; }
        public virtual DbSet<CustomerTastesColorToneModel> CustomerTastesColorToneModels { get; set; }
        public virtual DbSet<CustomerTastesModel> CustomerTastesModels { get; set; }
        public virtual DbSet<CustomerTastesProductGroupModel> CustomerTastesProductGroupModels { get; set; }
        public virtual DbSet<CustomerTastesWoodGrainModel> CustomerTastesWoodGrainModels { get; set; }
        public virtual DbSet<DeliveryDetailModel> DeliveryDetailModels { get; set; }
        public virtual DbSet<DeliveryDetailTempModel> DeliveryDetailTempModels { get; set; }
        public virtual DbSet<DeliveryModel> DeliveryModels { get; set; }
        public virtual DbSet<DeliveryTempModel> DeliveryTempModels { get; set; }
        public virtual DbSet<DepartmentModel> DepartmentModels { get; set; }
        public virtual DbSet<DimDateModel> DimDateModels { get; set; }
        public virtual DbSet<DistrictModel> DistrictModels { get; set; }
        public virtual DbSet<EmailAccountModel> EmailAccountModels { get; set; }
        public virtual DbSet<EmailConfig> EmailConfigs { get; set; }
        public virtual DbSet<EmailTemplateConfigModel> EmailTemplateConfigModels { get; set; }
        public virtual DbSet<ExistProfileModel> ExistProfileModels { get; set; }
        public virtual DbSet<ExternalMaterialGroupModel> ExternalMaterialGroupModels { get; set; }
        public virtual DbSet<FaceCheckInOutModel> FaceCheckInOutModels { get; set; }
        public virtual DbSet<FavoriteReportModel> FavoriteReportModels { get; set; }
        public virtual DbSet<FileAttachmentModel> FileAttachmentModels { get; set; }
        public virtual DbSet<FixingTypeModel> FixingTypeModels { get; set; }
        public virtual DbSet<FunctionModel> FunctionModels { get; set; }
        public virtual DbSet<GhNotificationModel> GhNotificationModels { get; set; }
        public virtual DbSet<HistoryModel> HistoryModels { get; set; }
        public virtual DbSet<ImageProductModel> ImageProductModels { get; set; }
        public virtual DbSet<InventoryModel> InventoryModels { get; set; }
        public virtual DbSet<KanbanDetailModel> KanbanDetailModels { get; set; }
        public virtual DbSet<KanbanModel> KanbanModels { get; set; }
        public virtual DbSet<KanbanTaskStatusMapping> KanbanTaskStatusMappings { get; set; }
        public virtual DbSet<LaborModel> LaborModels { get; set; }
        public virtual DbSet<LastRunCheckInOutModel> LastRunCheckInOutModels { get; set; }
        public virtual DbSet<MailServerProviderModel> MailServerProviderModels { get; set; }
        public virtual DbSet<MaterialAccessoryMappingModel> MaterialAccessoryMappingModels { get; set; }
        public virtual DbSet<MaterialFreightGroupModel> MaterialFreightGroupModels { get; set; }
        public virtual DbSet<MaterialGroupModel> MaterialGroupModels { get; set; }
        public virtual DbSet<MaterialInvoicePriceModel> MaterialInvoicePriceModels { get; set; }
        public virtual DbSet<MaterialMinMaxPriceBySaleOrgModel> MaterialMinMaxPriceBySaleOrgModels { get; set; }
        public virtual DbSet<MaterialMinMaxPriceModel> MaterialMinMaxPriceModels { get; set; }
        public virtual DbSet<MaterialModel> MaterialModels { get; set; }
        public virtual DbSet<MaterialPriceModel> MaterialPriceModels { get; set; }
        public virtual DbSet<MaterialPropertiesModel> MaterialPropertiesModels { get; set; }
        public virtual DbSet<MaterialRegistrationFeePriceModel> MaterialRegistrationFeePriceModels { get; set; }
        public virtual DbSet<MaterialServiceMappingModel> MaterialServiceMappingModels { get; set; }
        public virtual DbSet<MaterialServiceModel> MaterialServiceModels { get; set; }
        public virtual DbSet<MaterialSpecificationsModel> MaterialSpecificationsModels { get; set; }
        public virtual DbSet<MemberOfExternalProfileTargetGroupModel> MemberOfExternalProfileTargetGroupModels { get; set; }
        public virtual DbSet<MemberOfTargetGroupModel> MemberOfTargetGroupModels { get; set; }
        public virtual DbSet<MenuModel> MenuModels { get; set; }
        public virtual DbSet<MobileScreenFunctionModel> MobileScreenFunctionModels { get; set; }
        public virtual DbSet<MobileScreenModel> MobileScreenModels { get; set; }
        public virtual DbSet<MobileScreenPermissionModel> MobileScreenPermissionModels { get; set; }
        public virtual DbSet<ModuleModel> ModuleModels { get; set; }
        public virtual DbSet<NewsCategoryModel> NewsCategoryModels { get; set; }
        public virtual DbSet<NewsCompanyMapping> NewsCompanyMappings { get; set; }
        public virtual DbSet<NewsModel> NewsModels { get; set; }
        public virtual DbSet<NotificationAccountMappingModel> NotificationAccountMappingModels { get; set; }
        public virtual DbSet<NotificationModel> NotificationModels { get; set; }
        public virtual DbSet<PageFunctionModel> PageFunctionModels { get; set; }
        public virtual DbSet<PageModel> PageModels { get; set; }
        public virtual DbSet<PageModuleMapping> PageModuleMappings { get; set; }
        public virtual DbSet<PagePermissionModel> PagePermissionModels { get; set; }
        public virtual DbSet<PartnerModel> PartnerModels { get; set; }
        public virtual DbSet<PaymentMethodModel> PaymentMethodModels { get; set; }
        public virtual DbSet<PaymentNationalBudgetModel> PaymentNationalBudgetModels { get; set; }
        public virtual DbSet<PeriodicallyCheckingModel> PeriodicallyCheckingModels { get; set; }
        public virtual DbSet<PersonInChargeDeletedModel> PersonInChargeDeletedModels { get; set; }
        public virtual DbSet<PersonInChargeModel> PersonInChargeModels { get; set; }
        public virtual DbSet<PlateFeeDetailModel> PlateFeeDetailModels { get; set; }
        public virtual DbSet<PlateFeeModel> PlateFeeModels { get; set; }
        public virtual DbSet<PriceProductModel> PriceProductModels { get; set; }
        public virtual DbSet<ProductAttributeModel> ProductAttributeModels { get; set; }
        public virtual DbSet<ProductHierarchyModel> ProductHierarchyModels { get; set; }
        public virtual DbSet<ProductModel> ProductModels { get; set; }
        public virtual DbSet<ProductPeriodicallyCheckingMapping> ProductPeriodicallyCheckingMappings { get; set; }
        public virtual DbSet<ProductPlateFeeMapping> ProductPlateFeeMappings { get; set; }
        public virtual DbSet<ProductPromotionContactModel> ProductPromotionContactModels { get; set; }
        public virtual DbSet<ProductPromotionDetailModel> ProductPromotionDetailModels { get; set; }
        public virtual DbSet<ProductPromotionModel> ProductPromotionModels { get; set; }
        public virtual DbSet<ProductTypeModel> ProductTypeModels { get; set; }
        public virtual DbSet<ProductWarrantyModel> ProductWarrantyModels { get; set; }
        public virtual DbSet<ProfileAddressTempModel> ProfileAddressTempModels { get; set; }
        public virtual DbSet<ProfileBattributeModel> ProfileBattributeModels { get; set; }
        public virtual DbSet<ProfileCareerModel> ProfileCareerModels { get; set; }
        public virtual DbSet<ProfileCatalogMapping> ProfileCatalogMappings { get; set; }
        public virtual DbSet<ProfileCategoryModel> ProfileCategoryModels { get; set; }
        public virtual DbSet<ProfileCattributeModel> ProfileCattributeModels { get; set; }
        public virtual DbSet<ProfileConfigModel> ProfileConfigModels { get; set; }
        public virtual DbSet<ProfileContactAttributeDeletedModel> ProfileContactAttributeDeletedModels { get; set; }
        public virtual DbSet<ProfileContactAttributeModel> ProfileContactAttributeModels { get; set; }
        public virtual DbSet<ProfileContactTempModel> ProfileContactTempModels { get; set; }
        public virtual DbSet<ProfileDeletedModel> ProfileDeletedModels { get; set; }
        public virtual DbSet<ProfileEmailDeletedModel> ProfileEmailDeletedModels { get; set; }
        public virtual DbSet<ProfileEmailModel> ProfileEmailModels { get; set; }
        public virtual DbSet<ProfileExtendInfo> ProfileExtendInfos { get; set; }
        public virtual DbSet<ProfileFieldModel> ProfileFieldModels { get; set; }
        public virtual DbSet<ProfileFileMapping> ProfileFileMappings { get; set; }
        public virtual DbSet<ProfileGroupModel> ProfileGroupModels { get; set; }
        public virtual DbSet<ProfileLevelModel> ProfileLevelModels { get; set; }
        public virtual DbSet<ProfileModel> ProfileModels { get; set; }
        public virtual DbSet<ProfileNameTempModel> ProfileNameTempModels { get; set; }
        public virtual DbSet<ProfileOpportunityCompetitorDetailModel> ProfileOpportunityCompetitorDetailModels { get; set; }
        public virtual DbSet<ProfileOpportunityCompetitorModel> ProfileOpportunityCompetitorModels { get; set; }
        public virtual DbSet<ProfileOpportunityInternalModel> ProfileOpportunityInternalModels { get; set; }
        public virtual DbSet<ProfileOpportunityMaterialModel> ProfileOpportunityMaterialModels { get; set; }
        public virtual DbSet<ProfileOpportunityPartnerModel> ProfileOpportunityPartnerModels { get; set; }
        public virtual DbSet<ProfileOpportunityStatusMapping> ProfileOpportunityStatusMappings { get; set; }
        public virtual DbSet<ProfilePhoneDeletedModel> ProfilePhoneDeletedModels { get; set; }
        public virtual DbSet<ProfilePhoneModel> ProfilePhoneModels { get; set; }
        public virtual DbSet<ProfileProfileTypeMapping> ProfileProfileTypeMappings { get; set; }
        public virtual DbSet<ProfileTemp2Model> ProfileTemp2Models { get; set; }
        public virtual DbSet<ProfileTemp3Model> ProfileTemp3Models { get; set; }
        public virtual DbSet<ProfileTempModel> ProfileTempModels { get; set; }
        public virtual DbSet<ProfileTypeModel> ProfileTypeModels { get; set; }
        public virtual DbSet<ProfitCenterModel> ProfitCenterModels { get; set; }
        public virtual DbSet<PrognosisModel> PrognosisModels { get; set; }
        public virtual DbSet<PromotionBuyMaterialModel> PromotionBuyMaterialModels { get; set; }
        public virtual DbSet<PromotionByStoreModel> PromotionByStoreModels { get; set; }
        public virtual DbSet<PromotionGiftAccessoryModel> PromotionGiftAccessoryModels { get; set; }
        public virtual DbSet<PromotionModel> PromotionModels { get; set; }
        public virtual DbSet<PromotionProductModel> PromotionProductModels { get; set; }
        public virtual DbSet<PropertiesProductModel> PropertiesProductModels { get; set; }
        public virtual DbSet<ProspectModel> ProspectModels { get; set; }
        public virtual DbSet<ProvinceModel> ProvinceModels { get; set; }
        public virtual DbSet<QuestionBankModel> QuestionBankModels { get; set; }
        public virtual DbSet<RatingModel> RatingModels { get; set; }
        public virtual DbSet<RegisterReceiveNewsModel> RegisterReceiveNewsModels { get; set; }
        public virtual DbSet<RelatedNewsMapping> RelatedNewsMappings { get; set; }
        public virtual DbSet<RemindTaskModel> RemindTaskModels { get; set; }
        public virtual DbSet<RequestEccEmailConfigModel> RequestEccEmailConfigModels { get; set; }
        public virtual DbSet<ResourceModel> ResourceModels { get; set; }
        public virtual DbSet<RoleInChargeDeletedModel> RoleInChargeDeletedModels { get; set; }
        public virtual DbSet<RoleInChargeModel> RoleInChargeModels { get; set; }
        public virtual DbSet<RolesModel> RolesModels { get; set; }
        public virtual DbSet<SaleOrderDetailModel> SaleOrderDetailModels { get; set; }
        public virtual DbSet<SaleOrderMasterModel> SaleOrderMasterModels { get; set; }
        public virtual DbSet<SaleProcessModel> SaleProcessModels { get; set; }
        public virtual DbSet<SaleUnitModel> SaleUnitModels { get; set; }
        public virtual DbSet<SalesEmployeeModel> SalesEmployeeModels { get; set; }
        public virtual DbSet<SearchResultDetailTemplateModel> SearchResultDetailTemplateModels { get; set; }
        public virtual DbSet<SearchResultTemplateModel> SearchResultTemplateModels { get; set; }
        public virtual DbSet<SearchTemplateModel> SearchTemplateModels { get; set; }
        public virtual DbSet<SendMailCalendarModel> SendMailCalendarModels { get; set; }
        public virtual DbSet<ServiceAppointmentModel> ServiceAppointmentModels { get; set; }
        public virtual DbSet<ServiceFlagModel> ServiceFlagModels { get; set; }
        public virtual DbSet<ServiceOrderConsultModel> ServiceOrderConsultModels { get; set; }
        public virtual DbSet<ServiceOrderDetailAccessoryModel> ServiceOrderDetailAccessoryModels { get; set; }
        public virtual DbSet<ServiceOrderDetailModel> ServiceOrderDetailModels { get; set; }
        public virtual DbSet<ServiceOrderDetailServiceModel> ServiceOrderDetailServiceModels { get; set; }
        public virtual DbSet<ServiceOrderModel> ServiceOrderModels { get; set; }
        public virtual DbSet<ServiceOrderPoolModel> ServiceOrderPoolModels { get; set; }
        public virtual DbSet<ServiceOrderTypeModel> ServiceOrderTypeModels { get; set; }
        public virtual DbSet<ServiceTypeModel> ServiceTypeModels { get; set; }
        public virtual DbSet<ShowroomCategoryModel> ShowroomCategoryModels { get; set; }
        public virtual DbSet<Smsmodel> Smsmodels { get; set; }
        public virtual DbSet<SourceModel> SourceModels { get; set; }
        public virtual DbSet<SpecificationsModel> SpecificationsModels { get; set; }
        public virtual DbSet<SpecificationsProductModel> SpecificationsProductModels { get; set; }
        public virtual DbSet<StateTreasuryModel> StateTreasuryModels { get; set; }
        public virtual DbSet<StatusTransitionModel> StatusTransitionModels { get; set; }
        public virtual DbSet<StatusTransitionTaskMapping> StatusTransitionTaskMappings { get; set; }
        public virtual DbSet<StockModel> StockModels { get; set; }
        public virtual DbSet<StockReceivingDetailModel> StockReceivingDetailModels { get; set; }
        public virtual DbSet<StockReceivingMasterModel> StockReceivingMasterModels { get; set; }
        public virtual DbSet<StockStoreMapping> StockStoreMappings { get; set; }
        public virtual DbSet<StockTransferRequestDetailModel> StockTransferRequestDetailModels { get; set; }
        public virtual DbSet<StockTransferRequestModel> StockTransferRequestModels { get; set; }
        public virtual DbSet<StoreModel> StoreModels { get; set; }
        public virtual DbSet<StoreTypeModel> StoreTypeModels { get; set; }
        public virtual DbSet<StyleModel> StyleModels { get; set; }
        public virtual DbSet<SurveyDetailModel> SurveyDetailModels { get; set; }
        public virtual DbSet<SurveyMapping> SurveyMappings { get; set; }
        public virtual DbSet<SurveyModel> SurveyModels { get; set; }
        public virtual DbSet<TargetGroupModel> TargetGroupModels { get; set; }
        public virtual DbSet<TaskAssignModel> TaskAssignModels { get; set; }
        public virtual DbSet<TaskCommentModel> TaskCommentModels { get; set; }
        public virtual DbSet<TaskContactModel> TaskContactModels { get; set; }
        public virtual DbSet<TaskEventMapping> TaskEventMappings { get; set; }
        public virtual DbSet<TaskFileMapping> TaskFileMappings { get; set; }
        public virtual DbSet<TaskGroupDetailModel> TaskGroupDetailModels { get; set; }
        public virtual DbSet<TaskGroupModel> TaskGroupModels { get; set; }
        public virtual DbSet<TaskGtbTempModel> TaskGtbTempModels { get; set; }
        public virtual DbSet<TaskModel> TaskModels { get; set; }
        public virtual DbSet<TaskProductAccessoryModel> TaskProductAccessoryModels { get; set; }
        public virtual DbSet<TaskProductModel> TaskProductModels { get; set; }
        public virtual DbSet<TaskProductUsualErrorModel> TaskProductUsualErrorModels { get; set; }
        public virtual DbSet<TaskReferenceModel> TaskReferenceModels { get; set; }
        public virtual DbSet<TaskReporterModel> TaskReporterModels { get; set; }
        public virtual DbSet<TaskRoleInChargeModel> TaskRoleInChargeModels { get; set; }
        public virtual DbSet<TaskStatusModel> TaskStatusModels { get; set; }
        public virtual DbSet<TaskSurveyAnswerModel> TaskSurveyAnswerModels { get; set; }
        public virtual DbSet<TaskSurveyModel> TaskSurveyModels { get; set; }
        public virtual DbSet<TaxConfigModel> TaxConfigModels { get; set; }
        public virtual DbSet<TemperatureConditionModel> TemperatureConditionModels { get; set; }
        public virtual DbSet<TemplateAndGiftCampaignModel> TemplateAndGiftCampaignModels { get; set; }
        public virtual DbSet<TemplateAndGiftMemberAddressModel> TemplateAndGiftMemberAddressModels { get; set; }
        public virtual DbSet<TemplateAndGiftMemberModel> TemplateAndGiftMemberModels { get; set; }
        public virtual DbSet<TemplateAndGiftTargetGroupModel> TemplateAndGiftTargetGroupModels { get; set; }
        public virtual DbSet<TransferDetailModel> TransferDetailModels { get; set; }
        public virtual DbSet<TransferModel> TransferModels { get; set; }
        public virtual DbSet<Unfollow> Unfollows { get; set; }
        public virtual DbSet<VehicleInfoModel> VehicleInfoModels { get; set; }
        public virtual DbSet<ViewCatalogCategory> ViewCatalogCategories { get; set; }
        public virtual DbSet<ViewFaceCheckIn> ViewFaceCheckIns { get; set; }
        public virtual DbSet<ViewFaceCheckOut> ViewFaceCheckOuts { get; set; }
        public virtual DbSet<ViewPriorityModel> ViewPriorityModels { get; set; }
        public virtual DbSet<ViewProductProductInfo> ViewProductProductInfos { get; set; }
        public virtual DbSet<ViewProfileAddress> ViewProfileAddresses { get; set; }
        public virtual DbSet<ViewProfileCompanyMapping> ViewProfileCompanyMappings { get; set; }
        public virtual DbSet<ViewProfileContactPhone> ViewProfileContactPhones { get; set; }
        public virtual DbSet<ViewProfileDeletedContactPhone> ViewProfileDeletedContactPhones { get; set; }
        public virtual DbSet<ViewProfileExtendInfo> ViewProfileExtendInfos { get; set; }
        public virtual DbSet<ViewProfileMainContact> ViewProfileMainContacts { get; set; }
        public virtual DbSet<ViewProfileProfilePhone> ViewProfileProfilePhones { get; set; }
        public virtual DbSet<ViewProfileProjectContractLoseValue> ViewProfileProjectContractLoseValues { get; set; }
        public virtual DbSet<ViewProfileProjectContractValue> ViewProfileProjectContractValues { get; set; }
        public virtual DbSet<ViewProfileProjectContractWonValue> ViewProfileProjectContractWonValues { get; set; }
        public virtual DbSet<ViewStockDelivery> ViewStockDeliveries { get; set; }
        public virtual DbSet<ViewStockReceive> ViewStockReceives { get; set; }
        public virtual DbSet<ViewStockTransferFromDelivery> ViewStockTransferFromDeliveries { get; set; }
        public virtual DbSet<ViewStockTransferToReceive> ViewStockTransferToReceives { get; set; }
        public virtual DbSet<ViewTaskArea> ViewTaskAreas { get; set; }
        public virtual DbSet<ViewTaskGtb> ViewTaskGtbs { get; set; }
        public virtual DbSet<WardModel> WardModels { get; set; }
        public virtual DbSet<WarehouseModel> WarehouseModels { get; set; }
        public virtual DbSet<WarehouseProductModel> WarehouseProductModels { get; set; }
        public virtual DbSet<WarrantyModel> WarrantyModels { get; set; }
        public virtual DbSet<WorkFlowCategoryModel> WorkFlowCategoryModels { get; set; }
        public virtual DbSet<WorkFlowConfigModel> WorkFlowConfigModels { get; set; }
        public virtual DbSet<WorkFlowFieldModel> WorkFlowFieldModels { get; set; }
        public virtual DbSet<WorkFlowModel> WorkFlowModels { get; set; }
        public virtual DbSet<WorkingDateModel> WorkingDateModels { get; set; }
        public virtual DbSet<WorkingTimeConfigModel> WorkingTimeConfigModels { get; set; }
        public virtual DbSet<WorkingTimeDetailModel> WorkingTimeDetailModels { get; set; }
        public virtual DbSet<WorkingTimeModel> WorkingTimeModels { get; set; }
        public virtual DbSet<StockOnHand> StockOnHands { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //optionsBuilder.UseSqlServer("Server=192.168.100.233;Database=ISD_iCRM_Dev;User Id=sa;Password=123@abcd;");
                optionsBuilder.UseSqlServer("Server=172.18.13.2;Database=ISD_iCRM_GoLive_Test;User Id=icrm_user; Password=123@abcd;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<AboutModel>(entity =>
            {
                entity.Property(e => e.AboutId).ValueGeneratedNever();
            });

            modelBuilder.Entity<AccessoryCategoryModel>(entity =>
            {
                entity.Property(e => e.AccessoryCategoryId).ValueGeneratedNever();
            });

            modelBuilder.Entity<AccessoryDetailModel>(entity =>
            {
                entity.Property(e => e.AccessoryDetailId).ValueGeneratedNever();

                entity.HasOne(d => d.Accessory)
                    .WithMany(p => p.AccessoryDetailModels)
                    .HasForeignKey(d => d.AccessoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AccessoryDetailModel_AccessoryModel");
            });

            modelBuilder.Entity<AccessoryModel>(entity =>
            {
                entity.Property(e => e.AccessoryId).ValueGeneratedNever();
            });

            modelBuilder.Entity<AccessoryPriceModel>(entity =>
            {
                entity.Property(e => e.AccessoryPriceId).HasDefaultValueSql("(newid())");
            });

            modelBuilder.Entity<AccessoryProductModel>(entity =>
            {
                entity.Property(e => e.AccessoryProductId).ValueGeneratedNever();

                entity.HasOne(d => d.Accessory)
                    .WithMany(p => p.AccessoryProductModels)
                    .HasForeignKey(d => d.AccessoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AccessoryProductModel_AccessoryModel");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.AccessoryProductModels)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AccessoryProductModel_ProductModel");
            });

            modelBuilder.Entity<AccessorySaleOrderDetailModel>(entity =>
            {
                entity.Property(e => e.AccessorySaleOrderDetailId).ValueGeneratedNever();

                entity.HasOne(d => d.AccessorySaleOrder)
                    .WithMany(p => p.AccessorySaleOrderDetailModels)
                    .HasForeignKey(d => d.AccessorySaleOrderId)
                    .HasConstraintName("FK_AccessorySaleOrderDetailModel_AccessorySaleOrderModel");
            });

            modelBuilder.Entity<AccessorySaleOrderModel>(entity =>
            {
                entity.Property(e => e.AccessorySaleOrderId).ValueGeneratedNever();

                entity.Property(e => e.GeneratedCode).ValueGeneratedOnAdd();

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.AccessorySaleOrderModels)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_AccessorySaleOrderModel_CustomerModel");

                entity.HasOne(d => d.Vehicle)
                    .WithMany(p => p.AccessorySaleOrderModels)
                    .HasForeignKey(d => d.VehicleId)
                    .HasConstraintName("FK_AccessorySaleOrderModel_VehicleInfoModel");
            });

            modelBuilder.Entity<AccessorySellTypeModel>(entity =>
            {
                entity.Property(e => e.AccessorySellTypeId).ValueGeneratedNever();

                entity.Property(e => e.Actived).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<AccountDeviceMapping>(entity =>
            {
                entity.HasKey(e => new { e.AccountId, e.DeviceId });
            });

            modelBuilder.Entity<AccountInRoleModel>(entity =>
            {
                entity.HasKey(e => new { e.AccountId, e.RolesId });

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.AccountInRoleModels)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AccountInRoleModel_AccountModel");

                entity.HasOne(d => d.Roles)
                    .WithMany(p => p.AccountInRoleModels)
                    .HasForeignKey(d => d.RolesId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AccountInRoleModel_RolesModel");
            });

            modelBuilder.Entity<AccountInStoreModel>(entity =>
            {
                entity.HasKey(e => new { e.AccountId, e.StoreId });

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.AccountInStoreModels)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AccountInStoreModel_AccountModel");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.AccountInStoreModels)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AccountInStoreModel_StoreModel");
            });

            modelBuilder.Entity<AccountModel>(entity =>
            {
                entity.Property(e => e.AccountId).ValueGeneratedNever();
            });

            modelBuilder.Entity<AddressBookModel>(entity =>
            {
                entity.Property(e => e.AddressBookId).ValueGeneratedNever();

                entity.HasOne(d => d.Profile)
                    .WithMany(p => p.AddressBookModels)
                    .HasForeignKey(d => d.ProfileId)
                    .HasConstraintName("FK_AddressBookModel_ProfileModel");
            });

            modelBuilder.Entity<ApplicationLog>(entity =>
            {
                entity.Property(e => e.ApplicationLogId).HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.PerformedByAccount)
                    .WithMany(p => p.ApplicationLogs)
                    .HasForeignKey(d => d.PerformedByAccountId)
                    .HasConstraintName("FK_ApplicationLog_AccountModel");
            });

            modelBuilder.Entity<AppointmentModel>(entity =>
            {
                entity.Property(e => e.AppointmentId).ValueGeneratedNever();
            });

            modelBuilder.Entity<AutoConditionModel>(entity =>
            {
                entity.Property(e => e.AutoConditionId).ValueGeneratedNever();
            });

            modelBuilder.Entity<BangTinModel>(entity =>
            {
                entity.Property(e => e.NewsId).ValueGeneratedNever();

                entity.Property(e => e.NewsCode).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<BannerModel>(entity =>
            {
                entity.Property(e => e.BannerId).ValueGeneratedNever();
            });

            modelBuilder.Entity<BookingModel>(entity =>
            {
                entity.Property(e => e.BookingModelId).ValueGeneratedNever();

                entity.Property(e => e.BookingCode).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<CampaignModel>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CampaignCode).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.Property(e => e.Type).HasComment("Marketing|Event");

                entity.HasOne(d => d.StatusNavigation)
                    .WithMany(p => p.CampaignModels)
                    .HasForeignKey(d => d.Status)
                    .HasConstraintName("FK_CampaignModel_CatalogModel");
            });

            modelBuilder.Entity<CareerModel>(entity =>
            {
                entity.Property(e => e.CareerId).ValueGeneratedNever();
            });

            modelBuilder.Entity<CatalogModel>(entity =>
            {
                entity.Property(e => e.CatalogId).HasDefaultValueSql("(newid())");
            });

            modelBuilder.Entity<CategoryModel>(entity =>
            {
                entity.HasKey(e => e.CategoryId)
                    .HasName("PK_Category2Model");

                entity.Property(e => e.CategoryId).ValueGeneratedNever();
            });

            modelBuilder.Entity<CertificateAcmodel>(entity =>
            {
                entity.Property(e => e.CertificateId).ValueGeneratedNever();
            });

            modelBuilder.Entity<ChangeDataLogModel>(entity =>
            {
                entity.HasKey(e => e.LogId)
                    .HasName("PK_ChangeDataLog");

                entity.Property(e => e.LogId).ValueGeneratedNever();
            });

            modelBuilder.Entity<CheckInOutModel>(entity =>
            {
                entity.Property(e => e.CheckInOutId).ValueGeneratedNever();
            });

            modelBuilder.Entity<CheckingTimesNotificationModel>(entity =>
            {
                entity.Property(e => e.CheckingTimesId).HasDefaultValueSql("(newid())");
            });

            modelBuilder.Entity<ClaimAccessoryLogModel>(entity =>
            {
                entity.Property(e => e.ClaimAccessoryLogId).ValueGeneratedNever();
            });

            modelBuilder.Entity<ClaimAccessoryModel>(entity =>
            {
                entity.Property(e => e.ClaimAccessoryId).ValueGeneratedNever();
            });

            modelBuilder.Entity<ClaimAccessoryStatusModel>(entity =>
            {
                entity.Property(e => e.StatusId).ValueGeneratedNever();
            });

            modelBuilder.Entity<CollectingAuthorityModel>(entity =>
            {
                entity.Property(e => e.CollectingAuthorityId).ValueGeneratedNever();
            });

            modelBuilder.Entity<ColorModel>(entity =>
            {
                entity.Property(e => e.ColorId).ValueGeneratedNever();
            });

            modelBuilder.Entity<ColorProductModel>(entity =>
            {
                entity.Property(e => e.ColorProductId).ValueGeneratedNever();

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ColorProductModels)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ColorProductModel_ProductModel");

                entity.HasOne(d => d.Style)
                    .WithMany(p => p.ColorProductModels)
                    .HasForeignKey(d => d.StyleId)
                    .HasConstraintName("FK_ColorProductModel_StyleModel");
            });

            modelBuilder.Entity<CommentFileMapping>(entity =>
            {
                entity.HasKey(e => new { e.TaskCommentId, e.FileAttachmentId });

                entity.HasOne(d => d.FileAttachment)
                    .WithMany(p => p.CommentFileMappings)
                    .HasForeignKey(d => d.FileAttachmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Comment_File_Mapping_FileAttachmentModel");

                entity.HasOne(d => d.TaskComment)
                    .WithMany(p => p.CommentFileMappings)
                    .HasForeignKey(d => d.TaskCommentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Comment_File_Mapping_TaskCommentModel");
            });

            modelBuilder.Entity<CompanyModel>(entity =>
            {
                entity.Property(e => e.CompanyId).ValueGeneratedNever();
            });

            modelBuilder.Entity<CompetitorIndustryMapping>(entity =>
            {
                entity.Property(e => e.CompetitorIndustryId).ValueGeneratedNever();
            });

            modelBuilder.Entity<ConfigurationModel>(entity =>
            {
                entity.Property(e => e.ConfigurationId).ValueGeneratedNever();
            });

            modelBuilder.Entity<ContactDetailModel>(entity =>
            {
                entity.Property(e => e.ContactDetailId).ValueGeneratedNever();
            });

            modelBuilder.Entity<ContactModel>(entity =>
            {
                entity.Property(e => e.ContactId).ValueGeneratedNever();
            });

            modelBuilder.Entity<ContainerRequirementModel>(entity =>
            {
                entity.HasKey(e => e.ContainerRequirementCode)
                    .HasName("PK_OptionModel");

                entity.Property(e => e.Actived).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<ContentModel>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ContentCode).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.Property(e => e.Type).HasComment("Marketing|Event");

                entity.HasOne(d => d.CreateByNavigation)
                    .WithMany(p => p.ContentModelCreateByNavigations)
                    .HasForeignKey(d => d.CreateBy)
                    .HasConstraintName("FK_ContentModel_AccountModel");

                entity.HasOne(d => d.LastEditByNavigation)
                    .WithMany(p => p.ContentModelLastEditByNavigations)
                    .HasForeignKey(d => d.LastEditBy)
                    .HasConstraintName("FK_ContentModel_AccountModel1");
            });

            modelBuilder.Entity<CreditLimitModel>(entity =>
            {
                entity.Property(e => e.CreditLimitId).ValueGeneratedNever();
            });

            modelBuilder.Entity<CustomerGiftDetailModel>(entity =>
            {
                entity.HasKey(e => new { e.GiftId, e.CustomerId });

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CustomerGiftDetailModels)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_CustomerGiftDetailModel_CustomerModel");

                entity.HasOne(d => d.Gift)
                    .WithMany(p => p.CustomerGiftDetailModels)
                    .HasForeignKey(d => d.GiftId)
                    .HasConstraintName("FK_CustomerGiftDetailModel_CustomerGiftModel");
            });

            modelBuilder.Entity<CustomerGiftModel>(entity =>
            {
                entity.Property(e => e.GiftId).ValueGeneratedNever();
            });

            modelBuilder.Entity<CustomerLevelModel>(entity =>
            {
                entity.Property(e => e.CustomerLevelId).ValueGeneratedNever();
            });

            modelBuilder.Entity<CustomerModel>(entity =>
            {
                entity.Property(e => e.CustomerId).ValueGeneratedNever();

                entity.Property(e => e.GeneratedCode).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<CustomerPromotionModel>(entity =>
            {
                entity.Property(e => e.PromotionId).ValueGeneratedNever();
            });

            modelBuilder.Entity<CustomerPromotionProductModel>(entity =>
            {
                entity.HasKey(e => new { e.PromotionId, e.ProductId });

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.CustomerPromotionProductModels)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CustomerPromotionProductModel_ProductModel");

                entity.HasOne(d => d.Promotion)
                    .WithMany(p => p.CustomerPromotionProductModels)
                    .HasForeignKey(d => d.PromotionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CustomerPromotionProductModel_CustomerPromotionModel");
            });

            modelBuilder.Entity<CustomerTastesCollectionModel>(entity =>
            {
                entity.HasKey(e => e.CollectionId)
                    .HasName("PK_Customer_Tastes_Collection_Model_1");

                entity.Property(e => e.CollectionId).ValueGeneratedNever();
            });

            modelBuilder.Entity<CustomerTastesColorToneModel>(entity =>
            {
                entity.HasKey(e => e.ColorToneId)
                    .HasName("PK_Customer_Tastes_ColorTone_Model_1");

                entity.Property(e => e.ColorToneId).ValueGeneratedNever();
            });

            modelBuilder.Entity<CustomerTastesModel>(entity =>
            {
                entity.HasKey(e => e.CustomerTasteId)
                    .HasName("PK_Customer_Tastes_Model_1");

                entity.Property(e => e.CustomerTasteId).ValueGeneratedNever();
            });

            modelBuilder.Entity<CustomerTastesProductGroupModel>(entity =>
            {
                entity.HasKey(e => e.ProductGroupId)
                    .HasName("PK_Customer_Tastes_ProductGroup_Model_1");

                entity.Property(e => e.ProductGroupId).ValueGeneratedNever();
            });

            modelBuilder.Entity<CustomerTastesWoodGrainModel>(entity =>
            {
                entity.HasKey(e => e.WoodGrainId)
                    .HasName("PK_Customer_Tastes_WoodGrain_Model_1");

                entity.Property(e => e.WoodGrainId).ValueGeneratedNever();
            });

            modelBuilder.Entity<DeliveryDetailModel>(entity =>
            {
                entity.Property(e => e.DeliveryDetailId).ValueGeneratedNever();

                entity.HasOne(d => d.DateKeyNavigation)
                    .WithMany(p => p.DeliveryDetailModels)
                    .HasForeignKey(d => d.DateKey)
                    .HasConstraintName("FK_DeliveryDetailModel_DimDate");

                entity.HasOne(d => d.Delivery)
                    .WithMany(p => p.DeliveryDetailModels)
                    .HasForeignKey(d => d.DeliveryId)
                    .HasConstraintName("FK_DeliveryDetailModel_DeliveryModel");

                entity.HasOne(d => d.Inventory)
                    .WithMany(p => p.DeliveryDetailModels)
                    .HasForeignKey(d => d.InventoryId)
                    .HasConstraintName("FK_DeliveryDetailModel_InventoryModel");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.DeliveryDetailModels)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_DeliveryDetailModel_ProductModel");

                entity.HasOne(d => d.Stock)
                    .WithMany(p => p.DeliveryDetailModels)
                    .HasForeignKey(d => d.StockId)
                    .HasConstraintName("FK_DeliveryDetailModel_StockModel");
            });

            modelBuilder.Entity<DeliveryModel>(entity =>
            {
                entity.Property(e => e.DeliveryId).ValueGeneratedNever();

                entity.Property(e => e.DeliveryCode).ValueGeneratedOnAdd();

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.DeliveryModels)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK_DeliveryModel_CompanyModel");

                entity.HasOne(d => d.Profile)
                    .WithMany(p => p.DeliveryModels)
                    .HasForeignKey(d => d.ProfileId)
                    .HasConstraintName("FK_DeliveryModel_ProfileModel");

                entity.HasOne(d => d.SalesEmployeeCodeNavigation)
                    .WithMany(p => p.DeliveryModels)
                    .HasForeignKey(d => d.SalesEmployeeCode)
                    .HasConstraintName("FK_DeliveryModel_SalesEmployeeModel");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.DeliveryModels)
                    .HasForeignKey(d => d.StoreId)
                    .HasConstraintName("FK_DeliveryModel_StoreModel");
            });

            modelBuilder.Entity<DepartmentModel>(entity =>
            {
                entity.Property(e => e.DepartmentId).ValueGeneratedNever();

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.DepartmentModels)
                    .HasForeignKey(d => d.StoreId)
                    .HasConstraintName("FK_DepartmentModel_StoreModel");
            });

            modelBuilder.Entity<DimDateModel>(entity =>
            {
                entity.HasKey(e => e.DateKey)
                    .HasName("PK__DimDateM__40DF45E33D4BCE98");

                entity.Property(e => e.DateKey).ValueGeneratedNever();

                entity.Property(e => e.DayName).IsUnicode(false);

                entity.Property(e => e.DayOfMonth).IsUnicode(false);

                entity.Property(e => e.DayOfQuarter).IsUnicode(false);

                entity.Property(e => e.DayOfWeekInMonth).IsUnicode(false);

                entity.Property(e => e.DayOfWeekInYear).IsUnicode(false);

                entity.Property(e => e.DayOfWeekUk)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.DayOfWeekUsa)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.DayOfYear).IsUnicode(false);

                entity.Property(e => e.DaySuffix).IsUnicode(false);

                entity.Property(e => e.FiscalDayOfYear).IsUnicode(false);

                entity.Property(e => e.FiscalMmyyyy)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.FiscalMonth).IsUnicode(false);

                entity.Property(e => e.FiscalMonthYear)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.FiscalQuarter)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.FiscalQuarterName).IsUnicode(false);

                entity.Property(e => e.FiscalWeekOfYear).IsUnicode(false);

                entity.Property(e => e.FiscalYear)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.FiscalYearName)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.FullDateUk)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.FullDateUsa)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.HolidayUk).IsUnicode(false);

                entity.Property(e => e.HolidayUsa).IsUnicode(false);

                entity.Property(e => e.Mmyyyy)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Month).IsUnicode(false);

                entity.Property(e => e.MonthName).IsUnicode(false);

                entity.Property(e => e.MonthOfQuarter).IsUnicode(false);

                entity.Property(e => e.MonthYear)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Quarter)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.QuarterName).IsUnicode(false);

                entity.Property(e => e.WeekOfMonth).IsUnicode(false);

                entity.Property(e => e.WeekOfQuarter).IsUnicode(false);

                entity.Property(e => e.WeekOfYear).IsUnicode(false);

                entity.Property(e => e.Year)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.YearName)
                    .IsUnicode(false)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<DistrictModel>(entity =>
            {
                entity.Property(e => e.DistrictId).ValueGeneratedNever();
            });

            modelBuilder.Entity<EmailAccountModel>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<EmailConfig>(entity =>
            {
                entity.Property(e => e.EmailConfigId).ValueGeneratedNever();
            });

            modelBuilder.Entity<EmailTemplateConfigModel>(entity =>
            {
                entity.Property(e => e.EmailTemplateConfigId).ValueGeneratedNever();
            });

            modelBuilder.Entity<ExistProfileModel>(entity =>
            {
                entity.HasKey(e => new { e.ProfileForeignCode, e.CompanyCode });
            });

            modelBuilder.Entity<ExternalMaterialGroupModel>(entity =>
            {
                entity.Property(e => e.Actived).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<FaceCheckInOutModel>(entity =>
            {
                entity.Property(e => e.FaceId).IsUnicode(false);

                entity.Property(e => e.Avatar).IsUnicode(false);

                entity.Property(e => e.PersonId).IsUnicode(false);

                entity.Property(e => e.PlaceId).IsUnicode(false);
            });

            modelBuilder.Entity<FavoriteReportModel>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.FavoriteReportModels)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FavoriteReportModel_AccountModel");

                entity.HasOne(d => d.Page)
                    .WithMany(p => p.FavoriteReportModels)
                    .HasForeignKey(d => d.PageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FavoriteReportModel_PageModel");
            });

            modelBuilder.Entity<FileAttachmentModel>(entity =>
            {
                entity.Property(e => e.FileAttachmentId).ValueGeneratedNever();
            });

            modelBuilder.Entity<FixingTypeModel>(entity =>
            {
                entity.Property(e => e.FixingTypeId).ValueGeneratedNever();

                entity.Property(e => e.Actived).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<GhNotificationModel>(entity =>
            {
                entity.Property(e => e.NotificationId).ValueGeneratedNever();
            });

            modelBuilder.Entity<HistoryModel>(entity =>
            {
                entity.Property(e => e.HistoryModifyId).ValueGeneratedNever();
            });

            modelBuilder.Entity<ImageProductModel>(entity =>
            {
                entity.Property(e => e.ImageId).ValueGeneratedNever();

                entity.HasOne(d => d.ColorProduct)
                    .WithMany(p => p.ImageProductModels)
                    .HasForeignKey(d => d.ColorProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ImageProductModel_ColorProductModel");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ImageProductModels)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ImageProductModel_ProductModel");
            });

            modelBuilder.Entity<InventoryModel>(entity =>
            {
                entity.HasKey(e => e.InventoryId)
                    .HasName("PK_Inventory");

                entity.Property(e => e.InventoryId).ValueGeneratedNever();

                entity.Property(e => e.InventoryCode).ValueGeneratedOnAdd();

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.InventoryModels)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK_InventoryModel_CompanyModel");

                entity.HasOne(d => d.SalesEmployeeCodeNavigation)
                    .WithMany(p => p.InventoryModels)
                    .HasForeignKey(d => d.SalesEmployeeCode)
                    .HasConstraintName("FK_InventoryModel_SalesEmployeeModel");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.InventoryModels)
                    .HasForeignKey(d => d.StoreId)
                    .HasConstraintName("FK_InventoryModel_StoreModel");
            });

            modelBuilder.Entity<KanbanDetailModel>(entity =>
            {
                entity.Property(e => e.KanbanDetailId).ValueGeneratedNever();

                entity.HasOne(d => d.Kanban)
                    .WithMany(p => p.KanbanDetailModels)
                    .HasForeignKey(d => d.KanbanId)
                    .HasConstraintName("FK_KanbanDetailModel_KanbanModel");
            });

            modelBuilder.Entity<KanbanModel>(entity =>
            {
                entity.Property(e => e.KanbanId).ValueGeneratedNever();
            });

            modelBuilder.Entity<KanbanTaskStatusMapping>(entity =>
            {
                entity.HasKey(e => new { e.KanbanDetailId, e.TaskStatusId })
                    .HasName("PK_Kanban_Task_Mapping");

                entity.HasOne(d => d.KanbanDetail)
                    .WithMany(p => p.KanbanTaskStatusMappings)
                    .HasForeignKey(d => d.KanbanDetailId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Kanban_TaskStatus_Mapping_KanbanDetailModel");

                entity.HasOne(d => d.TaskStatus)
                    .WithMany(p => p.KanbanTaskStatusMappings)
                    .HasForeignKey(d => d.TaskStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Kanban_TaskStatus_Mapping_TaskStatusModel");
            });

            modelBuilder.Entity<LaborModel>(entity =>
            {
                entity.Property(e => e.Actived).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<LastRunCheckInOutModel>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<MailServerProviderModel>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<MaterialAccessoryMappingModel>(entity =>
            {
                entity.HasKey(e => new { e.MaterialCode, e.AccessoryId });

                entity.HasOne(d => d.MaterialCodeNavigation)
                    .WithMany(p => p.MaterialAccessoryMappingModels)
                    .HasForeignKey(d => d.MaterialCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MaterialAccessoryMappingModel_MaterialModel");
            });

            modelBuilder.Entity<MaterialFreightGroupModel>(entity =>
            {
                entity.Property(e => e.Actived).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<MaterialGroupModel>(entity =>
            {
                entity.Property(e => e.Actived).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<MaterialInvoicePriceModel>(entity =>
            {
                entity.Property(e => e.MaterialPriceId).ValueGeneratedNever();

                entity.HasOne(d => d.MaterialCodeNavigation)
                    .WithMany(p => p.MaterialInvoicePriceModels)
                    .HasForeignKey(d => d.MaterialCode)
                    .HasConstraintName("FK_MaterialInvoicePriceModel_MaterialModel1");
            });

            modelBuilder.Entity<MaterialMinMaxPriceBySaleOrgModel>(entity =>
            {
                entity.HasKey(e => new { e.MaterialMinMaxPriceId, e.StoreId });

                entity.HasOne(d => d.MaterialMinMaxPrice)
                    .WithMany(p => p.MaterialMinMaxPriceBySaleOrgModels)
                    .HasForeignKey(d => d.MaterialMinMaxPriceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MaterialMinMaxPriceBySaleOrgModel_MaterialMinMaxPriceModel");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.MaterialMinMaxPriceBySaleOrgModels)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MaterialMinMaxPriceBySaleOrgModel_StoreModel");
            });

            modelBuilder.Entity<MaterialMinMaxPriceModel>(entity =>
            {
                entity.Property(e => e.MaterialMinMaxPriceId).ValueGeneratedNever();

                entity.HasOne(d => d.MaterialCodeNavigation)
                    .WithMany(p => p.MaterialMinMaxPriceModels)
                    .HasForeignKey(d => d.MaterialCode)
                    .HasConstraintName("FK_MaterialMinMaxPriceModel_MaterialModel");
            });

            modelBuilder.Entity<MaterialModel>(entity =>
            {
                entity.Property(e => e.Actived).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.ContainerRequirementCodeNavigation)
                    .WithMany(p => p.MaterialModels)
                    .HasForeignKey(d => d.ContainerRequirementCode)
                    .HasConstraintName("FK_MaterialModel_ContainerRequirementModel");

                entity.HasOne(d => d.ExternalMaterialGroupCodeNavigation)
                    .WithMany(p => p.MaterialModels)
                    .HasForeignKey(d => d.ExternalMaterialGroupCode)
                    .HasConstraintName("FK_MaterialModel_ExternalMaterialGroupModel");

                entity.HasOne(d => d.LaborCodeNavigation)
                    .WithMany(p => p.MaterialModels)
                    .HasForeignKey(d => d.LaborCode)
                    .HasConstraintName("FK_MaterialModel_LaborModel");

                entity.HasOne(d => d.MaterialFreightGroupCodeNavigation)
                    .WithMany(p => p.MaterialModels)
                    .HasForeignKey(d => d.MaterialFreightGroupCode)
                    .HasConstraintName("FK_MaterialModel_MaterialFreightGroupModel");

                entity.HasOne(d => d.MaterialGroupCodeNavigation)
                    .WithMany(p => p.MaterialModels)
                    .HasForeignKey(d => d.MaterialGroupCode)
                    .HasConstraintName("FK_MaterialModel_MaterialGroupModel");

                entity.HasOne(d => d.ProductHierarchyCodeNavigation)
                    .WithMany(p => p.MaterialModels)
                    .HasForeignKey(d => d.ProductHierarchyCode)
                    .HasConstraintName("FK_MaterialModel_ProductHierarchyModel");

                entity.HasOne(d => d.ProfitCenterCodeNavigation)
                    .WithMany(p => p.MaterialModels)
                    .HasForeignKey(d => d.ProfitCenterCode)
                    .HasConstraintName("FK_MaterialModel_ProfitCenterModel");

                entity.HasOne(d => d.TemperatureConditionCodeNavigation)
                    .WithMany(p => p.MaterialModels)
                    .HasForeignKey(d => d.TemperatureConditionCode)
                    .HasConstraintName("FK_MaterialModel_TemperatureConditionModel");
            });

            modelBuilder.Entity<MaterialPriceModel>(entity =>
            {
                entity.Property(e => e.MaterialPriceId).ValueGeneratedNever();

                entity.HasOne(d => d.MaterialCodeNavigation)
                    .WithMany(p => p.MaterialPriceModels)
                    .HasForeignKey(d => d.MaterialCode)
                    .HasConstraintName("FK_MaterialPriceModel_MaterialModel");
            });

            modelBuilder.Entity<MaterialPropertiesModel>(entity =>
            {
                entity.Property(e => e.PropertiesId).ValueGeneratedNever();
            });

            modelBuilder.Entity<MaterialRegistrationFeePriceModel>(entity =>
            {
                entity.Property(e => e.MaterialPriceId).ValueGeneratedNever();

                entity.HasOne(d => d.MaterialCodeNavigation)
                    .WithMany(p => p.MaterialRegistrationFeePriceModels)
                    .HasForeignKey(d => d.MaterialCode)
                    .HasConstraintName("FK_MaterialRegistrationFeePriceModel_MaterialModel1");
            });

            modelBuilder.Entity<MaterialServiceMappingModel>(entity =>
            {
                entity.HasKey(e => new { e.MaterialCode, e.MaterialNumber });

                entity.HasOne(d => d.MaterialCodeNavigation)
                    .WithMany(p => p.MaterialServiceMappingModels)
                    .HasForeignKey(d => d.MaterialCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MaterialServiceMappingModel_MaterialModel");

                entity.HasOne(d => d.MaterialNumberNavigation)
                    .WithMany(p => p.MaterialServiceMappingModels)
                    .HasForeignKey(d => d.MaterialNumber)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MaterialServiceMappingModel_MaterialServiceModel");
            });

            modelBuilder.Entity<MaterialSpecificationsModel>(entity =>
            {
                entity.Property(e => e.MaterialSpecificationsId).ValueGeneratedNever();
            });

            modelBuilder.Entity<MemberOfExternalProfileTargetGroupModel>(entity =>
            {
                entity.Property(e => e.ExternalProfileTargetGroupId).ValueGeneratedNever();

                entity.HasOne(d => d.TargetGroup)
                    .WithMany(p => p.MemberOfExternalProfileTargetGroupModels)
                    .HasForeignKey(d => d.TargetGroupId)
                    .HasConstraintName("FK_MemberOfExternalProfileTargetGroupModel_TargetGroupModel");
            });

            modelBuilder.Entity<MemberOfTargetGroupModel>(entity =>
            {
                entity.HasKey(e => new { e.TargetGroupId, e.ProfileId })
                    .HasName("PK_MemberOfTagetGroupModel");

                entity.HasOne(d => d.TargetGroup)
                    .WithMany(p => p.MemberOfTargetGroupModels)
                    .HasForeignKey(d => d.TargetGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MemberOfTagetGroupModel_TargetGroupModel");
            });

            modelBuilder.Entity<MenuModel>(entity =>
            {
                entity.Property(e => e.MenuId).ValueGeneratedNever();

                entity.HasOne(d => d.Module)
                    .WithMany(p => p.MenuModels)
                    .HasForeignKey(d => d.ModuleId)
                    .HasConstraintName("FK_MenuModel_ModuleModel");
            });

            modelBuilder.Entity<MobileScreenFunctionModel>(entity =>
            {
                entity.HasKey(e => new { e.MobileScreenId, e.FunctionId });

                entity.HasOne(d => d.Function)
                    .WithMany(p => p.MobileScreenFunctionModels)
                    .HasForeignKey(d => d.FunctionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MobileScreenFunctionModel_FunctionModel");

                entity.HasOne(d => d.MobileScreen)
                    .WithMany(p => p.MobileScreenFunctionModels)
                    .HasForeignKey(d => d.MobileScreenId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MobileScreenFunctionModel_MobileScreenModel");
            });

            modelBuilder.Entity<MobileScreenModel>(entity =>
            {
                entity.Property(e => e.MobileScreenId).HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.Menu)
                    .WithMany(p => p.MobileScreenModels)
                    .HasForeignKey(d => d.MenuId)
                    .HasConstraintName("FK_MobileScreenModel_MenuModel");
            });

            modelBuilder.Entity<MobileScreenPermissionModel>(entity =>
            {
                entity.HasKey(e => new { e.RolesId, e.MobileScreenId, e.FunctionId });

                entity.HasOne(d => d.Function)
                    .WithMany(p => p.MobileScreenPermissionModels)
                    .HasForeignKey(d => d.FunctionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MobileScreenPermissionModel_FunctionModel");

                entity.HasOne(d => d.MobileScreen)
                    .WithMany(p => p.MobileScreenPermissionModels)
                    .HasForeignKey(d => d.MobileScreenId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MobileScreenPermissionModel_MobileScreenModel");

                entity.HasOne(d => d.Roles)
                    .WithMany(p => p.MobileScreenPermissionModels)
                    .HasForeignKey(d => d.RolesId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MobileScreenPermissionModel_RolesModel");
            });

            modelBuilder.Entity<ModuleModel>(entity =>
            {
                entity.Property(e => e.ModuleId).ValueGeneratedNever();
            });

            modelBuilder.Entity<NewsCategoryModel>(entity =>
            {
                entity.Property(e => e.NewsCategoryId).ValueGeneratedNever();

                entity.Property(e => e.NewsCategoryCode).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<NewsCompanyMapping>(entity =>
            {
                entity.HasKey(e => new { e.NewsId, e.CompanyId });
            });

            modelBuilder.Entity<NewsModel>(entity =>
            {
                entity.Property(e => e.NewsId).ValueGeneratedNever();

                entity.Property(e => e.NewsCode).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<NotificationAccountMappingModel>(entity =>
            {
                entity.HasKey(e => new { e.NotificationId, e.AccountId });

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.NotificationAccountMappingModels)
                    .HasForeignKey(d => d.AccountId)
                    .HasConstraintName("FK_NotificationAccountMappingModel_AccountModel");

                entity.HasOne(d => d.Notification)
                    .WithMany(p => p.NotificationAccountMappingModels)
                    .HasForeignKey(d => d.NotificationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NotificationAccountMappingModel_NotificationModel");
            });

            modelBuilder.Entity<NotificationModel>(entity =>
            {
                entity.Property(e => e.NotificationId).ValueGeneratedNever();
            });

            modelBuilder.Entity<PageFunctionModel>(entity =>
            {
                entity.HasKey(e => new { e.PageId, e.FunctionId });

                entity.HasOne(d => d.Function)
                    .WithMany(p => p.PageFunctionModels)
                    .HasForeignKey(d => d.FunctionId)
                    .HasConstraintName("FK_PageFunctionModel_FunctionModel");

                entity.HasOne(d => d.Page)
                    .WithMany(p => p.PageFunctionModels)
                    .HasForeignKey(d => d.PageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PageFunctionModel_PageModel");
            });

            modelBuilder.Entity<PageModel>(entity =>
            {
                entity.Property(e => e.PageId).ValueGeneratedNever();

                entity.HasOne(d => d.Menu)
                    .WithMany(p => p.PageModels)
                    .HasForeignKey(d => d.MenuId)
                    .HasConstraintName("FK_PageModel_MenuModel");
            });

            modelBuilder.Entity<PageModuleMapping>(entity =>
            {
                entity.HasKey(e => new { e.ModuleId, e.PageId });
            });

            modelBuilder.Entity<PagePermissionModel>(entity =>
            {
                entity.HasKey(e => new { e.RolesId, e.PageId, e.FunctionId });

                entity.HasOne(d => d.Function)
                    .WithMany(p => p.PagePermissionModels)
                    .HasForeignKey(d => d.FunctionId)
                    .HasConstraintName("FK_PagePermissionModel_FunctionModel");

                entity.HasOne(d => d.Page)
                    .WithMany(p => p.PagePermissionModels)
                    .HasForeignKey(d => d.PageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PagePermissionModel_PageModel");

                entity.HasOne(d => d.Roles)
                    .WithMany(p => p.PagePermissionModels)
                    .HasForeignKey(d => d.RolesId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PagePermissionModel_RolesModel");
            });

            modelBuilder.Entity<PartnerModel>(entity =>
            {
                entity.Property(e => e.PartnerId).ValueGeneratedNever();

                entity.HasOne(d => d.PartnerProfile)
                    .WithMany(p => p.PartnerModelPartnerProfiles)
                    .HasForeignKey(d => d.PartnerProfileId)
                    .HasConstraintName("FK_PartnerModel_ProfileModel1");

                entity.HasOne(d => d.Profile)
                    .WithMany(p => p.PartnerModelProfiles)
                    .HasForeignKey(d => d.ProfileId)
                    .HasConstraintName("FK_PartnerModel_ProfileModel");
            });

            modelBuilder.Entity<PaymentMethodModel>(entity =>
            {
                entity.Property(e => e.PaymentMethodId).ValueGeneratedNever();

                entity.Property(e => e.PaymentMethodType).HasComment("0: Tiền mặt, 1: Chuyển khoản, 2: Trả góp");
            });

            modelBuilder.Entity<PaymentNationalBudgetModel>(entity =>
            {
                entity.HasKey(e => e.PaymentNationalId)
                    .HasName("PK_ConfigPaymentNational");

                entity.Property(e => e.PaymentNationalId).ValueGeneratedNever();
            });

            modelBuilder.Entity<PeriodicallyCheckingModel>(entity =>
            {
                entity.Property(e => e.PeriodicallyCheckingId).ValueGeneratedNever();
            });

            modelBuilder.Entity<PersonInChargeDeletedModel>(entity =>
            {
                entity.Property(e => e.PersonInChargeId).ValueGeneratedNever();
            });

            modelBuilder.Entity<PersonInChargeModel>(entity =>
            {
                entity.Property(e => e.PersonInChargeId).ValueGeneratedNever();

                entity.Property(e => e.SalesEmployeeType).HasComment("1: NV kinh doanh, 2: NV sales admin");

                entity.HasOne(d => d.Profile)
                    .WithMany(p => p.PersonInChargeModels)
                    .HasForeignKey(d => d.ProfileId)
                    .HasConstraintName("FK_PersonInChargeModel_ProfileModel");

                entity.HasOne(d => d.SalesEmployeeCodeNavigation)
                    .WithMany(p => p.PersonInChargeModels)
                    .HasForeignKey(d => d.SalesEmployeeCode)
                    .HasConstraintName("FK_PersonInChargeModel_SalesEmployeeModel");
            });

            modelBuilder.Entity<PlateFeeDetailModel>(entity =>
            {
                entity.Property(e => e.PlateFeeDetailId).ValueGeneratedNever();

                entity.HasOne(d => d.PlateFee)
                    .WithMany(p => p.PlateFeeDetailModels)
                    .HasForeignKey(d => d.PlateFeeId)
                    .HasConstraintName("FK_PlateFeeDetailModel_PlateFeeModel");
            });

            modelBuilder.Entity<PlateFeeModel>(entity =>
            {
                entity.Property(e => e.PlateFeeId).ValueGeneratedNever();
            });

            modelBuilder.Entity<PriceProductModel>(entity =>
            {
                entity.Property(e => e.PriceProductId).ValueGeneratedNever();

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.PriceProductModels)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PriceProductModel_ProductModel");

                entity.HasOne(d => d.Style)
                    .WithMany(p => p.PriceProductModels)
                    .HasForeignKey(d => d.StyleId)
                    .HasConstraintName("FK_PriceProductModel_StyleModel");
            });

            modelBuilder.Entity<ProductAttributeModel>(entity =>
            {
                entity.Property(e => e.ProductId).ValueGeneratedNever();
            });

            modelBuilder.Entity<ProductHierarchyModel>(entity =>
            {
                entity.Property(e => e.Actived).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<ProductModel>(entity =>
            {
                entity.Property(e => e.ProductId).ValueGeneratedNever();

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.ProductModels)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK_ProductModel_CompanyModel");
            });

            modelBuilder.Entity<ProductPeriodicallyCheckingMapping>(entity =>
            {
                entity.HasKey(e => new { e.PeriodicallyCheckingId, e.ProductId });

                entity.HasOne(d => d.PeriodicallyChecking)
                    .WithMany(p => p.ProductPeriodicallyCheckingMappings)
                    .HasForeignKey(d => d.PeriodicallyCheckingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Product_PeriodicallyChecking_Mapping_PeriodicallyCheckingModel");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductPeriodicallyCheckingMappings)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Product_PeriodicallyChecking_Mapping_ProductModel");
            });

            modelBuilder.Entity<ProductPlateFeeMapping>(entity =>
            {
                entity.HasKey(e => new { e.PlateFeeId, e.ProductId });

                entity.HasOne(d => d.PlateFee)
                    .WithMany(p => p.ProductPlateFeeMappings)
                    .HasForeignKey(d => d.PlateFeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Product_PlateFee_Mapping_PlateFeeModel");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductPlateFeeMappings)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Product_PlateFee_Mapping_ProductModel");
            });

            modelBuilder.Entity<ProductPromotionContactModel>(entity =>
            {
                entity.Property(e => e.ProductPromotionContactId).ValueGeneratedNever();
            });

            modelBuilder.Entity<ProductPromotionDetailModel>(entity =>
            {
                entity.Property(e => e.ProductPromotionDetailId).ValueGeneratedNever();
            });

            modelBuilder.Entity<ProductPromotionModel>(entity =>
            {
                entity.Property(e => e.ProductPromotionId).ValueGeneratedNever();
            });

            modelBuilder.Entity<ProductTypeModel>(entity =>
            {
                entity.Property(e => e.ProductTypeId).ValueGeneratedNever();
            });

            modelBuilder.Entity<ProductWarrantyModel>(entity =>
            {
                entity.Property(e => e.ProductWarrantyId).ValueGeneratedNever();

                entity.Property(e => e.ProductWarrantyCode).ValueGeneratedOnAdd();

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductWarrantyModels)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductWarrantyModel_ProductModel");

                entity.HasOne(d => d.Profile)
                    .WithMany(p => p.ProductWarrantyModels)
                    .HasForeignKey(d => d.ProfileId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductWarrantyModel_ProfileModel");

                entity.HasOne(d => d.Warranty)
                    .WithMany(p => p.ProductWarrantyModels)
                    .HasForeignKey(d => d.WarrantyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductWarrantyModel_WarrantyModel");
            });

            modelBuilder.Entity<ProfileBattributeModel>(entity =>
            {
                entity.Property(e => e.ProfileId).ValueGeneratedNever();
            });

            modelBuilder.Entity<ProfileCareerModel>(entity =>
            {
                entity.Property(e => e.ProfileCareerId).ValueGeneratedNever();
            });

            modelBuilder.Entity<ProfileCatalogMapping>(entity =>
            {
                entity.Property(e => e.ProfileCatalogId).ValueGeneratedNever();
            });

            modelBuilder.Entity<ProfileCattributeModel>(entity =>
            {
                entity.HasKey(e => e.ProfileId)
                    .HasName("PK_ProfileCAttributeMode");

                entity.Property(e => e.ProfileId).ValueGeneratedNever();
            });

            modelBuilder.Entity<ProfileConfigModel>(entity =>
            {
                entity.HasKey(e => new { e.ProfileCategoryCode, e.FieldCode });
            });

            modelBuilder.Entity<ProfileContactAttributeDeletedModel>(entity =>
            {
                entity.Property(e => e.ProfileId).ValueGeneratedNever();
            });

            modelBuilder.Entity<ProfileContactAttributeModel>(entity =>
            {
                entity.HasKey(e => e.ProfileId)
                    .HasName("PK_ProfileContactAttributeModel_1");

                entity.Property(e => e.ProfileId).ValueGeneratedNever();
            });

            modelBuilder.Entity<ProfileDeletedModel>(entity =>
            {
                entity.Property(e => e.ProfileId).ValueGeneratedNever();
            });

            modelBuilder.Entity<ProfileEmailDeletedModel>(entity =>
            {
                entity.Property(e => e.EmailId).ValueGeneratedNever();
            });

            modelBuilder.Entity<ProfileEmailModel>(entity =>
            {
                entity.Property(e => e.EmailId).ValueGeneratedNever();

                entity.HasOne(d => d.Profile)
                    .WithMany(p => p.ProfileEmailModels)
                    .HasForeignKey(d => d.ProfileId)
                    .HasConstraintName("FK_ProfileEmailModel_ProfileModel");
            });

            modelBuilder.Entity<ProfileExtendInfo>(entity =>
            {
                entity.HasIndex(e => e.ProfileId, "ClusteredIndex-20211008-152033")
                    .IsClustered();
            });

            modelBuilder.Entity<ProfileFileMapping>(entity =>
            {
                entity.HasKey(e => new { e.ProfileId, e.FileAttachmentId });

                entity.Property(e => e.Note).IsFixedLength(true);

                entity.HasOne(d => d.FileAttachment)
                    .WithMany(p => p.ProfileFileMappings)
                    .HasForeignKey(d => d.FileAttachmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Profile_File_Mapping_FileAttachmentModel");

                entity.HasOne(d => d.Profile)
                    .WithMany(p => p.ProfileFileMappings)
                    .HasForeignKey(d => d.ProfileId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Profile_File_Mapping_ProfileModel");
            });

            modelBuilder.Entity<ProfileGroupModel>(entity =>
            {
                entity.Property(e => e.ProfileGroupId).ValueGeneratedNever();

                entity.HasOne(d => d.Profile)
                    .WithMany(p => p.ProfileGroupModels)
                    .HasForeignKey(d => d.ProfileId)
                    .HasConstraintName("FK_ProfileGroupModel_ProfileModel");
            });

            modelBuilder.Entity<ProfileLevelModel>(entity =>
            {
                entity.HasKey(e => e.CustomerLevelId)
                    .HasName("PK_CustomerLevelModel_1");

                entity.Property(e => e.CustomerLevelId).ValueGeneratedNever();
            });

            modelBuilder.Entity<ProfileModel>(entity =>
            {
                entity.HasIndex(e => e.ProfileForeignCode, "UQ_Profile_ProfileForeignCode")
                    .IsUnique()
                    .HasFilter("([ProfileForeignCode] IS NOT NULL)");

                entity.Property(e => e.ProfileId).ValueGeneratedNever();

                entity.Property(e => e.AutoformatFullName).HasDefaultValueSql("((1))");

                entity.Property(e => e.CreateRequestTime).HasComment("Thời gian yêu cầu tạo khách ở ECC");

                entity.Property(e => e.IsCreateRequest).HasComment("yêu cầu tạo khách ở ECC");

                entity.Property(e => e.IsSyncedFromSap).HasComment("Đánh dấu đã đồng bộ từ SAP");

                entity.Property(e => e.ProfileCode).ValueGeneratedOnAdd();

                entity.Property(e => e.ReferenceProfileId).HasComment("Chủ Đầu Tư");

                entity.Property(e => e.ReferenceProfileId2).HasComment("Tư vấn & TK");
            });

            modelBuilder.Entity<ProfileOpportunityCompetitorDetailModel>(entity =>
            {
                entity.Property(e => e.OpportunityCompetitorDetailId).ValueGeneratedNever();
            });

            modelBuilder.Entity<ProfileOpportunityCompetitorModel>(entity =>
            {
                entity.Property(e => e.OpportunityCompetitorId).ValueGeneratedNever();

                entity.HasOne(d => d.Profile)
                    .WithMany(p => p.ProfileOpportunityCompetitorModels)
                    .HasForeignKey(d => d.ProfileId)
                    .HasConstraintName("FK_Profile_Opportunity_CompetitorModel_ProfileModel_Profile");
            });

            modelBuilder.Entity<ProfileOpportunityInternalModel>(entity =>
            {
                entity.HasComment("Dự án - Spec + Thi công - Tab An Cường");

                entity.Property(e => e.OpportunityInternalId).ValueGeneratedNever();

                entity.HasOne(d => d.Profile)
                    .WithMany(p => p.ProfileOpportunityInternalModels)
                    .HasForeignKey(d => d.ProfileId)
                    .HasConstraintName("FK_Profile_Opportunity_InternalModel_ProfileModel_Profile");
            });

            modelBuilder.Entity<ProfileOpportunityMaterialModel>(entity =>
            {
                entity.Property(e => e.OpportunityMaterialId).ValueGeneratedNever();

                entity.Property(e => e.MaterialType).HasComment("1: Nội thất bàn giao, 2: Hạng mục trúng thầu");

                entity.HasOne(d => d.Profile)
                    .WithMany(p => p.ProfileOpportunityMaterialModels)
                    .HasForeignKey(d => d.ProfileId)
                    .HasConstraintName("FK_Profile_Opportunity_MaterialModel_ProfileModel_Profile");
            });

            modelBuilder.Entity<ProfileOpportunityPartnerModel>(entity =>
            {
                entity.Property(e => e.OpportunityPartnerId).ValueGeneratedNever();

                entity.Property(e => e.IsWon).HasComment("Trúng thầu/Thắng thầu");

                entity.Property(e => e.PartnerType).HasComment("1: Chủ đầu tư, 2: Thiết kế, 3: Tổng thầu, 4: Căn mẫu, 5: Đại trà");

                entity.HasOne(d => d.Partner)
                    .WithMany(p => p.ProfileOpportunityPartnerModelPartners)
                    .HasForeignKey(d => d.PartnerId)
                    .HasConstraintName("FK_Profile_Opportunity_PartnerModel_ProfileModel_Partner");

                entity.HasOne(d => d.Profile)
                    .WithMany(p => p.ProfileOpportunityPartnerModelProfiles)
                    .HasForeignKey(d => d.ProfileId)
                    .HasConstraintName("FK_Profile_Opportunity_PartnerModel_ProfileModel_Profile");
            });

            modelBuilder.Entity<ProfileOpportunityStatusMapping>(entity =>
            {
                entity.Property(e => e.OpportunityStatusId).ValueGeneratedNever();
            });

            modelBuilder.Entity<ProfilePhoneDeletedModel>(entity =>
            {
                entity.Property(e => e.PhoneId).ValueGeneratedNever();
            });

            modelBuilder.Entity<ProfilePhoneModel>(entity =>
            {
                entity.Property(e => e.PhoneId).ValueGeneratedNever();

                entity.HasOne(d => d.Profile)
                    .WithMany(p => p.ProfilePhoneModels)
                    .HasForeignKey(d => d.ProfileId)
                    .HasConstraintName("FK_ProfilePhoneModel_ProfileModel");
            });

            modelBuilder.Entity<ProfileProfileTypeMapping>(entity =>
            {
                entity.Property(e => e.ProfileTypeId).ValueGeneratedNever();
            });

            modelBuilder.Entity<ProfileTypeModel>(entity =>
            {
                entity.Property(e => e.ProfileTypeId).ValueGeneratedNever();
            });

            modelBuilder.Entity<ProfitCenterModel>(entity =>
            {
                entity.Property(e => e.Actived).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<PrognosisModel>(entity =>
            {
                entity.Property(e => e.PrognosisId).ValueGeneratedNever();
            });

            modelBuilder.Entity<PromotionBuyMaterialModel>(entity =>
            {
                entity.Property(e => e.BuyMaterialId).ValueGeneratedNever();
            });

            modelBuilder.Entity<PromotionByStoreModel>(entity =>
            {
                entity.HasKey(e => new { e.PromotionId, e.StoreId });

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.PromotionByStoreModels)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PromotionByStoreModel_StoreModel");
            });

            modelBuilder.Entity<PromotionGiftAccessoryModel>(entity =>
            {
                entity.HasKey(e => e.GiftMaterialId)
                    .HasName("PK_PromotionGiftMaterialModel");

                entity.Property(e => e.GiftMaterialId).ValueGeneratedNever();
            });

            modelBuilder.Entity<PromotionModel>(entity =>
            {
                entity.Property(e => e.PromotionId).ValueGeneratedNever();
            });

            modelBuilder.Entity<PromotionProductModel>(entity =>
            {
                entity.HasKey(e => new { e.PromotionId, e.ProductId })
                    .HasName("PK_ProductInPromotion");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.PromotionProductModels)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductInPromotion_ProductModel");

                entity.HasOne(d => d.Promotion)
                    .WithMany(p => p.PromotionProductModels)
                    .HasForeignKey(d => d.PromotionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductInPromotion_PromotionModel");
            });

            modelBuilder.Entity<PropertiesProductModel>(entity =>
            {
                entity.Property(e => e.PropertiesId).ValueGeneratedNever();

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.PropertiesProductModels)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PropertiesProductModel_ProductModel");
            });

            modelBuilder.Entity<ProspectModel>(entity =>
            {
                entity.Property(e => e.ProspectId).ValueGeneratedNever();

                entity.Property(e => e.GeneratedCode).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<ProvinceModel>(entity =>
            {
                entity.HasIndex(e => e.ProvinceCode, "ProvinceCode_UNIQUE")
                    .IsUnique()
                    .HasFilter("([ProvinceCode] IS NOT NULL)");

                entity.Property(e => e.ProvinceId).ValueGeneratedNever();
            });

            modelBuilder.Entity<QuestionBankModel>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.QuestionBankCode).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.HasOne(d => d.CreateByNavigation)
                    .WithMany(p => p.QuestionBankModelCreateByNavigations)
                    .HasForeignKey(d => d.CreateBy)
                    .HasConstraintName("FK_QuestionBankModel_AccountModel");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.QuestionBankModelDepartments)
                    .HasForeignKey(d => d.DepartmentId)
                    .HasConstraintName("FK_QuestionBankModel_CatalogModel1");

                entity.HasOne(d => d.LastEditByNavigation)
                    .WithMany(p => p.QuestionBankModelLastEditByNavigations)
                    .HasForeignKey(d => d.LastEditBy)
                    .HasConstraintName("FK_QuestionBankModel_AccountModel1");

                entity.HasOne(d => d.QuestionCategory)
                    .WithMany(p => p.QuestionBankModelQuestionCategories)
                    .HasForeignKey(d => d.QuestionCategoryId)
                    .HasConstraintName("FK_QuestionBankModel_CatalogModel");
            });

            modelBuilder.Entity<RatingModel>(entity =>
            {
                entity.Property(e => e.RatingId).ValueGeneratedNever();
            });

            modelBuilder.Entity<RegisterReceiveNewsModel>(entity =>
            {
                entity.Property(e => e.RegisterReceiveNewsId).ValueGeneratedNever();
            });

            modelBuilder.Entity<RelatedNewsMapping>(entity =>
            {
                entity.Property(e => e.RelatedNewsId).ValueGeneratedNever();
            });

            modelBuilder.Entity<RemindTaskModel>(entity =>
            {
                entity.Property(e => e.TaskId).ValueGeneratedNever();
            });

            modelBuilder.Entity<RequestEccEmailConfigModel>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.FromEmail).IsUnicode(false);

                entity.Property(e => e.FromEmailPassword).IsUnicode(false);

                entity.Property(e => e.Host).IsUnicode(false);

                entity.Property(e => e.ToEmail).IsUnicode(false);
            });

            modelBuilder.Entity<RoleInChargeDeletedModel>(entity =>
            {
                entity.Property(e => e.RoleInChargeId).ValueGeneratedNever();
            });

            modelBuilder.Entity<RoleInChargeModel>(entity =>
            {
                entity.Property(e => e.RoleInChargeId).ValueGeneratedNever();

                entity.HasOne(d => d.Profile)
                    .WithMany(p => p.RoleInChargeModels)
                    .HasForeignKey(d => d.ProfileId)
                    .HasConstraintName("FK_RoleInChargeModel_ProfileModel");

                entity.HasOne(d => d.Roles)
                    .WithMany(p => p.RoleInChargeModels)
                    .HasForeignKey(d => d.RolesId)
                    .HasConstraintName("FK_RoleInChargeModel_RolesModel");
            });

            modelBuilder.Entity<RolesModel>(entity =>
            {
                entity.HasKey(e => e.RolesId)
                    .HasName("PK__RolesMod__C4B278410A3A22E7")
                    .IsClustered(false);

                entity.Property(e => e.RolesId).HasDefaultValueSql("(newsequentialid())");
            });

            modelBuilder.Entity<SaleOrderDetailModel>(entity =>
            {
                entity.HasKey(e => e.SaleOrderDetailId)
                    .HasName("PK_SaleOrderDetailModel_1");

                entity.Property(e => e.SaleOrderDetailId).ValueGeneratedNever();

                entity.HasOne(d => d.SaleOrderMaster)
                    .WithMany(p => p.SaleOrderDetailModels)
                    .HasForeignKey(d => d.SaleOrderMasterId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_SaleOrderDetailModel_SaleOrderMasterModel");
            });

            modelBuilder.Entity<SaleOrderMasterModel>(entity =>
            {
                entity.HasKey(e => e.SaleOrderMasterId)
                    .HasName("PK_SaleOrderMaster");

                entity.Property(e => e.SaleOrderMasterId).ValueGeneratedNever();

                entity.Property(e => e.GeneratedCode).ValueGeneratedOnAdd();

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.SaleOrderMasterModels)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_SaleOrderMasterModel_CustomerModel");

                entity.HasOne(d => d.MaterialCodeNavigation)
                    .WithMany(p => p.SaleOrderMasterModels)
                    .HasForeignKey(d => d.MaterialCode)
                    .HasConstraintName("FK_SaleOrderMasterModel_MaterialModel");
            });

            modelBuilder.Entity<SaleProcessModel>(entity =>
            {
                entity.Property(e => e.SaleProcessId).ValueGeneratedNever();
            });

            modelBuilder.Entity<SaleUnitModel>(entity =>
            {
                entity.Property(e => e.SaleUnitId).ValueGeneratedNever();
            });

            modelBuilder.Entity<SalesEmployeeModel>(entity =>
            {
                entity.Property(e => e.Actived).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.SalesEmployeeModels)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK_SalesEmployeeModel_CompanyModel");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.SalesEmployeeModels)
                    .HasForeignKey(d => d.DepartmentId)
                    .HasConstraintName("FK_SalesEmployeeModel_DepartmentModel");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.SalesEmployeeModels)
                    .HasForeignKey(d => d.StoreId)
                    .HasConstraintName("FK_SalesEmployeeModel_StoreModel");
            });

            modelBuilder.Entity<SearchResultDetailTemplateModel>(entity =>
            {
                entity.Property(e => e.SearchResultDetailTemplateId).ValueGeneratedNever();

                entity.HasOne(d => d.SearchResultTemplate)
                    .WithMany(p => p.SearchResultDetailTemplateModels)
                    .HasForeignKey(d => d.SearchResultTemplateId)
                    .HasConstraintName("FK_SearchResultDetailTemplateModel_SearchResultTemplateModel");
            });

            modelBuilder.Entity<SearchResultTemplateModel>(entity =>
            {
                entity.Property(e => e.SearchResultTemplateId).ValueGeneratedNever();
            });

            modelBuilder.Entity<SearchTemplateModel>(entity =>
            {
                entity.Property(e => e.SearchTemplateId).ValueGeneratedNever();
            });

            modelBuilder.Entity<SendMailCalendarModel>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CheckinTime).HasComment("Thời gian checkin");

                entity.Property(e => e.ConfirmTime).HasComment("Thời gian xác nhận");

                entity.Property(e => e.IsCheckin).HasComment("Đã check in");

                entity.Property(e => e.IsConfirm).HasComment("Đã xác nhận");
            });

            modelBuilder.Entity<ServiceAppointmentModel>(entity =>
            {
                entity.Property(e => e.AppointmentId).ValueGeneratedNever();
            });

            modelBuilder.Entity<ServiceFlagModel>(entity =>
            {
                entity.Property(e => e.ServiceFlagId).ValueGeneratedNever();
            });

            modelBuilder.Entity<ServiceOrderConsultModel>(entity =>
            {
                entity.Property(e => e.ServiceOrderConsultId).HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.ServiceOrder)
                    .WithMany(p => p.ServiceOrderConsultModels)
                    .HasForeignKey(d => d.ServiceOrderId)
                    .HasConstraintName("FK_ServiceOrderConsultModel_ServiceOrderModel");
            });

            modelBuilder.Entity<ServiceOrderDetailAccessoryModel>(entity =>
            {
                entity.HasKey(e => e.ServiceOrderDetailAccessoryId)
                    .HasName("PK_ServiceOrderDetailAccessoryMode");

                entity.Property(e => e.ServiceOrderDetailAccessoryId).HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.ServiceOrder)
                    .WithMany(p => p.ServiceOrderDetailAccessoryModels)
                    .HasForeignKey(d => d.ServiceOrderId)
                    .HasConstraintName("FK_ServiceOrderDetailAccessoryModel_ServiceOrderModel");
            });

            modelBuilder.Entity<ServiceOrderDetailModel>(entity =>
            {
                entity.Property(e => e.ServiceOrderDetailId).ValueGeneratedNever();
            });

            modelBuilder.Entity<ServiceOrderDetailServiceModel>(entity =>
            {
                entity.Property(e => e.ServiceOrderDetailServiceId).HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.ServiceOrder)
                    .WithMany(p => p.ServiceOrderDetailServiceModels)
                    .HasForeignKey(d => d.ServiceOrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ServiceOrderDetailServiceModel_ServiceOrderModel");

                entity.HasOne(d => d.ServiceTypeCodeNavigation)
                    .WithMany(p => p.ServiceOrderDetailServiceModels)
                    .HasForeignKey(d => d.ServiceTypeCode)
                    .HasConstraintName("FK_ServiceOrderDetailServiceModel_ServiceTypeModel");
            });

            modelBuilder.Entity<ServiceOrderModel>(entity =>
            {
                entity.Property(e => e.ServiceOrderId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.GeneratedCode).ValueGeneratedOnAdd();

                entity.Property(e => e.IsNew).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.ServiceOrderModels)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_ServiceOrderModel_CustomerModel");

                entity.HasOne(d => d.SaleOrderMaster)
                    .WithMany(p => p.ServiceOrderModels)
                    .HasForeignKey(d => d.SaleOrderMasterId)
                    .HasConstraintName("FK_ServiceOrderModel_SaleOrderMasterModel");

                entity.HasOne(d => d.Vehicle)
                    .WithMany(p => p.ServiceOrderModels)
                    .HasForeignKey(d => d.VehicleId)
                    .HasConstraintName("FK_ServiceOrderModel_VehicleInfoModel");
            });

            modelBuilder.Entity<ServiceOrderPoolModel>(entity =>
            {
                entity.Property(e => e.ServiceOrderPoolId).ValueGeneratedNever();
            });

            modelBuilder.Entity<ServiceOrderTypeModel>(entity =>
            {
                entity.HasKey(e => e.ServiceOrderTypeCode)
                    .HasName("PK_ServiceCategoryModel_1");
            });

            modelBuilder.Entity<ServiceTypeModel>(entity =>
            {
                entity.Property(e => e.Actived).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<Smsmodel>(entity =>
            {
                entity.Property(e => e.Smsid).ValueGeneratedNever();
            });

            modelBuilder.Entity<SourceModel>(entity =>
            {
                entity.Property(e => e.SourceId).ValueGeneratedNever();
            });

            modelBuilder.Entity<SpecificationsModel>(entity =>
            {
                entity.Property(e => e.SpecificationsId).ValueGeneratedNever();
            });

            modelBuilder.Entity<SpecificationsProductModel>(entity =>
            {
                entity.Property(e => e.SpecificationsProductId).ValueGeneratedNever();

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.SpecificationsProductModels)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SpecificationsProductModel_ProductModel");

                entity.HasOne(d => d.Specifications)
                    .WithMany(p => p.SpecificationsProductModels)
                    .HasForeignKey(d => d.SpecificationsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SpecificationsProductModel_SpecificationsModel");
            });

            modelBuilder.Entity<StateTreasuryModel>(entity =>
            {
                entity.Property(e => e.StateTreasuryId).ValueGeneratedNever();
            });

            modelBuilder.Entity<StatusTransitionModel>(entity =>
            {
                entity.Property(e => e.StatusTransitionId).ValueGeneratedNever();

                entity.Property(e => e.BranchIn).IsUnicode(false);

                entity.Property(e => e.BranchOut).IsUnicode(false);

                entity.Property(e => e.StatusTransitionIn).IsUnicode(false);

                entity.Property(e => e.StatusTransitionOut).IsUnicode(false);

                entity.HasOne(d => d.FromStatus)
                    .WithMany(p => p.StatusTransitionModelFromStatuses)
                    .HasForeignKey(d => d.FromStatusId)
                    .HasConstraintName("FK_StatusTransitionModel_TaskStatusModel");

                entity.HasOne(d => d.ToStatus)
                    .WithMany(p => p.StatusTransitionModelToStatuses)
                    .HasForeignKey(d => d.ToStatusId)
                    .HasConstraintName("FK_StatusTransitionModel_TaskStatusModel1");

                entity.HasOne(d => d.WorkFlow)
                    .WithMany(p => p.StatusTransitionModels)
                    .HasForeignKey(d => d.WorkFlowId)
                    .HasConstraintName("FK_StatusTransitionModel_WorkFlowModel");
            });

            modelBuilder.Entity<StatusTransitionTaskMapping>(entity =>
            {
                entity.HasKey(e => e.TaskTransitionLogId)
                    .HasName("PK_TaskTransitionLogModel");

                entity.Property(e => e.TaskTransitionLogId).ValueGeneratedNever();
            });

            modelBuilder.Entity<StockModel>(entity =>
            {
                entity.Property(e => e.StockId).ValueGeneratedNever();
            });

            modelBuilder.Entity<StockReceivingDetailModel>(entity =>
            {
                entity.HasKey(e => e.StockReceivingDetailId)
                    .HasName("PK_StockRecevingDetailModel");

                entity.Property(e => e.StockReceivingDetailId).ValueGeneratedNever();

                entity.HasOne(d => d.DateKeyNavigation)
                    .WithMany(p => p.StockReceivingDetailModels)
                    .HasForeignKey(d => d.DateKey)
                    .HasConstraintName("FK_StockRecevingDetailModel_DimDate");

                entity.HasOne(d => d.Inventory)
                    .WithMany(p => p.StockReceivingDetailModels)
                    .HasForeignKey(d => d.InventoryId)
                    .HasConstraintName("FK_StockReceivingDetailModel_InventoryModel");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.StockReceivingDetailModels)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_StockRecevingDetailModel_ProductModel");

                entity.HasOne(d => d.Stock)
                    .WithMany(p => p.StockReceivingDetailModels)
                    .HasForeignKey(d => d.StockId)
                    .HasConstraintName("FK_StockRecevingDetailModel_StockModel");

                entity.HasOne(d => d.StockReceiving)
                    .WithMany(p => p.StockReceivingDetailModels)
                    .HasForeignKey(d => d.StockReceivingId)
                    .HasConstraintName("FK_StockRecevingDetailModel_StockReceivingMasterModel");
            });

            modelBuilder.Entity<StockReceivingMasterModel>(entity =>
            {
                entity.Property(e => e.StockReceivingId).ValueGeneratedNever();

                entity.Property(e => e.StockReceivingCode).ValueGeneratedOnAdd();

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.StockReceivingMasterModels)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK_StockReceivingMasterModel_CompanyModel");

                entity.HasOne(d => d.Profile)
                    .WithMany(p => p.StockReceivingMasterModels)
                    .HasForeignKey(d => d.ProfileId)
                    .HasConstraintName("FK_StockReceivingMasterModel_ProfileModel");

                entity.HasOne(d => d.SalesEmployeeCodeNavigation)
                    .WithMany(p => p.StockReceivingMasterModels)
                    .HasForeignKey(d => d.SalesEmployeeCode)
                    .HasConstraintName("FK_StockReceivingMasterModel_SalesEmployeeModel");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.StockReceivingMasterModels)
                    .HasForeignKey(d => d.StoreId)
                    .HasConstraintName("FK_StockReceivingMasterModel_StoreModel");
            });

            modelBuilder.Entity<StockStoreMapping>(entity =>
            {
                entity.HasKey(e => new { e.StockId, e.StoreId });

                entity.HasOne(d => d.Stock)
                    .WithMany(p => p.StockStoreMappings)
                    .HasForeignKey(d => d.StockId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Stock_Store_Mapping_StockModel");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.StockStoreMappings)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Stock_Store_Mapping_StoreModel");
            });

            modelBuilder.Entity<StockTransferRequestDetailModel>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.StockTransferRequestDetailModels)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_StockTransferRequestDetailModel_ProductModel");

                entity.HasOne(d => d.StockTransferRequest)
                    .WithMany(p => p.StockTransferRequestDetailModels)
                    .HasForeignKey(d => d.StockTransferRequestId)
                    .HasConstraintName("FK_StockTransferRequestDetailModel_StockTransferRequestModel");
            });

            modelBuilder.Entity<StockTransferRequestModel>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.StockTransferRequestCode).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.StockTransferRequestModels)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK_StockTransferRequestModel_CompanyModel");

                entity.HasOne(d => d.CreateByNavigation)
                    .WithMany(p => p.StockTransferRequestModelCreateByNavigations)
                    .HasForeignKey(d => d.CreateBy)
                    .HasConstraintName("FK_StockTransferRequestModel_AccountModel");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.StockTransferRequestModelDeletedByNavigations)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_StockTransferRequestModel_AccountModel2");

                entity.HasOne(d => d.FromStockNavigation)
                    .WithMany(p => p.StockTransferRequestModelFromStockNavigations)
                    .HasForeignKey(d => d.FromStock)
                    .HasConstraintName("FK_StockTransferRequestModel_StockModel");

                entity.HasOne(d => d.LastEditByNavigation)
                    .WithMany(p => p.StockTransferRequestModelLastEditByNavigations)
                    .HasForeignKey(d => d.LastEditBy)
                    .HasConstraintName("FK_StockTransferRequestModel_AccountModel1");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.StockTransferRequestModels)
                    .HasForeignKey(d => d.StoreId)
                    .HasConstraintName("FK_StockTransferRequestModel_StoreModel");

                entity.HasOne(d => d.ToStockNavigation)
                    .WithMany(p => p.StockTransferRequestModelToStockNavigations)
                    .HasForeignKey(d => d.ToStock)
                    .HasConstraintName("FK_StockTransferRequestModel_StockModel1");
            });

            modelBuilder.Entity<StoreModel>(entity =>
            {
                entity.Property(e => e.StoreId).ValueGeneratedNever();

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.StoreModels)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StoreModel_CompanyModel");
            });

            modelBuilder.Entity<StoreTypeModel>(entity =>
            {
                entity.HasKey(e => e.StoreTypeId)
                    .HasName("PK_StoreTypeMode");

                entity.Property(e => e.StoreTypeId).ValueGeneratedNever();
            });

            modelBuilder.Entity<StyleModel>(entity =>
            {
                entity.Property(e => e.StyleId).ValueGeneratedNever();
            });

            modelBuilder.Entity<SurveyDetailModel>(entity =>
            {
                entity.Property(e => e.SurveyDetailId).ValueGeneratedNever();
            });

            modelBuilder.Entity<SurveyMapping>(entity =>
            {
                entity.Property(e => e.SurveyMappingId).ValueGeneratedNever();
            });

            modelBuilder.Entity<SurveyModel>(entity =>
            {
                entity.Property(e => e.SurveyId).ValueGeneratedNever();
            });

            modelBuilder.Entity<TargetGroupModel>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.TargetGroupCode).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                entity.Property(e => e.Type).HasComment("Marketing|Event");

                entity.HasOne(d => d.CreateByNavigation)
                    .WithMany(p => p.TargetGroupModelCreateByNavigations)
                    .HasForeignKey(d => d.CreateBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TargetGroupModel_AccountModel");

                entity.HasOne(d => d.LastEditByNavigation)
                    .WithMany(p => p.TargetGroupModelLastEditByNavigations)
                    .HasForeignKey(d => d.LastEditBy)
                    .HasConstraintName("FK_TargetGroupModel_AccountModel1");
            });

            modelBuilder.Entity<TaskAssignModel>(entity =>
            {
                entity.Property(e => e.TaskAssignId).ValueGeneratedNever();
            });

            modelBuilder.Entity<TaskCommentModel>(entity =>
            {
                entity.Property(e => e.TaskCommentId).ValueGeneratedNever();
            });

            modelBuilder.Entity<TaskContactModel>(entity =>
            {
                entity.Property(e => e.TaskContactId).ValueGeneratedNever();
            });

            modelBuilder.Entity<TaskEventMapping>(entity =>
            {
                entity.Property(e => e.TaskEventMappingId).ValueGeneratedNever();
            });

            modelBuilder.Entity<TaskFileMapping>(entity =>
            {
                entity.HasKey(e => new { e.TaskId, e.FileAttachmentId });

                entity.HasOne(d => d.FileAttachment)
                    .WithMany(p => p.TaskFileMappings)
                    .HasForeignKey(d => d.FileAttachmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Task_File_Mapping_FileAttachmentModel");

                entity.HasOne(d => d.Task)
                    .WithMany(p => p.TaskFileMappings)
                    .HasForeignKey(d => d.TaskId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Task_File_Mapping_TaskModel");
            });

            modelBuilder.Entity<TaskGroupDetailModel>(entity =>
            {
                entity.HasKey(e => new { e.GroupId, e.AccountId });
            });

            modelBuilder.Entity<TaskGroupModel>(entity =>
            {
                entity.Property(e => e.GroupId).ValueGeneratedNever();
            });

            modelBuilder.Entity<TaskModel>(entity =>
            {
                entity.Property(e => e.TaskId).ValueGeneratedNever();

                entity.Property(e => e.ConstructionUnit).HasComment("đơn vị thi công");

                entity.Property(e => e.ConstructionUnitContact).HasComment("liên hệ của đơn vị thi công");

                entity.Property(e => e.HasSurvey)
                    .HasDefaultValueSql("((0))")
                    .HasComment("Có Survey => 1 || Không có survey => 0");

                entity.Property(e => e.Property6).HasComment("Giá trị ĐTB");

                entity.Property(e => e.ShortNote).HasComment("Ghi chú ngắn dùng để edit trực tiếp trên lịch");

                entity.Property(e => e.TaskCode).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<TaskProductAccessoryModel>(entity =>
            {
                entity.Property(e => e.TaskProductAccessoryId).ValueGeneratedNever();

                entity.HasOne(d => d.TaskProduct)
                    .WithMany(p => p.TaskProductAccessoryModels)
                    .HasForeignKey(d => d.TaskProductId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_TaskProductAccessoryModel_TaskProductModel");
            });

            modelBuilder.Entity<TaskProductModel>(entity =>
            {
                entity.Property(e => e.TaskProductId).ValueGeneratedNever();

                entity.HasOne(d => d.Task)
                    .WithMany(p => p.TaskProductModels)
                    .HasForeignKey(d => d.TaskId)
                    .HasConstraintName("FK_TaskProductModel_TaskModel");
            });

            modelBuilder.Entity<TaskProductUsualErrorModel>(entity =>
            {
                entity.Property(e => e.TaskProductUsualErrorId).ValueGeneratedNever();

                entity.HasOne(d => d.TaskProduct)
                    .WithMany(p => p.TaskProductUsualErrorModels)
                    .HasForeignKey(d => d.TaskProductId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_TaskProductUsualErrorModel_TaskProductModel");
            });

            modelBuilder.Entity<TaskReferenceModel>(entity =>
            {
                entity.Property(e => e.TaskReferenceId).ValueGeneratedNever();
            });

            modelBuilder.Entity<TaskReporterModel>(entity =>
            {
                entity.Property(e => e.TaskReporterId).ValueGeneratedNever();
            });

            modelBuilder.Entity<TaskRoleInChargeModel>(entity =>
            {
                entity.Property(e => e.RoleInChargeId).ValueGeneratedNever();
            });

            modelBuilder.Entity<TaskStatusModel>(entity =>
            {
                entity.Property(e => e.TaskStatusId).ValueGeneratedNever();

                entity.HasOne(d => d.WorkFlow)
                    .WithMany(p => p.TaskStatusModels)
                    .HasForeignKey(d => d.WorkFlowId)
                    .HasConstraintName("FK_TaskStatusModel_WorkFlowModel");
            });

            modelBuilder.Entity<TaskSurveyAnswerModel>(entity =>
            {
                entity.Property(e => e.TaskSurveyAnswerId).ValueGeneratedNever();
            });

            modelBuilder.Entity<TaskSurveyModel>(entity =>
            {
                entity.Property(e => e.TaskSurveyId).ValueGeneratedNever();
            });

            modelBuilder.Entity<TaxConfigModel>(entity =>
            {
                entity.Property(e => e.TaxId).ValueGeneratedNever();
            });

            modelBuilder.Entity<TemperatureConditionModel>(entity =>
            {
                entity.Property(e => e.Actived).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<TemplateAndGiftCampaignModel>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.TemplateAndGiftCampaignCode).ValueGeneratedOnAdd();

                entity.HasOne(d => d.CreateByNavigation)
                    .WithMany(p => p.TemplateAndGiftCampaignModelCreateByNavigations)
                    .HasForeignKey(d => d.CreateBy)
                    .HasConstraintName("FK_TemplateAndGiftCampaignModel_AccountModel");

                entity.HasOne(d => d.LastEditByNavigation)
                    .WithMany(p => p.TemplateAndGiftCampaignModelLastEditByNavigations)
                    .HasForeignKey(d => d.LastEditBy)
                    .HasConstraintName("FK_TemplateAndGiftCampaignModel_AccountModel1");

                entity.HasOne(d => d.TemplateAndGiftTargetGroup)
                    .WithMany(p => p.TemplateAndGiftCampaignModels)
                    .HasForeignKey(d => d.TemplateAndGiftTargetGroupId)
                    .HasConstraintName("FK_TemplateAndGiftCampaignModel_TemplateAndGiftTargetGroupModel");
            });

            modelBuilder.Entity<TemplateAndGiftMemberAddressModel>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.CreateByNavigation)
                    .WithMany(p => p.TemplateAndGiftMemberAddressModelCreateByNavigations)
                    .HasForeignKey(d => d.CreateBy)
                    .HasConstraintName("FK_TemplateAndGiftMemberAddressModel_AccountModel");

                entity.HasOne(d => d.LastEditByNavigation)
                    .WithMany(p => p.TemplateAndGiftMemberAddressModelLastEditByNavigations)
                    .HasForeignKey(d => d.LastEditBy)
                    .HasConstraintName("FK_TemplateAndGiftMemberAddressModel_AccountModel1");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.TemplateAndGiftMemberAddressModels)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_TemplateAndGiftMemberAddressModel_ProductModel");

                entity.HasOne(d => d.TempalteAndGiftMember)
                    .WithMany(p => p.TemplateAndGiftMemberAddressModels)
                    .HasForeignKey(d => d.TempalteAndGiftMemberId)
                    .HasConstraintName("FK_TemplateAndGiftMemberAddressModel_TemplateAndGiftMemberModel");
            });

            modelBuilder.Entity<TemplateAndGiftMemberModel>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Profile)
                    .WithMany(p => p.TemplateAndGiftMemberModels)
                    .HasForeignKey(d => d.ProfileId)
                    .HasConstraintName("FK_TemplateAndGiftMemberModel_ProfileModel");

                entity.HasOne(d => d.TemplateAndGiftTargetGroup)
                    .WithMany(p => p.TemplateAndGiftMemberModels)
                    .HasForeignKey(d => d.TemplateAndGiftTargetGroupId)
                    .HasConstraintName("FK_TemplateAndGiftMemberModel_TemplateAndGiftTargetGroupModel");
            });

            modelBuilder.Entity<TemplateAndGiftTargetGroupModel>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.TargetGroupCode).ValueGeneratedOnAdd();

                entity.HasOne(d => d.CreateByNavigation)
                    .WithMany(p => p.TemplateAndGiftTargetGroupModelCreateByNavigations)
                    .HasForeignKey(d => d.CreateBy)
                    .HasConstraintName("FK_TemplateAndGiftTargetGroupModel_AccountModel");

                entity.HasOne(d => d.LastEditByNavigation)
                    .WithMany(p => p.TemplateAndGiftTargetGroupModelLastEditByNavigations)
                    .HasForeignKey(d => d.LastEditBy)
                    .HasConstraintName("FK_TemplateAndGiftTargetGroupModel_AccountModel1");
            });

            modelBuilder.Entity<TransferDetailModel>(entity =>
            {
                entity.Property(e => e.TransferDetailId).ValueGeneratedNever();

                entity.HasOne(d => d.DateKeyNavigation)
                    .WithMany(p => p.TransferDetailModels)
                    .HasForeignKey(d => d.DateKey)
                    .HasConstraintName("FK_TransferDetailModel_DimDate");

                entity.HasOne(d => d.FromStock)
                    .WithMany(p => p.TransferDetailModelFromStocks)
                    .HasForeignKey(d => d.FromStockId)
                    .HasConstraintName("FK_TransferDetailModel_StockModel");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.TransferDetailModels)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_TransferDetailModel_ProductModel");

                entity.HasOne(d => d.ToStock)
                    .WithMany(p => p.TransferDetailModelToStocks)
                    .HasForeignKey(d => d.ToStockId)
                    .HasConstraintName("FK_TransferDetailModel_StockModel1");
            });

            modelBuilder.Entity<TransferModel>(entity =>
            {
                entity.Property(e => e.TransferId).ValueGeneratedNever();

                entity.Property(e => e.TransferCode).ValueGeneratedOnAdd();

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.TransferModels)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK_TransferModel_CompanyModel");

                entity.HasOne(d => d.SalesEmployeeCodeNavigation)
                    .WithMany(p => p.TransferModels)
                    .HasForeignKey(d => d.SalesEmployeeCode)
                    .HasConstraintName("FK_TransferModel_SalesEmployeeModel");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.TransferModels)
                    .HasForeignKey(d => d.StoreId)
                    .HasConstraintName("FK_TransferModel_StoreModel");
            });

            modelBuilder.Entity<Unfollow>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<VehicleInfoModel>(entity =>
            {
                entity.HasKey(e => e.VehicleId)
                    .HasName("PK_VehicleInfoModel_1");

                entity.Property(e => e.VehicleId).HasDefaultValueSql("(newid())");
            });

            modelBuilder.Entity<ViewCatalogCategory>(entity =>
            {
                entity.ToView("View_Catalog_Category");
            });

            modelBuilder.Entity<ViewFaceCheckIn>(entity =>
            {
                entity.ToView("View_FaceCheckIn", "Task");

                entity.Property(e => e.Avatar).IsUnicode(false);

                entity.Property(e => e.PersonId).IsUnicode(false);
            });

            modelBuilder.Entity<ViewFaceCheckOut>(entity =>
            {
                entity.ToView("View_FaceCheckOut", "Task");

                entity.Property(e => e.Avatar).IsUnicode(false);

                entity.Property(e => e.PersonId).IsUnicode(false);
            });

            modelBuilder.Entity<ViewPriorityModel>(entity =>
            {
                entity.ToView("View_PriorityModel");
            });

            modelBuilder.Entity<ViewProductProductInfo>(entity =>
            {
                entity.ToView("View_Product_ProductInfo", "tMasterData");
            });

            modelBuilder.Entity<ViewProfileAddress>(entity =>
            {
                entity.ToView("View_Profile_Address", "Customer");
            });

            modelBuilder.Entity<ViewProfileCompanyMapping>(entity =>
            {
                entity.ToView("View_Profile_Company_Mapping", "Customer");
            });

            modelBuilder.Entity<ViewProfileContactPhone>(entity =>
            {
                entity.ToView("View_Profile_ContactPhone", "Customer");
            });

            modelBuilder.Entity<ViewProfileDeletedContactPhone>(entity =>
            {
                entity.ToView("View_ProfileDeleted_ContactPhone", "Customer");
            });

            modelBuilder.Entity<ViewProfileExtendInfo>(entity =>
            {
                entity.ToView("View_Profile_ExtendInfo", "Customer");
            });

            modelBuilder.Entity<ViewProfileMainContact>(entity =>
            {
                entity.ToView("View_Profile_MainContact", "Customer");
            });

            modelBuilder.Entity<ViewProfileProfilePhone>(entity =>
            {
                entity.ToView("View_Profile_ProfilePhone", "Customer");
            });

            modelBuilder.Entity<ViewProfileProjectContractLoseValue>(entity =>
            {
                entity.ToView("View_Profile_ProjectContractLoseValue", "Customer");
            });

            modelBuilder.Entity<ViewProfileProjectContractValue>(entity =>
            {
                entity.ToView("View_Profile_ProjectContractValue", "Customer");
            });

            modelBuilder.Entity<ViewProfileProjectContractWonValue>(entity =>
            {
                entity.ToView("View_Profile_ProjectContractWonValue", "Customer");
            });

            modelBuilder.Entity<ViewStockDelivery>(entity =>
            {
                entity.ToView("View_Stock_Delivery", "Warehouse");
            });

            modelBuilder.Entity<ViewStockReceive>(entity =>
            {
                entity.ToView("View_Stock_Receive", "Warehouse");
            });

            modelBuilder.Entity<ViewStockTransferFromDelivery>(entity =>
            {
                entity.ToView("View_Stock_TransferFrom_Delivery", "Warehouse");
            });

            modelBuilder.Entity<ViewStockTransferToReceive>(entity =>
            {
                entity.ToView("View_Stock_TransferTo_Receive", "Warehouse");
            });

            modelBuilder.Entity<ViewTaskArea>(entity =>
            {
                entity.ToView("View_Task_Area", "Customer");
            });

            modelBuilder.Entity<ViewTaskGtb>(entity =>
            {
                entity.ToView("View_Task_GTB", "Task");
            });

            modelBuilder.Entity<WardModel>(entity =>
            {
                entity.Property(e => e.WardId).ValueGeneratedNever();
            });
          

            modelBuilder.Entity<WarehouseModel>(entity =>
            {
                entity.Property(e => e.WarehouseId).ValueGeneratedNever();
            });

            modelBuilder.Entity<WarehouseProductModel>(entity =>
            {
                entity.Property(e => e.WarehouseProductId).ValueGeneratedNever();

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.WarehouseProductModels)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WarehouseProductModel_ProductModel");

                entity.HasOne(d => d.Style)
                    .WithMany(p => p.WarehouseProductModels)
                    .HasForeignKey(d => d.StyleId)
                    .HasConstraintName("FK_WarehouseProductModel_StyleModel");

                entity.HasOne(d => d.Warehouse)
                    .WithMany(p => p.WarehouseProductModels)
                    .HasForeignKey(d => d.WarehouseId)
                    .HasConstraintName("FK_WarehouseProductModel_WarehouseModel");
            });

            modelBuilder.Entity<WarrantyModel>(entity =>
            {
                entity.Property(e => e.WarrantyId).ValueGeneratedNever();
            });

            modelBuilder.Entity<WorkFlowConfigModel>(entity =>
            {
                entity.HasKey(e => new { e.WorkFlowId, e.FieldCode });
            });

            modelBuilder.Entity<WorkFlowModel>(entity =>
            {
                entity.Property(e => e.WorkFlowId).ValueGeneratedNever();
            });

            modelBuilder.Entity<WorkingDateModel>(entity =>
            {
                entity.HasKey(e => e.WorkingDateId)
                    .HasName("PK_WorkingDateModel_1");

                entity.Property(e => e.WorkingDateId).ValueGeneratedNever();
            });

            modelBuilder.Entity<WorkingTimeConfigModel>(entity =>
            {
                entity.Property(e => e.WorkingTimeConfigId).ValueGeneratedNever();
            });

            modelBuilder.Entity<WorkingTimeDetailModel>(entity =>
            {
                entity.Property(e => e.WorkingTimeDetailId).ValueGeneratedNever();

                entity.HasOne(d => d.WorkingTime)
                    .WithMany(p => p.WorkingTimeDetailModels)
                    .HasForeignKey(d => d.WorkingTimeId)
                    .HasConstraintName("FK_WorkingTimeDetailModel_WorkingTimeModel");
            });

            modelBuilder.Entity<WorkingTimeModel>(entity =>
            {
                entity.Property(e => e.WorkingTimeId).ValueGeneratedNever();
            });
            modelBuilder.Entity<StockOnHand>(entity =>
            {
                entity.Property(e => e.ProductId).ValueGeneratedNever();
            });
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
