using ISD.EntityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class ProfileOverviewModel : ProfileModel
    {
        public string ProvinceName { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_ProfileShortName")]
        public string ProfileShortName { get; set; }
        public string DistrictName { get; set; }
        public string WardName { get; set; }
        public string isForeignName { get; set; }
        public string CustomerTypeName { get; set; }
        public string SaleOfficeName { get; set; }
        public string CustomerSourceName { get; set; }
        public string CreateAtSaleOrgName { get; set; }
        public string ActivedName { get; set; }
        public string CustomerCareerName { get; set; }
        public string AddressTypeName { get; set; }
        public List<ProfileGroupCreateViewModel> profileGroupList { get; set; }
        //Dự án
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TabInvestor")]
        public string Investor { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TabConsultingDesign")]
        public string Designer { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TabGeneralContractor")]
        public string Contractor { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TabConsultingUnit")]
        public string ConsultingUnit { get; set; }
        public string HandoverFurnitureList { get; set; }
        public string OpportunityType { get; set; }
        public string OpportunityLevel { get; set; }
        public string OpportunityPercentage { get; set; }
        public string ProjectStatus { get; set; }
        public string ProjectComplete { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? ProjectStartDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? ProjectEndDate { get; set; }
        public decimal? ContractWonValue { get; set; }
        public string ProjectStart
        {
            get
            {
                if (ProjectStartDate.HasValue)
                {
                    return string.Format("Tháng {0}/Năm {1}", ProjectStartDate.Value.Month, ProjectStartDate.Value.Year);
                }
                return string.Empty;
            }
        }

        public string ProjectEnd
        {
            get
            {
                if (ProjectEndDate.HasValue)
                {
                    return string.Format("Tháng {0}/Năm {1}", ProjectEndDate.Value.Month, ProjectEndDate.Value.Year);
                }
                return string.Empty;
            }
        }

    }
}
