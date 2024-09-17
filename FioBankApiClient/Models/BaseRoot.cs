using System.Text.Json.Serialization;

namespace FioBankApiClient.Models
{
    public class BaseRoot
    {
        [JsonPropertyName("accountStatement")]
        public AccountStatement AccountStatement { get; set; }
    }
}