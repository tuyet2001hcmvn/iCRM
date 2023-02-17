using ISD.Constant;
using ISD.EntityModels;
using ISD.Extensions;
using ISD.Repositories;
using ISD.Resources;
using ISD.ViewModels;
using ISD.ViewModels.Extension;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using ISD.Core;
using ISD.Repositories.Excel;

namespace Warehouse.Controllers
{
    public class BeginningInventoryController : BaseController
    {
        // GET: BeginningInventory
        #region Index
        public ActionResult Index()
        {
            CreateViewBagForSearch();
            return View();
        }
        public ActionResult _Search(StockReceivingSearchViewModel searchViewModel)
        {
            var stockReceiving = _unitOfWork.StockReceivingMasterRepository.SreachBeginInventory(searchViewModel);
            return PartialView(stockReceiving);
        }

        private void CreateViewBagForSearch()
        {
            //Dropdown Company
            var companyList = _unitOfWork.CompanyRepository.GetAll();
            ViewBag.SearchCompanyId = new SelectList(companyList, "CompanyId", "CompanyName");

            //Dropdown Store
            var storeList = _unitOfWork.StoreRepository.GetAllStore();
            ViewBag.SearchStoreId = new SelectList(storeList, "StoreId", "StoreName");

            //Dropdown Nhân viên
            var saleEmployeeList = _unitOfWork.SalesEmployeeRepository.GetAllForDropdownlist();
            ViewBag.SearchSalesEmployeeCode = new SelectList(saleEmployeeList, "SalesEmployeeCode", "SalesEmployeeName");

            //Dropdown Stock
            var listStock = _unitOfWork.StockRepository.GetAllForDropdown();
            ViewBag.SearchStockId = new SelectList(listStock, "StockId", "StockName");
        }
        #endregion

        #region Export Excel
        public ActionResult ExportEdit()
        {
            //Get data from server
            var product = (from p in _context.ProductModel
                           where p.isInventory == true
                           orderby p.ProductName
                           select new InventoryExcelViewModel()
                           {
                               ProductId = p.ProductId,
                               ProductCode = p.ProductCode,
                               ERPProductCode = p.ERPProductCode,
                               ProductName = p.ProductName,
                           }).ToList();

            return Export(product);
        }

        const string controllerCode = ConstExcelController.BeginningInventory;
        // const int startIndex = 9;
        //const int startDetailIndex = 7;
        [ISDAuthorizationAttribute]
        public FileContentResult Export(List<InventoryExcelViewModel> product)
        {
            //Columns to take
            List<ExcelTemplate> columns = new List<ExcelTemplate>();
            #region Master
            //Id
            columns.Add(new ExcelTemplate { ColumnName = "ProductId", isAllowedToEdit = false });
            //Mã SAP
            columns.Add(new ExcelTemplate { ColumnName = "ERPProductCode", isAllowedToEdit = false });
            //Mã sản phẩm
            columns.Add(new ExcelTemplate { ColumnName = "ProductCode", isAllowedToEdit = false });
            //Tên sản phẩm
            columns.Add(new ExcelTemplate { ColumnName = "ProductName", isAllowedToEdit = false });
            //Giá
            columns.Add(new ExcelTemplate { ColumnName = "Price", isAllowedToEdit = true, isCurrency = true });
            //Số lượng
            columns.Add(new ExcelTemplate { ColumnName = "Quantity", isAllowedToEdit = true });
            //Thành tiền
            columns.Add(new ExcelTemplate { ColumnName = "UnitPrice", isAllowedToEdit = false, isTotal = true, isCurrency = true });
            //Header
            string fileheader = string.Format(LanguageResource.Export_ExcelHeader, LanguageResource.BeginningInventory);

            //Default:
            //          1. heading[0] is controller code
            //          2. heading[1] is file name
            //          3. headinf[2] is warning (edit)
            heading.Add(new ExcelHeadingTemplate
            {
                Content = controllerCode,
                RowsToIgnore = 1,
                isWarning = false,
                isCode = true
            });
            heading.Add(new ExcelHeadingTemplate
            {
                Content = fileheader.ToUpper(),
                RowsToIgnore = 1,
                isWarning = false,
                isCode = false
            });
            heading.Add(new ExcelHeadingTemplate
            {
                Content = LanguageResource.Export_ExcelWarning1,
                RowsToIgnore = 0,
                isWarning = true,
                isCode = false
            });
            heading.Add(new ExcelHeadingTemplate
            {
                Content = LanguageResource.Export_ExcelWarning2,
                RowsToIgnore = 1,
                isWarning = true,
                isCode = false
            });

            //Master
            heading.Add(new ExcelHeadingTemplate
            {
                Content = LanguageResource.DocumentDate,
                RowsToIgnore = 0,
                isTable = true,
                isCode = true,
            });
            heading.Add(new ExcelHeadingTemplate
            {
                Content = LanguageResource.Company_CompanyCode,
                RowsToIgnore = 0,
                isTable = true,
                isCode = true
            });
            heading.Add(new ExcelHeadingTemplate
            {
                Content = LanguageResource.Store_StoreCode,
                RowsToIgnore = 0,
                isTable = true,
                isCode = true
            });
            heading.Add(new ExcelHeadingTemplate
            {
                Content = LanguageResource.StockCode,
                RowsToIgnore = 1,
                isTable = true,
                isCode = true
            });

            List<ExcelSheetTemplate> formatList = new List<ExcelSheetTemplate>();
            formatList.Add(new ExcelSheetTemplate
            {
                ColumnsToTake = columns,
                Heading = heading,
                showSlno = true
            });
            #endregion Master

            //Body
            byte[] filecontent;

            filecontent = ClassExportExcel.ExportExcel(product, columns, heading, true, HasExtraSheet: false);
            //File name       
            string fileNameWithFormat = string.Format("{0}.xlsx", _unitOfWork.UtilitiesRepository.RemoveSign4VietnameseString(fileheader.ToUpper()).Replace(" ", "_"));

            return File(filecontent, ClassExportExcel.ExcelContentType, fileNameWithFormat);
        }
        #endregion

