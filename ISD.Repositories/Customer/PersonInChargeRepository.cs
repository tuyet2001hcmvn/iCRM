using ISD.Constant;
using ISD.EntityModels;
using ISD.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ISD.Repositories
{
    public class PersonInChargeRepository
    {
        private EntityDataContext _context;

        /// <summary>
        /// Hảm khởi tạo Person In Charge Repository
        /// </summary>
        /// <param name="db">Truyền vào Db Context</param>
        public PersonInChargeRepository(EntityDataContext db)
        {
            _context = db;
        }

        /// <summary>
        /// Hàm trả vể danh sách người phụ trách theo profile Id
        /// </summary>
        /// <param name="profileId">truyền vào profileId</param>
        /// <param name="CompanyCode">Mã công ty</param>
        /// <param name="SalesEmployeeType">Loại nhân viên: 1: NV kinh doanh (Dự án), 2: NV sales admin, 3: NV Spec, 4: NV Master, 5: NV Truy cập, 6: NV TVVL, 7: NV Kinh doanh(Account)</param>
        /// <returns>Trả về danh sách ngưởi phụ trách theo cùa profile Id truyền vào</returns>
        public List<PersonInChargeViewModel> List(Guid? profileId, string CompanyCode = null, int? SalesEmployeeType = null)
        {
            var personInChargeList = (from p in _context.PersonInChargeModel
                                      join sal in _context.SalesEmployeeModel on p.SalesEmployeeCode equals sal.SalesEmployeeCode
                                      join acc in _context.AccountModel on p.CreateBy equals acc.AccountId into ag
                                      from ac in ag.DefaultIfEmpty()
                                      join se in _context.SalesEmployeeModel on ac.EmployeeCode equals se.SalesEmployeeCode into seg
                                      from s in seg.DefaultIfEmpty()
                                      //join ca in _context.CatalogModel on 
                                      //new { RoleCode = p.RoleCode, CatalogTypeCode = ConstCatalogType.Role } equals 
                                      //new { RoleCode = ca.CatalogCode, CatalogTypeCode = ca.CatalogTypeCode } into tmp1
                                      //from c in tmp1.DefaultIfEmpty()

                                      where p.ProfileId == profileId
                                      && (CompanyCode == null || p.CompanyCode == CompanyCode)
                                      && (SalesEmployeeType == null || p.SalesEmployeeType == SalesEmployeeType)
                                      orderby p.CreateTime descending
                                      select new PersonInChargeViewModel
                                      {
                                          PersonInChargeId = p.PersonInChargeId,
                                          ProfileId = p.ProfileId,
                                          SalesEmployeeCode = p.SalesEmployeeCode,
                                          SalesEmployeeName = sal.SalesEmployeeName,
                                          SalesEmployeeShortName = sal.SalesEmployeeShortName,
                                          CreateUser = s.SalesEmployeeName,
                                          CreateTime = p.CreateTime,
                                          CompanyCode = p.CompanyCode,
                                          //RoleCode = p.RoleCode,
                                          //RoleName = c.CatalogText_vi
                                      }).ToList();

            if (personInChargeList != null && personInChargeList.Count > 0)
            {
                foreach (var item in personInChargeList)
                {
                    var role = (from p in _context.AccountModel
                                from m in p.RolesModel
                                where p.EmployeeCode == item.SalesEmployeeCode
                                && m.isEmployeeGroup == true
                                select m.RolesName).FirstOrDefault();

                    role = role != null ? role : "";
                    item.RoleName = role;
                }
            }

            return personInChargeList;
        }

        /// <summary>
        /// Tạo mới người phụ trách truyền vào là một view model
        /// </summary>
        /// <param name="personInChargeVM">PersonInChargeViewModel</param>
        private void CreateNew(PersonInChargeViewModel personInChargeVM)
        {
            if (!string.IsNullOrEmpty(personInChargeVM.SalesEmployeeCode))
            {
                var personNew = new PersonInChargeModel
                {
                    PersonInChargeId = Guid.NewGuid(),
                    ProfileId = personInChargeVM.ProfileId,
                    SalesEmployeeCode = personInChargeVM.SalesEmployeeCode,
                    RoleCode = personInChargeVM.RoleCode,
                    CompanyCode = personInChargeVM.CompanyCode,
                    CreateBy = personInChargeVM.CreateBy,
                    CreateTime = DateTime.Now,
                    SalesEmployeeType = personInChargeVM.SalesEmployeeType,
                };
                _context.Entry(personNew).State = EntityState.Added;
            }
        }

        private void CreateNewNotRequired(PersonInChargeNotRequiredViewModel personInChargeVM)
        {
            if (!string.IsNullOrEmpty(personInChargeVM.SalesEmployeeCode))
            {
                var personNew = new PersonInChargeModel
                {
                    PersonInChargeId = Guid.NewGuid(),
                    ProfileId = personInChargeVM.ProfileId,
                    SalesEmployeeCode = personInChargeVM.SalesEmployeeCode,
                    RoleCode = personInChargeVM.RoleCode,
                    CompanyCode = personInChargeVM.CompanyCode,
                    CreateBy = personInChargeVM.CreateBy,
                    CreateTime = DateTime.Now,
                    SalesEmployeeType = personInChargeVM.SalesEmployeeType,
                };
                _context.Entry(personNew).State = EntityState.Added;
            }
        }

        /// <summary>
        /// Tạo mới người phụ trách
        /// </summary>
        /// <param name="personInChargeVM">PersonInChargeViewModel</param>
        /// <returns>True || False</returns>
        public bool Create(PersonInChargeViewModel personInChargeVM)
        {
            try
            {
                if (!string.IsNullOrEmpty(personInChargeVM.SalesEmployeeCode))
                {
                    var personNew = new PersonInChargeModel
                    {
                        PersonInChargeId = Guid.NewGuid(),
                        ProfileId = personInChargeVM.ProfileId,
                        SalesEmployeeCode = personInChargeVM.SalesEmployeeCode,
                        SalesEmployeeType = personInChargeVM.SalesEmployeeType,
                        RoleCode = personInChargeVM.RoleCode,
                        CompanyCode = personInChargeVM.CompanyCode,
                        CreateBy = personInChargeVM.CreateBy,
                        CreateTime = DateTime.Now,
                    };
                    _context.Entry(personNew).State = EntityState.Added;
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void CreateOrUpdate(List<PersonInChargeViewModel> listPersonInChargeVM, string CompanyCode)
        {
            var id = listPersonInChargeVM[0].ProfileId;
            var personInChargeList = _context.PersonInChargeModel.Where(p => p.ProfileId == id && p.CompanyCode == CompanyCode).ToList();
            //Nếu có personInchargeList => Xoá hết va cập nhật lại
            //Else => Thêm mới
            if (personInChargeList != null && personInChargeList.Count > 0)
            {
                for (int i = personInChargeList.Count - 1; i >= 0; i--)
                {
                    _context.Entry(personInChargeList[i]).State = EntityState.Deleted;
                }
                //foreach (var personincharge in personInChargeList)
                //{
                //    _context.PersonInChargeModel.Remove(personincharge);
                //}
                foreach (var item in listPersonInChargeVM)
                {
                    CreateNew(item);
                }
            }
            else
            {
                foreach (var item in listPersonInChargeVM)
                {
                    CreateNew(item);
                }
            }
        }

        public void CreateOrUpdateNotRequired(List<PersonInChargeNotRequiredViewModel> listPersonInChargeVM, string CompanyCode)
        {
            var id = listPersonInChargeVM[0].ProfileId;
            var personInChargeList = _context.PersonInChargeModel.Where(p => p.ProfileId == id && p.CompanyCode == CompanyCode).ToList();
            //Nếu có personInchargeList => Xoá hết va cập nhật lại
            //Else => Thêm mới
            if (personInChargeList != null && personInChargeList.Count > 0)
            {
                for (int i = personInChargeList.Count - 1; i >= 0; i--)
                {
                    _context.Entry(personInChargeList[i]).State = EntityState.Deleted;
                }
                //foreach (var personincharge in personInChargeList)
                //{
                //    _context.PersonInChargeModel.Remove(personincharge);
                //}
                foreach (var item in listPersonInChargeVM)
                {
                    CreateNewNotRequired(item);
                }
            }
            else
            {
                foreach (var item in listPersonInChargeVM)
                {
                    CreateNewNotRequired(item);
                }
            }
        }

        /// <summary>
        /// Lấy danh sách nhân viên
        /// </summary>
        /// <returns>Danh sách nhân viên</returns>
        public List<SalesEmployeeViewModel> GetListEmployee()
        {
            var employeeList = _context.SalesEmployeeModel.Where(p => p.Actived == true)
                                                        .Select(p => new SalesEmployeeViewModel()
                                                        {
                                                            SalesEmployeeCode = p.SalesEmployeeCode,
                                                            //Khách yêu cầu lấy tên ngắn
                                                            SalesEmployeeName = p.SalesEmployeeCode + " | " + p.SalesEmployeeShortName
                                                        }).ToList();
            return employeeList;
        }

        /// <summary>
        /// Lấy danh sách nhân viên theo nhóm
        /// </summary>
        /// <param name="RolesCode">Mã nhóm người dùng</param>
        /// <returns>Danh sách nhân viên</returns>
        public List<SalesEmployeeViewModel> GetListEmployeeByRoles(string RolesCode)
        {
            var employeeList = (from p in _context.SalesEmployeeModel
                                join acc in _context.AccountModel on p.SalesEmployeeCode equals acc.EmployeeCode
                                from r in acc.RolesModel
                                where p.Actived == true && r.RolesCode == RolesCode
                                select  new SalesEmployeeViewModel()
                                {
                                    SalesEmployeeCode = p.SalesEmployeeCode,
                                    //Khách yêu cầu lấy tên ngắn
                                    SalesEmployeeName = p.SalesEmployeeCode + " | " + p.SalesEmployeeShortName,
                                }).ToList();
            return employeeList;
        }

        /// <summary>
        /// Lấy danh sách vai trò của nhân viên
        /// </summary>
        /// <returns>Danh sách vai trò</returns>
        public List<CatalogViewModel> GetListPersonRole()
        {
            var roleList = _context.CatalogModel.Where(p => p.CatalogTypeCode == ConstCatalogType.Role && p.Actived == true)
                                                .Select(p => new CatalogViewModel
                                                {
                                                    CatalogCode = p.CatalogCode,
                                                    CatalogText_vi = p.CatalogText_vi,
                                                    OrderIndex = p.OrderIndex
                                                }).OrderBy(p => p.OrderIndex).ToList();
            return roleList;
        }
    }
}