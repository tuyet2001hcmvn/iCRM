namespace ISD.API.Core
{
    public static class Constant
    {       
        #region Marketing
        public const string CampaignStatus_Planned = "Campaign_Planned";
        public const string CampaignStatus_Actived = "Campaign_Actived";
        public const string CampaignStatus_Finnished = "Campaign_Finnished";
        public const string CampaignCatalogTypeCode = "CampaignStatus";
        #endregion

        public const string QuestionCatalogTypeCode = "QuestionCategory";
        public const string QuestionDepartmentTypeCode = "QuestionDepartment";

        public const string ReportMenuName = "Báo cáo";
        public const string FavoriteReportPage = "Báo cáo thường dùng";

        public const string RequestEccConfig_ToEmail = "RequestEccConfig_ToEmail";
        public const string RequestEccConfig_Subject = "RequestEccConfig_Subject";


        //Marketing 
        //Loại nội dung (EMAIL|SMS)
        public const string Marketing_Content = "Marketing_Content";
        public const string Email = "Email";
        public const string SMS = "SMS";
        //Loại email (Có sẵn|Cá nhân)
        public const string Marketing_Content_Email_Type = "Marketing_Content_Email_Type";
        public const string DefaultEmail = "DefaultEmail";
        public const string PersonalEmail = "PersonalEmail";

    }
}
