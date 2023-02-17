using ISD.Core;
using ISD.Repositories;
using ISD.ViewModels.Marketing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ISD.API.Controllers
{
    public class EmailsController : BaseController
    {
        public ActionResult CheckIn(string Id, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                var result = new CheckInViewModel();
                Guid id = Guid.Parse(Id);
                result = new EmailsRepository(_context).CheckIn(id);
                if (result != null)
                {
                    return _APISuccess(result);
                }
                return _APIError("Không tìm thấy thư mời!");
            });
        }

        public ActionResult UpdateQuantity(string Id, int Quantity, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                var result = new CheckInViewModel();
                Guid id = Guid.Parse(Id);
                new EmailsRepository(_context).UpdateQuantity(id, Quantity);

                return _APISuccess(null, "Xác nhận thông tin checkin thành công!");
            });
        }
    }
}