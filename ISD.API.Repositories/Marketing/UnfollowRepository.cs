using ISD.API.EntityModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.API.Repositories.Marketing
{
    public class UnfollowRepository : GenericRepository<Unfollow>, IUnfollowRepository
    {
        public UnfollowRepository(ICRMDbContext context) : base(context)
        {
        }

        public IQueryable<Unfollow> Search(string email, string companyCode)
        {
            var list = from t in context.Unfollows
                                      where 
                                       (email == null || email == "" || t.Email.Contains(email))
                                        && (companyCode ==null || companyCode =="" || t.CompanyCode==companyCode)
                                      orderby (t.Email)
                                      select t;
            return list;
        }
    }
}
