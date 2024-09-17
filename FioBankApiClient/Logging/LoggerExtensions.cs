using Microsoft.Extensions.Logging;
using System.Net;

namespace FioBankApiClient.Logging
{
    /// <summary>
    /// A class containing extension methods for the <see cref="ILogger"/> interface. This class
    /// cannot be inherited.
    /// </summary>
    public static class LoggerExtensions
    {
        /// <summary>
        /// Logging delegate for when Fio Bank Api returns error HttpStatusCode response.
        /// </summary>
        private static readonly Action<ILogger, HttpStatusCode, string, string, string, Exception> _LogStatusCodeErrorResponse = LoggerMessage.Define<HttpStatusCode, string, string, string>(
                    logLevel: LogLevel.Error,
                        eventId: EventIds.LogFioApiResponse,
                        formatString: Properties.Resources.HttpStatusCodeValue);

        /// <summary>
        /// Logging delegate for when exception is throw during Fio Bank Api call.
        /// </summary>
        private static readonly Action<ILogger, string, string, Exception> _LogFioApiCallException = LoggerMessage.Define<string, string>(
         logLevel: LogLevel.Error,
         eventId: EventIds.LogFioApiCallException,
         formatString: Properties.Resources.ExceptionDuringFioApiCall);

        /// <summary>
        /// Logging delegate for when exception is throw during json deserialization of api response data.
        /// </summary>
        private static readonly Action<ILogger, string, Exception> _LogJsonDeserializationException = LoggerMessage.Define<string>(
         logLevel: LogLevel.Error,
         eventId: EventIds.LogJsonDeserializationException,
         formatString: Properties.Resources.ExceptionDuringJsonDeserialization);

        /// <summary>
        /// Logging delegate for when stream to deserialize is closed.
        /// </summary>
        private static readonly Action<ILogger, Exception> _LogResponseStreamClosed = LoggerMessage.Define(logLevel: LogLevel.Error,
           eventId: EventIds.LogResponseStreamClosed,
           formatString: Properties.Resources.ResponseStreamClosed);

        /// <summary>
        /// Logging delegate for when stream to deserialize is null.
        /// </summary>
        private static readonly Action<ILogger, Exception> _LogResponseStreamNull = LoggerMessage.Define(logLevel: LogLevel.Error,
           eventId: EventIds.LogResponseStreamNull,
           formatString: Properties.Resources.ResponseStreamNull);

        /// <summary>
        /// Logs that Exception has been throw during Fio Bank Api call.
        /// </summary>
        /// <param name="logger">The logger to use.</param>
        /// <param name="exceptionName">The name of exception.</param>
        /// <param name="uri">The request Url.</param>
        /// <param name="exception">The exception that was thrown.</param>
        public static void LogFioApiCallException(this ILogger logger, string exceptionName, string uri, Exception exception)
        {
            _LogFioApiCallException(logger, exceptionName, uri, exception);
        }

        /// <summary>
        /// Logs error response from Fio Bank Api.
        /// </summary>
        /// <param name="logger">The logger to use.</param>
        /// <param name="statusCode">Returned HttpStatusCode from server.</param>
        /// <param name="uri">The request Url.</param>
        /// <param name="resource">Resource message.</param>
        /// <param name="serverResponse">Returned response from server.</param>
        public static void LogStatusCodeErrorResponse(this ILogger logger, HttpStatusCode statusCode, string uri, string resource, string serverResponse)
        {
            _LogStatusCodeErrorResponse(logger, statusCode, serverResponse, uri, resource, null);
        }

        /// <summary>
        /// Log that Fio bank Api response stream closed.
        /// </summary>
        /// <param name="logger">The logger to use.</param>
        public static void LogResponseStreamClosed(this ILogger logger)
        {
            _LogResponseStreamClosed(logger, null);
        }

        /// <summary>
        /// Log that Fio bank Api response stream null.
        /// </summary>
        /// <param name="logger">The logger to use.</param>
        public static void LogResponseStreamNull(this ILogger logger)
        {
            _LogResponseStreamNull(logger, null);
        }

        /// <summary>
        /// Logs that Exception has been throw during deserializaion of response stram from FIO Fio
        /// Bank Api call.
        /// </summary>
        /// <param name="logger">The logger to use.</param>
        /// <param name="exceptionName">The name of exception.</param>
        /// <param name="exception">The exception that was thrown.</param>
        public static void LogFioApiDeserializerException(this ILogger logger, string exceptionName, Exception exception)
        {
            _LogJsonDeserializationException(logger, exceptionName, exception);
        }
    }
}