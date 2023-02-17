using ISD.Constant;
using ISD.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.Repositories
{
    public class CompanyRepository
    {
        EntityDataContext _context;
        public CompanyRepository(EntityDataContext dataContext)
        {
            _context = dataContext;
        }

        public List<CompanyModel> GetAll()
        {
            var listCompany = _context.CompanyModel.Where(p => p.Actived == true).OrderBy(p => p.CompanyCode).ToList();
            return listCompany;
        }

        public List<CompanyModel> GetAll(bool isViewByStore, Guid? AccountId)
        {
            //Get All
            if (isViewByStore == false)
            {
                return GetAll();
            }
            //Get By Permission
            var listCompany = (from s in _context.StoreModel
                               from a in s.AccountModel
                               join c in _context.CompanyModel on s.CompanyId equals c.CompanyId
                               where a.AccountId == AccountId
                               select c).Distinct()
                             .OrderBy(p => p.CompanyCode)
                            .ToList();
            return listCompany;
        }

        /// <summary>
        /// Lấy danh sách công ty theo phân quyền xem dữ liệu
        /// </summary>
        /// <param name="AccountId"></param>
        /// <returns></returns>
        public List<CompanyModel> GetCompanyByViewPermission(Guid? AccountId)
        {
            var ViewPermission = _context.AccountModel.Where(p => p.AccountId == AccountId)
                                         .Select(p => p.ViewPermission)
                                         .FirstOrDefault();
            List<CompanyModel> companyList = new List<CompanyModel>();
            if (ViewPermission == ConstViewPermission.TATCA)
            {
                companyList = _context.CompanyModel.Where(p => p.Actived == true).OrderBy(p => p.CompanyCode).ToList();
            }
            else if (ViewPermission == ConstViewPermission.CHINHANH || ViewPermission == ConstViewPermission.CONGTY)
            {
                companyList = (from s in _context.StoreModel
                               from a in s.AccountModel
                               join c in _context.CompanyModel on s.CompanyId equals c.CompanyId
                               where a.AccountId == AccountId
                               select c).Distinct().OrderBy(p => p.CompanyCode).ToList();
            }
            return companyList;
        }

        public Guid GetCompanyIdBy(string CompanyCode)
        {
            return _context.CompanyModel.FirstOrDefault(p => p.CompanyCode == CompanyCode).CompanyId;
        }

        public CompanyModel GetBy(string SaleOrg)
        {
            return (from s in _context.StoreModel
                    where s.SaleOrgCode == SaleOrg
                    select s.CompanyModel).FirstOrDefault();
        }

        /// <summary>
        /// Lấy công ty theo danh sách chi nhánh
        /// </summary>
        /// <param name="storeList"></param>
        /// <returns></returns>
        public List<CompanyModel> GetCompanyByStoreList(List<string> storeList)
        {
            var compList = (from p in _context.CompanyModel
                            join s in _context.StoreModel on p.CompanyId equals s.CompanyId
                            where storeList.Contains(s.SaleOrgCode)
                            select p).Distinct().ToList();

            return compList.OrderBy(p => p.CompanyCode).ToList();
        }
    }
}
