using ISD.ViewModels;
using ISD.Core;
using System;
using System.Linq;
using System.Web.Mvc;
using ISD.Constant;

namespace Utilities.Controllers
{
    public class ChangeDataLogController : BaseController
    {
        // GET: ChangeDataLog
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult _List(Guid id, string Type = null)
        {
            var log = (from p in _context.ChangeDataLogModel
                       join acc in _context.AccountModel on p.LastEditBy equals acc.AccountId
                       join empTemp in _context.SalesEmployeeModel on acc.EmployeeCode equals empTemp.SalesEmployeeCode into empList
                       from emp in empList.DefaultIfEmpty()
                       where p.PrimaryKey == id
                       orderby p.LastEditTime descending
                       select new ChangeDataLogViewModel()
                       {
                           LogId = p.LogId,
                           TableName = p.TableName,
                           PrimaryKey = p.PrimaryKey,
                           FieldName = p.FieldName,
                           OldData = p.OldData,
                           NewData = p.NewData,
                           LastEditBy = p.LastEditBy,
                           LastEditUser = emp.SalesEmployeeShortName??acc.FullName??acc.UserName,
                           LastEditTime = p.LastEditTime
                       }).ToList();
            
            if (Type == ConstProfileType.Opportunity && log != null && log.Count() > 0)
            {
                log = log.Where(x => x.FieldName == "CountryCode" || 
                                     x.FieldName == "CustomerSourceCode" || 
                                     x.FieldName == "CreateAtSaleOrg" || 
                                     x.FieldName == "ProfileShortName" || 
                                     x.FieldName == "ReconcileAccountCode" || 
                                     x.FieldName == "Address" || 
                                     x.FieldName == "SaleOfficeCode" || 
                                     x.FieldName == "Title" || 
                                     x.FieldName == "CreateAtCompany" || 
                                     x.FieldName == "AddressTypeCode" || 
                                     x.FieldName == "ProfileName" || 
                                     x.FieldName == "Note" || 
                                     x.FieldName == "Phone" || 
                                     x.FieldName == "TaxNo" || 
                                     x.FieldName == "Email" || 
                                     x.FieldName == "Text10" || 
                                     x.FieldName == "Number4" || 
                                     x.FieldName == "Actived" || 
                                     x.FieldName == "ProjectLocation"
                                     ).ToList();
            }
            if (Type != null)
            {
                var configList = _context.ProfileConfigModel.Where(p => p.ProfileCategoryCode == Type).ToList();
                if (configList != null && configList.Count > 0)
                {
                    foreach (var item in configList)
                    {
                        if (string.IsNullOrEmpty(item.Note))
                        {
                            item.Note = PropertyHelper.GetDisplayNameByString<ProfileViewModel>(item.FieldCode);
                        }
                    }
                }
                ViewBag.ProfileConfig = configList;
                ViewBag.ProfileConfigCode = configList.Select(p => p.FieldCode).ToList();
            }
            return PartialView(log);
        }
    }
}