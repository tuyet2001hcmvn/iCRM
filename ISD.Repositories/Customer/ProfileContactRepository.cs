using ISD.Constant;
using ISD.EntityModels;
using ISD.Repositories.Infrastructure.Extensions;
using ISD.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.Repositories
{
    public class ProfileContactRepository
    {
        EntityDataContext _context;
        public ProfileContactRepository(EntityDataContext dataContext)
        {
            _context = dataContext;
        }

        public ProfileContactAttributeModel Create(ProfileViewModel profileViewModel)
        {
            var proContactNew = new ProfileContactAttributeModel();
            proContactNew.MapProfileContact(profileViewModel);
            _context.Entry(proContactNew).State = EntityState.Added;
            return proContactNew;
        }

        public ProfileViewModel GetByProfileId(Guid profileId)
        {
            var profileContact = (from p in _context.ProfileModel
                                  join c in _context.ProfileContactAttributeModel on p.ProfileId equals c.ProfileId
                                  join comp in _context.ProfileModel on c.CompanyId equals comp.ProfileId
                                  where p.ProfileId == profileId
                                  select new ProfileViewModel()
                                  {
                                      //GUID
                                      ProfileId = p.ProfileId,
                                      isForeignCustomer = p.isForeignCustomer,
                                      CustomerSourceCode = p.CustomerSourceCode,
                                      //Tên
                                      ProfileName = p.ProfileName,
                                      //Công ty
                                      CompanyId = c.CompanyId,
                                      CompanyName = comp.ProfileName,
                                      //Chức vụ
                                      ProfileContactPosition = c.Position,
                                      //Số điện thoại
                                      Phone = p.Phone,
                                      //Email
                                      Email = p.Email,
                                      //Địa chỉ
                                      Address = p.Address,
                                      //Quận/Huyện
                                      ProvinceId = p.ProvinceId,
                                      //Tỉnh/Thành phố
                                      DistrictId = p.DistrictId,
                                      //Phường/Xã
                                      WardId = p.WardId,
                                      DayOfBirth = p.DayOfBirth,
                                      MonthOfBirth = p.MonthOfBirth,
                                      IsMain = c.IsMain,
                                      DepartmentCode = c.DepartmentCode,
                                      Position = c.Position,
                                      Actived = p.Actived == true ? true : false
                                  }).FirstOrDefault();
            return profileContact;
        }

        public bool Update(ProfileViewModel profileViewModel)
        {
            var profileInDb = _context.ProfileModel.FirstOrDefault(p => p.ProfileId == profileViewModel.ProfileId);
            var contactInDb = _context.ProfileContactAttributeModel.FirstOrDefault(p => p.ProfileId == profileViewModel.ProfileId);
            if (profileInDb != null && contactInDb != null)
            {
                profileInDb.isForeignCustomer = profileViewModel.isForeignCustomer;
                profileInDb.LastEditBy = profileViewModel.LastEditBy;
                profileInDb.LastEditTime = profileViewModel.LastEditTime;

                profileInDb.MapProfile(profileViewModel);
                contactInDb.MapProfileContact(profileViewModel);

                _context.Entry(profileInDb).State = EntityState.Modified;
                _context.Entry(contactInDb).State = EntityState.Modified;
                return true;
            }
            else { return false; }
        }
    }
}
