using ISD.EntityModels;
using ISD.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.Repositories
{
    public class FaceCheckInOutRepository
    {
        EntityDataContext _context;
        public FaceCheckInOutRepository(EntityDataContext context)
        {
            _context = context;
        }
        public List<FaceCheckInOutViewModel> GetListFaceCheckInByDate(DateTime fronmDate, DateTime toDate, string type)
        {
            object[] SqlParams =
            {
                new SqlParameter("@fromDate",fronmDate),
                new SqlParameter("@toDate",toDate),
                new SqlParameter("@type",type)
            };
            var res = _context.Database.SqlQuery<FaceCheckInOutViewModel>("FaceCheckInOut @fromDate,@toDate, @type", SqlParams).ToList();
            return res;
        }
    }
}
