using ISD.Constant;
using ISD.Core;
using ISD.Extensions;
using ISD.Repositories;
using ISD.Resources;
using ISD.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Work.Controllers
{
    public class GranttChartController : BaseController
    {
        // GET: GranttChart
        public ActionResult Index(string Type = null, Guid? WorkFlowId = null, string Assignee = null, string TaskProcessCode_Input = null, string tab = null)
        {
            TaskSearchViewModel searchModel = new TaskSearchViewModel();
            //ViewBag.Actived = true;
            //Title
            string pageUrl = "/Work/Task";
            var parameter = "?Type=" + Type;
            //var containElemen = "?Type=" + Type;
            if (WorkFlowId != null)
            {
                parameter += "&WorkFlowId=" + WorkFlowId;
            }
            if (!string.IsNullOrEmpty(tab))
            {
                parameter += "&tab=" + tab;
            }
            var title = (from p in _context.PageModel
                         where p.PageUrl == pageUrl
                         && p.Parameter == parameter
                         select p.PageName).FirstOrDefault();
            ViewBag.Title = title;

            ViewBag.PageId = GetPageId(pageUrl, parameter);
            string Reporter = string.Empty;
            string TaskProcessCode = string.Empty;
            switch (Type)
            {
                //Nếu là công việc của tôi: chọn sẵn tôi là người được phân công là tôi
                case "MyWork":
                    if (string.IsNullOrEmpty(Assignee))
                    {
                        Assignee = CurrentUser.EmployeeCode;
                    }
                    break;
                //Nếu là công việc tôi đang theo dõi: chọn sẵn người giao việc là tôi
                case "MyFollow":
                    if (string.IsNullOrEmpty(Assignee))
                    {
                        Reporter = CurrentUser.EmployeeCode;
                    }
                    break;
                //Nếu góc vật liệu chọn tất cả
                case "GTB":
                    TaskProcessCode = string.Empty;
                    break;
                //Nếu là các công việc còn lại: chọn sẵn việc chưa hoàn thành
                default:
                    TaskProcessCode = ConstTaskStatus.Incomplete;
                    break;
            }
            if (!string.IsNullOrEmpty(TaskProcessCode_Input))
            {
                TaskProcessCode = TaskProcessCode_Input;
            }
            CreateViewBag(Type: Type, WorkFlowId: WorkFlowId, Reporter: Reporter, Assignee: Assignee, isEditMode: false);
            //ViewBag.GoogleMapAPIKey = GoogleMapAPIKey;
            CreateSearchViewBag(Type: Type, TaskProcessCode: TaskProcessCode);
            var estimatedTitle = (from p in _context.PageModel
                                  where p.PageUrl.Contains("EstimatedCalendar")
                                  select p.PageName).FirstOrDefault();
            ViewBag.EstimatedTitle = estimatedTitle;

            var companyList = _context.CompanyModel.OrderBy(p => p.CompanyCode).Select(p => new
            {
                CompanyCode = p.CompanyCode,
                CompanyName = p.CompanyCode + " | " + p.CompanyName
            }).ToList();
            ViewBag.CompanyCode = new SelectList(companyList, "CompanyCode", "CompanyName");


            return View(searchModel);
        }

        #region ViewBag
        private void CreateViewBag(string PriorityCode = "", Guid? WorkFlowId = null, string CommonMistakeCode = null, Guid? StoreId = null, string Reporter = null, Guid? ProfileId = null, Guid? TaskStatusId = null, string ShowroomCode = "", string CustomerClassCode = "", string CategoryCode = "", string ChannelCode = "", string SaleEmployeeCode = "", Guid? ContactId = null, bool? isEditMode = false, string Type = null, string Assignee = null, string ProcessCode = null, string WorkFlowName = null, int? TaskCode = null, string ServiceTechnicalTeamCode = null, string ErrorTypeCode = null, string ErrorCode = null, string VisitTypeCode = null, Guid? TaskId = null, string RemindCycle = null, string ProductLevelCode = null, string ProductColorCode = null, string UsualErrorCode = null, string ProductCategoryCode = null, bool? IsEditProduct = null, string TaskSourceCode = null, string Category = null, bool? isInPopup = null, string VisitSaleOfficeCode = null, string Unit = null, string AccErrorTypeCode = null, string Ratings = null, string CustomerSatisfactionCode = null, string SubtaskCode = null, Guid? ConstructionUnit = null, Guid? ConstructionUnitContact = null, List<string> UsualErrorCodeList = null, string CustomerRatings = null, string Ticket_CustomerReviews_Product = null, string Ticket_CustomerReviews_Service = null)
        {
            //Type: Loại (WorkflowCategoryCode)
            ViewBag.Type = Type;
            //GoogleMapAPIKey
            //ViewBag.GoogleMapAPIKey = GoogleMapAPIKey;

            string WorkFlowCode = string.Empty;
            string NewWorkFlowName = string.Empty;
            bool? IsDisabledSummary = false;
            if (WorkFlowId == null)
            {
                WorkFlowId = Guid.Empty;
            }
            else
            {
                var workflow = _context.WorkFlowModel.Where(p => p.WorkFlowId == WorkFlowId).FirstOrDefault();
                if (workflow != null)
                {
                    WorkFlowCode = workflow.WorkFlowCode;
                    NewWorkFlowName = workflow.WorkFlowName;
                    IsDisabledSummary = workflow.IsDisabledSummary;
                }
            }
            ViewBag.IsDisabledSummary = IsDisabledSummary;

            #region Task
            //Priority
            var listPriority = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.Priority);
            ViewBag.PriorityCode = new SelectList(listPriority, "CatalogCode", "CatalogText_vi", PriorityCode);

            //WorkFlow
            //Harcode: Nếu không phải là Activities hoặc Task Lấy tất cả all
            if (Type == ConstWorkFlowCategory.MyWork || Type == ConstWorkFlowCategory.MyFollow)
            {
                Type = ConstWorkFlowCategory.ALL;
            }
            var listWorkFlow = new List<WorkFlowViewModel>();
            //Nếu là thêm mới hoặc tìm kiếm => không load theo công ty
            if (isEditMode != true)
            {
                listWorkFlow = _unitOfWork.WorkFlowRepository.GetWorkFlowBy(Type);
                listWorkFlow = listWorkFlow.Where(p => p.WorkFlowCode != ConstWorkFlow.GT).ToList();
            }
            //Nếu là cập nhật => load theo công ty đang login
            else if (isEditMode == true)
            {
                listWorkFlow = _unitOfWork.WorkFlowRepository.GetWorkFlowBy(Type, CurrentUser.CompanyCode);
                if (listWorkFlow == null || listWorkFlow.Count == 0)
                {
                    listWorkFlow = new List<WorkFlowViewModel>();
                }
                //Nếu list workflow không có workflow hiện tại của task => add thêm
                var existWorkflowIdList = listWorkFlow.Select(p => p.WorkFlowId).ToList();
                if (WorkFlowId != null && WorkFlowId != Guid.Empty && !existWorkflowIdList.Contains((Guid)WorkFlowId))
                {
                    listWorkFlow.Add(new WorkFlowViewModel()
                    {
                        WorkFlowId = (Guid)WorkFlowId,
                        WorkFlowCode = WorkFlowCode,
                        WorkFlowName = NewWorkFlowName
                    });
                }
            }
            ViewBag.WorkFlowIdList = new SelectList(listWorkFlow, "WorkFlowId", "WorkFlowName", WorkFlowId);

            //TaskStatusId
            if (WorkFlowId == null)
            {
                if (listWorkFlow != null && listWorkFlow.Count > 0)
                {
                    WorkFlowId = listWorkFlow[0].WorkFlowId;
                }
            }
            //danh sách trạng thái
            var lst = _unitOfWork.TaskStatusRepository.GetTaskStatusByWorkFlow((Guid)WorkFlowId, Category);
            var TaskStatusId_GT = (from p in _context.TaskStatusModel
                                   join w in _context.WorkFlowModel on p.WorkFlowId equals w.WorkFlowId
                                   where w.WorkFlowCode == ConstWorkFlow.GT
                                   && p.TaskStatusCode == ConstWorkFlow.GT
                                   select p.TaskStatusId).FirstOrDefault();

            //cập nhật
            if (isEditMode == true && TaskStatusId != TaskStatusId_GT)
            {
                var taskProcessingList = _unitOfWork.TaskRepository.GetTaskProcessingList((Guid)TaskId);
                var existWorkFlowId = _context.TaskModel.Where(p => p.TaskId == TaskId).Select(p => p.WorkFlowId).FirstOrDefault();
                if (taskProcessingList != null && taskProcessingList.Count > 0 && existWorkFlowId == WorkFlowId)
                {
                    var taskStatusList = taskProcessingList.Select(p => p.TaskStatusId).ToList();
                    lst = lst.Where(p => taskStatusList.Contains(p.TaskStatusId)).ToList();
                }
            }
            //thêm mới
            else
            {
                if (TaskStatusId == null && WorkFlowId != null && WorkFlowId != Guid.Empty)
                {
                    var configTaskStatusId = _unitOfWork.TaskStatusRepository.GetDefaultValue((Guid)WorkFlowId, "TaskStatusId");
                    if (!string.IsNullOrEmpty(configTaskStatusId))
                    {
                        TaskStatusId = new Guid(configTaskStatusId);
                    }
                }

                ViewBag.DefaultTaskStatusId = TaskStatusId;
            }
            ViewBag.TaskStatusIdList = new SelectList(lst, "TaskStatusId", "TaskStatusName", TaskStatusId);
            ViewBag.TaskStatusId = new SelectList(lst, "TaskStatusId", "TaskStatusName", TaskStatusId);


            //CommonMistakeCode
            var commonMistakeList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CommonMistake);
            ViewBag.CommonMistakeCode = new SelectList(commonMistakeList, "CatalogCode", "CatalogText_vi", CommonMistakeCode);

            //StoreId: get all store
            var storeList = _unitOfWork.StoreRepository.GetAllStore();
            ViewBag.StoreIdList = new SelectList(storeList, "StoreId", "StoreName", StoreId);

            //CustomerType
            var customerTypeList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CustomerType);
            customerTypeList = customerTypeList.Where(p => p.CatalogCode != ConstCustomerType.Contact).ToList();
            ViewBag.CustomerTypeCode = new SelectList(customerTypeList, "CatalogCode", "CatalogText_vi");

            //Employee
            var empLst = _unitOfWork.SalesEmployeeRepository.GetAllForDropdownlist();
            ViewBag.EmployeeList = empLst;

            if (isEditMode == false)
            {
                var currentUser = _unitOfWork.SalesEmployeeRepository.GetSaleEmployeeCodeBy(CurrentUser.AccountId);
                ViewBag.EmployeeCode = currentUser;
            }
            //Role: Task Assign Type
            ViewBag.RoleList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.TaskAssignType);

            //Task Roles List (Phân công cho nhóm/phòng ban)
            var TaskRolesList = _unitOfWork.AccountRepository.GetRolesList(isEmployeeGroup: true);
            //Nếu có tạo nhóm ngoài hệ thống thì lấy thêm thông tin
            //Lấy tất cả thông tin nhóm được tạo từ CurrentUser
            var group = _context.TaskGroupModel.Where(p => p.CreatedAccountId == CurrentUser.AccountId && p.GroupType == ConstTaskGroupType.TaskGroup)
                                                .Select(p => new RolesViewModel()
                                                {
                                                    RolesCode = p.GroupId.ToString(),
                                                    RolesName = p.GroupName,
                                                }).ToList();
            if (group != null && group.Count > 0)
            {
                int groupIndex = 0;
                foreach (var item in group)
                {
                    TaskRolesList.Insert(groupIndex, item);
                    groupIndex++;
                }
            }
            ViewBag.TaskRolesList = TaskRolesList;
            //Reporter
            ViewBag.Reporter = Reporter;
            ViewBag.ReporterList = new SelectList(empLst, "SalesEmployeeCode", "SalesEmployeeName", Reporter);

            //Assignee
            ViewBag.Assignee = Assignee;
            ViewBag.AssigneeList = new SelectList(empLst, "SalesEmployeeCode", "SalesEmployeeName", Assignee);

            //CreateBy
            ViewBag.CreateBy = new SelectList(empLst, "SalesEmployeeCode", "SalesEmployeeName");

            //Profile
            //var profiles = _unitOfWork.ProfileRepository.GetProfiles();
            //ViewBag.ProfileId = new SelectList(profiles, "ProfileId", "ProfileName");

            //Contact
            if (ProfileId != null)
            {
                var contacts = _unitOfWork.ProfileRepository.GetContactListOfProfile(ProfileId);
                ViewBag.ContactId = new SelectList(contacts, "ProfileContactId", "ProfileContactName", ContactId);
            }
            else
            {
                var contacts = new List<ProfileContactViewModel>();
                ViewBag.ContactId = new SelectList(contacts, "ProfileId", "ProfileName");
            }
            //Construction unit
            if (ConstructionUnit != null)
            {
                var contacts = (from p in _context.ProfileContactAttributeModel
                                join pro in _context.ProfileModel on p.ProfileId equals pro.ProfileId
                                where p.CompanyId == ConstructionUnit
                                select new ConstructionUnitContactViewModel
                                {
                                    ContactId = pro.ProfileId,
                                    ContactName = pro.ProfileShortName != null ? pro.ProfileShortName : pro.ProfileName,
                                    IsMain = p.IsMain
                                }).OrderByDescending(p => p.IsMain == true).ToList();
                ViewBag.ConstructionUnitContact = new SelectList(contacts, "ContactId", "ContactName", ConstructionUnitContact);
            }
            else
            {
                var contacts = new List<ConstructionUnitContactViewModel>();
                ViewBag.ConstructionUnitContact = new SelectList(contacts, "ContactId", "ContactName");
            }

            //Task Status Color
            string TaskStatusBackgroundColor = "";
            string TaskStatusColor = "";
            //Todo
            if (ProcessCode == ConstProcess.todo)
            {
                TaskStatusBackgroundColor = "#fff";
                TaskStatusColor = "#000";
            }
            //Processing
            else if (ProcessCode == ConstProcess.processing)
            {
                TaskStatusBackgroundColor = "#0052CC";
                TaskStatusColor = "#fff";
            }
            //Completed
            else if (ProcessCode == ConstProcess.completed)
            {
                TaskStatusBackgroundColor = "#398439";
                TaskStatusColor = "#fff";
            }
            ViewBag.TaskStatusBackgroundColor = TaskStatusBackgroundColor;
            ViewBag.TaskStatusColor = TaskStatusColor;

            //Title on modal popup
            if (!string.IsNullOrEmpty(WorkFlowName) && TaskCode != null)
            {
                string editUrl = string.Empty;
                if (isInPopup == true)
                {
                    editUrl = string.Format("<a href='/Work/Task/Edit/{0}'><i class='fa fa-eye mr-5'></i></a> ", TaskId);
                }
                string CommentTitle = editUrl;
                if (ProfileId != null)
                {
                    var profileName = _unitOfWork.TaskRepository.GetProfileByTask(TaskCode).ProfileName;
                    if (!string.IsNullOrEmpty(profileName))
                    {
                        CommentTitle += profileName + "-";
                    }
                }
                CommentTitle += string.Format("{0}-{1}", WorkFlowName.Trim(), !string.IsNullOrEmpty(SubtaskCode) ? SubtaskCode : TaskCode.ToString());
                ViewBag.CommentTitle = CommentTitle;
            }

            //Name on logo
            var logoName = (from p in _context.SalesEmployeeModel
                            where p.SalesEmployeeCode == CurrentUser.EmployeeCode
                            select p.SalesEmployeeName).FirstOrDefault();
            logoName = logoName.GetCharacterForLogoName();
            ViewBag.LogoName = logoName;

            //ServiceTechnicalTeamCode
            var serviceLst = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.ServiceTechnicalTeam);
            ViewBag.ServiceTechnicalTeamCode = new SelectList(serviceLst, "CatalogCode", "CatalogText_vi", ServiceTechnicalTeamCode);

            //ErrorTypeCode
            var errorTypeLst = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.ErrorType);
            ViewBag.ErrorTypeCode = new SelectList(errorTypeLst, "CatalogCode", "CatalogText_vi", ErrorTypeCode);

            //ErrorCode
            var errorLst = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.Error);
            ViewBag.ErrorCode = new SelectList(errorLst, "CatalogCode", "CatalogText_vi", ErrorCode);

            //ErrorCode
            //var accErrorTypeLst = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.ErrorType);
            // ViewBag.AccErrorTypeCode = new SelectList(errorTypeLst, "CatalogCode", "CatalogText_vi", AccErrorTypeCode);

            //VisitTypeCode
            var visitTypeLst = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.VisitType);
            ViewBag.VisitTypeCode = new SelectList(visitTypeLst, "CatalogCode", "CatalogText_vi", VisitTypeCode);

            //VisitSaleOfficeCode
            var visitSaleOfficeLst = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.SaleOffice);
            ViewBag.VisitSaleOfficeCode = new SelectList(visitSaleOfficeLst, "CatalogCode", "CatalogText_vi", VisitSaleOfficeCode);

            //RemindCycle
            var remindCycleLst = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.RemindCycle);
            ViewBag.RemindCycle = new SelectList(remindCycleLst, "CatalogCode", "CatalogText_vi", RemindCycle);

            //Mã màu SP
            var productColorLst = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.ProductColor);
            ViewBag.ProductColorCode = new SelectList(productColorLst, "CatalogCode", "CatalogCode", ProductColorCode);

            var config = _context.WorkFlowConfigModel.Where(p => p.WorkFlowId == WorkFlowId && p.FieldCode == "ProductCategoryCode")
                                     .Select(p => p.Parameters).FirstOrDefault();

            //Nhóm vật tư
            var productSearchCategoryLst = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.ProductCategory);
            if (IsEditProduct == true && !string.IsNullOrEmpty(config))
            {
                var productCategoryLst = productSearchCategoryLst.Where(p => p.CatalogText_en.Contains(config)).ToList();
                ViewBag.ProductCategoryCode = new SelectList(productCategoryLst, "CatalogCode", "CatalogText_vi", ProductCategoryCode);
            }
            else
            {
                ViewBag.ProductCategoryCode = new SelectList(productSearchCategoryLst, "CatalogCode", "CatalogText_vi", ProductCategoryCode);
            }

            //Các lỗi bảo hành thuờng gặp
            if (IsEditProduct == true && !string.IsNullOrEmpty(ProductCategoryCode))
            {
                var usualErrorLst = _unitOfWork.TaskRepository.GetUsualErrorByProductCategory(ProductCategoryCode, config);
                //ViewBag.UsualErrorCode = new SelectList(usualErrorLst, "CatalogCode", "CatalogText_vi", UsualErrorCodeList);
                List<SelectListItem> usualErrorSelectList = new List<SelectListItem>();
                foreach (var item in usualErrorLst)
                {
                    var i = new SelectListItem()
                    {
                        Text = item.CatalogText_vi,
                        Value = item.CatalogCode.ToString(),

                    };
                    if (UsualErrorCodeList != null)
                    {
                        i.Selected = UsualErrorCodeList.Contains(item.CatalogCode);
                    }
                    usualErrorSelectList.Add(i);
                }
                ViewBag.UsualErrorCode = usualErrorSelectList;
                ViewBag.IsEditProduct = IsEditProduct;
            }
            else
            {
                var usualErrorLst = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.UsualError);
                ViewBag.UsualErrorCode = new SelectList(usualErrorLst, "CatalogCode", "CatalogText_vi");
            }
            //Nguồn tiếp nhận (TaskSourceCode)
            var taskSourceList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.TaskSource)
                                                                .Select(p => new SelectListItem()
                                                                {
                                                                    Text = p.CatalogText_vi,
                                                                    Value = p.CatalogCode,
                                                                });
            ViewBag.TaskSourceCode = new SelectList(taskSourceList, "Value", "Text", TaskSourceCode);

            //Hài lòng khách hàng
            var customerSatisfactionLst = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CustomerSatisfaction)
                                                                .Select(p => new SelectListItem()
                                                                {
                                                                    Text = p.CatalogText_vi,
                                                                    Value = p.CatalogCode,
                                                                });
            ViewBag.CustomerSatisfactionCode = new SelectList(customerSatisfactionLst, "Value", "Text", CustomerSatisfactionCode);

            //Ý kiến khách hàng
            var customerRatings = new List<SelectListItem>();
            customerRatings.Add(new SelectListItem()
            {
                Value = "none",
                Text = "Không ý kiến"
            });
            customerRatings.Add(new SelectListItem()
            {
                Value = "rating",
                Text = "Đánh giá theo sao & ý kiến khác"
            });
            //customerRatings.Add(new SelectListItem()
            //{
            //    Value = "other",
            //    Text = "Khác"
            //});
            ViewBag.CustomerRatings = new SelectList(customerRatings, "Value", "Text", CustomerRatings);
            //Đánh giá theo sao
            //1. Về sản phẩm
            var ticket_CustomerReviews_Product = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.Ticket_CustomerReviews_Product)
                                                                            .Select(p => new SelectListItem()
                                                                            {
                                                                                Value = p.CatalogCode,
                                                                                Text = p.CatalogText_vi,
                                                                            });
            ViewBag.Ticket_CustomerReviews_Product = new SelectList(ticket_CustomerReviews_Product, "Value", "Text", Ticket_CustomerReviews_Product);
            //2. Về dịch vụ
            var ticket_CustomerReviews_Service = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.Ticket_CustomerReviews_Service)
                                                                          .Select(p => new SelectListItem()
                                                                          {
                                                                              Value = p.CatalogCode,
                                                                              Text = p.CatalogText_vi,
                                                                          });
            ViewBag.Ticket_CustomerReviews_Service = new SelectList(ticket_CustomerReviews_Service, "Value", "Text", Ticket_CustomerReviews_Service);
            #endregion

            #region Appointment
            if (isEditMode == true && !string.IsNullOrEmpty(WorkFlowCode) && WorkFlowCode == ConstWorkFlow.GT)
            {
                //ShowRoom
                var showroomList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CustomerSource);
                ViewBag.ShowroomCode = new SelectList(showroomList, "CatalogCode", "CatalogText_vi", ShowroomCode);

                //Phân loại khách hàng
                var customerClassList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CustomerClass);
                ViewBag.CustomerClassCode = new SelectList(customerClassList, "CatalogCode", "CatalogText_vi", CustomerClassCode);
                ViewBag.CustomerClassCodeList = new SelectList(customerClassList, "CatalogCode", "CatalogText_vi", CustomerClassCode);

                //Danh mục
                var appointmentCategoryList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.Appoitment_Category);
                ViewBag.CategoryCode = new SelectList(appointmentCategoryList, "CatalogCode", "CatalogText_vi", CategoryCode);
                ViewBag.CategoryCodeList = new SelectList(appointmentCategoryList, "CatalogCode", "CatalogText_vi", CategoryCode);

                //Khách biết đến ACN qua
                var channelList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.Appoitment_Channel);
                ViewBag.ChannelCode = new SelectList(channelList, "CatalogCode", "CatalogText_vi", ChannelCode);

                //Người tiếp khách
                var _salesEmployeeRepository = new SalesEmployeeRepository(_context);
                var saleEmployeeList = _salesEmployeeRepository.GetAllForDropdownlist();
                ViewBag.SaleEmployeeCode = new SelectList(saleEmployeeList, "SalesEmployeeCode", "SalesEmployeeName", SaleEmployeeCode);

                //Ý kiến khách hàng
                var ratingLst = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CustomerReviews);
                ViewBag.Ratings = new SelectList(ratingLst, "CatalogCode", "CatalogText_vi", Ratings);
            }
            #endregion

            #region Product
            if (isEditMode == true)
            {
                //Phân loại vật tư
                var PhanLoaiVatTu = _context.CategoryModel.Where(p => p.CategoryCode == ConstCategoryCode.PHANLOAIVATTU).FirstOrDefault();
                var parentCategoryList = _context.CategoryModel.Where(p => p.ParentCategoryId == PhanLoaiVatTu.CategoryId && p.Actived == true)
                                                             .Select(p => new
                                                             {
                                                                 CategoryId = p.CategoryId,
                                                                 CategoryCode = p.CategoryCode,
                                                                 CategoryName = p.CategoryCode + " | " + p.CategoryName,
                                                                 OrderIndex = p.OrderIndex
                                                             })
                                                             .OrderBy(p => p.CategoryCode).ToList();
                ViewBag.ParentCategoryId = new SelectList(parentCategoryList, "CategoryId", "CategoryName");
                ViewBag.SearchParentCategoryId = new SelectList(parentCategoryList, "CategoryId", "CategoryName");

                //Nhóm vật tư
                var NhomVatTu = _context.CategoryModel.Where(p => p.CategoryCode == ConstCategoryCode.NHOMVATTU).FirstOrDefault();
                var categoryList = _context.CategoryModel.Where(p => p.ParentCategoryId == NhomVatTu.CategoryId && p.Actived == true)
                                                             .Select(p => new
                                                             {
                                                                 CategoryId = p.CategoryId,
                                                                 CategoryCode = p.CategoryCode,
                                                                 CategoryName = p.CategoryCode + " | " + p.CategoryName,
                                                                 OrderIndex = p.OrderIndex
                                                             })
                                                             .OrderBy(p => p.CategoryCode).ToList();
                ViewBag.CategoryId = new SelectList(categoryList, "CategoryId", "CategoryName");
                ViewBag.SearchCategoryId = new SelectList(categoryList, "CategoryId", "CategoryName");

                //Loại phụ kiện
                var productAccessoryTypeCode = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.ProductAccessoryType);
                //ViewBag.ProductAccessoryTypeCode = new SelectList(productAccessoryTypeCode, "CatalogCode", "CatalogText_vi");
                ViewBag.ProductAccessoryTypeCode = productAccessoryTypeCode;

                //Hình thức BH PK
                ViewBag.AccErrorTypeCode = errorTypeLst;

                //Phân cấp SP
                var productLevelLst = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.ProductLevel);
                ViewBag.ProductLevelCode = new SelectList(productLevelLst, "CatalogCode", "CatalogText_vi", ProductLevelCode);

                //ĐVT
                var unitLst = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.Unit);
                ViewBag.Unit = new SelectList(unitLst, "CatalogCode", "CatalogText_vi", Unit);
            }
            #endregion        
            //isCreatePrivateTask
            ViewBag.isCreatePrivateTask = _context.AccountModel.Where(p => p.AccountId == CurrentUser.AccountId).Select(p => p.isCreatePrivateTask).FirstOrDefault();

        }

        private void CreateSearchViewBag(string Type, string TaskProcessCode = null)
        {
            #region CommonDate
            var SelectedCommonDate = "Custom";
            //Common Date
            var commonDateList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CommonDate);
            ViewBag.CommonDate = new SelectList(commonDateList, "CatalogCode", "CatalogText_vi", SelectedCommonDate);

            //Common Date 2
            var commonDateList2 = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CommonDate2);
            ViewBag.CommonDate2 = new SelectList(commonDateList2, "CatalogCode", "CatalogText_vi", SelectedCommonDate);
            #endregion

            #region //TaskStatusCode - Trạng thái
            var isShowTaskStatusCode = true;
            if (Type == ConstWorkFlowCategory.MyFollow || Type == ConstWorkFlowCategory.MyWork)
            {
                isShowTaskStatusCode = false;
            }
            else
            {
                var result = (from wf in _context.WorkFlowModel
                              join ts in _context.TaskStatusModel on wf.WorkFlowId equals ts.WorkFlowId
                              where wf.WorkflowCategoryCode == Type
                              select new
                              {
                                  ts.OrderIndex,
                                  ts.TaskStatusCode,
                                  ts.TaskStatusName
                              }).Distinct().OrderBy(p => p.OrderIndex).ToList();
                if (result != null && result.Count > 0)
                {
                    ViewBag.TaskStatusCode = new SelectList(result, "TaskStatusCode", "TaskStatusName");
                }
                else
                {
                    isShowTaskStatusCode = false;
                }
            }
            ViewBag.isShowTaskStatusCode = isShowTaskStatusCode;
            #endregion

            #region //Kanban
            var kanban = _context.KanbanModel.Where(p => p.KanbanCode == Type && p.Actived == true).FirstOrDefault();
            if (kanban != null)
            {
                ViewBag.KanbanId = kanban.KanbanId;
            }
            #endregion

            #region //TaskProcessCode 
            var statusLst = _unitOfWork.TaskStatusRepository.GetTaskStatusList();
            ViewBag.TaskProcessCode = new SelectList(statusLst, "StatusCode", "StatusName", TaskProcessCode);
            #endregion

            #region //Get list CustomerGroup (Nhóm khách hàng doanh nghiệp)
            var customerGroupList = _unitOfWork.CatalogRepository.GetCustomerCategory(CurrentUser.CompanyCode);
            ViewBag.ProfileGroupCode = new SelectList(customerGroupList, "CatalogCode", "CatalogText_vi");
            #endregion

            #region //Get list SalesSupervisor (NV kinh doanh)
            var empList = _unitOfWork.PersonInChargeRepository.GetListEmployee();
            ViewBag.SalesSupervisorCode = new SelectList(empList, "SalesEmployeeCode", "SalesEmployeeName");
            ViewBag.CompletedEmployee = new SelectList(empList, "SalesEmployeeCode", "SalesEmployeeName");
            #endregion

            #region //Get list Roles (Phòng ban)
            var rolesList = _context.RolesModel.Where(p => p.Actived == true && p.isEmployeeGroup == true).ToList();
            ViewBag.DepartmentCode = new SelectList(rolesList, "RolesCode", "RolesName");
            #endregion

            #region //Actived (Trạng thái hoạt động)
            var activedList = new List<ISDSelectBoolItem>();
            activedList.Add(new ISDSelectBoolItem()
            {
                id = null,
                name = "Tất cả",
            }); activedList.Add(new ISDSelectBoolItem()
            {
                id = true,
                name = "Hoạt động",
            });
            activedList.Add(new ISDSelectBoolItem()
            {
                id = false,
                name = "Đã hủy",
            });
            ViewBag.Actived = new SelectList(activedList, "id", "name");
            #endregion

            #region //Get list ServiceTechnicalTeamCode (Trung tâm bảo hành)
            var serviceTechnicalTeamCodeList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.ServiceTechnicalTeam);
            ViewBag.ServiceTechnicalTeamCode = new SelectList(serviceTechnicalTeamCodeList, "CatalogCode", "CatalogText_vi");
            #endregion

            //VisitTypeCode
            var visitTypeLst = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.VisitType);
            ViewBag.VisitTypeCode = new SelectList(visitTypeLst, "CatalogCode", "CatalogText_vi");

            #region //Filters
            var filterLst = new List<DropdownlistFilter>();
            //Khách hàng
            filterLst.Add(new DropdownlistFilter { FilterCode = ConstFilter.WorkFlowId, FilterName = LanguageResource.Type });
            //Liên hệ
            filterLst.Add(new DropdownlistFilter { FilterCode = ConstFilter.ContactId, FilterName = LanguageResource.Profile_Contact });
            //Người tạo
            filterLst.Add(new DropdownlistFilter { FilterCode = ConstFilter.CreateBy, FilterName = LanguageResource.CreateBy });
            //Mức độ
            filterLst.Add(new DropdownlistFilter { FilterCode = ConstFilter.PriorityCode, FilterName = LanguageResource.Task_PriorityCode });
            //Ngày tạo
            if (Type == ConstWorkFlowCategory.TICKET_MLC)
            {
                filterLst.Add(new DropdownlistFilter { FilterCode = ConstFilter.CreateDate, FilterName = LanguageResource.CreatedDate });
            }
            //Ngày tiếp nhận
            filterLst.Add(new DropdownlistFilter { FilterCode = ConstFilter.ReceiveDate, FilterName = LanguageResource.Task_ReceiveDate });
            //Ngày bắt đầu
            //filterLst.Add(new DropdownlistFilter { FilterCode = ConstFilter.StartDate, FilterName = LanguageResource.Task_StartDate });
            //Ngày đến hạn
            filterLst.Add(new DropdownlistFilter { FilterCode = ConstFilter.EstimateEndDate, FilterName = LanguageResource.Task_EstimateEndDate });
            //Ngày kết thúc
            filterLst.Add(new DropdownlistFilter { FilterCode = ConstFilter.EndDate, FilterName = LanguageResource.Task_EndDate });
            //filterLst.Add(new DropdownlistFilter { FilterCode = ConstFilter.ConstructionUnit, FilterName = LanguageResource.ConstructionUnit });
            //filterLst.Add(new DropdownlistFilter { FilterCode = ConstFilter.CommonMistakeCode, FilterName = LanguageResource.Task_CommonMistakeCode });

            //Hình thức bảo hành
            filterLst.Add(new DropdownlistFilter { FilterCode = ConstFilter.ErrorTypeCode, FilterName = LanguageResource.Task_ErrorTypeCode2 });
            //Phương thức xử lý
            filterLst.Add(new DropdownlistFilter { FilterCode = ConstFilter.ErrorCode, FilterName = LanguageResource.Task_ErrorCode2 });
            //Nhóm vật tư
            filterLst.Add(new DropdownlistFilter { FilterCode = ConstFilter.ProductCategoryCode, FilterName = LanguageResource.Sale_Category });
            //Các lỗi BH thường gặp
            filterLst.Add(new DropdownlistFilter { FilterCode = ConstFilter.UsualErrorCode, FilterName = LanguageResource.UsualErrorCode });
            //Mã màu
            filterLst.Add(new DropdownlistFilter { FilterCode = ConstFilter.ProductColorCode, FilterName = LanguageResource.ProductColorCode });
            //Nhóm KH
            filterLst.Add(new DropdownlistFilter { FilterCode = ConstFilter.CustomerGroupCode, FilterName = LanguageResource.Profile_General_CustomerGroup });
            //NV kinh doanh
            filterLst.Add(new DropdownlistFilter { FilterCode = ConstFilter.SalesSupervisorCode, FilterName = LanguageResource.PersonInCharge });
            //Phòng ban
            filterLst.Add(new DropdownlistFilter { FilterCode = ConstFilter.DepartmentCode, FilterName = LanguageResource.Profile_Department });
            //Trạng thái hoạt động
            filterLst.Add(new DropdownlistFilter { FilterCode = ConstFilter.Actived, FilterName = "Trạng thái hoạt động" });
            //Trung tâm bảo hành
            if (Type == ConstWorkFlowCategory.TICKET_MLC)
            {
                filterLst.Add(new DropdownlistFilter { FilterCode = ConstFilter.ServiceTechnicalTeamCode, FilterName = LanguageResource.Task_ServiceTechnicalTeamCode });
                filterLst.Add(new DropdownlistFilter { FilterCode = ConstFilter.CompletedEmployee, FilterName = LanguageResource.CompletedEmployee });
            }
            //Phân loại chuyến thăm
            if (Type == ConstWorkFlowCategory.THKH)
            {
                filterLst.Add(new DropdownlistFilter { FilterCode = ConstFilter.VisitTypeCode, FilterName = LanguageResource.Task_VisitTypeCode });
            }
            ViewBag.Filters = filterLst;
            #endregion
        }
        #endregion
    }
}