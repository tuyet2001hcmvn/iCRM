using ISD.Constant;
using ISD.Core;
using ISD.EntityModels;
using ISD.Extensions;
using ISD.Repositories.Excel;
using ISD.Resources;
using ISD.ViewModels;
using ISD.ViewModels.MasterData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;

namespace MasterData.Controllers
{
    public class QuestionBankController : BaseController
    {
        // GET: QuestionBank
        public ActionResult Index()
        {
            ViewBag.Actived = new List<SelectListItem>()
            {
                new SelectListItem(){Text="Đã trả lời",Value="true"},
                new SelectListItem(){Text="Chưa trả lời", Value="false" }
            };
            return View();
        }
        public ActionResult Create()
        {
            return View();
        }
        public ActionResult Edit(Guid id)
        {
            ViewBag.Id = id;
            return View();
        }

        #region ExportExcel
        public ActionResult ExportCreate()
        {
            List<QuestionExportViewModel> viewModel = new List<QuestionExportViewModel>();
            return Export(viewModel, isEdit: false);
        }

        public ActionResult ExportEdit(QuestionSearchViewModel searchModel)
        {
            List<QuestionExportViewModel> result = new List<QuestionExportViewModel>();
            var categoryId = Guid.NewGuid();
            if (!string.IsNullOrEmpty(searchModel.QuestionCategoryId))
            {
                categoryId = Guid.Parse(searchModel.QuestionCategoryId);
            }
            result = (from question in _context.QuestionBankModel
                      join category in _context.CatalogModel on question.QuestionCategoryId equals category.CatalogId
                      join department in _context.CatalogModel on question.DepartmentId equals department.CatalogId
                      join account in _context.AccountModel on question.CreateBy equals account.AccountId
                      where (searchModel.QuestionBankCode == null || question.QuestionBankCode == searchModel.QuestionBankCode) &&
                             (searchModel.Question == null || searchModel.Question == "" || question.Question.Contains(searchModel.Question)) &&
                              (searchModel.QuestionCategoryId == null || searchModel.QuestionCategoryId == "" || question.QuestionCategoryId == categoryId) &&
                              (searchModel.Actived == null || question.Actived == searchModel.Actived)
                      orderby question.QuestionBankCode
                      select new QuestionExportViewModel()
                      {
                          QuestionBankCode = question.QuestionBankCode,
                          Question = question.Question,
                          Answer = question.Answer,
                          AnswerC = question.AnswerC,
                          AnswerB = question.AnswerB,
                          QuestionCategoryName = category.CatalogText_vi,
                          CreateBy = account.FullName,
                          DepartmentName = department.CatalogText_vi,
                          CreateTime = question.CreateTime
                      }).ToList();
            var s = ConvertHtmlToText(result);
            return Export(ConvertHtmlToText(result), isEdit: true);
        }


