namespace TradeTool.Domain;

/// <summary>
/// Throw to inform the user a problem from the response
/// </summary>
public class ResponseException: Exception
{
    public ResponseException(string message): base (message)
    {
    }
}