        #region Import
        [ISDAuthorizationAttribute]
        public ActionResult Import()
        {
            return ExcuteImportExcel(() =>
            {
                //dataset sắp xếp các sheet theo alphabet
                DataSet ds = GetDataSetFromExcel();
                List<string> errorList = new List<string>();
                if (ds.Tables != null && ds.Tables.Count > 0)
                {
                    using (TransactionScope ts = new TransactionScope())
                    {
                        int startDetailIndex = 6;
                        int startRowIndex = 12;
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];

                            //Get controller code from Excel file
                            string contCode = dt.Columns[0].ColumnName.ToString();
                            //Import data with accordant controller and action
                            if (contCode == controllerCode)
                            {
                                var index = 0;
                                var masterIndex = 0;

                                StockReceivingMasterViewModel masterModel = new StockReceivingMasterViewModel();
                                masterModel.StockReceivingId = Guid.NewGuid();
                                var stockCode = "";

                                foreach (DataRow dr in dt.Rows)
                                {
                                    //get infor Master
                                    if (dt.Rows.IndexOf(dr) >= startDetailIndex && !string.IsNullOrEmpty(dr.ItemArray[0].ToString()))
                                    {
                                        index++;
                                        InventoryMasterExcellViewModel master = new InventoryMasterExcellViewModel();
                                        master.Index = masterIndex;
                                        master.Value = dr.ItemArray[1].ToString();

                                        InventoryExcelViewModel invenIsValid = CheckTemplateInventoryMaster(master, index);
                                        if (!string.IsNullOrEmpty(invenIsValid.Error))
                                        {
                                            string error = invenIsValid.Error;
                                            errorList.Add(error);
                                        }
                                        else
                                        {
                                            switch (master.Index)
                                            {
                                                case 0:
                                                    {
                                                        masterModel.DocumentDate = invenIsValid.DocumentDate;
                                                        break;
                                                    }
                                                case 1:
                                                    {
                                                        masterModel.CompanyCode = invenIsValid.CompanyCode;
                                                        break;
                                                    }
                                                case 2:
                                                    {
                                                        masterModel.StoreCode = invenIsValid.StoreCode;
                                                        break;
                                                    }
                                                case 3:
                                                    {
                                                        stockCode = invenIsValid.StockCode;
                                                        break;
                                                    }
                                                default:
                                                    {
                                                        break;
                                                    }
                                            }
                                        }
                                        masterIndex++;
                                    }
                                    //Create master
                                    if (dt.Rows.IndexOf(dr) == startRowIndex)
                                    {
                                        string result = ExecuteImportExcelInventoryMaster(masterModel);
                                        var a = dt.Rows.IndexOf(dr);
                                        if (result != LanguageResource.ImportSuccess)
                                        {
                                            errorList.Add(result);
                                            //if file is unchanged => break foreach loop and return error
                                            if (result == LanguageResource.Validation_ImportExcelUnchangeFile)
                                            {
                                                break;
                                            }
                                        }
                                    }
                                    //Detail
                                    if (dt.Rows.IndexOf(dr) >= startRowIndex && !string.IsNullOrEmpty(dr.ItemArray[0].ToString()))
                                    {
                                        index++;
                                        //Check correct template
                                        InventoryExcelViewModel invenIsValid = CheckTemplateInventory(dr.ItemArray, index);

                                        if (!string.IsNullOrEmpty(invenIsValid.Error))
                                        {
                                            string error = invenIsValid.Error;
                                            errorList.Add(error);
                                        }
                                        else
                                        {
                                            //Get stockId
                                            invenIsValid.StockId = _unitOfWork.StockRepository.GetStockIdByStockCode(stockCode);
                                            invenIsValid.DateKey = _unitOfWork.UtilitiesRepository.ConvertDateTimeToInt(masterModel.DocumentDate);
                                            invenIsValid.StockReceivingId = masterModel.StockReceivingId;

                                            string result = ExecuteImportExcelInventory(invenIsValid);
                                            if (result != LanguageResource.ImportSuccess)
                                            {
                                                errorList.Add(result);
                                                //if file is unchanged => break foreach loop and return error
                                                if (result == LanguageResource.Validation_ImportExcelUnchangeFile)
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                                _context.SaveChanges();
                            }
                            else
                            {
                                string error = string.Format(LanguageResource.Validation_ImportCheckController, LanguageResource.BeginningInventory.ToLower());
                                errorList.Add(error);
                            }
                        }
                        if (errorList != null && errorList.Count > 0)
                        {
                            return Json(new
                            {
                                Code = System.Net.HttpStatusCode.NotModified,
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

        #region CheckInventory Detail
        public InventoryExcelViewModel CheckTemplateInventory(object[] row, int index)
        {
            InventoryExcelViewModel inventoryVM = new InventoryExcelViewModel();
            var fieldName = "";
            try
            {
                for (int i = 0; i <= row.Length; i++)
                {
                    #region Convert data to import
                    switch (i)
                    {
                        //Index
                        case 0:
                            {
                                fieldName = LanguageResource.NumberIndex;
                                int rowIndex = int.Parse(row[i].ToString());
                                inventoryVM.RowIndex = rowIndex;
                                break;
                            }
                        //ProductId
                        case 1:
                            {
                                fieldName = LanguageResource.ProductId;
                                string ProductId = row[i].ToString();
                                if (string.IsNullOrEmpty(ProductId))
                                {
                                    inventoryVM.isNullValueId = true;
                                }
                                else
                                {
                                    inventoryVM.ProductId = Guid.Parse(ProductId);
                                    inventoryVM.isNullValueId = false;
                                }
                                break;
                            }
                        //Mã SAP
                        case 2:
                            {
                                fieldName = LanguageResource.ProductName;
                                inventoryVM.ProductName = row[i].ToString();
                                break;
                            }
                        //Tên sản phẩm
                        case 4:
                            {
                                fieldName = LanguageResource.ProductName;
                                inventoryVM.ProductName = row[i].ToString();
                                break;
                            }
                        //Số lượng
                        case 5:
                            {
                                fieldName = LanguageResource.SaleOrder_Quantity;
                                var Quantity = row[i].ToString();
                                if (string.IsNullOrEmpty(Quantity))
                                {
                                    inventoryVM.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, LanguageResource.SaleOrder_Quantity), inventoryVM.RowIndex);
                                }
                                else
                                {
                                    inventoryVM.Quantity = int.Parse(Quantity);
                                }
                                break;
                            }
                        //Giá
                        case 6:
                            {
                                fieldName = LanguageResource.Price;
                                var Price = row[i].ToString();
                                if (string.IsNullOrEmpty(Price))
                                {
                                    inventoryVM.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, LanguageResource.Price), inventoryVM.RowIndex);
                                }
                                else
                                {
                                    inventoryVM.Price = decimal.Parse(Price);
                                }
                                break;
                            }
                    }
                    #endregion Convert data to import
                }
            }
            catch (FormatException ex)
            {
                var Message = ex.Message;
                inventoryVM.Error = string.Format(LanguageResource.Validation_ImportCastValid, fieldName, index);
            }
            catch (InvalidCastException ex)
            {
                var Message = ex.Message;
                inventoryVM.Error = string.Format(LanguageResource.Validation_ImportCastValid, fieldName, index);
            }
            catch (Exception ex)
            {
                var Message = ex.Message;
                inventoryVM.Error = string.Format(LanguageResource.Validate_ImportException, fieldName, index);
            }
            return inventoryVM;
        }
        #endregion

        #region CheckInventoryMaster
        public InventoryExcelViewModel CheckTemplateInventoryMaster(InventoryMasterExcellViewModel master, int index)
        {
            InventoryExcelViewModel inventoryVM = new InventoryExcelViewModel();
            var fieldName = "";
            try
            {
                switch (master.Index)
                {
                    //1. Ngày chứng từ
                    case 0:
                        {
                            fieldName = LanguageResource.DocumentDate;
                            //inventoryVM.DocumentDate = DateTime.Parse(master.Value.ToString());

                            UtilitiesRepository _utilitiesRepository = new UtilitiesRepository();
                            inventoryVM.DocumentDate = Convert.ToDateTime(_utilitiesRepository.ConvertDateTimeFromddMMyyyy(master.Value.ToString()));
                            break;
                        }
                    //2. Công ty
                    case 1:
                        {
                            fieldName = LanguageResource.Company_CompanyCode;
                            inventoryVM.CompanyCode = master.Value;
                            break;
                        }
                    //3. Chi nhánh
                    case 2:
                        {
                            fieldName = LanguageResource.Store_StoreCode;
                            inventoryVM.StoreCode = master.Value;
                            break;
                        }
                    //4. Kho
                    case 3:
                        {
                            fieldName = LanguageResource.StockCode;
                            inventoryVM.StockCode = master.Value;
                            break;
                        }

                    default:
                        break;
                }
            }
            catch (FormatException ex)
            {
                var Message = ex.Message;
                inventoryVM.Error = string.Format(LanguageResource.Validation_ImportCastValid, fieldName, index);
            }
            catch (InvalidCastException ex)
            {
                var Message = ex.Message;
                inventoryVM.Error = string.Format(LanguageResource.Validation_ImportCastValid, fieldName, index);
            }
            catch (Exception ex)
            {
                var Message = ex.Message;
                inventoryVM.Error = string.Format(LanguageResource.Validate_ImportException, fieldName, index);
            }
            return inventoryVM;
        }
        #endregion

        #region ExecuteImport Inventory
        public string ExecuteImportExcelInventory(InventoryExcelViewModel invenIsValid)
        {
            #region Insert
            try
            {
                //Check sản phẩm đã nhập kho
                var invenIsExist = _context.StockReceivingDetailModel
                                         .Where(p => p.ProductId == invenIsValid.ProductId && p.StockId == invenIsValid.StockId)
                                         .FirstOrDefault();
                if (invenIsExist == null)
                {

                    StockReceivingDetailModel detailModel = new StockReceivingDetailModel();
                    detailModel.StockReceivingDetailId = Guid.NewGuid();
                    detailModel.StockReceivingId = invenIsValid.StockReceivingId;
                    detailModel.ProductId = invenIsValid.ProductId;
                    detailModel.StockId = invenIsValid.StockId;
                    detailModel.DateKey = invenIsValid.DateKey;
                    detailModel.Quantity = invenIsValid.Quantity;
                    detailModel.Price = invenIsValid.Price;
                    detailModel.UnitPrice = invenIsValid.UnitPrice;
                    detailModel.Note = LanguageResource.BeginInventory_Note;

                    _context.Entry(detailModel).State = EntityState.Added;
                }
                else
                {
                    return "Lỗi: sản phẩm " + invenIsValid.ProductName + " đã được cập nhật tồn kho lần đầu!";
                }
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

            #endregion Insert
            return LanguageResource.ImportSuccess;
        }
        public string ExecuteImportExcelInventoryMaster(StockReceivingMasterViewModel receiveMasterVM)
        {
            #region Insert
            try
            {
                receiveMasterVM.CompanyId = _unitOfWork.CompanyRepository.GetCompanyIdBy(receiveMasterVM.CompanyCode);
                receiveMasterVM.StoreId = _unitOfWork.StoreRepository.GetStoreIdBySaleOrgCode(receiveMasterVM.StoreCode);
                //receiveMasterVM.ProfileId = Guid.Parse("49939b59-e728-45b6-ac47-a34d3927ce54");//Công Ty TNHH Sản Xuất Gỗ An Cường
                receiveMasterVM.SalesEmployeeCode = CurrentUser.EmployeeCode;
                receiveMasterVM.Note = LanguageResource.BeginInventory_Note;
                receiveMasterVM.CreateBy = CurrentUser.AccountId;
                receiveMasterVM.CreateTime = DateTime.Now;
                receiveMasterVM.isFirst = true;

                _unitOfWork.StockReceivingMasterRepository.Create(receiveMasterVM);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    return string.Format("Cập nhật tồn kho đã xảy ra lỗi: {0}", ex.InnerException.Message);
                }
                else
                {
                    return string.Format("Cập nhật tồn kho đã xảy ra lỗi: {0}", ex.Message);
                }
            }

            #endregion Insert

            return LanguageResource.ImportSuccess;
        }

        #endregion
        #endregion Import
    }
}