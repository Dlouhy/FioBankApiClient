using FioBankApiClient.Properties;
using FioBankApiClient.Serialization;
using Microsoft.Extensions.Options;
using NetCoreUtils;
using System.Globalization;

namespace FioBankApiClient.ApiClientRequests
{
    /// <summary>
    /// The RequestUrlFactory class provides methods to construct URLs for interacting with the FIO API.
    /// </summary>
    public class RequestUrlFactory
    {
        private const string FioApiDateFormat = "yyyy-MM-dd";
        private readonly string _baseUrl = "https://fioapi.fio.cz/v1/rest";
        private readonly IOptions<ApiClientOption> _apiClientOption;

        public RequestUrlFactory(IOptions<ApiClientOption> option)
        {
            _apiClientOption = option;
        }

        /// <summary>
        /// Builds a URL to retrieve transactions for a specified date range.
        /// </summary>
        /// <param name="dateTimeRange">The date range for which to retrieve transactions.</param>
        /// <param name="outputFormat">The desired output format.</param>
        /// <returns>A URL string that can be used to retrieve transactions.</returns>
        public string TransactionsInDateRangeUrl(DateTimeRange dateTimeRange, ApiDataFormat outputFormat)
        {
            var fioFormatStartDay = dateTimeRange.StartDateTimeFormatted(FioApiDateFormat);
            var fioFormatEndDay = dateTimeRange.EndDateTimeFormatted(FioApiDateFormat);

            string dateTimeRangeUrl = $"{_baseUrl}/periods/{_apiClientOption.Value.AuthenticationToken.Token}/{fioFormatStartDay}/{fioFormatEndDay}/transactions.{outputFormat.ToLowerFioFormat()}";
            return dateTimeRangeUrl;
        }

        /// <summary>
        /// Builds a URL to retrieve all transactions since a certain statement ID and year.
        /// </summary>
        /// <param name="year">The year of the statement.</param>
        /// <param name="idOfStatement">The ID of the statement.</param>
        /// <param name="outputFormat">The desired output format.</param>
        /// <returns>A URL string that can be used to retrieve transactions.</returns>
        public string TransactionsByStatementIdAndYearUrl(int year, int idOfStatement,
                                                     ApiDataFormat outputFormat)
        {
            if (idOfStatement < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(idOfStatement), Resources.StatementIDMustBePositive);
            }

            if (year < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(year), Resources.YearMustBePositive);
            }

            string byIdUrl = $"{_baseUrl}/by-id/{_apiClientOption.Value.AuthenticationToken.Token}/{year}/{idOfStatement}/transactions.{outputFormat.ToLowerFioFormat()}";
            return byIdUrl;
        }

        /// <summary>
        /// Builds a URL to retrieve transactions since last download.
        /// </summary>
        /// <param name="outputFormat">The desired output format.</param>
        /// <returns>A URL string that can be used to retrieve transactions.</returns>
        public string LastTransactionsUrl(ApiDataFormat outputFormat)
        {
            string lastUrl = $"{_baseUrl}/last/{_apiClientOption.Value.AuthenticationToken.Token}/transactions.{outputFormat.ToLowerFioFormat()}";
            return lastUrl;
        }

        /// <summary>
        /// Builds a URL to get number and year of the last official statement created.
        /// </summary>
        /// <param name="year">
        /// The optional year for which to retrieve the last statement. If null, retrieves the most
        /// recent statement.
        /// </param>
        /// <returns>
        /// A URL string that can be used to retrieve the last statement nember and year.
        /// </returns>
        public string LastStatementUrl(int? year)
        {
            if (year != null)
            {
                if (year.Value < 1)
                {
                    throw new ArgumentOutOfRangeException(nameof(year), Resources.YearMustBePositive);
                }

                return $"{_baseUrl}/lastStatement/{_apiClientOption.Value.AuthenticationToken.Token}/statement?year={year}";
            }
            else
            {
                return $"{_baseUrl}/lastStatement/{_apiClientOption.Value.AuthenticationToken.Token}/statement";
            }
        }

        /// <summary>
        /// Builds a URL that set cursor to the date of the last unsuccessful download day.
        /// </summary>
        /// <param name="date">The date of the last unsuccessfully downloaded transaction.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if the date is earlier than DateTimeOffset.MinValue.
        /// </exception>
        /// <returns>A URL string that can be used to set cursor.</returns>
        public string SetLastDateUrl(DateTimeOffset date)
        {
            if (date.UtcDateTime < DateTimeOffset.MinValue.UtcDateTime)
            {
                throw new ArgumentOutOfRangeException(nameof(date), Resources.DateTimeCannotBeEarlier);
            }

            string setLastDateUrl = $"{_baseUrl}/set-last-date/{_apiClientOption.Value.AuthenticationToken.Token}/{date.ToString(FioApiDateFormat, CultureInfo.InvariantCulture)}/";
            return setLastDateUrl;
        }

        /// <summary>
        /// Builds a URL that set the cursor to the ID of the last successfully downloaded movement.
        /// </summary>
        /// <param name="movementId">The ID of the last successfully downloaded movement.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if the movement ID is not positive.
        /// </exception>
        /// <returns>A URL string that can be used to set cursor.</returns>
        public string SetLastIdUrl(long movementId)
        {
            if (movementId < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(movementId), Resources.MovementIdMustBePositive);
            }

            string setLastIdUrl = $"{_baseUrl}/set-last-id/{_apiClientOption.Value.AuthenticationToken.Token}/{movementId}/";
            return setLastIdUrl;
        }
    }
}