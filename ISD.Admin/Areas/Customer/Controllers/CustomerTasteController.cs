using ISD.Core;
using ISD.Repositories;
using System;
using System.Web.Mvc;

namespace Customer.Controllers
{
    public class CustomerTasteController : BaseController
    {
        public ActionResult _List(Guid? id, bool? isLoadContent = false)
        {
            return ExecuteSearch(() =>
            {
                var customerTasteList = _unitOfWork.CustomerTasteRepository.GetTastes(id);
                if (isLoadContent == true)
                {
                    return PartialView("_ListContent", customerTasteList);
                }
                return PartialView(customerTasteList);
            });
        }
    }
}