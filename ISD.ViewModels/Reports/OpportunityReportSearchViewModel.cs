using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class OpportunityReportSearchViewModel
    {
        //Dự án
        public string ProfileShortName { get; set; }
        public string ProfileName { get; set; }
        public string HandoverFurniture { get; set; }
        public string ProfileForeignCode { get; set; }
        public string SaleOfficeName { get; set; }
        public string ProvinceName { get; set; }
        public string ProjectGabarit { get; set; }
        public List<string> OpportunityStatusType { get; set; }
        public Nullable<decimal> ProjectValue { get; set; }
        public Nullable<decimal> ProjectWonValue { get; set; }
        public string SalesEmployeeName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TabNumber4")]
        public Nullable<decimal> Number4 { get; set; }

        public string Dropdownlist7 { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TabInvestor")]
        public List<Guid> Investor { get; set; }
        public string InvestorName { get; set; }
        public bool? IsTopInvestor { get; set; }
        
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TabConsultingDesign")]
        public List<Guid> Designer { get; set; }
        public string DesignerName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TabGeneralContractor")]
        public List<Guid> Contractor { get; set; }
        public string ContractorName { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "TabCompetitor")]
        public List<Guid> Competitor { get; set; }
        public string CompetitorName { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Profile_SaleOfficeCode")]
        public List<string> SaleOfficeCode { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "OpportunityPercentage")]
        public List<Guid> OpportunityPercentage { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "SaleSpecCode")]
        public string SaleSpecCode { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProjectLocation")]
        public string ProjectLocation { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "OpportunityName")]
        public string OpportunityName { get; set; }
        public List<Guid> Opportunity { get; set; }
        public bool IsView { get; set; }
        public string SearchProjectMode { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ProvinceId")]
        public Guid? ProvinceId { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "DistrictId")]
        public List<Guid> DistrictId { get; set; }
        public string ReportType { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FromYear")]
        public string FromCompleteYear { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ToYear")]
        public string ToCompleteYear { get; set; }

        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "FromValue")]
        public decimal? FromValue { get; set; }
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "ToValue")]
        public decimal? ToValue { get; set; }

        public List<string>  PersonInChargeSales { get; set; }
        public List<string> PersonInChargeSalesAdmin { get; set; }
        public List<string> PersonInChargeSpec { get; set; }

        //Phòng ban
        [Display(ResourceType = typeof(Resources.LanguageResource), Name = "Master_Department")]
        public List<string> RolesCode { get; set; }
        public string OrderBy { get; set; }
        public string TypeSort { get; set; }
    }
}
