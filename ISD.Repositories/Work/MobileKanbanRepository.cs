using ISD.Constant;
using ISD.EntityModels;
using ISD.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.Repositories
{
    public class MobileKanbanRepository
    {
        EntityDataContext _context;

        /// <summary>
        /// Khởi tạo Task Repository
        /// </summary>
        /// <param name="db">Entity Data Context</param>
        public MobileKanbanRepository(EntityDataContext db)
        {
            _context = db;
        }

        public List<KanbanMenuViewModel> GetKanbanMenuByRole(Guid AccountId, string CurrentCompanyCode)
        {
            var result = new List<KanbanMenuViewModel>();
            //get roles by AccountId
            var roles = _context.AccountModel.Where(p => p.AccountId == AccountId).Select(p => p.RolesModel).FirstOrDefault();
            if (roles != null && roles.Count > 0)
            {
                var roleIdList = roles.Select(p => p.RolesId).ToList();
                result = (from m in _context.MobileScreenModel
                          join p in _context.MobileScreenPermissionModel on m.MobileScreenId equals p.MobileScreenId
                          where roleIdList.Contains(p.RolesId) 
                          && m.Actived == true
                          orderby m.OrderIndex
                          select new KanbanMenuViewModel()
                          {
                              MenuId = m.MobileScreenId,
                              MenuCode = m.ScreenCode,
                              MenuName = CurrentCompanyCode == "1000" && m.ScreenCode == "TICKET" ? "XỬ LÝ KHIẾU NẠI" : m.ScreenName,
                              IconType = m.IconType,
                              IconName = m.IconName,
                              OrderIndex = m.OrderIndex,
                              isCreated = m.isCreated,
                              isReporter = m.isReporter,
                              isAssignee = m.isAssignee,
                              Visible = m.Visible,
                          }).Distinct().OrderBy(p => p.OrderIndex).ToList();

                //Quyền được tạo task
                if (result != null && result.Count > 0)
                {
                    foreach (var item in result)
                    {
                        var permission = _context.MobileScreenPermissionModel.Where(p => roleIdList.Contains(p.RolesId)
                                                                                        && p.MobileScreenId == item.MenuId
                                                                                        && p.FunctionId == "M_CREATE")
                                                                             .FirstOrDefault();
                        if (permission != null)
                        {
                            item.isHasCreateTaskPermission = true;
                        }
                    }
                }
            }
            return result;
        }
    }
}
