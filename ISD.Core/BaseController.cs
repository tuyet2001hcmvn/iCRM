using System;
using System.Collections.Generic;
using System.Linq;
using ISD.EntityModels;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Infrastructure;
using ISD.Resources;
using System.Security.Claims;
using ISD.ViewModels;
using System.Data;
using System.IO;
using ISD.Constant;
using System.Data.Entity.Validation;
using ISD.Repositories;
using ISD.Repositories.Excel;
using System.Globalization;

namespace ISD.Core
{
    public class BaseController : System.Web.Mvc.Controller
    {
        //Entity
        public EntityDataContext _context;
        public UnitOfWork _unitOfWork;
        public List<ExcelHeadingTemplate> heading = new List<ExcelHeadingTemplate>();

        protected BaseController()
        {
            _context = new EntityDataContext();
            _context.Database.CommandTimeout = 1800;
            _unitOfWork = new UnitOfWork(_context);
        }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            GetMenuList();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            base.Dispose(disposing);
        }

        protected JsonResult ValidationInvalid()
        {
            var errorList = ModelState.Values.SelectMany(v => v.Errors).ToList();
            ModelState.Clear();
            foreach (var error in errorList)
            {
                if (string.IsNullOrEmpty(error.ErrorMessage) && error.Exception != null)
                {
                    ModelState.AddModelError("", error.Exception.Message);
                }
                else
                {
                    ModelState.AddModelError("", error.ErrorMessage);
                }
            }
            return Json(new
            {
                Code = System.Net.HttpStatusCode.InternalServerError,
                Success = false,
                Data = ModelState.Values.SelectMany(v => v.Errors).ToList()
            });
        }

        protected Guid GetPageId(string PageUrl, string Parameter = null)
        {
            var page = _context.PageModel.Where(p => p.PageUrl == PageUrl && (Parameter == null || Parameter == "" || p.Parameter == Parameter) && p.Actived == true).FirstOrDefault();
            if (page == null)
            {
                return Guid.Empty;
            }
            return page.PageId;
        }

