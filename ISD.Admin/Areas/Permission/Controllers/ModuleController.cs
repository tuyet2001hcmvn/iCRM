using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using ISD.Constant;
using ISD.EntityModels;
using ISD.Extensions;
using ISD.Resources;
using ISD.ViewModels;
using ISD.Core;
using ISD.Repositories.Excel;

namespace Permission.Controllers
{
    public class ModuleController : BaseController
    {
        const string controllerCode = ConstExcelController.Module;
        const int startIndex = 6;
        //GET: /Module/Index
        #region Index, _Search
        [ISDAuthorizationAttribute]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult _Search(string ModuleName)
        {
            return ExecuteSearch(() =>
            {
                var ModuleNameIsNullOrEmpty = string.IsNullOrEmpty(ModuleName);
                var moduleList = _context.ModuleModel.Where(p => ModuleNameIsNullOrEmpty || p.ModuleName.ToLower().Contains(ModuleName.ToLower()))
                                                 .OrderBy(p => p.OrderIndex)
                                                 .ToList();
                return PartialView(moduleList);
            });
        }
        #endregion

        //GET: /Module/Create
        #region Create
        [ISDAuthorizationAttribute]
        public ActionResult Create()
        {
            return View();
        }
        //POST: Create
        [HttpPost]
        [ValidateAjax]
        [ISDAuthorizationAttribute]
        public JsonResult Create(ModuleModel model, HttpPostedFileBase file)
        {
            return ExecuteContainer(() =>
            {
                model.ModuleId = Guid.NewGuid();
                if (file != null)
                {
                    model.ImageUrl = Upload(file, "Module");
                }
                else
                {
                    model.ImageUrl = "noimage.jpg";
                }
                _context.Entry(model).State = EntityState.Added;
                _context.SaveChanges();

                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.Permission_ModuleModel.ToLower())
                });
            });
        }
        #endregion

        //GET: /Module/Edit
        #region Edit
        [ISDAuthorizationAttribute]
        public ActionResult Edit(Guid id)
        {
            var module = (from p in _context.ModuleModel
                          where p.ModuleId == id
                          select new ModuleViewModel()
                          {
                              ModuleId = p.ModuleId,
                              ModuleName = p.ModuleName,
                              Icon = p.Icon,
                              isSystemModule = p.isSystemModule,
                              OrderIndex = p.OrderIndex,
                              ImageUrl = p.ImageUrl,
                              Description = p.Description,
                              Details = p.Details,
                              Url = p.Url,
                          })
                          .FirstOrDefault();

            if (module == null)
            {
                return RedirectToAction("ErrorText", "Error", new { area = "", statusCode = 404, exception = string.Format(LanguageResource.Error_NotExist, LanguageResource.Permission_ModuleModel.ToLower()) });
            }

            module.ActivedPageList = (from p in _context.PageModel
                                      join m in _context.Page_Module_Mapping on p.PageId equals m.PageId
                                      where m.ModuleId == module.ModuleId
                                      select p).ToList();

            //Get list permission menu
            List<MenuModel> menuLst = new List<MenuModel>();
            if (module.ActivedPageList != null && module.ActivedPageList.Count > 0)
            {
                foreach (var item in module.ActivedPageList)
                {
                    var menu = (from p in _context.MenuModel
                                join s in _context.PageModel on p.MenuId equals s.MenuId
                                where s.PageId == item.PageId
                                select p).FirstOrDefault();
                    menuLst.Add(menu);
                }
                menuLst = menuLst.Distinct().OrderBy(p => p.OrderIndex).ToList();
            }
            ViewBag.ActivedMenuList = menuLst;

            CreateViewBag();
            return View(module);
        }
        //POST: Edit
        [HttpPost]
        [ISDAuthorizationAttribute]
        public JsonResult Edit(ModuleModel model, HttpPostedFileBase file, List<Guid> MenuId, List<Guid> PageId)
        {
            return ExecuteContainer(() =>
            {
                if (file != null)
                {
                    model.ImageUrl = Upload(file, "Module");
                }
                _context.Entry(model).State = EntityState.Modified;

                #region Check Company has store value
                List<Guid> tempMenuIdId = new List<Guid>();
                if (PageId != null)
                {
                    var exist_mapping = _context.Page_Module_Mapping
                                                    .Where(p => p.ModuleId == model.ModuleId)
                                                    .ToList();
                    _context.Page_Module_Mapping.RemoveRange(exist_mapping);

                    foreach (var item in PageId)
                    {
                        var menu = _context.PageModel.Where(p => p.PageId == item).Select(p => p.MenuId).FirstOrDefault();
                        tempMenuIdId.Add((Guid)menu);

                        Page_Module_Mapping mapping = new Page_Module_Mapping();
                        mapping.ModuleId = model.ModuleId;
                        mapping.PageId = item;
                        _context.Entry(mapping).State = EntityState.Added;
                    }
                    //ManyToManyPage(model, PageId);
                }
                tempMenuIdId = tempMenuIdId.Distinct().ToList();
                if (MenuId != null && !MenuId.SequenceEqual(tempMenuIdId))
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = "Vui lòng chọn trang sau khi chọn danh mục!"
                    });
                }
                #endregion

                _context.SaveChanges();

                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.Permission_ModuleModel.ToLower())
                });
            });
        }
        #endregion

        //GET: /Module/Delete
        #region Delete
        [HttpPost]
        [ISDAuthorizationAttribute]
        public ActionResult Delete(Guid id)
        {
            return ExecuteDelete(() =>
            {
                var module = _context.ModuleModel.FirstOrDefault(p => p.ModuleId == id);
                if (module != null)
                {
                    _context.Entry(module).State = EntityState.Deleted;
                    _context.SaveChanges();

                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.Created,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.Permission_ModuleModel.ToLower())
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = ""
                    });
                }
            });
        }
        #endregion

        #region Helper
        //Get company by permission menu
        public void CreateViewBag()
        {
            List<MenuModel> lst = new List<MenuModel>();
            var menuLst = (from m in _context.MenuModel
                           join p in _context.PageModel on m.MenuId equals p.MenuId
                           where p.Actived == true
                           select m)
                           .GroupBy(p => new { p.MenuId, p.MenuName, p.OrderIndex })
                           .Select(p => p.Key)
                           .OrderBy(p => p.OrderIndex).ToList();

            if (menuLst != null && menuLst.Count > 0)
            {
                foreach (var item in menuLst)
                {
                    lst.Add(new MenuModel() { MenuId = item.MenuId, MenuName = item.MenuName });
                }
            }
            ViewBag.MenuId = lst;
        }
        //GetPageByMenu
        public ActionResult GetPageByMenu(Guid MenuId)
        {
            var pageLst = _context.PageModel.Where(p => p.MenuId == MenuId && p.Actived == true)
                                         .Select(p =>
                                         new
                                         {
                                             PageId = p.PageId,
                                             PageName = p.PageName,
                                             OrderIndex = p.OrderIndex
                                         })
                                         .OrderBy(p => p.OrderIndex).ToList();
            return Json(pageLst, JsonRequestBehavior.AllowGet);
        }
        #endregion

        //GET: /Module/ExportToExcel
        #region Export to excel
        public ActionResult ExportCreate()
        {
            List<ModuleModel> module = new List<ModuleModel>();
            return Export(module);
        }

        public ActionResult ExportEdit()
        {
            //Get data from server
            List<ModuleModel> Module = _context.ModuleModel.OrderBy(p => p.OrderIndex).ToList();
            return Export(Module);
        }

        public FileContentResult Export(List<ModuleModel> module)
        {
            //Columns to take
            //string[] columns = { "ModuleId", "ModuleName", "OrderIndex", "Icon" };
            List<ExcelTemplate> columns = new List<ExcelTemplate>();
            columns.Add(new ExcelTemplate() { ColumnName = "ModuleId", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate() { ColumnName = "ModuleName", isAllowedToEdit = true });
            columns.Add(new ExcelTemplate() { ColumnName = "OrderIndex", isAllowedToEdit = true });
            columns.Add(new ExcelTemplate() { ColumnName = "Icon", isAllowedToEdit = true });
            //Header
            string fileheader = string.Format(LanguageResource.Export_ExcelHeader, LanguageResource.Permission_ModuleModel);

            //List<ExcelHeadingTemplate> heading initialize in BaseController
            //Default:
            //          1. heading[0] is controller code
            //          2. heading[1] is file name
            //          3. headinf[2] is warning (edit)
            heading.Add(new ExcelHeadingTemplate()
            {
                Content = controllerCode,
                RowsToIgnore = 1,
                isWarning = false,
                isCode = true
            });
            heading.Add(new ExcelHeadingTemplate()
            {
                Content = fileheader.ToUpper(),
                RowsToIgnore = 1,
                isWarning = false,
                isCode = false
            });
            heading.Add(new ExcelHeadingTemplate()
            {
                Content = LanguageResource.Export_ExcelWarning1,
                RowsToIgnore = 1,
                isWarning = true,
                isCode = false
            });

            //Body
            byte[] filecontent = ClassExportExcel.ExportExcel(module, columns, heading, true);
            //File name
            string fileNameWithFormat = string.Format("{0}.xlsx", _unitOfWork.UtilitiesRepository.RemoveSign4VietnameseString(fileheader.ToUpper()).Replace(" ", "_"));

            return File(filecontent, ClassExportExcel.ExcelContentType, fileNameWithFormat);
        }
        #endregion Export to excel

        //POST: /Module/ImportFromExcel
        #region Import from excel
        public ActionResult Import()
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
                            string contCode = dt.Columns[0].ColumnName.ToString();
                            //Import data with accordant controller and action
                            if (contCode == controllerCode)
                            {
                                foreach (DataRow dr in dt.Rows)
                                {
                                    if (dt.Rows.IndexOf(dr) >= startIndex && !string.IsNullOrEmpty(dr.ItemArray[0].ToString()))
                                    {
                                        //Check correct template
                                        ModuleViewModel moduleIsValid = CheckTemplate(dr.ItemArray);

                                        if (!string.IsNullOrEmpty(moduleIsValid.Error))
                                        {
                                            string error = moduleIsValid.Error;
                                            errorList.Add(error);
                                        }
                                        else
                                        {
                                            string result = ExecuteImportExcelModule(moduleIsValid);
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
                                string error = string.Format(LanguageResource.Validation_ImportCheckController, LanguageResource.Permission_ModuleModel);
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

        #region Insert/Update data from excel file
        public string ExecuteImportExcelModule(ModuleViewModel moduleIsValid)
        {
            //Check:
            //1. If ModuleId == "" then => Insert
            //2. Else then => Update
            if (moduleIsValid.isNullValueId == true)
            {
                ModuleModel module = new ModuleModel();
                module.ModuleId = Guid.NewGuid();
                module.ModuleName = moduleIsValid.ModuleName;
                module.OrderIndex = moduleIsValid.OrderIndex;
                module.Icon = moduleIsValid.Icon;

                _context.Entry(module).State = EntityState.Added;
            }
            else
            {
                ModuleModel module = _context.ModuleModel.Where(p => p.ModuleId == moduleIsValid.ModuleId).FirstOrDefault();
                if (module != null)
                {
                    if (module.ModuleName != moduleIsValid.ModuleName)
                    {
                        module.ModuleName = moduleIsValid.ModuleName;
                    }
                    if (module.OrderIndex != moduleIsValid.OrderIndex)
                    {
                        module.OrderIndex = moduleIsValid.OrderIndex;
                    }
                    if (module.Icon != moduleIsValid.Icon)
                    {
                        module.Icon = moduleIsValid.Icon;
                    }
                    _context.Entry(module).State = EntityState.Modified;
                }
                else
                {
                    return string.Format(LanguageResource.Validation_ImportExcelIdNotExist,
                                            LanguageResource.ModuleId, moduleIsValid.ModuleId,
                                            string.Format(LanguageResource.Export_ExcelHeader,
                                            LanguageResource.Permission_ModuleModel));
                }
            }
            _context.SaveChanges();
            return LanguageResource.ImportSuccess;
        }
        #endregion Insert/Update data from excel file

        #region Check data type 
        public ModuleViewModel CheckTemplate(object[] row)
        {
            ModuleViewModel moduleVM = new ModuleViewModel();
            for (int i = 0; i <= row.Length; i++)
            {
                //Row index
                if (i == 0)
                {
                    int rowIndex = int.Parse(row[i].ToString());
                    moduleVM.RowIndex = rowIndex;
                }
                //ModuleId
                else if (i == 1)
                {
                    string moduleId = row[i].ToString();
                    if (string.IsNullOrEmpty(moduleId))
                    {
                        moduleVM.isNullValueId = true;
                    }
                    else
                    {
                        moduleVM.ModuleId = Guid.Parse(moduleId);
                        moduleVM.isNullValueId = false;
                    }
                }
                //ModuleName
                else if (i == 2)
                {
                    string moduleName = row[i].ToString();
                    if (string.IsNullOrEmpty(moduleName))
                    {
                        moduleVM.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, LanguageResource.ModuleName), moduleVM.RowIndex);
                    }
                    else
                    {
                        moduleVM.ModuleName = moduleName;
                    }
                }
                //OrderIndex
                else if (i == 3)
                {
                    int orderIndex = string.IsNullOrEmpty(row[i].ToString()) ? 0 : int.Parse(row[i].ToString());
                    moduleVM.OrderIndex = orderIndex;
                }
                //Icon
                else if (i == 4)
                {
                    moduleVM.Icon = row[i].ToString();
                }
            }
            return moduleVM;
        }
        #endregion Check data type

        #endregion Import from excel
    }
}