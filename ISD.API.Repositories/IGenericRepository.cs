using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ISD.API.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        //public Task<TEntity> GetById(object id, string[] paths = null);
        TEntity GetById(object id);
        TEntity GetBy(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes);
        IQueryable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includes);
        void Add(TEntity entity);
        void Delete(object id);
        void Delete(TEntity entityToDelete);
        void Update(TEntity entityToUpdate);
        public IEnumerable<TEntity> Get( Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");
    }
}