        public ActionResult Export(List<QuestionExportViewModel> viewModel, bool isEdit)
        {
            #region Dropdownlist
            List<DropdownIdTypeStringModel> CategoryCode = (from c in _context.CatalogModel
                                                            where c.Actived == true && c.CatalogTypeCode == ConstCatalogType.QuestionCategory
                                                            orderby c.CatalogCode
                                                            select new DropdownIdTypeStringModel()
                                                            {
                                                                Id = c.CatalogCode,
                                                                Name = c.CatalogText_vi,
                                                            }).ToList();

            List<DropdownIdTypeStringModel> DepartmentCode = (from c in _context.CatalogModel
                                                              where c.Actived == true && c.CatalogTypeCode == ConstCatalogType.QuestionDepartment
                                                              orderby c.CatalogCode
                                                              select new DropdownIdTypeStringModel()
                                                              {
                                                                  Id = c.CatalogCode,
                                                                  Name = c.CatalogText_vi,
                                                              }).ToList();

            #endregion

            #region Master
            List<ExcelTemplate> columns = new List<ExcelTemplate>();


            //insert => dropdownlist
            //edit => not allow edit
            if (isEdit == true)
            {
                columns.Add(new ExcelTemplate { ColumnName = "QuestionBankCode", isAllowedToEdit = false });
                columns.Add(new ExcelTemplate { ColumnName = "Question", isAllowedToEdit = false, isWraptext = true });
                columns.Add(new ExcelTemplate { ColumnName = "Answer", isAllowedToEdit = false, isWraptext = true });
                columns.Add(new ExcelTemplate { ColumnName = "AnswerC", isAllowedToEdit = false, isWraptext = true });
                columns.Add(new ExcelTemplate { ColumnName = "AnswerB", isAllowedToEdit = false, isWraptext = true });
                columns.Add(new ExcelTemplate { ColumnName = "CreateBy", isAllowedToEdit = false });
                columns.Add(new ExcelTemplate { ColumnName = "CreateTime", isAllowedToEdit = false, isDateTime = true });
                columns.Add(new ExcelTemplate { ColumnName = "QuestionCategoryName", isAllowedToEdit = false });
                columns.Add(new ExcelTemplate() { ColumnName = "DepartmentName", isAllowedToEdit = false });
            }
            else
            {
                columns.Add(new ExcelTemplate { ColumnName = "Question", isAllowedToEdit = true, isWraptext = true });
                columns.Add(new ExcelTemplate { ColumnName = "Answer", isAllowedToEdit = true, isWraptext = true });
                columns.Add(new ExcelTemplate { ColumnName = "AnswerC", isAllowedToEdit = true, isWraptext = true });
                columns.Add(new ExcelTemplate { ColumnName = "AnswerB", isAllowedToEdit = true, isWraptext = true });
                columns.Add(new ExcelTemplate()
                {
                    ColumnName = "QuestionCategoryName",
                    isAllowedToEdit = true,
                    isDropdownlist = true,
                    TypeId = ConstExcelController.StringId,
                    DropdownIdTypeStringData = CategoryCode
                });
                columns.Add(new ExcelTemplate()
                {
                    ColumnName = "DepartmentName",
                    isAllowedToEdit = true,
                    isDropdownlist = true,
                    TypeId = ConstExcelController.StringId,
                    DropdownIdTypeStringData = DepartmentCode
                });
            }
            #endregion Master
            //Header
            string fileheader = string.Empty;
            fileheader = "NGÂN HÀNG CÂU HỎI";

            #region Master

            // columns.Add(new ExcelTemplate { ColumnName = "CreateTime", isAllowedToEdit = false });
            #endregion

            heading.Add(new ExcelHeadingTemplate()
            {
                Content = controllerCode,//controllerCode,
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
                RowsToIgnore = 0,
                isWarning = true,
                isCode = false
            });
            heading.Add(new ExcelHeadingTemplate()
            {
                Content = LanguageResource.Export_ExcelWarning2,
                RowsToIgnore = 0,
                isWarning = true,
                isCode = false
            });

            //Body
            byte[] filecontent = ClassExportExcel.ExportExcel(viewModel, columns, heading, true);
            //Insert => THEM_MOI
            //Edit => CAP_NHAT
            string exportType = LanguageResource.exportType_Insert;
            if (isEdit == true)
            {
                exportType = LanguageResource.exportType_Edit;
            }
            string fileNameWithFormat = string.Format("{0}_{1}.xlsx", exportType, _unitOfWork.UtilitiesRepository.RemoveSign4VietnameseString(fileheader.ToUpper()).Replace(" ", "_"));

            return File(filecontent, ClassExportExcel.ExcelContentType, fileNameWithFormat);
        }
        #endregion


