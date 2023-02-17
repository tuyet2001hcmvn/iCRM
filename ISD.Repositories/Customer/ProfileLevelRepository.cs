using ISD.EntityModels;
using ISD.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ISD.Repositories
{
    public class ProfileLevelRepository
    {
        private EntityDataContext _context;

        public ProfileLevelRepository(EntityDataContext db)
        {
            _context = db;
        }

        /// <summary>
        /// Tìm kiếm cấp bậc KH
        /// </summary>
        /// <param name="CompanyId">Công ty</param>
        /// <param name="CustomerLevelCode">Mã cấp bậc</param>
        /// <param name="CustomerLevelName">Tên cấp bậc</param>
        /// <param name="Actived">Trạng thái</param>
        /// <returns>Danh sách cấp bậc KH</returns>
        public List<ProfileLevelViewModel> Search(Guid? CompanyId = null, string CustomerLevelCode = "", string CustomerLevelName = "", bool? Actived = null)
        {
            var lst = (from p in _context.ProfileLevelModel
                       join c in _context.CompanyModel on p.CompanyId equals c.CompanyId
                       where
                       //Search by CompanyId
                       (CompanyId == null || p.CompanyId == CompanyId)
                       //Search by CustomerLevelCode
                       && (CustomerLevelCode == "" || p.CustomerLevelCode.Contains(CustomerLevelCode))
                       //Search by CustomerLevelName
                       && (CustomerLevelName == "" || p.CustomerLevelName.Contains(CustomerLevelName))
                       //Search by Actived
                       && (Actived == null || p.Actived == Actived)
                       orderby p.CustomerLevelCode
                       select new ProfileLevelViewModel()
                       {
                           CustomerLevelId = p.CustomerLevelId,
                           CustomerLevelCode = p.CustomerLevelCode,
                           CustomerLevelName = p.CustomerLevelName,
                           LineOfLevel = p.LineOfLevel,
                           ExchangeValue = p.ExchangeValue,
                           Actived = p.Actived,
                           FromDate = p.FromDate,
                           ToDate = p.ToDate,
                           CompanyId = p.CompanyId,
                           CompanyName = c.CompanyName
                       }).ToList();
            return lst;
        }

        /// <summary>
        /// Get cấp bậc KH trong DB theo id
        /// </summary>
        /// <param name="levelId">ProfileLevelId</param>
        /// <returns>Warranty Model</returns>
        public ProfileLevelModel GetProfileLevel(Guid levelId)
        {
            var level = _context.ProfileLevelModel.FirstOrDefault(p => p.CustomerLevelId == levelId);
            if (level != null)
            {
                if (level.LineOfLevel != null)
                {
                    level.LineOfLevel = Convert.ToInt32(level.LineOfLevel);
                }
                if (level.ExchangeValue != null)
                {
                    level.ExchangeValue = Convert.ToInt32(level.ExchangeValue);
                }
            }
            return level;
        }

        public IEnumerable<ProfileLevelModel> GetAll()
        {
            var levelLst = _context.ProfileLevelModel.Where(p => p.Actived == true).OrderBy(p => p.CustomerLevelCode);
            return levelLst;
        }

        /// <summary>
        /// Thêm mới cấp bậc KH
        /// </summary>
        /// <param name="model">Profile Level Model</param>
        /// <returns>Warranty model</returns>
        public ProfileLevelModel Create(ProfileLevelModel model)
        {
            return _context.ProfileLevelModel.Add(model);
        }

        /// <summary>
        /// Cập nhật cấp bậc KH
        /// </summary>
        /// <param name="model">Profile Level Model</param>
        public void Update(ProfileLevelModel model)
        {
            var level = _context.ProfileLevelModel.FirstOrDefault(p => p.CustomerLevelId == model.CustomerLevelId);
            if (level != null)
            {
                level.CustomerLevelCode = model.CustomerLevelCode;
                level.CustomerLevelName = model.CustomerLevelName;
                level.LineOfLevel = model.LineOfLevel;
                level.ExchangeValue = model.ExchangeValue;
                level.FromDate = model.FromDate;
                level.ToDate = model.ToDate;
                level.Actived = model.Actived;
                level.CompanyId = model.CompanyId;

                level.LastEditBy = model.LastEditBy;
                level.LastEditTime = model.LastEditTime;

                _context.Entry(level).State = EntityState.Modified;
            }
        }
    }
}