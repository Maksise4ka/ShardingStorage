namespace ShardingStorage.Orders.Client.Headers;

public class HeadersProvider : IHeadersProvider
{
    private readonly Dictionary<string, string> _headers = new();
    
    public void Add(string key, string value)
    {
        _headers[key] = value;
    }

    public void Remove(string key)
    {
        _headers.Remove(key);
    }

    public IDictionary<string, string> Get()
    {
        return _headers;
    }
}