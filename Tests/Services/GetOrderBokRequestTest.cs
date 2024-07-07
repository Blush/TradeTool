using ConsoleApp1.Configuration;
using TradeTool.Domain;
using TradeTool.Requests;
using TradeTool.Services;
using Moq;

namespace Tests.Services;

public class GetOrderBokRequestTest
{
    [Fact]
    public void ExecuteReturnsBook()
    {
        //Arrange
        var config = new BinanceConfig();
        var clientMock = new Mock<IWebClient>();
        clientMock.Setup(it => it.GetAsync("https://api.binance.com/api/v3/depth?symbol=BTCUSDT&limit=1000"))
            .ReturnsAsync(BinanceApiResponses.DepthBtcUsdt);
        
        var request = new GetOrderBokRequest(clientMock.Object, config);
        
        //Act
        var book = request.ExecuteAsync("BTCUSDT").Result;
        
        //Assert
        Assert.Equivalent(new OrderBook
        {
            BuyOrders = new List<Order>
            {
                new Order { Price = 60309.65000000m, Amount = 5.84473000m },
                new Order { Price = 60309.61000000m, Amount = 1.55181000m }
            },
            SellOrders = new List<Order>
            {
                new Order { Price = 60309.66000000m, Amount = 2.37566000m },
                new Order { Price = 60311.38000000m, Amount = 0.01858000m }
            }
        }, book);
    }
}