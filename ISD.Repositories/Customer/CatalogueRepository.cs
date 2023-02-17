using ISD.Constant;
using ISD.EntityModels;
using ISD.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ISD.Repositories
{
    public class CatalogueRepository
    {
        private EntityDataContext _context;
        public CatalogueRepository(EntityDataContext dataContext)
        {
            _context = dataContext;
        }

        public List<CatalogueViewModel> GetAll(Guid? profileId)
        {
            var catalogueList = (from a in _context.DeliveryModel
                                 join b in _context.DeliveryDetailModel on a.DeliveryId equals b.DeliveryId
                                 //Profile
                                 join c in _context.ProfileModel on a.ProfileId equals c.ProfileId
                                 //CustomerSource
                                 join d in _context.CatalogModel on new { CustomerSource = c.CustomerSourceCode, Type = ConstCatalogType.CustomerSource } equals new { CustomerSource = d.CatalogCode, Type = d.CatalogTypeCode } into srTemp
                                 from cat in srTemp.DefaultIfEmpty()
                                     //Product
                                 join e in _context.ProductModel on b.ProductId equals e.ProductId
                                 //Create User
                                 join acc in _context.AccountModel on a.CreateBy equals acc.AccountId
                                 where a.ProfileId == profileId
                                 orderby a.CreateTime descending
                                 select new CatalogueViewModel
                                 {
                                     DeliveryDetailId = b.DeliveryDetailId,
                                     CustomerSourceName = cat.CatalogText_vi,
                                     ERPProductCode = e.ERPProductCode,
                                     ProductCode = e.ProductCode,
                                     ProductName = e.ProductName,
                                     Quantity = (int)b.Quantity,
                                     CreatedDate = a.CreateTime,
                                     isDeleted = a.isDeleted
                                 }).ToList();
            return catalogueList;
        }

        public List<DeliveryViewModel> GetAllCatalog(Guid? profileId, Guid? TaskId = null, string DeliveryType = null)
        {
            var lst = (from a in _context.DeliveryModel
                       //Profile
                       join c in _context.ProfileModel on a.ProfileId equals c.ProfileId
                       //CustomerSource
                       join d in _context.CatalogModel on c.CustomerSourceCode equals d.CatalogCode into dGroup
                       from cat in dGroup.DefaultIfEmpty()
                       //Create User
                       join acc in _context.AccountModel on a.CreateBy equals acc.AccountId
                       join s in _context.SalesEmployeeModel on acc.EmployeeCode equals s.SalesEmployeeCode into sg
                       from emp in sg.DefaultIfEmpty()
                           //Store
                       join store in _context.StoreModel on a.StoreId equals store.StoreId into stg
                       from st in stg.DefaultIfEmpty()
                       //Task
                       join t in _context.TaskModel on a.TaskId equals t.TaskId into tg
                       from task in tg.DefaultIfEmpty()
                       //SalesEmployee
                       join salesEmp in _context.SalesEmployeeModel on a.SalesEmployeeCode equals salesEmp.SalesEmployeeCode

                       where a.ProfileId == profileId 
                       && (cat == null || cat.CatalogTypeCode == ConstCatalogType.CustomerSource)
                       && (TaskId == null || (TaskId != null && a.TaskId == TaskId && (DeliveryType == null || a.DeliveryType == DeliveryType)))

                       orderby a.DocumentDate descending
                       select new DeliveryViewModel
                       {
                           DeliveryId = a.DeliveryId,
                           DeliveryCode = a.DeliveryCode,
                           ProfileId = a.ProfileId,
                           ProfileName = c.ProfileName,
                           CreateByName = emp.SalesEmployeeName ?? salesEmp.SalesEmployeeName,
                           DocumentDate = a.DocumentDate,
                           StoreName = st != null ? st.SaleOrgCode + " | " + st.StoreName : "",
                           TaskId = a.TaskId,
                           Summary = task.Summary,
                           isDeleted = a.isDeleted,
                           DeliveryType = a.DeliveryType,
                       }).ToList();

            //if (TaskId == null)
            //{
            //    lst = lst.Where(p => p.TaskId == null).ToList();
            //}

            //Cập nhật giá trị catalog đã xuất
            if (lst != null && lst.Count > 0)
            {
                foreach (var item in lst)
                {
                    item.Total = 0;
                    var details = _context.DeliveryDetailModel.Where(p => p.DeliveryId == item.DeliveryId).ToList();
                    if (details != null && details.Count > 0)
                    {
                        foreach (var detail in details)
                        {
                            item.Total += (detail.Quantity * detail.Price);
                        }
                    }
                }
            }

            return lst;
        }

        public List<CatalogueViewModel> GetCustomerCatalogueBy(Guid profileId, DateTime visitDate)
        {
            var catalogueList = (from a in _context.DeliveryModel
                                 join b in _context.DeliveryDetailModel on a.DeliveryId equals b.DeliveryId
                                 //Profile
                                 join c in _context.ProfileModel on a.ProfileId equals c.ProfileId
                                 //CustomerSource
                                 join d in _context.CatalogModel on c.CustomerSourceCode equals d.CatalogCode into dGroup
                                 from cat in dGroup.DefaultIfEmpty()
                                     //Product
                                 join e in _context.ProductModel on b.ProductId equals e.ProductId
                                 //Create User
                                 join acc in _context.AccountModel on a.CreateBy equals acc.AccountId
                                 where a.ProfileId == profileId
                                 && cat.CatalogTypeCode == ConstCatalogType.CustomerSource
                                 && a.DocumentDate == visitDate
                                 orderby a.CreateTime descending
                                 select new CatalogueViewModel
                                 {
                                     DeliveryDetailId = b.DeliveryDetailId,
                                     CustomerSourceName = cat.CatalogText_vi,
                                     ERPProductCode = e.ERPProductCode,
                                     ProductCode = e.ProductCode,
                                     ProductName = e.ProductName,
                                     Quantity = (int)b.Quantity,
                                     CreatedDate = a.CreateTime,
                                     customerCatalogueLst = e.ProductCode + " (" + b.Quantity + ")",
                                 }).ToList();
            return catalogueList;
        }

        public List<CatalogueReportViewModel> GetCatalogueReport(CatalogueReportSearchViewModel searchViewModel)
        {
            var catalogueList = (from a in _context.DeliveryModel
                                 join b in _context.DeliveryDetailModel on a.DeliveryId equals b.DeliveryId
                                 //Profile
                                 join c in _context.ProfileModel on a.ProfileId equals c.ProfileId
                                 //Store
                                 join d in _context.StoreModel on a.StoreId equals d.StoreId
                                 //Product
                                 join e in _context.ProductModel on b.ProductId equals e.ProductId
                                 //Category
                                 join f in _context.CategoryModel on e.CategoryId equals f.CategoryId
                                 //Account
                                 join g in _context.AccountModel on a.CreateBy equals g.AccountId
                                 //Product Attribute
                                 join h in _context.ProductAttributeModel on e.ProductId equals h.ProductId into hTemp
                                 from productA in hTemp.DefaultIfEmpty()
                                 where //Theo chi nhánh
                                 (searchViewModel.SaleOrgCode == null || d.SaleOrgCode == searchViewModel.SaleOrgCode)
                                 //Từ ngày => đến ngày
                                 && (searchViewModel.FromDate == null || a.CreateTime >= searchViewModel.FromDate)
                                 && (searchViewModel.ToDate == null || a.CreateTime <= searchViewModel.ToDate)
                                 //Theo User
                                 && (searchViewModel.SaleEmployeeCode == null || g.EmployeeCode == searchViewModel.SaleEmployeeCode)
                                 //Theo nguồn KH
                                 && (searchViewModel.CustomerSourceCode == null || c.CustomerSourceCode == searchViewModel.CustomerSourceCode)
                                 //Theo loại Catalogue
                                 && (searchViewModel.CategoryId == null || e.CategoryId == searchViewModel.CategoryId)
                                 orderby a.CreateTime descending
                                 select new CatalogueReportViewModel
                                 {
                                     MaCatalogue = e.ProductCode,
                                     TenCTL = e.ProductName,
                                     DVT = productA.Unit,
                                     SoLuong = (int)b.Quantity,
                                     StoreId = a.StoreId,
                                 }).ToList();

            //Theo chi nhánh
            if (string.IsNullOrEmpty(searchViewModel.SaleOrgCode) && searchViewModel.StoreId != null && searchViewModel.StoreId.Count > 0)
            {
                catalogueList = catalogueList.Where(p => searchViewModel.StoreId.Contains(p.StoreId.Value)).ToList();
            }

            catalogueList = catalogueList.GroupBy(item => new { item.MaCatalogue, item.TenCTL, item.DVT })
                             .Select(group => new CatalogueReportViewModel()
                             {
                                 MaCatalogue = group.Key.MaCatalogue,
                                 TenCTL = group.Key.TenCTL,
                                 DVT = group.Key.DVT,
                                 SoLuong = group.Sum(p => p.SoLuong),
                             }).ToList();

            return catalogueList;
        }
    }
}
