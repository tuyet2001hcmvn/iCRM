using ISD.Constant;
using ISD.EntityModels;
using ISD.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.Repositories
{
    public class StoreRepository
    {
        EntityDataContext _context;

        /// <summary>
        /// Khởi tạo Store Repository
        /// </summary>
        /// <param name="dataContext">EntityDataContext</param>
        public StoreRepository(EntityDataContext dataContext)
        {
            _context = dataContext;
        }

        /// <summary>
        /// Lấy tất cả chi nhánh thuộc công ty
        /// </summary>
        /// <param name="companyId">Guid: CompanyId</param>
        /// <param name="IfCompanyIdNullGetAll">lấy tất cả khi companyId = true</param>
        /// <returns>Danh sách chi nhánh</returns>
        public List<StoreViewModel> GetStoreByCompany(Guid? companyId, bool? IfCompanyIdNullGetAll = false)
        {
            var storeList = (from p in _context.StoreModel
                             where ((companyId == null && IfCompanyIdNullGetAll == true) || p.CompanyId == companyId)
                             && p.Actived == true
                             orderby p.SaleOrgCode
                             select new StoreViewModel
                             {
                                 StoreId = p.StoreId,
                                 StoreName = p.SaleOrgCode + " | " + p.StoreName,
                                 OrderIndex = p.OrderIndex,
                             }).ToList();
            return storeList;
        }

        //Theo phân quyền
        public List<StoreViewModel> GetStoreByCompany(Guid? companyId, bool isViewByStore, Guid? AccountId, bool? IfCompanyIdNullGetAll = false)
        {
            // Get All
            if (isViewByStore == false)
            {
                return GetStoreByCompany(companyId, IfCompanyIdNullGetAll);
            }
            //Get By Permission
            var storeList = (from s in _context.StoreModel
                             from a in s.AccountModel
                             where a.AccountId == AccountId
                             && ((companyId == null && IfCompanyIdNullGetAll == true) || (s.CompanyId == companyId && s.Actived == true))
                             orderby s.SaleOrgCode
                             select new StoreViewModel
                             {
                                 StoreId = s.StoreId,
                                 StoreName = s.SaleOrgCode + " | " + s.StoreName,
                                 OrderIndex = s.OrderIndex,
                             }).ToList();

            return storeList;
        }

        /// <summary>
        /// Lấy CompanyId bởi mã chi nhánh
        /// </summary>
        /// <param name="SaleOrgCode">string: SaleOrgCode</param>
        /// <returns>CompanyId</returns>
        public Guid GetCompanyIdBySaleOrgCode(string SaleOrgCode)
        {
            var StoreInDb = _context.StoreModel.FirstOrDefault(p => p.SaleOrgCode == SaleOrgCode);
            return StoreInDb.CompanyId;
        }

        /// <summary>
        /// Lấy CompanyId bởi StoreId
        /// </summary>
        /// <param name="StoreId">Guid: StoreId</param>
        /// <returns>CompanyId</returns>
        public Guid GetCompanyIdByStoreId(Guid StoreId)
        {
            var storeInDb = _context.StoreModel.FirstOrDefault(p => p.StoreId == StoreId);
            return storeInDb.CompanyId;
        }
        public string GetCompanyCodeBySaleOrgCode(string SaleOrgCode)
        {
            var StoreInDb = _context.StoreModel.FirstOrDefault(p => p.SaleOrgCode == SaleOrgCode);
            var CompanyCode = _context.CompanyModel.Where(p => p.CompanyId == StoreInDb.CompanyId).FirstOrDefault();
            return CompanyCode.CompanyCode;
        }

        /// <summary>
        /// Lấy StoreId bởi SaleOrg
        /// </summary>
        /// <param name="SaleOrg">string: SaleOrg</param>
        /// <returns>StoreId</returns>
        public Guid GetStoreIdBySaleOrgCode(string SaleOrg)
        {
            var storeId = _context.StoreModel.Where(p => p.SaleOrgCode == SaleOrg).Select(p => p.StoreId).FirstOrDefault();
            return storeId;
        }
        public string GetSaleOrgCodeByStoreId(Guid? StoreId)
        {
            var SaleOrg = _context.StoreModel.Where(p => p.StoreId == StoreId).Select(p => p.SaleOrgCode).FirstOrDefault();
            return SaleOrg;
        }

        /// <summary>
        /// Lấy chi nhánh theo saleorgcode
        /// </summary>
        /// <param name="SaleOrgCode">string: SaleOrgCode</param>
        /// <returns>StoreModel</returns>
        public StoreModel GetBySaleOrgCode(string SaleOrgCode)
        {
            return _context.StoreModel.FirstOrDefault(p => p.SaleOrgCode == SaleOrgCode);
        }

        public StoreViewModel Find(Guid? StoreId)
        {
            return _context.StoreModel.Where(p => p.StoreId == StoreId).Select(p => new StoreViewModel()
            {
                StoreId = p.StoreId,
                StoreName = p.StoreName
            }).FirstOrDefault();
        }

        /// <summary>
        /// Lấy danh sách chi nhánh theo phân quyền
        /// </summary>
        /// <param name="CurrentAccountId">Guid: CurrentAccountId</param>
        /// <returns>Danh sách chi nhánh</returns>
        public List<StoreViewModel> GetStoreByPermission(Guid? CurrentAccountId)
        {
            var storeList = (from ac in _context.AccountModel
                             from s in ac.StoreModel
                             join c in _context.CompanyModel on s.CompanyId equals c.CompanyId
                             where ac.AccountId == CurrentAccountId
                             && s.Actived == true
                             && c.Actived == true
                             orderby c.OrderIndex, s.SaleOrgCode
                             select new StoreViewModel
                             {
                                 StoreId = s.StoreId,
                                 SaleOrgCode = s.SaleOrgCode,
                                 StoreName = s.SaleOrgCode + " | " + s.StoreName
                             })
                             .OrderBy(p => p.SaleOrgCode)
                            .ToList();
            return storeList;
        }

        /// <summary>
        /// Lấy danh sách chi nhánh theo phân quyền và theo công ty
        /// </summary>
        /// <param name="CurrentAccountId">Guid: CurrentAccountId</param>
        /// <param name="CompanyId">Guid: Công ty</param>
        /// <returns>Danh sách chi nhánh</returns>
        public List<StoreViewModel> GetStoreByCompanyPermission(Guid? CurrentAccountId, Guid? CompanyId)
        {
            var storeList = (from ac in _context.AccountModel
                             from s in ac.StoreModel
                             join c in _context.CompanyModel on s.CompanyId equals c.CompanyId
                             where ac.AccountId == CurrentAccountId
                             && s.CompanyId == CompanyId
                             && s.Actived == true
                             && c.Actived == true
                             orderby c.OrderIndex, s.SaleOrgCode
                             select new StoreViewModel
                             {
                                 StoreId = s.StoreId,
                                 SaleOrgCode = s.SaleOrgCode,
                                 StoreName = s.SaleOrgCode + " | " + s.StoreName
                             })
                             .OrderBy(p => p.SaleOrgCode)
                            .ToList();
            return storeList;
        }
        /// <summary>
        /// Lấy danh sách chi nhánh theo phân quyền và theo công ty
        /// </summary>
        /// <param name="CurrentAccountId">Guid: CurrentAccountId</param>
        /// <param name="CompanyCode">Mã Công ty</param>
        /// <returns>Danh sách chi nhánh</returns>
        public List<StoreViewModel> GetStoreByCompanyPermission(Guid? CurrentAccountId, string CompanyCode)
        {
            var storeList = (from ac in _context.AccountModel
                             from s in ac.StoreModel
                             join c in _context.CompanyModel on s.CompanyId equals c.CompanyId
                             where ac.AccountId == CurrentAccountId
                             && c.CompanyCode == CompanyCode
                             && s.Actived == true
                             orderby c.OrderIndex, s.SaleOrgCode
                             select new StoreViewModel
                             {
                                 StoreId = s.StoreId,
                                 SaleOrgCode = s.SaleOrgCode,
                                 StoreName = s.SaleOrgCode + " | " + s.StoreName,
                                 Area = s.Area,
                                 DefaultCustomerSource = s.DefaultCustomerSource,
                             })
                             .OrderBy(p => p.SaleOrgCode)
                            .ToList();
            return storeList;
        }
        /// <summary>
        /// Lấy danh sách tất cả chi nhánh
        /// </summary>
        /// <returns>Danh sách chi nhánh</returns>
        public List<StoreViewModel> GetAllStore()
        {
            var storeList = (from s in _context.StoreModel
                             where s.Actived == true
                             orderby s.SaleOrgCode
                             select new StoreViewModel
                             {
                                 StoreId = s.StoreId,
                                 SaleOrgCode = s.SaleOrgCode,
                                 StoreName = s.SaleOrgCode + " | " + s.StoreName,
                                 DefaultCustomerSource = s.DefaultCustomerSource,
                             })
                             .OrderBy(p => p.SaleOrgCode)
                            .ToList();
            return storeList;
        }

        public StoreViewModel GetBy(Guid? StoreId)
        {
            var store = (from s in _context.StoreModel
                         where s.Actived == true && s.StoreId == StoreId
                         orderby s.SaleOrgCode
                         select new StoreViewModel
                         {
                             StoreId = s.StoreId,
                             SaleOrgCode = s.SaleOrgCode,
                             StoreName = s.SaleOrgCode + " | " + s.StoreName
                         })
                             .FirstOrDefault();
            return store;
        }


        public List<StoreViewModel> GetAllStore(bool isViewByStore, Guid? AccountId)
        {
            //Get All
            if (isViewByStore == false)
            {
                return GetAllStore();
            }
            //Get By Permission
            var storeList = (from s in _context.StoreModel
                             from a in s.AccountModel
                             where a.AccountId == AccountId
                             orderby s.SaleOrgCode
                             select new StoreViewModel
                             {
                                 StoreId = s.StoreId,
                                 SaleOrgCode = s.SaleOrgCode,
                                 StoreName = s.SaleOrgCode + " | " + s.StoreName
                             })
                             .OrderBy(p => p.SaleOrgCode)
                             .ToList();
            return storeList;
        }

        public List<ProductViewModel> GetAllProduct()
        {
            var productList = (from s in _context.ProductModel
                             where s.Actived == true
                             orderby s.ProductCode
                             select new ProductViewModel
                             {
                                 ProductId = s.ProductId,
                                 ProductCode = s.ProductCode,
                                 ProductName = s.ProductCode + " | " + s.ProductName
                             })
                             .OrderBy(p => p.ProductCode)
                            .ToList();
            return productList;
        }

        /// <summary>
        /// Lấy danh sách chi nhánh theo phân quyền xem dữ liệu
        /// Nếu ViewPermission = TATCA, lấy hết tất cả chi nhánh
        /// Nếu ViewPermission = CHINHANH, lấy các chi nhánh được phân quyền
        /// Nếu ViewPermission = CONGTY, lấy tất cả các chi nhánh theo công ty được phân quyền
        /// </summary>
        /// <param name="AccountId"></param>
        /// <returns>list StoreViewModel</returns>
        public List<StoreViewModel> GetStoreByViewPermission(Guid? AccountId, Guid? CompanyId = null)
        {
            var ViewPermission = _context.AccountModel.Where(p => p.AccountId == AccountId)
                                         .Select(p => p.ViewPermission)
                                         .FirstOrDefault();
            List<StoreViewModel> storeList = new List<StoreViewModel>();
            if (ViewPermission == ConstViewPermission.TATCA)
            {
                storeList = (from s in _context.StoreModel
                             where (CompanyId == null || s.CompanyId == CompanyId)
                             orderby s.SaleOrgCode
                             select new StoreViewModel
                             {
                                 StoreId = s.StoreId,
                                 SaleOrgCode = s.SaleOrgCode,
                                 StoreName = s.SaleOrgCode + " | " + s.StoreName
                             })
                             .OrderBy(p => p.SaleOrgCode)
                             .ToList();
            }
            else if (ViewPermission == ConstViewPermission.CHINHANH)
            {
                storeList = (from s in _context.StoreModel
                             from a in s.AccountModel
                             where a.AccountId == AccountId
                             && (CompanyId == null || s.CompanyId == CompanyId)
                             orderby s.SaleOrgCode
                             select new StoreViewModel
                             {
                                 StoreId = s.StoreId,
                                 SaleOrgCode = s.SaleOrgCode,
                                 StoreName = s.SaleOrgCode + " | " + s.StoreName
                             })
                             .OrderBy(p => p.SaleOrgCode)
                             .ToList();
            }
            else if (ViewPermission == ConstViewPermission.CONGTY)
            {
                var companyList = (from s in _context.StoreModel
                                   from a in s.AccountModel
                                   join c in _context.CompanyModel on s.CompanyId equals c.CompanyId
                                   where a.AccountId == AccountId
                                   && (CompanyId == null || s.CompanyId == CompanyId)
                                   orderby s.SaleOrgCode
                                   select c.CompanyId).Distinct().ToList();

                if (companyList != null && companyList.Count > 0)
                {
                    foreach (var item in companyList)
                    {
                        var stores = (from s in _context.StoreModel
                                      where s.CompanyId == item
                                      orderby s.SaleOrgCode
                                      select new StoreViewModel
                                      {
                                          StoreId = s.StoreId,
                                          SaleOrgCode = s.SaleOrgCode,
                                          StoreName = s.SaleOrgCode + " | " + s.StoreName
                                      })
                                      .OrderBy(p => p.SaleOrgCode)
                                      .ToList();
                        storeList.AddRange(stores);
                    }
                    storeList = storeList.OrderBy(p => p.SaleOrgCode).ToList();
                }
            }
            return storeList;
        }

        public List<StoreViewModel> GetStoreByCustomerSource(Guid AccountId, string CompanyCode, string CustomerSource)
        {

            var storeList = (from s in _context.StoreModel
                             from a in s.AccountModel
                             join c in _context.CompanyModel on s.CompanyId equals c.CompanyId
                             where s.Actived == true && a.AccountId == AccountId && c.CompanyCode == CompanyCode
                             orderby s.SaleOrgCode
                             select new StoreViewModel
                             {
                                 StoreId = s.StoreId,
                                 SaleOrgCode = s.SaleOrgCode,
                                 StoreName = s.SaleOrgCode + " | " + s.StoreName,
                                 DefaultCustomerSource = s.DefaultCustomerSource
                             })
                            .OrderBy(p => p.SaleOrgCode)
                           .ToList();

            return storeList;
        }

        public List<StoreViewModel> GetStoreByCustomerSource(string CustomerSource)
        {
            var storeList = (from s in _context.StoreModel
                             where s.Actived == true && (CustomerSource == "" || s.DefaultCustomerSource == CustomerSource)
                             orderby s.SaleOrgCode
                             select new StoreViewModel
                             {
                                 StoreId = s.StoreId,
                                 SaleOrgCode = s.SaleOrgCode,
                                 StoreName = s.SaleOrgCode + " | " + s.StoreName,
                                 DefaultCustomerSource = s.DefaultCustomerSource
                             })
                             .OrderBy(p => p.SaleOrgCode)
                            .ToList();
            return storeList;
        }

        public List<StoreViewModel> GetStoreBySaleOfice(string SaleOfficeCode, Guid? AccountId = null)
        {
            var storeList = new List<StoreViewModel>();
            var saleOrgList = (from s in _context.StoreModel
                               where s.Actived == true
                               && s.Area == SaleOfficeCode
                               orderby s.SaleOrgCode
                               select s.SaleOrgCode)
                             .ToList();

            if (AccountId != null)
            {
                storeList = (from s in _context.StoreModel
                             from acc in s.AccountModel
                             where acc.AccountId == AccountId
                             && s.Actived == true
                             //&& (AccountId == null || saleOrgList.Contains(s.SaleOrgCode))
                             select new StoreViewModel
                             {
                                 StoreId = s.StoreId,
                                 SaleOrgCode = s.SaleOrgCode,
                                 StoreName = s.SaleOrgCode + " | " + s.StoreName,
                                 DefaultCustomerSource = s.DefaultCustomerSource,
                                 Area = s.Area
                             })
                             .OrderBy(p => p.SaleOrgCode)
                            .ToList();

                List<string> areaList = new List<string>();
                areaList.Add(SaleOfficeCode);

                var removeList = (from p in storeList
                                  where p.SaleOrgCode == ConstCompanyCode.AnCuong
                                  || p.SaleOrgCode == ConstCompanyCode.Malloca
                                  || p.SaleOrgCode == ConstCompanyCode.Aconcept
                                  select p).ToList();

                storeList = (from p in storeList
                             where p.SaleOrgCode != ConstCompanyCode.AnCuong
                                   && p.SaleOrgCode != ConstCompanyCode.Malloca
                                   && p.SaleOrgCode != ConstCompanyCode.Aconcept
                             select p).ToList();

                if (storeList != null && storeList.Count > 0)
                {
                    foreach (var item in storeList)
                    {
                        if (!areaList.Contains(item.Area))
                        {
                            areaList.Add(item.Area);
                        }
                    }
                }
                areaList = areaList.OrderBy(p => p != SaleOfficeCode).ThenBy(p => p).ToList();
                storeList = storeList.OrderBy(p => areaList.FindIndex(p1 => p1 == p.Area)).ThenBy(p => p.SaleOrgCode).ToList();

                storeList.AddRange(removeList);
            }

            return storeList;
        }

        /// <summary>
        /// Lấy chi nhánh theo khách hàng/liên hệ
        /// </summary>
        /// <param name="ProfileId"></param>
        /// <returns></returns>
        public string GetSaleOrgByProfile(Guid? ProfileId)
        {
            var SaleOrg = _context.ProfileModel.Where(p => p.ProfileId == ProfileId).Select(p => p.CreateAtSaleOrg).FirstOrDefault();
            return SaleOrg;
        }
    }
}
