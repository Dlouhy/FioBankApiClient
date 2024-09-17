using CSharpFunctionalExtensions;
using FioBankApiClient.Properties;

namespace FioBankApiClient
{
    /// <summary>
    /// Represents value object for Fio Bank API access token.
    /// </summary>
    public sealed class AccessToken : ValueObject
    {
        public string Token { get; }

        private AccessToken(string token)
        {
            Token = token;
        }

        /// <summary>
        /// Creates a new AccessToken object from a string.
        /// </summary>
        /// <param name="token">The access token string.</param>
        /// <returns>
        /// Returns a Result object indicating success or failure. On failure, the Error property
        /// will contain an error message.
        /// </returns>
        public static Result<AccessToken> Create(string token)
        {
            token = (token ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(token))
                return Result.Failure<AccessToken>(Resources.AccessTokenCanNotBeEmpty);

            if (token.Length < 64)
                return Result.Failure<AccessToken>(Resources.AccessTokenMinimalLength);

            if (!token.All(char.IsLetterOrDigit) || !token.All(char.IsAscii))
            {
                var forbided = FindForbiddenChars(token);
                return Result.Failure<AccessToken>(Resources.AccessTokenHasForbiddenChars + $"{forbided}");
            }

            return Result.Success(new AccessToken(token));
        }

        /// <summary>
        /// Gets a comma-separated string containing all forbidden characters found in the provided token.
        /// </summary>
        /// <param name="token">The access token string.</param>
        /// <returns>Returns comma-separated string containing all forbidden characters.</returns>
        private static string FindForbiddenChars(string token)
        {
            IEnumerable<char> forbiddenChars = token.Where(c => !char.IsLetterOrDigit(c) || !char.IsAscii(c));
            string commaSeparatedString = string.Join(", ", forbiddenChars);
            return commaSeparatedString;
        }

        public override string ToString()
        {
            return Token;
        }

        protected override IEnumerable<IComparable> GetEqualityComponents()
        {
            yield return Token;
        }
    }
}