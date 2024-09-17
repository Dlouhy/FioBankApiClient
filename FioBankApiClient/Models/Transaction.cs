using System.Text.Json.Serialization;

namespace FioBankApiClient.Models
{
    /// <summary>
    /// Represents movement on the account for the given period.
    /// </summary>
    public class Transaction
    {
        [JsonPropertyName("column22")]
        public Column<long> IdPohybu { get; set; }

        [JsonPropertyName("column0")]
        public Column<string> Datum { get; set; }

        [JsonPropertyName("column1")]
        public Column<decimal> Objem { get; set; }

        [JsonPropertyName("column14")]
        public Column<string> Mena { get; set; }

        [JsonPropertyName("column2")]
        public Column<string> Protiucet { get; set; }

        [JsonPropertyName("column10")]
        public Column<string> NazevProtiuctu { get; set; }

        [JsonPropertyName("column3")]
        public Column<string> KodBankyProtiuctu { get; set; }

        [JsonPropertyName("column12")]
        public Column<string> NazevBankyProtiuctu { get; set; }

        [JsonPropertyName("column4")]
        public Column<string> KonstantniSymbol { get; set; }

        [JsonPropertyName("column5")]
        public Column<string> VariabilniSymbol { get; set; }

        [JsonPropertyName("column6")]
        public Column<string> SpecifickySymbol { get; set; }

        [JsonPropertyName("column7")]
        public Column<string> UzivatelskaIdentifikace { get; set; }

        [JsonPropertyName("column16")]
        public Column<string> ZpravaProPrijemce { get; set; }

        [JsonPropertyName("column8")]
        public Column<string> TypOperace { get; set; }

        [JsonPropertyName("column9")]
        public Column<string> Provedl { get; set; }

        [JsonPropertyName("column18")]
        public Column<string> Upresneni { get; set; }

        [JsonPropertyName("column25")]
        public Column<string> Komentar { get; set; }

        [JsonPropertyName("column26")]
        public Column<string> BIC { get; set; }

        [JsonPropertyName("column17")]
        public Column<long> IdPokynu { get; set; }

        /// <summary>
        /// Closer identification of payments according to the agreement between payment
        /// participants.
        /// </summary>
        [JsonPropertyName("column27")]
        public Column<string> ReferencePlatce { get; set; }
    }
}