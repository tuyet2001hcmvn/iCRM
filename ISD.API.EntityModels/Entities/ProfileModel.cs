using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ISD.API.EntityModels.Entities
{
    [Table("ProfileModel", Schema = "Customer")]
    public partial class ProfileModel
    {
        public ProfileModel()
        {
            AddressBookModels = new HashSet<AddressBookModel>();
            DeliveryModels = new HashSet<DeliveryModel>();
            PartnerModelPartnerProfiles = new HashSet<PartnerModel>();
            PartnerModelProfiles = new HashSet<PartnerModel>();
            PersonInChargeModels = new HashSet<PersonInChargeModel>();
            ProductWarrantyModels = new HashSet<ProductWarrantyModel>();
            ProfileEmailModels = new HashSet<ProfileEmailModel>();
            ProfileFileMappings = new HashSet<ProfileFileMapping>();
            ProfileGroupModels = new HashSet<ProfileGroupModel>();
            ProfileOpportunityCompetitorModels = new HashSet<ProfileOpportunityCompetitorModel>();
            ProfileOpportunityInternalModels = new HashSet<ProfileOpportunityInternalModel>();
            ProfileOpportunityMaterialModels = new HashSet<ProfileOpportunityMaterialModel>();
            ProfileOpportunityPartnerModelPartners = new HashSet<ProfileOpportunityPartnerModel>();
            ProfileOpportunityPartnerModelProfiles = new HashSet<ProfileOpportunityPartnerModel>();
            ProfilePhoneModels = new HashSet<ProfilePhoneModel>();
            RoleInChargeModels = new HashSet<RoleInChargeModel>();
            StockReceivingMasterModels = new HashSet<StockReceivingMasterModel>();
            TemplateAndGiftMemberModels = new HashSet<TemplateAndGiftMemberModel>();
        }

        [Key]
        public Guid ProfileId { get; set; }
        public int ProfileCode { get; set; }
        [StringLength(20)]
        public string ProfileForeignCode { get; set; }
        [Column("isForeignCustomer")]
        public bool? IsForeignCustomer { get; set; }
        [StringLength(20)]
        public string CustomerTypeCode { get; set; }
        [StringLength(10)]
        public string Title { get; set; }
        [StringLength(255)]
        public string ProfileName { get; set; }
        [StringLength(255)]
        public string ProfileShortName { get; set; }
        [StringLength(50)]
        public string AbbreviatedName { get; set; }
        public int? DayOfBirth { get; set; }
        public int? MonthOfBirth { get; set; }
        public int? YearOfBirth { get; set; }
        [StringLength(10)]
        public string Age { get; set; }
        [StringLength(50)]
        public string Phone { get; set; }
        [Column("SAPPhone")]
        [StringLength(100)]
        public string Sapphone { get; set; }
        [StringLength(500)]
        public string Email { get; set; }
        [StringLength(4000)]
        public string Address { get; set; }
        [StringLength(10)]
        public string CountryCode { get; set; }
        [StringLength(10)]
        public string SaleOfficeCode { get; set; }
        public Guid? ProvinceId { get; set; }
        public Guid? DistrictId { get; set; }
        public Guid? WardId { get; set; }
        [StringLength(1000)]
        public string Note { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? VisitDate { get; set; }
        public bool? Actived { get; set; }
        [StringLength(1000)]
        public string ImageUrl { get; set; }
        [StringLength(20)]
        public string CreateByEmployee { get; set; }
        [StringLength(50)]
        public string CreateAtCompany { get; set; }
        [StringLength(50)]
        public string CreateAtSaleOrg { get; set; }
        public Guid? CreateBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateTime { get; set; }
        public Guid? LastEditBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LastEditTime { get; set; }
        [StringLength(50)]
        public string CustomerAccountGroupCode { get; set; }
        [StringLength(50)]
        public string CustomerGroupCode { get; set; }
        [StringLength(50)]
        public string PaymentTermCode { get; set; }
        [StringLength(50)]
        public string CustomerAccountAssignmentGroupCode { get; set; }
        [StringLength(50)]
        public string CashMgmtGroupCode { get; set; }
        [StringLength(50)]
        public string ReconcileAccountCode { get; set; }
        [StringLength(50)]
        public string CustomerSourceCode { get; set; }
        [StringLength(50)]
        public string TaxNo { get; set; }
        [StringLength(50)]
        public string Website { get; set; }
        [StringLength(50)]
        public string CustomerCareerCode { get; set; }
        [StringLength(50)]
        public string CompanyNumber { get; set; }
        [StringLength(50)]
        public string AddressTypeCode { get; set; }
        public int? ProjectCode { get; set; }
        [StringLength(50)]
        public string ProjectStatusCode { get; set; }
        [StringLength(50)]
        public string QualificationLevelCode { get; set; }
        [StringLength(50)]
        public string ProjectSourceCode { get; set; }
        public Guid? ReferenceProfileId { get; set; }
        public Guid? ReferenceProfileId2 { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? ContractValue { get; set; }
        public string Text1 { get; set; }
        public string Text2 { get; set; }
        public string Text3 { get; set; }
        public string Text4 { get; set; }
        public string Text5 { get; set; }
        public string Text6 { get; set; }
        public string Text7 { get; set; }
        public string Text8 { get; set; }
        public string Text9 { get; set; }
        public string Text10 { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        public decimal? Number1 { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        public decimal? Number2 { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        public decimal? Number3 { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        public decimal? Number4 { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        public decimal? Number5 { get; set; }
        [StringLength(50)]
        public string Dropdownlist1 { get; set; }
        [StringLength(50)]
        public string Dropdownlist2 { get; set; }
        [StringLength(50)]
        public string Dropdownlist3 { get; set; }
        [StringLength(50)]
        public string Dropdownlist4 { get; set; }
        [StringLength(50)]
        public string Dropdownlist5 { get; set; }
        [StringLength(50)]
        public string Dropdownlist6 { get; set; }
        [StringLength(50)]
        public string Dropdownlist7 { get; set; }
        [StringLength(50)]
        public string Dropdownlist8 { get; set; }
        [StringLength(50)]
        public string Dropdownlist9 { get; set; }
        [StringLength(50)]
        public string Dropdownlist10 { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Date1 { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Date2 { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Date3 { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Date4 { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Date5 { get; set; }
        [StringLength(4000)]
        public string ProjectLocation { get; set; }
        public bool? IsAnCuongAccessory { get; set; }
        public bool? IsThiCong { get; set; }
        public string Laminate { get; set; }
        [Column("MFC")]
        public string Mfc { get; set; }
        public string Veneer { get; set; }
        public string Flooring { get; set; }
        public string Accessories { get; set; }
        public string KitchenEquipment { get; set; }
        public string OtherBrand { get; set; }
        public string HandoverFurniture { get; set; }
        [Column("isCreateRequest")]
        public bool? IsCreateRequest { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateRequestTime { get; set; }
        [StringLength(50)]
        public string PaymentMethodCode { get; set; }
        [StringLength(50)]
        public string PartnerFunctionCode { get; set; }
        [StringLength(50)]
        public string CurrencyCode { get; set; }
        [StringLength(50)]
        public string TaxClassificationCode { get; set; }
        [StringLength(50)]
        public string Manager { get; set; }
        [StringLength(50)]
        public string DebsEmployee { get; set; }
        [Column("IsSyncedFromSAP")]
        public bool? IsSyncedFromSap { get; set; }
        public bool? IsTopInvestor { get; set; }
        public bool? IsInvestor { get; set; }
        public bool? IsDesigner { get; set; }
        public bool? IsContractor { get; set; }
        public bool? AutoformatFullName { get; set; }

        [InverseProperty(nameof(AddressBookModel.Profile))]
        public virtual ICollection<AddressBookModel> AddressBookModels { get; set; }
        [InverseProperty(nameof(DeliveryModel.Profile))]
        public virtual ICollection<DeliveryModel> DeliveryModels { get; set; }
        [InverseProperty(nameof(PartnerModel.PartnerProfile))]
        public virtual ICollection<PartnerModel> PartnerModelPartnerProfiles { get; set; }
        [InverseProperty(nameof(PartnerModel.Profile))]
        public virtual ICollection<PartnerModel> PartnerModelProfiles { get; set; }
        [InverseProperty(nameof(PersonInChargeModel.Profile))]
        public virtual ICollection<PersonInChargeModel> PersonInChargeModels { get; set; }
        [InverseProperty(nameof(ProductWarrantyModel.Profile))]
        public virtual ICollection<ProductWarrantyModel> ProductWarrantyModels { get; set; }
        [InverseProperty(nameof(ProfileEmailModel.Profile))]
        public virtual ICollection<ProfileEmailModel> ProfileEmailModels { get; set; }
        [InverseProperty(nameof(ProfileFileMapping.Profile))]
        public virtual ICollection<ProfileFileMapping> ProfileFileMappings { get; set; }
        [InverseProperty(nameof(ProfileGroupModel.Profile))]
        public virtual ICollection<ProfileGroupModel> ProfileGroupModels { get; set; }
        [InverseProperty(nameof(ProfileOpportunityCompetitorModel.Profile))]
        public virtual ICollection<ProfileOpportunityCompetitorModel> ProfileOpportunityCompetitorModels { get; set; }
        [InverseProperty(nameof(ProfileOpportunityInternalModel.Profile))]
        public virtual ICollection<ProfileOpportunityInternalModel> ProfileOpportunityInternalModels { get; set; }
        [InverseProperty(nameof(ProfileOpportunityMaterialModel.Profile))]
        public virtual ICollection<ProfileOpportunityMaterialModel> ProfileOpportunityMaterialModels { get; set; }
        [InverseProperty(nameof(ProfileOpportunityPartnerModel.Partner))]
        public virtual ICollection<ProfileOpportunityPartnerModel> ProfileOpportunityPartnerModelPartners { get; set; }
        [InverseProperty(nameof(ProfileOpportunityPartnerModel.Profile))]
        public virtual ICollection<ProfileOpportunityPartnerModel> ProfileOpportunityPartnerModelProfiles { get; set; }
        [InverseProperty(nameof(ProfilePhoneModel.Profile))]
        public virtual ICollection<ProfilePhoneModel> ProfilePhoneModels { get; set; }
        [InverseProperty(nameof(RoleInChargeModel.Profile))]
        public virtual ICollection<RoleInChargeModel> RoleInChargeModels { get; set; }
        [InverseProperty(nameof(StockReceivingMasterModel.Profile))]
        public virtual ICollection<StockReceivingMasterModel> StockReceivingMasterModels { get; set; }
        [InverseProperty(nameof(TemplateAndGiftMemberModel.Profile))]
        public virtual ICollection<TemplateAndGiftMemberModel> TemplateAndGiftMemberModels { get; set; }
    }
}
