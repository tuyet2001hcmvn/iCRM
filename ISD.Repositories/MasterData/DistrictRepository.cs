using ISD.Constant;
using ISD.EntityModels;
using ISD.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ISD.Repositories
{
    public class DistrictRepository
    {
        EntityDataContext _context;
        public DistrictRepository(EntityDataContext db)
        {
            _context = db;
        }
        public DistrictViewModel Find(Guid? DistrictId)
        {
            var ret = (from p in _context.DistrictModel
                       where p.Actived == true
                       && p.DistrictId == DistrictId
                       select new DistrictViewModel()
                       {
                           DistrictId = p.DistrictId,
                           DistrictName = p.Appellation + " " + p.DistrictName
                       }).FirstOrDefault();
            return ret;
        }
        public List<DistrictViewModel> GetBy(Guid? ProvinceId)
        {
            var ret = (from p in _context.DistrictModel
                       where p.Actived == true
                       && (ProvinceId != null && p.ProvinceId == ProvinceId)
                       orderby p.OrderIndex, p.DistrictName
                       select new DistrictViewModel()
                       {
                           DistrictId = p.DistrictId,
                           DistrictName = p.Appellation + " " + p.DistrictName
                       }).ToList();
            return ret;
        }
        public List<DistrictViewModel> GetBy(List<Guid?> ProvinceId)
        {
            var ret = (from p in _context.DistrictModel
                       join pro in _context.ProvinceModel on p.ProvinceId equals pro.ProvinceId
                       join a in ProvinceId on pro.ProvinceId equals a.Value
                       where p.Actived == true
                       //&& (ProvinceId != null && p.ProvinceId == ProvinceId)
                       orderby pro.OrderIndex, p.OrderIndex, p.DistrictName
                       select new DistrictViewModel()
                       {
                           DistrictId = p.DistrictId,
                           DistrictName = pro.ProvinceName + " | " +  p.Appellation + " " + p.DistrictName
                       }).ToList();
            return ret;
        }

        public List<DistrictViewModel> GetAll()
        {
            var ret = (from p in _context.DistrictModel
                       join province in _context.ProvinceModel on p.ProvinceId equals province.ProvinceId
                       where p.Actived == true
                       orderby p.OrderIndex, p.DistrictName
                       select new DistrictViewModel()
                       {
                           DistrictId = p.DistrictId,
                           DistrictName = province.ProvinceName + "_" + p.Appellation + " " + p.DistrictName,
                           ProvinceId = province.ProvinceId,
                           ProvinceName = province.ProvinceName,
                       }).ToList();
            return ret;
        }
    }
}