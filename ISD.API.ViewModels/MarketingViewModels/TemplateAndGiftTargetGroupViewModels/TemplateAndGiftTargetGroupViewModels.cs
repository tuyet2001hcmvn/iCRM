using System;

namespace ISD.API.ViewModels
{
    public class TemplateAndGiftTargetGroupViewModels
    {
        public Guid Id { get; set; }
        public string TargetGroupName { get; set; }
        public int TargetGroupCode { get; set; }
        public bool Actived { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateTime { get; set; }
        public string LastEditBy { get; set; }
        public DateTime? LastEditTime { get; set; }
    }
}
