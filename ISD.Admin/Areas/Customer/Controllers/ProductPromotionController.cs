using ISD.Constant;
using ISD.Core;
using ISD.EntityModels;
using ISD.Repositories;
using ISD.Resources;
using ISD.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Customer.Controllers
{
    public class ProductPromotionController : BaseController
    {
        private ProductPromotionRepository _productPromotionRepository;

        public ProductPromotionController()
        {
            _productPromotionRepository = new ProductPromotionRepository(_context);
        }
        // GET: Activities
        public ActionResult _List(Guid id, bool? isLoadContent = false)
        {
            return ExecuteSearch(() =>
            {
                var result = new List<ProductPromotionViewModel>();
                result = _productPromotionRepository.GetByProfile(id);

                if (isLoadContent == true)
                {
                    return PartialView("_ListContent", result);
                }

                return PartialView(result);
            });
        }
    }
}