using ConsoleApp1.Configuration;
using TradeTool.Adapters;
using TradeTool.Requests;
using TradeTool.Services;
var config = new TradeConfig();

if(args.Any(arg => arg == "-v" || arg == "--verbose"))
{
    config.Ui.Verbose = true;
}

var request = new GetOrderBokRequest(new WebClient(), config.Binance);
var adapter = new CalculateAdapter(request, config);

if (args.Length == 1 && (args[0] == "/?" || args[0] == "-h" || args[0] == "--help"))
{
    Console.WriteLine("Trade tool - experimental application.");
    Console.WriteLine("At the moment implemented only function to calculate the market price for the given instrument and quantity.");
    Console.WriteLine("TradeTool <instrument> <quantity>");
    Console.WriteLine("instrument\t string (e.g. BTCUSDT), max length 10 characters.");
    Console.WriteLine("quantity\t Decimal number. Enter positive number for buy and negative for sell.");
    Console.WriteLine("");    
}

if (args.Length < 2)
{
    Console.WriteLine("Expected 2 arguments.");
    return;
}

var instrument = args[0];
var quantityParsed = decimal.TryParse(args[1], out var quantity);
if (string.IsNullOrWhiteSpace(instrument) || instrument.Trim().Length >10)
{
    Console.WriteLine("The instrument is required and must be max 10 characters long.");
    return; 
}

if (!quantityParsed || quantity == 0)
{
    Console.WriteLine("The quantity is required and must be a decimal number different from 0.");
    return; 
}

var price = await adapter.GetMarketPriceAsync(instrument, quantity);
Console.WriteLine($"The market price for {quantity} {instrument} is {price}.");