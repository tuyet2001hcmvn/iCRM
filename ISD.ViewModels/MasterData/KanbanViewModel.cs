using ISD.EntityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class KanbanViewModel : KanbanModel
    {
        public string CreateByName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Task_WorkFlowId")]
        public Guid? WorkFlowId { get; set; }
    }

    public class KanbanDetailViewModel
    {
        public Guid? KanbanDetailId { get; set; }
        public System.Guid? KanbanId { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Kanban_ColumnName")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public string ColumnName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "OrderIndex")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Validation_OrderIndex")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "Required")]
        public Nullable<int> OrderIndex { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Note")]
        public string Note { get; set; }
    }

    public class KanbanSearchViewModel
    {
        public string SearchKanbanCode { get; set; }
        public string SearchKanbanName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Actived")]
        public Nullable<bool> Actived { get; set; }
    }
}
