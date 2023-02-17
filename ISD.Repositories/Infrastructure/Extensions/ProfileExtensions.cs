using ISD.Constant;
using ISD.EntityModels;
using ISD.ViewModels;

namespace ISD.Repositories.Infrastructure.Extensions
{
    public static class ProfileExtensions
    {
        public static void MapProfile(this ProfileModel profileModel, ProfileViewModel profileViewModel)
        {
            profileModel.ProfileId = profileViewModel.ProfileId;
            //profileModel.ProfileCode = profileViewModel.ProfileCode;
            //profileModel.ProfileForeignCode = profileViewModel.ProfileForeignCode;
            //Đối tượng (Trong nước: false| Nước ngoài: true)
            profileModel.isForeignCustomer = profileViewModel.isForeignCustomer;
            /*
            Khi chọn đối tượng KH => cập nhật field CustomerAccountGroupCode (Phân nhóm khách hàng).
                  Z002	KH trong nước
                  Z003	KH nước ngoài
           */
            if (profileModel.isForeignCustomer == true)
            {
                profileModel.CustomerAccountGroupCode = "Z003";
            }
            else
            {
                profileModel.CustomerAccountGroupCode = "Z002";
            }
            //Loại (B | C | Contact)
            profileModel.CustomerTypeCode = profileViewModel.CustomerTypeCode;
            //Danh xưng
            profileModel.Title = profileViewModel.Title;
            //Họ va Tên|Tên công ty
            profileModel.ProfileName = profileViewModel.ProfileName;
            //Tên ngắn
            profileModel.ProfileShortName = profileViewModel.ProfileShortName;
            //Tên viết tắt
            profileModel.AbbreviatedName = profileViewModel.AbbreviatedName;
            //Ngày sinh
            profileModel.DayOfBirth = profileViewModel.DayOfBirth;
            //Tháng sinh
            profileModel.MonthOfBirth = profileViewModel.MonthOfBirth;
            //Năm sinh
            profileModel.YearOfBirth = profileViewModel.YearOfBirth;
            //Độ tuổi
            profileModel.Age = profileViewModel.Age;
            //Số điện thoại
            profileModel.Phone = profileViewModel.Phone;
            //profileModel.SAPPhone = profileViewModel.SAPPhone;
            //Email
            profileModel.Email = profileViewModel.Email;
            //Địa chỉ
            profileModel.Address = profileViewModel.Address;
            //Khu vực
            profileModel.SaleOfficeCode = profileViewModel.SaleOfficeCode;
            //Tỉnh/Thành phố
            profileModel.ProvinceId = profileViewModel.ProvinceId;
            //Quận/Huyện
            profileModel.DistrictId = profileViewModel.DistrictId;
            //Phường/Xã
            profileModel.WardId = profileViewModel.WardId;
            //Ghi chú
            profileModel.Note = profileViewModel.Note;
            //Ngày ghé thăm
            profileModel.VisitDate = profileViewModel.VisitDate;
            // Hình ảnh
            profileModel.ImageUrl = profileViewModel.ImageUrl;
            //Nhân viên tạo
            profileModel.CreateByEmployee = profileViewModel.CreateByEmployee;
            //Tạo tại công ty
            profileModel.CreateAtCompany = profileViewModel.CreateAtCompany;
            //Tạo tại cửa hàng
            profileModel.CreateAtSaleOrg = profileViewModel.CreateAtSaleOrg;

            //profileModel.CustomerAccountGroupCode = profileViewModel.CustomerAccountGroupCode;
            //profileModel.CustomerGroupCode = profileViewModel.CustomerGroupCode;
            //profileModel.PaymentTermCode = profileViewModel.PaymentTermCode;
            //profileModel.CustomerAccountAssignmentGroupCode = profileViewModel.CustomerAccountAssignmentGroupCode;
            //profileModel.CashMgmtGroupCode = profileViewModel.CashMgmtGroupCode;
            // profileModel.ReconcileAccountCode = profileViewModel.ReconcileAccountCode;

            //Nguồn khách hàng
            profileModel.CustomerSourceCode = profileViewModel.CustomerSourceCode;
            //Trạng thái
            profileModel.Actived = profileViewModel.Actived;
            //Nhóm khách hàng
            profileModel.CustomerGroupCode = profileViewModel.CustomerGroupCode;
            profileModel.CompanyNumber = profileViewModel.CompanyNumber;
            //Website
            profileModel.Website = profileViewModel.Website;
            //TaxNo
            profileModel.TaxNo = profileViewModel.TaxNo;
            //Ngành nghề
            profileModel.CustomerCareerCode = profileViewModel.CustomerCareerCode;
            //Loại địa chỉ
            profileModel.AddressTypeCode = profileViewModel.AddressTypeCode;
            //Yêu cầu tạo khách ở ECC
            profileModel.isCreateRequest = profileViewModel.isCreateRequest;
            if (profileViewModel.isCreateRequest.HasValue && profileViewModel.isCreateRequest == true)
            {
                profileModel.CreateRequestTime = profileViewModel.CreateTime;
            }
        }

        public static void MapProfileContact(this ProfileContactAttributeModel profileContacModel, ProfileViewModel profileViewModel)
        {
            profileContacModel.ProfileId = profileViewModel.ProfileId;
            profileContacModel.CompanyId = profileViewModel.CompanyId;
            profileContacModel.Position = profileViewModel.PositionB;
            profileContacModel.DepartmentCode = profileViewModel.DepartmentCode;
            profileContacModel.IsMain = profileViewModel.IsMain;
        }
    }
}