using ISD.Constant;
using ISD.EntityModels;
using ISD.Repositories.Infrastructure.Extensions;
using ISD.Resources;
using ISD.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ISD.Repositories
{
    public class AddressBookRepository
    {
        private EntityDataContext _context;
        public AddressBookRepository(EntityDataContext dataContext)
        {
            _context = dataContext;
        }

        /// <summary>
        /// Danh sách Địa chỉ của khách hàng
        /// Địa chỉ chính: Address trong ProfileModel
        /// Địa chỉ còn lại: AddressBookModel
        /// </summary>
        /// <param name="profileId"></param>
        /// <returns></returns>
        public List<AddressBookViewModel> GetAll(Guid? profileId)
        {
            var addressBookList = (from a in _context.AddressBookModel
                                       //Catalog (AddressType)
                                       //join c in _context.CatalogModel on a.AddressTypeCode equals c.CatalogCode
                                   join cTemp in _context.CatalogModel on new { AddressTypeCode = a.AddressTypeCode, Type = ConstCatalogType.AddressType } equals new { AddressTypeCode = cTemp.CatalogCode, Type = cTemp.CatalogTypeCode } into list0
                                   from c in list0.DefaultIfEmpty()
                                       //Province
                                   join prTmp in _context.ProvinceModel on a.ProvinceId equals prTmp.ProvinceId into list1
                                   from pr in list1.DefaultIfEmpty()
                                       //District
                                   join dsTmp in _context.DistrictModel on a.DistrictId equals dsTmp.DistrictId into list2
                                   from ds in list2.DefaultIfEmpty()
                                       //Ward
                                   join waTmp in _context.WardModel on a.WardId equals waTmp.WardId into list3
                                   from wa in list3.DefaultIfEmpty()
                                       //Catalog: Loại địa chỉ
                                       //join coTmp in _context.CatalogModel on a.CountryCode equals coTmp.CatalogCode into list4
                                       //from co in list4.DefaultIfEmpty()
                                   join coTmp in _context.CatalogModel on new { CountryCode = a.CountryCode, Type = ConstCatalogType.Country } equals new { CountryCode = coTmp.CatalogCode, Type = coTmp.CatalogTypeCode } into list4
                                   from co in list4.DefaultIfEmpty()
                                       //Create User
                                   join acc in _context.AccountModel on a.CreateBy equals acc.AccountId
                                   join s in _context.SalesEmployeeModel on acc.EmployeeCode equals s.SalesEmployeeCode
                                   where a.ProfileId == profileId
                                   //Type of Address type
                                   && c.CatalogTypeCode == ConstCatalogType.AddressType

                                   orderby a.isMain descending, a.CreateTime descending
                                   select new AddressBookViewModel
                                   {
                                       AddressBookId = a.AddressBookId,
                                       ProfileId = a.ProfileId,
                                       AddressTypeCode = a.AddressTypeCode,
                                       AddressTypeName = c.CatalogText_vi,
                                       Address = a.Address,
                                       Address2 = a.Address2,
                                       CountryCode = a.CountryCode,
                                       CountryName = co.CatalogText_vi,
                                       Note = a.Note,
                                       //Create User
                                       CreateUser = s.SalesEmployeeName,
                                       CreateTime = a.CreateTime,
                                       ProvinceId = a.ProvinceId,
                                       ProvinceName = pr == null ? "" : ", " + pr.ProvinceName,
                                       DistrictId = a.DistrictId,
                                       DistrictName = ds == null ? "" : ", " + ds.Appellation + " " + ds.DistrictName,
                                       WardId = a.WardId,
                                       WardName = wa == null ? "" : ", " + wa.Appellation + " " + wa.WardName,
                                       //isMain = a.isMain
                                       isMain = false

                                   }).ToList();

            //Địa chỉ chính
            var mainAddress = (from p in _context.ProfileModel
                                   //Province
                               join prTmp in _context.ProvinceModel on p.ProvinceId equals prTmp.ProvinceId into list1
                               from pr in list1.DefaultIfEmpty()
                                   //District
                               join dsTmp in _context.DistrictModel on p.DistrictId equals dsTmp.DistrictId into list2
                               from ds in list2.DefaultIfEmpty()
                                   //Ward
                               join waTmp in _context.WardModel on p.WardId equals waTmp.WardId into list3
                               from wa in list3.DefaultIfEmpty()
                                   //Create User
                               join acc in _context.AccountModel on p.CreateBy equals acc.AccountId into ag
                               from ac in ag.DefaultIfEmpty()
                                   //Catalog: Loại địa chỉ
                                   //join coTmp in _context.CatalogModel on p.AddressTypeCode equals coTmp.CatalogCode into list4
                                   //from co in list4.DefaultIfEmpty()
                               join coTmp in _context.CatalogModel on new { AddressTypeCode = p.AddressTypeCode, Type = ConstCatalogType.AddressType } equals new { AddressTypeCode = coTmp.CatalogCode, Type = coTmp.CatalogTypeCode } into list4
                               from co in list4.DefaultIfEmpty()
                               join cotryTmp in _context.CatalogModel on new { CountryCode = p.CountryCode, Type = ConstCatalogType.Country } equals new { CountryCode = cotryTmp.CatalogCode, Type = cotryTmp.CatalogTypeCode } into list5
                               from cotry in list5.DefaultIfEmpty()
                               where p.ProfileId == profileId
                               //Type of Address type
                               //&& co.CatalogTypeCode == ConstCatalogType.AddressType
                               select new AddressBookViewModel()
                               {
                                   ProfileId = p.ProfileId,
                                   Address = p.Address,
                                   //Create User
                                   CreateUser = ac.FullName == null ? ac.UserName : ac.FullName,
                                   CreateTime = p.CreateTime,
                                   ProvinceId = p.ProvinceId,
                                   ProvinceName = pr == null ? "" : ", " + pr.ProvinceName,
                                   DistrictId = p.DistrictId,
                                   DistrictName = ds == null ? "" : ", " + ds.Appellation + " " + ds.DistrictName,
                                   WardId = p.WardId,
                                   WardName = wa == null ? "" : ", " + wa.Appellation + " " + wa.WardName,
                                   isMain = true,
                                   AddressTypeCode = co == null ? "" : co.CatalogCode,
                                   AddressTypeName = co == null ? "" : co.CatalogText_vi,
                                   CountryCode = p.CountryCode,
                                   CountryName = cotry.CatalogText_vi,
                               }).FirstOrDefault();

            addressBookList.Insert(0, mainAddress);
            if (addressBookList != null && addressBookList.Count > 0)
            {
                if (addressBookList[0] != null)
                {
                    addressBookList = addressBookList.OrderBy(p => p.isMain != true).ThenBy(p => p.isMain).ToList();
                }
                else
                {
                    addressBookList = new List<AddressBookViewModel>();
                }
            }
            return addressBookList;
        }

        public AddressBookViewModel GetById(Guid? addressBookId)
        {
            var addressBook = (from a in _context.AddressBookModel
                                   //Catalog (AddressType)
                                   //join c in _context.CatalogModel on a.AddressTypeCode equals c.CatalogCode
                               join cTemp in _context.CatalogModel on new { AddressTypeCode = a.AddressTypeCode, Type = ConstCatalogType.AddressType } equals new { AddressTypeCode = cTemp.CatalogCode, Type = cTemp.CatalogTypeCode } into list0
                               from c in list0.DefaultIfEmpty()
                                   //Province
                               join prTmp in _context.ProvinceModel on a.ProvinceId equals prTmp.ProvinceId into list1
                               from pr in list1.DefaultIfEmpty()
                                   //District
                               join dsTmp in _context.DistrictModel on a.DistrictId equals dsTmp.DistrictId into list2
                               from ds in list2.DefaultIfEmpty()
                                   //Ward
                               join waTmp in _context.WardModel on a.WardId equals waTmp.WardId into list3
                               from wa in list3.DefaultIfEmpty()
                               where a.AddressBookId == addressBookId
                               select new AddressBookViewModel()
                               {
                                   //GUID
                                   AddressBookId = a.AddressBookId,
                                   //Mã Profile
                                   ProfileId = a.ProfileId,
                                   //Loại địa chỉ
                                   AddressTypeCode = a.AddressTypeCode,
                                   //Địa chỉ
                                   Address = a.Address,
                                   Address2 = a.Address2,
                                   //Quận/Huyện
                                   ProvinceId = a.ProvinceId,
                                   ProvinceName = pr.ProvinceName,
                                   //Tỉnh/Thành phố
                                   DistrictId = a.DistrictId,
                                   DistrictName = ds.DistrictName,
                                   //Phường xã
                                   WardId = a.WardId,
                                   WardName = wa.WardName,
                                   //Quốc Gia
                                   CountryCode = a.CountryCode,
                                   //Ghi chú
                                   Note = a.Note,
                                   //Liên hệ chính
                                   //isMain = addressBook.isMain
                                   isMain = false
                               }).FirstOrDefault();
            return addressBook;
        }

        /// <summary>
        /// Thêm mới địa chỉ
        /// 1. Nếu       tick "Địa chỉ chính" (isMain) thì update Address trong ProfileModel, lấy Address hiện tại trong ProfileModel lưu vào AddressBookModel
        /// 2. Nếu không tick "Địa chỉ chính" (isMain) thì create AddressBookModel
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public AddressBookModel Create(AddressBookModel viewModel)
        {
            //1.
            if (viewModel.isMain == true)
            {
                var profile = _context.ProfileModel.FirstOrDefault(p => p.ProfileId == viewModel.ProfileId);
                if (profile != null)
                {
                    //lấy Address hiện tại trong ProfileModel lưu vào AddressBookModel (isMain = false)
                    AddressBookModel addressBook = new AddressBookModel();
                    addressBook.AddressBookId = Guid.NewGuid();
                    //addressBook.AddressTypeCode = ConstAddressType.GH;
                    addressBook.AddressTypeCode = profile.AddressTypeCode;
                    addressBook.Address = profile.Address;
                    addressBook.ProfileId = viewModel.ProfileId;
                    addressBook.ProvinceId = profile.ProvinceId;
                    addressBook.DistrictId = profile.DistrictId;
                    addressBook.WardId = profile.WardId;
                    // addressBook.CountryCode = ConstCountry.VN;
                    addressBook.CountryCode = profile.CountryCode;
                    addressBook.isMain = false;
                    addressBook.CreateBy = viewModel.CreateBy;
                    addressBook.CreateTime = viewModel.CreateTime;
                    _context.Entry(addressBook).State = EntityState.Added;

                    //update Address trong ProfileModel
                    profile.AddressTypeCode = viewModel.AddressTypeCode;
                    profile.Address = viewModel.Address;
                    profile.ProvinceId = viewModel.ProvinceId;
                    profile.DistrictId = viewModel.DistrictId;
                    profile.WardId = viewModel.WardId;
                    profile.LastEditBy = viewModel.LastEditBy;
                    profile.LastEditTime = viewModel.LastEditTime;
                    profile.CountryCode = viewModel.CountryCode;
                    //profile.isForeignCustomer = viewModel.CountryCode == ConstCountry.QT ? true : false;
                }
            }
            //2.
            else
            {
                // var AddressNew = new AddressBookModel();
                //AddressNew.MapAddressBook(viewModel);
                // AddressNew.CreateBy = viewModel.CreateBy;
                viewModel.AddressBookId = Guid.NewGuid();
                _context.Entry(viewModel).State = EntityState.Added;
                return viewModel;
            }
            return null;
        }

        /// <summary>
        /// Cập nhật địa chỉ
        /// 1. Nếu cập nhật địa chỉ thường thành địa chỉ chính => update Address trong ProfileModel, xóa địa chỉ chính (nếu có) trong AddressBookModel
        /// 2. Nếu cập nhật địa chỉ thường (không cập nhật isMain) => cập nhật AddressBookModel
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public bool Update(AddressBookModel viewModel)
        {
            if (viewModel.isMain == true)
            {
                var profile = _context.ProfileModel.FirstOrDefault(p => p.ProfileId == viewModel.ProfileId);
                if (profile != null)
                {
                    //lấy Address hiện tại trong ProfileModel lưu vào AddressBookModel (isMain = false)
                    AddressBookModel addressBook = new AddressBookModel();
                    addressBook.AddressBookId = Guid.NewGuid();
                    //addressBook.AddressTypeCode = ConstAddressType.GH;
                    addressBook.AddressTypeCode = profile.AddressTypeCode;
                    addressBook.Address = profile.Address;
                    addressBook.ProfileId = viewModel.ProfileId;
                    addressBook.ProvinceId = profile.ProvinceId;
                    addressBook.DistrictId = profile.DistrictId;
                    addressBook.WardId = profile.WardId;
                    //  addressBook.CountryCode = ConstCountry.VN;
                    addressBook.CountryCode = profile.CountryCode;
                    addressBook.isMain = false;
                    addressBook.CreateBy = viewModel.LastEditBy;
                    addressBook.CreateTime = viewModel.LastEditTime;
                    _context.Entry(addressBook).State = EntityState.Added;

                    //Cập nhật địa chỉ chính
                    profile.AddressTypeCode = viewModel.AddressTypeCode;
                    profile.Address = viewModel.Address;
                    profile.ProvinceId = viewModel.ProvinceId;
                    profile.DistrictId = viewModel.DistrictId;
                    profile.WardId = viewModel.WardId;
                    profile.LastEditBy = viewModel.LastEditBy;
                    profile.LastEditTime = viewModel.LastEditTime;
                    profile.CountryCode = viewModel.CountryCode;
                    //profile.isForeignCustomer = viewModel.CountryCode == ConstCountry.QT ? true : false;

                    var existAddress = _context.AddressBookModel.FirstOrDefault(p => p.AddressBookId == viewModel.AddressBookId);
                    if (existAddress != null)
                    {
                        _context.AddressBookModel.Remove(existAddress);
                    }

                    return true;
                }
            }
            else
            {
                var AddressInDB = _context.AddressBookModel.FirstOrDefault(p => p.AddressBookId == viewModel.AddressBookId);
                if (AddressInDB != null)
                {
                    // AddressInDB.MapAddressBook(viewModel);
                    AddressInDB.AddressTypeCode = viewModel.AddressTypeCode;
                    AddressInDB.CountryCode = viewModel.CountryCode;
                    AddressInDB.ProvinceId = viewModel.ProvinceId;
                    AddressInDB.DistrictId = viewModel.DistrictId;
                    AddressInDB.WardId = viewModel.WardId;
                    AddressInDB.Address = viewModel.Address;
                    AddressInDB.Address2 = viewModel.Address2;
                    AddressInDB.Note = viewModel.Note;
                    AddressInDB.LastEditBy = viewModel.LastEditBy;
                    AddressInDB.LastEditTime = viewModel.LastEditTime;
                    _context.Entry(AddressInDB).State = EntityState.Modified;
                    return true;
                }
            }
            return false;
        }
    }
}
