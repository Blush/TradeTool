namespace ConsoleApp1.Configuration;

public class TradeConfig
{
    public BinanceConfig Binance { get; set; } = new BinanceConfig();
    public UiConfig Ui { get; set; } = new UiConfig();

    public class UiConfig
    {
        public bool Verbose { get; set; } = false;
    }
}

public class BinanceConfig
{
    public string ApiUrl { get; set; } = "https://api.binance.com/api/v3";
}