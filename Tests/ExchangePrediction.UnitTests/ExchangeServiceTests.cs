namespace ExchangePrediction.UnitTests
{
    using Microsoft.Extensions.Options;
    using Services.Contracts;
    using Services.Impl;
    using Services.Impl.Configs;
    using Xunit;

    public class ExchangeServiceTests
    {
        private readonly IExchangeService _exchangeService;

        public ExchangeServiceTests()
        {
            _exchangeService = new ExchangeService(Options.Create(new ApiConfig
            {
                AppId = "6c4b389eb6d140caa0b6ebbbb460d515",
                HistoricalApi = "https://openexchangerates.org/api/historical/",
                LatestApi = "https://openexchangerates.org/api/latest.json",
                RequestRetryNumber = 3
            }));
        }

        [Theory]
        [InlineData("TRY", "2016-01-15", 3.04359)]
        [InlineData("TRY", "2016-02-15", 2.945926)]
        [InlineData("TRY", "2016-03-15", 2.893866)]
        [InlineData("TRY", "2016-04-15", 2.854589)]
        [InlineData("TRY", "2016-05-15", 2.970913)]
        [InlineData("TRY", "2016-06-15", 2.925991)]
        [InlineData("TRY", "2016-07-15", 2.990882)]
        [InlineData("TRY", "2016-08-15", 2.944941)]
        [InlineData("TRY", "2016-09-15", 2.970704)]
        [InlineData("TRY", "2016-10-15", 3.089218)]
        [InlineData("TRY", "2016-11-15", 3.284672)]
        [InlineData("TRY", "2016-12-15", 3.503773)]
        public void Get_ExchangeRateOfCurrencyIn2016_ReturnsRates(string currency, string date, double expected)
        {
            var rate = _exchangeService.GetHistory(date, new[] { currency }).Result[currency];

            Assert.Equal(rate, expected);
        }


        [Theory]
        [InlineData("USD")]
        [InlineData("TRY")]
        public void Get_LatestExchangeRateOfCurrency_ReturnsRate(string currency)
        {
            var rate = _exchangeService.GetCurrent(new[] { currency }).Result;

            Assert.True(rate.ContainsKey(currency));
        }
    }
}