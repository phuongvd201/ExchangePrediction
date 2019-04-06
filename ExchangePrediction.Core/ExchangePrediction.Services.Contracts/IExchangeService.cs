namespace ExchangePrediction.Services.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IExchangeService
    {
        Task<Dictionary<string, double>> GetHistory(string date, IEnumerable<string> symbols);

        Task<Dictionary<string, double>> GetCurrent(IEnumerable<string> symbols);
    }
}
