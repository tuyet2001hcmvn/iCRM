using ISD.Core;
using System;
using System.Linq;
using System.Web.Mvc;

namespace MasterData.Controllers
{
    public class ProductController : BaseController
    {
        // GET: Product
        public ActionResult Index()
        {
            CreateViewBag();
            return View();
        }
        public ActionResult _Search(string productCode = "", string productName = "", Guid? categoryId = null, bool? Actived = null)
        {
            return ExecuteSearch(() =>
            {
                var productList = (from p in _context.ProductModel
                                   where (productCode == "" || p.ProductCode.Contains(productCode))
                                   && (productName == "" || p.ProductName.Contains(productCode))
                                   && (categoryId == null || p.CategoryId == categoryId)
                                   && (Actived == null || p.Actived == Actived)
                                   select p).ToList();
                return PartialView(productList);
            });
        }

        private void CreateViewBag()
        {
            var catagoryList = _context.CategoryModel.Where(p => p.Actived == true).ToList();

            ViewBag.CategoryId = new SelectList(catagoryList, "CategoryId", "CategoryName");
        }
    }
}