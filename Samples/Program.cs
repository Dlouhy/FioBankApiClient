using CSharpFunctionalExtensions;
using FioBankApiClient;
using FioBankApiClient.Models;
using FioBankApiClient.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetCoreUtils;

namespace Samples
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var applicationBuilder = Host.CreateApplicationBuilder(args);

            var apiKey = "pIWmw39EG..........";

            applicationBuilder.Services.AddFioBankApiClient(apiKey);

            using IHost host = applicationBuilder.Build();

            // Important: Fio API allows only one request per 30 seconds
            ApiClientService apiClientService = host.Services.GetRequiredService<ApiClientService>();

            // Request transactions in date range
            Result<DateTimeRange> rangeOrFailure = DateTimeRange.CreatePreviousMonth();

            if (rangeOrFailure.IsSuccess)
            {
                Result<AccountStatement> transactions = await apiClientService.GetTransactionsInDateRangeAsync(rangeOrFailure.Value);

                Result<string> xmlTransactions = await
                apiClientService.GetTransactionsInDateRangeAsync(rangeOrFailure.Value, ApiDataFormat.Xml);
            }

            // Request transactions since last download
            Result<AccountStatement> lastTransactions = await apiClientService.GetTransactionsSinceLastDownloadAsync();

            Result<string> csvLastTransactions = await apiClientService.GetTransactionsSinceLastDownloadAsync(ApiDataFormat.Csv);

            // Request all transactions since a certain statement ID and year
            Result<AccountStatement> transactionsByIdYear = await apiClientService.GetTransactionsByStatementIdAndYearAsync(1, 2024);

            Result<string> xmlTransactionsByIdYear = await apiClientService.GetTransactionsByStatementIdAndYearAsync(1, 2024, ApiDataFormat.Xml);

            // Gets Id and Year of last created official account statement, optionally filtered by year
            Result<string> lastStatement = await apiClientService.GetLastStatementAsync(2024);

            // Set the cursor to the date of the last unsuccessful download transaction
            DateTimeOffset unsuccessfulDateTime = new DateTimeOffset(2024, 5, 1, 8, 6, 32, new TimeSpan(1, 0, 0));
            await apiClientService.SetCursorToLastDateAsync(unsuccessfulDateTime);

            //Set the cursor to the ID of the last successfully downloaded movement
            await apiClientService.SetCursorToLastIdAsync(2);
        }
    }
}