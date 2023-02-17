using ISD.EntityModels;
using ISD.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.Repositories
{
    public class SalesEmployeeRepository
    {
        EntityDataContext _context;
        public SalesEmployeeRepository(EntityDataContext dataContext)
        {
            _context = dataContext;
        }

        /// <summary>
        /// Lấy tất cả nhân viên để gắn vào DropdownList
        /// </summary>
        /// <returns>Danh sách nhân viên</returns>
        public List<SalesEmployeeViewModel> GetAllForDropdownlist(bool? isMobile = false)
        {
            var salesEmployeeList = (from p in _context.SalesEmployeeModel
                                     join a in _context.AccountModel on p.SalesEmployeeCode equals a.EmployeeCode
                                     where p.Actived == true
                                     orderby p.SalesEmployeeCode
                                     select new SalesEmployeeViewModel
                                     {
                                         AccountId = a.AccountId,
                                         SalesEmployeeCode = p.SalesEmployeeCode,
                                         SalesEmployeeName = isMobile == true ? p.SalesEmployeeShortName : p.SalesEmployeeCode + " | " + p.SalesEmployeeName,
                                         RolesName = a.RolesModel.Where(m => m.isEmployeeGroup == true).Select(p => p.RolesName).FirstOrDefault(),
                                     }).ToList();
           
            return salesEmployeeList;
        }

        /// <summary>
        /// Lấy danh sách phòng ban
        /// </summary>
        /// <param name="isMobile"></param>
        /// <returns></returns>
        public List<DepartmentViewModel> GetAllDropDownListDepartment()
        {
            var deparmentList = (from d in _context.DepartmentModel
                                 where d.Actived == true
                                 orderby d.OrderIndex
                                 select new DepartmentViewModel
                                 {
                                     DepartmentId = d.DepartmentId,
                                     DepartmentCode = d.DepartmentCode,
                                     DepartmentName = d.DepartmentName
                                 }).ToList();
            return deparmentList;
        }

        /// <summary>
        /// Lấy tất cả nhân viên để gắn vào DropdownList ( Không Roles
        /// </summary>
        /// <returns>Danh sách nhân viên</returns>
        public List<SalesEmployeeViewModel> GetAllForDropdownlistWithoutRoles()
        {
            var salesEmployeeList = (from p in _context.SalesEmployeeModel
                                     where p.Actived == true
                                     orderby p.SalesEmployeeCode
                                     select new SalesEmployeeViewModel
                                     {
                                         SalesEmployeeCode = p.SalesEmployeeCode,
                                         SalesEmployeeName = p.SalesEmployeeCode + " | " + p.SalesEmployeeShortName
                                     }).ToList();

            return salesEmployeeList;
        }

        public List<SalesEmployeeViewModel> GetAllForDropdownlistBy(bool? isMobile = false)
        {
            var salesEmployeeList = (from p in _context.SalesEmployeeModel
                                     join a in _context.AccountModel on p.SalesEmployeeCode equals a.EmployeeCode
                                     where p.Actived == true
                                     orderby p.SalesEmployeeCode
                                     select new SalesEmployeeViewModel
                                     {
                                         AccountId = a.AccountId,
                                         SalesEmployeeCode = p.SalesEmployeeCode,
                                         SalesEmployeeName = isMobile == true ? p.SalesEmployeeName : p.SalesEmployeeCode + " | " + p.SalesEmployeeShortName,
                                         RolesName = a.RolesModel.Where(m => m.isEmployeeGroup == true).Select(p => p.RolesName).FirstOrDefault(),
                                     }).Take(10).ToList();

            return salesEmployeeList;
        }

        public List<SalesEmployeeViewModel> GetSalesEmployeeByRoles(string RolesCode, bool? isMobile = false)
        {
            var rolesId = _context.RolesModel.Where(p => p.RolesCode == RolesCode).Select(p => p.RolesId).FirstOrDefault();
            var salesEmployeeList = (from p in _context.SalesEmployeeModel
                                     join a in _context.AccountModel on p.SalesEmployeeCode equals a.EmployeeCode
                                     from r in a.RolesModel
                                     where p.Actived == true && r.RolesId == rolesId
                                     orderby p.SalesEmployeeCode
                                     select new SalesEmployeeViewModel
                                     {
                                         AccountId = a.AccountId,
                                         SalesEmployeeCode = p.SalesEmployeeCode,
                                         SalesEmployeeName = isMobile == true ? p.SalesEmployeeName : p.SalesEmployeeCode + " | " + p.SalesEmployeeName,
                                         SalesEmployeeShortName = isMobile == true ? p.SalesEmployeeName : p.SalesEmployeeCode + " | " + p.SalesEmployeeShortName,
                                         RolesName = a.RolesModel.Where(m => m.isEmployeeGroup == true).Select(p => p.RolesName).FirstOrDefault(),
                                     }).ToList();

            salesEmployeeList = salesEmployeeList.GroupBy(x => new { x.AccountId, x.SalesEmployeeCode,x.SalesEmployeeName, x.SalesEmployeeShortName, x.RolesName })
                                                .Select(x => new SalesEmployeeViewModel() {
                                                    AccountId = x.Key.AccountId,
                                                    SalesEmployeeCode = x.Key.SalesEmployeeCode,
                                                    SalesEmployeeName = x.Key.SalesEmployeeName,
                                                    SalesEmployeeShortName = x.Key.SalesEmployeeShortName,
                                                    RolesName = x.Key.RolesName,
                                                }).ToList();

            return salesEmployeeList;
        }

        public string GetSaleEmployeeCodeBy(Guid? AccountId = null)
        {
            var salesEmployee = (from p in _context.AccountModel
                                 where p.AccountId == AccountId
                                 && p.Actived == true
                                 select p.EmployeeCode).FirstOrDefault();
            return salesEmployee;
        }

        public string GetSaleEmployeeNameBy(string SalesEmployeeCode)
        {
            var SalesEmployeeName = (from p in _context.SalesEmployeeModel
                                     where p.SalesEmployeeCode == SalesEmployeeCode
                                     && p.Actived == true
                                     select p.SalesEmployeeShortName).FirstOrDefault();
            return SalesEmployeeName;
        }

        public SalesEmployeeModel Find(Guid? AccountId)
        {
            var emp = (from p in _context.SalesEmployeeModel
                       join acc in _context.AccountModel on p.SalesEmployeeCode equals acc.EmployeeCode
                       where acc.AccountId == AccountId
                       select p).FirstOrDefault();
            return emp;
        }
        public SalesEmployeeModel Find(string SaleEmployee)
        {
            var emp = (from p in _context.SalesEmployeeModel
                       where p.SalesEmployeeCode == SaleEmployee
                       select p).FirstOrDefault();
            return emp;
        }
    }
}
