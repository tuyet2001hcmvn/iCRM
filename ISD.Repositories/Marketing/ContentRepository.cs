using ISD.Constant;
using ISD.EntityModels;
using ISD.ViewModels;
using ISD.ViewModels.Marketing;
using ISD.ViewModels.Sale;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;

namespace ISD.Repositories
{
    public class ContentRepository
    {
        private EntityDataContext _context;
        /// <summary>
        /// Khởi tạo ProductRepository truyển vào DataContext
        /// </summary>
        /// <param name="db">EntityDataContext</param>
        public ContentRepository(EntityDataContext db)
        {
            _context = db;
        }
        /// <summary>
        /// Lấy thông tin Campaign
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ContentModel GetBy(Guid? Id)
        {
            var data = _context.ContentModel.Find(Id);
            return data;
        }

        public ContentModel GetBy(int? Code)
        {
            var data = _context.ContentModel.Where(x=>x.ContentCode == Code).FirstOrDefault();
            return data;
        }

        public IEnumerable<ContentModel> GetAll()
        {
            var data = _context.ContentModel;
            return data;
        }


        public ContentModel GetContentByCampaign(Guid? Id)
        {
            var data = (from c in _context.ContentModel
                        join p in _context.CampaignModel on c.Id equals p.ContentId
                        where p.Id == Id
                        select c).FirstOrDefault();
            return data;
        }



    }
}