namespace ExchangePrediction.Services.Contracts
{
    using System;

    public interface IPredictionService
    {
        (DateTime date, double prediction) Predict(string from, string to);

        double PredictOn(DateTime dateTime, string from, string to);
    }
}