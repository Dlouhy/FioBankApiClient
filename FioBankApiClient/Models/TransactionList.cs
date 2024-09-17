using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace FioBankApiClient.Models
{
    /// <summary>
    /// Movements on the account for the given period.
    /// </summary>
    public class TransactionList
    {
        [JsonPropertyName("transaction")]
        public Collection<Transaction> Transactions { get; set; }
    }
}