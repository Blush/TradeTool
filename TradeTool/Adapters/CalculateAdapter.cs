using ConsoleApp1.Configuration;
using TradeTool.Domain;
using TradeTool.Requests;

namespace TradeTool.Adapters;

public interface ICalculateAdapter
{
    Task<decimal> GetMarketPriceAsync(string instrument, decimal quantity);
}

public class CalculateAdapter: ICalculateAdapter
{
    private readonly IGetOrderBokRequest _getOrderBokRequest;
    private readonly TradeConfig _config;
    
    public CalculateAdapter(IGetOrderBokRequest getOrderBokRequest, TradeConfig config)
    {
        _getOrderBokRequest = getOrderBokRequest ?? throw new ArgumentNullException(nameof(getOrderBokRequest));
        _config = config ?? throw new ArgumentNullException(nameof(config));
    }

    /// <summary>
    /// Calculate the market price for the given instrument and quantity
    /// </summary>
    /// <returns>Expected price</returns>
    public async Task<decimal> GetMarketPriceAsync(string instrument, decimal inputQuantity)
    {
        if(inputQuantity == 0)
        {
            return 0m;
        }
        var isBuy = inputQuantity > 0;
        
        
        var quantity = Math.Abs(inputQuantity);

        var orders = await GetOrdersAsync(instrument, isBuy, 5000);
        return GetPrice(orders, quantity, isBuy);
    }

    private async Task<List<Order>> GetOrdersAsync(string instrument, bool isBuy, int limit)
    {
        var book = await _getOrderBokRequest.ExecuteAsync(instrument, limit);
        List<Order> orders;
            
        if(isBuy)
        {
            orders = book.SellOrders.OrderBy(it => it.Price).ToList();
        }
        else
        {
            orders = book.BuyOrders.OrderByDescending(it => it.Price).ToList();
        }

        DisplayVerbose($"To {operationType(isBuy)} {instrument} are prices from " +
            $"{orders.First().Price} ({orders.First().Amount}) to {orders.Last().Price} ({orders.Last().Amount}).");

        return orders;
    } 
    
    private string operationType (bool isBuy) => isBuy ? "buy" : "sell";

    private void DisplayVerbose(string message)
    {
        if (_config.Ui.Verbose)
            Console.WriteLine(message);
    }
    private decimal GetPrice(List<Order> orders, decimal quantity, bool isBuy)
    {
        var price = 0m;

        foreach (var order in orders)
        {
            if (quantity == 0)
            {
                DisplayVerbose("All the positions are calculated");
                
                break;
            };
            
            if (quantity < order.Amount)
            {
                DisplayVerbose($"Order price {order.Price}, amount {order.Amount}" +
                    $" ({order.Price * quantity} in sum), left to {operationType(isBuy)} {quantity}.");
                price += order.Price * quantity;
                quantity = 0;
            }
            else
            {
                DisplayVerbose($"Order price {order.Price}, amount {order.Amount}" +
                    $" ({order.Price * order.Amount} in sum), left to {operationType(isBuy)} {quantity}.");
                price += order.Price * order.Amount;
                quantity -= order.Amount;
            }
        }

        return price;
    }
}