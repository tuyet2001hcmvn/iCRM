using ISD.Core.Framework.Tasks;
using ISD.EntityModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ISD.Core.Framework
{
    public class TransactionPerRequest :
    IRunOnEachRequest, IRunOnError, IRunAfterEachRequest
    {
        private readonly EntityDataContext _dbContext;
        private readonly HttpContextBase _httpContext;

        public TransactionPerRequest(EntityDataContext dbContext,
            HttpContextBase httpContext)
        {
            _dbContext = dbContext;
            _httpContext = httpContext;
        }

        void IRunOnEachRequest.Execute()
        {
            _httpContext.Items["_Transaction"] =
                _dbContext.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
        }

        void IRunOnError.Execute()
        {
            _httpContext.Items["_Error"] = true;
        }

        void IRunAfterEachRequest.Execute()
        {
            var transaction = (DbContextTransaction)_httpContext.Items["_Transaction"];

            if (_httpContext.Items["_Error"] != null)
            {
                transaction.Rollback();
            }
            else
            {
                transaction.Commit();
            }
        }
    }
}
