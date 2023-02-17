using ISD.Constant;
using ISD.Core;
using ISD.EntityModels;
using ISD.Extensions;
using ISD.Repositories;
using ISD.Repositories.Customer;
using ISD.Repositories.Infrastructure.Extensions;
using ISD.Resources;
using ISD.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Customer.Controllers
{
    public class ProfileContactController : BaseController
    {
        // GET: ProfileContact
        public ActionResult _List(Guid? id, bool? isLoadContent = false, string ProfileType = null)
        {
            return ExecuteSearch(() =>
            {
                var contactProfileList = (from p in _context.ProfileModel
                                          join a in _context.ProfileContactAttributeModel on p.ProfileId equals a.ProfileId
                                          join acc in _context.AccountModel on p.CreateBy equals acc.AccountId into ag
                                          from ac in ag.DefaultIfEmpty()
                                          join s in _context.SalesEmployeeModel on  ac.EmployeeCode equals s.SalesEmployeeCode into sg
                                          from emp in sg.DefaultIfEmpty()
                                              //Phòng ban
                                          join dept in _context.CatalogModel on a.DepartmentCode equals dept.CatalogCode into deptG
                                          from d in deptG.DefaultIfEmpty()
                                         

                                          //Province
                                          join pr in _context.ProvinceModel on p.ProvinceId equals pr.ProvinceId into prG
                                          from province in prG.DefaultIfEmpty()
                                              //District
                                          join dt in _context.DistrictModel on p.DistrictId equals dt.DistrictId into dG
                                          from district in dG.DefaultIfEmpty()
                                              //Ward
                                          join w in _context.WardModel on p.WardId equals w.WardId into wG
                                          from ward in wG.DefaultIfEmpty()

                                          where a.CompanyId == id
                                          orderby a.IsMain descending, p.CreateTime descending
                                          select new ProfileViewModel()
                                          {
                                              ProfileId = a.ProfileId,
                                              ProfileCode = p.ProfileCode,
                                              ProfileName = p.ProfileName,
                                              ProfileContactPosition = a.Position,
                                              Phone = p.Phone,
                                              Email = p.Email,
                                              IsMain = a.IsMain,
                                              CreateUser = emp.SalesEmployeeName,
                                              CreateTime = p.CreateTime,
                                              DepartmentName = d.CatalogText_vi,
                                              Position = a.Position,
                                              Address = p.Address,
                                              ProvinceName = province == null ? "" : ", " + province.ProvinceName,
                                              DistrictName = district == null ? "" : ", " + district.Appellation + " " + district.DistrictName,
                                              WardName = ward == null ? "" : ", " + ward.Appellation + " " + ward.WardName,
                                              DayOfBirth = p.DayOfBirth,
                                              MonthOfBirth = p.MonthOfBirth,
                                              Actived = p.Actived,
                                          }).ToList();

                if (contactProfileList != null && contactProfileList.Count() > 0)
                {
                  

                    var contactIdList = contactProfileList.Select(p => p.ProfileId).ToList();
                    var allAssigneeLst = (from p in _context.PersonInChargeModel
                                          join s in _context.SalesEmployeeModel on p.SalesEmployeeCode equals s.SalesEmployeeCode
                                          where contactIdList.Contains(p.ProfileId.Value)
                                          && p.SalesEmployeeType == ConstSalesEmployeeType.NVKD_A
                                          && p.CompanyCode == CurrentUser.CompanyCode
                                          select new SalesEmployeeViewModel
                                          {
                                              CompanyId = p.ProfileId,
                                              SalesEmployeeCode = s.SalesEmployeeCode,
                                              SalesEmployeeName = s.SalesEmployeeName
                                          }).ToList();

                    foreach (var item in contactProfileList)
                    {
                        //Chức vụ
                        var position = (from p in _context.CatalogModel
                                        where item.Position == p.CatalogCode && p.CatalogTypeCode == "Position"
                                        select p.CatalogText_vi).ToList();
                        if (position.Count > 0)
                        {
                            item.PositionName = position.FirstOrDefault();
                        }
                        //Danh sách số điện thoại theo từng liên hệ
                        var phoneLst = (from p in _context.ProfilePhoneModel
                                        where p.ProfileId == item.ProfileId
                                        select p.PhoneNumber).ToList();
                        //Danh sách email theo từng liên hệ
                        var emailLst = (from p in _context.ProfileEmailModel
                                        where p.ProfileId == item.ProfileId
                                        select p.Email).ToList();
                        item.Address = string.Format("{0}{1}{2}{3}", item.Address, item.WardName, item.DistrictName, item.ProvinceName);
                        //Assignee
                        var assigneeLst = (from p in allAssigneeLst
                                           where p.CompanyId == item.ProfileId
                                           select new SalesEmployeeViewModel
                                           {
                                               SalesEmployeeCode = p.SalesEmployeeCode,
                                               SalesEmployeeName = p.SalesEmployeeName
                                           }).ToList();
                        if (assigneeLst != null && assigneeLst.Count() > 0)
                        {
                            item.PersonInChargeListName = string.Join(", ", assigneeLst.Select(p => p.SalesEmployeeName).ToArray());
                        }
                        //Phone
                        if (phoneLst != null && phoneLst.Count() > 0)
                        {
                            item.Phone = item.Phone + "," + string.Join(", ", phoneLst);
                        }
                        //Email
                        if (emailLst != null && emailLst.Count() > 0)
                        {
                            item.Email = string.Join(", ", emailLst);
                        }
                    }
                }

                ViewBag.Type = ProfileType;
                if (isLoadContent == true)
                {
                    return PartialView("_ListContent", contactProfileList);
                }
                return PartialView(contactProfileList);
            });
        }

        #region Handle Data
        public ActionResult _Create(Guid CompanyId, string CompanyName)
        {
            ProfileViewModel profileContactVM = new ProfileViewModel();
            profileContactVM.CompanyId = CompanyId;
            profileContactVM.CompanyName = CompanyName;

            //set default value
            if (CompanyId != null)
            {
                var account = _unitOfWork.ProfileRepository.GetById(CompanyId);
                profileContactVM.isForeignCustomer = account.isForeignCustomer;
                profileContactVM.ProvinceId = account.ProvinceId;
                profileContactVM.DistrictId = account.DistrictId;
                profileContactVM.WardId = account.WardId;
                profileContactVM.Address = account.Address;
            }

            CreateViewBag(profileContactVM.ProvinceId, profileContactVM.DistrictId, profileContactVM.WardId);
            //Profile Config
            var configList = GetProfileConfig(ConstProfileType.Contact, true, profileContactVM);
            return PartialView("_FromProfileContact", profileContactVM);
        }

        public ActionResult _Edit(Guid? ProfileId)
        {
            ViewBag.isEditMode = true;
            List<ProfileConfigModel> configList = new List<ProfileConfigModel>();
            if (ProfileId != null)
            {
                var profileContact = _unitOfWork.ProfileContactRepository.GetByProfileId(ProfileId.Value);
                CreateViewBag(profileContact.ProvinceId, profileContact.DistrictId, profileContact.WardId, profileContact.DayOfBirth, profileContact.MonthOfBirth, ProfileId, Position: profileContact.ProfileContactPosition, DepartmentCode: profileContact.DepartmentCode);
                //Profile Config
                configList = GetProfileConfig(ConstProfileType.Contact, true, profileContact);
                #region //Get list Other Phone (Số điện thoại khách)
                var morePhoneList = _context.ProfilePhoneModel.Where(p => p.ProfileId == ProfileId).ToList();
                List<string> phone = new List<string>();
                foreach (var item in morePhoneList)
                {
                    phone.Add(item.PhoneNumber);
                }
                ViewBag.Phones = phone;
                #endregion

                //Liên hệ => nếu liên hệ thuộc KH đồng bộ từ ECC => khóa các trường đồng bộ
                var ProfileForeignCode = _context.ProfileModel.Where(p => p.ProfileId == profileContact.CompanyId).Select(p => p.ProfileForeignCode).FirstOrDefault();
                if (!string.IsNullOrEmpty(ProfileForeignCode))
                {
                    ViewBag.ProfileForeignCode = ProfileForeignCode;
                }

                return PartialView("_FromProfileContact", profileContact);
            }

            CreateViewBag();

            configList = GetProfileConfig(ConstProfileType.Contact, true);
            return PartialView("_FromProfileContact");
        }

        [HttpPost]
        public ActionResult Save(ProfileViewModel profileContactVM, List<PersonInChargeViewModel> personInChargeList, List<string> Phone, List<string> Email)//, List<RoleInChargeViewModel> roleInChargeList
        {
            return ExecuteContainer(() =>
            {
                if (profileContactVM.ProfileId == Guid.Empty)
                {
                    #region Create
                    //ProfileModel
                    //set field
                    if (string.IsNullOrEmpty(profileContactVM.Phone))
                    {
                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.NotModified,
                            Success = false,
                            Data = "Vui lòng nhập thông tin \"SĐT liên hệ\""
                        });
                    }
                    else
                    {
                        //Check SĐT liên hệ nhập sai định dạng: phải bắt đầu bằng số 0, >= 10 số, < 15 số
                        if (!profileContactVM.Phone.StartsWith("0") || profileContactVM.Phone.Length < 10 || profileContactVM.Phone.Length >= 15)
                        {
                            return Json(new
                            {
                                Code = System.Net.HttpStatusCode.NotModified,
                                Success = false,
                                Data = "SĐT liên hệ không đúng định dạng! (phải bắt đầu bằng số 0, >= 10 số, < 15 số)"
                            });
                        }
                    }
                    profileContactVM.ProfileId = Guid.NewGuid();
                    profileContactVM.CustomerTypeCode = ConstProfileType.Contact;
                    profileContactVM.ProfileTypeCode = ConstProfileType.Contact;
                    profileContactVM.CreateBy = CurrentUser.AccountId;
                    profileContactVM.CreateByEmployee = CurrentUser.EmployeeCode;
                    profileContactVM.CreateAtCompany = CurrentUser.CompanyCode;
                    profileContactVM.CreateAtSaleOrg = CurrentUser.SaleOrg;
                    profileContactVM.CreateTime = DateTime.Now;
                    profileContactVM.Actived = true;
                    profileContactVM.AbbreviatedName = profileContactVM.ProfileName.ToAbbreviation();
                    _unitOfWork.ProfileRepository.Create(profileContactVM);

                    //Profile contact
                    _unitOfWork.ProfileContactRepository.Create(profileContactVM);

                    //PersonInCharge
                    if (personInChargeList != null && personInChargeList.Count > 0)
                    {
                        foreach (var item in personInChargeList)
                        {
                            item.ProfileId = profileContactVM.ProfileId;
                            item.CreateBy = profileContactVM.CreateBy;
                            item.CompanyCode = profileContactVM.CreateAtCompany;
                            item.SalesEmployeeType = ConstSalesEmployeeType.NVKD_A;
                        }
                        _unitOfWork.PersonInChargeRepository.CreateOrUpdate(personInChargeList, CurrentUser.CompanyCode);
                    }
                    //RoleIncharge
                    //if (roleInChargeList != null && roleInChargeList.Count > 0)
                    //{
                    //    foreach (var item in roleInChargeList)
                    //    {
                    //        item.ProfileId = profileContactVM.ProfileId;
                    //        item.CreateBy = profileContactVM.CreateBy;
                    //    }
                    //    _unitOfWork.RoleInChargeRepository.CreateOrUpdate(roleInChargeList);
                    //}
                    if (profileContactVM.IsMain == true)
                    {
                        var contactList = _context.ProfileContactAttributeModel.Where(p => p.CompanyId == profileContactVM.CompanyId).ToList();
                        foreach (var item in contactList)
                        {
                            item.IsMain = false;
                            _context.Entry(item).State = EntityState.Modified;
                        }
                    }

                    //Số điện thoại
                    #region More phone number
                    if (Phone != null && Phone.Count > 1)
                    {
                        string errMess = string.Empty;
                        Phone.Remove(profileContactVM.Phone);
                        ProfilePhoneRepository phoneRepository = new ProfilePhoneRepository(_context);
                        var isSuccess = phoneRepository.UpdatePhone(Phone, profileContactVM.ProfileId, out errMess);

                        if (isSuccess == false)
                        {
                            return Json(new
                            {
                                Code = System.Net.HttpStatusCode.NotModified,
                                Success = false,
                                Data = errMess
                            });
                        }
                    }
                    else
                    {
                        var morePhone = _context.ProfilePhoneModel.Where(p => p.ProfileId == profileContactVM.ProfileId).ToList();
                        if (morePhone != null && morePhone.Count > 0)
                        {
                            for (int i = morePhone.Count - 1; i >= 0; i--)
                            {
                                _context.Entry(morePhone[i]).State = EntityState.Deleted;
                            }
                        }
                    }
                    #endregion

                    //Email
                    #region More email
                    if (Email != null && Email.Count > 0)
                    {
                        string errMess = string.Empty;
                        ProfileEmailRepository emailRepository = new ProfileEmailRepository(_context);
                        var isSuccess = emailRepository.UpdateEmail(Email, profileContactVM.ProfileId, out errMess);

                        if (isSuccess == false)
                        {
                            return Json(new
                            {
                                Code = System.Net.HttpStatusCode.NotModified,
                                Success = false,
                                Data = errMess
                            });
                        }
                    }
                    else
                    {
                        var moreEmail = _context.ProfileEmailModel.Where(p => p.ProfileId == profileContactVM.ProfileId).ToList();
                        if (moreEmail != null && moreEmail.Count > 0)
                        {
                            for (int i = moreEmail.Count - 1; i >= 0; i--)
                            {
                                _context.Entry(moreEmail[i]).State = EntityState.Deleted;
                            }
                        }
                    }
                    #endregion

                    _context.SaveChanges();

                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.Created,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.Profile_Contact.ToLower())
                    });
                    #endregion
                }
                else
                {
                    #region Edit
                    //var profileContactInDB = _context.ProfileModel.FirstOrDefault(p => p.ProfileId == profileContactVM.ProfileId);
                    //if (profileContactInDB != null)
                    //{
                    //    //Tên 
                    //    profileContactInDB.ProfileName = profileContactVM.ProfileName;
                    //    //Số điện thoại
                    //    profileContactInDB.Phone = profileContactVM.Phone;
                    //    //Email
                    //    profileContactInDB.Email = profileContactVM.Email;
                    //    //Địa chỉ
                    //    profileContactInDB.Address = profileContactVM.Address;
                    //    //Tỉnh/Thành phố
                    //    profileContactInDB.ProvinceId = profileContactVM.ProvinceId;
                    //    ///Quận/Huyện
                    //    profileContactInDB.DistrictId = profileContactVM.DistrictId;
                    //    //Người sửa cuối
                    //    profileContactInDB.LastEditBy = CurrentUser.AccountId;
                    //    //Thời gian cập nhật cuối cùng
                    //    profileContactInDB.LastEditTime = DateTime.Now;

                    //    _context.Entry(profileContactInDB).State = EntityState.Modified;

                    profileContactVM.CustomerTypeCode = ConstProfileType.Contact;
                    profileContactVM.ProfileTypeCode = ConstProfileType.Contact;
                    profileContactVM.LastEditBy = CurrentUser.AccountId;
                    profileContactVM.LastEditTime = DateTime.Now;

                    var result = _unitOfWork.ProfileContactRepository.Update(profileContactVM);
                    //PersonInCharge
                    if (personInChargeList != null && personInChargeList.Count > 0)
                    {
                        foreach (var item in personInChargeList)
                        {
                            item.ProfileId = profileContactVM.ProfileId;
                            item.CreateBy = profileContactVM.LastEditBy;
                            item.CompanyCode = CurrentUser.CompanyCode;
                            item.SalesEmployeeType = ConstSalesEmployeeType.NVKD_A;
                        }
                        _unitOfWork.PersonInChargeRepository.CreateOrUpdate(personInChargeList, CurrentUser.CompanyCode);
                    }
                    //RoleIncharge
                    //if (roleInChargeList != null && roleInChargeList.Count > 0)
                    //{
                    //    foreach (var item in roleInChargeList)
                    //    {
                    //        item.ProfileId = profileContactVM.ProfileId;
                    //        item.CreateBy = profileContactVM.LastEditBy;
                    //    }
                    //    _unitOfWork.RoleInChargeRepository.CreateOrUpdate(roleInChargeList);
                    //}
                    if (profileContactVM.IsMain == true)
                    {
                        var contactList = _context.ProfileContactAttributeModel.Where(p => p.CompanyId == profileContactVM.CompanyId && p.ProfileId != profileContactVM.ProfileId).ToList();
                        foreach (var item in contactList)
                        {
                            item.IsMain = false;
                            _context.Entry(item).State = EntityState.Modified;
                        }
                    }

                    //Số điện thoại
                    #region More phone number
                    if (Phone != null && Phone.Count > 1)
                    {
                        string errMess = string.Empty;
                        Phone.Remove(profileContactVM.Phone);
                        ProfilePhoneRepository phoneRepository = new ProfilePhoneRepository(_context);
                        var isSuccess = phoneRepository.UpdatePhone(Phone, profileContactVM.ProfileId, out errMess);

                        if (isSuccess == false)
                        {
                            return Json(new
                            {
                                Code = System.Net.HttpStatusCode.NotModified,
                                Success = false,
                                Data = errMess
                            });
                        }
                    }
                    else
                    {
                        var morePhone = _context.ProfilePhoneModel.Where(p => p.ProfileId == profileContactVM.ProfileId).ToList();
                        if (morePhone != null && morePhone.Count > 0)
                        {
                            for (int i = morePhone.Count - 1; i >= 0; i--)
                            {
                                _context.Entry(morePhone[i]).State = EntityState.Deleted;
                            }
                        }
                    }
                    #endregion

                    //Email
                    #region More email
                    if (Email != null && Email.Count > 0)
                    {
                        string errMess = string.Empty;
                        ProfileEmailRepository emailRepository = new ProfileEmailRepository(_context);
                        var isSuccess = emailRepository.UpdateEmail(Email, profileContactVM.ProfileId, out errMess);

                        if (isSuccess == false)
                        {
                            return Json(new
                            {
                                Code = System.Net.HttpStatusCode.NotModified,
                                Success = false,
                                Data = errMess
                            });
                        }
                    }
                    else
                    {
                        var moreEmail = _context.ProfileEmailModel.Where(p => p.ProfileId == profileContactVM.ProfileId).ToList();
                        if (moreEmail != null && moreEmail.Count > 0)
                        {
                            for (int i = moreEmail.Count - 1; i >= 0; i--)
                            {
                                _context.Entry(moreEmail[i]).State = EntityState.Deleted;
                            }
                        }
                    }
                    #endregion

                    _context.SaveChanges();
                    if (result == true)
                    {

                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.Created,
                            Success = true,
                            Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.Profile_Contact.ToLower())
                        });

                    }

                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotFound,
                        Success = false,
                        Data = ""
                    });
                    #endregion
                }


            });

        }
        #endregion Handle Data

        #region Delete
        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            return ExecuteDelete(() =>
            {
                var profileContact = _context.ProfileModel.FirstOrDefault(p => p.ProfileId == id);

                if (profileContact != null)
                {
                    //Coppy to deleted model
                    var profileDeleted = new ProfileDeletedModel();
                    profileDeleted.MapFromProfile(profileContact);
                    //Người xóa
                    profileDeleted.CreateBy = CurrentUser.AccountId;
                    //Thời gian xóa
                    profileDeleted.CreateTime = DateTime.Now;
                    _context.Entry(profileDeleted).State = EntityState.Added;

                    var contact = _context.ProfileContactAttributeModel.FirstOrDefault(p => p.ProfileId == id);
                    if (contact != null)
                    {
                        //Coppy to deleted model
                        var contactAttributeDeleted = new ProfileContactAttributeDeletedModel();
                        contactAttributeDeleted.MapFromContactAttribute(contact);
                        _context.Entry(contactAttributeDeleted).State = EntityState.Added;

                        var personChargeList = _context.PersonInChargeModel.Where(p => p.ProfileId == id).ToList();
                        if (personChargeList != null && personChargeList.Count > 0)
                        {
                            foreach (var item in personChargeList)
                            {
                                var personInChargeDeleted = new PersonInChargeDeletedModel();
                                personInChargeDeleted.MapFromPersonInCharge(item);
                                personInChargeDeleted.CreateBy = CurrentUser.AccountId;
                                personInChargeDeleted.CreateTime = DateTime.Now;

                                _context.Entry(personInChargeDeleted).State = EntityState.Added;
                                _context.Entry(item).State = EntityState.Deleted;
                            }
                        }
                        var roleChargeList = _context.RoleInChargeModel.Where(p => p.ProfileId == id).ToList();
                        if (roleChargeList != null && roleChargeList.Count > 0)
                        {
                            foreach (var item in roleChargeList)
                            {
                                var roleInChargeDeleted = new RoleInChargeDeletedModel();
                                roleInChargeDeleted.MapFromRoleInCharge(item);
                                roleInChargeDeleted.CreateBy = CurrentUser.AccountId;
                                roleInChargeDeleted.CreateTime = DateTime.Now;

                                _context.Entry(roleInChargeDeleted).State = EntityState.Added;
                                _context.Entry(item).State = EntityState.Deleted;
                            }
                        }
                        var profilePhoneList = _context.ProfilePhoneModel.Where(p => p.ProfileId == id).ToList();
                        if (profilePhoneList != null && profilePhoneList.Count > 0)
                        {
                            foreach (var item in profilePhoneList)
                            {
                                var proPhoneDel = new ProfilePhoneDeletedModel();
                                proPhoneDel.MapFromProfilePhone(item);

                                _context.Entry(proPhoneDel).State = EntityState.Added;
                                _context.Entry(item).State = EntityState.Deleted;
                            }
                        }

                        var profileEmailList = _context.ProfileEmailModel.Where(p => p.ProfileId == id).ToList();
                        if (profileEmailList != null && profileEmailList.Count > 0)
                        {
                            foreach (var item in profileEmailList)
                            {
                                var proEmailDel = new ProfileEmailDeletedModel();
                                proEmailDel.MapFromProfileEmail(item);

                                _context.Entry(proEmailDel).State = EntityState.Added;
                                _context.Entry(item).State = EntityState.Deleted;
                            }
                        }
                        _context.Entry(contact).State = EntityState.Deleted;
                    }
                    _context.Entry(profileContact).State = EntityState.Deleted;
                    _context.SaveChanges();

                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.Created,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.Profile_Contact.ToLower())
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = ""
                    });
                }
            });
        }
        #endregion

        #region CreateViewBag, Helper
        public void CreateViewBag(Guid? ProvinceId = null, Guid? DistrictId = null, Guid? WardId = null, int? DayOfBirth = null, int? MonthOfBirth = null, Guid? ProfileId = null, string Position = null, string DepartmentCode = null)
        {
            //Get province
            var provinceList = _context.ProvinceModel.Where(p => p.Actived == true).OrderBy(p => p.Area)
                                                                                   .ThenBy(p => p.ProvinceName)
                                                                                   .ToList();
            ViewBag.ProvinceId = new SelectList(provinceList, "ProvinceId", "ProvinceName", ProvinceId);

            //Get District
            var districtList = _context.DistrictModel.Where(p => p.Actived == true
                                                        && (p.ProvinceId == null || p.ProvinceId == ProvinceId))
                                                        .Select(p => new DistrictViewModel()
                                                        {
                                                            DistrictId = p.DistrictId,
                                                            Appellation = p.Appellation,
                                                            DistrictName = p.Appellation + " " + p.DistrictName,
                                                            OrderIndex = p.OrderIndex
                                                        })
                                                        .OrderByDescending(p => p.Appellation)
                                                        .ThenBy(p => p.DistrictName).ToList();
            ViewBag.DistrictId = new SelectList(districtList, "DistrictId", "DistrictName", DistrictId);

            #region //Get list Ward (Phường/Xã)
            var _wardRepository = new WardRepository(_context);
            var wardList = _wardRepository.GetBy(DistrictId);
            ViewBag.WardId = new SelectList(wardList, "WardId", "WardName", WardId);
            #endregion

            //PersonInCharge
            ViewBag.EmployeeList = _unitOfWork.PersonInChargeRepository.GetListEmployee();
            ViewBag.PersonRoleCodeList = _unitOfWork.PersonInChargeRepository.GetListPersonRole();

            ViewBag.PersonInChargeList = _unitOfWork.PersonInChargeRepository.List(ProfileId,CurrentUser.CompanyCode, ConstSalesEmployeeType.NVKD_A);

            //RoleInCharge
            ViewBag.RoleList = _unitOfWork.RoleInChargeRepository.GetRoleList();
            ViewBag.RoleInChargeList = _unitOfWork.RoleInChargeRepository.GetListtRoleByProfileId(ProfileId);

            #region //DayOfBirth (Ngày sinh)
            List<int> DayOfBirthList = new List<int>();
            for (int i = 1; i < 32; i++)
            {
                DayOfBirthList.Add(i);
            }
            ViewBag.DayOfBirth = new SelectList(DayOfBirthList, DayOfBirth);
            #endregion

            #region //MonthOfBith (Tháng sinh)
            List<int> MonthOfBirthList = new List<int>();
            for (int i = 1; i < 13; i++)
            {
                MonthOfBirthList.Add(i);
            }
            ViewBag.MonthOfBirth = new SelectList(MonthOfBirthList, MonthOfBirth);
            #endregion

            #region Position (Chức vụ)
            var _catalogRepository = new CatalogRepository(_context);
            var positionList = _catalogRepository.GetBy(ConstCatalogType.Position);
            ViewBag.ProfileContactPosition = new SelectList(positionList, "CatalogCode", "CatalogText_vi", Position);
            ViewBag.PositionB = new SelectList(positionList, "CatalogCode", "CatalogText_vi", Position);
            #endregion Position (Chức vụ)

            #region Department (Phòng ban)
            var departmentList = _catalogRepository.GetBy(ConstCatalogType.Department);
            ViewBag.DepartmentCode = new SelectList(departmentList, "CatalogCode", "CatalogText_vi", DepartmentCode);
            ViewBag.DepartmentCodeList = new SelectList(departmentList, "CatalogCode", "CatalogText_vi", DepartmentCode);
            #endregion Department (Phòng ban)
        }

        //GetDistrictByProvince
        public ActionResult GetDistrictByProvince(Guid? ProvinceId)
        {
            var districtList = _context.DistrictModel.Where(p => p.ProvinceId == ProvinceId)
                                                         .Select(p => new
                                                         {
                                                             Id = p.DistrictId,
                                                             Name = p.Appellation + " " + p.DistrictName,
                                                             ShortName = p.DistrictName
                                                         })
                                                         .OrderByDescending(p => p.Name).ThenBy(p => p.ShortName).ToList();

            var districtIdList = new SelectList(districtList, "Id", "Name");
            return Json(districtIdList, JsonRequestBehavior.AllowGet);
        }

        //GetWardByDistrict
        public ActionResult GetWardByDistrict(Guid? DistrictId)
        {
            var wardList = _context.WardModel.Where(p => p.DistrictId == DistrictId)
                                                         .Select(p => new
                                                         {
                                                             Id = p.WardId,
                                                             Name = p.Appellation + " " + p.WardName,
                                                             ShortName = p.WardName
                                                         })
                                                         .OrderByDescending(p => p.Name).ThenBy(p => p.ShortName).ToList();

            var wardIdList = new SelectList(wardList, "Id", "Name");
            return Json(wardIdList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Lấy các field theo Profile config và tạo dynamic ViewBag
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="isCreateViewBag"></param>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public List<ProfileConfigModel> GetProfileConfig(string Type, bool? isCreateViewBag = false, ProfileViewModel viewModel = null)
        {
            List<ProfileConfigModel> configList = new List<ProfileConfigModel>();
            configList = _context.ProfileConfigModel.Where(p => p.ProfileCategoryCode == Type).ToList();
            if (configList != null && configList.Count > 0)
            {
                foreach (var item in configList)
                {
                    if (string.IsNullOrEmpty(item.Note))
                    {
                        item.Note = PropertyHelper.GetDisplayNameByString<ProfileViewModel>(item.FieldCode);
                    }
                    //CreateViewBag
                    if (!string.IsNullOrEmpty(item.Parameters) && isCreateViewBag == true)
                    {
                        var lst = _unitOfWork.CatalogRepository.GetBy(item.Parameters);
                        object value = null;
                        if (viewModel != null)
                        {
                            var propertyName = _unitOfWork.ProfileRepository.GetPropertyNameByParameter(item.ProfileCategoryCode, item.Parameters);
                            value = viewModel.GetType().GetProperty(propertyName).GetValue(viewModel, null);
                        }
                        var objectData = new SelectList(lst, "CatalogCode", "CatalogText_vi", value);
                        ViewData.Add(item.Parameters, objectData);
                    }
                }
            }
            if (isCreateViewBag == true)
            {
                ViewBag.ProfileConfig = configList;
                ViewBag.ProfileConfigCode = configList.Select(p => p.FieldCode).ToList();
            }
            return configList;
        }
        #endregion
    }
}