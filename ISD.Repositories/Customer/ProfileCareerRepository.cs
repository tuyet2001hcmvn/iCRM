using ISD.Constant;
using ISD.EntityModels;
using ISD.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ISD.Repositories
{
    public class ProfileCareerRepository
    {
        EntityDataContext _context;
        public ProfileCareerRepository(EntityDataContext db)
        {
            _context = db;
        }
        public ProfileCareerViewModel GetProfileCareerBy(Guid? profileId, string CompanyCode)
        {
            var profileCareer = (from r in _context.ProfileCareerModel
                                 join c in _context.CatalogModel on new 
                                 { 
                                     ProfileCareerCode = r.ProfileCareerCode, 
                                     CustomerCareer = ConstCatalogType.CustomerCareer 
                                 } equals new
                                 {
                                     ProfileCareerCode = c.CatalogCode,
                                     CustomerCareer = c.CatalogTypeCode
                                 }
                                 where r.ProfileId == profileId
                                 //&& c.CatalogTypeCode == ConstCatalogType.CustomerGroup
                                 && r.CompanyCode == CompanyCode
                                 orderby c.CatalogCode
                                 select new ProfileCareerViewModel
                                 {
                                     ProfileCareerId = r.ProfileCareerId,
                                     ProfileId = r.ProfileId,
                                     ProfileCareerCode = r.ProfileCareerCode,
                                     ProfileCareerName = c.CatalogText_vi
                                 }).FirstOrDefault();
            return profileCareer;
        }

        public List<string> GetProfileCareerOtherCompanyBy(Guid? profileId, string CompanyCode)
        {
            var profileCareer = (from r in _context.ProfileCareerModel
                                 join c in _context.CatalogModel on new 
                                 { 
                                     ProfileCareerCode = r.ProfileCareerCode, 
                                     CustomerCareer = ConstCatalogType.CustomerCareer 
                                 } equals new
                                 {
                                     ProfileCareerCode = c.CatalogCode,
                                     CustomerCareer = c.CatalogTypeCode
                                 }
                                 where r.ProfileId == profileId
                                 //&& c.CatalogTypeCode == ConstCatalogType.CustomerGroup
                                 && r.CompanyCode != CompanyCode
                                 orderby c.CatalogCode
                                 select r.CompanyCode + ": " + c.CatalogText_vi
                                 ).ToList();
            return profileCareer;
        }

        public void CreateOrUpdate(Guid ProfileId, string CompanyCode, string CustomerCareerCode, Guid? AccountId)
        {
            var profileCareer = _context.ProfileCareerModel.Where(p => p.ProfileId == ProfileId && p.CompanyCode == CompanyCode).FirstOrDefault();
            //Nếu đã có thông tin ngành nghề khách hàng trong dữ liệu => Update
            //Nếu chưa có dữ liệu => Insert
            if (profileCareer != null)
            {
                profileCareer.ProfileCareerCode = CustomerCareerCode;
                profileCareer.LastEditBy = AccountId;
                profileCareer.LastEditTime = DateTime.Now;
                _context.Entry(profileCareer).State = EntityState.Modified;
            }
            else
            {
                ProfileCareerModel profileCareerAdd = new ProfileCareerModel();
                profileCareerAdd.ProfileCareerId = Guid.NewGuid();
                profileCareerAdd.ProfileId = ProfileId;
                profileCareerAdd.CompanyCode = CompanyCode;
                profileCareerAdd.ProfileCareerCode = CustomerCareerCode;
                profileCareerAdd.CreateBy = AccountId;
                profileCareerAdd.CreateTime = DateTime.Now;

                _context.Entry(profileCareerAdd).State = EntityState.Added;
            }
        }
    }
}
