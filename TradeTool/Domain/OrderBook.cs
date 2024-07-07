namespace TradeTool.Domain;

public class OrderBook
{
    public IEnumerable<Order> BuyOrders { get; set; }
    public IEnumerable<Order> SellOrders { get; set; }
}