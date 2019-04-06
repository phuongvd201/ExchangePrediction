namespace ExchangePrediction.Services.Impl
{
    using Providers.Contracts;
    using Services.Contracts;
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Threading.Tasks;

    public class PredictionService : IPredictionService
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IExchangeService _exchangeService;
        private readonly IRegressionEquationService _regressionEquationService;

        public PredictionService(
            IDateTimeProvider dateTimeProvider,
            IExchangeService exchangeService,
            IRegressionEquationService regressionEquationService)
        {
            _dateTimeProvider = dateTimeProvider;
            _exchangeService = exchangeService;
            _regressionEquationService = regressionEquationService;
        }

        public (DateTime date, double prediction) Predict(string from, string to)
        {
            var date = _dateTimeProvider.GetDate();

            return (date, PredictOn(date, from, to));
        }

        public double PredictOn(DateTime dateTime, string from, string to)
        {
            var previousDates = Enumerable.Range(0, 12).Select(i => dateTime.AddMonths(-(i + 1)));
            var samples = new ConcurrentBag<(double x, double y)>();

            Task.WaitAll(previousDates.Select(date => Task.Run(async () =>
            {
                try
                {
                    var dateStr = date.ToString("yyyy-MM-dd");
                    var exchangeRate = await _exchangeService.GetHistory(dateStr, new[] { from, to });
                    (double x, double y) sample = (0, 0);

                    sample.x = date.Month;
                    sample.y = exchangeRate[to] / exchangeRate[from];
                    samples.Add(sample);
                }
                catch
                {
                    throw;
                }
            })).ToArray());

            return _regressionEquationService.SimpleLinear(dateTime.Month, samples);
        }
    }
}