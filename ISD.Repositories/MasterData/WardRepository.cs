using AutoMapper;
using ISD.Constant;
using ISD.EntityModels;
using ISD.Resources;
using ISD.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;

namespace ISD.Repositories
{
    public class WardRepository
    {
        EntityDataContext _context;
        public WardRepository(EntityDataContext db)
        {
            _context = db;
        }
        public WardViewModel Find(Guid? WardId)
        {
            var ret = (from p in _context.WardModel
                       where p.WardId == WardId
                       select new WardViewModel()
                       {
                           WardId = p.WardId,
                           WardName = p.Appellation + " " + p.WardName
                       }).FirstOrDefault();
            return ret;
        }
        public List<WardViewModel> GetBy(Guid? DistrictId)
        {
            var ret = (from p in _context.WardModel
                       where DistrictId != null && p.DistrictId == DistrictId
                       orderby p.OrderIndex, p.WardName
                       select new WardViewModel()
                       {
                           WardId = p.WardId,
                           WardName = p.Appellation + " " + p.WardName,
                       }).ToList();
            return ret;
        }
        public List<WardViewModel> GetAll()
        {
            var ret = (from p in _context.WardModel
                       join district in _context.DistrictModel on p.DistrictId equals district.DistrictId
                       join province in _context.ProvinceModel on district.ProvinceId equals province.ProvinceId
                       orderby p.OrderIndex, p.WardName
                       select new WardViewModel()
                       {
                           WardId = p.WardId,
                           WardName = province.ProvinceName + "_" + district.Appellation + " " + district.DistrictName + "_" + p.Appellation + " " + p.WardName,
                           DistrictId = district.DistrictId,
                           DistrictName = province.ProvinceName + "_" + district.Appellation + " " + district.DistrictName,
                           ProvinceId = province.ProvinceId,
                           ProvinceName = province.ProvinceName,
                       }).ToList();
            return ret;
        }
    }
}