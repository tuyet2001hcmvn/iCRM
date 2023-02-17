namespace ISD.Constant
{
    public static class ConstantApi
    {
        public const string BaseAddress = "http://192.168.100.237:3380/api/";

        #region Marketing
        public const string GetTargetGroupByIdToEditUrl = "http://192.168.100.237:3380/api/Marketing/TargetGroups/";
        public const string TargetGroupSearchUrl = "http://192.168.100.237:3380/api/Marketing/TargetGroups?TargetGroupCode={0}&TargetGroupName={1}&Actived={2}";
        public const string ImportMemberToTargetGroupUrl = "https://localhost:44367/api/Marketing/TargetGroups/Members/";
        public const string GetMemberOfTargetGroupUrl = "https://localhost:44367/api/Marketing/TargetGroups/Members/{0}";
        #endregion
    }
}