        #region Execute
        protected JsonResult ExecuteContainer(Func<JsonResult> codeToExecute)
        {
            //1. using: ModelState.IsValid
            if (ModelState.IsValid)
            {
                try
                {
                    // All code will run here
                    // Usage: return ExecuteContainer(() => { ALL RUNNING CODE HERE, remember to return });
                    return codeToExecute.Invoke();
                }
                //2. handle: DbUpdateException
                catch (DbUpdateException ex)
                {
                    foreach (var errorMessage in ErrorHepler.GetaAllMessages(ex))
                    {
                        ModelState.AddModelError("", errorMessage);
                    }
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = ModelState.Values.SelectMany(v => v.Errors)
                    });
                }
                //3. Validation db
                catch (DbEntityValidationException e)
                {
                    var errorList = e.EntityValidationErrors;
                    ModelState.Clear();
                    foreach (var error in errorList)
                    {
                        foreach (var ve in error.ValidationErrors)
                        {
                            ModelState.AddModelError("", ve.ErrorMessage);
                        }
                    }
                }
                //4. handle: Other Exception
                catch (Exception ex)
                {
                    var messs = ex.Message;
                    if (ex.InnerException != null)
                    {
                        if (ex.InnerException.InnerException != null)
                        {
                            messs = ex.InnerException.InnerException.Message;
                        }
                    }
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = messs
                    });
                }
            }//4. using: ValidationInvalid()
            return ValidationInvalid();
        }

        protected ActionResult ExecuteSearch(Func<ActionResult> codeToExecute)
        {
            try
            {
                return codeToExecute.Invoke();
            }
            catch (Exception ex)
            {
                var messs = ex.Message;
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.InnerException != null)
                    {
                        messs = ex.InnerException.InnerException.Message;
                    }
                }
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.InternalServerError,
                    Success = false,
                    Data = messs
                });
            }
        }

        protected JsonResult ExecuteDelete(Func<JsonResult> codeToExecute)
        {
            try
            {
                // All code will run here
                // Usage: return ExecuteContainer(() => { ALL RUNNING CODE HERE, remember to return });
                return codeToExecute.Invoke();
            }
            //1. handle: DbUpdateException
            catch (DbUpdateException ex)
            {
                foreach (var errorMessage in ErrorHepler.GetaAllMessages(ex))
                {
                    ModelState.AddModelError("", errorMessage);
                }
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.NotModified,
                    Success = false,
                    Data = ModelState.Values.SelectMany(v => v.Errors)
                });
            }
            //2. handle: Exception
            catch (Exception ex)
            {
                var messs = ex.Message;
                if (ex.InnerException != null)
                {
                    messs = ex.InnerException.InnerException.Message;
                }
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.NotModified,
                    Success = false,
                    Data = messs
                });
            }
        }
        #endregion Execute

        #region Language
        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            string lang = null;
            HttpCookie langCookie = Request.Cookies["culture"];
            if (langCookie != null)
            {
                lang = langCookie.Value;
            }
            else
            {
                var userLanguage = Request.UserLanguages;
                var userLang = userLanguage != null ? userLanguage[0] : "";
                if (userLang != "")
                {
                    lang = userLang;
                }
                else
                {
                    lang = MultiLanguage.GetDefaultLanguage();
                }
            }

            new MultiLanguage().SetLanguage(lang);
            return base.BeginExecuteCore(callback, state);
        }
        #endregion Language

        #region Permission
        public AppUserPrincipal CurrentUser
        {
            get
            {
                return new AppUserPrincipal(this.User as ClaimsPrincipal);
            }
        }

        public bool isDeveloper
        {
            get
            {
                var accountId = CurrentUser.AccountId;
                var currentAccount = _context.AccountModel.Where(p => p.AccountId == accountId).FirstOrDefault();
                if (currentAccount.RolesModel != null && currentAccount.RolesModel.Count > 0)
                {
                    var role = currentAccount.RolesModel.Where(p => p.OrderIndex == 0).FirstOrDefault();
                    if (role != null)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        public void GetMenuList()
        {
            if (Session["Menu"] == null)
            {
                PermissionViewModel permission = null;
                //check login
                //just get user identity AFTER login -> must set function GetMenuList in Home/Index
                if (User.Identity.IsAuthenticated && !string.IsNullOrEmpty(CurrentUser.UserName))
                {
                    permission = new PermissionViewModel();
                    //using dataset to get multiple table in store procedure: page, menu, page permission
                    DataSet ds = new DataSet();
                    using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["cnStr"].ConnectionString))
                    {
                        using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand())
                        {
                            cmd.CommandText = "pms.QTHT_PagePermission_GetPagePermissionByAccountId";
                            cmd.Parameters.AddWithValue("@AccountId", CurrentUser.AccountId);
                            cmd.Connection = conn;
                            cmd.CommandType = CommandType.StoredProcedure;
                            conn.Open();
                            System.Data.SqlClient.SqlDataAdapter adapter = new System.Data.SqlClient.SqlDataAdapter(cmd);
                            adapter.Fill(ds);
                            conn.Close();
                        }
                    }
                    ds.Tables[0].TableName = "PageModel";
                    ds.Tables[1].TableName = "MenuModel";
                    ds.Tables[2].TableName = "PagePermissionModel";
                    ds.Tables[3].TableName = "ModuleModel";

                    //Convert datatable into list
                    var pageList = (from p in ds.Tables[0].AsEnumerable()
                                    select new PageViewModel()
                                    {
                                        PageId = p.Field<Guid>("PageId"),
                                        PageName = p.Field<string>("PageName"),
                                        PageUrl = p.Field<string>("PageUrl"),
                                        Parameter = p.Field<string>("Parameter"),
                                        ModuleId = p.Field<Nullable<Guid>>("ModuleId"),
                                        MenuId = p.Field<Guid>("MenuId"),
                                        Icon = p.Field<string>("Icon"),
                                        Color = p.Field<string>("Color"),
                                    }).ToList();

                    var menuList = (from p in ds.Tables[1].AsEnumerable()
                                    select new MenuViewModel()
                                    {
                                        MenuId = p.Field<Guid>("MenuId"),
                                        MenuName = p.Field<string>("MenuName"),
                                        ModuleId = p.Field<Guid?>("ModuleId"),
                                        Icon = p.Field<string>("Icon")
                                    }).ToList();

                    var funcList = (from p in ds.Tables[2].AsEnumerable()
                                    select new PagePermissionViewModel()
                                    {
                                        PageId = p.Field<Guid>("PageId"),
                                        FunctionId = p.Field<string>("FunctionId"),
                                    }).ToList();

                    var moduleList = (from p in ds.Tables[3].AsEnumerable()
                                      select new ModuleViewModel()
                                      {
                                          ModuleId = p.Field<Guid>("ModuleId"),
                                          ModuleName = p.Field<string>("ModuleName"),
                                          isSystemModule = p.Field<bool?>("isSystemModule"),
                                          Icon = p.Field<string>("Icon"),
                                          ImageUrl = p.Field<string>("ImageUrl"),
                                      }).ToList();

                    //add list into model Permission
                    permission.PageModel = pageList;
                    permission.MenuModel = menuList;
                    permission.PagePermissionModel = funcList;
                    permission.ModuleModel = moduleList;
                }
                Session["Menu"] = permission;
            }
        }
        #endregion Permission

        #region Import Excel
        protected DataSet GetDataSetFromExcel()
        {
            DataSet ds = new DataSet();
            try
            {
                if (Request.Files.Count > 0)
                {
                    var file = Request.Files[0];
                    if (file != null && file.ContentLength > 0)
                    {
                        //Check file is excel
                        if (file.FileName.Contains("xls") || file.FileName.Contains("xlsx"))
                        {
                            var fileName = Path.GetFileName(file.FileName);
                            var mapPath = Server.MapPath("~/Upload/ImportExcel/");
                            if (!Directory.Exists(mapPath))
                            {
                                Directory.CreateDirectory(mapPath);
                            }
                            var path = Path.Combine(mapPath, fileName);
                            file.SaveAs(path);

                            using (ClassImportExcel excelHelper = new ClassImportExcel(path))
                            {
                                excelHelper.Hdr = "YES";
                                excelHelper.Imex = "1";
                                ds = excelHelper.ReadDataSet();
                            }
                        }
                    }
                }
                return ds;
            }
            //handle: Exception
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected DataSet GetDataSetFromExcelNew(bool? isUseHeaderRow = null)
        {
            DataSet ds = new DataSet();
            try
            {
                if (Request.Files.Count > 0)
                {
                    var file = Request.Files[0];
                    if (file != null && file.ContentLength > 0)
                    {
                        //Check file is excel
                        if (file.FileName.Contains("xls") || file.FileName.Contains("xlsx"))
                        {
                            var fileName = Path.GetFileName(file.FileName);
                            var mapPath = Server.MapPath("~/Upload/ImportExcel/");
                            if (!Directory.Exists(mapPath))
                            {
                                Directory.CreateDirectory(mapPath);
                            }
                            var path = Path.Combine(mapPath, fileName);
                            file.SaveAs(path);

                            ImportExcelHelper excelHelper = new ImportExcelHelper(path);
                            ds = excelHelper.GetDataSet(true);
                            
                        }
                    }
                }
                return ds;
            }
            //handle: Exception
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected ActionResult ExcuteImportExcel(Func<JsonResult> codeToExecute)
        {
            try
            {
                // All code will run here
                // Usage: return ExecuteContainer(() => { ALL RUNNING CODE HERE, remember to return });
                return codeToExecute.Invoke();
            }
            //1. handle: DbUpdateException
            catch (DbUpdateException ex)
            {
                string Message = "";
                foreach (var errorMessage in ErrorHepler.GetaAllMessages(ex))
                {
                    //ModelState.AddModelError("", errorMessage);
                    Message += ex.InnerException.InnerException.Message + "<br />";
                }
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.NotModified,
                    Success = false,
                    Data = Message//ModelState.Values.SelectMany(v => v.Errors)
                });
            }
            //3. Validation db
            //catch (DbEntityValidationException e)
            //{
            //    var errorList = e.EntityValidationErrors;
            //    ModelState.Clear();
            //    foreach (var error in errorList)
            //    {
            //        foreach (var ve in error.ValidationErrors)
            //        {
            //            ModelState.AddModelError("", ve.ErrorMessage);
            //        }
            //    }
            //    return Json(new
            //    {
            //        Code = System.Net.HttpStatusCode.NotModified,
            //        Success = false,
            //        Data = ModelState.Values.SelectMany(v => v.Errors)
            //    });
            //}
            //2. handle: Exception
            catch (Exception ex)
            {
                string Message = "";
                if (ex.InnerException != null)
                {
                    Message = ex.InnerException.Message;

                    if (ex.InnerException.InnerException != null)
                    {
                        Message += ex.InnerException.InnerException.Message;
                    }
                }
                else
                {
                    Message = ex.Message;
                }
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.NotModified,
                    Success = false,
                    Data = "Lỗi: " + Message
                });
            }
        }

        protected bool AreAllColumnsEmpty(DataRow dr)
        {
            if (dr == null)
            {
                return true;
            }
            else
            {
                foreach (var value in dr.ItemArray)
                {
                    if (value != null)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        #region GetTypeFunction
        protected dynamic GetTypeFunction<T>(string value, int index)
        {
            if (typeof(T) == typeof(bool))
            {
                if (string.IsNullOrEmpty(value))
                {
                    return false;
                }
                else
                {
                    if (value.ToUpper() == "FALSE")
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            else if (typeof(T) == typeof(DateTime))
            {
                if (string.IsNullOrEmpty(value))
                {
                    return null;
                }
                else
                {
                    //DateTime result = DateTime.Parse(_unitOfWork.UtilitiesRepository.ConvertToDateTime(value.ToString()));
                    DateTime result = DateTime.ParseExact(value.ToString(),
                                                              "dd/MM/yyyy",
                                                               CultureInfo.InvariantCulture);
                    return result;
                }
            }
            else if (typeof(T) == typeof(int))
            {
                if (string.IsNullOrEmpty(value))
                {
                    return null;
                }
                else
                {
                    return Convert.ToInt32(value);
                }
            }
            else if (typeof(T) == typeof(decimal?))
            {
                if (string.IsNullOrEmpty(value))
                {
                    return null;
                }
                else
                {
                    return Convert.ToDecimal(value);
                }
            }
            else if (typeof(T) == typeof(Guid))
            {
                if (string.IsNullOrEmpty(value))
                {
                    return null;
                }
                else
                {
                    return new Guid(value);
                }
            }
            else
            {
                return null;
            }
        }
        #endregion GetTypeFunction
        #endregion Import Excel

        #region Export Excel

        protected void CreateExportHeader(string fileheader, string controllerCode, List<string> noteList = null, bool? isHasHeader = true)
        {
            //Default:
            //1. heading[0] is controller code
            heading.Add(new ExcelHeadingTemplate()
            {
                Content = controllerCode,
                RowsToIgnore = 1,
                isWarning = false,
                isCode = true,
                isHasBorder = false,
            });
            //2. heading[1] is file name
            heading.Add(new ExcelHeadingTemplate()
            {
                Content = fileheader.ToUpper(),
                RowsToIgnore = 1,
                isWarning = false,
                isCode = false,
                isHasBorder = false,
            });
            if (isHasHeader == true)
            {
                //3. heading[2] is warning (edit)
                //Warning
                heading.Add(new ExcelHeadingTemplate()
                {
                    Content = LanguageResource.Export_ExcelWarning1,
                    RowsToIgnore = 0,
                    isWarning = true,
                    isCode = false,
                    isHasBorder = true,
                });

                heading.Add(new ExcelHeadingTemplate()
                {
                    Content = LanguageResource.Export_ExcelWarning2,
                    RowsToIgnore = noteList != null ? 0 : 1,
                    isWarning = true,
                    isCode = false,
                    isHasBorder = false,
                });
                int index = 0;
                if (noteList != null && noteList.Count > 0)
                {
                    int rowCount = noteList.Count;
                    foreach (var item in noteList)
                    {
                        index++;
                        heading.Add(new ExcelHeadingTemplate()
                        {
                            Content = item,
                            RowsToIgnore = rowCount == index ? 1 : 0,
                            isWarning = true,
                            isCode = false,
                            isHasBorder = false,
                        });
                    }
                }

            }

        }
        protected void CreateExportHeader(List<ExcelHeadingTemplate> heading2, string fileheader, string controllerCode)
        {
            //Default:
            //1. heading[0] is controller code
            heading2.Add(new ExcelHeadingTemplate()
            {
                Content = controllerCode,
                RowsToIgnore = 1,
                isWarning = false,
                isCode = true,
                isHasBorder = false,
            });
            //2. heading[1] is file name
            heading2.Add(new ExcelHeadingTemplate()
            {
                Content = fileheader.ToUpper(),
                RowsToIgnore = 1,
                isWarning = false,
                isCode = false,
                isHasBorder = false,
            });
            //3. heading[2] is warning (edit)
            //Warning
            heading2.Add(new ExcelHeadingTemplate()
            {
                Content = LanguageResource.Export_ExcelWarning1,
                RowsToIgnore = 0,
                isWarning = true,
                isCode = false,
                isHasBorder = true,
            });
            heading2.Add(new ExcelHeadingTemplate()
            {
                Content = LanguageResource.Export_ExcelWarning2,
                RowsToIgnore = 1,
                isWarning = true,
                isCode = false,
                isHasBorder = false,
            });
        }
        #endregion Export Excel

        #region Upload Image
        //Upload hình ảnh
        protected string Upload(HttpPostedFileBase file, string folder, int minWidth = 300, int maxWidth = 1600, int minHeight = 300, int maxHeight = 1600)
        {
            string ret = "";
            string parth = "";
            string thumparth = "";
            try
            {
                if (file != null && file.ContentLength > 0)
                {// nếu có chọn file

                    string filename = _unitOfWork.RepositoryLibrary.ConvertToNoMarkString(file.FileName);
                    string type = filename.Substring(filename.Length - 4);
                    //Nếu là jpeg thì đổi thành jpg 
                    if (type.ToLower() == "jpeg")
                    {
                        filename = filename.Substring(0, filename.Length - 5) + ".jpg";
                    }
                    //Đổi tên lại thành chuỗi phân biệt tránh trùng
                    filename = filename.Substring(0, filename.Length - 3) + "." + filename.Substring(filename.Length - 3);
                    string strName = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH'-'mm'-'ss") + filename;

                    type = filename.Substring(filename.Length - 3);

                    //folder path
                    var existPath = Server.MapPath("~/Upload/" + folder);
                    Directory.CreateDirectory(existPath);

                    var existThumbPath = Server.MapPath("~/Upload/" + folder + "/thum/");
                    Directory.CreateDirectory(existThumbPath);

                    //file path
                    parth = Server.MapPath("~/Upload/" + folder + "/" + strName);
                    thumparth = Server.MapPath("~/Upload/" + folder + "/thum/" + strName);

                    //gán giá trị trả về
                    ret = strName;

                    //Nếu không phải ảnh động hay ảnh trong suốt thì tiến hành resize
                    if (type.ToLower() != "gif" && type.ToLower() != "png" && type.ToLower() != "svg")
                    {
                        var img = System.Drawing.Image.FromStream(file.InputStream, true, true);
                        int w = img.Width;
                        int h = img.Height;
                        //save to root folder
                        if (w >= maxWidth || h >= maxHeight)
                        {
                            _unitOfWork.RepositoryLibrary.ResizeStream(maxWidth, maxHeight, file.InputStream, parth);
                        }
                        else
                        {
                            _unitOfWork.RepositoryLibrary.ResizeStream(w, h, file.InputStream, parth);
                        }
                        //save to thum
                        if (w >= minWidth || h >= minHeight)
                        {
                            _unitOfWork.RepositoryLibrary.ResizeStream(maxWidth, minHeight, file.InputStream, thumparth);
                        }
                        else
                        {
                            _unitOfWork.RepositoryLibrary.ResizeStream(w, h, file.InputStream, thumparth);
                        }
                    }
                    else
                    {
                        file.SaveAs(parth);
                        file.SaveAs(thumparth);
                    }
                }
                else
                {
                    ret = "noimage.jpg";
                }
            }
            catch
            {
                ret = "noimage.jpg";
            }
            return ret;
        }
        //Upload hình ảnh
        protected string UploadDocumentFile(HttpPostedFileBase file, string folder, int minWidth = 300, int maxWidth = 1600, int minHeight = 300, int maxHeight = 1600, string FileType = null)
        {
            string ret = string.Empty;
            string parth = string.Empty;
            string thumparth = string.Empty;
            try
            {
                if (file != null && file.ContentLength > 0)
                {// nếu có chọn file

                    int indexDot = file.FileName.LastIndexOf('.');
                    string filename = file.FileName.Substring(0, indexDot);

                    //Convert file name
                    filename = _unitOfWork.RepositoryLibrary.ConvertToNoMarkString(filename);
                    //get file type
                    string fileType = file.FileName.Substring(indexDot);

                    //Đổi tên lại thành chuỗi phân biệt tránh trùng
                    filename = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH'-'mm'-'ss") + filename;


                    //folder path
                    var existPath = Server.MapPath("~/Upload/" + folder);
                    Directory.CreateDirectory(existPath);

                    var existThumbPath = Server.MapPath("~/Upload/" + folder + "/thum/");
                    Directory.CreateDirectory(existThumbPath);

                    //file path
                    filename = filename + fileType;
                    parth = Server.MapPath("~/Upload/" + folder + "/" + filename);
                    thumparth = Server.MapPath("~/Upload/" + folder + "/thum/" + filename);

                    //gán giá trị trả về
                    ret = filename;
                    ////Save File
                    //file.SaveAs(parth);
                    //Nếu không phải ảnh động hay ảnh trong suốt thì tiến hành resize
                    if (FileType == ConstFileAttachmentCode.Image && fileType.ToLower() != ".gif" && fileType.ToLower() != ".png" && fileType.ToLower() != ".svg")
                    {
                        var img = System.Drawing.Image.FromStream(file.InputStream, true, true);
                        int w = img.Width;
                        int h = img.Height;
                        //save to root folder
                        if (w >= maxWidth || h >= maxHeight)
                        {
                            _unitOfWork.RepositoryLibrary.ResizeStream(maxWidth, maxHeight, file.InputStream, parth);
                        }
                        else
                        {
                            _unitOfWork.RepositoryLibrary.ResizeStream(w, h, file.InputStream, parth);
                        }
                        //save to thum
                        if (w >= minWidth || h >= minHeight)
                        {
                            _unitOfWork.RepositoryLibrary.ResizeStream(maxWidth, minHeight, file.InputStream, thumparth);
                        }
                        else
                        {
                            _unitOfWork.RepositoryLibrary.ResizeStream(w, h, file.InputStream, thumparth);
                        }
                    }
                    else
                    {
                        file.SaveAs(parth);
                        file.SaveAs(thumparth);
                    }
                }
                else
                {
                    ret = null;
                }
            }
            catch
            {
                ret = null;
            }
            return ret;
        }
        protected string UploadDocumentFile(HttpPostedFileBase file, string folder, string path, int minWidth = 300, int maxWidth = 1600, int minHeight = 300, int maxHeight = 1600, string FileType = null)
        {
            string ret = string.Empty;
            string parth = string.Empty;
            string thumparth = string.Empty;
            try
            {
                if (file != null && file.ContentLength > 0)
                {// nếu có chọn file

                    int indexDot = file.FileName.LastIndexOf('.');
                    string filename = file.FileName.Substring(0, indexDot);

                    //Convert file name
                    filename = _unitOfWork.RepositoryLibrary.ConvertToNoMarkString(filename);
                    //get file type
                    string fileType = file.FileName.Substring(indexDot);

                    //Đổi tên lại thành chuỗi phân biệt tránh trùng
                    filename = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH'-'mm'-'ss") + filename;


                    //folder path
                    var existPath = path + folder;
                    Directory.CreateDirectory(existPath);

                    var existThumbPath = path + folder + "/thum/";
                    Directory.CreateDirectory(existThumbPath);

                    //file path
                    filename = filename + fileType;
                    parth = path + folder + "/" + filename;
                    thumparth = path + folder + "/thum/" + filename;

                    //gán giá trị trả về
                    ret = filename;
                    ////Save File
                    //file.SaveAs(parth);
                    //Nếu không phải ảnh động hay ảnh trong suốt thì tiến hành resize
                    if (FileType == ConstFileAttachmentCode.Image && fileType.ToLower() != ".gif" && fileType.ToLower() != ".png" && fileType.ToLower() != ".svg")
                    {
                        var img = System.Drawing.Image.FromStream(file.InputStream, true, true);
                        int w = img.Width;
                        int h = img.Height;
                        //save to root folder
                        if (w >= maxWidth || h >= maxHeight)
                        {
                            _unitOfWork.RepositoryLibrary.ResizeStream(maxWidth, maxHeight, file.InputStream, parth);
                        }
                        else
                        {
                            _unitOfWork.RepositoryLibrary.ResizeStream(w, h, file.InputStream, parth);
                        }
                        //save to thum
                        if (w >= minWidth || h >= minHeight)
                        {
                            _unitOfWork.RepositoryLibrary.ResizeStream(maxWidth, minHeight, file.InputStream, thumparth);
                        }
                        else
                        {
                            _unitOfWork.RepositoryLibrary.ResizeStream(w, h, file.InputStream, thumparth);
                        }
                    }
                    else
                    {
                        file.SaveAs(parth);
                        file.SaveAs(thumparth);
                    }
                }
                else
                {
                    ret = null;
                }
            }
            catch
            {
                ret = null;
            }
            return ret;
        }
        //Upload hình ảnh lưu vào domain của website khác
        public string UploadRedirectToAPI(HttpPostedFileBase file)
        {
            string ret = "";
            try
            {
                if (file != null && file.ContentLength > 0)
                {// nếu có chọn file

                    string filename = _unitOfWork.RepositoryLibrary.ConvertToNoMarkString(file.FileName);
                    string type = filename.Substring(filename.Length - 4);
                    //Nếu là jpeg thì đổi thành jpg 
                    if (type.ToLower() == "jpeg")
                    {
                        filename = filename.Substring(0, filename.Length - 5) + ".jpg";
                    }
                    //Đổi tên lại thành chuỗi phân biệt tránh trùng
                    filename = filename.Substring(0, filename.Length - 3) + "." + filename.Substring(filename.Length - 3);
                    string strName = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH'-'mm'-'ss") + filename;

                    type = filename.Substring(filename.Length - 3);



                    //gán giá trị trả về
                    ret = strName;
                }
                else
                {
                    ret = "noimage.jpg";
                }
            }
            catch
            {
                ret = "noimage.jpg";
            }
            return ret;
        }
        //Upload hình ảnh không rename tên hình
        public string UploadNotRename(HttpPostedFileBase file, string folder)
        {
            var fileName = Path.GetFileName(file.FileName);

            //Create dynamically folder to save file
            var existPath = Server.MapPath(string.Format("~/Upload/{0}", folder));
            Directory.CreateDirectory(existPath);
            var path = Path.Combine(existPath, fileName);

            var existThumbPath = Server.MapPath("~/Upload/" + folder + "/thum/");
            Directory.CreateDirectory(existThumbPath);
            var thumbPath = Path.Combine(existThumbPath, fileName);

            file.SaveAs(path);
            file.SaveAs(thumbPath);

            return fileName;
        }
        #endregion

        #region API helper
        protected JsonResult ExecuteAPIContainer(string token, string key, Func<JsonResult> codeToExecute)
        {
            if (//(token == ConstDomain.tokenConst && key == ConstDomain.keyConst) ||
                (token == ConstDomain.tokenConst_New && key == ConstDomain.keyConst_New))
            {
                try
                {
                    // All code will run here
                    // Usage: return ExecuteContainer(() => { ALL RUNNING CODE HERE, remember to return });
                    return codeToExecute.Invoke();
                }
                //2. handle: DbUpdateException
                catch (DbUpdateException ex)
                {
                    var mess = string.Empty;
                    foreach (var errorMessage in ErrorHepler.GetaAllMessages(ex))
                    {
                        if (!string.IsNullOrEmpty(mess))
                        {
                            mess += ". ";
                        }
                        mess += errorMessage;
                    }
                    return _APIError(mess);
                }
                //3. Validation db
                catch (DbEntityValidationException e)
                {
                    var mess = string.Empty;
                    var errorList = e.EntityValidationErrors;
                    foreach (var error in errorList)
                    {
                        foreach (var ve in error.ValidationErrors)
                        {
                            if (!string.IsNullOrEmpty(mess))
                            {
                                mess += ". ";
                            }
                            mess += ve.ErrorMessage;
                        }
                    }
                    return _APIError(mess);
                }
                catch (Exception ex)
                {
                    //[2019-08-08 04:04 PM]: Chỗ này Linh sửa
                    var mess = ex.Message;
                    if (ex.InnerException != null)
                    {
                        if (ex.InnerException.InnerException != null)
                        {
                            if (!string.IsNullOrEmpty(ex.InnerException.InnerException.Message))
                            {
                                mess = ex.InnerException.InnerException.Message;
                            }
                        }
                        else if (ex.InnerException != null)
                        {
                            mess = ex.InnerException.Message;
                        }
                    }
                    return _APIError(mess);
                }
            }
            else
            {
                return _APIError(LanguageResource.Account_AccessDenied, new { isUpdate = true });
            }
        }

        protected JsonResult ExecuteAPIWithoutAuthContainer(Func<JsonResult> codeToExecute)
        {
            try
            {
                // All code will run here
                // Usage: return ExecuteContainer(() => { ALL RUNNING CODE HERE, remember to return });
                return codeToExecute.Invoke();
            }
            catch (Exception ex)
            {
                //[2019-08-08 04:04 PM]: Chỗ này Linh sửa
                var mess = ex.Message;
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.InnerException != null)
                    {
                        if (!string.IsNullOrEmpty(ex.InnerException.InnerException.Message))
                        {
                            mess = ex.InnerException.InnerException.Message;
                        }
                    }
                    else if (ex.InnerException != null)
                    {
                        mess = ex.InnerException.Message;
                    }
                }
                return _APIError(mess);
            }
        }

        //2019/08/08: [Châu][Thêm ExecuteAPIContainer2 để trả về Message]
        //protected JsonResult ExecuteAPIContainer2(Func<JsonResult> codeToExecute)
        //{
        //    try
        //    {
        //        // All code will run here
        //        // Usage: return ExecuteContainer(() => { ALL RUNNING CODE HERE, remember to return });
        //        return codeToExecute.Invoke();
        //    }
        //    catch (Exception ex)
        //    {
        //        var mess = ex.Message;
        //        if (ex.InnerException != null)
        //        {
        //            mess = ex.InnerException.InnerException.Message;
        //        }
        //        return Json(new
        //        {
        //            IsSuccess = false,
        //            Data = "",
        //            Message = mess
        //        }, JsonRequestBehavior.AllowGet);
        //    }
        //}
        protected JsonResult _APIError(string errorMessage, object data = null)
        {
            return Json(new
            {
                IsSuccess = false,
                Data = data,
                Message = errorMessage
            }, JsonRequestBehavior.AllowGet);
        }
        protected JsonResult _APISuccess(object data, string message = "")
        {
            //return Json(new
            //{
            //    IsSuccess = true,
            //    Data = data,
            //    Message = message
            //}, JsonRequestBehavior.AllowGet);
            var jsonResult = Json(new
            {
                IsSuccess = true,
                Data = data,
                Message = message
            }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        #endregion

        #region Warranty
        /// <summary>
        /// Set thông báo MVC
        /// </summary>
        /// <param name="message">Nội dung thông báo</param>
        /// <param name="type">success | warning | error</param>
        protected void SetAlert(string message, string type)
        {
            TempData["AlertMessage"] = message;
            if (type == "success")
            {
                TempData["AlertType"] = "alert-success";
            }
            else if (type == "warning")
            {
                TempData["AlertType"] = "alert-warning";
            }
            else if (type == "error")
            {
                TempData["AlertType"] = "alert-danger";
            }
        }
        protected string EnCodePhone(string phoneNumber)
        {
            var length = phoneNumber.Length;
            var numHead = phoneNumber.Substring(0, 3);
            var numEnd = phoneNumber.Substring(length - 3);
            var result = numHead + "****" + numEnd;
            return result;
        }
        #endregion

    }
}
