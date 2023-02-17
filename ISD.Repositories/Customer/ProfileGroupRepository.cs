using ISD.Constant;
using ISD.EntityModels;
using ISD.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ISD.Repositories
{
    public class ProfileGroupRepository
    {
        EntityDataContext _context;
        public ProfileGroupRepository(EntityDataContext db)
        {
            _context = db;
        }
        public List<ProfileGroupCreateViewModel> GetListProfileGroupBy(Guid? profileId, string CompanyCode)
        {
            var listRole = (from r in _context.ProfileGroupModel
                            join c in _context.CatalogModel on new 
                            { 
                                ProfileGroupCode = r.ProfileGroupCode, 
                                CustomerGroup = ConstCatalogType.CustomerGroup 
                            } equals new
                            {
                                ProfileGroupCode = c.CatalogCode,
                                CustomerGroup = c.CatalogTypeCode
                            }
                            where r.ProfileId == profileId
                            //&& c.CatalogTypeCode == ConstCatalogType.CustomerGroup
                            && r.CompanyCode == CompanyCode
                            orderby c.CatalogCode
                            select new ProfileGroupCreateViewModel
                            {
                                ProfileGroupId = r.ProfileGroupId,
                                ProfileId = r.ProfileId,
                                ProfileGroupCode = r.ProfileGroupCode,
                                ProfileGroupName = c.CatalogText_vi,
                            }).ToList();
            return listRole;
        }

        public List<string> GetListProfileGroupOtherCompanyBy(Guid? profileId, string CompanyCode)
        {
            var listRole = (from r in _context.ProfileGroupModel
                            join c in _context.CatalogModel on new 
                            { 
                                ProfileGroupCode = r.ProfileGroupCode, 
                                CustomerGroup = ConstCatalogType.CustomerGroup 
                            } equals new
                            {
                                ProfileGroupCode = c.CatalogCode,
                                CustomerGroup = c.CatalogTypeCode
                            }
                            where r.ProfileId == profileId
                            //&& c.CatalogTypeCode == ConstCatalogType.CustomerGroup
                            && r.CompanyCode != CompanyCode
                            orderby c.CatalogCode
                            select r.CompanyCode + ": " + c.CatalogText_vi).ToList();
            return listRole;
        }

        public void CreateOrUpdate(List<ProfileGroupCreateViewModel> listProfileGroupVM)
        {
            var profileId = listProfileGroupVM[0].ProfileId;
            var companyCode = listProfileGroupVM[0].CompanyCode;
            var profileGroupList = _context.ProfileGroupModel.Where(p => p.ProfileId == profileId && p.CompanyCode == companyCode).ToList();
            //Nếu có roleChargeList thì xoá và thêm mới (Update)
            //else => CreateNew
            if (profileGroupList != null && profileGroupList.Count > 0)
            {
                for (int i = profileGroupList.Count - 1; i >= 0; i--)
                {
                    _context.Entry(profileGroupList[i]).State = EntityState.Deleted;
                }
                foreach (var item in listProfileGroupVM)
                {
                    CreateNew(item);
                }
            }
            else
            {
                foreach (var item in listProfileGroupVM)
                {
                    CreateNew(item);
                }
            }
        }

        private void CreateNew(ProfileGroupCreateViewModel listProfileGroupVM)
        {
            if (!string.IsNullOrEmpty(listProfileGroupVM.ProfileGroupCode))
            {
                var newProfileGroup = new ProfileGroupModel
                {
                    ProfileGroupId = Guid.NewGuid(),
                    ProfileId = listProfileGroupVM.ProfileId,
                    ProfileGroupCode = listProfileGroupVM.ProfileGroupCode,
                    CompanyCode = listProfileGroupVM.CompanyCode,
                    CreateBy = listProfileGroupVM.CreateBy,
                    CreateTime = DateTime.Now
                };
                _context.ProfileGroupModel.Add(newProfileGroup);
            }
        }
    }
}
