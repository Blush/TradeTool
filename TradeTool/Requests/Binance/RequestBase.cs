using System.Text.Json;
using TradeTool.Configuration;
using TradeTool.Services;

namespace TradeTool.Requests.Binance;

public abstract class RequestBase
{
    private string BaseUrl;
    private readonly IWebClient Client;

    protected RequestBase(IWebClient client, BinanceConfig config)
    {
        Client = client ?? throw new ArgumentNullException(nameof(client));
        BaseUrl = config.ApiUrl;
    }

    protected async Task<T> GetAsync<T>(string endpoint, Action<HttpResponseMessage> responseStatusHandler = null)
    {
        var url = $"{BaseUrl}/{endpoint}";
        var body = await Client.GetAsync(url, responseStatusHandler);
        return JsonSerializer.Deserialize<T>(body);
    }
}