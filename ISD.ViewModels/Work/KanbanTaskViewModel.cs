using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ISD.ViewModels
{
    public class KanbanTaskViewModel
    {
        public string id { get; set; }
        public string state { get; set; }

        [AllowHtml]
        public string label { get; set; }

        [AllowHtml]
        public string tags { get; set; }
        public string hex { get; set; }
        public string code { get; set; }
        public List<string> resourceId { get; set; }

        //Calendar
        public Guid? TaskId { get; set; }
        public string Summary { get; set; }
        public int? TaskCode { get; set; }
        public string VisitAddress { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EstimateEndDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string ProcessCode { get; set; }
        public string TaskStatusBackgroundColor { get; set; }
        public string TaskStatusColor { get; set; }
        public bool? Actived { get; set; }
    }

    public class KanbanColumnViewModel
    {
        [AllowHtml]
        public string text { get; set; }
        public string dataField { get; set; }
    }
}