using ConsoleApp1.Configuration;
using TradeTool.Domain;
using TradeTool.Services;

namespace TradeTool.Requests;

public interface IGetOrderBokRequest
{
    Task<OrderBook> ExecuteAsync(string instrument, int limit = 1000);
}

public class GetOrderBokRequest: BinanceRequestBase, IGetOrderBokRequest
{
    
    public GetOrderBokRequest(IWebClient client, BinanceConfig config) : base(client, config)
    {
    }
    
    public async Task<OrderBook> ExecuteAsync(string instrument, int limit = 1000)
    {
        var endpoint = $"depth?symbol={instrument}&limit={limit}";
        var orderBook = await GetAsync<BinanceOrderBook>(endpoint);
        var book = new OrderBook()
        {
            BuyOrders = orderBook.bids.Select(x => new Order()
            {
                Price = decimal.Parse(x[0]),
                Amount = decimal.Parse(x[1])
            }),
            SellOrders = orderBook.asks.Select(x => new Order()
            {
                Price = decimal.Parse(x[0]),
                Amount = decimal.Parse(x[1])
            })
        };
        return book;
    }
}

public class BinanceOrderBook
{
    public long lastUpdateId { get; set; }
    public string[][] bids { get; set; }
    public string[][] asks { get; set; }
}

public class BinanceErrorMessage
{
    public int code { get; set; }
    public string msg { get; set; }
}


