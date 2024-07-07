namespace TradeTool.Services;

public interface IWebClient
{
    Task<string> GetAsync(string url, Action<HttpResponseMessage> responseStatusHandler = null);
}

public class WebClient: IWebClient
{
    private readonly HttpClient Client = new HttpClient();
    
    public async Task<string> GetAsync(string url, Action<HttpResponseMessage> responseStatusHandler = null)
    {
        var response = await Client.GetAsync(url);
        if(responseStatusHandler != null)
            responseStatusHandler(response);
        else
            response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}