﻿using System.Net;
using System.Text.Json;
using TradeTool.Configuration;
using TradeTool.Domain;
using TradeTool.Services;

namespace TradeTool.Requests.Binance;

public interface IGetOrderBookRequest
{
    Task<OrderBook> ExecuteAsync(string instrument, int limit = 1000);
}

public class GetOrderBookRequest: RequestBase, IGetOrderBookRequest
{
    
    public GetOrderBookRequest(IWebClient client, BinanceConfig config) : base(client, config)
    {
    }
    
    public async Task<OrderBook> ExecuteAsync(string instrument, int limit = 1000)
    {
        var endpoint = $"depth?symbol={instrument}&limit={limit}";
        var orderBook = await GetAsync<BinanceOrderBook>(endpoint, HandleResponseStatus);  
        
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
    
    private void HandleResponseStatus(HttpResponseMessage response)
    {
        if(response.StatusCode != HttpStatusCode.OK)
        {
            var content = response.Content.ReadAsStringAsync().Result;
            var error = JsonSerializer.Deserialize<BinanceErrorMessage>(content);
            throw new ResponseException($"Error happened during the orders book request: {error.msg}");
        }
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


