using System.Text.Json.Serialization;

namespace FioBankApiClient.Models
{
    /// <summary>
    /// Provides information about the account, the opening and closing balances on that account and
    /// the period they are for given transactions displayed, identification of the statement, the
    /// last download of movements.
    /// </summary>
    public class Info
    {
        [JsonPropertyName("accountId")]
        public string AccountId { get; set; }

        [JsonPropertyName("bankId")]
        public string BankId { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("iban")]
        public string IBAN { get; set; }

        [JsonPropertyName("bic")]
        public string BIC { get; set; }

        [JsonPropertyName("openingBalance")]
        public double OpeningBalance { get; set; }

        [JsonPropertyName("closingBalance")]
        public double ClosingBalance { get; set; }

        [JsonPropertyName("dateStart")]
        public string DateStart { get; set; }

        [JsonPropertyName("dateEnd")]
        public string DateEnd { get; set; }

        /// <summary>
        /// Year of the selected statement.
        /// </summary>
        [JsonPropertyName("yearList")]
        public object YearList { get; set; }

        /// <summary>
        /// Number of the selected statement.
        /// </summary>
        [JsonPropertyName("idList")]
        public object IdList { get; set; }

        /// <summary>
        /// The number of the first move in the given selection.
        /// </summary>
        [JsonPropertyName("idFrom")]
        public long? IdFrom { get; set; }

        /// <summary>
        /// Last move number in the given selection.
        /// </summary>
        [JsonPropertyName("idTo")]
        public long? IdTo { get; set; }

        /// <summary>
        /// The number of the last successfully downloaded movement.
        /// </summary>
        [JsonPropertyName("idLastDownload")]
        public object IdLastDownload { get; set; }
    }
}