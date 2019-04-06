namespace ExchangePrediction.Services.Impl
{
    using Contracts;
    using System.Collections.Generic;

    public class RegressionEquationService : IRegressionEquationService
    {
        public RegressionEquationService()
        {
        }

        public double SimpleLinear(double value, IEnumerable<(double x, double y)> samples)
        {
            int n = 0;
            double sumX = 0, sumY = 0, sumXY = 0, sumPowX = 0;

            foreach (var (x, y) in samples)
            {
                sumX += x;
                sumY += y;
                sumXY += (x * y);
                sumPowX += (x * x);
                n++;
            }

            var slope = CalcSlope(n, sumX, sumY, sumXY, sumPowX);
            var intercept = CalcIntercept(n, sumX, sumY, slope);

            return intercept + slope * value;
        }

        private double CalcSlope(int n, double sumX, double sumY, double sumXY, double sumPowX)
        {
            var divider = ((n * sumPowX) - (sumX * sumX));
            return ((n * sumXY) - (sumX * sumY)) / (divider == 0 ? 1 : divider) ;
        }

        private double CalcIntercept(int n, double sumX, double sumY, double slope)
        {
            return (sumY - (slope * sumX)) / n;
        }
    }
}
