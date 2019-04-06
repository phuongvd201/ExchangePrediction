namespace ExchangePrediction.Providers.Contracts
{
    using System;

    public interface IDateTimeProvider
    {
        DateTime GetDate();
    }
}