using ISD.Constant;
using ISD.EntityModels;
using ISD.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ISD.Repositories
{
    public class CatalogRepository
    {
        EntityDataContext _context;
        public CatalogRepository(EntityDataContext db)
        {
            _context = db;
        }

        /// <summary>
        /// Lấy Catalog theo CatalogCode and CatalogType
        /// </summary>
        /// <param name="CataLogType">string: CatalogType</param>
        /// <returns>Danh sách catalog</returns>
        public CatalogViewModel GetBy(string CatalogCode, string CataLogType)
        {
            var ret = (from p in _context.CatalogModel
                       where CataLogType == p.CatalogTypeCode 
                       && CatalogCode == p.CatalogCode
                       && p.Actived == true
                       orderby p.OrderIndex, p.CatalogCode
                       select new CatalogViewModel()
                       {
                           CatalogId = p.CatalogId,
                           CatalogCode = p.CatalogCode,
                           CatalogText_vi = p.CatalogText_vi,
                           CatalogText_en = p.CatalogText_en,
                           OrderIndex = p.OrderIndex,
                       }).FirstOrDefault();
            return ret;
        }

        /// <summary>
        /// Lấy danh sách Catalog theo CatalogType
        /// </summary>
        /// <param name="CataLogType">string: CatalogType</param>
        /// <returns>Danh sách catalog</returns>
        public List<CatalogViewModel> GetBy(string CataLogType)
        {
            var ret = (from p in _context.CatalogModel
                       where CataLogType == p.CatalogTypeCode && p.Actived == true
                       orderby p.OrderIndex, p.CatalogCode
                       select new CatalogViewModel()
                       {
                           CatalogId = p.CatalogId,
                           CatalogCode = p.CatalogCode,
                           CatalogText_vi = p.CatalogText_vi,
                           CatalogText_en = p.CatalogText_en,
                           OrderIndex = p.OrderIndex,
                       }).ToList();
            
            
            return ret;
        }

        /// <summary>
        /// Lấy danh sách phân nhóm khách hàng
        /// </summary>
        /// <param name=""></param>
        /// <returns>Danh sách phân nhóm khách hàng</returns>
        public List<CatalogViewModel> GetCustomerAccountGroup()
        {
            var ret = (from p in _context.CatalogModel
                       where p.CatalogTypeCode == ConstCatalogType.CustomerAccountGroup && p.Actived == true
                       && p.CatalogCode.StartsWith("Z")
                       orderby p.OrderIndex, p.CatalogCode
                       select new CatalogViewModel()
                       {
                           CatalogCode = p.CatalogCode,
                           CatalogText_vi = p.CatalogText_vi,
                           CatalogText_en = p.CatalogText_en,
                           OrderIndex = p.OrderIndex,
                       }).ToList();


            return ret;
        }

        /// <summary>
        /// Lấy danh sách ngành hàng theo loại đối thủ
        /// </summary>
        /// <param name="CompetitorType"></param>
        /// <returns></returns>
        public List<CatalogViewModel> GetDistributionIndustryBy(string CompetitorType)
        {
            var result = GetBy(ConstCatalogType.DistributionIndustry)
                         .Where(p => p.CatalogText_en.Contains(CompetitorType)).ToList();
          
            return result;
        }

        /// <summary>
        /// Lấy Khu vực theo Đối tượng
        /// </summary>
        /// <param name="isForeignCustomer"></param>
        /// <returns></returns>
        public List<CatalogViewModel> GetSaleOffice(bool? isForeignCustomer)
        {
            var saleOfficeList = GetBy(ConstCatalogType.SaleOffice);

            //Trong nước
            if (isForeignCustomer == false)
            {
                saleOfficeList = saleOfficeList.Where(p => p.CatalogCode == ConstSaleOffice.MienBac ||
                                                           p.CatalogCode == ConstSaleOffice.MienTrung ||
                                                           p.CatalogCode == ConstSaleOffice.MienNam ||
                                                           p.CatalogCode == ConstSaleOffice.Khac).ToList();
            }
            //Nước ngoài
            else if (isForeignCustomer == true)
            {
                saleOfficeList = saleOfficeList.Where(p => p.CatalogCode != ConstSaleOffice.MienBac &&
                                                           p.CatalogCode != ConstSaleOffice.MienTrung &&
                                                           p.CatalogCode != ConstSaleOffice.MienNam).ToList();
            }
            //Chưa chọn thì không có gì
            else
            {
                saleOfficeList = new List<CatalogViewModel>();
            }
            return saleOfficeList;
        }

        public List<CatalogViewModel> GetOpportunityRegion(bool? isForeignCustomer)
        {
            var opportunityRegionList = GetBy(ConstCatalogType.Opportunity_Region);

            //Trong nước
            if (isForeignCustomer == false)
            {
                opportunityRegionList = opportunityRegionList.Where(p => p.CatalogCode == ConstOpportunityRegion.MienBac ||
                                                           p.CatalogCode == ConstOpportunityRegion.MienTrung ||
                                                           p.CatalogCode == ConstOpportunityRegion.MienNam ||
                                                           p.CatalogCode == ConstOpportunityRegion.ToanQuoc).ToList();
            }
            //Nước ngoài
            else if (isForeignCustomer == true)
            {
                opportunityRegionList = opportunityRegionList.Where(p => p.CatalogCode != ConstOpportunityRegion.MienBac &&
                                                           p.CatalogCode != ConstOpportunityRegion.MienTrung &&
                                                           p.CatalogCode != ConstOpportunityRegion.MienNam &&
                                                           p.CatalogCode != ConstOpportunityRegion.ToanQuoc).ToList();
            }
            //Chưa chọn thì không có gì
            else
            {
                opportunityRegionList = new List<CatalogViewModel>();
            }
            return opportunityRegionList;
        }
        public List<CatalogViewModel> GetProfileShortName(bool? isForeignCustomer)
        {
            var profileShortNameList = GetBy(ConstCatalogType.ProfileShortName);

            //Trong nước
            if (isForeignCustomer == false)
            {
                profileShortNameList = profileShortNameList.Where(p => p.CatalogCode == ConstOpportunityRegion.MienBac ||
                                                           p.CatalogCode == ConstOpportunityRegion.MienTrung ||
                                                           p.CatalogCode == ConstOpportunityRegion.MienNam ||
                                                           p.CatalogCode == ConstOpportunityRegion.ToanQuoc).ToList();
            }
            //Nước ngoài
            else if (isForeignCustomer == true)
            {
                profileShortNameList = profileShortNameList.Where(p => p.CatalogCode != ConstOpportunityRegion.MienBac &&
                                                           p.CatalogCode != ConstOpportunityRegion.MienTrung &&
                                                           p.CatalogCode != ConstOpportunityRegion.MienNam &&
                                                           p.CatalogCode != ConstOpportunityRegion.ToanQuoc).ToList();
            }
            //Chưa chọn thì không có gì
            else
            {
                profileShortNameList = new List<CatalogViewModel>();
            }
            return profileShortNameList;
        }
        

        public List<CatalogViewModel> GetCustomerCategory(string CompanyCode)
        {
            var customerGroupList = GetBy(ConstCatalogType.CustomerGroup);
            //Load nhóm KH theo công ty (đang đăng nhập)
            List<CatalogViewModel> customerGroupBySaleOrgLst = new List<CatalogViewModel>();
            if (customerGroupList != null && customerGroupList.Count > 0)
            {
                foreach (var item in customerGroupList)
                {
                    if (!string.IsNullOrEmpty(item.CatalogText_en) && item.CatalogText_en.Contains(CompanyCode))
                    {
                        customerGroupBySaleOrgLst.Add(item);
                    }
                }
            }
            return customerGroupBySaleOrgLst;
        }

        public List<CatalogViewModel> GetCustomerCareer(string CompanyCode)
        {
            var customerCareerList = GetBy(ConstCatalogType.CustomerCareer);
            //Load nhóm KH theo công ty (đang đăng nhập)
            List<CatalogViewModel> customerCareerBySaleOrgLst = new List<CatalogViewModel>();
            if (customerCareerList != null && customerCareerList.Count > 0)
            {
                foreach (var item in customerCareerList)
                {
                    if (!string.IsNullOrEmpty(item.CatalogText_en) && item.CatalogText_en.Contains(CompanyCode))
                    {
                        customerCareerBySaleOrgLst.Add(item);
                    }
                }
            }
            return customerCareerBySaleOrgLst;
        }

        public List<CatalogViewModel> GetDropDowmByType(string catalogType)
        {
            var ret = (from cat in _context.CatalogModel
                       where cat.CatalogTypeCode == catalogType && cat.Actived == true
                       orderby cat.OrderIndex, cat.CatalogCode
                       select new CatalogViewModel()
                       {
                           CatalogId = cat.CatalogId,
                           CatalogCode = cat.CatalogCode,
                           CatalogText_vi = cat.CatalogCode + " | " +cat.CatalogText_vi,
                           CatalogText_en = cat.CatalogCode + " | " + cat.CatalogText_en,
                           OrderIndex = cat.OrderIndex,
                       }).ToList();

            return ret;
        }
    }
}