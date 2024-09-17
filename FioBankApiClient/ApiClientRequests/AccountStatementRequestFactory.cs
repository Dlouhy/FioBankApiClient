using FioBankApiClient.Serialization;
using NetCoreUtils;

namespace FioBankApiClient.ApiClientRequests
{
    /// <summary>
    /// Helps create AccountStatementRequest objects containing URLs for retrieving account
    /// statement information and method for process response data from Fio Bank API.
    /// </summary>
    public class AccountStatementRequestFactory
    {
        private readonly RequestUrlFactory _requestUrlFactory;
        private readonly Func<AccountStatementRequest> _factory;

        public AccountStatementRequestFactory(RequestUrlFactory requestUrlFactory, Func<AccountStatementRequest> factory)
        {
            _requestUrlFactory = requestUrlFactory;
            _factory = factory;
        }

        /// <summary>
        /// Creates a AccountStatementRequest for fetching the last transactions.
        /// </summary>
        /// <returns>A AccountStatementRequest instance for fetching the last transactions.</returns>
        public AccountStatementRequest LastTransactionsRequest()
        {
            string url = _requestUrlFactory.LastTransactionsUrl(ApiDataFormat.Json);

            return CreateAccountStatementRequest(url);
        }

        /// <summary>
        /// Creates a AccountStatementRequest to retrieve transactions for a specified date range.
        /// </summary>
        /// <param name="dateTimeRange">The date range for the transaction query.</param>
        /// <returns>A AccountStatementRequest instance to fetch transactions.</returns>
        public AccountStatementRequest TransactionsInDateRangeRequest(DateTimeRange dateTimeRange)
        {
            string url = _requestUrlFactory.TransactionsInDateRangeUrl(dateTimeRange, ApiDataFormat.Json);

            return CreateAccountStatementRequest(url);
        }

        /// <summary>
        /// Creates a AccountStatementRequest to retrieve all transactions since a certain statement
        /// ID and year.
        /// </summary>
        /// <param name="year">The year of the statement.</param>
        /// <param name="idOfStatement">The ID of the statement.</param>
        /// <returns>A AccountStatementRequest instance to fetch transactions.</returns>
        public AccountStatementRequest TransactionsByStatementIdAndYearRequest(int year, int idOfStatement)
        {
            string url = _requestUrlFactory.TransactionsByStatementIdAndYearUrl(year, idOfStatement, ApiDataFormat.Json);

            return CreateAccountStatementRequest(url);
        }

        private AccountStatementRequest CreateAccountStatementRequest(string url)
        {
            var output = _factory();
            output.Url = url;
            return output;
        }
    }
}