using FioBankApiClient.ApiClientRequests;
using FioBankApiClient.Serialization;
using Microsoft.Extensions.DependencyInjection;

namespace FioBankApiClient
{
    /// <summary>
    /// Provides extension methods for registering FIO Bank API client services in a dependency
    /// injection container.
    /// </summary>
    public static class ApiClientExtensions
    {
        /// <summary> Adds FIO Bank Api Client services to the specified Microsoft.Extensions.DependencyInjection.IServiceCollection.</summary>
        /// <param name="serviceCollection">The Microsoft.Extensions.DependencyInjection.IServiceCollection to add services to.</param>
        /// <param name="token">The authentication token for the FIO API.</param>
        /// <returns>
        /// The Microsoft.Extensions.DependencyInjection.IServiceCollection so that additional
        /// calls can be chained.
        ///</returns>
        public static IServiceCollection AddFioBankApiClient(this IServiceCollection serviceCollection, string token)
        {
            var tokenOrError = AccessToken.Create(token);
            if (tokenOrError.IsFailure)
                throw new ArgumentException(tokenOrError.Error, token);

            serviceCollection.AddTransient<ApiClientService>();
            serviceCollection.AddHttpClient<ApiClient>();
            serviceCollection.AddOptions<ApiClientOption>()
                    .Configure(options => options.AuthenticationToken = tokenOrError.Value);

            serviceCollection.AddTransient<RequestUrlFactory>();
            serviceCollection.AddSingleton<JsonApiDataSerializer>();

            serviceCollection.AddTransient<StringRequest>();
            serviceCollection.AddSingleton<Func<StringRequest>>(x => () => x.GetService<StringRequest>()!);
            serviceCollection.AddSingleton<StringRequestFactory>();

            serviceCollection.AddTransient<AccountStatementRequest>();
            serviceCollection.AddSingleton<Func<AccountStatementRequest>>(x => () => x.GetService<AccountStatementRequest>()!);
            serviceCollection.AddSingleton<AccountStatementRequestFactory>();

            return serviceCollection;
        }
    }
}