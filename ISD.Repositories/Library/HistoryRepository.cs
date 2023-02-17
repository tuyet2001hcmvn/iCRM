using ISD.EntityModels;
using System;
using System.Web;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ISD.Repositories
{
    public interface IRepository
    {
        void SaveUpdateHistory<T>(Guid Id, string UserName, Guid? PageId, T obj, params Expression<Func<T, object>>[] propertiesToUpdate) where T : class;
    }

    public class HistoryRepository : IRepository
    {
        EntityDataContext _context;
        public HistoryRepository(EntityDataContext ct)
        {
            _context = ct;
        }

        public const string LastModifiedTime = "LastModifiedTime";

        public void SaveUpdateHistory<T>(Guid Id, string UserName, Guid? PageId, T obj, params Expression<Func<T, object>>[] propertiesToUpdate) where T : class
        {
            //Ensure only modified fields are updated.
            var dbEntityEntry = _context.Entry(obj);
            if (propertiesToUpdate.Any())
            {
                //update explicitly mentioned properties
                foreach (var property in propertiesToUpdate)
                {
                    dbEntityEntry.Property(property).IsModified = true;
                }
            }
            else
            {
                //no items mentioned, so find out the updated entries
                foreach (var property in dbEntityEntry.OriginalValues.PropertyNames)
                {
                    var original = dbEntityEntry.OriginalValues.GetValue<object>(property);
                    var current = dbEntityEntry.CurrentValues.GetValue<object>(property);
                    if (original != null && !original.Equals(current) && property != LastModifiedTime)
                    {
                        //dbEntityEntry.Property(property).IsModified = true;

                        //NOTE: MUST USING VIEW MODEL
                        #region Insert into HistoryModel
                        HistoryModel model = new HistoryModel();
                        model.HistoryModifyId = Guid.NewGuid();
                        //get PageId
                        //var path = System.Web.HttpContext.Current.Request.Url.AbsolutePath;
                        //Guid? PageId = _context.PageModel.Where(p => path.Contains(p.PageUrl))
                        //                               .Select(p => p.PageId).FirstOrDefault();
                        model.PageId = (PageId == Guid.Empty) ? null : PageId;
                        model.FieldId = Id;
                        model.FieldName = property;
                        model.OldData = original.ToString();
                        model.LastModifiedUser = UserName;
                        model.LastModifiedTime = DateTime.Now;
                        _context.Entry(model).State = EntityState.Added;
                        #endregion Insert into HistoryModel
                    }
                }
            }
        }
    }
}
