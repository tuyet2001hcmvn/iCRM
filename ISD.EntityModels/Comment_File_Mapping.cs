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
    
    public partial class Comment_File_Mapping
    {
        public System.Guid TaskCommentId { get; set; }
        public System.Guid FileAttachmentId { get; set; }
        public string Note { get; set; }
    
        public virtual FileAttachmentModel FileAttachmentModel { get; set; }
        public virtual TaskCommentModel TaskCommentModel { get; set; }
    }
}
