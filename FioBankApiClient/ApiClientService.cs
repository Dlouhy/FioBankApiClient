using CSharpFunctionalExtensions;
using FioBankApiClient.ApiClientRequests;
using FioBankApiClient.Models;
using FioBankApiClient.Serialization;
using NetCoreUtils;

namespace FioBankApiClient
{
    /// <summary>
    /// Provides methods for interacting with the Fio Bank Api Client to retrieve account statements
    /// and transactions.
    /// </summary>
    public class ApiClientService
    {
        private readonly ApiClient _apiClient;
        private readonly AccountStatementRequestFactory _accountStatementRequestFactory;
        private readonly StringRequestFactory _stringRequestFactory;

        public ApiClientService(ApiClient apiClient, AccountStatementRequestFactory accountStatementRequestFactory, StringRequestFactory stringRequestFactory)
        {
            _apiClient = apiClient;
            _accountStatementRequestFactory = accountStatementRequestFactory;
            _stringRequestFactory = stringRequestFactory;
        }

        /// <summary>
        /// Gets account movements since the last download.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns>
        /// The task result contains a Result object with the downloaded AccountStatemet object or
        /// an error message on failure.
        /// </returns>
        public async Task<Result<AccountStatement>> GetTransactionsSinceLastDownloadAsync(CancellationToken cancellationToken = default)
        {
            var fioApiRequest = _accountStatementRequestFactory.LastTransactionsRequest();
            return await _apiClient.SendToFioApiAsync(fioApiRequest, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets account movements since the last download in specific format.
        /// </summary>
        /// <param name="fioDataFormat">The desired format for the downloaded transaction data.</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns>
        /// The task result contains a Result object with the downloaded data as a string or an
        /// error message on failure.
        /// </returns>
        public async Task<Result<string>> GetTransactionsSinceLastDownloadAsync(ApiDataFormat fioDataFormat, CancellationToken cancellationToken = default)
        {
            var fioApiRequest = _stringRequestFactory.LastTransactionsRequest(fioDataFormat);
            return await _apiClient.SendToFioApiAsync(fioApiRequest, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets transactions for a specified date range.
        /// </summary>
        /// <param name="dateTimeRange">The date range for which to retrieve transactions.</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns>
        /// The task result contains a Result object with the downloaded AccountStatemet object or
        /// an error message on failure.
        /// </returns>
        public async Task<Result<AccountStatement>> GetTransactionsInDateRangeAsync(DateTimeRange dateTimeRange, CancellationToken cancellationToken = default)
        {
            var fioApiRequest = _accountStatementRequestFactory.TransactionsInDateRangeRequest(dateTimeRange);

            return await _apiClient.SendToFioApiAsync(fioApiRequest, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets transactions for a specified date range in specific format.
        /// </summary>
        /// <param name="dateTimeRange">The date range for which to retrieve transactions.</param>
        /// <param name="fioDataFormat">The desired format for the downloaded transaction data.</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns>
        /// The task result contains a Result object with the downloaded transactions as a string on
        /// success, or an error message on failure.
        /// </returns>
        public async Task<Result<string>> GetTransactionsInDateRangeAsync(DateTimeRange dateTimeRange, ApiDataFormat fioDataFormat, CancellationToken cancellationToken = default)
        {
            var fioApiRequest = _stringRequestFactory.TransactionsInDateRangeRequest(dateTimeRange, fioDataFormat);

            return await _apiClient.SendToFioApiAsync(fioApiRequest, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets all transactions since a certain statement ID and year.
        /// </summary>
        /// <param name="idOfStatement">The ID of the statement to retrieve transactions for.</param>
        /// <param name="year">The year of the statement.</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns>
        /// The task result contains a Result object with the downloaded AccountStatemet object or
        /// an error message on failure.
        /// </returns>
        public async Task<Result<AccountStatement>> GetTransactionsByStatementIdAndYearAsync(int idOfStatement, int year, CancellationToken cancellationToken = default)
        {
            var fioApiRequest = _accountStatementRequestFactory.TransactionsByStatementIdAndYearRequest(year, idOfStatement);
            return await _apiClient.SendToFioApiAsync(fioApiRequest, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets all transactions since a certain statement ID and year in specific format.
        /// </summary>
        /// <param name="idOfStatement">The ID of the statement to retrieve transactions for.</param>
        /// <param name="year">The year of the statement.</param>
        /// <param name="fioDataFormat">The desired format for the downloaded transaction data.</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns>
        /// The task result contains a Result object with the downloaded data as a string or an
        /// error message on failure.
        /// </returns>
        public async Task<Result<string>> GetTransactionsByStatementIdAndYearAsync(int idOfStatement, int year, ApiDataFormat fioDataFormat, CancellationToken cancellationToken = default)
        {
            var fioApiRequest = _stringRequestFactory.TransactionsByStatementIdAndYearRequest(year, idOfStatement, fioDataFormat);
            return await _apiClient.SendToFioApiAsync(fioApiRequest, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets Id and Year of last created official account statement, optionally filtered by year.
        /// </summary>
        /// <param name="year">
        /// The optional year for which to retrieve the last statement. If null, retrieves the most
        /// recent statement.
        /// </param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns>
        /// The task result contains a Result object with string that have ID and year of last statement.
        /// </returns>
        public async Task<Result<string>> GetLastStatementAsync(int? year, CancellationToken cancellationToken = default)
        {
            var fioApiRequest = _stringRequestFactory.LastStatementRequest(year);
            return await _apiClient.SendToFioApiAsync(fioApiRequest, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Set the cursor to the date of the last unsuccessful download transaction.
        /// </summary>
        /// <param name="date">The date of the last unsuccessfully downloaded transaction.</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task SetCursorToLastDateAsync(DateTimeOffset date, CancellationToken cancellationToken = default)
        {
            var fioApiRequest = _stringRequestFactory.SetLastDateRequest(date);
            await _apiClient.SendToFioApiAsync(fioApiRequest, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Set the cursor to the ID of the last successfully downloaded movement.
        /// </summary>
        /// <param name="movementId">The ID of the last successfully downloaded movement.</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task SetCursorToLastIdAsync(long movementId, CancellationToken cancellationToken = default)
        {
            var fioApiRequest = _stringRequestFactory.SetLastIdRequest(movementId);
            await _apiClient.SendToFioApiAsync(fioApiRequest, cancellationToken).ConfigureAwait(false);
        }
    }
}