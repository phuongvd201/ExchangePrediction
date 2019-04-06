namespace ExchangePrediction.IntegrationTests
{
    using ExchangePrediction.Providers.Contracts;
    using Microsoft.Extensions.Options;
    using Moq;
    using Services.Contracts;
    using Services.Impl;
    using Services.Impl.Configs;
    using System;
    using Xunit;

    public class PredictionIntegrationTests
    {
        private readonly IRegressionEquationService _regressionEquationService;
        private readonly IExchangeService _exchangeService;

        public PredictionIntegrationTests()
        {
            _regressionEquationService = new RegressionEquationService();
            _exchangeService = new ExchangeService(Options.Create(new ApiConfig
            {
                AppId = "6c4b389eb6d140caa0b6ebbbb460d515",
                HistoricalApi = "https://openexchangerates.org/api/historical/",
                LatestApi = "https://openexchangerates.org/api/latest.json",
                RequestRetryNumber = 3
            }));
        }

        [Fact]
        public void Predict_UsdTryOn2018Nov02()
        {
            var dateTimeProvider = CreateDateTimeProvider(2018, 11, 02);
            var predictionService = new PredictionService(dateTimeProvider, _exchangeService, _regressionEquationService);
            var prediction = Math.Round(predictionService.Predict("USD", "TRY").prediction, 3);
            var expected = 5.03;

            Assert.Equal(expected, prediction);
        }

        [Fact]
        public void Predict_UsdTryOn2017Jan15()
        {
            var dateTimeProvider = CreateDateTimeProvider(2017, 1, 15);
            var predictionService = new PredictionService(dateTimeProvider, _exchangeService, _regressionEquationService);
            var prediction = Math.Round(predictionService.Predict("USD", "TRY").prediction, 3);
            var expected = 2.842;

            Assert.Equal(expected, prediction);
        }

        private IDateTimeProvider CreateDateTimeProvider(int year, int month, int day)
        {
            var dateTimeProviderStub = new Mock<IDateTimeProvider>();

            dateTimeProviderStub.Setup(p => p.GetDate()).Returns(new DateTime(year, month, day));

            return dateTimeProviderStub.Object;
        }

    }
}