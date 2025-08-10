using Microsoft.AspNetCore.Mvc;
using ShardingStorage.Orders.WebApi.Models;
using ShardingStorage.Orders.WebApi.Requests;

namespace ShardingStorage.Orders.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private static readonly List<Order> Orders = new();
    
    [HttpPost]
    public ActionResult<Order> Create([FromBody] CreateOrderRequest request)
    {
        var order = new Order
        {
            Id = Orders.Count + 1,
            Items = request.Items
        };
        
        Orders.Add(order);
        
        return Ok(order);
    }

    [HttpGet]
    public ActionResult<IReadOnlyCollection<Order>> Get()
    {
        return Ok(Orders);
    }

    [HttpGet("{orderId:long}")]
    public ActionResult<Order> GetById(long orderId)
    {
        var order = Orders.Find(x => x.Id == orderId);

        if (order is null)
            return NotFound();

        return Ok(order);
    }
}