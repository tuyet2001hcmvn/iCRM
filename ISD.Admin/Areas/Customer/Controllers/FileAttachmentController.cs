using ISD.Constant;
using ISD.EntityModels;
using ISD.Core;
using ISD.Repositories;
using ISD.Resources;
using ISD.ViewModels;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Customer.Controllers
{
    public class FileAttachmentController : BaseController
    {
        // GET: FileAttachment
        public ActionResult _List(Guid id, bool? isLoadContent = false)
        {
            var fileAttachmentList = (from p in _context.FileAttachmentModel
                                      join c in _context.CatalogModel on p.FileType equals c.CatalogCode
                                      //Create User
                                      join acc in _context.AccountModel on p.CreateBy equals acc.AccountId
                                      join se in _context.SalesEmployeeModel on acc.EmployeeCode equals se.SalesEmployeeCode
                                      where p.ObjectId == id
                                      && c.CatalogTypeCode == ConstCatalogType.FileType
                                      orderby p.CreateTime descending
                                      select new FileAttachmentViewModel
                                      {
                                          FileAttachmentId = p.FileAttachmentId,
                                          ObjectId = p.ObjectId,
                                          FileTypeName = c.CatalogText_vi,
                                          FileAttachmentName = p.FileAttachmentName,
                                          FileType = p.FileType,
                                          FileUrl = p.FileUrl,
                                          //Create User
                                          CreateUser = se.SalesEmployeeName,
                                          CreateTime = p.CreateTime
                                      }).ToList();
            if (isLoadContent == true)
            {
                return PartialView("_ListContent", fileAttachmentList);
            }
            return PartialView(fileAttachmentList);
        }

        public ActionResult _Create(Guid ObjectId)
        {
            var fileView = new FileAttachmentViewModel
            {
                ObjectId = ObjectId,
                ProfileId = ObjectId
            };
            CreateViewBag();
            return PartialView("_FromUploadFile", fileView);
        }

        #region Save
        [HttpPost]
        public ActionResult Save(FileAttachmentViewModel modelView, HttpPostedFileBase FileUpload)
        {
            return ExecuteContainer(() =>
            {
                if (FileUpload == null)
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = string.Format(LanguageResource.ChooseRequired, LanguageResource.Customer_FileAttachment.ToLower())
                    });
                }
                var fileNew = new FileAttachmentModel();
                //1. GUID
                fileNew.FileAttachmentId = Guid.NewGuid();
                //2. Mã Profile
                fileNew.ObjectId = (Guid)modelView.ObjectId;
                
                if (FileUpload != null)
                {
                    string FileExtension = _unitOfWork.UtilitiesRepository.FileExtension(FileUpload.FileName);
                    string FileType = _unitOfWork.UtilitiesRepository.GetFileTypeByExtension(FileExtension);
                    //3. Tên file
                    fileNew.FileAttachmentName = FileUpload.FileName;
                    //4. Đường dẫn
                    fileNew.FileUrl = UploadDocumentFile(FileUpload, "Document", FileType: FileType);
                    //5. Đuôi file
                    fileNew.FileExtention = FileExtension;
                    //6. Loại file
                    fileNew.FileAttachmentCode = FileType;
                    //7. Loại file 
                    fileNew.FileType = modelView.FileType;
                }
                //7. người tạo
                fileNew.CreateBy = CurrentUser.AccountId;
                //8. thời gian tạo
                fileNew.CreateTime = DateTime.Now;
                _context.Entry(fileNew).State = EntityState.Added;

                //Save into mapping table
                Profile_File_Mapping mapping = new Profile_File_Mapping();
                mapping.ProfileId = (Guid)modelView.ProfileId;
                mapping.FileAttachmentId = fileNew.FileAttachmentId;
                _context.Entry(mapping).State = EntityState.Added;

                _context.SaveChanges();

                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.Customer_FileAttachment.ToLower())
                });
            });
        }
        #endregion

        #region Delete

        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            return ExecuteDelete(() =>
            {
                var fileInBd = _context.FileAttachmentModel.FirstOrDefault(p => p.FileAttachmentId == id);
                if (fileInBd != null)
                {
                    //Xóa bảng mapping
                    var mapping = _context.Profile_File_Mapping
                                          .FirstOrDefault(p => p.ProfileId == fileInBd.ObjectId && p.FileAttachmentId == id);
                    if (mapping != null)
                    {
                        _context.Entry(mapping).State = EntityState.Deleted;
                    }
                    _context.Entry(fileInBd).State = EntityState.Deleted;

                    _context.SaveChanges();
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.Created,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.Customer_FileAttachment.ToLower())
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = string.Format(LanguageResource.NotFound, LanguageResource.Customer_FileAttachment.ToLower())
                    });
                }
            });
        }

        #endregion Delete

        public void CreateViewBag()
        {
            var fileTypeList = _context.CatalogModel.Where(p => p.CatalogTypeCode == ConstCatalogType.FileType).ToList();
            ViewBag.FileType = new SelectList(fileTypeList, "CatalogCode", "CatalogText_vi");
        }
    }
}