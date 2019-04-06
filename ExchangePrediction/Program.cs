namespace ExchangePrediction
{
    using Handlers;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Providers.Contracts;
    using Providers.Impl;
    using Services.Contracts;
    using Services.Impl;
    using Services.Impl.Configs;
    using System;
    using System.IO;

    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Path.Combine(AppContext.BaseDirectory)).AddJsonFile("appsettings.json", true, true);
            var configuration = builder.Build();
            var serviceCollection = new ServiceCollection();
            var serviceProvider = serviceCollection
                .AddOptions()
                .AddLogging(opt => opt.AddConsole())
                .Configure<ApiConfig>(configuration.GetSection(nameof(ApiConfig)))
                .AddScoped<IDateTimeProvider, CurrentDateTimeProvider>()
                .AddScoped<IExchangeService, ExchangeService>()
                .AddScoped<IRegressionEquationService, RegressionEquationService>()
                .AddScoped<IPredictionService, PredictionService>()
                .AddScoped<IAutoCompleteHandler, AutoCompleteHandler>()
                .AddScoped<ExchangePredictionApp>()
                .BuildServiceProvider();

            serviceProvider.GetService<ExchangePredictionApp>().Start();
        }
    }
}