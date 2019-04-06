namespace ExchangePrediction.Providers.Impl
{
    using Contracts;
    using System;

    public class CurrentDateTimeProvider : IDateTimeProvider
    {
        public DateTime GetDate()
        {
            return DateTime.Now;
        }
    }
}
