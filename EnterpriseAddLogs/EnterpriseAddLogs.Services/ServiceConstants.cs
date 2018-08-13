namespace EnterpriseAddLogs.Services
{
    public static class ServiceConstants
    {
        public sealed class Defaults
        {
            public const int ServiceTimeoutInSecs = 30;
        }

        public sealed class Urls
        {
            //User
            public const string GetUserEntity = "https://enterprizepoc.azure-api.net/api/UserApi/GetUserEntity?id={0}";
            public const string GetAllUserEntities = "https://enterprizepoc.azure-api.net/api/UserApi/GetUserEntities";
            public const string DeleteUserEntity = "https://enterprizepoc.azure-api.net/api/UserApi/DeleteUserEntity?id={0}";
            public const string SAveUserEntity = "https://enterprizepoc.azure-api.net/api/UserApi/SaveUserEntity";

            //Engine
            public const string GetAllEngineEntities = "https://enterprizepoc.azure-api.net/api/EngineApi/GetEngineEntities";

            //Log
            public const string GetAllLogEntities = "https://enterprizepoc.azure-api.net/api/LogApi/GetLogEntities";
            public const string GetLogEntity = "https://enterprizepoc.azure-api.net/api/LogApi/GetLogEntity?id={0}";
            public const string SaveLogEntity = "https://enterprizepoc.azure-api.net/api/LogApi/SaveLogEntity";

            //LogType
            public const string GetAllLogTypeEntities = "https://enterprizepoc.azure-api.net/api/LogTypeApi/GetLogTypeEntities";

            //Product Group
            public const string GetAllProductGroupEntities = "https://enterprizepoc.azure-api.net/api/ProductGroupApi/GetProductGroups";

            //Unit
            public const string GetAllUnitEntities = "https://enterprizepoc.azure-api.net/api/UnitApi/GetUnitEntities";
        }

        public sealed class SubscriptionKeys
        {
            //unlimited subscription
            public const string PrimaryKey = "1a6ab1ee03374a2aac3841fb203d09b4";
            public const string SecondaryKey = "223c256f3cb74912a74d049a9708e8c0";
        }
    }
}
