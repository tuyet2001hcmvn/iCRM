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
    public class AccountRepository
    {
        EntityDataContext _context;
        /// <summary>
        /// Khởi tạo account repository
        /// </summary>
        /// <param name="dataContext">EntityDataContext</param>
        public AccountRepository(EntityDataContext dataContext)
        {
            _context = dataContext;
        }

        /// <summary>
        /// Lấy danh sach tất cả tài khoản
        /// </summary>
        /// <returns>List AccountViewModel</returns>
        public List<AccountViewModel> GetAll()
        {
            var accountLst = (from p in _context.AccountModel
                              where p.Actived == true
                              orderby p.UserName
                              select new AccountViewModel
                              {
                                  AccountId = p.AccountId,
                                  UserName = p.UserName + " | " + p.FullName,
                                  Password = p.Password,
                                  FullName = p.FullName,
                                  EmployeeCode = p.EmployeeCode,
                                  Actived = true
                              }).ToList();
            return accountLst;
        }

        public string GetNameBy(Guid? AccountId)
        {
            var employee = from acc in _context.AccountModel
                           join emp in _context.SalesEmployeeModel on acc.EmployeeCode equals emp.SalesEmployeeCode
                           where acc.AccountId == AccountId
                           select emp.SalesEmployeeName;
                           
            return employee.FirstOrDefault();
        }

        public AccountModel GetNameBy(string EmployeeCode)
        {
            return _context.AccountModel.Where(p => p.EmployeeCode == EmployeeCode).FirstOrDefault();
        }
        public AccountModel GetBy(Guid? AccountId)
        {
            return _context.AccountModel.Where(p => p.AccountId == AccountId).FirstOrDefault();
        }
        /// <summary>
        /// Lấy danh sách nhóm người dùng
        /// </summary>
        /// <returns></returns>
        public List<RolesViewModel> GetRolesList(bool? isEmployeeGroup = null)
        {
            var lst = (from p in _context.RolesModel
                       where p.Actived == true
                       && (p.isEmployeeGroup == null ||p. isEmployeeGroup == isEmployeeGroup)
                       orderby p.OrderIndex
                       select new RolesViewModel()
                       {
                           RolesId = p.RolesId,
                           RolesCode = p.RolesCode,
                           RolesName = p.RolesName,
                           OrderIndex = p.OrderIndex,
                           Actived = p.Actived
                       }).ToList();
            return lst;
        }
    }
}
