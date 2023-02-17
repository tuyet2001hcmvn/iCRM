using ISD.Constant;
using ISD.EntityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ISD.ViewModels
{
    public class SurveyViewModel
    {
        public Guid? TaskSurveyId { get; set; }
        public Guid? SurveyId { get; set; }
        public string Question { get; set; }
        public string Type { get; set; }
        public int? OrderIndex { get; set; }
        public Guid? CreateBy { get; set; }
        public string CreateByCode { get; set; }
        public DateTime? CreateTime { get; set; }

        public List<SurveyDetailViewModel> SurveyDetail { get; set; }
        public List<SurveyDetailViewModel> SurveyDetailSelected { get; set; }
    }
    public class SurveyDetailViewModel
    {
        public Guid? SurveyId { get; set; }
        public Guid? SurveyDetailId { get; set; }
        public string AnswerText { get; set; }
        public string AnswerValue { get; set; }
        public DateTime? AnswerDatetime { get; set; }
        public int? OrderIndex { get; set; }

    }
}
