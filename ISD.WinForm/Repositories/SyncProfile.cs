using ISD.Constant;
using ISD.EntityModels;
using ISD.Extensions;
using ISD.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ISD.WinForm.Repositories
{
    public class SyncProfile
    {
        private static string appPath = System.IO.Path.GetDirectoryName(new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).LocalPath);
        private static string logFilePath = System.IO.Path.Combine(appPath, DateTime.Now.ToString("yyyyMMdd") + "-SyncAllProfileFromSAP" + ".log");
        public Guid SYSTEM = new Guid("FD68F5F8-01F9-480F-ACB7-BA5D74D299C8");

        EntityDataContext _context;
        public SyncProfile(EntityDataContext db)
        {
            _context = db;
            _context.Database.CommandTimeout = 1800;
        }

        public void Sync()
        {
            //1. Lấy danh sách các KH chưa đánh dấu đồng bộ.
            //2. Đồng bộ về DB và đánh dấu lại là đã đồng bộ.
            var syncProfileList = _context.ProfileModel.Where(p => p.ProfileForeignCode != null && p.IsSyncedFromSAP == false)
                                                        .OrderBy(p => p.ProfileForeignCode)
                                                        .Take(1000).ToList();
           
            if (syncProfileList != null && syncProfileList.Count > 0)
            {
                foreach (var item in syncProfileList)
                {
                    //new Thread(delegate ()
                    //{
                    //    SyncProfileItem(item);
                    //}).Start();

                    SyncProfileItem(item);
                }
            }
        }

        private bool SyncProfileItem(ProfileModel profile)
        {
            try
            {
                #region Get datatable
                //Khởi tạo thư viện và kết nối
                var _sap = new SAPRepository();
                var destination = _sap.GetRfcWithConfig();
                //Định nghĩa hàm cần gọi
                var function = destination.Repository.CreateFunction(ConstantFunctionName.YAC_FM_CRM_GET_DATALIST);
                var newdate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                var date = new DateTime(2019, 01, 01);

                var dt = new DataTable();
                var companyList = _context.CompanyModel.Select(p => p.CompanyCode).ToList();
                if (companyList != null && companyList.Count > 0)
                {
                    foreach (var item in companyList)
                    {
                        function.SetValue("IM_TYPE", 1);
                        function.SetValue("IM_FRDATE", date.ToString("yyyyMMdd"));
                        function.SetValue("IM_TODATE", DateTime.Now.ToString("yyyyMMdd"));
                        function.SetValue("IM_BUKRS", item);
                        function.SetValue("IM_VKORG", 1000);
                        function.SetValue("IM_KUNNR", profile.ProfileForeignCode.Length < 10 ? "00" + profile.ProfileForeignCode : profile.ProfileForeignCode);

                        function.Invoke(destination);
                        var datatable = function.GetTable("DATA_T").ToDataTable("DATA_T");
                        dt.Merge(datatable);
                    }
                }
                #endregion

                if (dt != null && dt.Rows.Count > 0)
                {
                    try
                    {
                        //Tỉnh thành
                        var provinceLst = _context.ProvinceModel.Where(p => p.Actived == true).ToList();
                        //Quận huyện
                        var districtLst = _context.DistrictModel.Where(p => p.Actived == true && p.ProvinceId != null && p.DistrictName != null).AsNoTracking().ToList();
                        //Phường xã
                        var wardLst = _context.WardModel.Where(p => p.DistrictId != null && p.WardName != null).ToList();
                        //Nhân viên (SalesEmployee)
                        var employeeList = _context.SalesEmployeeModel.Select(p => p.SalesEmployeeCode).ToList();

                        //_context.Configuration.AutoDetectChangesEnabled = false;
                        List<ProfileModel> profileList = new List<ProfileModel>();
                        List<string> profileCodeList = new List<string>();
                        List<ProfilePhoneModel> phoneList = new List<ProfilePhoneModel>();
                        List<ProfileBAttributeModel> bAttrList = new List<ProfileBAttributeModel>();
                        List<ProfileContactAttributeModel> contactList = new List<ProfileContactAttributeModel>();
                        List<PersonInChargeModel> personList = new List<PersonInChargeModel>();
                        List<RoleInChargeModel> roleList = new List<RoleInChargeModel>();
                        List<AddressBookModel> addressList = new List<AddressBookModel>();
                        List<ProfileTypeModel> typeList = new List<ProfileTypeModel>();
                        List<ProfileGroupModel> groupList = new List<ProfileGroupModel>();
                        //List<ExistProfileModel> existList = new List<ExistProfileModel>();

                        foreach (DataRow item in dt.Rows)
                        {
                            try
                            {
                                //Mã SAP
                                var ProfileForeignCode = item["KUNNR"].ToString();
                                //Mã CRM
                                var ProfileCode_Str = item["LOCCO"].ToString();
                                int ProfileCode = 0;
                                if (!string.IsNullOrEmpty(ProfileCode_Str))
                                {
                                    ProfileCode = Convert.ToInt32(item["LOCCO"].ToString());
                                }
                                //Mã công ty
                                var CompanyCode = item["BUKRS"].ToString();
                                //Mã SAP lớn hơn 8 ký tự là loại địa chỉ
                                bool isAddressBook = false;
                                if (ProfileForeignCode.Length > 8)
                                {
                                    isAddressBook = true;
                                }
                                //Tạo mới ProfileId
                                var ProfileId = Guid.NewGuid();
                                //DateTime
                                UtilitiesRepository _utilities = new UtilitiesRepository();
                                var CreateTime = _utilities.ConvertDateTime(item["ERDAT"].ToString());
                                var Phone = item["TEL_NUMBER"].ToString();
                                var TaxNo = item["STCEG"].ToString();
                                //Trong nước | Nước ngoài: 
                                //1. Nếu LAND1 == "VN" => trong nước
                                //2. Nếu LAND1 != "VN" => nước ngoài
                                bool isForeignCustomer = false;
                                string CountryCode = item["LAND1"].ToString();
                                if (!string.IsNullOrEmpty(CountryCode) && CountryCode != "VN")
                                {
                                    isForeignCustomer = true;
                                }
                                //Tỉnh thành
                                var ProvinceCode = item["VKGRP"].ToString();
                                Guid? ProvinceId = null;
                                //Nếu là khách nước ngoài => lưu province là quốc gia
                                if (isForeignCustomer == true && !string.IsNullOrEmpty(CountryCode))
                                {
                                    ProvinceId = provinceLst.Where(p => p.ProvinceCode == CountryCode)
                                                    .Select(p => p.ProvinceId).FirstOrDefault();
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(ProvinceCode))
                                    {
                                        ProvinceId = provinceLst.Where(p => p.ProvinceCode == ProvinceCode)
                                                                .Select(p => p.ProvinceId).FirstOrDefault();
                                    }
                                    else
                                    {
                                        //Nếu không có Tỉnh thành => Lưu "Khác"
                                        ProvinceId = Guid.Parse("33b02d3c-c3dc-41ca-a909-09c951b9ad22");
                                    }
                                }

                                //Quận huyện
                                //var DistrictCode = item["BZIRK"].ToString();
                                //Guid? DistrictId = null;
                                //if (!string.IsNullOrEmpty(DistrictCode) && DistrictCode != "999999")
                                //{
                                //    DistrictId = districtLst.Where(p => p.DistrictCode == DistrictCode)
                                //                            .Select(p => p.DistrictId).FirstOrDefault();
                                //}
                                var DistrictName = item["BAHNE"].ToString();
                                Guid? DistrictId = null;
                                if (!string.IsNullOrEmpty(DistrictName) && ProvinceId.HasValue)
                                {
                                    if (DistrictName == "Phan Rang - Tháp Chàm")
                                    {
                                        DistrictName = "Thành Phố " + DistrictName;
                                    }
                                    var district = districtLst.Where(p => DistrictName == (p.Appellation + " " + p.DistrictName) && p.ProvinceId == ProvinceId).FirstOrDefault();
                                    if (district != null)
                                    {
                                        DistrictId = district.DistrictId;
                                    }
                                }
                                //Phường xã
                                //var WardName = item["BAHNS_BAHNE"].ToString();
                                var WardName = item["BAHNS"].ToString();
                                Guid? WardId = null;
                                if (!string.IsNullOrEmpty(WardName) && DistrictId.HasValue)
                                {
                                    var ward = wardLst.Where(p => WardName == (p.Appellation + " " + p.WardName) && p.DistrictId == DistrictId).FirstOrDefault();
                                    if (ward != null)
                                    {
                                        WardId = ward.WardId;
                                    }
                                }
                                //Phân loại khách hàng: 1.KH Tiêu dùng. 2.KH Doanh nghiệp
                                var PhanLoaiKH = item["NHOMKH"].ToString();
                                var ProfileTypeCode = string.Empty;
                                if (!string.IsNullOrEmpty(PhanLoaiKH))
                                {
                                    if (PhanLoaiKH == "1")
                                    {
                                        ProfileTypeCode = ConstCustomerType.Customer;
                                    }
                                    else if (PhanLoaiKH == "2")
                                    {
                                        ProfileTypeCode = ConstCustomerType.Bussiness;
                                    }
                                }

                                //Email
                                string Email = item["SMTP_ADDR"].ToString();

                                #region Profile
                                //Check profile exists có mã CRM
                                var existProfileWithCRMCode = _context.ProfileModel.Where(p => p.CustomerTypeCode == ConstCustomerType.Account && p.ProfileCode == ProfileCode).FirstOrDefault();
                                //Nếu đã có profile trong DB và chưa có Mã SAP => cập nhật
                                if (existProfileWithCRMCode != null
                                    //Chưa có mã SAP thì cập nhật
                                    //&& string.IsNullOrEmpty(existProfileWithCRMCode.ProfileForeignCode)
                                    )
                                {
                                    if (isAddressBook == false)
                                    {
                                        //Nếu mã SAP đã tồn tại => check mã CRM
                                        // => Trùng mã CRM thì cập nhật
                                        // => Khác mã CRM thì báo cho user
                                        var existsSAPProfile = _context.ProfileModel.Where(p => p.ProfileForeignCode == ProfileForeignCode).Select(p => new
                                        {
                                            p.ProfileCode,
                                            p.ProfileForeignCode
                                        }).FirstOrDefault();
                                        //CRM chưa có mã SAP hoặc trùng mã CRM thì mới cập nhật
                                        if (existsSAPProfile == null || existsSAPProfile.ProfileCode == ProfileCode)
                                        {
                                            //1. GUID
                                            ProfileId = existProfileWithCRMCode.ProfileId;
                                            //2. ProfileCode
                                            //2. ProfileCode => cannot update identity column
                                            //if (ProfileCode != 0)
                                            //{
                                            //    existProfileWithCRMCode.ProfileCode = ProfileCode;
                                            //}
                                            //3. ProfileForeignCode
                                            existProfileWithCRMCode.ProfileForeignCode = ProfileForeignCode;
                                            //4. Đối tượng (Trong nước: false| Nước ngoài: true)
                                            existProfileWithCRMCode.isForeignCustomer = isForeignCustomer;
                                            existProfileWithCRMCode.CountryCode = CountryCode;
                                            //5. Danh xưng
                                            var Title = item["ANRED"].ToString();
                                            if (Title.Length > 10)
                                            {
                                                Title = null;
                                            }
                                            existProfileWithCRMCode.Title = Title;
                                            ////6. Loại
                                            existProfileWithCRMCode.CustomerTypeCode = ConstCustomerType.Account;

                                            //Phân loại KH
                                            var existProfileType = _context.ProfileTypeModel.Where(p => p.ProfileId == ProfileId && p.CompanyCode == CompanyCode).FirstOrDefault();
                                            if (existProfileType == null)
                                            {
                                                ProfileTypeModel profileType = new ProfileTypeModel();
                                                profileType.ProfileTypeId = Guid.NewGuid();
                                                profileType.ProfileId = ProfileId;
                                                //if (existProfileWithCRMCode.Title == "Company")
                                                //{
                                                //    profileType.CustomerTypeCode = ConstCustomerType.Bussiness;
                                                //}
                                                //else
                                                //{
                                                //    profileType.CustomerTypeCode = ConstCustomerType.Customer;
                                                //}
                                                profileType.CustomerTypeCode = ProfileTypeCode;
                                                profileType.CompanyCode = item["BUKRS"].ToString();
                                                profileType.CreateBy = SYSTEM;
                                                profileType.CreateTime = CreateTime;
                                                _context.Entry(profileType).State = EntityState.Added;
                                                //typeList.Add(profileType);
                                            }
                                            else
                                            {
                                                existProfileType.CustomerTypeCode = ProfileTypeCode;
                                                _context.Entry(existProfileType).State = EntityState.Modified;
                                            }
                                            //7. Họ va Tên|Tên công ty
                                            existProfileWithCRMCode.ProfileName = item["FULLNAME"].ToString();
                                            //8. Tên ngắn
                                            existProfileWithCRMCode.ProfileShortName = item["NAME1"].ToString();
                                            //9. Tên viết tắt
                                            existProfileWithCRMCode.AbbreviatedName = existProfileWithCRMCode.ProfileName?.ToAbbreviation();
                                            //10. Ngày sinh
                                            //11. Tháng sinh
                                            //12. Năm sinh
                                            //13. Độ tuổi
                                            //14. Số điện thoại
                                            try
                                            {
                                                //Delete all
                                                var phoneExistLst = _context.ProfilePhoneModel.Where(p => p.ProfileId == ProfileId).ToList();
                                                if (phoneExistLst != null && phoneExistLst.Count > 0)
                                                {
                                                    _context.ProfilePhoneModel.RemoveRange(phoneExistLst);
                                                }
                                                //Add again
                                                if (!string.IsNullOrEmpty(Phone))
                                                {
                                                    if (Phone.Contains("-"))
                                                    {
                                                        var arr = Phone.Split('-').ToList();
                                                        var PhoneNumber = RepositoryLibrary.ConvertToNoSpecialCharacters(arr[0]);

                                                        var phoneArray = arr.Where(p => !p.Contains(arr[0])).ToList();
                                                        foreach (var phoneItem in phoneArray)
                                                        {
                                                            ProfilePhoneModel phoneModel = new ProfilePhoneModel();
                                                            phoneModel.PhoneId = Guid.NewGuid();
                                                            phoneModel.ProfileId = ProfileId;
                                                            phoneModel.PhoneNumber = RepositoryLibrary.ConvertToNoSpecialCharacters(phoneItem).Trim();
                                                            if (PhoneNumber != phoneModel.PhoneNumber && !string.IsNullOrEmpty(phoneModel.PhoneNumber))
                                                            {
                                                                _context.Entry(phoneModel).State = EntityState.Added;
                                                                //phoneList.Add(phoneModel);
                                                            }
                                                        }
                                                        existProfileWithCRMCode.Phone = PhoneNumber.Trim();
                                                    }
                                                    else
                                                    {
                                                        existProfileWithCRMCode.Phone = RepositoryLibrary.ConvertToNoSpecialCharacters(Phone);
                                                    }
                                                }
                                                else
                                                {
                                                    existProfileWithCRMCode.Phone = null;
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                // ghi log
                                                string mess = ex.Message;
                                                if (ex.InnerException != null)
                                                {
                                                    mess = ex.InnerException.Message;
                                                    if (ex.InnerException.InnerException != null)
                                                    {
                                                        mess = ex.InnerException.InnerException.Message;
                                                    }
                                                }
                                                WriteLogFile(logFilePath, "Sync Data error: " + mess);
                                                continue;
                                            }
                                            //15. Email
                                            //existProfileWithCRMCode.Email = item["SMTP_ADDR"].ToString();
                                            //if (!string.IsNullOrEmpty(existProfileWithCRMCode.Email))
                                            //{
                                            //    if (IsValidEmail(existProfileWithCRMCode.Email) == false)
                                            //    {
                                            //        existProfileWithCRMCode.Note = string.Format("(Email: {0})", existProfileWithCRMCode.Email);
                                            //        existProfileWithCRMCode.Email = null;
                                            //    }
                                            //    else
                                            //    {
                                            //        if (!string.IsNullOrEmpty(existProfileWithCRMCode.Note) && existProfileWithCRMCode.Note.Contains("Email:"))
                                            //        {
                                            //            existProfileWithCRMCode.Note = null;
                                            //        }
                                            //    }
                                            //}
                                            //else
                                            //{
                                            //    existProfileWithCRMCode.Email = null;
                                            //}
                                            existProfileWithCRMCode.Email = null;
                                            if (!string.IsNullOrEmpty(Email))
                                            {
                                                try
                                                {
                                                    //Delete all
                                                    var emailExistLst = _context.ProfileEmailModel.Where(p => p.ProfileId == ProfileId).ToList();
                                                    if (emailExistLst != null && emailExistLst.Count > 0)
                                                    {
                                                        _context.ProfileEmailModel.RemoveRange(emailExistLst);
                                                    }
                                                    //Add again
                                                    if (Email.Contains(";"))
                                                    {
                                                        var emailArray = Email.Split(';').ToList();
                                                        foreach (var emailItem in emailArray)
                                                        {
                                                            if (IsValidEmail(emailItem) == true)
                                                            {
                                                                ProfileEmailModel emailModel = new ProfileEmailModel();
                                                                emailModel.EmailId = Guid.NewGuid();
                                                                emailModel.ProfileId = ProfileId;
                                                                emailModel.Email = emailItem.Trim();
                                                                _context.Entry(emailModel).State = EntityState.Added;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (IsValidEmail(Email) == true)
                                                        {
                                                            ProfileEmailModel emailModel = new ProfileEmailModel();
                                                            emailModel.EmailId = Guid.NewGuid();
                                                            emailModel.ProfileId = ProfileId;
                                                            emailModel.Email = Email.Trim();
                                                            _context.Entry(emailModel).State = EntityState.Added;
                                                        }
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    // ghi log
                                                    string mess = ex.Message;
                                                    if (ex.InnerException != null)
                                                    {
                                                        mess = ex.InnerException.Message;
                                                        if (ex.InnerException.InnerException != null)
                                                        {
                                                            mess = ex.InnerException.InnerException.Message;
                                                        }
                                                    }
                                                    WriteLogFile(logFilePath, "Sync Data error: " + mess);
                                                    continue;
                                                }
                                            }
                                            //16. Khu vực
                                            //existProfileWithCRMCode.SaleOfficeCode = item["VKBUR"].ToString();
                                            //=> Khi đồng bộ từ SAP về dựa vào tỉnh thành mình set luôn khu vực chứ không đồng bộ data khu vực từ SAP về tránh SAP sai
                                            var SaleOfficeCode = provinceLst.Where(p => p.ProvinceId == ProvinceId).Select(p => p.Area).FirstOrDefault();
                                            if (!string.IsNullOrEmpty(SaleOfficeCode))
                                            {
                                                existProfileWithCRMCode.SaleOfficeCode = SaleOfficeCode;
                                            }
                                            //17. Địa chỉ
                                            //existProfileWithCRMCode.Address = item["ADDRESS"].ToString();
                                            existProfileWithCRMCode.Address = !string.IsNullOrEmpty(item["STCD5"].ToString()) ? item["STCD5"].ToString() : item["ADDRESS"].ToString();
                                            //18. Tỉnh/Thành phố
                                            existProfileWithCRMCode.ProvinceId = ProvinceId;
                                            //19. Quận/Huyện
                                            existProfileWithCRMCode.DistrictId = DistrictId;
                                            //20. Phường/Xã
                                            existProfileWithCRMCode.WardId = WardId;
                                            //21. Ghi chú
                                            //22. Ngày ghé thăm
                                            //23. Trạng thái (not update this field)
                                            //Trạng thái hoạt động: X: ngưng hoạt động
                                            string TrangThaiHoatDong = item["LOEVM"].ToString();
                                            bool isActived = true;
                                            if (!string.IsNullOrEmpty(TrangThaiHoatDong) && TrangThaiHoatDong.ToUpper() == "X")
                                            {
                                                isActived = false;
                                            }
                                            if (existProfileWithCRMCode.Actived != isActived)
                                            {
                                                existProfileWithCRMCode.Actived = isActived;
                                            }
                                            //24. Hình ảnh
                                            //25. Nhân viên tạo
                                            //26. Tạo tại công ty 
                                            existProfileWithCRMCode.CreateAtCompany = item["BUKRS"].ToString();
                                            //27. Tạo tại cửa hàng
                                            existProfileWithCRMCode.CreateAtSaleOrg = item["VKORG"].ToString();
                                            //28. CreateBy (not update this field)
                                            //29. Thời gian tạo
                                            //exist.CreateTime = CreateTime;
                                            //30. LastEditBy
                                            existProfileWithCRMCode.LastEditBy = SYSTEM;
                                            //31. Thời gian sửa
                                            existProfileWithCRMCode.LastEditTime = DateTime.Now;
                                            //32. Phân nhóm KH/ Customer Account Group
                                            existProfileWithCRMCode.CustomerAccountGroupCode = item["KTOKD"].ToString();
                                            //33.Mã nhóm  KH/ Customer Group
                                            existProfileWithCRMCode.CustomerGroupCode = item["KDGRP"].ToString();
                                            //34. Mã Điều khoản thanh toán/ Payment Term
                                            existProfileWithCRMCode.PaymentTermCode = item["ZTERM"].ToString();
                                            //35.Mã phân loại doanh thu/ Customer Account Assignment Group
                                            existProfileWithCRMCode.CustomerAccountAssignmentGroupCode = item["KTGRD"].ToString();
                                            //36. Mã phân nhóm dòng tiền/ Cash mgmt Group
                                            existProfileWithCRMCode.CashMgmtGroupCode = item["FDGRV"].ToString();
                                            //37. Mã tài khoản công nợ/ Reconcile Account
                                            existProfileWithCRMCode.ReconcileAccountCode = item["AKONT"].ToString();
                                            //38. Số điện thoại (SAP)
                                            existProfileWithCRMCode.SAPPhone = Phone;
                                            //39. Mã số thuế TaxNo
                                            if (!string.IsNullOrEmpty(TaxNo) && !TaxNo.ToUpper().Contains("X"))
                                            {
                                                existProfileWithCRMCode.TaxNo = TaxNo;
                                            }
                                            else
                                            {
                                                existProfileWithCRMCode.TaxNo = null;
                                            }
                                            //nguồn KH
                                            existProfileWithCRMCode.CustomerSourceCode = "SHOWROOM";
                                            //Loại địa chỉ
                                            string LoaiDiaChi = item["ADDRTYPE"].ToString();
                                            if (!string.IsNullOrEmpty(LoaiDiaChi))
                                            {
                                                existProfileWithCRMCode.AddressTypeCode = LoaiDiaChi;
                                            }
                                            //Ngành nghề
                                            string NganhNghe = item["BRSCH"].ToString();
                                            if (!string.IsNullOrEmpty(NganhNghe))
                                            {
                                                //NganhNghe = NganhNghe.PadLeft(4, '0');

                                                var existProfileCareer = _context.ProfileCareerModel.Where(p => p.ProfileId == ProfileId && p.CompanyCode == CompanyCode).FirstOrDefault();
                                                if (existProfileCareer == null)
                                                {
                                                    ProfileCareerModel profileCareer = new ProfileCareerModel();
                                                    profileCareer.ProfileCareerId = Guid.NewGuid();
                                                    profileCareer.ProfileId = ProfileId;
                                                    profileCareer.ProfileCareerCode = NganhNghe;
                                                    profileCareer.CompanyCode = item["BUKRS"].ToString();
                                                    profileCareer.CreateBy = SYSTEM;
                                                    profileCareer.CreateTime = CreateTime;
                                                    _context.Entry(profileCareer).State = EntityState.Added;
                                                }
                                                else
                                                {
                                                    existProfileCareer.ProfileCareerCode = NganhNghe;
                                                    _context.Entry(existProfileCareer).State = EntityState.Modified;
                                                }
                                            }
                                            //Yêu cầu tạo khách ở ECC
                                            existProfileWithCRMCode.isCreateRequest = false;
                                            _context.Entry(existProfileWithCRMCode).State = EntityState.Modified;

                                            //thêm nhóm KH
                                            AddProfileGroup(item, CompanyCode, ProfileId);

                                            //thêm NVKD
                                            AddPersonInCharge(employeeList, item, CompanyCode, ProfileId, CreateTime);
                                        }
                                        else
                                        {
                                            WriteLogFile(logFilePath, "Sync Profile error: Update CRM: " + ProfileCode + " - Voi SAP: " + ProfileForeignCode + ". Nhung da Ton tai:" + existsSAPProfile.ProfileCode);
                                        }
                                        //profileCodeList.Add(ProfileForeignCode);
                                    }
                                    //cập nhật address book
                                    else
                                    {

                                    }
                                }

                                //Nếu không phải là yêu cầu cập nhật => tìm theo mã SAP
                                else if (existProfileWithCRMCode == null)
                                {
                                    //Tìm theo mã SAP
                                    var existProfileWithSAPCode = _context.ProfileModel.Where(p => p.ProfileForeignCode == ProfileForeignCode).FirstOrDefault();
                                    if (existProfileWithSAPCode == null)
                                    {
                                        //Nếu phân nhóm KH là Z012 thì không thêm vào DB
                                        var PNKH = item["KTOKD"].ToString();
                                        if (isAddressBook == false && (string.IsNullOrEmpty(PNKH) || PNKH != "Z012"))
                                        {
                                            ProfileModel model = new ProfileModel();
                                            //1. GUID
                                            model.ProfileId = ProfileId;
                                            //2. ProfileCode
                                            if (ProfileCode != 0)
                                            {
                                                model.ProfileCode = ProfileCode;
                                            }
                                            //3. ProfileForeignCode
                                            model.ProfileForeignCode = ProfileForeignCode;
                                            //4. Đối tượng (Trong nước: false| Nước ngoài: true)
                                            model.isForeignCustomer = isForeignCustomer;
                                            model.CountryCode = CountryCode;
                                            //5. Danh xưng
                                            var Title = item["ANRED"].ToString();
                                            if (Title.Length > 10)
                                            {
                                                Title = null;
                                            }
                                            model.Title = Title;
                                            //6. Loại
                                            model.CustomerTypeCode = ConstCustomerType.Account;

                                            #region  //Phân loại KH
                                            ProfileTypeModel profileType = new ProfileTypeModel();
                                            profileType.ProfileTypeId = Guid.NewGuid();
                                            profileType.ProfileId = ProfileId;
                                            //if (model.Title == "Company")
                                            //{
                                            //    profileType.CustomerTypeCode = ConstCustomerType.Bussiness;
                                            //}
                                            //else
                                            //{
                                            //    profileType.CustomerTypeCode = ConstCustomerType.Customer;
                                            //}
                                            profileType.CustomerTypeCode = ProfileTypeCode;
                                            profileType.CompanyCode = item["BUKRS"].ToString();
                                            profileType.CreateBy = SYSTEM;
                                            profileType.CreateTime = CreateTime;
                                            _context.Entry(profileType).State = EntityState.Added;
                                            #endregion
                                            //typeList.Add(profileType);
                                            //7. Họ va Tên|Tên công ty
                                            model.ProfileName = item["FULLNAME"].ToString();
                                            //8. Tên ngắn
                                            model.ProfileShortName = item["NAME1"].ToString();
                                            //9. Tên viết tắt
                                            model.AbbreviatedName = model.ProfileName.ToAbbreviation();
                                            //10. Ngày sinh
                                            //11. Tháng sinh
                                            //12. Năm sinh
                                            //13. Độ tuổi
                                            //14. Số điện thoại
                                            try
                                            {
                                                if (!string.IsNullOrEmpty(Phone))
                                                {
                                                    if (Phone.Contains("-"))
                                                    {
                                                        var arr = Phone.Split('-').ToList();
                                                        var PhoneNumber = RepositoryLibrary.ConvertToNoSpecialCharacters(arr[0]);
                                                        var phoneArray = arr.Where(p => !p.Contains(arr[0])).ToList();
                                                        foreach (var phoneItem in phoneArray)
                                                        {
                                                            ProfilePhoneModel phoneModel = new ProfilePhoneModel();
                                                            phoneModel.PhoneId = Guid.NewGuid();
                                                            phoneModel.ProfileId = ProfileId;
                                                            phoneModel.PhoneNumber = RepositoryLibrary.ConvertToNoSpecialCharacters(phoneItem).Trim();
                                                            if (PhoneNumber != phoneModel.PhoneNumber && !string.IsNullOrEmpty(phoneModel.PhoneNumber))
                                                            {
                                                                _context.Entry(phoneModel).State = EntityState.Added;
                                                                //phoneList.Add(phoneModel);
                                                            }
                                                        }
                                                        model.Phone = PhoneNumber.Trim();
                                                    }
                                                    else
                                                    {
                                                        model.Phone = RepositoryLibrary.ConvertToNoSpecialCharacters(Phone).Trim();
                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                // ghi log
                                                string mess = ex.Message;
                                                if (ex.InnerException != null)
                                                {
                                                    mess = ex.InnerException.Message;
                                                    if (ex.InnerException.InnerException != null)
                                                    {
                                                        mess = ex.InnerException.InnerException.Message;
                                                    }
                                                }
                                                WriteLogFile(logFilePath, "Sync Data error: " + mess);
                                                continue;
                                            }
                                            //15. Email
                                            //model.Email = item["SMTP_ADDR"].ToString();
                                            //if (!string.IsNullOrEmpty(model.Email) && IsValidEmail(model.Email) == false)
                                            //{
                                            //    model.Note = string.Format("(Email: {0})", model.Email);
                                            //    model.Email = null;
                                            //}
                                            //else
                                            //{
                                            //    model.Email = null;
                                            //}
                                            if (!string.IsNullOrEmpty(Email))
                                            {
                                                try
                                                {
                                                    //Delete all
                                                    var emailExistLst = _context.ProfileEmailModel.Where(p => p.ProfileId == ProfileId).ToList();
                                                    if (emailExistLst != null && emailExistLst.Count > 0)
                                                    {
                                                        _context.ProfileEmailModel.RemoveRange(emailExistLst);
                                                    }
                                                    //Add again
                                                    if (Email.Contains(";"))
                                                    {
                                                        var emailArray = Email.Split(';').ToList();
                                                        foreach (var emailItem in emailArray)
                                                        {
                                                            if (IsValidEmail(emailItem) == true)
                                                            {
                                                                ProfileEmailModel emailModel = new ProfileEmailModel();
                                                                emailModel.EmailId = Guid.NewGuid();
                                                                emailModel.ProfileId = ProfileId;
                                                                emailModel.Email = emailItem.Trim();
                                                                _context.Entry(emailModel).State = EntityState.Added;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (IsValidEmail(Email) == true)
                                                        {
                                                            ProfileEmailModel emailModel = new ProfileEmailModel();
                                                            emailModel.EmailId = Guid.NewGuid();
                                                            emailModel.ProfileId = ProfileId;
                                                            emailModel.Email = Email.Trim();
                                                            _context.Entry(emailModel).State = EntityState.Added;
                                                        }
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    // ghi log
                                                    string mess = ex.Message;
                                                    if (ex.InnerException != null)
                                                    {
                                                        mess = ex.InnerException.Message;
                                                        if (ex.InnerException.InnerException != null)
                                                        {
                                                            mess = ex.InnerException.InnerException.Message;
                                                        }
                                                    }
                                                    WriteLogFile(logFilePath, "Sync Data error: " + mess);
                                                    continue;
                                                }
                                            }
                                            //16. Khu vực
                                            //model.SaleOfficeCode = item["VKBUR"].ToString();
                                            //=> Khi đồng bộ từ SAP về dựa vào tỉnh thành mình set luôn khu vực chứ không đồng bộ data khu vực từ SAP về tránh SAP sai
                                            var SaleOfficeCode = provinceLst.Where(p => p.ProvinceId == ProvinceId).Select(p => p.Area).FirstOrDefault();
                                            if (!string.IsNullOrEmpty(SaleOfficeCode))
                                            {
                                                model.SaleOfficeCode = SaleOfficeCode;
                                            }
                                            //17. Địa chỉ
                                            //model.Address = item["ADDRESS"].ToString();
                                            model.Address = !string.IsNullOrEmpty(item["STCD5"].ToString()) ? item["STCD5"].ToString() : item["ADDRESS"].ToString();
                                            //18. Tỉnh/Thành phố
                                            model.ProvinceId = ProvinceId;
                                            //19. Quận/Huyện
                                            model.DistrictId = DistrictId;
                                            //20. Phường/Xã
                                            model.WardId = WardId;
                                            //21. Ghi chú
                                            //22. Ngày ghé thăm
                                            //23. Trạng thái
                                            //model.Actived = true;
                                            //Trạng thái hoạt động: X: ngưng hoạt động
                                            string TrangThaiHoatDong = item["LOEVM"].ToString();
                                            bool isActived = true;
                                            if (!string.IsNullOrEmpty(TrangThaiHoatDong) && TrangThaiHoatDong.ToUpper() == "X")
                                            {
                                                isActived = false;
                                            }
                                            model.Actived = isActived;
                                            //24. Hình ảnh
                                            //25. Nhân viên tạo
                                            //26. Tạo tại công ty
                                            model.CreateAtCompany = item["BUKRS"].ToString();
                                            //27. Tạo tại cửa hàng
                                            model.CreateAtSaleOrg = item["VKORG"].ToString();
                                            //28. CreateBy
                                            model.CreateBy = SYSTEM;
                                            //29. Thời gian tạo
                                            model.CreateTime = CreateTime;
                                            //30. LastEditBy
                                            //31. Thời gian tạo
                                            //32. Phân nhóm KH/ Customer Account Group
                                            model.CustomerAccountGroupCode = item["KTOKD"].ToString();
                                            //33.Mã nhóm  KH/ Customer Group
                                            model.CustomerGroupCode = item["KDGRP"].ToString();
                                            //34. Mã Điều khoản thanh toán/ Payment Term
                                            model.PaymentTermCode = item["ZTERM"].ToString();
                                            //35.Mã phân loại doanh thu/ Customer Account Assignment Group
                                            model.CustomerAccountAssignmentGroupCode = item["KTGRD"].ToString();
                                            //36. Mã phân nhóm dòng tiền/ Cash mgmt Group
                                            model.CashMgmtGroupCode = item["FDGRV"].ToString();
                                            //37. Mã tài khoản công nợ/ Reconcile Account
                                            model.ReconcileAccountCode = item["AKONT"].ToString();
                                            //38. Số điện thoại (SAP)
                                            model.SAPPhone = Phone;
                                            //39. Mã số thuế TaxNo
                                            if (!string.IsNullOrEmpty(TaxNo) && !TaxNo.ToUpper().Contains("X"))
                                            {
                                                model.TaxNo = TaxNo;
                                            }
                                            //nguồn KH
                                            model.CustomerSourceCode = "SHOWROOM";
                                            //Loại địa chỉ
                                            string LoaiDiaChi = item["ADDRTYPE"].ToString();
                                            if (!string.IsNullOrEmpty(LoaiDiaChi))
                                            {
                                                model.AddressTypeCode = LoaiDiaChi;
                                            }
                                            //Ngành nghề
                                            string NganhNghe = item["BRSCH"].ToString();
                                            if (!string.IsNullOrEmpty(NganhNghe))
                                            {
                                                //NganhNghe = NganhNghe.PadLeft(4, '0');

                                                ProfileCareerModel profileCareer = new ProfileCareerModel();
                                                profileCareer.ProfileCareerId = Guid.NewGuid();
                                                profileCareer.ProfileId = ProfileId;
                                                profileCareer.ProfileCareerCode = NganhNghe;
                                                profileCareer.CompanyCode = item["BUKRS"].ToString();
                                                profileCareer.CreateBy = SYSTEM;
                                                profileCareer.CreateTime = CreateTime;
                                                _context.Entry(profileCareer).State = EntityState.Added;

                                            }
                                            //Yêu cầu tạo khách ở ECC
                                            model.isCreateRequest = false;

                                            _context.Entry(model).State = EntityState.Added;
                                            //thêm nhóm KH
                                            AddProfileGroup(item, CompanyCode, ProfileId);

                                            //thêm NVKD
                                            AddPersonInCharge(employeeList, item, CompanyCode, ProfileId, CreateTime);

                                            //profileCodeList.Add(ProfileForeignCode);
                                            //if (profileCodeList.Contains(ProfileForeignCode))
                                            //{
                                            //    profileList.Add(model);
                                            //}
                                        }
                                        //thêm address book
                                        else
                                        {

                                        }
                                    }
                                    else
                                    {
                                        if (isAddressBook == false)
                                        {
                                            //1. GUID
                                            ProfileId = existProfileWithSAPCode.ProfileId;
                                            //2. ProfileCode => cannot update ProfileCode => identity
                                            //if (ProfileCode != 0)
                                            //{
                                            //    existProfileWithSAPCode.ProfileCode = ProfileCode;
                                            //}
                                            //3. ProfileForeignCode
                                            existProfileWithSAPCode.ProfileForeignCode = ProfileForeignCode;
                                            //4. Đối tượng (Trong nước: false| Nước ngoài: true)
                                            existProfileWithSAPCode.isForeignCustomer = isForeignCustomer;
                                            existProfileWithSAPCode.CountryCode = CountryCode;
                                            //5. Danh xưng
                                            var Title = item["ANRED"].ToString();
                                            if (Title.Length > 10)
                                            {
                                                Title = null;
                                            }
                                            existProfileWithSAPCode.Title = Title;
                                            ////6. Loại
                                            existProfileWithSAPCode.CustomerTypeCode = ConstCustomerType.Account;

                                            //Phân loại KH
                                            var existProfileType = _context.ProfileTypeModel.Where(p => p.ProfileId == ProfileId && p.CompanyCode == CompanyCode).FirstOrDefault();
                                            if (existProfileType == null)
                                            {
                                                ProfileTypeModel profileType = new ProfileTypeModel();
                                                profileType.ProfileTypeId = Guid.NewGuid();
                                                profileType.ProfileId = ProfileId;
                                                //if (existProfileWithSAPCode.Title == "Company")
                                                //{
                                                //    profileType.CustomerTypeCode = ConstCustomerType.Bussiness;
                                                //}
                                                //else
                                                //{
                                                //    profileType.CustomerTypeCode = ConstCustomerType.Customer;
                                                //}
                                                profileType.CustomerTypeCode = ProfileTypeCode;
                                                profileType.CompanyCode = item["BUKRS"].ToString();
                                                profileType.CreateBy = SYSTEM;
                                                profileType.CreateTime = CreateTime;
                                                _context.Entry(profileType).State = EntityState.Added;
                                                //typeList.Add(profileType);
                                            }
                                            else
                                            {
                                                existProfileType.CustomerTypeCode = ProfileTypeCode;
                                                _context.Entry(existProfileType).State = EntityState.Modified;
                                            }
                                            //7. Họ va Tên|Tên công ty
                                            existProfileWithSAPCode.ProfileName = item["FULLNAME"].ToString();
                                            //8. Tên ngắn
                                            existProfileWithSAPCode.ProfileShortName = item["NAME1"].ToString();
                                            //9. Tên viết tắt
                                            existProfileWithSAPCode.AbbreviatedName = existProfileWithSAPCode.ProfileName.ToAbbreviation();
                                            //10. Ngày sinh
                                            //11. Tháng sinh
                                            //12. Năm sinh
                                            //13. Độ tuổi
                                            //14. Số điện thoại
                                            try
                                            {
                                                //Delete all
                                                var phoneExistLst = _context.ProfilePhoneModel.Where(p => p.ProfileId == ProfileId).ToList();
                                                if (phoneExistLst != null && phoneExistLst.Count > 0)
                                                {
                                                    _context.ProfilePhoneModel.RemoveRange(phoneExistLst);
                                                }
                                                //Add again
                                                if (!string.IsNullOrEmpty(Phone))
                                                {
                                                    if (Phone.Contains("-"))
                                                    {
                                                        var arr = Phone.Split('-').ToList();
                                                        var PhoneNumber = RepositoryLibrary.ConvertToNoSpecialCharacters(arr[0]);

                                                        var phoneArray = arr.Where(p => !p.Contains(arr[0])).ToList();
                                                        foreach (var phoneItem in phoneArray)
                                                        {
                                                            ProfilePhoneModel phoneModel = new ProfilePhoneModel();
                                                            phoneModel.PhoneId = Guid.NewGuid();
                                                            phoneModel.ProfileId = ProfileId;
                                                            phoneModel.PhoneNumber = RepositoryLibrary.ConvertToNoSpecialCharacters(phoneItem).Trim();
                                                            if (PhoneNumber != phoneModel.PhoneNumber && !string.IsNullOrEmpty(phoneModel.PhoneNumber))
                                                            {
                                                                _context.Entry(phoneModel).State = EntityState.Added;
                                                                //phoneList.Add(phoneModel);
                                                            }
                                                        }
                                                        existProfileWithSAPCode.Phone = PhoneNumber.Trim();
                                                    }
                                                    else
                                                    {
                                                        existProfileWithSAPCode.Phone = RepositoryLibrary.ConvertToNoSpecialCharacters(Phone);
                                                    }
                                                }
                                                else
                                                {
                                                    existProfileWithSAPCode.Phone = null;
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                // ghi log
                                                string mess = ex.Message;
                                                if (ex.InnerException != null)
                                                {
                                                    mess = ex.InnerException.Message;
                                                    if (ex.InnerException.InnerException != null)
                                                    {
                                                        mess = ex.InnerException.InnerException.Message;
                                                    }
                                                }
                                                WriteLogFile(logFilePath, "Sync Data error: " + mess);
                                                continue;
                                            }
                                            //15. Email
                                            //existProfileWithSAPCode.Email = item["SMTP_ADDR"].ToString();
                                            //if (!string.IsNullOrEmpty(existProfileWithSAPCode.Email))
                                            //{
                                            //    if (IsValidEmail(existProfileWithSAPCode.Email) == false)
                                            //    {
                                            //        existProfileWithSAPCode.Note = string.Format("(Email: {0})", existProfileWithSAPCode.Email);
                                            //        existProfileWithSAPCode.Email = null;
                                            //    }
                                            //    else
                                            //    {
                                            //        if (!string.IsNullOrEmpty(existProfileWithSAPCode.Note) && existProfileWithSAPCode.Note.Contains("Email:"))
                                            //        {
                                            //            existProfileWithSAPCode.Note = null;
                                            //        }
                                            //    }
                                            //}
                                            //else
                                            //{
                                            //    existProfileWithSAPCode.Email = null;
                                            //}
                                            existProfileWithSAPCode.Email = null;
                                            if (!string.IsNullOrEmpty(Email))
                                            {
                                                try
                                                {
                                                    //Delete all
                                                    var emailExistLst = _context.ProfileEmailModel.Where(p => p.ProfileId == ProfileId).ToList();
                                                    if (emailExistLst != null && emailExistLst.Count > 0)
                                                    {
                                                        _context.ProfileEmailModel.RemoveRange(emailExistLst);
                                                    }
                                                    //Add again
                                                    if (Email.Contains(";"))
                                                    {
                                                        var emailArray = Email.Split(';').ToList();
                                                        foreach (var emailItem in emailArray)
                                                        {
                                                            if (IsValidEmail(emailItem) == true)
                                                            {
                                                                ProfileEmailModel emailModel = new ProfileEmailModel();
                                                                emailModel.EmailId = Guid.NewGuid();
                                                                emailModel.ProfileId = ProfileId;
                                                                emailModel.Email = emailItem.Trim();
                                                                _context.Entry(emailModel).State = EntityState.Added;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (IsValidEmail(Email) == true)
                                                        {
                                                            ProfileEmailModel emailModel = new ProfileEmailModel();
                                                            emailModel.EmailId = Guid.NewGuid();
                                                            emailModel.ProfileId = ProfileId;
                                                            emailModel.Email = Email.Trim();
                                                            _context.Entry(emailModel).State = EntityState.Added;
                                                        }
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    // ghi log
                                                    string mess = ex.Message;
                                                    if (ex.InnerException != null)
                                                    {
                                                        mess = ex.InnerException.Message;
                                                        if (ex.InnerException.InnerException != null)
                                                        {
                                                            mess = ex.InnerException.InnerException.Message;
                                                        }
                                                    }
                                                    WriteLogFile(logFilePath, "Sync Data error: " + mess);
                                                    continue;
                                                }
                                            }
                                            //16. Khu vực
                                            //existProfileWithSAPCode.SaleOfficeCode = item["VKBUR"].ToString();
                                            //=> Khi đồng bộ từ SAP về dựa vào tỉnh thành mình set luôn khu vực chứ không đồng bộ data khu vực từ SAP về tránh SAP sai
                                            var SaleOfficeCode = provinceLst.Where(p => p.ProvinceId == ProvinceId).Select(p => p.Area).FirstOrDefault();
                                            if (!string.IsNullOrEmpty(SaleOfficeCode))
                                            {
                                                existProfileWithSAPCode.SaleOfficeCode = SaleOfficeCode;
                                            }
                                            //17. Địa chỉ
                                            //existProfileWithSAPCode.Address = item["ADDRESS"].ToString();
                                            existProfileWithSAPCode.Address = !string.IsNullOrEmpty(item["STCD5"].ToString()) ? item["STCD5"].ToString() : item["ADDRESS"].ToString();
                                            //18. Tỉnh/Thành phố
                                            existProfileWithSAPCode.ProvinceId = ProvinceId;
                                            //19. Quận/Huyện
                                            existProfileWithSAPCode.DistrictId = DistrictId;
                                            //20. Phường/Xã
                                            existProfileWithSAPCode.WardId = WardId;
                                            //21. Ghi chú
                                            //22. Ngày ghé thăm
                                            //23. Trạng thái (not update this field)
                                            //24. Hình ảnh
                                            //25. Nhân viên tạo
                                            //26. Tạo tại công ty 
                                            existProfileWithSAPCode.CreateAtCompany = item["BUKRS"].ToString();
                                            //27. Tạo tại cửa hàng
                                            existProfileWithSAPCode.CreateAtSaleOrg = item["VKORG"].ToString();
                                            //28. CreateBy (not update this field)
                                            //29. Thời gian tạo
                                            //exist.CreateTime = CreateTime;
                                            //30. LastEditBy
                                            existProfileWithSAPCode.LastEditBy = SYSTEM;
                                            //31. Thời gian sửa
                                            existProfileWithSAPCode.LastEditTime = DateTime.Now;
                                            //32. Phân nhóm KH/ Customer Account Group
                                            existProfileWithSAPCode.CustomerAccountGroupCode = item["KTOKD"].ToString();
                                            //33.Mã nhóm  KH/ Customer Group
                                            existProfileWithSAPCode.CustomerGroupCode = item["KDGRP"].ToString();
                                            //34. Mã Điều khoản thanh toán/ Payment Term
                                            existProfileWithSAPCode.PaymentTermCode = item["ZTERM"].ToString();
                                            //35.Mã phân loại doanh thu/ Customer Account Assignment Group
                                            existProfileWithSAPCode.CustomerAccountAssignmentGroupCode = item["KTGRD"].ToString();
                                            //36. Mã phân nhóm dòng tiền/ Cash mgmt Group
                                            existProfileWithSAPCode.CashMgmtGroupCode = item["FDGRV"].ToString();
                                            //37. Mã tài khoản công nợ/ Reconcile Account
                                            existProfileWithSAPCode.ReconcileAccountCode = item["AKONT"].ToString();
                                            //38. Số điện thoại (SAP)
                                            existProfileWithSAPCode.SAPPhone = Phone;
                                            //39. Mã số thuế TaxNo
                                            if (!string.IsNullOrEmpty(TaxNo) && !TaxNo.ToUpper().Contains("X"))
                                            {
                                                existProfileWithSAPCode.TaxNo = TaxNo;
                                            }
                                            else
                                            {
                                                existProfileWithSAPCode.TaxNo = null;
                                            }
                                            existProfileWithSAPCode.CustomerSourceCode = "SHOWROOM";
                                            //Loại địa chỉ
                                            string LoaiDiaChi = item["ADDRTYPE"].ToString();
                                            if (!string.IsNullOrEmpty(LoaiDiaChi))
                                            {
                                                existProfileWithSAPCode.AddressTypeCode = LoaiDiaChi;
                                            }
                                            //Ngành nghề
                                            string NganhNghe = item["BRSCH"].ToString();
                                            if (!string.IsNullOrEmpty(NganhNghe))
                                            {
                                                //NganhNghe = NganhNghe.PadLeft(4, '0');

                                                var existProfileCareer = _context.ProfileCareerModel.Where(p => p.ProfileId == ProfileId && p.CompanyCode == CompanyCode).FirstOrDefault();
                                                if (existProfileCareer == null)
                                                {
                                                    ProfileCareerModel profileCareer = new ProfileCareerModel();
                                                    profileCareer.ProfileCareerId = Guid.NewGuid();
                                                    profileCareer.ProfileId = ProfileId;
                                                    profileCareer.ProfileCareerCode = NganhNghe;
                                                    profileCareer.CompanyCode = item["BUKRS"].ToString();
                                                    profileCareer.CreateBy = SYSTEM;
                                                    profileCareer.CreateTime = CreateTime;
                                                    _context.Entry(profileCareer).State = EntityState.Added;
                                                }
                                                else
                                                {
                                                    existProfileCareer.ProfileCareerCode = NganhNghe;
                                                    _context.Entry(existProfileCareer).State = EntityState.Modified;
                                                }
                                            }

                                            //Yêu cầu tạo khách ở ECC
                                            existProfileWithSAPCode.isCreateRequest = false;
                                            _context.Entry(existProfileWithSAPCode).State = EntityState.Modified;

                                            //thêm nhóm KH
                                            AddProfileGroup(item, CompanyCode, ProfileId);

                                            //thêm NVKD
                                            AddPersonInCharge(employeeList, item, CompanyCode, ProfileId, CreateTime);
                                            //profileCodeList.Add(ProfileForeignCode);
                                        }
                                        //cập nhật address book
                                        else
                                        {

                                        }
                                    }
                                }
                                #endregion

                                #region ProfileBAttributeModel: TaxNo
                                //Delete all
                                //var existAttr = _context.ProfileBAttributeModel
                                //                        .FirstOrDefault(p => p.ProfileId == ProfileId);
                                //if (existAttr != null)
                                //{
                                //    _context.Entry(existAttr).State = EntityState.Deleted;
                                //}
                                ////Add again
                                //ProfileBAttributeModel attr = new ProfileBAttributeModel();
                                //attr.ProfileId = ProfileId;
                                //if (!string.IsNullOrEmpty(TaxNo) && !TaxNo.Contains("X"))
                                //{
                                //    attr.TaxNo = TaxNo;
                                //}
                                //_context.Entry(attr).State = EntityState.Added;
                                //if (!profileCodeList.Contains(ProfileForeignCode))
                                //{
                                //    bAttrList.Add(attr);
                                //}
                                #endregion

                                #region RoleInChargeModel
                                //var roleCharge = _context.RoleInChargeModel
                                //                         .FirstOrDefault(p => p.ProfileId == ProfileId && p.CreateBy == SYSTEM);
                                //if (roleCharge != null)
                                //{
                                //    _context.Entry(roleCharge).State = EntityState.Deleted;
                                //}

                                //var RolesCode = item["ZP_DEPT"].ToString();
                                //if (!string.IsNullOrEmpty(RolesCode) && RolesCode != "00000000")
                                //{
                                //    var role = _context.RolesModel.Where(p => p.RolesCode == RolesCode).FirstOrDefault();
                                //    if (role != null)
                                //    {
                                //        RoleInChargeModel roleModel = new RoleInChargeModel();
                                //        roleModel.RoleInChargeId = Guid.NewGuid();
                                //        roleModel.ProfileId = ProfileId;
                                //        roleModel.RolesId = role.RolesId;
                                //        roleModel.CreateBy = SYSTEM;
                                //        roleModel.CreateTime = CreateTime;
                                //        _context.Entry(roleModel).State = EntityState.Added;
                                //        //if (profileCodeList.Contains(ProfileForeignCode))
                                //        //{
                                //        //    roleList.Add(roleModel);
                                //        //}
                                //    }
                                //}
                                #endregion

                                #region AddressBookModel
                                //if (isContact == true)
                                //{
                                //    var existAddress = _context.AddressBookModel.FirstOrDefault(p => p.ProfileId == ProfileId);
                                //    if (existAddress != null)
                                //    {
                                //        _context.Entry(existAddress).State = EntityState.Deleted;
                                //    }

                                //    AddressBookModel address = new AddressBookModel();
                                //    address.AddressBookId = Guid.NewGuid();
                                //    address.ProfileId = ProfileId;
                                //    address.Address = item["ADDRESS"].ToString();
                                //    address.ProvinceId = ProvinceId;
                                //    address.DistrictId = DistrictId;
                                //    address.CountryCode = ConstCountry.VN;
                                //    address.AddressTypeCode = ConstAddressType.GH;
                                //    address.CreateBy = SYSTEM;
                                //    address.CreateTime = DateTime.Now;
                                //    //_context.Entry(address).State = EntityState.Added;
                                //    addressList.Add(address);
                                //}
                                #endregion

                                #region ProfileContactAttributeModel
                                //if (isContact == true)
                                //{
                                //    var existProfileCode = ProfileForeignCode.Substring(0, 8);
                                //    var existProfile = _context.ProfileModel.FirstOrDefault(p => p.ProfileForeignCode == existProfileCode);
                                //    if (existProfile != null)
                                //    {
                                //        var existContact = _context.ProfileContactAttributeModel.FirstOrDefault(p => p.ProfileId == ProfileId);
                                //        if (existContact != null)
                                //        {
                                //            _context.Entry(existContact).State = EntityState.Deleted;
                                //        }

                                //        ProfileContactAttributeModel contactAttr = new ProfileContactAttributeModel();
                                //        contactAttr.ProfileId = ProfileId;
                                //        contactAttr.CompanyId = existProfile.ProfileId;
                                //        //_context.Entry(contactAttr).State = EntityState.Added;
                                //        contactList.Add(contactAttr);
                                //    }
                                //}
                                #endregion

                                _context.SaveChanges();

                                var profileIsSynced = _context.ProfileModel.Where(p => p.ProfileId == profile.ProfileId).FirstOrDefault();
                                if (profileIsSynced != null)
                                {
                                    profileIsSynced.IsSyncedFromSAP = true;
                                    _context.Entry(profileIsSynced).State = EntityState.Modified;
                                    _context.SaveChanges();
                                }

                                WriteLogFile(logFilePath, "Sync Successfully: " + item["KUNNR"].ToString() + "-" + item["BUKRS"].ToString());
                            }
                            catch (DbEntityValidationException e)
                            {
                                foreach (var eve in e.EntityValidationErrors)
                                {
                                    WriteLogFile(logFilePath, string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors (ProfileSAPCode: {2}): ",
                                        eve.Entry.Entity.GetType().Name, eve.Entry.State, item["KUNNR"].ToString()));
                                    foreach (var ve in eve.ValidationErrors)
                                    {
                                        if (ve.PropertyName == "Email")
                                        {
                                            //var ProfileForeignCode = item["KUNNR"].ToString();
                                            //var profile = _context.ProfileModel.Where(p => p.ProfileForeignCode == ProfileForeignCode).FirstOrDefault();
                                            //if (profile != null && !string.IsNullOrEmpty(profile.Email))
                                            //{
                                            //    profile.Note = string.Format("(Email: {0})", profile.Email);
                                            //    profile.Email = null;
                                            //    _context.Entry(profile).State = EntityState.Modified;
                                            //    _context.SaveChanges();

                                            //    WriteLogFile(logFilePath, "Update Email => Note");
                                            //}
                                            WriteLogFile(logFilePath, "Email: " + item["SMTP_ADDR"].ToString());
                                        }
                                        else
                                        {
                                            WriteLogFile(logFilePath, string.Format("Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                                        }
                                    }
                                }
                                continue;
                            }
                            catch (Exception ex)
                            {
                                var mess = ex.Message;
                                if (ex.InnerException != null)
                                {
                                    if (ex.InnerException.InnerException != null)
                                    {
                                        mess = ex.InnerException.InnerException.Message;
                                    }
                                    else
                                    {
                                        mess = ex.InnerException.Message;
                                    }
                                }
                                WriteLogFile(logFilePath, "Sync Data error: " + mess + " SAPCode: " + item["KUNNR"].ToString());
                                continue;
                            }
                        }

                        //WriteLogFile(logFilePath, string.Format("SYNC PROFILE: Stop. (Company: {0})", Company));
                    }
                    catch (DbEntityValidationException e)
                    {
                        foreach (var eve in e.EntityValidationErrors)
                        {
                            WriteLogFile(logFilePath, string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                eve.Entry.Entity.GetType().Name, eve.Entry.State));
                            foreach (var ve in eve.ValidationErrors)
                            {
                                WriteLogFile(logFilePath, string.Format("Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        var mess = ex.Message;
                        if (ex.InnerException != null)
                        {
                            if (ex.InnerException.InnerException != null)
                            {
                                mess = ex.InnerException.InnerException.Message;
                            }
                            else
                            {
                                mess = ex.InnerException.Message;
                            }
                        }
                        //ghi log
                        //MessageBox.Show(mess);
                        WriteLogFile(logFilePath, "Sync Data error: " + mess);
                    }
                }
                else
                {
                    var profileIsSynced = _context.ProfileModel.Where(p => p.ProfileId == profile.ProfileId).FirstOrDefault();
                    if (profileIsSynced != null)
                    {
                        profileIsSynced.IsSyncedFromSAP = true;
                        _context.Entry(profileIsSynced).State = EntityState.Modified;
                        _context.SaveChanges();

                        WriteLogFile(logFilePath, "Sync No data found from SAP: " + profileIsSynced.ProfileForeignCode);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                string mess = ex.Message;
                if (ex.InnerException != null)
                {
                    mess = ex.InnerException.Message;
                    if (ex.InnerException.InnerException != null)
                    {
                        mess = ex.InnerException.InnerException.Message;
                    }
                }

                //Ghi log
                WriteLogFile(logFilePath, "Sync Data error: " + mess);

                return false;
            }
        }

        private void AddPersonInCharge(List<string> employeeList, DataRow item, string CompanyCode, Guid ProfileId, DateTime? CreateTime)
        {
            #region PersonInChargeModel
            var existPerson = _context.PersonInChargeModel.Where(p => p.ProfileId == ProfileId && p.CompanyCode == CompanyCode).ToList();
            //Nếu có tồn tại thì xóa add lại
            if (existPerson != null && existPerson.Count > 0)
            {
                _context.PersonInChargeModel.RemoveRange(existPerson);
                _context.SaveChanges();
            }

            //if (existPerson == null || existPerson.Count == 0)
            //{
            //Sale Employee
            var EmployeeCode = item["EMPLOYEENO"].ToString();
            if (!string.IsNullOrEmpty(EmployeeCode) && EmployeeCode != "00000000" && employeeList.Contains(EmployeeCode))
            {
                PersonInChargeModel person = new PersonInChargeModel();
                person.PersonInChargeId = Guid.NewGuid();
                person.ProfileId = ProfileId;
                person.SalesEmployeeCode = EmployeeCode;
                person.RoleCode = ConstRoleCode.SALE_EMPLOYEE;
                person.CompanyCode = CompanyCode;
                person.CreateBy = SYSTEM;
                person.CreateTime = CreateTime;
                _context.Entry(person).State = EntityState.Added;
                //personList.Add(person);
            }

            //Sale Employee 2
            var EmployeeCode2 = item["SE_PERNR"].ToString();
            if (!string.IsNullOrEmpty(EmployeeCode2) && EmployeeCode2 != "00000000" && EmployeeCode2 != EmployeeCode && employeeList.Contains(EmployeeCode2))
            {
                PersonInChargeModel person = new PersonInChargeModel();
                person.PersonInChargeId = Guid.NewGuid();
                person.ProfileId = ProfileId;
                person.SalesEmployeeCode = EmployeeCode2;
                person.RoleCode = ConstRoleCode.SALE_EMPLOYEE2;
                person.CompanyCode = CompanyCode;
                person.CreateBy = SYSTEM;
                person.CreateTime = CreateTime;
                _context.Entry(person).State = EntityState.Added;
                //personList.Add(person);
            }
            //}
            #endregion
        }

        private void AddProfileGroup(DataRow item, string CompanyCode, Guid ProfileId)
        {
            #region ProfileGroupModel
            //Check tồn tại nhóm KH
            var existProfileGroup = _context.ProfileGroupModel.Where(p => p.ProfileId == ProfileId && p.CompanyCode == CompanyCode).FirstOrDefault();
            if (existProfileGroup == null)
            {
                //Chưa có => Thêm mới
                if (!string.IsNullOrEmpty(item["KDGRP"].ToString()))
                {
                    ProfileGroupModel profileGroup = new ProfileGroupModel();
                    profileGroup.ProfileGroupId = Guid.NewGuid();
                    profileGroup.ProfileId = ProfileId;
                    profileGroup.ProfileGroupCode = item["KDGRP"].ToString();
                    profileGroup.CompanyCode = CompanyCode;
                    profileGroup.CreateBy = SYSTEM;
                    profileGroup.CreateTime = DateTime.Now;
                    _context.Entry(profileGroup).State = EntityState.Added;
                    //groupList.Add(profileGroup);
                }
            }
            else
            {
                //Cập nhật
                existProfileGroup.ProfileGroupCode = item["KDGRP"].ToString();
                existProfileGroup.LastEditBy = SYSTEM;
                existProfileGroup.LastEditTime = DateTime.Now;
                _context.Entry(existProfileGroup).State = EntityState.Modified;
            }
            #endregion
        }

        #region WriteLogFile
        public static void WriteLogFile(string filePath, string message)
        {
            if (System.IO.File.Exists(filePath))
            {
                if (!System.IO.File.Exists(filePath))
                    System.IO.File.Create(filePath);
            }
            using (FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
            {
                fileStream.Flush();
                fileStream.Close();
            }

            using (StreamWriter sw = new StreamWriter(filePath, true))
            {
                string lastRecordText = "# " + System.DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + " # " + Environment.NewLine + "#" + message + " #" + Environment.NewLine;
                sw.WriteLine(lastRecordText);
                sw.Close();
            }
        }
        #endregion

        #region Check email is valid
        bool IsValidEmail(string email)
        {
            //try
            //{
            //    var addr = new MailAddress(email);
            //    return addr.Address == email;
            //}
            //catch
            //{
            //    return false;
            //}
            string RegexPattern = @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*" +
                                          @"@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";

            // only return true if there is only 1 '@' character
            // and it is neither the first nor the last character
            return Regex.IsMatch(email, RegexPattern, RegexOptions.IgnoreCase);
        }
        #endregion
    }
}
