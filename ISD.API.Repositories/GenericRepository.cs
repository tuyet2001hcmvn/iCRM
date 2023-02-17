using ISD.API.EntityModels.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ISD.API.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        internal ICRMDbContext context;
        internal DbSet<TEntity> dbSet;

        public GenericRepository(ICRMDbContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }
        public void Add(TEntity entity)
        {
            dbSet.Add(entity);
        }

        public void Delete(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }

        public void Delete(TEntity entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }

        public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }
        //public async Task<TEntity> GetById(object id, string[] paths = null)
        //{
        //    var model = await dbSet.FindAsync(id);
        //    foreach (var path in paths)
        //    {
        //        context.Entry(model).Reference(path).Load();
        //    }
        //    return model;
        //}
        public IQueryable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> set = dbSet.AsNoTracking();

            foreach (var include in includes)
            {
                set = set.Include(include);
            }

            return set.AsQueryable<TEntity>();
        }
        public TEntity GetBy(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
        {
            var result = GetAll();
            if (includes.Any())
            {
                foreach (var include in includes)
                {
                    result = result.Include(include);
                }
            }
            return result.FirstOrDefault(predicate);
        }
        public TEntity GetById(object id)
        {
            return dbSet.Find(id);
        }

        public void Update(TEntity entityToUpdate)
        {
            dbSet.Attach(entityToUpdate);
            context.Entry(entityToUpdate).State = EntityState.Modified;
        }
    }
}
