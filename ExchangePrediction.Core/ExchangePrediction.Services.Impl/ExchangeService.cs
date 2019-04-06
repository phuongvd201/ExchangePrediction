namespace ExchangePrediction.Services.Impl
{
    using Contracts;
    using Impl.Configs;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using Polly;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class ExchangeService : IExchangeService, IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly ApiConfig _apiConfig;

        public ExchangeService(IOptions<ApiConfig> apiConfig)
        {
            _httpClient = new HttpClient();
            _apiConfig = apiConfig.Value;
        }

        public async Task<Dictionary<string, double>> GetHistory(string date, IEnumerable<string> symbols)
        {
            return await GetRates($"{_apiConfig.HistoricalApi}{date}.json?app_id={_apiConfig.AppId}&symbols={string.Join(",", symbols)}");
        }

        public async Task<Dictionary<string, double>> GetCurrent(IEnumerable<string> symbols)
        {
            return await GetRates($"{_apiConfig.LatestApi}?app_id={_apiConfig.AppId}&symbols={string.Join(",", symbols)}");
        }

        private async Task<Dictionary<string, double>> GetRates(string requestUri)
        {
            var response = await Policy.Handle<HttpRequestException>().RetryAsync(_apiConfig.RequestRetryNumber, (exception, retryCount, context) =>
            {
                if (retryCount == _apiConfig.RequestRetryNumber)
                {
                    throw new Exception($"Failed to fetch data from ${requestUri}");
                }
            }).ExecuteAsync(async () => await _httpClient.GetStringAsync(requestUri));

            var rates = JsonConvert.DeserializeAnonymousType(response, new { rates = new Dictionary<string, double>() })?.rates;

            if (rates == null)
            {
                throw new Exception($"No data found from ${requestUri}");
            }

            return rates;
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}