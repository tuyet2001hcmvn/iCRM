using ISD.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.Extensions
{
    public static class RepositoryHelper
    {
        public static string GetConfigByKey(EntityDataContext _context, string key)
        {
            var ret = "";

            var config = _context.ApplicationConfig.SingleOrDefault(a => a.ConfigKey == key);

            if (config != null)
            {
                ret = config.ConfigValue;
            }

            return ret;
        }
    }
}
