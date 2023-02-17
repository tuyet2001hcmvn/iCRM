using ISD.EntityModels;
using ISD.Extensions;
using ISD.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.Repositories
{
    public class PivotGridTemplateRepository
    {
        EntityDataContext _context;
       
        /// <summary>
        /// Khởi tạo repository
        /// </summary>
        /// <param name="dataContext">EntityDataContext</param>
        public PivotGridTemplateRepository(EntityDataContext dataContext)
        {
            _context = dataContext;
        }
        public void Create(string  templateName,bool isDefault, bool isSystem, Guid? accountId, Guid pageId, List<FieldSettingModel> settings, string LayoutConfigs = null, string orderBy = null, string typeSort = null)
        {
            SearchResultTemplateModel newTemplate = new SearchResultTemplateModel();
            newTemplate.SearchResultTemplateId = Guid.NewGuid();
            newTemplate.TemplateName = templateName;
            newTemplate.PageId = pageId;
            newTemplate.LayoutConfigs = LayoutConfigs;
            // ASC: Tăng dần, DESC: Giảm dần
            newTemplate.TypeSort = typeSort;
            // Sort theo:
            newTemplate.OrderBy = orderBy;
            if (isSystem)
            {
                newTemplate.isSystem = true;
                // nếu template mới là system + default => set các template system khác default = false
                if (isDefault)
                {
                    newTemplate.IsDefaultTemplate = true;
                    var existingTempalte = _context.SearchResultTemplateModel.Where(s => s.PageId == pageId && s.isSystem == true).ToList();
                    if (existingTempalte.Count > 0)
                    {
                        foreach(var item in existingTempalte)
                        {
                            item.IsDefaultTemplate = false;
                            _context.Entry(item).State = EntityState.Modified;
                        }    
                    }
                }
            }
            else
            {
                newTemplate.isSystem = false;
                newTemplate.AccountId = accountId;
                //nếu template mới là default + user => set các user template khác default = false
                if (isDefault)
                {
                    newTemplate.IsDefaultTemplate = true;
                    var existingTempalte = _context.SearchResultTemplateModel.Where(s => s.PageId == pageId && s.isSystem == false && s.AccountId == accountId).ToList();
                    if (existingTempalte.Count > 0)
                    {
                        foreach (var item in existingTempalte)
                        {
                            item.IsDefaultTemplate = false;
                            _context.Entry(item).State = EntityState.Modified;
                        }
                    }
                }
            }
            _context.SearchResultTemplateModel.Add(newTemplate);
            // add detail 
            foreach (var field in settings)
            {
                SearchResultDetailTemplateModel detail = new SearchResultDetailTemplateModel();
                detail.SearchResultTemplateId = newTemplate.SearchResultTemplateId;
                detail.SearchResultDetailTemplateId = Guid.NewGuid();
                detail.PivotArea = field.PivotArea;
                detail.FieldName = field.FieldName;
                detail.CellFormat_FormatType = field.CellFormat_FormatType;
                detail.CellFormat_FormatString = field.CellFormat_FormatString;
                detail.Caption = field.Caption;
                detail.AreaIndex = field.AreaIndex;
                detail.Visible = field.Visible;
                detail.Width = field.Width;
                _context.SearchResultDetailTemplateModel.Add(detail);
            }
            
        }
        public void Update(Guid templateId, string templateName,bool isDefault, List<FieldSettingModel> settings, string LayoutConfigs = null, string orderBy = null, string typeSort = null)
        {
            SearchResultTemplateModel update = _context.SearchResultTemplateModel.FirstOrDefault(s => s.SearchResultTemplateId == templateId);
            if(update !=null)
            {
                update.TemplateName = templateName;
                update.OrderBy = orderBy;
                update.TypeSort = typeSort;
                update.LayoutConfigs = LayoutConfigs;

                if (isDefault)
                {
                    //nếu là system + default => set default các system template khác => false
                    if (update.isSystem == true)
                    {

                        var existingTempalte = _context.SearchResultTemplateModel.Where(s => s.PageId == update.PageId && s.isSystem == true).ToList();
                        if (existingTempalte.Count > 0)
                        {
                            foreach (var item in existingTempalte)
                            {
                                item.IsDefaultTemplate = false;
                                _context.Entry(item).State = EntityState.Modified;
                            }
                        }
                    }
                    else
                    {
                        //nếu là user template + default => set default các user template khác => false
                        var existingTempalte = _context.SearchResultTemplateModel.Where(s => s.PageId == update.PageId && s.isSystem == false && s.AccountId == update.AccountId).ToList();
                        if (existingTempalte.Count > 0)
                        {
                            foreach (var item in existingTempalte)
                            {
                                item.IsDefaultTemplate = false;
                                _context.Entry(item).State = EntityState.Modified;
                            }
                        }
                    }
                    update.IsDefaultTemplate = isDefault;
                    _context.Entry(update).State = EntityState.Modified;
                }
                else
                {
                    update.IsDefaultTemplate = isDefault;
                    _context.Entry(update).State = EntityState.Modified;
                }
                // update detail
                if(settings !=null && settings.Count>0)
                {
                    var detail = _context.SearchResultDetailTemplateModel.Where(s => s.SearchResultTemplateId == templateId).ToList();
                    foreach (var item in detail)
                    {
                        foreach (var setting in settings)
                        {
                            // update từnng field
                            if (item.FieldName == setting.FieldName)
                            {
                                item.PivotArea = setting.PivotArea;
                                item.FieldName = setting.FieldName;
                                item.CellFormat_FormatType = setting.CellFormat_FormatType;
                                item.CellFormat_FormatString = setting.CellFormat_FormatString;
                                item.Caption = setting.Caption;
                                item.AreaIndex = setting.AreaIndex;
                                item.Visible = setting.Visible;
                                item.Width = setting.Width;
                                _context.Entry(item).State = EntityState.Modified;
                            }
                        }
                    }
                }    
                
            }


        }
        public void Delete(Guid templateId)
        {
            SearchResultTemplateModel delete = _context.SearchResultTemplateModel.FirstOrDefault(s => s.SearchResultTemplateId == templateId);
            if (delete != null)
            {
                _context.Entry(delete).State = EntityState.Deleted;
                var detail = _context.SearchResultDetailTemplateModel.Where(s => s.SearchResultTemplateId == templateId).ToList();
                foreach (var item in detail)
                {  
                  _context.Entry(item).State = EntityState.Deleted;                   
                }
            }
        }
        public List<PivotTemplateViewModel> GetSystemTemplate(Guid pageId)
        {
            var list = _context.SearchResultTemplateModel.Where(s => s.isSystem == true && s.PageId == pageId)
                                                           .OrderBy(n => n.TemplateName)
                                                          .Select(s => new PivotTemplateViewModel
                                                          {
                                                              SearchResultTemplateId = s.SearchResultTemplateId,
                                                              TemplateName = s.TemplateName,
                                                              IsDefault = s.IsDefaultTemplate,
                                                              OrderBy = s.OrderBy,
                                                              TypeSort = s.TypeSort
                                                          }).ToList();
            if (list != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    int? index = null;
                    if (item.TemplateName.Contains("."))
                    {
                        index = Convert.ToInt32(item.TemplateName.Split('.')[0]);
                    }
                    item.OrderIndex = index;
                }

                list = list.OrderByDescending(p => p.OrderIndex.HasValue).ThenBy(p => p.OrderIndex).ToList();
            }
            return list;
        }
        public List<PivotTemplateViewModel> GetUserTemplate(Guid pageId, Guid accountId)
        {
            var list = _context.SearchResultTemplateModel.Where(s => s.AccountId == accountId && s.PageId == pageId)
                                                           .OrderBy(n => n.TemplateName)
                                                          .Select(s => new PivotTemplateViewModel
                                                          {
                                                              SearchResultTemplateId = s.SearchResultTemplateId,
                                                              TemplateName = s.TemplateName,
                                                              IsDefault = s.IsDefaultTemplate
                                                          }).ToList();
            return list;
        }
        public List<FieldSettingModel> GetSettingByTemplate(Guid templateId)
        {
            var list = _context.SearchResultDetailTemplateModel.Where(s => s.SearchResultTemplateId == templateId)
                                                          .Select(s => new FieldSettingModel
                                                          {
                                                              FieldName = s.FieldName,
                                                              Caption = s.Caption,
                                                              PivotArea = s.PivotArea,
                                                              AreaIndex = s.AreaIndex,
                                                              CellFormat_FormatString = s.CellFormat_FormatString,
                                                              CellFormat_FormatType = s.CellFormat_FormatType,
                                                              Visible = s.Visible,
                                                              Width = s.Width
                                                          }).ToList();
            return list;
        }

        public SearchResultTemplateModel GetTemplateById(Guid id)
        {
            if(id != Guid.Empty)
            {
                return _context.SearchResultTemplateModel.Find(id);
            }
            return null;
        }
    }
}
