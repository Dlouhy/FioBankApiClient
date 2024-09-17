using CSharpFunctionalExtensions;

namespace FioBankApiClient.ApiClientRequests
{
    /// <summary>
    /// Represents a FIO API request.
    /// </summary>
    /// <typeparam name="T">The type of the response data.</typeparam>
    public interface IFioApiRequest<T>
    {
        /// <summary>
        /// Gets or sets the URL for the API request.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Processes the HTTP content asynchronously and returns the result.
        /// </summary>
        /// <param name="responseContent">The HTTP content to process.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
        /// <returns>
        /// An asynchronous task that represents the operation, containing string or an error.
        /// </returns>
        Task<Result<T>> ProcessContentAsync(HttpContent responseContent, CancellationToken cancellationToken);
    }
}