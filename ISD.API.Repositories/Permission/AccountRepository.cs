using ISD.API.Core;
using ISD.API.EntityModels.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.API.Repositories.Permission
{
    public class AccountRepository : GenericRepository<AccountModel>, IAccountRepository
    {
        public AccountRepository(ICRMDbContext context) : base(context)
        {
        }

        public SalesEmployeeModel GetEmployee(string employeeCode)
        {
            return context.SalesEmployeeModels.Where(x=>x.SalesEmployeeCode == employeeCode).FirstOrDefault();
        }

    }
}