        #region ImportExcel
        const string controllerCode = ConstExcelController.QuestionBank;
        const int startIndex = 6;
        [ISDAuthorizationAttribute]
        public ActionResult Import()
        {
            return ExcuteImportExcel(() =>
            {
                DataSet ds = GetDataSetFromExcelNew();
                List<string> errorList = new List<string>();
                if (ds.Tables != null && ds.Tables.Count > 0)
                {
                    var dt = ds.Tables[0];
                    using (TransactionScope ts = new TransactionScope())
                    {
                        //foreach (DataTable dt in ds.Tables)
                        //{
                        //Get controller code from Excel file
                        string contCode = dt.Columns[0].ColumnName.ToString();
                        //string contCode = dt.Rows[0][0].ToString();
                        //Import data with accordant controller and action
                        if (contCode == controllerCode)
                        {
                            var index = 0;
                            foreach (DataRow dr in dt.Rows)
                            {
                                if (dt.Rows.IndexOf(dr) >= startIndex && !string.IsNullOrEmpty(dr.ItemArray[0].ToString()))
                                {
                                    index++;
                                    //Check correct template
                                    QuestionImportViewModel questionBankValid = CheckTemplate(dr.ItemArray, index);

                                    if (!string.IsNullOrEmpty(questionBankValid.Error))
                                    {
                                        string error = questionBankValid.Error;
                                        errorList.Add(error);
                                    }
                                    else
                                    {
                                        string result = ExecuteImportExcelSalesEmployee(questionBankValid);
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
                            string error = string.Format(LanguageResource.Validation_ImportCheckController, LanguageResource.QuestionBank);
                            errorList.Add(error);
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

        private QuestionImportViewModel CheckTemplate(object[] row, int index)
        {
            QuestionImportViewModel questionBankViewModel = new QuestionImportViewModel();
            var fieldName = "";
            try
            {
                for (int i = 0; i <= row.Length; i++)
                {
                    string data = null;
                    #region Convert data to import
                    switch (i)
                    {
                        //Index
                        case 0:
                            fieldName = LanguageResource.NumberIndex;
                            int rowIndex = int.Parse(row[i].ToString());
                            questionBankViewModel.RowIndex = rowIndex;
                            break;

                        //Nội dung câu hỏi
                        case 1:
                            fieldName = LanguageResource.QuestionContent;
                            data = row[i].ToString();
                            if (string.IsNullOrEmpty(data))
                            {
                                questionBankViewModel.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, LanguageResource.QuestionContent), questionBankViewModel.RowIndex);
                            }
                            else
                            {
                                questionBankViewModel.Question = data;
                            }
                            break;
                        //Câu trả lời chung
                        case 2:
                            fieldName = LanguageResource.Answer;
                            data = row[i].ToString();
                            if (string.IsNullOrEmpty(data))
                            {
                                questionBankViewModel.Answer = null;
                            }
                            else
                            {
                                questionBankViewModel.Answer = data;
                            }
                            break;
                        //Trả lời riêng cho Khách tiêu dùng
                        case 3:
                            fieldName = LanguageResource.AnswerB;
                            data = row[i].ToString();
                            if (string.IsNullOrEmpty(data))
                            {
                                questionBankViewModel.AnswerB = null;
                            }
                            else
                            {
                                questionBankViewModel.AnswerB = data;
                            }
                            break;
                        //Trả lời riêng cho Khách tiêu dùng
                        case 4:
                            fieldName = LanguageResource.AnswerC;
                            data = row[i].ToString();
                            if (string.IsNullOrEmpty(data))
                            {
                                questionBankViewModel.AnswerC = null;
                            }
                            else
                            {
                                questionBankViewModel.AnswerC = data;
                            }
                            break;
                        //Mã bộ phận
                        case 6:
                            fieldName = LanguageResource.Department;
                            data = row[i].ToString();
                            if (string.IsNullOrEmpty(data))
                            {
                                questionBankViewModel.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, LanguageResource.Department_DepartmentCode), questionBankViewModel.RowIndex);
                            }
                            else
                            {
                                questionBankViewModel.DepartmentName = data;
                            }
                            break;
                        //Mã bộ phận
                        case 8:
                            fieldName = LanguageResource.Department;
                            data = row[i].ToString();
                            if (string.IsNullOrEmpty(data))
                            {
                                questionBankViewModel.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, LanguageResource.Department_DepartmentCode), questionBankViewModel.RowIndex);
                            }
                            else
                            {
                                questionBankViewModel.DepartmentCode = data;
                            }
                            break;
                        //Loại câu hỏi
                        case 5:
                            fieldName = LanguageResource.QuestionCategoryName;
                            data = row[i].ToString();
                            if (string.IsNullOrEmpty(data))
                            {
                                questionBankViewModel.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, LanguageResource.QuestionCategoryName), questionBankViewModel.RowIndex);
                            }
                            else
                            {
                                questionBankViewModel.QuestionCategoryName = data;
                            }
                            break;
                        //Loại câu hỏi
                        case 7:
                            fieldName = LanguageResource.QuestionCategoryName;
                            data = row[i].ToString();
                            if (string.IsNullOrEmpty(data))
                            {
                                questionBankViewModel.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, LanguageResource.QuestionCategoryName), questionBankViewModel.RowIndex);
                            }
                            else
                            {
                                questionBankViewModel.QuestionCategoryCode = data;
                            }
                            break;
                    }
                    #endregion Convert data to import
                }
            }
            catch (FormatException ex)
            {
                var Message = ex.Message;
                questionBankViewModel.Error = string.Format(LanguageResource.Validation_ImportCastValid, fieldName, index);
            }
            catch (InvalidCastException ex)
            {
                var Message = ex.Message;
                questionBankViewModel.Error = string.Format(LanguageResource.Validation_ImportCastValid, fieldName, index);
            }
            catch (Exception ex)
            {
                var Message = ex.Message;
                questionBankViewModel.Error = string.Format(LanguageResource.Validate_ImportException, fieldName, index);
            }
            return questionBankViewModel;
        }


