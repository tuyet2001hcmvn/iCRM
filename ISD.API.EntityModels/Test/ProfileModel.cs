using System;
using System.Collections.Generic;

#nullable disable

namespace ISD.API.EntityModels.Test
{
    public partial class ProfileModel
    {
        public ProfileModel()
        {
            TemplateAndGiftMemberModels = new HashSet<TemplateAndGiftMemberModel>();
        }

        public Guid ProfileId { get; set; }
        public int ProfileCode { get; set; }
        public string ProfileForeignCode { get; set; }
        public bool? IsForeignCustomer { get; set; }
        public string CustomerTypeCode { get; set; }
        public string Title { get; set; }
        public string ProfileName { get; set; }
        public string ProfileShortName { get; set; }
        public string AbbreviatedName { get; set; }
        public int? DayOfBirth { get; set; }
        public int? MonthOfBirth { get; set; }
        public int? YearOfBirth { get; set; }
        public string Age { get; set; }
        public string Phone { get; set; }
        public string Sapphone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string CountryCode { get; set; }
        public string SaleOfficeCode { get; set; }
        public Guid? ProvinceId { get; set; }
        public Guid? DistrictId { get; set; }
        public Guid? WardId { get; set; }
        public string Note { get; set; }
        public DateTime? VisitDate { get; set; }
        public bool? Actived { get; set; }
        public string ImageUrl { get; set; }
        public string CreateByEmployee { get; set; }
        public string CreateAtCompany { get; set; }
        public string CreateAtSaleOrg { get; set; }
        public Guid? CreateBy { get; set; }
        public DateTime? CreateTime { get; set; }
        public Guid? LastEditBy { get; set; }
        public DateTime? LastEditTime { get; set; }
        public string CustomerAccountGroupCode { get; set; }
        public string CustomerGroupCode { get; set; }
        public string PaymentTermCode { get; set; }
        public string CustomerAccountAssignmentGroupCode { get; set; }
        public string CashMgmtGroupCode { get; set; }
        public string ReconcileAccountCode { get; set; }
        public string CustomerSourceCode { get; set; }
        public string TaxNo { get; set; }
        public string Website { get; set; }
        public string CustomerCareerCode { get; set; }
        public string CompanyNumber { get; set; }
        public string AddressTypeCode { get; set; }
        public int? ProjectCode { get; set; }
        public string ProjectStatusCode { get; set; }
        public string QualificationLevelCode { get; set; }
        public string ProjectSourceCode { get; set; }
        public Guid? ReferenceProfileId { get; set; }
        public Guid? ReferenceProfileId2 { get; set; }
        public decimal? ContractValue { get; set; }
        public string Text1 { get; set; }
        public string Text2 { get; set; }
        public string Text3 { get; set; }
        public string Text4 { get; set; }
        public string Text5 { get; set; }
        public string Dropdownlist1 { get; set; }
        public string Dropdownlist2 { get; set; }
        public string Dropdownlist3 { get; set; }
        public string Dropdownlist4 { get; set; }
        public string Dropdownlist5 { get; set; }
        public string Dropdownlist6 { get; set; }
        public string Dropdownlist7 { get; set; }
        public string Dropdownlist8 { get; set; }
        public string Dropdownlist9 { get; set; }
        public string Dropdownlist10 { get; set; }
        public DateTime? Date1 { get; set; }
        public DateTime? Date2 { get; set; }
        public DateTime? Date3 { get; set; }
        public DateTime? Date4 { get; set; }
        public DateTime? Date5 { get; set; }
        public string ProjectLocation { get; set; }
        public bool? IsAnCuongAccessory { get; set; }
        public string Laminate { get; set; }
        public string Mfc { get; set; }
        public string Veneer { get; set; }
        public string Flooring { get; set; }
        public string Accessories { get; set; }
        public string KitchenEquipment { get; set; }
        public string OtherBrand { get; set; }
        public string HandoverFurniture { get; set; }
        public bool? IsCreateRequest { get; set; }
        public DateTime? CreateRequestTime { get; set; }

        public virtual ICollection<TemplateAndGiftMemberModel> TemplateAndGiftMemberModels { get; set; }
    }
}
