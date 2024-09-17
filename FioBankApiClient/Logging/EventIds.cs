using Microsoft.Extensions.Logging;

namespace FioBankApiClient.Logging
{
    /// <summary>
    /// Contains static readonly EventIds used for logging purposes within the class
    /// LoggerExtensions.
    /// </summary>
    internal static class EventIds
    {
        internal static readonly EventId LogFioApiCallException = new(1, nameof(LogFioApiCallException));

        internal static readonly EventId LogFioApiResponse = new(2, nameof(LogFioApiResponse));

        internal static readonly EventId LogJsonDeserializationException = new(3, nameof(LogJsonDeserializationException));

        internal static readonly EventId LogResponseStreamClosed = new(4, nameof(LogResponseStreamClosed));

        internal static readonly EventId LogResponseStreamNull = new(5, nameof(LogResponseStreamNull));
    }
}