        #region Insert/Update data from excel file
        public string ExecuteImportExcelSalesEmployee(QuestionImportViewModel questionBankValid)
        {
            //Get Catalog 
            var CatalogId = _context.CatalogModel.Where(x => x.CatalogCode == questionBankValid.QuestionCategoryCode && x.CatalogTypeCode == ConstCatalogType.QuestionCategory).Select(x => x.CatalogId).FirstOrDefault();
            //Get Department
            var DepartmentId = _context.CatalogModel.Where(x => x.CatalogCode == questionBankValid.DepartmentCode && x.CatalogTypeCode == ConstCatalogType.QuestionDepartment).Select(x => x.CatalogId).FirstOrDefault();
            //Get employee in Db by employeeCode
            QuestionBankModel questionBankExist = _context.QuestionBankModel
                                                     .FirstOrDefault(p => p.Question == questionBankValid.Question && p.QuestionCategoryId == CatalogId && p.DepartmentId == DepartmentId);
            //Check:
            //1. If employee == null then => Insert
            //2. Else then => Update
            #region Insert
            if (questionBankExist == null)
            {
                try
                {
                    QuestionBankModel questionBank = new QuestionBankModel();
                    questionBank.Id = Guid.NewGuid();
                    questionBank.Question = questionBankValid.Question;
                    questionBank.DepartmentId = DepartmentId;
                    questionBank.QuestionCategoryId = CatalogId;
                    questionBank.Answer = questionBankValid.Answer;
                    questionBank.AnswerB = questionBankValid.AnswerB;
                    questionBank.AnswerC = questionBankValid.AnswerC;
                    questionBank.Actived = true;
                    questionBank.CreateBy = CurrentUser.AccountId;
                    questionBank.CreateTime = DateTime.Now;
                    _context.Entry(questionBank).State = EntityState.Added;
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null)
                    {
                        return string.Format("Thêm mới đã xảy ra lỗi: {0}", ex.InnerException.Message);
                    }
                    else
                    {
                        return string.Format("Thêm mới đã xảy ra lỗi: {0}", ex.Message);
                    }
                }
            }
            #endregion Insert

            #region Update
            else
            {
                try
                {
                    questionBankExist.Question = questionBankValid.Question;
                    questionBankExist.DepartmentId = DepartmentId;
                    questionBankExist.QuestionCategoryId = CatalogId;
                    questionBankExist.Answer = questionBankValid.Answer;
                    questionBankExist.AnswerB = questionBankValid.AnswerB;
                    questionBankExist.AnswerC = questionBankValid.AnswerC;
                    questionBankExist.LastEditBy = CurrentUser.AccountId;
                    questionBankExist.LastEditTime = DateTime.Now;
                    _context.Entry(questionBankExist).State = EntityState.Modified;
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null)
                    {
                        return string.Format("Cập nhật đã xảy ra lỗi: {0}", ex.InnerException.Message);
                    }
                    else
                    {
                        return string.Format("Cập nhật đã xảy ra lỗi: {0}", ex.Message);
                    }
                }
            }
            #endregion Update

            _context.SaveChanges();
            return LanguageResource.ImportSuccess;
        }
        #endregion Insert/Update data from excel file
        #endregion

        private List<QuestionExportViewModel> ConvertHtmlToText(List<QuestionExportViewModel> list)
        {
            foreach (var item in list)
            {
                if (!string.IsNullOrEmpty(item.Answer))
                {
                    item.Answer = HtmlToText.ConvertHtml(item.Answer);
                }

                if (!string.IsNullOrEmpty(item.AnswerC))
                {
                    item.AnswerC = HtmlToText.ConvertHtml(item.AnswerC);
                }
                if (!string.IsNullOrEmpty(item.AnswerB))
                {
                    item.AnswerB = HtmlToText.ConvertHtml(item.AnswerB);
                }
            }
            return list;
        }
    }
}