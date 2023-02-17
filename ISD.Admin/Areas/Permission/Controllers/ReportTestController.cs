using ISD.Constant;
using ISD.EntityModels;
using ISD.Resources;
using ISD.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using ISD.Core;

namespace Permission.Controllers
{
    public class ReportTestController : BaseController
    {
        const int startIndex = 4;
        const string controller = ConstExcelController.Menu;
        // GET: ReportTest
        public ActionResult Index(HttpPostedFileBase importexcelfile)
        {
            return View();
        }


        public ActionResult ImportExcelMTDynamic()
        {
            DataSet ds = GetDataSetFromExcel();
            List<string> errorList = new List<string>();
            return ExcuteImportExcel(() =>
            {
                if (ds.Tables.Count > 0)
                {
                    using (TransactionScope ts = new TransactionScope())
                    {
                        foreach (DataTable dt in ds.Tables)
                        {
                            //Get controller code from Excel file
                            string controllerCode = dt.Columns[0].ColumnName.ToString();
                            //Import data with accordant controller and action
                            if (controllerCode == controller)
                            {
                                foreach (DataRow dr in dt.Rows)
                                {
                                    if (dt.Rows.IndexOf(dr) >= startIndex && !string.IsNullOrEmpty(dr.ItemArray[0].ToString()))
                                    {
                                        //Check correct template
                                        MenuViewModel menuIsValid = CheckTemplate(dr.ItemArray);

                                        if (!string.IsNullOrEmpty(menuIsValid.Error))
                                        {
                                            string error = menuIsValid.Error;
                                            errorList.Add(error);
                                        }
                                        else
                                        {
                                            string result = ExecuteImportExcelMenu(menuIsValid);
                                            if (result != LanguageResource.ImportSuccess)
                                            {
                                                errorList.Add(result);
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                string error = string.Format(LanguageResource.Validation_ImportCheckController, LanguageResource.Permission_MenuModel);
                                errorList.Add(error);
                            }

                        }
                        if (errorList != null && errorList.Count > 0)
                        {
                            return Json(new
                            {
                                Code = System.Net.HttpStatusCode.Created,
                                Success = false,
                                Data = errorList
                            });
                        }
                        ts.Complete();
                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.Created,
                            Success = true,
                            Data = LanguageResource.ImportSuccess
                        });
                    }
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.Created,
                        Success = false,
                        Data = LanguageResource.Validation_ImportExcelFile
                    });
                }
            });

        }

        public string ExecuteImportExcelMenu(MenuViewModel menuIsValid)
        {
            //Check:
            //1. If MenuId == "" then => Insert
            //2. Else then => Update
            if (menuIsValid.isNullValueId == true)
            {
                MenuModel menu = new MenuModel();
                menu.MenuId = Guid.NewGuid();
                menu.MenuName = menuIsValid.MenuName;
                menu.OrderIndex = menuIsValid.OrderIndex;
                menu.Icon = menuIsValid.Icon;

                _context.Entry(menu).State = EntityState.Added;
            }
            else
            {
                MenuModel menu = _context.MenuModel.Where(p => p.MenuId == menuIsValid.MenuId).FirstOrDefault();
                if (menu != null)
                {
                    if (menu.MenuName != menuIsValid.MenuName)
                    {
                        menu.MenuName = menuIsValid.MenuName;
                    }
                    if (menu.OrderIndex != menuIsValid.OrderIndex)
                    {
                        menu.OrderIndex = menuIsValid.OrderIndex;
                    }
                    if (menu.Icon != menuIsValid.Icon)
                    {
                        menu.Icon = menuIsValid.Icon;
                    }

                    _context.Entry(menu).State = EntityState.Modified;
                }
                else
                {
                    return string.Format(LanguageResource.Validation_ImportExcelIdNotExist,
                                            LanguageResource.MenuId, menuIsValid.MenuId,
                                            string.Format(LanguageResource.Export_ExcelHeader,
                                            LanguageResource.Permission_MenuModel));
                }
            }
            _context.SaveChanges();
            return LanguageResource.ImportSuccess;
        }

        public MenuViewModel CheckTemplate(object[] row)
        {
            MenuViewModel menuVM = new MenuViewModel();
            for (int i = 0; i <= row.Length; i++)
            {
                //Row index
                if (i == 0)
                {
                    int rowIndex = int.Parse(row[i].ToString());
                    menuVM.RowIndex = rowIndex;
                }
                //MenuId
                else if (i == 1)
                {
                    string menuId = row[i].ToString();
                    if (string.IsNullOrEmpty(menuId))
                    {
                        menuVM.isNullValueId = true;
                    }
                    else
                    {
                        menuVM.MenuId = Guid.Parse(menuId);
                        menuVM.isNullValueId = false;
                    }
                }
                //MenuName
                else if (i == 2)
                {
                    string menuName = row[i].ToString();
                    if (string.IsNullOrEmpty(menuName))
                    {
                        menuVM.Error = string.Format(LanguageResource.Validation_ImportRequired, LanguageResource.MenuName, menuVM.RowIndex);
                    }
                    else
                    {
                        menuVM.MenuName = menuName;
                    }
                }
                //OrderIndex
                else if (i == 3)
                {
                    int orderIndex = string.IsNullOrEmpty(row[i].ToString()) ? 0 : int.Parse(row[i].ToString());
                    menuVM.OrderIndex = orderIndex;
                }
                //Icon
                else if (i == 4)
                {
                    menuVM.Icon = row[i].ToString();
                }
            }


            return menuVM;
        }

    }
}