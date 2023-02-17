using ISD.EntityModels;
using ISD.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ISD.Repositories
{
    public class ProductWarrantyRepository
    {
        private EntityDataContext _context;

        public ProductWarrantyRepository(EntityDataContext db)
        {
            _context = db;
        }

        /// <summary>
        /// Tìm kiếm bảo hành theo SerriNo || ProductWarrantyNo || Actived 
        /// </summary>
        /// <param name="searchModel">Model Searh của ProductWarranty</param>
        /// <returns>Danh sách ProductWarranty</returns>
        public List<ProductWarrantyViewModel> Search(ProductWarrantySearchViewModel searchModel)
        {
            var proWarrantyList = SearchQuery(searchModel).ToList();

            return proWarrantyList;
        }

        /// <summary>
        /// Hàm query tìm kiếm
        /// </summary>
        /// <param name="searchModel">Model Searh của ProductWarranty</param>
        /// <returns>Câu truy vấn tìm kiếm</returns>
        public IQueryable<ProductWarrantyViewModel> SearchQuery(ProductWarrantySearchViewModel searchModel)
        {
            var proWarrantyList = (from p in _context.ProductWarrantyModel
                                   join a in _context.ProfileModel on p.ProfileId equals a.ProfileId
                                   join s in _context.ProductModel on p.ProductId equals s.ProductId
                                   join w in _context.WarrantyModel on p.WarrantyId equals w.WarrantyId
                                   //Province
                                   join pro in _context.ProvinceModel on p.ProvinceId equals pro.ProvinceId into pg
                                   from province in pg.DefaultIfEmpty()
                                   //District
                                   join d in _context.DistrictModel on p.DistrictId equals d.DistrictId into dg
                                   from district in dg.DefaultIfEmpty()
                                   //Ward
                                   join wa in _context.WardModel on p.WardId equals wa.WardId into wg
                                   from ward in wg.DefaultIfEmpty()
                                   orderby p.ProductWarrantyCode
                                   where
                                   (searchModel.SerriNo == null || p.SerriNo.Contains(searchModel.SerriNo))
                                   && (searchModel.ProductWarrantyNo == null || p.ProductWarrantyNo.Contains(searchModel.ProductWarrantyNo))
                                   //profile id
                                   && (searchModel.ProfileId == null || p.ProfileId == searchModel.ProfileId)
                                   //Cong ty
                                   && (searchModel.CompanyId == null || p.CompanyId == searchModel.CompanyId)
                                   && (
                                       searchModel.ProfileName == null || 
                                       (p.ProfileName.Contains(searchModel.ProfileName) || p.ProfileShortName.Contains(searchModel.ProfileName))
                                   )
                                   && (searchModel.Phone == null || p.Phone.Contains(searchModel.Phone))
                                   && (searchModel.Age == null || p.Age == searchModel.Age)
                                   && (searchModel.SaleOrder == null || p.SaleOrder.Contains(searchModel.SaleOrder))
                                   && (searchModel.OrderDelivery == null || p.OrderDelivery.Contains(searchModel.OrderDelivery))
                                   && (searchModel.Actived == null || p.Actived == searchModel.Actived)
                                   //Ngày bắt đầu
                                   && (searchModel.FromDate_From == null || searchModel.FromDate_From <= p.FromDate)
                                   && (searchModel.FromDate_To == null || p.FromDate <= searchModel.FromDate_To)
                                   //Ngày kết thúc
                                   && (searchModel.ToDate_From == null || searchModel.ToDate_From <= p.ToDate)
                                   && (searchModel.ToDate_To == null || p.ToDate <= searchModel.ToDate_To)

                                   orderby p.ProductWarrantyCode descending
                                   select new ProductWarrantyViewModel
                                   {
                                       ProductWarrantyId = p.ProductWarrantyId,
                                       ERPProductCode = s.ERPProductCode,
                                       ProductId = p.ProductId,
                                       ProductWarrantyCode = p.ProductWarrantyCode,
                                       ProfileId = p.ProfileId,
                                       ProfileName = a.ProfileName,
                                       Profile_ProfileName = a.ProfileName,
                                       ProductName = s.ProductName,
                                       FromDate = p.FromDate,
                                       WarrantyId = p.WarrantyId,
                                       WarrantyName = w.WarrantyName,
                                       SerriNo = p.SerriNo,
                                       ProductWarrantyNo = p.ProductWarrantyNo,
                                       ToDate = p.ToDate,
                                       CreateBy = p.CreateBy,
                                       CreateTime = p.CreateTime,
                                       LastEditBy = p.LastEditBy,
                                       LastEditTime = p.LastEditTime,
                                       Actived = p.Actived,
                                       //Thông tin khách hàng
                                       SaleOrder = p.SaleOrder,
                                       OrderDelivery = p.OrderDelivery,
                                       Phone = p.Phone,
                                       Address = p.Address,
                                       ProvinceName = province.ProvinceName == null ? "" : ", " + province.ProvinceName,
                                       DistrictName = district.DistrictName == null ? "" : ", " + district.Appellation + " " + district.DistrictName,
                                       WardName = ward.WardName == null ? "" : ", " + ward.Appellation + " " + ward.WardName
                                   });

            #region Search by Phone (not used)
            //if (!string.IsNullOrEmpty(searchModel.Phone))
            //{
            //    //Tìm số điện thoại trong bảng phụ
            //    var query1 = (from phone in _context.ProfilePhoneModel
            //                  where phone.PhoneNumber.Contains(searchModel.Phone)
            //                  group phone by phone.ProfileId into g
            //                  select g.Key.Value);
            //    //Tìm số điện thoại trong bảng chính 
            //    var query2 = (from phone in _context.ProfileModel
            //                  where phone.Phone.Contains(searchModel.Phone)
            //                  group phone by phone.ProfileId into g
            //                  select g.Key).Union(query1);
            //    //Tìm số điện thoại trong Đăng ký bảo hành
            //    var query3 = (from phone in proWarrantyList
            //                  where phone.Phone.Contains(searchModel.Phone)
            //                  group phone by phone.ProfileId into g
            //                  select g.Key).Union(query2);

            //    proWarrantyList = from warranty in proWarrantyList
            //                      join profileId in query3 on warranty.ProfileId equals profileId
            //                      orderby warranty.ProductWarrantyCode descending
            //                      select warranty;
            //}
            #endregion

            //Search địa chỉ
            if (!string.IsNullOrEmpty(searchModel.Address))
            {
                var addressLst = searchModel.Address.Replace(',', ' ').Replace('.', ' ').Replace("  ", " ").Split(' ');
                if (addressLst != null && addressLst.Count() > 0)
                {
                    foreach (var address in addressLst)
                    {
                        proWarrantyList = from warranty in proWarrantyList
                                          where (warranty.Address + warranty.WardName + warranty.ProvinceName + warranty.DistrictName).Contains(address)
                                          select warranty;
                    }
                }
            }
            return proWarrantyList;
        }

        /// <summary>
        /// Get ProductWarranty by Id
        /// </summary>
        /// <param name="id">ProductWarrantyId</param>
        /// <returns>ProductWarranty View Model</returns>
        public ProductWarrantyViewModel GetById(Guid id)
        {
            var proWarranty = (from p in _context.ProductWarrantyModel
                               join a in _context.ProfileModel on p.ProfileId equals a.ProfileId
                               join s in _context.ProductModel on p.ProductId equals s.ProductId
                               join w in _context.WarrantyModel on p.WarrantyId equals w.WarrantyId
                               orderby p.ProductWarrantyCode
                               where p.ProductWarrantyId == id
                               select new ProductWarrantyViewModel
                               {
                                   ProductWarrantyId = p.ProductWarrantyId,
                                   ProductId = p.ProductId,
                                   CompanyId = p.CompanyId,
                                   ProductWarrantyCode = p.ProductWarrantyCode,
                                   ProfileId = p.ProfileId,
                                   ProfileName = a.ProfileName,
                                   Profile_ProfileName = a.ProfileName,
                                   ProductName = s.ProductName,
                                   FromDate = p.FromDate,
                                   WarrantyId = p.WarrantyId,
                                   WarrantyName = w.WarrantyName,
                                   SerriNo = p.SerriNo,
                                   ProductWarrantyNo = p.ProductWarrantyNo,
                                   ToDate = p.ToDate,
                                   CreateBy = p.CreateBy,
                                   CreateTime = p.CreateTime,
                                   LastEditBy = p.LastEditBy,
                                   LastEditTime = p.LastEditTime,
                                   Actived = p.Actived,
                                   //Thông tin khách hàng
                                   SaleOrder = p.SaleOrder,
                                   OrderDelivery = p.OrderDelivery,
                                   Warranty_ProfileName = p.ProfileName,
                                   ProfileShortName = p.ProfileShortName,
                                   Age = p.Age,
                                   Phone = p.Phone,
                                   Email = p.Email,
                                   ProvinceId = p.ProvinceId,
                                   DistrictId = p.DistrictId,
                                   WardId = p.WardId,
                                   Address = p.Address,
                                   Note = p.Note,
                                   ActivatedQuantity = p.ActivatedQuantity ?? 1
                               }).FirstOrDefault();
            return proWarranty;
        }

        public TaskProductWarrantyViewModel GetTaskProductWarrantyById(Guid id)
        {
            var proWarranty = (from p in _context.ProductWarrantyModel
                               join a in _context.ProfileModel on p.ProfileId equals a.ProfileId
                               join s in _context.ProductModel on p.ProductId equals s.ProductId
                               join w in _context.WarrantyModel on p.WarrantyId equals w.WarrantyId
                               orderby p.ProductWarrantyCode
                               where p.ProductWarrantyId == id
                               select new TaskProductWarrantyViewModel
                               {
                                   ProductWarrantyId = p.ProductWarrantyId,
                                   ProductId = p.ProductId,
                                   ProductWarrantyCode = p.ProductWarrantyCode,
                                   ProfileId = p.ProfileId,
                                   ProfileName = a.ProfileName,
                                   Profile_ProfileName = a.ProfileName,
                                   ProductName = s.ProductName,
                                   FromDate = p.FromDate,
                                   WarrantyId = p.WarrantyId,
                                   WarrantyName = w.WarrantyName,
                                   SerriNo = p.SerriNo,
                                   ProductWarrantyNo = p.ProductWarrantyNo,
                                   ToDate = p.ToDate,
                                   CreateBy = p.CreateBy,
                                   CreateTime = p.CreateTime,
                                   LastEditBy = p.LastEditBy,
                                   LastEditTime = p.LastEditTime,
                                   Actived = p.Actived
                               }).FirstOrDefault();
            return proWarranty;
        }

        public ProductWarrantyModel Create(ProductWarrantyViewModel viewModel)
        {
            var proWarrantyNew = new ProductWarrantyModel
            {
                ProductWarrantyId = Guid.NewGuid(),
                ProductId = viewModel.ProductId,
                ProfileId = viewModel.ProfileId,
                WarrantyId = viewModel.WarrantyId,
                CompanyId = viewModel.CompanyId,
                FromDate = viewModel.FromDate,
                SerriNo = viewModel.SerriNo,
                ProductWarrantyNo = viewModel.ProductWarrantyNo,
                ToDate = viewModel.ToDate,
                CreateBy = viewModel.CreateBy,
                CreateTime = DateTime.Now,
                Actived = true,
                //Thông tin khách hàng
                SaleOrder = viewModel.SaleOrder,
                OrderDelivery = viewModel.OrderDelivery,
                ProfileName = viewModel.Warranty_ProfileName,
                ProfileShortName = viewModel.ProfileShortName,
                Age = viewModel.Age,
                Phone = viewModel.Phone,
                Email = viewModel.Email,
                ProvinceId = viewModel.ProvinceId,
                DistrictId = viewModel.DistrictId,
                WardId = viewModel.WardId,
                Address = viewModel.Address,
                Note = viewModel.Note,
            };
            return _context.ProductWarrantyModel.Add(proWarrantyNew);
        }

        public void Update(ProductWarrantyViewModel productWarrantyVM)
        {
            var productWarInDb = _context.ProductWarrantyModel
                                         .FirstOrDefault(p => p.ProductWarrantyId == productWarrantyVM.ProductWarrantyId);
            if (productWarInDb != null)
            {
                productWarInDb.FromDate = productWarrantyVM.FromDate;
                productWarInDb.ToDate = productWarrantyVM.ToDate;
                productWarInDb.ProfileId = productWarrantyVM.ProfileId;
                productWarInDb.ProductId = productWarrantyVM.ProductId;
                productWarInDb.WarrantyId = productWarrantyVM.WarrantyId;
                productWarInDb.CompanyId = productWarrantyVM.CompanyId;
                productWarInDb.SerriNo = productWarrantyVM.SerriNo;
                productWarInDb.ProductWarrantyNo = productWarrantyVM.ProductWarrantyNo;
                productWarInDb.LastEditBy = productWarrantyVM.LastEditBy;
                productWarInDb.LastEditTime = productWarrantyVM.LastEditTime;
                productWarInDb.Actived = productWarrantyVM.Actived;
                //Thông tin khách hàng
                productWarInDb.SaleOrder = productWarrantyVM.SaleOrder;
                productWarInDb.OrderDelivery = productWarrantyVM.OrderDelivery;
                productWarInDb.ProfileName = productWarrantyVM.Warranty_ProfileName;
                productWarInDb.ProfileShortName = productWarrantyVM.ProfileShortName;
                productWarInDb.Age = productWarrantyVM.Age;
                productWarInDb.Phone = productWarrantyVM.Phone;
                productWarInDb.Email = productWarrantyVM.Email;
                productWarInDb.ProvinceId = productWarrantyVM.ProvinceId;
                productWarInDb.DistrictId = productWarrantyVM.DistrictId;
                productWarInDb.WardId = productWarrantyVM.WardId;
                productWarInDb.Address = productWarrantyVM.Address;
                productWarInDb.Note = productWarrantyVM.Note;

                _context.Entry(productWarInDb).State = EntityState.Modified;
            }
        }

        public bool Delete(Guid id)
        {
            var proWarrarnty = _context.ProductWarrantyModel.FirstOrDefault(p => p.ProductWarrantyId == id);
            if (proWarrarnty != null)
            {
                _context.Entry(proWarrarnty).State = EntityState.Deleted;
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}