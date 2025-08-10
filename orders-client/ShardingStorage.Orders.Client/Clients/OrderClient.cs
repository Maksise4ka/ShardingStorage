using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using ShardingStorage.Orders.Client.Headers;
using ShardingStorage.Orders.Client.Models;
using ShardingStorage.Orders.Client.Requests;

namespace ShardingStorage.Orders.Client.Clients;

public class OrderClient : IOrderClient
{
    private readonly HttpClient _httpClient;
    private readonly IHeadersProvider _headersProvider;

    public OrderClient(HttpClient httpClient, IHeadersProvider headersProvider)
    {
        _httpClient = httpClient;
        _headersProvider = headersProvider;
    }

    public Order Create(CreateOrderRequest request)
    {
        var httpRequest = new HttpRequestMessage(HttpMethod.Post, new Uri("api/order", UriKind.Relative))
        {
            Content = new StringContent(JsonSerializer.Serialize(request), new MediaTypeHeaderValue("application/json"))
        };
        
        AddHeaders(_headersProvider, httpRequest.Headers);
        
        var httpPesponse = _httpClient.Send(httpRequest);
        
        if (httpPesponse.IsSuccessStatusCode is false)
            throw new HttpRequestException($"Status code is {httpPesponse.StatusCode}");

        return JsonSerializer.Deserialize<Order>(httpPesponse.Content.ReadAsStringAsync().Result)
            ?? throw new ArgumentNullException();
    }

    public IReadOnlyCollection<Order> Get()
    {
        var httpRequest = new HttpRequestMessage(HttpMethod.Get, new Uri("/api/order", UriKind.Relative));

        AddHeaders(_headersProvider, httpRequest.Headers);
        
        var httpPesponse = _httpClient.Send(httpRequest);
        
        if (httpPesponse.IsSuccessStatusCode is false)
            throw new HttpRequestException($"Status code is {httpPesponse.StatusCode}");

        return JsonSerializer.Deserialize<IReadOnlyCollection<Order>>(httpPesponse.Content.ReadAsStringAsync().Result)
               ?? throw new ArgumentNullException();
    }

    private static void AddHeaders(IHeadersProvider headersProvider, HttpRequestHeaders headers)
    {
        foreach (var header in headersProvider.Get())
            headers.Add(header.Key, header.Value);
    }
}