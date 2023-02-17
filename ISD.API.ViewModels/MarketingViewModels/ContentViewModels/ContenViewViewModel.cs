using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.API.ViewModels.MarketingViewModels.ContentViewModels
{
    public class ContenViewViewModel
    {
        public Guid Id { get; set; }
        //Loại nội dung: EMAIL | SMS
        public string CatalogCode { get; set; }
        //Email cá nhân | Email có sẵn
        public string EmailType { get; set; }
        public string SentFrom { get; set; }
        public int ContentCode { get; set; }
        public string Type { get; set; }
        public string ContentName { get; set; }
        public string FromEmailAccount { get; set; }
        public string Domain { get; set; }
        public Guid FromEmailAccountId { get; set; }
        public string SenderName { get; set; }
        public string CompanyCode { get; set; }
        public string SaleOrg { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public bool? Actived { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateTime { get; set; }
        public string LastEditBy { get; set; }
        public DateTime? LastEditTime { get; set; }
    }
}
