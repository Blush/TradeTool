using System.Text.Json;
using ConsoleApp1.Configuration;
using TradeTool.Services;

namespace TradeTool.Requests;

public abstract class BinanceRequestBase
{
    private string BaseUrl;
    private readonly IWebClient Client;

    protected BinanceRequestBase(IWebClient client, BinanceConfig config)
    {
        Client = client ?? throw new ArgumentNullException(nameof(client));
        BaseUrl = config.ApiUrl;
    }

    protected async Task<T> GetAsync<T>(string endpoint)
    {
        var url = $"{BaseUrl}/{endpoint}";
        var body = await Client.GetAsync(url);
        return JsonSerializer.Deserialize<T>(body);
    }
}