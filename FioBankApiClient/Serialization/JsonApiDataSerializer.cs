using CSharpFunctionalExtensions;
using FioBankApiClient.Logging;
using FioBankApiClient.Models;
using FioBankApiClient.Properties;
using Microsoft.Extensions.Logging;
using SmartFormat;
using System.Text.Json;

namespace FioBankApiClient.Serialization
{
    /// <summary>
    /// JsonApiSerializer supports deserialization objects into the json:api format.
    /// </summary>
    public class JsonApiDataSerializer
    {
        private static readonly JsonSerializerOptions DefaultJsonSerializerOptions = new(JsonSerializerDefaults.General)
        {
            PropertyNameCaseInsensitive = true
        };

        private readonly ILogger<JsonApiDataSerializer> _logger;

        public JsonApiDataSerializer(ILogger<JsonApiDataSerializer> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Attempts to asynchronously deserialize a stream containing JSON data into an
        /// AccountStatement object.
        /// </summary>
        /// <param name="streamContent">Stream with data in Json format to deserialize.</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns>
        /// On success, the deserialized AccountStatement object is returned. Otherwise, a
        /// Result.Failure with an informative error message is returned.
        /// </returns>
        public async Task<Result<AccountStatement>> TryDeserializeAsync(Stream streamContent, CancellationToken cancellationToken)
        {
            if (streamContent == null)
            {
                _logger.LogResponseStreamNull();

                return Result.Failure<AccountStatement>(Resources.ResponseStreamNull);
            }

            if (!streamContent.CanRead)
            {
                _logger.LogResponseStreamClosed();

                return Result.Failure<AccountStatement>(Resources.ResponseStreamClosed);
            }

            try
            {
                var result = await JsonSerializer.DeserializeAsync<BaseRoot>(streamContent, DefaultJsonSerializerOptions, cancellationToken).ConfigureAwait(false);

                if (result != null)
                {
                    return Result.Success(result.AccountStatement);
                }
                else
                {
                    return Result.Failure<AccountStatement>(Resources.JsonDeserializationResultNull);
                }
            }
            catch (JsonException ex)
            {
                return LogExceptionDuringJsonDeserialization(ex);
            }
            catch (NotSupportedException ex)
            {
                return LogExceptionDuringJsonDeserialization(ex);
            }
        }

        private Result<AccountStatement> LogExceptionDuringJsonDeserialization(Exception ex)
        {
            _logger.LogFioApiDeserializerException(ex.GetType().ToString(), ex);

            var data = new { ExceptionName = ex.GetType().ToString() };

            var failureMessage = Smart.Format(Resources.ExceptionDuringJsonDeserialization, data);

            return Result.Failure<AccountStatement>(failureMessage);
        }
    }
}