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
    public class ProvinceRepository
    {
        EntityDataContext _context;
        public ProvinceRepository(EntityDataContext db)
        {
            _context = db;
        }
        public ProvinceViewModel Find(Guid? ProvinceId)
        {
            var ret = (from p in _context.ProvinceModel
                       where p.Actived == true
                       && p.ProvinceId == ProvinceId
                       select new ProvinceViewModel()
                       {
                           ProvinceId = p.ProvinceId,
                           ProvinceName = p.ProvinceName,
                           Area = p.Area
                       }).FirstOrDefault();
            return ret;
        }

        public List<ProvinceViewModel> GetAll()
        {
            var ret = (from p in _context.ProvinceModel
                       where p.Actived == true
                       orderby p.Area, p.ProvinceCode
                       select new ProvinceViewModel()
                       {
                           ProvinceId = p.ProvinceId,
                           ProvinceName = p.ProvinceName
                       }).ToList();
            return ret;
        }
        public List<ProvinceViewModel> GetForeign()
        {
            var ret = (from p in _context.ProvinceModel
                       where p.Actived == true
                       && p.IsForeign == true
                       orderby p.Area, p.ProvinceCode
                       select new ProvinceViewModel()
                       {
                           ProvinceCode = p.ProvinceCode,
                           ProvinceName = p.ProvinceName
                       }).ToList();
            return ret;
        }

        public string GetAreaByProvince(Guid? ProvinceId)
        {
            string ret = string.Empty;
            var area = (from p in _context.ProvinceModel
                        where p.Actived == true
                        && p.ProvinceId == ProvinceId
                        select p.Area).FirstOrDefault();
            if (area != null)
            {
                ret = area.ToString();
            }
            return ret;
        }
    }
}