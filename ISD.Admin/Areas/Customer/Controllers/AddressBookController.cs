using ISD.Constant;
using ISD.Core;
using ISD.EntityModels;
using ISD.Repositories;
using ISD.Resources;
using ISD.ViewModels;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Customer.Controllers
{
    public class AddressBookController : BaseController
    {
        private AddressBookRepository _addressBookRepository;

        public AddressBookController()
        {
            _addressBookRepository = new AddressBookRepository(_context);
        }
        public ActionResult _List(Guid? id, bool? isLoadContent = false)
        {
            return ExecuteSearch(() =>
            {
                var addressBookList = _addressBookRepository.GetAll(id);
                if (isLoadContent == true)
                {
                    return PartialView("_ListContent", addressBookList);
                }
                return PartialView(addressBookList);
            });
        }
        public ActionResult _Create(Guid ProfileId, string ProfileType = null)
        {
            AddressBookViewModel adbVM = new AddressBookViewModel()
            {
                ProfileId = ProfileId
            };
            CreateViewBag(ProfileType: ProfileType);
            return PartialView("_FromAddress", adbVM);
        }
        public ActionResult _Edit(Guid AddressBookId)
        {
            var addressBookVM = _addressBookRepository.GetById(AddressBookId);
            CreateViewBag(addressBookVM.ProvinceId, addressBookVM.AddressTypeCode, addressBookVM.CountryCode, addressBookVM.DistrictId, addressBookVM.WardId);
            return PartialView("_FromAddress", addressBookVM);
        }
        [HttpPost]
        public ActionResult Save(AddressBookModel addressBook)
        {
            if (addressBook.CountryCode == ConstCountry.VN && (addressBook.ProvinceId == null || addressBook.ProvinceId == Guid.Empty))
            {
                ModelState.AddModelError("ProvinceId", "Vui lòng nhập thông tin \"Tỉnh / Thành phố\"");
            }
            return ExecuteContainer(() =>
            {
                //Hiện tại QT chưa có tỉnh thành phố => set province vể null trước khi thêm hoặc sửa
                if (addressBook.CountryCode == ConstCountry.QT)
                {
                    addressBook.ProvinceId = null;
                    addressBook.DistrictId = null;
                    addressBook.WardId = null;
                }
                //Thêm mới
                if (addressBook.AddressBookId == Guid.Empty)
                {
                    #region Create
                    addressBook.CreateBy = CurrentUser.AccountId;
                    addressBook.CreateTime = DateTime.Now;

                    //Nếu chọn là địa chỉ chính thì set địa chỉ còn lại là false
                    //if (addressBookVM.isMain == true)
                    //{
                    //    var addressbookList = _context.AddressBookModel.Where(p => p.ProfileId == addressBookVM.ProfileId).ToList();
                    //    foreach (var item in addressbookList)
                    //    {
                    //        item.isMain = false;
                    //        _context.Entry(item).State = EntityState.Modified;
                    //    }
                    //}

                    _addressBookRepository.Create(addressBook);
                    _context.SaveChanges();

                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.Created,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.Customer_CustomerAddress.ToLower())
                    });
                    #endregion
                }
                else
                {
                    #region Edit
                    addressBook.LastEditBy = CurrentUser.AccountId;
                    addressBook.LastEditTime = DateTime.Now;

                    //if (addressBookVM.isMain == true)
                    //{
                    //    var addressBookList = _context.AddressBookModel.Where(p => p.ProfileId == addressBookVM.ProfileId && p.AddressBookId != addressBookVM.AddressBookId).ToList();
                    //    foreach (var item in addressBookList)
                    //    {
                    //        item.isMain = false;
                    //        _context.Entry(item).State = EntityState.Modified;
                    //    }
                    //}

                    var ret = _addressBookRepository.Update(addressBook);
                    if (ret)
                    {
                        _context.SaveChanges();
                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.Created,
                            Success = true,
                            Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.Customer_CustomerAddress.ToLower())
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.NotFound,
                            Success = false,
                            Data = ""
                        });
                    }
                    #endregion
                }
            });
        }
        //GET: /AddressBook/Delete
        #region Delete
        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            return ExecuteDelete(() =>
            {
                var addressBook = _context.AddressBookModel.FirstOrDefault(p => p.AddressBookId == id);
                if (addressBook != null)
                {
                    _context.Entry(addressBook).State = EntityState.Deleted;
                    _context.SaveChanges();

                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.Created,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.AddressBook_Address.ToLower())
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = ""
                    });
                }
            });
        }
        #endregion
        #region CreateViewBag, Helper
        public void CreateViewBag(Guid? ProvinceId = null, string AddressTypeCode = null, string CountryCode = ConstCountryCode.VN, Guid? DistrictId = null, Guid? WardId = null, string ProfileType = null)
        {
            //Get province
            var _provinceRepository = new ProvinceRepository(_context);
            var provinceList = _provinceRepository.GetAll();
            ViewBag.ProvinceId = new SelectList(provinceList, "ProvinceId", "ProvinceName", ProvinceId);

            //Get District
            var _districtRepository = new DistrictRepository(_context);
            var districtList = _districtRepository.GetBy(ProvinceId);
            ViewBag.DistrictId = new SelectList(districtList, "DistrictId", "DistrictName", DistrictId);

            //Get ward
            var _wardRepository = new WardRepository(_context);
            var wardList = _wardRepository.GetBy(DistrictId);
            ViewBag.WardId = new SelectList(wardList, "WardId", "WardName", WardId);

            //Get AddressType
            var _catalogRepository = new CatalogRepository(_context);
            var addressList = _catalogRepository.GetBy(ConstCatalogType.AddressType);
            if (ProfileType == ConstProfileType.Competitor && string.IsNullOrEmpty(AddressTypeCode))
            {
                AddressTypeCode = ConstAddressType.NhaMay;
            }
            ViewBag.AddressTypeCode = new SelectList(addressList, "CatalogCode", "CatalogText_vi", AddressTypeCode);

            //Get cuontry
            var countryList = _catalogRepository.GetBy(ConstCatalogType.Country);
            ViewBag.CountryCode = new SelectList(countryList, "CatalogCode", "CatalogText_vi", CountryCode);
        }

        //GetDistrictByProvince
        public ActionResult GetDistrictByProvince(Guid? ProvinceId)
        {
            var _districtRepository = new DistrictRepository(_context);
            var districtList = _districtRepository.GetBy(ProvinceId);
            var districtIdList = new SelectList(districtList, "Id", "Name");
            return Json(districtIdList, JsonRequestBehavior.AllowGet);
        }

        //GetWardByDistrict
        public ActionResult GetWardByDistrict(Guid? DistrictId)
        {
            var _wardRepository = new WardRepository(_context);
            var wardList = _wardRepository.GetBy(DistrictId);
            var wardIdList = new SelectList(wardList, "Id", "Name");
            return Json(wardIdList, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}