using ISD.Constant;
using ISD.EntityModels;
using ISD.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ISD.Repositories
{
    public class ConstructionRepository
    {
        private EntityDataContext _context;

        /// <summary>
        /// Hảm khởi tạo
        /// </summary>
        /// <param name="db">Truyền vào Db Context</param>
        public ConstructionRepository(EntityDataContext db)
        {
            _context = db;
        }
    }
}
