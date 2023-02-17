using ISD.Constant;
using ISD.EntityModels;
using ISD.Repositories.Infrastructure.Extensions;
using ISD.Resources;
using ISD.ViewModels;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

namespace ISD.Repositories
{
    public class SponsRepository
    {
        private EntityDataContext _context;
        public SponsRepository(EntityDataContext dataContext)
        {
            _context = dataContext;
        }

        /// <summary>
        /// Danh sách Địa chỉ của khách hàng
        /// Địa chỉ chính: Address trong ProfileModel
        /// Địa chỉ còn lại: SponsModel
        /// </summary>
        /// <param name="profileId"></param>
        /// <returns></returns>
        public List<SponsViewModel> GetAll(Guid? ProfileId, string Type)
        {
            var sponsList = (from p in _context.SponsModel
                             where p.ProfileId == ProfileId && p.Type == Type
                             select new SponsViewModel
                             {
                                 SponsId = p.SponsId,
                                 ProfileId = p.ProfileId,
                                 Time = p.Time,
                                 Type = p.Type,
                                 Value = p.Value,
                                 Descriptions = p.Descriptions
                             }).ToList();
            return sponsList;
        } 
        
   
        public SponsViewModel GetById(Guid? sponsId)
        {
            var spons = (from p in _context.SponsModel
                         where p.SponsId == sponsId
                         select new SponsViewModel { 
                            SponsId = p.SponsId,
                            ProfileId = p.ProfileId,
                            Time = p.Time,
                            Type = p.Type,
                             Value = p.Value,
                             Descriptions = p.Descriptions
                         }).FirstOrDefault();
            return spons;
        }

        /// <summary>
        /// Thêm mới địa chỉ
        /// 1. Nếu       tick "Địa chỉ chính" (isMain) thì update Address trong ProfileModel, lấy Address hiện tại trong ProfileModel lưu vào SponsModel
        /// 2. Nếu không tick "Địa chỉ chính" (isMain) thì create SponsModel
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public SponsModel Create(SponsModel viewModel)
        {
            viewModel.SponsId = Guid.NewGuid();
            _context.Entry(viewModel).State = EntityState.Added;
            return viewModel;
        }

        /// <summary>
        /// Cập nhật địa chỉ
        /// 1. Nếu cập nhật địa chỉ thường thành địa chỉ chính => update Address trong ProfileModel, xóa địa chỉ chính (nếu có) trong SponsModel
        /// 2. Nếu cập nhật địa chỉ thường (không cập nhật isMain) => cập nhật SponsModel
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public bool Update(SponsModel viewModel)
        {
            var certificateInDB = _context.SponsModel.FirstOrDefault(p => p.SponsId == viewModel.SponsId);
            if (certificateInDB != null)
            {
                certificateInDB.Descriptions = viewModel.Descriptions;
                certificateInDB.Value = viewModel.Value;
                certificateInDB.Time = viewModel.Time;
                _context.Entry(certificateInDB).State = EntityState.Modified;
                return true;
            }
            return false;
        }

        public void Delete(Guid? sponsId)
        {
            var spons = _context.SponsModel.Find(sponsId);
            if (spons != null)
            {
                _context.Entry(spons).State = EntityState.Deleted;
                _context.SaveChanges();
            }
        }
    }
}
