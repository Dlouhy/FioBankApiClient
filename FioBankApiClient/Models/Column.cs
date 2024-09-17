using System.Text.Json.Serialization;

namespace FioBankApiClient.Models
{
    public class Column<T>
    {
        [JsonPropertyName("id")]
        public int ColumnId { get; set; }

        [JsonPropertyName("name")]
        public string ColumnName { get; set; }

        [JsonPropertyName("value")]
        public T ColumnValue { get; set; }
    }
}