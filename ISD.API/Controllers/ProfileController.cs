using ISD.Constant;
using ISD.EntityModels;
using ISD.Extensions;
using ISD.Repositories;
using ISD.Resources;
using ISD.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ISD.Core;
using System.Net.Mail;
using System.Text;

namespace ISD.API.Controllers
{
    public class ProfileController : BaseController
    {
        // GET: Profile

        #region Helper get master data

        #region Get customer source code
        public ActionResult GetCustomerSource(bool? isSRNQ, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                var _catalogRepository = new CatalogRepository(_context);

                var srcLst = _catalogRepository.GetBy(ConstCatalogType.CustomerSource)
                                                .Select(p => new ISDSelectStringItem()
                                                {
                                                    id = p.CatalogCode,
                                                    name = p.CatalogText_vi,
                                                });

                //User NPP/SRNQ: Yêu cầu Chỉ hiển thị Nguồn KH HT NPP/ĐL 
                //User Nhân viên AC: Hide Nguồn NPP/SRNQ
                if (isSRNQ == true)
                {
                    srcLst = srcLst.Where(p => p.id == "NPP").ToList();
                }
                else
                {
                    srcLst = srcLst.Where(p => p.id != "NPP").ToList();
                }
                return _APISuccess(srcLst);
            });
        }
        #endregion Get customer source code

        #region Get customer category code
        public ActionResult GetCustomerCategory(string CompanyCode, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                var _catalogRepository = new CatalogRepository(_context);
                var catLst = new List<ISDSelectStringItem>();
                //Không truyền CompanyCode => load nhóm KH all
                if (string.IsNullOrEmpty(CompanyCode))
                {
                    catLst = _catalogRepository.GetBy(ConstCatalogType.CustomerCategory)
                                                 .Select(p => new ISDSelectStringItem()
                                                 {
                                                     id = p.CatalogCode,
                                                     name = p.CatalogCode + "_" + p.CatalogText_vi,
                                                 }).OrderBy(p => p.id).ToList();
                }

                //Nếu có truyền CompanyCode => load nhóm KH theo CompanyCode
                else
                {
                    catLst = _catalogRepository.GetCustomerCategory(CompanyCode)
                                                 .Select(p => new ISDSelectStringItem()
                                                 {
                                                     id = p.CatalogCode,
                                                     name = p.CatalogCode + "_" + p.CatalogText_vi,
                                                 }).OrderBy(p => p.id).ToList();
                }

                return _APISuccess(catLst);
            });
        }
        #endregion Get customer category code

        #region Get customer career code
        public ActionResult GetCustomerCareer(string CompanyCode, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                var _catalogRepository = new CatalogRepository(_context);

                var careerLst = _catalogRepository.GetCustomerCareer(CompanyCode)
                                                      .Select(p => new ISDSelectStringItem()
                                                      {
                                                          id = p.CatalogCode,
                                                          name = p.CatalogCode + "_" + p.CatalogText_vi,
                                                      });
                return _APISuccess(careerLst);
            });
        }
        #endregion Get customer career code

        #region Get sale org
        public ActionResult GetSaleOrg(Guid? AccountId, string CompanyCode, string CustomerSourceCode, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                var storeLst = _unitOfWork.StoreRepository.GetStoreByCustomerSource(AccountId.Value, CompanyCode, CustomerSourceCode)
                                .Select(p => new ISDSelectStringItemWithGuid()
                                {
                                    id = p.SaleOrgCode,
                                    name = p.StoreName,
                                    value = p.StoreId,
                                }).OrderBy(p => p.id == "1000").OrderBy(p => p.id == "2000").OrderBy(p => p.id == "3000");
                return _APISuccess(storeLst);
            });
        }

        public ActionResult GetSaleOrgBy(Guid? AccountId, string CompanyCode, string SaleOfficeCode, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                StoreRepository _storeRepo = new StoreRepository(_context);
                var storeLst = new List<ISDSelectStringItemWithGuid>();
                //Cập nhật: Load chi nhánh theo Phân quyền

                storeLst = _storeRepo.GetStoreByCompanyPermission(AccountId, CompanyCode)
                         .Select(p => new ISDSelectStringItemWithGuid()
                         {
                             id = p.SaleOrgCode,
                             name = p.StoreName,
                             value = p.StoreId,
                         }).OrderBy(p => p.id == "1000").OrderBy(p => p.id == "2000").OrderBy(p => p.id == "3000").ToList();

                return _APISuccess(storeLst);
            });
        }
        #endregion Get sale org

        #region Get category code
        public ActionResult GetCategory(string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                var _catalogRepository = new CatalogRepository(_context);

                var categoryList = _catalogRepository.GetBy(ConstCatalogType.Appoitment_Category)
                                                .Select(p => new ISDSelectStringItem()
                                                {
                                                    id = p.CatalogCode,
                                                    name = p.CatalogText_vi,
                                                });
                return _APISuccess(categoryList);
            });
        }
        #endregion Get category code

        #region Get age
        public ActionResult GetAge(string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                var _catalogRepository = new CatalogRepository(_context);

                var ageLst = _catalogRepository.GetBy(ConstCatalogType.Age)
                                                .Select(p => new ISDSelectStringItem()
                                                {
                                                    id = p.CatalogCode,
                                                    name = p.CatalogText_vi,
                                                });
                return _APISuccess(ageLst);
            });
        }
        #endregion Get age

        #region Get sale office
        public ActionResult GetSaleOffice(bool isForeignCustomer, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                var _catalogRepository = new CatalogRepository(_context);

                var saleOfficeLst = _catalogRepository.GetSaleOffice(isForeignCustomer)
                                                .Select(p => new ISDSelectStringItem()
                                                {
                                                    id = p.CatalogCode,
                                                    name = p.CatalogText_vi,
                                                });
                return _APISuccess(saleOfficeLst);
            });
        }
        #endregion Get sale office

        #region Get province
        public ActionResult GetProvince(string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                var _provinceRepository = new ProvinceRepository(_context);
                var provinceList = _provinceRepository.GetAll()
                                                .Where(p => p.Area == null)
                                                .Select(p => new ISDSelectGuidItem()
                                                {
                                                    id = p.ProvinceId,
                                                    name = p.ProvinceName,
                                                });
                return _APISuccess(provinceList.OrderByDescending(p => p.name == "Hồ Chí Minh").ThenByDescending(p => p.name == "Hà Nội"));
            });
        }

        public ActionResult GetProvinceBy(string SaleOfficeCode, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                //Load Tỉnh thành theo Khu vực (sắp xếp theo thứ tự các tỉnh thuộc khu vực chọn sẽ được xếp trước)
                var provinceList = new List<ISDSelectGuidItem>();
                var provinceLst = _context.ProvinceModel.Where(p => p.Actived == true)
                                       .Select(p => new ProvinceViewModel()
                                       {
                                           ProvinceId = p.ProvinceId,
                                           ProvinceCode = p.ProvinceCode,
                                           ProvinceName = p.ProvinceName,
                                           Area = p.Area,
                                           OrderIndex = p.OrderIndex
                                       }).ToList();

                if (!string.IsNullOrEmpty(SaleOfficeCode) && SaleOfficeCode != ConstSaleOffice.Khac)
                {
                    //int SaleOffice = int.Parse(SaleOfficeCode);
                    string SaleOffice = SaleOfficeCode;

                    //List<int> areaList = new List<int>();
                    //areaList.Add(SaleOffice);

                    //if (provinceLst != null && provinceLst.Count > 0)
                    //{
                    //    foreach (var item in provinceLst)
                    //    {
                    //        if (!areaList.Contains((int)item.Area))
                    //        {
                    //            areaList.Add((int)item.Area);
                    //        }
                    //    }
                    //}
                    //areaList = areaList.OrderBy(p => p != SaleOffice).ThenBy(p => p).ToList();
                    //provinceLst = provinceLst.OrderBy(p => areaList.FindIndex(p1 => p1 == p.Area)).ThenBy(p => p.ProvinceCode).ToList();

                    var existList = provinceLst.Select(p => p.Area).ToList();
                    if (!existList.Contains(SaleOffice))
                    {
                        provinceLst = provinceLst.Where(p => p.Area == null).OrderBy(p => p.ProvinceCode).ToList();
                    }
                    else
                    {
                        provinceLst = provinceLst.Where(p => p.Area == SaleOffice).OrderBy(p => p.ProvinceCode).ToList();
                    }
                }
                else
                {
                    //provinceLst = provinceLst.OrderBy(p => p.ProvinceCode).ToList();
                    provinceLst = provinceLst.Where(p => p.Area == null).OrderBy(p => p.ProvinceCode).ToList();
                }

                if (provinceLst != null && provinceLst.Count > 0)
                {
                    provinceList = provinceLst.Select(p => new ISDSelectGuidItem()
                    {
                        id = p.ProvinceId,
                        name = p.ProvinceName,
                    }).ToList();
                }
                return _APISuccess(provinceList.OrderByDescending(p => p.name == "Hồ Chí Minh").ThenByDescending(p => p.name == "Hà Nội"));
            });
        }
        #endregion Get province

        #region Get district
        public ActionResult GetDistrictBy(Guid ProvinceId, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                var _districtRepository = new DistrictRepository(_context);
                var districtList = _districtRepository.GetBy(ProvinceId)
                                                .Select(p => new ISDSelectGuidItem()
                                                {
                                                    id = p.DistrictId,
                                                    name = p.DistrictName,
                                                }).OrderBy(p => p.name.Length).ThenBy(p => p.name);
                return _APISuccess(districtList);
            });
        }
        #endregion Get district

        #region Get ward
        public ActionResult GetWardBy(Guid DistrictId, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                var _wardRepository = new WardRepository(_context);
                var wardList = _wardRepository.GetBy(DistrictId)
                                                .Select(p => new ISDSelectGuidItem()
                                                {
                                                    id = p.WardId,
                                                    name = p.WardName,
                                                }).OrderBy(p => p.name.Length).ThenBy(p => p.name);
                return _APISuccess(wardList);
            });
        }
        #endregion Get ward

        #region Get task status for GT
        //Chờ xử lý
        //Đang xử lý
        //Đã xong
        public ActionResult GetTaskStatus(string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                var _taskStatusRepository = new TaskStatusRepository(_context);
                var workflow = _context.WorkFlowModel.Where(p => p.WorkFlowCode == ConstWorkFlow.GT).FirstOrDefault();
                if (workflow != null)
                {
                    var taskStatusList = _taskStatusRepository.GetTaskStatusByWorkFlow(workflow.WorkFlowId)
                                                                 .Select(p => new ISDSelectGuidItem()
                                                                 {
                                                                     id = (Guid)p.TaskStatusId,
                                                                     name = p.TaskStatusName,
                                                                 });
                    return _APISuccess(taskStatusList);
                }
                else
                {
                    return _APISuccess(null);
                }

            });
        }
        #endregion Get task status for GT

        #region Get catalog category
        public ActionResult GetCatalogCategory(string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                var catalogCategoryList = _context.View_Catalog_Category.OrderBy(p => p.OrderIndex).Select(p => new ISDSelectGuidItem()
                {
                    id = p.CategoryId,
                    name = p.CategoryName,
                }).ToList();
                return _APISuccess(catalogCategoryList);
            });
        }
        #endregion Get catalog category

        #region Get catalog
        public ActionResult GetCatalog(Guid CategoryId, string SaleOrgCode, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                //parent category = CTL
                var parentCategory = _context.CategoryModel.Where(p => p.CategoryCode == "CTL").FirstOrDefault();
                if (parentCategory != null)
                {
                    var catalogList = (from p in _context.ProductModel
                                       where p.ParentCategoryId == parentCategory.CategoryId
                                       //theo danh mục
                                       && p.CategoryId == CategoryId
                                       && p.Actived == true
                                       orderby p.OrderIndex
                                       select new CatalogueViewModel()
                                       {
                                           ProductId = p.ProductId,
                                           ERPProductCode = p.ERPProductCode,
                                           ProductCode = p.ERPProductCode,
                                           ProductName = p.ProductName,
                                           Quantity = 0,
                                       }).ToList();

                    if (catalogList != null && catalogList.Count > 0)
                    {
                        var onHandList = _unitOfWork.StockRepository.GetStockOnHandBySaleOrg(SaleOrgCode);
                        foreach (var catalog in catalogList)
                        {
                            if (onHandList != null && onHandList.Count > 0)
                            {
                                var onHand = onHandList.Where(p => p.ProductId == catalog.ProductId).FirstOrDefault();
                                if (onHand != null)
                                {
                                    catalog.Quantity = (int)onHand.Qty;
                                }
                            }
                        }
                    }

                    return _APISuccess(catalogList);
                }
                else
                {
                    return _APISuccess(null);
                }
            });
        }
        #endregion Get catalog

        #region Get title
        public ActionResult GetTitle(string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                var _catalogRepository = new CatalogRepository(_context);
                var titleLst = _catalogRepository.GetBy(ConstCatalogType.Title)
                                                 .Where(p => p.CatalogCode != ConstTitle.Company)
                                                 .Select(p => new ISDSelectStringItem()
                                                 {
                                                     id = p.CatalogCode,
                                                     name = p.CatalogText_vi,
                                                 });

                return _APISuccess(titleLst);
            });
        }
        #endregion Get title

        #region Get contact position
        public ActionResult GetContactPosition(string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                var _catalogRepository = new CatalogRepository(_context);

                var positionLst = _catalogRepository.GetBy(ConstCatalogType.Position)
                                                .Select(p => new ISDSelectStringItem()
                                                {
                                                    id = p.CatalogCode,
                                                    name = p.CatalogText_vi,
                                                });
                return _APISuccess(positionLst);
            });
        }
        #endregion Get contact position

        #region Get contact department
        public ActionResult GetContactDepartment(string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                var _catalogRepository = new CatalogRepository(_context);

                var departmentLst = _catalogRepository.GetBy(ConstCatalogType.Department)
                                                .Select(p => new ISDSelectStringItem()
                                                {
                                                    id = p.CatalogCode,
                                                    name = p.CatalogText_vi,
                                                });
                return _APISuccess(departmentLst);
            });
        }
        #endregion Get contact department

        #region Get address type code
        public ActionResult GetAddressType(string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                var _catalogRepository = new CatalogRepository(_context);

                var addressList = _catalogRepository.GetBy(ConstCatalogType.AddressType)
                                                .Select(p => new ISDSelectStringItem()
                                                {
                                                    id = p.CatalogCode,
                                                    name = p.CatalogText_vi,
                                                });
                return _APISuccess(addressList);
            });
        }
        #endregion Get address type code

        #region Get contact info by contact id
        public ActionResult GetContactInfoBy(Guid ContactId, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                var profile = _context.ProfileModel.Where(p => p.ProfileId == ContactId && p.CustomerTypeCode == ConstCustomerType.Contact).FirstOrDefault();
                if (profile != null)
                {
                    return _APISuccess(new { profile.Phone, profile.Email });
                }
                else
                {
                    return _APIError(null);
                }

            });
        }
        #endregion Get contact info by contact id

        #region Get channel
        public ActionResult GetChannel(string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                var _catalogRepository = new CatalogRepository(_context);

                var channelLst = _catalogRepository.GetBy(ConstCatalogType.Appoitment_Channel)
                                                .Select(p => new ISDSelectStringItem()
                                                {
                                                    id = p.CatalogCode,
                                                    name = p.CatalogText_vi,
                                                });
                return _APISuccess(channelLst);
            });
        }
        #endregion Get channel

        #region Get ratings
        public ActionResult GetRatings(string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                var _catalogRepository = new CatalogRepository(_context);

                var ratingsLst = _catalogRepository.GetBy(ConstCatalogType.CustomerReviews)
                                                .Select(p => new ISDSelectStringItem()
                                                {
                                                    id = p.CatalogCode,
                                                    name = p.CatalogText_vi,
                                                });
                return _APISuccess(ratingsLst);
            });
        }

        public ActionResult GetEmployeeRatings(string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                var _catalogRepository = new CatalogRepository(_context);

                var ratingsLst = _catalogRepository.GetBy(ConstCatalogType.EmployeeRatings)
                                                .Select(p => new ISDSelectStringItem()
                                                {
                                                    id = p.CatalogCode,
                                                    name = p.CatalogText_vi,
                                                    additional = p.CatalogText_vi == "1" ? "worried" :
                                                                 (p.CatalogText_vi == "2" ? "anguished" :
                                                                 (p.CatalogText_vi == "3" ? "neutral_face" :
                                                                 (p.CatalogText_vi == "4" ? "blush" :
                                                                 (p.CatalogText_vi == "5" ? "kissing_heart" : null
                                                                 )))),
                                                    additional2 = p.CatalogText_vi == "1" ? "RẤT KÉM" :
                                                                 (p.CatalogText_vi == "2" ? "KÉM" :
                                                                 (p.CatalogText_vi == "3" ? "BÌNH THƯỜNG" :
                                                                 (p.CatalogText_vi == "4" ? "TỐT" :
                                                                 (p.CatalogText_vi == "5" ? "RẤT TỐT" : null
                                                                 )))),
                                                });

                return _APISuccess(ratingsLst);
            });
        }
        #endregion Get ratings

        #region Get wood grain
        public ActionResult GetWoodGrain(string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                var woodGrainLst = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.WoodGrainCategory)
                                                                  .Select(p => new ISDSelectStringItem()
                                                                  {
                                                                      id = p.CatalogCode,
                                                                      name = p.CatalogText_vi,
                                                                  }).ToList();

                return _APISuccess(woodGrainLst);
            });
        }
        #endregion Get wood grain

        #region Get collection
        public ActionResult GetCollection(string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                var collectionLst = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.ProductCollection)
                                                                  .Select(p => new ISDSelectStringItem()
                                                                  {
                                                                      id = p.CatalogCode,
                                                                      name = p.CatalogText_vi,
                                                                      additional = "CustomerTaste3",
                                                                  }).ToList();

                return _APISuccess(collectionLst);
            });
        }
        #endregion Get collection

        #region Get customer tastes ratings
        public ActionResult GetCustomerTastesRatings(string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                var _catalogRepository = new CatalogRepository(_context);

                var customerTastesRatingsLst = _catalogRepository.GetBy(ConstCatalogType.CustomerTastes)
                                                .OrderByDescending(p => p.OrderIndex)
                                                .Select(p => new ISDSelectStringItem()
                                                {
                                                    id = p.CatalogCode,
                                                    name = p.CatalogText_vi,
                                                });
                return _APISuccess(customerTastesRatingsLst);
            });
        }
        #endregion Get customer tastes ratings

        #region Get product group
        public ActionResult GetProductGroup(string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                var productGroupLst = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.ProductGroup)
                                                                  .Select(p => new ISDSelectStringItem()
                                                                  {
                                                                      id = p.CatalogCode,
                                                                      name = p.CatalogText_vi,
                                                                  }).ToList();

                return _APISuccess(productGroupLst);
            });
        }
        #endregion Get product group

        #region Get create ECC request
        public ActionResult GetCreateECCRequest(string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                var createRequestLst = new List<ISDSelectBoolItem>();
                createRequestLst.Add(new ISDSelectBoolItem()
                {
                    id = null,
                    name = "Không tạo",
                });
                createRequestLst.Add(new ISDSelectBoolItem()
                {
                    id = true,
                    name = "Đang yêu cầu",
                });
                createRequestLst.Add(new ISDSelectBoolItem()
                {
                    id = false,
                    name = "Đã tạo",
                });

                return _APISuccess(createRequestLst);
            });
        }
        #endregion

        #endregion Helper get master data

        #region Search Profile
        public ActionResult SearchProfile_bk(MobileProfileSearchViewModel searchViewModel, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                ProfileRepository repo = new ProfileRepository(_context);
                var profiles = repo.MobileSearch(searchViewModel);

                if (profiles != null && profiles.Count > 0)
                {
                    //get address list
                    foreach (var profile in profiles)
                    {
                        profile.AddressList = new List<string>();
                        var addressList = _unitOfWork.AddressBookRepository.GetAll(profile.ProfileId);
                        if (addressList != null && addressList.Count > 0)
                        {
                            foreach (var item in addressList)
                            {
                                item.Address += item.WardName + item.DistrictName + item.ProvinceName;
                                profile.AddressList.Add(item.Address);
                            }
                        }
                    }

                    return _APISuccess(profiles);
                }
                else
                {
                    return _APISuccess(profiles, "Không tìm thấy kết quả phù hợp! Bạn có muốn tạo KH mới không?");
                }

            });
        }

        public ActionResult SearchProfile(MobileProfileSearchViewModel searchViewModel, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                ProfileRepository repo = new ProfileRepository(_context);

                ProfileSearchViewModel searchStoreViewModel = new ProfileSearchViewModel();
                searchStoreViewModel.PageSize = searchViewModel.PageSize;
                searchStoreViewModel.PageNumber = searchViewModel.PageNumber;
                //Mã CRM
                searchStoreViewModel.ProfileCode = searchViewModel.ProfileCode;
                //Mã SAP
                searchStoreViewModel.ProfileForeignCode = searchViewModel.ProfileForeignCode;
                //Tên khách hàng
                searchStoreViewModel.ProfileName = searchViewModel.ProfileName;
                //Số điện thoại
                searchStoreViewModel.Phone = searchViewModel.ProfilePhone;
                //Mã số thuế
                searchStoreViewModel.TaxNo = searchViewModel.TaxNo;
                //Địa chỉ
                searchStoreViewModel.Address = searchViewModel.Address;
                //Loại
                searchStoreViewModel.Type = ConstCustomerType.Account;
                searchStoreViewModel.isMobile = true;
                //Công ty
                //var companyId = _unitOfWork.CompanyRepository.GetCompanyIdBy(searchViewModel.CompanyCode);
                //searchStoreViewModel.CompanyId = companyId;
                int filteredResultsCount;

                //Không hiển thị các Profile không hoạt động nữa.
                searchStoreViewModel.Actived = true;
                var profiles = repo.SearchQueryProfile(searchStoreViewModel, searchViewModel.AccountId, searchViewModel.CompanyCode, out filteredResultsCount);

                if (profiles != null && profiles.Count > 0)
                {
                    foreach (var profile in profiles)
                    {
                        #region //NV kinh doanh
                        var salesSupervisor = (from p in _context.PersonInChargeModel
                                               join e in _context.SalesEmployeeModel on p.SalesEmployeeCode equals e.SalesEmployeeCode
                                               join acc in _context.AccountModel on e.SalesEmployeeCode equals acc.EmployeeCode
                                               from roles in acc.RolesModel 
                                               where p.ProfileId == profile.ProfileId && p.CompanyCode == searchViewModel.CompanyCode
                                               select new
                                               {
                                                   SalesSupervisorCode = p.SalesEmployeeCode,
                                                   SalesSupervisorName = e.SalesEmployeeName,
                                                   DepartmentName = roles.isEmployeeGroup == true ? roles.RolesName : ""
                                               }).FirstOrDefault();

                        if (salesSupervisor != null)
                        {
                            profile.SalesSupervisorCode = salesSupervisor.SalesSupervisorCode;
                            profile.SalesSupervisorName = salesSupervisor.SalesSupervisorName;
                            profile.DepartmentName = salesSupervisor.DepartmentName;
                        }
                        #endregion
                    }

                    //get address list
                    foreach (var profile in profiles)
                    {
                        profile.AddressList = new List<string>();
                        var addressList = _unitOfWork.AddressBookRepository.GetAll(profile.ProfileId);
                        if (addressList != null && addressList.Count > 0)
                        {
                            foreach (var item in addressList)
                            {
                                item.Address += item.WardName + item.DistrictName + item.ProvinceName;
                                profile.AddressList.Add(item.Address);
                            }
                        }
                    }

                    return _APISuccess(new { profiles, totalResult = filteredResultsCount });
                }
                else
                {
                    return _APISuccess(new { profiles }, "Không tìm thấy kết quả phù hợp! Bạn có muốn tạo KH mới không?");
                }
            });
        }

        public ActionResult GetProfileDetails(Guid ProfileId, string CompanyCode, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                ProfileRepository repo = new ProfileRepository(_context);
                ProfileContactRepository contactRepository = new ProfileContactRepository(_context);

                bool error = false;
                var profileView = _unitOfWork.ProfileRepository.GetProfileDetails(ProfileId, CompanyCode, "Mode_1", out error);

                if (profileView != null)
                {
                    #region //Nhóm KH
                    if (profileView.profileGroupList != null && profileView.profileGroupList.Count > 0)
                    {
                        profileView.CustomerGroupCode = profileView.profileGroupList.Select(p => p.ProfileGroupCode)
                                                                                    .FirstOrDefault();
                    }
                    #endregion
                    //Url xem thông tin tổng quan của KH
                    profileView.GeneralInformationUrl = ConstDomain.Domain + "/Customer/Profile/Detail?Id=" + ProfileId + "&CompanyCode=" + CompanyCode;
                }
                return _APISuccess(profileView);

            });
        }

        public ActionResult GetAccountFormConfig(string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                List<ProfileConfigModel> configList = new List<ProfileConfigModel>();
                configList = _context.ProfileConfigModel.Where(p => p.ProfileCategoryCode == ConstCustomerType.Account).ToList();
                if (configList != null && configList.Count > 0)
                {
                    foreach (var item in configList)
                    {
                        if (string.IsNullOrEmpty(item.Note))
                        {
                            item.Note = PropertyHelper.GetDisplayNameByString<ProfileViewModel>(item.FieldCode);
                        }
                    }
                }
                return _APISuccess(configList);
            });
        }
        #endregion

        #region Search Contact
        public ActionResult SearchContact(Guid ProfileId, Guid? ProfileContactId, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                ProfileRepository repo = new ProfileRepository(_context);
                var profiles = repo.GetContactListOfProfile(ProfileId);

                if (ProfileContactId.HasValue)
                {
                    profiles = profiles.OrderByDescending(p => p.ProfileContactId == ProfileContactId).ToList();
                }

                return _APISuccess(profiles);
            });
        }
        #endregion Search Contact

        #region Create Contact
        public ActionResult CreateContact(Guid ProfileId, MobileProfileContactViewModel contact, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                #region Handle null value
                if (!string.IsNullOrEmpty(contact.ContactEmail) && contact.ContactEmail == "undefined")
                {
                    contact.ContactEmail = null;
                }
                if (!string.IsNullOrEmpty(contact.ContactPosition) && contact.ContactPosition == "undefined")
                {
                    contact.ContactPosition = null;
                }
                if (!string.IsNullOrEmpty(contact.ContactDepartment) && contact.ContactDepartment == "undefined")
                {
                    contact.ContactDepartment = null;
                }
                #endregion Handle null value

                var profile = _context.ProfileModel.Where(p => p.ProfileId == ProfileId).FirstOrDefault();
                var _repoPersonInCharge = new PersonInChargeRepository(_context);

                Guid? profileContactId = null;

                if (!string.IsNullOrEmpty(contact.ContactName))
                {
                    if (string.IsNullOrEmpty(contact.ContactPhone))
                    {
                        return _APIError("Vui lòng nhập SĐT liên hệ");
                    }
                    else
                    {
                        //Check SĐT liên hệ nhập sai định dạng: phải bắt đầu bằng số 0, >= 10 số, < 15 số
                        if (!contact.ContactPhone.StartsWith("0") || contact.ContactPhone.Length < 10 || contact.ContactPhone.Length >= 15)
                        {
                            return _APIError("SĐT liên hệ không đúng định dạng! (phải bắt đầu bằng số 0, >= 10 số, < 15 số)");
                        }
                    }
                    profileContactId = Guid.NewGuid();
                    contact.ProfileContactId = profileContactId;
                    ProfileModel profileContact = new ProfileModel();
                    profileContact.ProfileId = profileContactId.Value;
                    profileContact.CustomerTypeCode = ConstCustomerType.Contact;
                    profileContact.isForeignCustomer = profile.isForeignCustomer;
                    profileContact.ProfileName = contact.ContactName;
                    profileContact.AbbreviatedName = contact.ContactName?.ToAbbreviation();
                    profileContact.Phone = contact.ContactPhone;
                    profileContact.Email = contact.ContactEmail;
                    profileContact.Address = profile.Address;
                    profileContact.ProvinceId = profile.ProvinceId;
                    profileContact.DistrictId = profile.DistrictId;
                    profileContact.WardId = profile.WardId;
                    profileContact.Actived = true;
                    profileContact.CreateByEmployee = profile.CreateByEmployee;
                    profileContact.CreateAtCompany = profile.CreateAtCompany;
                    profileContact.CreateAtSaleOrg = profile.CreateAtSaleOrg;

                    profileContact.CreateBy = profile.CreateBy;
                    profileContact.CreateTime = DateTime.Now;
                    _context.Entry(profileContact).State = EntityState.Added;

                    //ProfileContact
                    ProfileContactAttributeModel contactAttAdd = new ProfileContactAttributeModel();
                    //1. GUID
                    contactAttAdd.ProfileId = profileContactId.Value;
                    //2. Công ty
                    contactAttAdd.CompanyId = profile.ProfileId;
                    //3. Chức vụ
                    contactAttAdd.Position = contact.ContactPosition;
                    //4. Liên hệ chính
                    contactAttAdd.IsMain = contact.IsMain;
                    //5. Phòng ban
                    contactAttAdd.DepartmentCode = contact.ContactDepartment;

                    _context.Entry(contactAttAdd).State = EntityState.Added;

                    //Nhân viên phụ trách
                    //List<PersonInChargeViewModel> personInChargeContactViewModel = new List<PersonInChargeViewModel>();
                    //personInChargeContactViewModel.Add(new PersonInChargeViewModel()
                    //{
                    //    SalesEmployeeCode = profile.CreateByEmployee,
                    //    ProfileId = profileContactId,
                    //    CreateBy = profile.CreateBy,
                    //    RoleCode = "EMPLOYEE_RES",
                    //});
                    //_repoPersonInCharge.CreateOrUpdate(personInChargeContactViewModel, profile.CreateAtCompany);
                }

                //Update main contact
                if (contact.IsMain == true)
                {
                    var contactList = _context.ProfileContactAttributeModel.Where(p => p.CompanyId == profile.ProfileId).ToList();
                    foreach (var item in contactList)
                    {
                        item.IsMain = false;
                        _context.Entry(item).State = EntityState.Modified;
                    }
                }
                _context.SaveChanges();

                //return contact list after add
                ProfileRepository repo = new ProfileRepository(_context);
                var contacts = repo.GetContactListOfProfile(ProfileId);

                return _APISuccess(new { ProfileContactId = profileContactId, contactList = contacts }, string.Format(LanguageResource.Alert_Create_Success, LanguageResource.Profile_Contact));
            });
        }
        #endregion Create Contact

        #region Delete Contact
        public ActionResult DeleteContact(Guid ProfileId, Guid ProfileContactId, string token, string key)
        {
            return ExecuteDelete(() =>
            {
                var profileContact = _context.ProfileModel.FirstOrDefault(p => p.ProfileId == ProfileContactId);
                if (profileContact != null)
                {
                    var contact = _context.ProfileContactAttributeModel.FirstOrDefault(p => p.ProfileId == ProfileContactId);
                    if (contact != null)
                    {
                        var personChargeList = _context.PersonInChargeModel.Where(p => p.ProfileId == ProfileContactId).ToList();
                        if (personChargeList != null && personChargeList.Count > 0)
                        {
                            foreach (var item in personChargeList)
                            {
                                _context.Entry(item).State = EntityState.Deleted;
                            }
                        }
                        var roleChargeList = _context.RoleInChargeModel.Where(p => p.ProfileId == ProfileContactId).ToList();
                        if (roleChargeList != null && roleChargeList.Count > 0)
                        {
                            foreach (var item in roleChargeList)
                            {
                                _context.Entry(item).State = EntityState.Deleted;
                            }
                        }
                        _context.Entry(contact).State = EntityState.Deleted;
                    }

                    _context.Entry(profileContact).State = EntityState.Deleted;
                    _context.SaveChanges();

                    //return contact list after delete
                    ProfileRepository repo = new ProfileRepository(_context);
                    var contacts = repo.GetContactListOfProfile(ProfileId);
                    return _APISuccess(new { contactList = contacts }, string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.Profile_Contact.ToLower()));
                }
                else
                {
                    return _APIError(string.Format(LanguageResource.Error_NotExist, LanguageResource.Profile_Contact.ToLower()));
                }
            });
        }
        #endregion Delete Contact

        #region Search Address
        public ActionResult SearchAddress(Guid ProfileId, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                AddressBookRepository _addressBookRepository = new AddressBookRepository(_context);
                var addressBookList = _addressBookRepository.GetAll(ProfileId);

                return _APISuccess(addressBookList);
            });
        }
        #endregion Search Address

        #region Create Address
        public ActionResult CreateAddress(MobileAddressBookViewModel viewModel, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                #region Handle null value
                if (!string.IsNullOrEmpty(viewModel.Note) && viewModel.Note == "undefined")
                {
                    viewModel.Note = null;
                }
                #endregion Handle null value

                AddressBookRepository _addressBookRepository = new AddressBookRepository(_context);

                //set value from view model to AddressBookModel
                AddressBookModel addressBook = new AddressBookModel();
                addressBook.ProfileId = viewModel.ProfileId;
                addressBook.CreateBy = viewModel.AccountId;
                addressBook.CreateTime = DateTime.Now;
                addressBook.AddressTypeCode = viewModel.AddressTypeCode;
                addressBook.CountryCode = viewModel.CountryCode;
                addressBook.ProvinceId = viewModel.ProvinceId;
                addressBook.DistrictId = viewModel.DistrictId;
                addressBook.WardId = viewModel.WardId;
                addressBook.Address = viewModel.Address;
                addressBook.Note = viewModel.Note;
                addressBook.isMain = viewModel.isMain;

                _addressBookRepository.Create(addressBook);
                _context.SaveChanges();

                //get address list after add
                var addressBookList = _addressBookRepository.GetAll(viewModel.ProfileId);

                return _APISuccess(new { addressBook.AddressBookId, addressBookList }, string.Format(LanguageResource.Alert_Create_Success, LanguageResource.AddressBook));
            });
        }
        #endregion Created Address

        #region Delete Address
        public ActionResult DeleteAddress(Guid ProfileId, Guid AddressBookId, string token, string key)
        {
            return ExecuteDelete(() =>
            {
                var addressBook = _context.AddressBookModel.FirstOrDefault(p => p.AddressBookId == AddressBookId);
                if (addressBook != null)
                {
                    _context.Entry(addressBook).State = EntityState.Deleted;
                    _context.SaveChanges();

                    //get address list after add
                    AddressBookRepository _addressBookRepository = new AddressBookRepository(_context);
                    var addressBookList = _addressBookRepository.GetAll(ProfileId);

                    return _APISuccess(new { addressBookList }, string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.AddressBook.ToLower()));
                }
                else
                {
                    return _APIError(string.Format(LanguageResource.Error_NotExist, LanguageResource.Profile_Contact.ToLower()));
                }
            });
        }
        #endregion Delete Address

        #region Create Profile
        public ActionResult CreateProfile(ProfileViewModel profile, MobileProfileContactViewModel contact, HttpPostedFileBase file, Guid? AddressBookId, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                string CustomerTypeCode = profile.CustomerTypeCode;
                if (string.IsNullOrEmpty(profile.CreateAtSaleOrg))
                {
                    return _APIError("Vui lòng chọn chi nhánh!");
                }
                if (CustomerTypeCode == ConstCustomerType.Bussiness)
                {
                    if (string.IsNullOrEmpty(profile.ContactName))
                    {
                        return _APIError("Vui lòng nhập tên người liên hệ!");
                    }
                    if (string.IsNullOrEmpty(profile.ContactPhone))
                    {
                        return _APIError("Vui lòng nhập SĐT liên hệ!");
                    }
                    if (string.IsNullOrEmpty(profile.CustomerGroupCode))
                    {
                        return _APIError("Vui lòng chọn nhóm khách hàng!");
                    }
                    if (string.IsNullOrEmpty(profile.CustomerCareerCode))
                    {
                        return _APIError("Vui lòng chọn ngành nghề!");
                    }
                }

                #region Handle null value

                if (profile.CreateBy == null)
                {
                    profile.CreateBy = _unitOfWork.AccountRepository.GetNameBy(profile.CreateByEmployee)?.AccountId;
                }
                if (string.IsNullOrEmpty(profile.CreateAtCompany) || profile.CreateAtCompany == "undefined")
                {
                    profile.CreateAtCompany = _unitOfWork.CompanyRepository.GetBy(profile.CreateAtSaleOrg)?.CompanyCode;
                }

                if (!string.IsNullOrEmpty(profile.CustomerGroupCode) && profile.CustomerGroupCode == "undefined")
                {
                    profile.CustomerGroupCode = null;
                }

                if (!string.IsNullOrEmpty(profile.TaxNo) && profile.TaxNo == "null")
                {
                    profile.TaxNo = null;
                }

                if (!string.IsNullOrEmpty(profile.Email) && profile.Email == "null")
                {
                    profile.Email = null;
                }

                if (!string.IsNullOrEmpty(contact.ContactEmail) && contact.ContactEmail == "null")
                {
                    contact.ContactEmail = null;
                }

                //if (!string.IsNullOrEmpty(profile.CompanyNumber) && profile.CompanyNumber == "null")
                //{
                //    profile.CompanyNumber = null;
                //}

                if (!string.IsNullOrEmpty(profile.Phone) && (profile.Phone == "null" || profile.Phone == "undefined"))
                {
                    profile.Phone = null;
                }

                if (!string.IsNullOrEmpty(profile.Website) && profile.Website == "null")
                {
                    profile.Website = null;
                }

                if (!string.IsNullOrEmpty(profile.ProfileShortName) && profile.ProfileShortName == "null")
                {
                    profile.ProfileShortName = null;
                }

                if (!string.IsNullOrEmpty(contact.ContactPosition) && contact.ContactPosition == "null")
                {
                    contact.ContactPosition = null;
                }

                if (!string.IsNullOrEmpty(contact.ContactDepartment) && contact.ContactDepartment == "null")
                {
                    contact.ContactDepartment = null;
                }

                if (!string.IsNullOrEmpty(profile.Note) && profile.Note == "null")
                {
                    profile.Note = null;
                }

                if (!string.IsNullOrEmpty(profile.Address) && (profile.Address == "null" || profile.Address == "undefined"))
                {
                    profile.Address = null;
                }
                #endregion Handle null value

                string customerClassCode = string.Empty;
                string responseContent = string.Empty;

                #region Profile
                //set value to save to database
                var _profileRepo = new ProfileRepository(_context);

                //Convert date
                //Ngày ghé thăm
                if (!string.IsNullOrEmpty(profile.VisitDate_String))
                {
                    profile.VisitDate = _unitOfWork.RepositoryLibrary.VNStringToDateTime(profile.VisitDate_String);
                }
                //Thời gian bắt đầu
                if (!string.IsNullOrEmpty(profile.StartDate_String))
                {
                    profile.StartDate = _unitOfWork.RepositoryLibrary.VNStringToDateTime(profile.StartDate_String);
                }
                if (!string.IsNullOrEmpty(profile.StartTime_String))
                {
                    var StartTime = _unitOfWork.RepositoryLibrary.VNStringToTimeSpan(profile.StartTime_String);
                    profile.StartDate = profile.StartDate.Value.Add(StartTime.Value);
                }

                //Thời gian kết thúc
                if (!string.IsNullOrEmpty(profile.EndDate_String))
                {
                    profile.EndDate = _unitOfWork.RepositoryLibrary.VNStringToDateTime(profile.EndDate_String);
                }
                if (!string.IsNullOrEmpty(profile.EndTime_String))
                {
                    var EndTime = _unitOfWork.RepositoryLibrary.VNStringToTimeSpan(profile.EndTime_String);
                    profile.EndDate = profile.EndDate.Value.Add(EndTime.Value);
                }

                //New profile
                if (profile.ProfileId == Guid.Empty)
                {
                    profile.ProfileId = Guid.NewGuid();
                    customerClassCode = ConstCustomerClassCode.New;
                    //Check MST
                    if (!string.IsNullOrEmpty(profile.TaxNo))
                    {
                        var existsTaxNo = _context.ProfileModel.Where(p => p.CustomerTypeCode == ConstProfileType.Account && p.TaxNo == profile.TaxNo).FirstOrDefault();
                        if (existsTaxNo != null)
                        {
                            return _APIError("Mã số thuế đã tồn tại. Vui lòng kiểm tra lại.");
                        }
                    }
                    //ImageUrl
                    if (file != null)
                    {
                        profile.ImageUrl = UploadDocumentFile(file, "Profile", ConstDomain.DocumentDomain, FileType: ConstFileAttachmentCode.Image);
                    }

                    profile.CreateTime = DateTime.Now;
                    profile.Actived = true;
                    //ProfileName
                    //AbbreviatedName
                    if (!string.IsNullOrEmpty(profile.ProfileName))
                    {
                        var toLowerOtherChar = true;
                        if (profile.CustomerTypeCode == ConstCustomerType.Bussiness)
                        {
                            toLowerOtherChar = false;
                        }
                        profile.AutoformatFullName = profile.AutoformatFullName ?? true;
                        if (profile.AutoformatFullName == true)
                        {
                            profile.ProfileName = profile.ProfileName.FirstCharToUpper(toLowerOtherChar);
                        }
                        else
                        {
                            profile.ProfileName = profile.ProfileName;
                        }
                        profile.AbbreviatedName = profile.ProfileName.ToAbbreviation();
                    }
                    //Address
                    if (!string.IsNullOrEmpty(profile.Address))
                    {
                        profile.Address = profile.Address.FirstCharToUpper();
                    }
                    //Position
                    profile.PositionB = profile.Position;
                    //CustomerTypeCode
                    if (profile.CustomerTypeCode == ConstCustomerType.Bussiness)
                    {
                        profile.Title = ConstTitle.Company;
                    }
                    else if (profile.CustomerTypeCode == ConstCustomerType.Customer)
                    {
                        profile.CustomerGroupCode = "23";
                    }
                    //Phone
                    //Customer
                    if (profile.CustomerTypeCode == ConstCustomerType.Customer)
                    {
                        if (string.IsNullOrEmpty(profile.Phone))
                        {
                            return _APIError("Vui lòng nhập SĐT liên hệ");
                        }
                    }

                    if (!string.IsNullOrEmpty(profile.Phone))
                    {
                        //Check SĐT liên hệ nhập sai định dạng: phải bắt đầu bằng số 0, >= 10 số, < 15 số
                        if (!profile.Phone.StartsWith("0") || profile.Phone.Length < 10 || profile.Phone.Length >= 15)
                        {
                            return _APIError("SĐT liên hệ không đúng định dạng! (phải bắt đầu bằng số 0, >= 10 số, < 15 số)");
                        }
                        //profile.CompanyNumber = profile.Phone;
                    }


                    //Profile Career
                    if (profile.CustomerTypeCode == ConstCustomerType.Bussiness)
                    {
                        _unitOfWork.ProfileCareerRepository.CreateOrUpdate(profile.ProfileId, profile.CreateAtCompany, profile.CustomerCareerCode, profile.CreateBy);
                    }

                    //Profile Type
                    profile.CustomerTypeCode = ConstCustomerType.Account;

                    //Profile
                    _profileRepo.Create(profile);

                    //ProfileBAttribute
                    _profileRepo.CreateProfileB(profile);

                    //PersonInCharge
                    var _repoPersonInCharge = new PersonInChargeRepository(_context);
                    List<PersonInChargeViewModel> personInChargeViewModel = new List<PersonInChargeViewModel>();
                    personInChargeViewModel.Add(new PersonInChargeViewModel()
                    {
                        SalesEmployeeCode = profile.CreateByEmployee,
                        ProfileId = profile.ProfileId,
                        CreateBy = profile.CreateBy,
                        RoleCode = "EMPLOYEE_RES",
                        CompanyCode = profile.CreateAtCompany,
                    });
                    _repoPersonInCharge.CreateOrUpdate(personInChargeViewModel, profile.CreateAtCompany);

                    #region ProfileGroup
                    if (!string.IsNullOrEmpty(profile.CustomerGroupCode))
                    {
                        ProfileGroupModel profileGroup = new ProfileGroupModel();
                        profileGroup.ProfileGroupId = Guid.NewGuid();
                        profileGroup.ProfileId = profile.ProfileId;
                        profileGroup.CompanyCode = profile.CreateAtCompany;
                        profileGroup.ProfileGroupCode = profile.CustomerGroupCode;
                        profileGroup.CreateBy = profile.CreateBy;
                        profileGroup.CreateTime = DateTime.Now;

                        _context.Entry(profileGroup).State = EntityState.Added;
                    }
                    #endregion ProfileGroup

                    #region ProfileType
                    if (!string.IsNullOrEmpty(CustomerTypeCode))
                    {
                        ProfileTypeModel profileType = new ProfileTypeModel();
                        profileType.ProfileTypeId = Guid.NewGuid();
                        profileType.ProfileId = profile.ProfileId;
                        profileType.CompanyCode = profile.CreateAtCompany;
                        profileType.CustomerTypeCode = CustomerTypeCode;
                        profileType.CreateBy = profile.CreateBy;
                        profileType.CreateTime = DateTime.Now;

                        _context.Entry(profileType).State = EntityState.Added;
                    }
                    #endregion ProfileType

                    responseContent = LanguageResource.Mobile_CreateProfile_Successfully;
                }
                else
                {

                    customerClassCode = ConstCustomerClassCode.Old;
                    var existProfile = _context.ProfileModel.Where(p => p.ProfileId == profile.ProfileId).FirstOrDefault();
                    //Tự động format theo mẫu
                    existProfile.AutoformatFullName = profile.AutoformatFullName ?? true;

                    //Check trùng MST: nếu không có mã SAP mới check trùng MST
                    if (!string.IsNullOrEmpty(existProfile.TaxNo) && string.IsNullOrEmpty(existProfile.ProfileForeignCode))
                    {
                        var existsTaxNo = _context.ProfileModel.Where(p => p.CustomerTypeCode == ConstProfileType.Account && p.TaxNo == profile.TaxNo && p.ProfileId != existProfile.ProfileId && p.Actived == true).FirstOrDefault();
                        if (existsTaxNo != null)
                        {
                            return _APIError("Mã số thuế đã tồn tại. Vui lòng kiểm tra lại.");
                        }
                    }
                    /*
                    Khi chọn đối tượng KH => cập nhật field CustomerAccountGroupCode (Phân nhóm khách hàng).
                            Z002	KH trong nước
                            Z003	KH nước ngoài
                    */
                    if (existProfile.isForeignCustomer == true)
                    {
                        existProfile.CustomerAccountGroupCode = "Z003";
                    }
                    else
                    {
                        existProfile.CustomerAccountGroupCode = "Z002";
                    }
                    existProfile.LastEditTime = DateTime.Now;
                    existProfile.LastEditBy = profile.CreateBy;

                    //Nếu có truyền AddresBookId thì chuyển địa chỉ chính qua AddressBookModel
                    if (AddressBookId.HasValue && AddressBookId != Guid.Empty)
                    {
                        // lấy Address hiện tại trong ProfileModel lưu vào AddressBookModel
                        AddressBookModel addressBook = new AddressBookModel();
                        addressBook.AddressBookId = Guid.NewGuid();
                        //addressBook.AddressTypeCode = ConstAddressType.GH;
                        addressBook.AddressTypeCode = existProfile.AddressTypeCode;
                        addressBook.Address = existProfile.Address;
                        addressBook.ProfileId = profile.ProfileId;
                        addressBook.ProvinceId = existProfile.ProvinceId;
                        addressBook.DistrictId = existProfile.DistrictId;
                        addressBook.WardId = existProfile.WardId;
                        addressBook.CountryCode = existProfile.CountryCode;
                        addressBook.isMain = false;
                        addressBook.CreateBy = profile.CreateBy;
                        addressBook.CreateTime = DateTime.Now;
                        _context.Entry(addressBook).State = EntityState.Added;

                        var existAddress = _context.AddressBookModel.FirstOrDefault(p => p.AddressBookId == AddressBookId);
                        if (existAddress != null)
                        {
                            _context.AddressBookModel.Remove(existAddress);
                        }
                    }

                    //Update visit date
                    existProfile.VisitDate = profile.VisitDate;
                    //Loại địa chỉ
                    existProfile.AddressTypeCode = profile.AddressTypeCode;

                    //PersonInCharge
                    /*
                    var _repoPersonInCharge = new PersonInChargeRepository(_context);
                    List<PersonInChargeViewModel> personInChargeViewModel = new List<PersonInChargeViewModel>();
                    personInChargeViewModel.Add(new PersonInChargeViewModel()
                    {
                        SalesEmployeeCode = profile.CreateByEmployee,
                        ProfileId = profile.ProfileId,
                        CreateBy = profile.CreateBy,
                        RoleCode = "EMPLOYEE_RES",
                        CompanyCode = profile.CreateAtCompany,
                    });
                    //Check exist personInCharge 
                    //If exist but different from current user => add list
                    var existPersonInChargeList = _context.PersonInChargeModel
                                                          .Where(p => p.ProfileId == existProfile.ProfileId && p.CompanyCode == profile.CreateAtCompany && p.SalesEmployeeCode != profile.CreateByEmployee)
                                                          .Select(p => new PersonInChargeViewModel()
                                                          {
                                                              ProfileId = p.ProfileId,
                                                              SalesEmployeeCode = p.SalesEmployeeCode,
                                                              CreateBy = p.CreateBy,
                                                              CreateTime = p.CreateTime,
                                                              RoleCode = p.RoleCode,
                                                              CompanyCode = p.CompanyCode,
                                                          }).ToList();
                    if (existPersonInChargeList != null)
                    {
                        personInChargeViewModel.AddRange(existPersonInChargeList);
                    }
                    _repoPersonInCharge.CreateOrUpdate(personInChargeViewModel, profile.CreateAtCompany);
                    */

                    //Nếu là KH Cũ, update thêm các field: 
                    //A: Không cho sửa: Đối tượng, Phân loại khách hàng => Update: Nếu khác công ty thì có thể cập nhật (Extens)
                    //B: Khách cá nhân update các field
                    //2.1 Danh xưng
                    //2.2 Tên khách hàng
                    //2.3 Tên ngắn
                    //2.4 Độ tuổi
                    //2.5 Số điện thoại(Làm sau)
                    //2.6 Email
                    //2.7 Khu vực
                    //2.8 Tỉnh thành
                    //2.9 Quận Huyện
                    //2.10 Phường Xã
                    //2.11 Địa chỉ
                    //2.12 Ghi chú
                    //2.13 Hình ảnh(nếu có)
                    var existCustomerType = _context.ProfileTypeModel.Where(p => p.ProfileId == profile.ProfileId && p.CompanyCode == profile.CreateAtCompany).FirstOrDefault();
                    if (existCustomerType != null)
                    {
                        CustomerTypeCode = existCustomerType.CustomerTypeCode;
                    }
                    //Update Customer
                    if (CustomerTypeCode == ConstCustomerType.Customer)
                    {
                        //2.1 Danh xưng
                        existProfile.Title = profile.Title;
                        //2.2 Tên khách hàng
                        if (profile.AutoformatFullName == true)
                        {
                            existProfile.ProfileName = profile.ProfileName.FirstCharToUpper();
                        }
                        else
                        {
                            existProfile.ProfileName = profile.ProfileName;
                        }
                        //2.3 Tên ngắn
                        existProfile.ProfileShortName = profile.ProfileShortName;
                        //2.4 Độ tuổi
                        existProfile.Age = profile.Age;
                        //2.5 Số điện thoại(Làm sau)
                        existProfile.Phone = profile.Phone.Trim().Replace(" ", "");
                        if (string.IsNullOrEmpty(existProfile.Phone))
                        {
                            return _APIError("Vui lòng nhập SĐT liên hệ");
                        }
                        else
                        {
                            //Check SĐT liên hệ nhập sai định dạng: phải bắt đầu bằng số 0, >= 10 số, < 15 số
                            if (!existProfile.Phone.StartsWith("0") || existProfile.Phone.Length < 10 || existProfile.Phone.Length >= 15)
                            {
                                return _APIError("SĐT liên hệ không đúng định dạng! (phải bắt đầu bằng số 0, >= 10 số, < 15 số)", new { FieldName = "Phone" });
                            }
                        }
                        //2.6 Email
                        existProfile.Email = profile.Email;
                        //2.7 Khu vực
                        existProfile.SaleOfficeCode = profile.SaleOfficeCode;
                        //2.8 Tỉnh thành
                        existProfile.ProvinceId = profile.ProvinceId;
                        //2.9 Quận Huyện
                        existProfile.DistrictId = profile.DistrictId;
                        //2.10 Phường Xã
                        existProfile.WardId = profile.WardId;
                        //2.11 Địa chỉ
                        existProfile.Address = profile.Address;
                        //2.12 Ghi chú
                        existProfile.Note = profile.Note;

                    }

                    //2.1 Tên doanh nghiệp
                    //2.2 Tên ngắn
                    //2.3 Mã số thuế
                    //2.4 Số điện thoại(Làm sau)
                    //2.5 Email
                    //2.6 Website
                    //2.7 Khu vực
                    //2.8 Tỉnh thành
                    //2.9 Quận Huyện
                    //2.10 Phường Xã
                    //2.11 Địa chỉ
                    //2.12 Ghi chú
                    //2.13 Hình ảnh(nếu có)
                    //2.14 Liên hệ
                    //2.15 Chức vụ
                    //2.16 Nhóm khách hàng
                    //2.17 Ngành nghề khách hàng
                    //2.18 Số điện thoại công ty

                    //Update Business
                    else if (CustomerTypeCode == ConstCustomerType.Bussiness)
                    {
                        //2.1 Tên doanh nghiệp
                        if (profile.AutoformatFullName == true)
                        {
                            existProfile.ProfileName = profile.ProfileName.FirstCharToUpper(false);
                        }
                        else
                        {
                            existProfile.ProfileName = profile.ProfileName;
                        }
                        //2.2 Tên ngắn
                        existProfile.ProfileShortName = profile.ProfileShortName;
                        //2.3 Mã số thuế
                        existProfile.TaxNo = profile.TaxNo;
                        //2.4 Số điện thoại(Làm sau)
                        existProfile.Phone = profile.Phone;
                        if (!string.IsNullOrEmpty(existProfile.Phone))
                        {
                            //Check SĐT liên hệ nhập sai định dạng: phải bắt đầu bằng số 0, >= 10 số, < 15 số
                            if (!existProfile.Phone.StartsWith("0") || existProfile.Phone.Length < 10 || existProfile.Phone.Length >= 15)
                            {
                                return _APIError("SĐT công ty không đúng định dạng! (phải bắt đầu bằng số 0, >= 10 số, < 15 số)", new { FieldName = "Phone" });
                            }
                        }
                        //existProfile.CompanyNumber = profile.Phone;

                        //2.5 Email
                        existProfile.Email = profile.Email;
                        //2.6 Website
                        existProfile.Website = profile.Website;
                        //2.7 Khu vực
                        existProfile.SaleOfficeCode = profile.SaleOfficeCode;
                        //2.8 Tỉnh thành
                        existProfile.ProvinceId = profile.ProvinceId;
                        //2.9 Quận Huyện
                        existProfile.DistrictId = profile.DistrictId;
                        //2.10 Phường Xã
                        existProfile.WardId = profile.WardId;
                        //2.11 Địa chỉ
                        existProfile.Address = profile.Address;
                        //2.12 Ghi chú
                        existProfile.Note = profile.Note;
                        //2.13 Hình ảnh(nếu có)
                        //2.17 Ngành nghề khách hàng
                        existProfile.CustomerCareerCode = profile.CustomerCareerCode;
                        _unitOfWork.ProfileCareerRepository.CreateOrUpdate(profile.ProfileId, profile.CreateAtCompany, profile.CustomerCareerCode, profile.CreateBy);

                        #region ProfileGroup
                        if (!string.IsNullOrEmpty(profile.CustomerGroupCode))
                        {
                            var companyCode = _unitOfWork.CompanyRepository.GetBy(profile.CreateAtSaleOrg)?.CompanyCode;
                            var existProfileGroup = _context.ProfileGroupModel.Where(p => p.ProfileId == profile.ProfileId && p.CompanyCode == companyCode && p.ProfileGroupCode == profile.CustomerGroupCode).FirstOrDefault();
                            if (existProfileGroup == null)
                            {
                                ProfileGroupModel profileGroup = new ProfileGroupModel();
                                profileGroup.ProfileGroupId = Guid.NewGuid();
                                profileGroup.ProfileId = profile.ProfileId;
                                profileGroup.CompanyCode = companyCode;
                                profileGroup.ProfileGroupCode = profile.CustomerGroupCode;
                                profileGroup.CreateBy = profile.CreateBy;
                                profileGroup.CreateTime = DateTime.Now;

                                _context.Entry(profileGroup).State = EntityState.Added;
                            }

                        }
                        #endregion ProfileGroup
                    }

                    //Contact
                    #region Contact
                    //if (!string.IsNullOrEmpty(profile.ContactName))
                    //{
                    //    var profileContactId = Guid.NewGuid();
                    //    var existContact = (from p in _context.ProfileModel
                    //                        join c in _context.ProfileContactAttributeModel on p.ProfileId equals c.CompanyId
                    //                        join pr in _context.ProfileModel on c.ProfileId equals pr.ProfileId
                    //                        select pr).FirstOrDefault();


                    //    if (existContact == null)
                    //    {
                    //        ProfileModel profileContact = new ProfileModel();
                    //        profileContact.ProfileId = profileContactId;
                    //        profileContact.ProfileName = profile.ContactName;
                    //        profileContact.AbbreviatedName = profile.ProfileName.ToAbbreviation();
                    //        profileContact.CustomerTypeCode = ConstCustomerType.Contact;
                    //        profileContact.isForeignCustomer = false;
                    //        profileContact.Phone = profile.Phone;
                    //        if (string.IsNullOrEmpty(profileContact.Phone))
                    //        {
                    //            return _APIError("Vui lòng nhập SĐT liên hệ");
                    //        }
                    //        else
                    //        {
                    //            //Check SĐT liên hệ nhập sai định dạng: phải bắt đầu bằng số 0, >= 10 số, < 15 số
                    //            if (!profileContact.Phone.StartsWith("0") || profileContact.Phone.Length < 10 || profileContact.Phone.Length >= 15)
                    //            {
                    //                return _APIError("SĐT liên hệ không đúng định dạng! (phải bắt đầu bằng số 0, >= 10 số, < 15 số)");
                    //            }
                    //        }
                    //        profileContact.Email = profile.EmailBusiness;
                    //        profileContact.Address = profile.Address;
                    //        profileContact.ProvinceId = profile.ProvinceId;
                    //        profileContact.DistrictId = profile.DistrictId;
                    //        profileContact.WardId = profile.WardId;
                    //        profileContact.Actived = true;
                    //        profileContact.CreateByEmployee = profile.CreateByEmployee;
                    //        profileContact.CreateAtCompany = profile.CreateAtCompany;
                    //        profileContact.CreateAtSaleOrg = profile.CreateAtSaleOrg;

                    //        profileContact.CreateBy = profile.CreateBy;
                    //        profileContact.CreateTime = DateTime.Now;
                    //        _context.Entry(profileContact).State = EntityState.Added;

                    //        //ProfileContact
                    //        ProfileContactAttributeModel contactAttAdd = new ProfileContactAttributeModel();
                    //        //1. GUID
                    //        contactAttAdd.ProfileId = profileContactId;
                    //        //2. Công ty
                    //        contactAttAdd.CompanyId = profile.ProfileId;
                    //        //3. Chức vụ
                    //        contactAttAdd.Position = profile.ProfileContactPosition;
                    //        //4. Liên hệ chính
                    //        contactAttAdd.IsMain = true;
                    //        //5. Phòng ban
                    //        contactAttAdd.DepartmentCode = profile.DepartmentCode;

                    //        _context.Entry(contactAttAdd).State = EntityState.Added;
                    //    }
                    //    else
                    //    {
                    //        profileContactId = existContact.ProfileId;

                    //        existContact.ProfileName = profile.ContactName;
                    //        existContact.AbbreviatedName = profile.ProfileName.ToAbbreviation();
                    //        existContact.CustomerTypeCode = ConstCustomerType.Contact;
                    //        existContact.isForeignCustomer = false;
                    //        existContact.Phone = profile.Phone;
                    //        if (string.IsNullOrEmpty(existContact.Phone))
                    //        {
                    //            return _APIError("Vui lòng nhập SĐT liên hệ");
                    //        }
                    //        else
                    //        {
                    //            //Check SĐT liên hệ nhập sai định dạng: phải bắt đầu bằng số 0, >= 10 số, < 15 số
                    //            if (!existContact.Phone.StartsWith("0") || existContact.Phone.Length < 10 || existContact.Phone.Length >= 15)
                    //            {
                    //                return _APIError("SĐT liên hệ không đúng định dạng! (phải bắt đầu bằng số 0, >= 10 số, < 15 số)");
                    //            }
                    //        }
                    //        existContact.Email = profile.EmailBusiness;
                    //        existContact.Address = profile.Address;
                    //        existContact.ProvinceId = profile.ProvinceId;
                    //        existContact.DistrictId = profile.DistrictId;
                    //        existContact.WardId = profile.WardId;

                    //        existContact.CreateBy = profile.LastEditBy;
                    //        existContact.CreateTime = DateTime.Now;
                    //        _context.Entry(existContact).State = EntityState.Modified;

                    //        var existContactAttr = _context.ProfileContactAttributeModel.FirstOrDefault(p => p.ProfileId == profileContactId);
                    //        if (existContactAttr == null)
                    //        {
                    //            //ProfileContact
                    //            ProfileContactAttributeModel contactAttAdd = new ProfileContactAttributeModel();
                    //            //1. GUID
                    //            contactAttAdd.ProfileId = profileContactId;
                    //            //2. Công ty
                    //            contactAttAdd.CompanyId = profile.ProfileId;
                    //            //3. Chức vụ
                    //            contactAttAdd.Position = profile.ProfileContactPosition;
                    //            //4. Liên hệ chính
                    //            contactAttAdd.IsMain = true;
                    //            //5. Phòng ban
                    //            contactAttAdd.DepartmentCode = profile.DepartmentCode;

                    //            _context.Entry(contactAttAdd).State = EntityState.Added;
                    //        }
                    //        else
                    //        {
                    //            //3. Chức vụ
                    //            existContactAttr.Position = profile.ProfileContactPosition;
                    //            //4. Liên hệ chính
                    //            existContactAttr.IsMain = true;
                    //            //5. Phòng ban
                    //            existContactAttr.DepartmentCode = profile.DepartmentCode;
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    return _APIError("Vui lòng nhập Họ và tên người liên hệ");
                    //}
                    #endregion

                    #region ProfileType
                    var existProfileType = _context.ProfileTypeModel.Where(p => p.ProfileId == profile.ProfileId && p.CompanyCode == profile.CreateAtCompany).FirstOrDefault();
                    if (existProfileType == null)
                    {
                        ProfileTypeModel profileType = new ProfileTypeModel();
                        profileType.ProfileTypeId = Guid.NewGuid();
                        profileType.ProfileId = profile.ProfileId;
                        profileType.CompanyCode = profile.CreateAtCompany;
                        profileType.CustomerTypeCode = CustomerTypeCode;
                        profileType.CreateBy = profile.CreateBy;
                        profileType.CreateTime = DateTime.Now;

                        _context.Entry(profileType).State = EntityState.Added;
                    }
                    #endregion ProfileType

                    //Yêu cầu tạo khách ở ECC
                    if (existProfile.isCreateRequest.HasValue)
                    {
                        if (existProfile.isCreateRequest == null && profile.isCreateRequest == true)
                        {
                            existProfile.CreateRequestTime = existProfile.LastEditTime;
                        }
                    }
                    else
                    {
                        existProfile.CreateRequestTime = null;
                    }
                    existProfile.isCreateRequest = profile.isCreateRequest;

                    _context.Entry(existProfile).State = EntityState.Modified;

                    responseContent = LanguageResource.Mobile_UpdateProfile_Successfully;
                }
                #endregion Profile

                //Contact
                #region Contact
                if (CustomerTypeCode == ConstCustomerType.Bussiness)
                {
                    var _repoPersonInCharge = new PersonInChargeRepository(_context);
                    if (contact.ProfileContactId != null)
                    {
                        //profile
                        var profileContactDB = _context.ProfileModel.Where(p => p.ProfileId == contact.ProfileContactId).FirstOrDefault();
                        if (profileContactDB != null)
                        {
                            profileContactDB.ProfileName = contact.ContactName;
                            profileContactDB.Phone = contact.ContactPhone;
                            profileContactDB.Email = contact.ContactEmail;
                        }
                        _context.Entry(profileContactDB).State = EntityState.Modified;

                        //contact attribue
                        var profileContactAttributeDB = _context.ProfileContactAttributeModel.Where(p => p.ProfileId == contact.ProfileContactId).FirstOrDefault();
                        if (profileContactAttributeDB != null)
                        {
                            profileContactAttributeDB.Position = contact.ContactPosition;
                            profileContactAttributeDB.DepartmentCode = contact.ContactDepartment;
                        }
                        _context.Entry(profileContactAttributeDB).State = EntityState.Modified;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(contact.ContactName))
                        {
                            if (string.IsNullOrEmpty(contact.ContactPhone))
                            {
                                return _APIError("Vui lòng nhập SĐT liên hệ");
                            }
                            else
                            {
                                //Check SĐT liên hệ nhập sai định dạng: phải bắt đầu bằng số 0, >= 10 số, < 15 số
                                if (!contact.ContactPhone.StartsWith("0") || contact.ContactPhone.Length < 10 || contact.ContactPhone.Length >= 15)
                                {
                                    return _APIError("SĐT liên hệ không đúng định dạng! (phải bắt đầu bằng số 0, >= 10 số, < 15 số)", new { FieldName = "ContactPhone" });
                                }
                            }

                            var profileContactId = Guid.NewGuid();
                            contact.ProfileContactId = profileContactId;
                            ProfileModel profileContact = new ProfileModel();
                            profileContact.ProfileId = profileContactId;
                            profileContact.CustomerTypeCode = ConstCustomerType.Contact;
                            profileContact.isForeignCustomer = profile.isForeignCustomer;
                            profileContact.ProfileName = contact.ContactName;
                            profileContact.AbbreviatedName = contact.ContactName?.ToAbbreviation();
                            profileContact.Phone = contact.ContactPhone;
                            profileContact.Email = contact.ContactEmail;
                            profileContact.Address = profile.Address;
                            profileContact.ProvinceId = profile.ProvinceId;
                            profileContact.DistrictId = profile.DistrictId;
                            profileContact.WardId = profile.WardId;
                            profileContact.Actived = true;
                            profileContact.CreateByEmployee = profile.CreateByEmployee;
                            profileContact.CreateAtCompany = profile.CreateAtCompany;
                            profileContact.CreateAtSaleOrg = profile.CreateAtSaleOrg;

                            profileContact.CreateBy = profile.CreateBy;
                            profileContact.CreateTime = DateTime.Now;
                            _context.Entry(profileContact).State = EntityState.Added;

                            //ProfileContact
                            ProfileContactAttributeModel contactAttAdd = new ProfileContactAttributeModel();
                            //1. GUID
                            contactAttAdd.ProfileId = profileContactId;
                            //2. Công ty
                            contactAttAdd.CompanyId = profile.ProfileId;
                            //3. Chức vụ
                            contactAttAdd.Position = contact.ContactPosition;
                            //4. Liên hệ chính
                            contactAttAdd.IsMain = true;
                            //5. Phòng ban
                            contactAttAdd.DepartmentCode = contact.ContactDepartment;

                            _context.Entry(contactAttAdd).State = EntityState.Added;

                            //Nhân viên phụ trách
                            //List<PersonInChargeViewModel> personInChargeContactViewModel = new List<PersonInChargeViewModel>();
                            //personInChargeContactViewModel.Add(new PersonInChargeViewModel()
                            //{
                            //    SalesEmployeeCode = profile.CreateByEmployee,
                            //    ProfileId = profileContactId,
                            //    CreateBy = profile.CreateBy,
                            //    RoleCode = "EMPLOYEE_RES",
                            //});
                            //_repoPersonInCharge.CreateOrUpdate(personInChargeContactViewModel, profile.CreateAtCompany);
                        }
                    }

                }
                #endregion

                #region Nếu user chọn "Có ghé thăm" thì mới lưu dữ liệu ghé thăm
                if (profile.IsHasAppointment == true)
                {
                    //Task
                    #region Task
                    var _taskRepository = new TaskRepository(_context);
                    var _taskStatusRepository = new TaskStatusRepository(_context);
                    var _storeRepository = new StoreRepository(_context);

                    //TaskViewModel taskViewModel = new TaskViewModel();
                    //taskViewModel.TaskId = Guid.NewGuid();
                    var _appointmentRepository = new AppointmentRepository(_context);
                    AppointmentViewModel appointmentViewModel = new AppointmentViewModel();

                    //Channel Code
                    appointmentViewModel.ChannelCode = profile.ChannelCode;

                    //get workflow id
                    var workflow = _context.WorkFlowModel.Where(p => p.WorkFlowCode == ConstWorkFlow.GT).FirstOrDefault();
                    appointmentViewModel.WorkFlowId = workflow.WorkFlowId;
                    var taskStatus = _taskStatusRepository.GetTaskStatusByWorkFlow(appointmentViewModel.WorkFlowId);
                    if (taskStatus != null)
                    {
                        appointmentViewModel.TaskStatusId = profile.TaskStatusId.GetValueOrDefault();
                    }

                    //get company id
                    var store = _context.StoreModel.Where(p => p.SaleOrgCode == profile.CreateAtSaleOrg).FirstOrDefault();
                    if (store != null)
                    {
                        appointmentViewModel.StoreId = store.StoreId;
                        appointmentViewModel.CompanyId = _storeRepository.GetCompanyIdByStoreId(store.StoreId);
                    }
                    appointmentViewModel.ProfileId = profile.ProfileId;

                    //auto generate summary if task status code = GT
                    //if (string.IsNullOrEmpty(profile.Summary))
                    //{
                    //    var CreateBy = string.Empty;
                    //    CreateBy = _unitOfWork.AccountRepository.GetNameBy(profile.CreateBy);

                    //    profile.Summary = string.Format("{0}-{1}-{2}", CreateBy, profile.VisitDate.GetValueOrDefault().ToString("dd/MM"), profile.ProfileName);
                    //}
                    appointmentViewModel.Description = profile.Note;
                    appointmentViewModel.Requirement = profile.Summary;
                    if (!string.IsNullOrEmpty(profile.Summary))
                    {
                        appointmentViewModel.Description += " - " + profile.Summary;
                    }

                    var CreateBy = string.Empty;
                    CreateBy = _unitOfWork.AccountRepository.GetNameBy(profile.CreateBy);
                    //profile.Summary = string.Format("{0}-{1}-{2}", CreateBy, profile.VisitDate.GetValueOrDefault().ToString("dd/MM"), profile.ProfileName);
                    profile.Summary = _unitOfWork.TaskRepository.GetSummary(ProfileId: profile.ProfileId, EmployeeName: CreateBy, ProfileName: !string.IsNullOrEmpty(profile.ProfileShortName) ? profile.ProfileShortName : profile.ProfileName);

                    appointmentViewModel.Summary = profile.Summary;
                    appointmentViewModel.PriorityCode = ConstPriotityCode.NORMAL;
                    appointmentViewModel.CreateBy = profile.CreateBy;
                    appointmentViewModel.CreateTime = DateTime.Now;
                    appointmentViewModel.Actived = true;
                    appointmentViewModel.Reporter = profile.CreateByEmployee;
                    //_taskRepository.Create(taskViewModel);
                    //Thời gian bắt đầu
                    appointmentViewModel.StartDate = profile.StartDate;
                    //Thời gian kết thúc
                    appointmentViewModel.EndDate = profile.EndDate;
                    #endregion Task

                    //Appointment
                    #region Appointment

                    appointmentViewModel.AppointmentId = appointmentViewModel.TaskId;
                    appointmentViewModel.CustomerClassCode = customerClassCode;
                    appointmentViewModel.CategoryCode = profile.CategoryCode;
                    appointmentViewModel.ShowroomCode = profile.CustomerSourceCode;
                    appointmentViewModel.SaleEmployeeCode = profile.CreateByEmployee;
                    appointmentViewModel.VisitDate = profile.VisitDate;
                    appointmentViewModel.ReceiveDate = profile.VisitDate;
                    appointmentViewModel.PrimaryContactId = contact.ProfileContactId;
                    //Đề xuất
                    appointmentViewModel.SaleEmployeeOffer = profile.SaleEmployeeOffer;
                    //Ý kiến/phản hồi của KH + Đánh giá
                    appointmentViewModel.Reviews = profile.Reviews;
                    appointmentViewModel.Ratings = profile.Ratings;
                    //Có ghé thăm Cabinet Pro
                    appointmentViewModel.isVisitCabinetPro = profile.isVisitCabinetPro;
                    appointmentViewModel.VisitCabinetProRequest = profile.VisitCabinetProRequest;

                    _appointmentRepository.Create(appointmentViewModel);
                    #endregion Appointment

                    //Delivery - Xuất kho
                    #region Delivery
                    if (profile.CatalogList != null && profile.CatalogList.Count > 0)
                    {
                        DeliveryModel delivery = new DeliveryModel();
                        delivery.DeliveryId = Guid.NewGuid();
                        delivery.DocumentDate = profile.VisitDate;
                        delivery.CompanyId = appointmentViewModel.CompanyId;
                        delivery.StoreId = appointmentViewModel.StoreId;
                        delivery.SalesEmployeeCode = profile.CreateByEmployee;
                        delivery.ProfileId = profile.ProfileId;
                        delivery.Note = profile.Note;
                        delivery.CreateBy = profile.CreateBy;
                        delivery.CreateTime = DateTime.Now;
                        delivery.isDeleted = false;

                        #region Thông tin xuất caltalog
                        var district = _unitOfWork.DistrictRepository.Find(profile.DistrictId);
                        if (district == null)
                        {
                            district = new DistrictViewModel();
                        }
                        var province = _unitOfWork.ProvinceRepository.Find(profile.ProvinceId);
                        if (province == null)
                        {
                            province = new ProvinceViewModel();
                        }
                        //Người nhận (Recipient)
                        if (profile.CustomerTypeCode == ConstCustomerType.Bussiness)
                        {
                            delivery.RecipientCompany = profile.ProfileName;
                            delivery.RecipientName = profile.ContactName;
                        }
                        else
                        {
                            delivery.RecipientName = profile.ProfileName;
                        }
                        delivery.RecipientPhone = profile.Phone;
                        delivery.RecipientAddress = string.Format("{0}, {1}, {2}", profile.Address, district.DistrictName, province.ProvinceName);
                        //Người gửi (Sender)
                        var employee = _unitOfWork.SalesEmployeeRepository.Find(profile.CreateBy);
                        delivery.SenderName = employee.SalesEmployeeName;
                        delivery.SenderPhone = employee.Phone;
                        #endregion

                        _context.Entry(delivery).State = EntityState.Added;

                        //Save detail
                        foreach (var catalog in profile.CatalogList)
                        {
                            //Check tồn kho nếu số lượng tồn kho đáp ứng đủ nhu cầu thì mới lưu
                            var onHand = _unitOfWork.StockRepository.GetStockOnHandBy(profile.CreateAtSaleOrg, catalog.ProductId);
                            if (onHand != null)
                            {
                                if (catalog.Quantity > onHand.Qty)
                                {
                                    return _APIError(string.Format("\"{0}\" không đủ số lượng tồn kho!", catalog.ProductCode));
                                }
                            }
                            DeliveryDetailModel deliveryDetail = new DeliveryDetailModel();
                            deliveryDetail.DeliveryDetailId = Guid.NewGuid();
                            deliveryDetail.DeliveryId = delivery.DeliveryId;
                            deliveryDetail.ProductId = catalog.ProductId;
                            //stock id
                            var stock_store = _context.Stock_Store_Mapping.Where(p => p.StoreId == delivery.StoreId).FirstOrDefault();
                            if (stock_store != null)
                            {
                                deliveryDetail.StockId = stock_store.StockId;
                            }
                            deliveryDetail.DateKey = _unitOfWork.UtilitiesRepository.ConvertDateTimeToInt(delivery.DocumentDate);
                            deliveryDetail.Quantity = catalog.Quantity;
                            deliveryDetail.Price = 0;
                            deliveryDetail.UnitPrice = 0;
                            deliveryDetail.Note = profile.Note;
                            deliveryDetail.isDeleted = false;

                            _context.Entry(deliveryDetail).State = EntityState.Added;
                        }

                    }
                    #endregion Delivery

                    //CustomerTaste - Thị hiếu khách hàng
                    #region CustomerTaste
                    //Màu
                    if (profile.ColorList != null && profile.ColorList.Count > 0)
                    {
                        foreach (var color in profile.ColorList)
                        {
                            CustomerTastesModel customerTaste = new CustomerTastesModel();
                            customerTaste.CustomerTasteId = Guid.NewGuid();
                            customerTaste.ERPProductCode = color.MaterialCode;
                            customerTaste.ProductCode = color.ProductCode.Split('-')[0].Trim();
                            customerTaste.ProductName = color.ProductName;
                            customerTaste.ProfileId = profile.ProfileId;
                            customerTaste.AppointmentId = appointmentViewModel.AppointmentId;
                            customerTaste.CreatedDate = DateTime.Now;
                            var storeCustomerTaste = _context.StoreModel.Where(p => p.SaleOrgCode == color.SaleOrgCode).FirstOrDefault();
                            if (storeCustomerTaste != null)
                            {
                                customerTaste.StoreId = storeCustomerTaste.StoreId;
                                customerTaste.CompanyId = _storeRepository.GetCompanyIdByStoreId(storeCustomerTaste.StoreId);
                            }
                            else
                            {
                                customerTaste.CompanyId = appointmentViewModel.CompanyId;
                            }
                            _context.Entry(customerTaste).State = EntityState.Added;
                        }
                    }
                    //Nhóm vân gỗ
                    if (profile.WoodGrainList != null && profile.WoodGrainList.Count > 0)
                    {
                        foreach (var woodGrain in profile.WoodGrainList)
                        {
                            CustomerTastes_WoodGrainModel customerTasteWoodGrain = new CustomerTastes_WoodGrainModel();
                            customerTasteWoodGrain.WoodGrainId = Guid.NewGuid();
                            customerTasteWoodGrain.WoodGrainCode = woodGrain.WoodGrainCode;
                            customerTasteWoodGrain.WoodGrainName = woodGrain.WoodGrainName;
                            customerTasteWoodGrain.ProfileId = profile.ProfileId;
                            customerTasteWoodGrain.CompanyId = appointmentViewModel.CompanyId;
                            customerTasteWoodGrain.StoreId = appointmentViewModel.StoreId;
                            customerTasteWoodGrain.AppointmentId = appointmentViewModel.AppointmentId;
                            customerTasteWoodGrain.CreatedDate = DateTime.Now;
                            _context.Entry(customerTasteWoodGrain).State = EntityState.Added;
                        }
                    }
                    //Tông màu
                    if (profile.ColorToneList != null && profile.ColorToneList.Count > 0)
                    {
                        foreach (var colorTone in profile.ColorToneList)
                        {
                            CustomerTastes_ColorToneModel customerTasteColorTone = new CustomerTastes_ColorToneModel();
                            customerTasteColorTone.ColorToneId = Guid.NewGuid();
                            customerTasteColorTone.ColorToneName = colorTone.ColorToneName;
                            customerTasteColorTone.ProfileId = profile.ProfileId;
                            customerTasteColorTone.CompanyId = appointmentViewModel.CompanyId;
                            customerTasteColorTone.StoreId = appointmentViewModel.StoreId;
                            customerTasteColorTone.AppointmentId = appointmentViewModel.AppointmentId;
                            customerTasteColorTone.CreatedDate = DateTime.Now;
                            _context.Entry(customerTasteColorTone).State = EntityState.Added;
                        }
                    }
                    //Bộ sưu tập
                    if (profile.CollectionList != null && profile.CollectionList.Count > 0)
                    {
                        foreach (var collection in profile.CollectionList)
                        {
                            CustomerTastes_CollectionModel customerTasteCollection = new CustomerTastes_CollectionModel();
                            customerTasteCollection.CollectionId = Guid.NewGuid();
                            customerTasteCollection.CollectionCode = collection.CollectionCode;
                            customerTasteCollection.CollectionName = collection.CollectionName;
                            customerTasteCollection.ProfileId = profile.ProfileId;
                            customerTasteCollection.CompanyId = appointmentViewModel.CompanyId;
                            customerTasteCollection.StoreId = appointmentViewModel.StoreId;
                            customerTasteCollection.AppointmentId = appointmentViewModel.AppointmentId;
                            customerTasteCollection.CreatedDate = DateTime.Now;
                            _context.Entry(customerTasteCollection).State = EntityState.Added;

                            //Ratings
                            RatingModel rating = new RatingModel();
                            rating.RatingId = Guid.NewGuid();
                            rating.RatingTypeCode = ConstCatalogType.CustomerTastes;
                            rating.ReferenceId = customerTasteCollection.CollectionId;
                            rating.Ratings = collection.Ratings;

                            _context.Entry(rating).State = EntityState.Added;
                        }
                    }
                    //Dòng sản phẩm
                    if (profile.ProductGroupList != null && profile.ProductGroupList.Count > 0)
                    {
                        foreach (var productGroup in profile.ProductGroupList)
                        {
                            CustomerTastes_ProductGroupModel customerTasteProductGroup = new CustomerTastes_ProductGroupModel();
                            customerTasteProductGroup.ProductGroupId = Guid.NewGuid();
                            customerTasteProductGroup.ProductGroupCode = productGroup.ProductGroupCode;
                            customerTasteProductGroup.ProductGroupName = productGroup.ProductGroupName;
                            customerTasteProductGroup.ProfileId = profile.ProfileId;
                            customerTasteProductGroup.CompanyId = appointmentViewModel.CompanyId;
                            customerTasteProductGroup.StoreId = appointmentViewModel.StoreId;
                            customerTasteProductGroup.AppointmentId = appointmentViewModel.AppointmentId;
                            customerTasteProductGroup.CreatedDate = DateTime.Now;
                            _context.Entry(customerTasteProductGroup).State = EntityState.Added;
                        }
                    }

                    #endregion CustomerTaste

                    #region Message return
                    if (responseContent == LanguageResource.Mobile_CreateProfile_Successfully)
                    {
                        responseContent = LanguageResource.Mobile_CreateProfile_Appointment_Successfully;
                    }
                    else if (responseContent == LanguageResource.Mobile_UpdateProfile_Successfully)
                    {
                        responseContent = LanguageResource.Mobile_UpdateProfile_Appointment_Successfully;
                    }
                    #endregion Message return
                }
                #endregion Nếu user chọn "Có ghé thăm" thì mới lưu dữ liệu ghé thăm

                _context.SaveChanges();

                #region sync_Profile_ExtendInfo_To_Table
                _unitOfWork.ProfileRepository.sync_Profile_ExtendInfo_To_Table(profile.ProfileId);
                #endregion
                //Save success => send sms
                #region Send SMS
                //bool isSentSMS = ConstDomain.isSentSMS;
                //if (!string.IsNullOrEmpty(profile.Phone) && isSentSMS == true)
                //{
                //    //Check if role is has send SMS permission
                //    var roleHasSendSMSPermission = (from acc in _context.AccountModel
                //                                    from accRole in acc.RolesModel
                //                                    join r in _context.RolesModel on accRole.RolesId equals r.RolesId
                //                                    where r.isSendSMSPermission == true && acc.AccountId == profile.CreateBy
                //                                    select acc).FirstOrDefault();

                //    if (roleHasSendSMSPermission != null)
                //    {
                //        SendSMSViewModel smsViewModel = new SendSMSViewModel();
                //        smsViewModel.PhoneNumber = profile.Phone;

                //        string brandName = string.Empty;
                //        string tokenSMS = string.Empty;
                //        string message = string.Empty;
                //        var company = _context.CompanyModel.Where(p => p.CompanyId == appointmentViewModel.CompanyId).FirstOrDefault();
                //        if (company != null)
                //        {
                //            string companyCode = company.CompanyCode;
                //            switch (companyCode)
                //            {
                //                case ConstCompanyCode.AnCuong:
                //                    brandName = ConstCompanyCode.BrandName_AnCuong;
                //                    tokenSMS = ConstDomain.TokenAnCuong;
                //                    break;
                //                case ConstCompanyCode.Malloca:
                //                    brandName = ConstCompanyCode.BrandName_Malloca;
                //                    tokenSMS = ConstDomain.TokenMalloca;
                //                    break;
                //                case ConstCompanyCode.Aconcept:
                //                    brandName = ConstCompanyCode.BrandName_Aconcept;
                //                    tokenSMS = ConstDomain.TokenAconcept;
                //                    break;
                //                default:
                //                    brandName = ConstCompanyCode.BrandName_AnCuong;
                //                    tokenSMS = ConstDomain.TokenAnCuong;
                //                    break;
                //            }
                //        }
                //        //brand name
                //        smsViewModel.BrandName = brandName;

                //        //token
                //        smsViewModel.Token = tokenSMS;

                //        //message
                //        message = _context.CatalogModel.Where(p => p.CatalogTypeCode == ConstCatalogType.SMSTemplate && p.CatalogCode == store.SMSTemplateCode).Select(p => p.CatalogText_vi).FirstOrDefault();
                //        //smsViewModel.Message = string.Format("Cam on quy khach da den tham quan {0}", store.StoreName);
                //        bool isSent = false;
                //        if (message != null)
                //        {
                //            isSent = true;
                //            var title = _context.CatalogModel.Where(p => p.CatalogTypeCode == ConstCatalogType.Title && p.CatalogCode == profile.Title).Select(p => p.CatalogText_vi).FirstOrDefault();
                //            if (title != null)
                //            {
                //                #region Cập nhật nội dung tin nhắn dựa vào danh xưng; bỏ bớt trùng lặp
                //                //Bỏ dấu Danh xưng
                //                var titleRemoveSign = _unitOfWork.UtilitiesRepository.RemoveSign4VietnameseString(title);
                //                //Bỏ dấu tên Khách hàng
                //                var profileName = _unitOfWork.UtilitiesRepository.RemoveSign4VietnameseString(profile.ProfileName);
                //                string[] words = null;

                //                //Tách tên KH dựa theo khoảng trắng
                //                if (!string.IsNullOrEmpty(profileName))
                //                {
                //                    words = profileName.Split(' ');
                //                }

                //                bool IsRemoveTitle = false;
                //                if (words.Length >= 1)
                //                {
                //                    //Nếu KH là Doanh nghiệp
                //                    if (profile.CustomerTypeCode == ConstCustomerType.Bussiness)
                //                    {
                //                        if (words[0].ToLower() == "cty")
                //                        {
                //                            IsRemoveTitle = true;
                //                        }
                //                        else
                //                        {
                //                            var titleProfileName = _unitOfWork.UtilitiesRepository.RemoveSign4VietnameseString(words[0] + " " + words[1]).ToLower();
                //                            if (titleProfileName == titleRemoveSign.ToLower())
                //                            {
                //                                IsRemoveTitle = true;
                //                            }
                //                        }
                //                    }
                //                    //Nếu KH là Tiêu dùng
                //                    else
                //                    {
                //                        if (words[0].ToLower() == titleRemoveSign.ToLower())
                //                        {
                //                            IsRemoveTitle = true;
                //                        }
                //                    }
                //                }
                //                var titleMessage = titleRemoveSign;
                //                if (IsRemoveTitle == true)
                //                {
                //                    titleRemoveSign = "";
                //                }
                //                #endregion

                //                smsViewModel.Message = message.Replace("[Title]", titleRemoveSign).Replace("[Name]", _unitOfWork.UtilitiesRepository.RemoveSign4VietnameseString(profile.ProfileName));
                //            }
                //        }
                //        else
                //        {
                //            isSent = false;
                //        }

                //        if (isSent == true)
                //        {
                //            _unitOfWork.SendSMSRepository.SendSMSToCustomer(smsViewModel);

                //            _context.SaveChanges();
                //        }
                //    }
                //}
                #endregion

                return _APISuccess(null, responseContent);
            });
        }
        #endregion Create Profile

        #region Get Notification
        public ActionResult GetNotificationBy(Guid AccountId, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                var result = new List<NotificationMobileViewModel>();
                //Get notification id by AccountId
                //var notificationAccountLst = _context.NotificationAccountMappingModel.Where(p => p.AccountId == AccountId && p.IsRead != true).ToList();
                var notificationAccountLst = (from p in _context.NotificationModel
                                              join a in _context.NotificationAccountMappingModel on p.NotificationId equals a.NotificationId
                                              join t in _context.TaskModel on p.TaskId equals t.TaskId
                                              where a.AccountId == AccountId && a.IsRead != true
                                              select new NotificationMobileViewModel()
                                              {
                                                  NotificationId = a.NotificationId,
                                                  IsRead = a.IsRead,
                                                  TaskId = p.TaskId,
                                                  Title = p.Title,
                                                  Description = p.Description,
                                                  Detail = p.Detail,
                                                  CreatedDate = p.CreatedDate,
                                              }).ToList();

                return _APISuccess(notificationAccountLst.Count);
            });
        }
        #endregion Get Notification

        #region Send SMS
        public ActionResult SendSMS(Guid AccountId, Guid AppointmentId, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                string PhoneNumber = string.Empty;
                string SMSContent = string.Empty;
                var appointment = _context.AppointmentModel.Where(p => p.AppointmentId == AppointmentId && p.isSentSMS != true).FirstOrDefault();
                var task = _context.TaskModel.Where(p => p.TaskId == AppointmentId).FirstOrDefault();
                Guid CompanyId = Guid.Empty;
                Guid StoreId = Guid.Empty;
                if (appointment != null && task != null)
                {
                    CompanyId = task.CompanyId;
                    StoreId = task.StoreId;
                    //1. Nếu có PrimaryContactId thì tìm trong bảng Profile và lấy Phone để gửi SMS
                    //2. Nếu PrimaryContactId = null thì vào bảng Task tìm ProfileId, sau đó tìm trong bảng Profile để lấy Phone gửi SMS => nếu không có thì lấy từ Contact
                    if (appointment.PrimaryContactId != null)
                    {
                        var profile = _context.ProfileModel.Where(p => p.ProfileId == appointment.PrimaryContactId).FirstOrDefault();
                        if (profile != null)
                        {
                            PhoneNumber = profile.Phone;
                        }
                    }
                    else
                    {
                        var profile = _context.ProfileModel.Where(p => p.ProfileId == task.ProfileId).FirstOrDefault();
                        if (profile != null && !string.IsNullOrEmpty(profile.Phone))
                        {
                            PhoneNumber = profile.Phone;
                        }
                        else
                        {
                            var contact = _context.ProfileContactAttributeModel.Where(p => p.CompanyId == task.ProfileId).ToList();
                            if (contact != null && contact.Count > 0)
                            {
                                var isMain = contact.Where(p => p.IsMain == true).FirstOrDefault();
                                if (isMain != null)
                                {
                                    PhoneNumber = _context.ProfileModel.Where(p => p.ProfileId == isMain.ProfileId).Select(p => p.Phone).FirstOrDefault();
                                }
                                else
                                {
                                    Guid ProfileId = contact[0].ProfileId;
                                    PhoneNumber = _context.ProfileModel.Where(p => p.ProfileId == ProfileId).Select(p => p.Phone).FirstOrDefault();
                                }
                            }
                        }
                    }

                    bool isSentSMS = ConstDomain.isSentSMS;
                    if (!string.IsNullOrEmpty(PhoneNumber) && isSentSMS == true)
                    {
                        //Check if role is has send SMS permission
                        var roleHasSendSMSPermission = (from acc in _context.AccountModel
                                                        from accRole in acc.RolesModel
                                                        join r in _context.RolesModel on accRole.RolesId equals r.RolesId
                                                        where r.isSendSMSPermission == true && acc.AccountId == AccountId
                                                        select acc).FirstOrDefault();

                        if (roleHasSendSMSPermission != null)
                        {
                            SendSMSViewModel smsViewModel = new SendSMSViewModel();
                            smsViewModel.PhoneNumber = PhoneNumber;

                            string brandName = string.Empty;
                            string tokenSMS = string.Empty;
                            string message = string.Empty;
                            var company = _context.CompanyModel.Where(p => p.CompanyId == CompanyId).FirstOrDefault();
                            if (company != null)
                            {
                                string companyCode = company.CompanyCode;
                                switch (companyCode)
                                {
                                    case ConstCompanyCode.AnCuong:
                                        brandName = ConstCompanyCode.BrandName_AnCuong;
                                        tokenSMS = ConstDomain.TokenAnCuong;
                                        break;
                                    case ConstCompanyCode.Malloca:
                                        brandName = ConstCompanyCode.BrandName_Malloca;
                                        tokenSMS = ConstDomain.TokenMalloca;
                                        break;
                                    case ConstCompanyCode.Aconcept:
                                        brandName = ConstCompanyCode.BrandName_Aconcept;
                                        tokenSMS = ConstDomain.TokenAconcept;
                                        break;
                                    default:
                                        brandName = ConstCompanyCode.BrandName_AnCuong;
                                        tokenSMS = ConstDomain.TokenAnCuong;
                                        break;
                                }
                            }
                            //brand name
                            smsViewModel.BrandName = brandName;

                            //token
                            smsViewModel.Token = tokenSMS;

                            //message
                            var store = _context.StoreModel.Where(p => p.StoreId == StoreId).FirstOrDefault();
                            message = _context.CatalogModel.Where(p => p.CatalogTypeCode == ConstCatalogType.SMSTemplate && p.CatalogCode == store.SMSTemplateCode).Select(p => p.CatalogText_vi).FirstOrDefault();
                            //smsViewModel.Message = string.Format("Cam on quy khach da den tham quan {0}", store.StoreName);
                            bool isSent = false;
                            if (message != null)
                            {
                                isSent = true;
                                smsViewModel.Message = message;
                                SMSContent = message;
                            }
                            else
                            {
                                isSent = false;
                            }

                            //Nếu đủ điều kiện gửi SMS thì mới tiến hành gửi
                            if (isSent == true)
                            {
                                var response = _unitOfWork.SendSMSRepository.SendSMSToCustomer(smsViewModel);
                                if (response != null)
                                {
                                    _context.SaveChanges();
                                    if (response.isSent == false)
                                    {
                                        return _APIError("Đã xảy ra lỗi khi gửi tin: " + response.ErrorMessage);
                                    }
                                    else
                                    {
                                        appointment.isSentSMS = true;
                                        _context.Entry(appointment).State = EntityState.Modified;
                                        _context.SaveChanges();
                                    }
                                }
                            }
                        }
                        else
                        {
                            return _APIError("Tài khoản này chưa được cấp quyền gửi SMS. Vui lòng liên hệ bộ phận IT để được cấp quyền.");
                        }
                    }
                }

                return _APISuccess(new { PhoneNumber, SMSContent });
            });
        }
        #endregion Send SMS

        #region Test Send message
        public ActionResult TestSendMessage(string CustomerTypeCode, string Title, string phoneNumber, string ProfileName)
        {
            bool isSentSMS = ConstDomain.isSentSMS;
            if (!string.IsNullOrEmpty(phoneNumber) && isSentSMS == true)
            {
                ////Check if role is has send SMS permission
                //var roleHasSendSMSPermission = (from acc in _context.AccountModel
                //                                from accRole in acc.RolesModel
                //                                join r in _context.RolesModel on accRole.RolesId equals r.RolesId
                //                                where r.isSendSMSPermission == true && acc.AccountId == profile.CreateBy
                //                                select acc).FirstOrDefault();

                //if (roleHasSendSMSPermission != null)
                //{
                SendSMSViewModel smsViewModel = new SendSMSViewModel();
                smsViewModel.PhoneNumber = phoneNumber;
                //brand name
                smsViewModel.BrandName = ConstCompanyCode.BrandName_AnCuong;

                //token
                smsViewModel.Token = ConstDomain.TokenAnCuong;
                var message = string.Empty;
                //message
                message = _context.CatalogModel.Where(p => p.CatalogTypeCode == ConstCatalogType.SMSTemplate && p.CatalogCode == "ACC").Select(p => p.CatalogText_vi).FirstOrDefault();
                //smsViewModel.Message = string.Format("Cam on quy khach da den tham quan {0}", store.StoreName);
                bool isSent = false;
                if (message != null)
                {
                    isSent = true;
                    var title = _context.CatalogModel.Where(p => p.CatalogTypeCode == ConstCatalogType.Title && p.CatalogCode == Title).Select(p => p.CatalogText_vi).FirstOrDefault();
                    if (title != null)
                    {
                        //Bỏ dấu Danh xưng
                        var titleRemoveSign = _unitOfWork.UtilitiesRepository.RemoveSign4VietnameseString(title);
                        //Bỏ dấu tên Khách hàng
                        var profileName = _unitOfWork.UtilitiesRepository.RemoveSign4VietnameseString(ProfileName);
                        string[] words = null;

                        //Tách tên KH dựa theo khoảng trắng
                        if (!string.IsNullOrEmpty(profileName))
                        {
                            words = profileName.Split(' ');
                        }

                        bool IsRemoveTitle = false;
                        if (words.Length >= 1)
                        {
                            //Nếu KH là Doanh nghiệp
                            if (CustomerTypeCode == ConstCustomerType.Bussiness)
                            {
                                if (words[0].ToLower() == "cty")
                                {
                                    IsRemoveTitle = true;
                                }
                                else
                                {
                                    var titleProfileName = _unitOfWork.UtilitiesRepository.RemoveSign4VietnameseString(words[0] + " " + words[1]).ToLower();
                                    if (titleProfileName == titleRemoveSign.ToLower())
                                    {
                                        IsRemoveTitle = true;
                                    }
                                }
                            }
                            //Nếu KH là Tiêu dùng
                            else
                            {
                                if (words[0].ToLower() == titleRemoveSign.ToLower())
                                {
                                    IsRemoveTitle = true;
                                }
                            }
                        }

                        var titleMessage = titleRemoveSign;
                        if (IsRemoveTitle == true)
                        {
                            titleRemoveSign = "";
                        }
                        smsViewModel.Message = message.Replace("[Title]", titleRemoveSign).Replace("[Name]", _unitOfWork.UtilitiesRepository.RemoveSign4VietnameseString(ProfileName));
                    }
                }
                else
                {
                    isSent = false;
                }

                if (isSent == true)
                {
                    //_unitOfWork.SendSMSRepository.SendSMSToCustomer(smsViewModel);

                    _context.SaveChanges();
                }
            }
            return _APISuccess(null);
        }
        #endregion

        #region Employee ratings
        public ActionResult EmployeeRatings(Guid AccountId, string Ratings, string Reviews, string FullName, string PhoneNumber, string Email, bool? isHasOtherReviews, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                var currentDate = DateTime.Now;
                if (isHasOtherReviews.HasValue && isHasOtherReviews == true)
                {
                    Ratings = (from p in _context.CatalogModel
                               where p.CatalogCode == "EmployeeRatings6"
                               select p.CatalogCode).FirstOrDefault();
                    if (string.IsNullOrEmpty(Reviews))
                    {
                        return _APIError("Quý khách vui lòng nhập ý kiến khác trước khi bấm nút GỬI Ý KIẾN");
                    }

                    //Gửi mail
                    /*
                        Mail nhận: infoacc@ancuong.com; tuyenhtn@ancuong.com
                        ======================================================
                        Subject: HỘP THƯ GÓP Ý-ĐÁNH GIÁ CỦA KHÁCH HÀNG 
                        ======================================================
                        Content:
                        ------------------------------
                        Họ và Tên:
                        SDT:
                        Email:
                        Nội dung góp ý:
                        User gửi: <lấy user đang tiếp khách>
                        Thời gian gửi: <ngày giờ khách hàng gửi ý kiến>
                        ------------------------------
                        - Về From email: thì lấy info@ancuong.com
                        - Về To email: thì cho anh chỗ thiết lập, để sau này có thay đổi email thì vào update lại
                    */
                    //User
                    var user = _context.AccountModel.Where(p => p.AccountId == AccountId).FirstOrDefault();
                    //GET email account
                    EmailAccountModel emailAccount = _context.EmailAccountModel.Where(s => s.SenderName == "Gỗ An Cường" && s.IsSender == true).FirstOrDefault();
                    //get mail server provider
                    MailServerProviderModel provider = _context.MailServerProviderModel.Where(s => s.Id == emailAccount.ServerProviderId).FirstOrDefault();
                    var emailConfigLst = _context.CatalogModel.Where(p => p.CatalogTypeCode == ConstCatalogType.FeedbackEmailConfig).ToList();
                    //FromEmail
                    string FromEmail = emailConfigLst.Where(p => p.CatalogCode == "Feedback_FromEmail").Select(p => p.CatalogText_vi).FirstOrDefault();
                    //ToEmail
                    string ToEmail = emailConfigLst.Where(p => p.CatalogCode == "Feedback_ToEmail").Select(p => p.CatalogText_vi).FirstOrDefault();
                    //Subject
                    string Subject = "HỘP THƯ GÓP Ý-ĐÁNH GIÁ CỦA KHÁCH HÀNG";
                    //Content
                    string EmailContent = string.Empty;
                    EmailContent += "Họ và Tên: [FullName]";
                    EmailContent += "<br />";
                    EmailContent += "SDT: [PhoneNumber]";
                    EmailContent += "<br />";
                    EmailContent += "Email: [Email]";
                    EmailContent += "<br />";
                    EmailContent += "Nội dung góp ý: [Reviews]";
                    EmailContent += "<br />";
                    EmailContent += "User gửi: [UserName]";
                    EmailContent += "<br />";
                    EmailContent += "Thời gian gửi: [CurrentDate]";
                    EmailContent += "<br />";

                    EmailContent = EmailContent.Replace("[FullName]", FullName)
                                                .Replace("[PhoneNumber]", PhoneNumber)
                                                .Replace("[Email]", Email)
                                                .Replace("[Reviews]", Reviews)
                                                .Replace("[UserName]", string.IsNullOrEmpty(user.FullName) ? user.UserName : user.FullName)
                                                .Replace("[CurrentDate]", string.Format("{0:dd/MM/yyyy HH:mm:ss}", currentDate))
                                               ;

                    SendMail(EmailContent, Subject, FromEmail, emailAccount.Account, emailAccount.Password, provider.OutgoingHost, provider.OutgoingPort, emailAccount.EnableSsl ?? false, ToEmail);
                }
                //Ratings
                RatingModel rating = new RatingModel();
                rating.RatingId = Guid.NewGuid();
                rating.RatingTypeCode = ConstCatalogType.EmployeeRatings;
                rating.ReferenceId = AccountId;
                rating.Ratings = Ratings;
                rating.Reviews = Reviews;
                rating.FullName = FullName;
                rating.PhoneNumber = PhoneNumber;
                rating.Email = Email;
                rating.CreateTime = currentDate;

                _context.Entry(rating).State = EntityState.Added;
                _context.SaveChanges();

                return _APISuccess(new { message = "Cảm ơn quý khách! \n Đánh giá của quý khách giúp chúng tôi cải thiện chất lượng phục vụ tốt hơn.", uri = "https://crm.ancuong.com/Images/check-mark.png" });
            });
        }

        private void SendMail(string EmailContent, string Subject, string FromMail, string Account, string FromEmailPassword, string Host, int Port, bool EnableSsl, string ToMail)
        {
            MailMessage email = new MailMessage();
            email.From = new MailAddress(FromMail);
            email.Sender = new MailAddress(FromMail);
            List<string> toEmailList = ToMail.Split(';').ToList();
            foreach (var toEmail in toEmailList.Distinct())
            {
                if (!string.IsNullOrEmpty(toEmail))
                {
                    email.To.Add(new MailAddress(toEmail.Trim()));
                }
            }
            //email.CC.Add(new MailAddress(FromMail.Trim()));
            email.Body = EmailContent;
            email.IsBodyHtml = true;
            email.BodyEncoding = Encoding.UTF8;
            email.Subject = Subject;

            string message = "";
            using (var smtp = new SmtpClient())
            {
                smtp.Host = Host;
                smtp.Port = Port;
                smtp.EnableSsl = EnableSsl;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(Account, FromEmailPassword);
                try
                {
                    smtp.Send(email);
                }
                catch (SmtpException ex)
                {
                    message = ex.Message;
                    if (ex.InnerException != null)
                    {
                        if (ex.InnerException.InnerException != null)
                        {
                            message = ex.InnerException.InnerException.Message;
                        }
                        else
                        {
                            message = ex.InnerException.Message;
                        }
                    }
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                    if (ex.InnerException != null)
                    {
                        if (ex.InnerException.InnerException != null)
                        {
                            message = ex.InnerException.InnerException.Message;
                        }
                        else
                        {
                            message = ex.InnerException.Message;
                        }
                    }
                }
            }
        }
        #endregion Employee ratings
    }
}