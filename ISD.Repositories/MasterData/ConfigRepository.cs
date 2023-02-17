using ISD.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.Repositories
{
    public class ConfigRepository
    {
        EntityDataContext _context;
        public ConfigRepository(EntityDataContext db)
        {
            _context = db;
        }

        /// <summary>
        /// Lấy config theo key
        /// </summary>
        /// <param name="ConfigKey">string: ConfigKey</param>
        /// <returns>ConfigValue</returns>
        public string GetBy(string ConfigKey)
        {
            var ret = _context.ApplicationConfig.Where(p => p.ConfigKey == ConfigKey).Select(p => p.ConfigValue).FirstOrDefault();
            return ret;
        }
        
    }
}
