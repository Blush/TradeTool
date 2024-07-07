using System.Runtime.CompilerServices;
using TradeTool.Configuration;
using TradeTool.Requests.Binance;
using Moq;
using TradeTool.Adapters;
using TradeTool.Domain;

namespace Tests.Adapters;

public class CalculateAdapterTest
{
    private readonly Mock<IGetOrderBookRequest> _request = new Mock<IGetOrderBookRequest>();

    [Fact]
    public void GetMarketPriceAsync_ReturnsZero_WhenQuantityZero()
    {
        //Arrange
        var adapter = new CalculateAdapter(_request.Object, new TradeConfig());
        
        //Act
        var result = adapter.GetMarketPriceAsync("BTCUSDT", 0).Result;
        
        //Assert
        Assert.Equal(0m, result);
    }
    
    [Theory]
    [InlineData("instrument", 0.5, 3)]
    [InlineData("instrument", 1, 6)]
    [InlineData("instrument", 2, 13)]
    [InlineData("instrument", 20, 153)]
    [InlineData("instrument", 0, 0)]
    [InlineData("instrument", -0.5, 4)]
    [InlineData("instrument", -1, 8)]
    [InlineData("instrument", -2, 15)]
    [InlineData("instrument", -20, 127)]
    public async Task GetMarketPriceAsync_ShouldReturnExpectedPrice(string instrument, decimal quantity, decimal expectedPrice)
    {
        // Arrange
        var mockOrderBookRequest = new Mock<IGetOrderBookRequest>();
        mockOrderBookRequest.Setup(m => m.ExecuteAsync(instrument, It.IsAny<int>()))
            .ReturnsAsync(GetSampleOrderBook(instrument));
        var calculateAdapter = new CalculateAdapter(mockOrderBookRequest.Object, new TradeConfig());

        // Act
        var result = await calculateAdapter.GetMarketPriceAsync(instrument, quantity);

        // Assert
        Assert.Equal(expectedPrice, result);
    }

    private OrderBook GetSampleOrderBook(string instrument)
    {
        return new OrderBook
        {
            BuyOrders = new List<Order>
            {
                new Order { Price = 6, Amount = 20 },
                new Order { Price = 8, Amount = 1 },
                new Order { Price = 7, Amount = 5 }
            },
            SellOrders = new List<Order>
            {
                new Order { Price = 6, Amount = 1 },
                new Order { Price = 8, Amount = 20 },
                new Order { Price = 7, Amount = 5 }
            }
        };
    }
}


