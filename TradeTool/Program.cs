using TradeTool.Configuration;
using TradeTool.Requests.Binance;
using TradeTool.Adapters;
using TradeTool.Domain;
using TradeTool.Services;

internal class Program
{
    public static async Task Main(string[] args)
    {
        DisplayInfo(args);
        
        string instrument;
        decimal quantity;
        decimal price;
        var config = new TradeConfig();
        try
        {
            (instrument, quantity) = ParseArguments(args, config);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return;
        }
        
        IGetOrderBookRequest request = new GetOrderBookRequest(new WebClient(), config.Binance);
        ICalculateAdapter adapter = new CalculateAdapter(request, config);

        try
        {
            price = await adapter.GetMarketPriceAsync(instrument, quantity);
        }
        catch (ResponseException e)
        {
            Console.WriteLine(e.Message);
            return;
        }
        catch (Exception e)
        {
            Console.WriteLine("Something went wrong during the calculation.");
            return;
        }

        Console.WriteLine($"The market price for {quantity} {instrument} is {price}.");
    }

    private static void DisplayInfo(string[] args)
    {
        if (args.Length == 1 && (args[0] == "/?" || args[0] == "-h" || args[0] == "--help"))
        {
            Console.WriteLine("Trade tool - experimental application. (C)Dmitrii Gavrilov 2024 blush@gmail.com");
            Console.WriteLine(
                "At the moment implemented only function to calculate the market price for the given instrument and quantity.");
            Console.WriteLine("TradeTool <instrument> <quantity> [-v|-verbose] [-h|-help]");
            Console.WriteLine("instrument\t string (e.g. BTCUSDT), max length 10 characters.");
            Console.WriteLine("quantity\t Decimal number. Enter positive number for buy and negative for sell.");
            Console.WriteLine("-v|-verbose\t Display verbose information.");
            Console.WriteLine("-h|-help\t Display this help.");
        }
    }
    
    private static (string instrument, decimal quantity) ParseArguments(string[] args, TradeConfig config)
    {
        if (args.Any(arg => arg == "-v" || arg == "--verbose"))
        {
            config.Ui.Verbose = true;
        }
        
        if (args.Length < 2)
        {
            throw new ArgumentException("Expected 2 arguments.");
        }

        var instrument = args[0];
        var quantityParsed = decimal.TryParse(args[1], out var quantity);
        
        if (string.IsNullOrWhiteSpace(instrument) || instrument.Trim().Length > 10)
        {
            throw new ArgumentException("The instrument is required and must be max 10 characters long.");
        }

        if (!quantityParsed || quantity == 0)
        {
            throw new ArgumentException("The quantity is required and must be a decimal number different from 0.");
        }

        return (instrument, quantity);
    }
}
