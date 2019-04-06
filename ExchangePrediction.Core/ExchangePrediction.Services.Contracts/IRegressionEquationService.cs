namespace ExchangePrediction.Services.Contracts
{
    using System.Collections.Generic;

    public interface IRegressionEquationService
    {
        double SimpleLinear(double value, IEnumerable<(double x, double y)> samples);
    }
}
