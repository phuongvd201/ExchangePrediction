namespace ExchangePrediction.Services.Impl.Configs
{
    public class ApiConfig
    {
        public string AppId { get; set; }

        public string HistoricalApi { get; set; }

        public string LatestApi { get; set; }

        public int RequestRetryNumber { get; set; }
    }
}
