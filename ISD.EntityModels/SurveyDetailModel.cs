//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ISD.EntityModels
{
    using System;
    using System.Collections.Generic;
    
    public partial class SurveyDetailModel
    {
        public System.Guid SurveyDetailId { get; set; }
        public Nullable<System.Guid> SurveyId { get; set; }
        public string AnswerText { get; set; }
        public string AnswerValue { get; set; }
        public Nullable<System.DateTime> AnswerDatetime { get; set; }
        public Nullable<int> OrderIndex { get; set; }
    }
}