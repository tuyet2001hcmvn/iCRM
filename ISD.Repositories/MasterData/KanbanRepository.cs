using ISD.Constant;
using ISD.EntityModels;
using ISD.Repositories.Infrastructure.Extensions;
using ISD.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.Repositories
{
    public class KanbanRepository
    {
        EntityDataContext _context;

        /// <summary>
        /// Khởi tạo Kanban Repository
        /// </summary>
        /// <param name="db">EntityDataContext</param>
        public KanbanRepository(EntityDataContext db)
        {
            _context = db;
        }

        /// <summary>
        /// Tìm kiếm kanban
        /// </summary>
        /// <param name="searchModel">Kanban Search Model</param>
        /// <returns>List WorkFlowViewModel</returns>
        public List<KanbanViewModel> Search(KanbanSearchViewModel searchModel)
        {
            var lst = (from p in _context.KanbanModel
                       join a in _context.AccountModel on p.CreateBy equals a.AccountId
                       where
                       (searchModel.SearchKanbanCode == null || p.KanbanCode.Contains(searchModel.SearchKanbanCode))
                       && (searchModel.SearchKanbanName == null || p.KanbanName.Contains(searchModel.SearchKanbanName))
                       && (searchModel.Actived == null || p.Actived == searchModel.Actived)
                       orderby p.OrderIndex
                       select new KanbanViewModel
                       {
                           KanbanId = p.KanbanId,
                           KanbanCode = p.KanbanCode,
                           KanbanName = p.KanbanName,
                           OrderIndex = p.OrderIndex,
                           Actived = p.Actived,
                           CreateBy = p.CreateBy,
                           CreateTime = p.CreateTime,
                           CreateByName = a.UserName
                       }).ToList();
            return lst;
        }

        /// <summary>
        /// Get All kanban
        /// </summary>
        /// <returns>Danh sách kanban</returns>
        public List<KanbanViewModel> GetAll()
        {
            var lst = (from p in _context.KanbanModel
                       where p.Actived == true
                       select new KanbanViewModel
                       {
                           KanbanId = p.KanbanId,
                           KanbanCode = p.KanbanCode,
                           KanbanName = p.KanbanName,
                           OrderIndex = p.OrderIndex,
                           Actived = p.Actived,
                           CreateBy = p.CreateBy,
                           CreateTime = p.CreateTime
                       }).ToList();
            return lst;
        }

        /// <summary>
        /// Lấy kanban theo kanbanId
        /// </summary>
        /// <param name="kanbanId">Guid kanbanId</param>
        /// <returns>KanbanViewModel</returns>
        public KanbanViewModel GetKanban(Guid kanbanId)
        {
            var workFlow = (from w in _context.KanbanModel
                            where w.KanbanId == kanbanId
                            select new KanbanViewModel
                            {
                                KanbanId = w.KanbanId,
                                KanbanCode = w.KanbanCode,
                                KanbanName = w.KanbanName,
                                OrderIndex = w.OrderIndex,
                                Actived = w.Actived,
                                CreateBy = w.CreateBy,
                                CreateTime = w.CreateTime,
                                LastEditBy = w.LastEditBy,
                                LastEditTime = w.LastEditTime
                            })
                            .FirstOrDefault();
            return workFlow;
        }

        /// <summary>
        /// Thêm mới kanban
        /// </summary>
        /// <param name="model">KanbanModel</param>
        /// <returns>KanbanModel</returns>
        public KanbanModel Create(KanbanModel model, List<KanbanDetailViewModel> detailList)
        {
            //master
            model.KanbanId = Guid.NewGuid();
            model.CreateTime = DateTime.Now;
            
            //detail
            if (detailList != null && detailList.Count > 0)
            {
                foreach (var item in detailList)
                {
                    KanbanDetailModel detail = new KanbanDetailModel();
                    detail.KanbanDetailId = Guid.NewGuid();
                    detail.KanbanId = model.KanbanId;
                    detail.ColumnName = item.ColumnName;
                    detail.OrderIndex = item.OrderIndex;
                    detail.Note = item.Note;
                    detail.CreateBy = model.CreateBy;
                    detail.CreateTime = DateTime.Now;
                    model.KanbanDetailModel.Add(detail);
                }
            }
            _context.KanbanModel.Add(model);
            return model;
        }

        /// <summary>
        /// Cập nhật kanban
        /// </summary>
        /// <param name="viewModel">KanbanViewModel</param>
        public void Update(KanbanViewModel viewModel, List<KanbanDetailViewModel> detailList)
        {
            var kanban = _context.KanbanModel.FirstOrDefault(p => p.KanbanId == viewModel.KanbanId);
            if (kanban != null)
            {
                //master
                kanban.KanbanCode = viewModel.KanbanCode;
                kanban.KanbanName = viewModel.KanbanName;
                kanban.OrderIndex = viewModel.OrderIndex;
                kanban.Actived = viewModel.Actived;
                kanban.LastEditBy = viewModel.LastEditBy;
                kanban.LastEditTime = DateTime.Now;
                _context.Entry(kanban).State = EntityState.Modified;

                //detail
                //Danh sách detail từ view
                var detailIdList = detailList.Where(p => p.KanbanDetailId != null).Select(p => p.KanbanDetailId).ToList();
                //Danh sách detail từ db
                var existDetailList = _context.KanbanDetailModel.Where(p => p.KanbanId == viewModel.KanbanId)
                                              .Select(p => p.KanbanDetailId).ToList();
                foreach (var item in existDetailList)
                {
                    //Nếu có trong db nhưng ko có trong view => Xoá
                    if (!detailIdList.Contains(item))
                    {
                        //Kanban Mapping
                        var delItem_Mapping = _context.Kanban_TaskStatus_Mapping.Where(p => p.KanbanDetailId == item).ToList();
                        _context.Kanban_TaskStatus_Mapping.RemoveRange(delItem_Mapping);

                        //Kanban Detail
                        var delItem = _context.KanbanDetailModel.FirstOrDefault(p => p.KanbanDetailId == item);
                        if (delItem != null)
                        {
                            _context.Entry(delItem).State = EntityState.Deleted;
                        }
                    }
                }

                if (detailList != null && detailList.Count > 0)
                {
                    foreach (var item in detailList)
                    {
                        //Nếu có Id => Sửa
                        if (item.KanbanDetailId != null && item.KanbanDetailId != Guid.Empty)
                        {
                            var existDetail = _context.KanbanDetailModel.FirstOrDefault(p => p.KanbanDetailId == item.KanbanDetailId);
                            if (existDetail != null)
                            {
                                existDetail.ColumnName = item.ColumnName;
                                existDetail.OrderIndex = item.OrderIndex;
                                existDetail.Note = item.Note;
                                existDetail.LastEditBy = viewModel.LastEditBy;
                                existDetail.LastEditTime = DateTime.Now;
                            }
                        }
                        //Nếu không có Id => Thêm
                        else
                        {
                            KanbanDetailModel detail = new KanbanDetailModel();
                            detail.KanbanDetailId = Guid.NewGuid();
                            detail.KanbanId = viewModel.KanbanId;
                            detail.ColumnName = item.ColumnName;
                            detail.OrderIndex = item.OrderIndex;
                            detail.Note = item.Note;
                            detail.CreateBy = viewModel.LastEditBy;
                            detail.CreateTime = DateTime.Now;
                            kanban.KanbanDetailModel.Add(detail);
                        }
                    }
                }
            }
        }
    }
}
