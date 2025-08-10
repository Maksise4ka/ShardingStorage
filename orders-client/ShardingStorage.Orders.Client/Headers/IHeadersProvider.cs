namespace ShardingStorage.Orders.Client.Headers;

public interface IHeadersProvider
{
    void Add(string key, string value);

    void Remove(string key);

    IDictionary<string, string> Get();
}