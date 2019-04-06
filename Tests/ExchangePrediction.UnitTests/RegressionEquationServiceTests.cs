namespace ExchangePrediction.UnitTests
{
    using Services.Contracts;
    using Services.Impl;
    using System;
    using Xunit;

    public class RegressionEquationServiceTests
    {
        private readonly IRegressionEquationService _regressionEquationService;

        public RegressionEquationServiceTests()
        {
            _regressionEquationService = new RegressionEquationService();
        }

        [Fact]
        public void Calc_UsdAndTryOn2017Jan15_ReturnsTryRate()
        {
            var samples = new(double x, double y)[]
            {
                (1, 3.04359),
                (2, 2.945926),
                (3, 2.893866),
                (4, 2.854589),
                (5, 2.970913),
                (6, 2.925991),
                (7, 2.990882),
                (8, 2.944941),
                (9, 2.970704),
                (10, 3.089218),
                (11, 3.284672),
                (12, 3.503773),
            };
            var prediction = Math.Round(_regressionEquationService.SimpleLinear(1, samples), 3);
            var expected = 2.842;

            Assert.Equal(expected, prediction);
        }

        [Fact]
        public void Calc_EasyCalculationSample_ReturnsY()
        {
            var samples = new(double x, double y)[]
            {
                (60, 3.1),
                (61, 3.6),
                (62, 3.8),
                (63, 4),
                (65, 4.1)
            };
            var prediction = Math.Round(_regressionEquationService.SimpleLinear(64, samples), 3);
            var expected = 4.058;

            Assert.Equal(expected, prediction);
        }
    }
}