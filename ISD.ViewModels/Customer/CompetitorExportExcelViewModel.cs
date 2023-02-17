using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
    public class CompetitorExportExcelViewModel
    {
        [Display(Name = "TÊN CÔNG TY")]
        public string ProfileName { get; set; }

        public bool DistributionIndustryContent1 { get; set; }
        public bool DistributionIndustryContent2 { get; set; }
        public bool DistributionIndustryContent3 { get; set; }
        public bool DistributionIndustryContent4 { get; set; }
        public bool DistributionIndustryContent5 { get; set; }
        public bool DistributionIndustryContent6 { get; set; }
        public bool DistributionIndustryContent7 { get; set; }
        public bool DistributionIndustryContent8 { get; set; }
        public bool DistributionIndustryContent9 { get; set; }
        public bool DistributionIndustryContent10 { get; set; }
        public bool DistributionIndustryContent11 { get; set; }
        public bool DistributionIndustryContent12 { get; set; }
        public bool DistributionIndustryContent13 { get; set; }
        public bool DistributionIndustryContent14 { get; set; }
        public bool DistributionIndustryContent15 { get; set; }
        public bool DistributionIndustryContent16 { get; set; }
        public bool DistributionIndustryContent17 { get; set; }
        public bool DistributionIndustryContent18 { get; set; }
        public bool DistributionIndustryContent19 { get; set; }
        public bool DistributionIndustryContent20 { get; set; }

        [Display(Name = "Miền Bắc")]
        public bool Area1 { get; set; }

        [Display(Name = "Miền Trung")]
        public bool Area2 { get; set; }

        [Display(Name = "Miền Nam")]
        public bool Area3 { get; set; }

        [Display(Name = "Tỉnh/Thành phố")]
        public string ProvinceName { get; set; }

        [Display(Name = "Độ phủ thị trường (%)")]
        public decimal? Number2 { get; set; }

        [Display(Name = "Vốn pháp định (Tỷ)")]
        public decimal? Number1 { get; set; }

        [Display(Name = "Website")]
        public string Website { get; set; }

     

        [Display(Name = "ĐỊA CHỈ")]
        public string Address { get; set; }

        [Display(Name = "CÓ NHÀ MÁY (Có/Không)")]
        public string IsHasFactory { get; set; }

        [Display(Name = "ĐỊA CHỈ NHÀ MÁY")]
        public string FactoryAddress { get; set; }

        [Display(Name = "ĐÁNH GIÁ TẦM CỠ")]
        public string CaliberAssessment { get; set; }

        [Display(Name = "GHI CHÚ KHÁC")]
        public string Note { get; set; }

    }
}
