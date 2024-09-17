using FioBankApiClient.Serialization;
using NetCoreUtils;

namespace FioBankApiClient.ApiClientRequests
{
    /// <summary>
    /// Helps create StringRequest objects containing URLs for retrieving account statement
    /// information and method for reading response data from api as string.
    /// </summary>
    public class StringRequestFactory
    {
        private readonly RequestUrlFactory _requestUrlFactory;
        private readonly Func<StringRequest> _factory;

        public StringRequestFactory(RequestUrlFactory requestUrlFactory, Func<StringRequest> factory)
        {
            _requestUrlFactory = requestUrlFactory;
            _factory = factory;
        }

        /// <summary>
        /// Creates a StringRequest for fetching the last transactions.
        /// </summary>
        /// <param name="fioDataFormat">The format of the FIO data.</param>
        /// <returns>A StringRequest instance for fetching the last transactions.</returns>
        public StringRequest LastTransactionsRequest(ApiDataFormat fioDataFormat)
        {
            string url = _requestUrlFactory.LastTransactionsUrl(fioDataFormat);

            return CreateStringRequest(url);
        }

        /// <summary>
        /// Creates a StringRequest to retrieve transactions for a specified date range..
        /// </summary>
        /// <param name="dateTimeRange">The date range for the transaction query.</param>
        /// <param name="fioDataFormat">The format of the FIO data.</param>
        /// <returns>A StringRequest instance to fetch transactions.</returns>
        public StringRequest TransactionsInDateRangeRequest(DateTimeRange dateTimeRange, ApiDataFormat fioDataFormat)
        {
            string url = _requestUrlFactory.TransactionsInDateRangeUrl(dateTimeRange, fioDataFormat);

            return CreateStringRequest(url);
        }

        /// <summary>
        /// Creates a StringRequest to retrieve all transactions since a certain statement ID and year.
        /// </summary>
        /// <param name="year">The year of the statement.</param>
        /// <param name="idOfStatement">The ID of the statement.</param>
        /// <param name="fioDataFormat">The format of the FIO data.</param>
        /// <returns>A StringRequest instance to fetch transactions.</returns>
        public StringRequest TransactionsByStatementIdAndYearRequest(int year, int idOfStatement, ApiDataFormat fioDataFormat)
        {
            string url = _requestUrlFactory.TransactionsByStatementIdAndYearUrl(year, idOfStatement, fioDataFormat);

            return CreateStringRequest(url);
        }

        /// <summary>
        /// Creates a StringRequest to get number and year of the last official statement created,
        /// optionally filtered by year.
        /// </summary>
        /// <param name="year">
        /// The optional year for which to retrieve the last statement. If null, retrieves the most
        /// recent statement.
        /// </param>
        /// <returns>A StringRequest instance to fetch the last statement.</returns>
        public StringRequest LastStatementRequest(int? year)
        {
            string url = _requestUrlFactory.LastStatementUrl(year);

            return CreateStringRequest(url);
        }

        /// <summary>
        /// Creates a StringRequest that set the date of the last unsuccessful download day.
        /// </summary>
        /// <param name="date">The date to set as the new last unsuccessful download day.</param>
        /// <returns>A StringRequest instance to send.</returns>
        public StringRequest SetLastDateRequest(DateTimeOffset date)
        {
            string url = _requestUrlFactory.SetLastDateUrl(date);

            return CreateStringRequest(url);
        }

        /// <summary>
        /// Creates a StringRequest to reset the last ID by movement ID.
        /// </summary>
        /// <param name="movementId">The ID of the last successfully downloaded movement.</param>
        /// <returns>A StringRequest instance to reset the cursor.</returns>
        public StringRequest SetLastIdRequest(long movementId)
        {
            string url = _requestUrlFactory.SetLastIdUrl(movementId);

            return CreateStringRequest(url);
        }

        private StringRequest CreateStringRequest(string url)
        {
            var output = _factory();
            output.Url = url;
            return output;
        }
    }
}