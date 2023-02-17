using ISD.EntityModels;
using ISD.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.Repositories
{
    public class RoleInChargeRepository
    {
        EntityDataContext _context;
        public RoleInChargeRepository(EntityDataContext db)
        {
            _context = db;
        }
        public List<RoleInChargeViewModel> GetListtRoleByProfileId(Guid? profileId)
        {
            var listRole = (from r in _context.RoleInChargeModel
                            join p in _context.RolesModel on r.RolesId equals p.RolesId
                            where r.ProfileId == profileId
                            select new RoleInChargeViewModel
                            {
                                RoleInChargeId = r.RoleInChargeId,
                                ProfileId = r.ProfileId,
                                RolesId = r.RolesId,
                                RoleName = p.RolesName
                            }).ToList();
            return listRole;
        }

        /// <summary>
        /// Lấy danh sách role từ bảng RolesModel
        /// </summary>
        /// <returns>Danh sách role</returns>
        public List<RolesViewModel> GetRoleList()
        {
            var rolesList = _context.RolesModel.Where(p => p.isEmployeeGroup == true && p.Actived == true)
                                                         .Select(p => new RolesViewModel()
                                                         {
                                                             RolesId = p.RolesId,
                                                             RolesName = p.RolesName,
                                                             OrderIndex = p.OrderIndex
                                                         }).OrderBy(p => p.OrderIndex).ToList();
            return rolesList;
        }
        public void CreateOrUpdate(List<RoleInChargeViewModel> listRoleInChargeVM)
        {
            var profileId = listRoleInChargeVM[0].ProfileId;
            var roleChargeList = _context.RoleInChargeModel.Where(p => p.ProfileId == profileId).ToList();
            //Nếu có roleChargeList thì xoá và thêm mới (Update)
            //else => CreateNew
            if (roleChargeList != null && roleChargeList.Count >0)
            {
                for (int i = roleChargeList.Count-1; i >= 0; i--)
                {
                    _context.Entry(roleChargeList[i]).State = EntityState.Deleted;
                }
                foreach (var item in listRoleInChargeVM)
                {
                    CreateNew(item);
                }
            }
            else
            {
                foreach (var item in listRoleInChargeVM)
                {
                    CreateNew(item);
                }
            }
        }

        private void CreateNew(RoleInChargeViewModel roleInChargeVM)
        {
            var newRoleIncharge = new RoleInChargeModel
            {
                RoleInChargeId = Guid.NewGuid(),
                ProfileId = roleInChargeVM.ProfileId,
                RolesId = roleInChargeVM.RolesId,
                CreateBy = roleInChargeVM.CreateBy,
                CreateTime = DateTime.Now
            };

            _context.RoleInChargeModel.Add(newRoleIncharge);
        }
    }
}
