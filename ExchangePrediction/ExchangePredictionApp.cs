namespace ExchangePrediction
{
    using Constants;
    using Microsoft.Extensions.Logging;
    using Services.Contracts;
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading;

    public class ExchangePredictionApp
    {
        private readonly IPredictionService _predictionService;
        private readonly ILogger<ExchangePredictionApp> _logger;

        public ExchangePredictionApp(
            IPredictionService predictionService,
            ILogger<ExchangePredictionApp> logger,
            IAutoCompleteHandler autoCompleteHandler)
        {
            _predictionService = predictionService;
            _logger = logger;

            ReadLine.AutoCompletionHandler = autoCompleteHandler;
        }

        public void Start()
        {
            try
            {
                var command = ReadLine.Read(">>> ").Trim();

                if (Commands.Exit.Equals(command, StringComparison.OrdinalIgnoreCase))
                {
                    return;
                }

                if (Commands.Clear.Equals(command, StringComparison.OrdinalIgnoreCase))
                {
                    Console.Clear();
                }
                else if (command.IndexOf(Commands.ExchangePredict, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    if (!IsCommandValid(command))
                    {
                        _logger.LogError("Invalid Command");
                        Thread.Sleep(100);
                    }
                    else
                    {
                        var (from, to) = GetParameters(command);
                        var (date, prediction) = _predictionService.Predict(from, to);

                        Console.WriteLine($"The predicted currency exchange from {from} to {to} for {date:dd/MM/yyyy} is {prediction}.");
                    }
                }
                else
                {
                    _logger.LogError("Command not found");
                    Thread.Sleep(100);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Start();
        }

        private bool IsCommandValid(string input)
        {
            return Regex.IsMatch(input, "exchangePredict[ ]+from=[a-zA-Z]+[ ]+to=[a-zA-Z]+", RegexOptions.IgnoreCase);
        }

        private (string from, string to) GetParameters(string command)
        {
            var arr = command.Split(' ').Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();

            return (arr[1].Split('=')[1].Trim().ToUpper(), arr[2].Split('=')[1].Trim().ToUpper());
        }
    }
}