using ISD.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.Repositories.Infrastructure.Extensions
{
    public static class ProfileDeletedExtensions
    {
        public static void MapFromProfile(this ProfileDeletedModel model, ProfileModel profileModel)
        {
            model.ProfileId = profileModel.ProfileId;
            model.ProfileCode = profileModel.ProfileCode;
            model.ProfileForeignCode = profileModel.ProfileForeignCode;
            model.isForeignCustomer = profileModel.isForeignCustomer;
            model.CustomerTypeCode = profileModel.CustomerTypeCode;
            model.Title = profileModel.Title;
            model.ProfileName = profileModel.ProfileName;
            model.ProfileShortName = profileModel.ProfileShortName;
            model.AbbreviatedName = profileModel.AbbreviatedName;
            model.DayOfBirth = profileModel.DayOfBirth;
            model.MonthOfBirth = profileModel.MonthOfBirth;
            model.YearOfBirth = profileModel.YearOfBirth;
            model.Age = profileModel.Age;
            model.Phone = profileModel.Phone;
            model.SAPPhone = profileModel.SAPPhone;
            model.Email = profileModel.Email;
            model.Address = profileModel.Address;
            model.CountryCode = profileModel.CountryCode;
            model.SaleOfficeCode = profileModel.SaleOfficeCode;
            model.ProvinceId = profileModel.ProvinceId;
            model.DistrictId = profileModel.DistrictId;
            model.WardId = profileModel.WardId;
            model.Note = profileModel.Note;
            model.VisitDate = profileModel.VisitDate;
            model.Actived = profileModel.Actived;
            model.ImageUrl = profileModel.ImageUrl;
            model.CreateByEmployee = profileModel.CreateByEmployee;
            model.CreateAtCompany = profileModel.CreateAtCompany;
            model.CreateAtSaleOrg = profileModel.CreateAtSaleOrg;
            model.CreateBy = profileModel.CreateBy;
            model.CreateTime = profileModel.CreateTime;
            model.LastEditBy = profileModel.LastEditBy;
            model.LastEditTime = profileModel.LastEditTime;
            model.CustomerAccountGroupCode = profileModel.CustomerAccountGroupCode;
            model.CustomerGroupCode = profileModel.CustomerGroupCode;
            model.PaymentTermCode = profileModel.PaymentTermCode;
            model.CustomerAccountAssignmentGroupCode = profileModel.CustomerAccountAssignmentGroupCode;
            model.CashMgmtGroupCode = profileModel.CashMgmtGroupCode;
            model.ReconcileAccountCode = profileModel.ReconcileAccountCode;
            model.CustomerSourceCode = profileModel.CustomerSourceCode;
            model.TaxNo = profileModel.TaxNo;
            model.Website = profileModel.Website;
            model.CustomerCareerCode = profileModel.CustomerCareerCode;
            model.CompanyNumber = profileModel.CompanyNumber;
            model.AddressTypeCode = profileModel.AddressTypeCode;
            model.ProjectCode = profileModel.ProjectCode;
            model.ProjectStatusCode = profileModel.ProjectStatusCode;
            model.QualificationLevelCode = profileModel.QualificationLevelCode;
            model.ProjectSourceCode = profileModel.ProjectSourceCode;
            model.ReferenceProfileId = profileModel.ReferenceProfileId;
            model.ContractValue = profileModel.ContractValue;
            model.Text1 = profileModel.Text1;
            model.Text2 = profileModel.Text2;
            model.Text3 = profileModel.Text3;
            model.Text4 = profileModel.Text4;
            model.Text5 = profileModel.Text5;
            model.Dropdownlist1 = profileModel.Dropdownlist1;
            model.Dropdownlist2 = profileModel.Dropdownlist2;
            model.Dropdownlist3 = profileModel.Dropdownlist3;
            model.Dropdownlist4 = profileModel.Dropdownlist4;
            model.Dropdownlist5 = profileModel.Dropdownlist5;
            model.Dropdownlist6 = profileModel.Dropdownlist6;
            model.Dropdownlist7 = profileModel.Dropdownlist7;
            model.Dropdownlist8 = profileModel.Dropdownlist8;
            model.Dropdownlist9 = profileModel.Dropdownlist9;
            model.Dropdownlist10 = profileModel.Dropdownlist10;
            model.Date1 = profileModel.Date1;
            model.Date2 = profileModel.Date2;
            model.Date3 = profileModel.Date3;
            model.Date4 = profileModel.Date4;
            model.Date5 = profileModel.Date5;
            model.ProjectLocation = profileModel.ProjectLocation;
            model.IsAnCuongAccessory = profileModel.IsAnCuongAccessory;
            model.Laminate = profileModel.Laminate;
            model.MFC = profileModel.MFC;
            model.Veneer = profileModel.Veneer;
            model.Flooring = profileModel.Flooring;
            model.Accessories = profileModel.Accessories;
            model.KitchenEquipment = profileModel.KitchenEquipment;
            model.OtherBrand = profileModel.OtherBrand;
            model.HandoverFurniture = profileModel.HandoverFurniture;
        }

        public static void MapFromContactAttribute(this ProfileContactAttributeDeletedModel model, ProfileContactAttributeModel contactAttributeModel)
        {
            model.ProfileId = contactAttributeModel.ProfileId;
            model.CompanyId = contactAttributeModel.CompanyId;
            model.Position = contactAttributeModel.Position;
            model.DepartmentCode = contactAttributeModel.DepartmentCode;
            model.IsMain = contactAttributeModel.IsMain;
        }

        public static void MapFromProfilePhone(this ProfilePhoneDeletedModel model, ProfilePhoneModel profilePhoneModel)
        {
            model.PhoneId = profilePhoneModel.PhoneId;
            model.ProfileId = profilePhoneModel.ProfileId;
            model.PhoneNumber = profilePhoneModel.PhoneNumber;
        }

        public static void MapFromProfileEmail(this ProfileEmailDeletedModel model, ProfileEmailModel profileEmailModel)
        {
            model.EmailId = profileEmailModel.EmailId;
            model.ProfileId = profileEmailModel.ProfileId;
            model.Email = profileEmailModel.Email;
        }

        public static void MapFromPersonInCharge(this PersonInChargeDeletedModel model, PersonInChargeModel personInChargeModel)
        {
            model.PersonInChargeId = personInChargeModel.PersonInChargeId;
            model.ProfileId = personInChargeModel.ProfileId;
            model.SalesEmployeeCode = personInChargeModel.SalesEmployeeCode;
            model.CreateBy = personInChargeModel.CreateBy;
            model.CreateTime = personInChargeModel.CreateTime;
            model.RoleCode = personInChargeModel.RoleCode;
            model.CompanyCode = personInChargeModel.CompanyCode;
        }

        public static void MapFromRoleInCharge(this RoleInChargeDeletedModel model, RoleInChargeModel roleInChargeModel)
        {
            model.RoleInChargeId = roleInChargeModel.RoleInChargeId;
            model.ProfileId = roleInChargeModel.ProfileId;
            model.RolesId = roleInChargeModel.RolesId;
            model.CreateBy = roleInChargeModel.CreateBy;
            model.CreateTime = roleInChargeModel.CreateTime;
        }

        //Map ngược lại
        public static void MapFromProfileDeleted(this ProfileModel model, ProfileDeletedModel profileDeletedModel)
        {
            model.ProfileId = profileDeletedModel.ProfileId;
            model.ProfileCode = profileDeletedModel.ProfileCode;
            model.ProfileForeignCode = profileDeletedModel.ProfileForeignCode;
            model.isForeignCustomer = profileDeletedModel.isForeignCustomer;
            model.CustomerTypeCode = profileDeletedModel.CustomerTypeCode;
            model.Title = profileDeletedModel.Title;
            model.ProfileName = profileDeletedModel.ProfileName;
            model.ProfileShortName = profileDeletedModel.ProfileShortName;
            model.AbbreviatedName = profileDeletedModel.AbbreviatedName;
            model.DayOfBirth = profileDeletedModel.DayOfBirth;
            model.MonthOfBirth = profileDeletedModel.MonthOfBirth;
            model.YearOfBirth = profileDeletedModel.YearOfBirth;
            model.Age = profileDeletedModel.Age;
            model.Phone = profileDeletedModel.Phone;
            model.SAPPhone = profileDeletedModel.SAPPhone;
            model.Email = profileDeletedModel.Email;
            model.Address = profileDeletedModel.Address;
            model.CountryCode = profileDeletedModel.CountryCode;
            model.SaleOfficeCode = profileDeletedModel.SaleOfficeCode;
            model.ProvinceId = profileDeletedModel.ProvinceId;
            model.DistrictId = profileDeletedModel.DistrictId;
            model.WardId = profileDeletedModel.WardId;
            model.Note = profileDeletedModel.Note;
            model.VisitDate = profileDeletedModel.VisitDate;
            model.Actived = profileDeletedModel.Actived;
            model.ImageUrl = profileDeletedModel.ImageUrl;
            model.CreateByEmployee = profileDeletedModel.CreateByEmployee;
            model.CreateAtCompany = profileDeletedModel.CreateAtCompany;
            model.CreateAtSaleOrg = profileDeletedModel.CreateAtSaleOrg;
            model.CreateBy = profileDeletedModel.CreateBy;
            model.CreateTime = profileDeletedModel.CreateTime;
            model.LastEditBy = profileDeletedModel.LastEditBy;
            model.LastEditTime = profileDeletedModel.LastEditTime;
            model.CustomerAccountGroupCode = profileDeletedModel.CustomerAccountGroupCode;
            model.CustomerGroupCode = profileDeletedModel.CustomerGroupCode;
            model.PaymentTermCode = profileDeletedModel.PaymentTermCode;
            model.CustomerAccountAssignmentGroupCode = profileDeletedModel.CustomerAccountAssignmentGroupCode;
            model.CashMgmtGroupCode = profileDeletedModel.CashMgmtGroupCode;
            model.ReconcileAccountCode = profileDeletedModel.ReconcileAccountCode;
            model.CustomerSourceCode = profileDeletedModel.CustomerSourceCode;
            model.TaxNo = profileDeletedModel.TaxNo;
            model.Website = profileDeletedModel.Website;
            model.CustomerCareerCode = profileDeletedModel.CustomerCareerCode;
            model.CompanyNumber = profileDeletedModel.CompanyNumber;
            model.AddressTypeCode = profileDeletedModel.AddressTypeCode;
            model.ProjectCode = profileDeletedModel.ProjectCode;
            model.ProjectStatusCode = profileDeletedModel.ProjectStatusCode;
            model.QualificationLevelCode = profileDeletedModel.QualificationLevelCode;
            model.ProjectSourceCode = profileDeletedModel.ProjectSourceCode;
            model.ReferenceProfileId = profileDeletedModel.ReferenceProfileId;
            model.ContractValue = profileDeletedModel.ContractValue;
            model.Text1 = profileDeletedModel.Text1;
            model.Text2 = profileDeletedModel.Text2;
            model.Text3 = profileDeletedModel.Text3;
            model.Text4 = profileDeletedModel.Text4;
            model.Text5 = profileDeletedModel.Text5;
            model.Dropdownlist1 = profileDeletedModel.Dropdownlist1;
            model.Dropdownlist2 = profileDeletedModel.Dropdownlist2;
            model.Dropdownlist3 = profileDeletedModel.Dropdownlist3;
            model.Dropdownlist4 = profileDeletedModel.Dropdownlist4;
            model.Dropdownlist5 = profileDeletedModel.Dropdownlist5;
            model.Dropdownlist6 = profileDeletedModel.Dropdownlist6;
            model.Dropdownlist7 = profileDeletedModel.Dropdownlist7;
            model.Dropdownlist8 = profileDeletedModel.Dropdownlist8;
            model.Dropdownlist9 = profileDeletedModel.Dropdownlist9;
            model.Dropdownlist10 = profileDeletedModel.Dropdownlist10;
            model.Date1 = profileDeletedModel.Date1;
            model.Date2 = profileDeletedModel.Date2;
            model.Date3 = profileDeletedModel.Date3;
            model.Date4 = profileDeletedModel.Date4;
            model.Date5 = profileDeletedModel.Date5;
            model.ProjectLocation = profileDeletedModel.ProjectLocation;
            model.IsAnCuongAccessory = profileDeletedModel.IsAnCuongAccessory;
            model.Laminate = profileDeletedModel.Laminate;
            model.MFC = profileDeletedModel.MFC;
            model.Veneer = profileDeletedModel.Veneer;
            model.Flooring = profileDeletedModel.Flooring;
            model.Accessories = profileDeletedModel.Accessories;
            model.KitchenEquipment = profileDeletedModel.KitchenEquipment;
            model.OtherBrand = profileDeletedModel.OtherBrand;
            model.HandoverFurniture = profileDeletedModel.HandoverFurniture;
        }

        public static void MapFromContactAttributeDeleted(this ProfileContactAttributeModel model, ProfileContactAttributeDeletedModel profileContactAttributeDeletedModel)
        {
            model.ProfileId = profileContactAttributeDeletedModel.ProfileId;
            model.CompanyId = profileContactAttributeDeletedModel.CompanyId;
            model.Position = profileContactAttributeDeletedModel.Position;
            model.DepartmentCode = profileContactAttributeDeletedModel.DepartmentCode;
            model.IsMain = profileContactAttributeDeletedModel.IsMain;
        }

        public static void MapFromProfilePhoneDeleted(this ProfilePhoneModel model, ProfilePhoneDeletedModel profilePhoneDeletedModel)
        {
            model.PhoneId = profilePhoneDeletedModel.PhoneId;
            model.ProfileId = profilePhoneDeletedModel.ProfileId;
            model.PhoneNumber = profilePhoneDeletedModel.PhoneNumber;
        }

        public static void MapFromProfileEmailDeleted(this ProfileEmailModel model, ProfileEmailDeletedModel profileEmailDeletedModel)
        {
            model.EmailId = profileEmailDeletedModel.EmailId;
            model.ProfileId = profileEmailDeletedModel.ProfileId;
            model.Email = profileEmailDeletedModel.Email;
        }

        public static void MapFromPersonInChargeDeleted(this PersonInChargeModel model, PersonInChargeDeletedModel personInChargeDeletedModel)
        {
            model.PersonInChargeId = personInChargeDeletedModel.PersonInChargeId;
            model.ProfileId = personInChargeDeletedModel.ProfileId;
            model.SalesEmployeeCode = personInChargeDeletedModel.SalesEmployeeCode;
            model.CreateBy = personInChargeDeletedModel.CreateBy;
            model.CreateTime = personInChargeDeletedModel.CreateTime;
            model.RoleCode = personInChargeDeletedModel.RoleCode;
            model.CompanyCode = personInChargeDeletedModel.CompanyCode;
        }

        public static void MapFromRoleInChargeDeleted(this RoleInChargeModel model, RoleInChargeDeletedModel roleInChargeDeletedModel)
        {
            model.RoleInChargeId = roleInChargeDeletedModel.RoleInChargeId;
            model.ProfileId = roleInChargeDeletedModel.ProfileId;
            model.RolesId = roleInChargeDeletedModel.RolesId;
            model.CreateBy = roleInChargeDeletedModel.CreateBy;
            model.CreateTime = roleInChargeDeletedModel.CreateTime;
        }

    }
}
