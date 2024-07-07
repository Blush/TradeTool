namespace TradeTool.Services;

public interface IWebClient
{
    Task<string> GetAsync(string url);
}

public class WebClient: IWebClient
{
    private readonly HttpClient Client = new HttpClient();
    
    public async Task<string> GetAsync(string url)
    {
        var response = await Client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}