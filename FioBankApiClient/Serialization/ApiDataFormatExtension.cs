namespace FioBankApiClient.Serialization
{
    public static class ApiDataFormatExtension
    {
        public static string ToLowerFioFormat(this ApiDataFormat fioDataFormatEnum)
        {
            return fioDataFormatEnum.ToString().ToLowerInvariant();
        }
    }
}