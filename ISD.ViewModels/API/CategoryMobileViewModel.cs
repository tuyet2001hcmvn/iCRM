using ISD.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.Permission
{
    public class CategoryMobileViewModel
    {
        public Guid CategoryId { get; set; }
        public Guid? ParentCategoryId{ get; set; }
        public string CategoryCode { get; set; }
        public string CategoryName { get; set; }
        public int? OrderIndex { get; set; }
        public string ImageUrlTemp { get; set; }
        public int? ProductTypeId { get; set; }
        public string ImageUrl
        {
            get
            {
                return string.Format("{0}/{1}", ConstDomain.DomainImageCategory,
                                                ImageUrlTemp != null ? ImageUrlTemp : ConstImageUrl.noImage);
            }
        }
    }
    public class CategoryMobileDetailViewModel
    {
        public CategoryMobileViewModel Parent { get; set; }
        public List<CategoryMobileViewModel> Children { get; set; }
    }
}
