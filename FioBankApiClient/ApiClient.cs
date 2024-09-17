using CSharpFunctionalExtensions;
using FioBankApiClient.ApiClientRequests;
using FioBankApiClient.Logging;
using FioBankApiClient.Properties;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using SmartFormat;
using System.Diagnostics;
using System.Net;

namespace FioBankApiClient
{
    /// <summary>
    /// The `ApiClient` class is used to send asynchronous requests to the Fio Bank API and process
    /// the response data.
    /// </summary>
    public class ApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiClient> _logger;

        public ApiClient(HttpClient httpClient, ILogger<ApiClient> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _httpClient.DefaultRequestHeaders.Clear();

            if (logger == null)
            {
                Console.WriteLine(Resources.NullLoggerConsole);
                Trace.TraceWarning(Resources.NullLoggerTrace);
                Debug.WriteLine(Resources.NullLoggerDebug);
            }

            _logger = logger ?? NullLogger<ApiClient>.Instance;
        }

        /// <summary>
        /// Sends a request to the Fio Bank API and processes the response.
        /// </summary>
        /// <typeparam name="T">The type of the data expected in the response.</typeparam>
        /// <param name="fioRequest">
        /// The IFioApiRequest object containing the URL and the response content processor.
        /// </param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns>
        /// A Task of type Result containing the processed response data or an error message.
        /// </returns>
        public async Task<Result<T>> SendToFioApiAsync<T>(IFioApiRequest<T> fioRequest, CancellationToken cancellationToken)
        {
            if (fioRequest == null)
                return Result.Failure<T>(Resources.FioRequestNull);

            using var request = new HttpRequestMessage(HttpMethod.Get, fioRequest.Url);

            try
            {
                using (var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false))
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return await fioRequest.ProcessContentAsync(response.Content, cancellationToken).ConfigureAwait(false);
                    }
                    else
                    {
                        return await ProcessErrorStatusCodeResponseAsync(fioRequest, response, cancellationToken).ConfigureAwait(false);
                    }
                }
            }
            catch (HttpRequestException exception)
            {
                return LogExceptionResponse<T>(exception, fioRequest.Url);
            }
            catch (TimeoutException exception)
            {
                return LogExceptionResponse<T>(exception, fioRequest.Url);
            }
            catch (OperationCanceledException exception)
            {
                return LogExceptionResponse<T>(exception, fioRequest.Url);
            }
        }

        /// <summary>
        /// Handles responses with non-successful status codes (anything other than
        /// HttpStatusCode.OK) from the Fio Bank API. This method retrieves the server response
        /// content (if any) and creates a Result.Failure object based on the specific status code.
        /// </summary>
        /// <param name="fioRequest">
        /// The IFioApiRequest object containing the URL and the response content processor.
        /// </param>
        /// <param name="response">
        /// The HttpResponseMessage object containing the response from the Fio API.
        /// </param>
        /// <param name="cancellationToken">The cancellation token to cancel the operation (optional).</param>
        /// <returns>
        /// A Task of type Result containing an error message based on the specific status code and
        /// potentially server response content.
        /// </returns>
        private async Task<Result<T>> ProcessErrorStatusCodeResponseAsync<T>(IFioApiRequest<T> fioRequest, HttpResponseMessage response, CancellationToken cancellationToken)
        {
            var serverResponse = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

            if (response.StatusCode == HttpStatusCode.Conflict)
            {
                return LogErrorStatusCodeResponse<T>(response.StatusCode, fioRequest.Url, serverResponse, Resources.StatusCodeConflict);
            }

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return LogErrorStatusCodeResponse<T>(response.StatusCode, fioRequest.Url, serverResponse, Resources.StatusCodeNotFound);
            }

            if (response.StatusCode == HttpStatusCode.RequestEntityTooLarge)
            {
                return LogErrorStatusCodeResponse<T>(response.StatusCode, fioRequest.Url, serverResponse, Resources.StatusCodeRequestEntityTooLarge);
            }

            if (response.StatusCode == HttpStatusCode.UnprocessableEntity)
            {
                return LogErrorStatusCodeResponse<T>(response.StatusCode, fioRequest.Url, serverResponse, "");
            }

            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                if (response.Content.Headers.ContentLength == 0)
                {
                    return LogErrorStatusCodeResponse<T>(response.StatusCode, fioRequest.Url, serverResponse, "");
                }
                else
                {
                    return LogErrorStatusCodeResponse<T>(response.StatusCode, fioRequest.Url, serverResponse, "");
                }
            }

            return LogErrorStatusCodeResponse<T>(response.StatusCode, fioRequest.Url, serverResponse, "");
        }

        /// <summary>
        /// Logs the exception using the provided logger. Returns a Result.Failure object based on
        /// an exception that occurred.
        /// </summary>
        /// <typeparam name="T">The type of the data expected in the response.</typeparam>
        /// <param name="exception">The exception that occurred.</param>
        /// <param name="url">The URL of the API call.</param>
        /// <returns>A Result.Failure object containing an error message based on the exception.</returns>
        private Result<T> LogExceptionResponse<T>(Exception exception, string url)
        {
            _logger.LogFioApiCallException(exception.GetType().ToString(), url, exception);

            var data = new { ExceptionName = exception.GetType().ToString(), Uri = url };

            var failureMessage = Smart.Format(Resources.ExceptionDuringFioApiCall, data);

            return Result.Failure<T>(failureMessage);
        }

        /// <summary>
        /// Logs the API response using the provided logger. Creates a Result.Failure object based
        /// on a non-successful HTTP status code received from the Fio API.
        /// </summary>
        /// <typeparam name="T">The type of the data expected in the response.</typeparam>
        /// <param name="statusCode">The HTTP status code received from the API.</param>
        /// <param name="url">The URL of the API call.</param>
        /// <param name="serverResponse">The server response content (if any).</param>
        /// <param name="resource">The resource string associated with the status code.</param>
        /// <returns>
        /// A Result.Failure object containing an error message based on the status code and server response.
        /// </returns>
        private Result<T> LogErrorStatusCodeResponse<T>(HttpStatusCode statusCode, string url, string serverResponse, string resource)
        {
            _logger.LogStatusCodeErrorResponse(statusCode, url, resource, serverResponse);

            var data = new { StatusCode = statusCode, Uri = url, ServerResponse = serverResponse, Resource = resource };

            var str = Smart.Format(Resources.HttpStatusCodeValue, data);

            return Result.Failure<T>(str);
        }
    }
}