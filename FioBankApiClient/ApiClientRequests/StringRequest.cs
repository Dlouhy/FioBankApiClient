using CSharpFunctionalExtensions;

namespace FioBankApiClient.ApiClientRequests
{
    /// <summary>
    /// Contain URL for the API request and method for reading response data from api as string.
    /// </summary>
    public class StringRequest : IFioApiRequest<string>
    {
        /// <summary>
        /// Gets or sets the URL for the API request.
        /// </summary>
        public string Url
        {
            get;
            set;
        }

        /// <summary>
        /// Read the content of the HTTP response as string.
        /// </summary>
        /// <param name="responseContent">The HTTP response content.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
        /// <returns>
        /// An asynchronous task that represents the operation, containing string or an error.
        /// </returns>
        public async Task<Result<string>> ProcessContentAsync(HttpContent responseContent, CancellationToken cancellationToken)
        {
            var data = await responseContent.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            return Result.Success(data);
        }
    }
}