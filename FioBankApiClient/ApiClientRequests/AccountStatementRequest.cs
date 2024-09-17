using CSharpFunctionalExtensions;
using FioBankApiClient.Models;
using FioBankApiClient.Serialization;

namespace FioBankApiClient.ApiClientRequests
{
    /// <summary>
    /// Contain URL for retrieving account statement information and method for reading response
    /// data from api and deserialize it to AccountStatement object.
    /// </summary>
    public class AccountStatementRequest : IFioApiRequest<AccountStatement>
    {
        private readonly JsonApiDataSerializer _jsonFioApiSerializer;

        /// <summary>
        /// Gets or sets the URL for the API request.
        /// </summary>
        public string Url
        {
            get;
            set;
        }

        public AccountStatementRequest(JsonApiDataSerializer jsonFioApiSerializer)
        {
            _jsonFioApiSerializer = jsonFioApiSerializer;
        }

        /// <summary>
        /// Processes the content of the HTTP response asynchronously and deserializes it into an
        /// <see cref="AccountStatement"/> object.
        /// </summary>
        /// <param name="responseContent">The HTTP response content.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
        /// <returns>
        /// An asynchronous task that represents the operation, containing the deserialized account
        /// statement or an error.
        /// </returns>
        public async Task<Result<AccountStatement>> ProcessContentAsync(HttpContent responseContent, CancellationToken cancellationToken)
        {
            var dataToDeserialize = await responseContent.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);

            return await _jsonFioApiSerializer.TryDeserializeAsync(dataToDeserialize, cancellationToken).ConfigureAwait(false);
        }
    }
}