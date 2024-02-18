using System.Collections.Concurrent;
using Microsoft.AspNetCore.Mvc;
using SampleApi.Models;

namespace SampleApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ItemsController : ControllerBase
{
    private readonly ConcurrentDictionary<Guid, Item> _items = new();

    public ItemsController()
    {
        for (var i = 0; i < 5; i++)
        {
            var id = Guid.NewGuid();
            var item = new Item(id, $"Item {i}", $"Description {i}", DateTime.Now, null);
            _items.TryAdd(id, item);
        }
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Item>), 200)]
    public async Task<ActionResult<IEnumerable<Item>>> Get()
    {
        await Task.Delay(10);

        var items = _items.Values.OrderBy(x => x.CreatedAt).ToList();
        return Ok(items);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(Item), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<Item>> Get(Guid id)
    {
        await Task.Delay(10);

        if (_items.TryGetValue(id, out var item))
        {
            return Ok(item);
        }

        return NotFound();
    }

    [HttpPost]
    [ProducesResponseType(typeof(Item), 201)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<Item>> Post([FromBody] PostItemRequest postItemRequest)
    {
        await Task.Delay(10);

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var item = new Item(Guid.NewGuid(), postItemRequest.Name, postItemRequest.Description, DateTime.Now, null);

        return CreatedAtAction(nameof(Get), new {id = item.Id}, item);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(Item), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<Item>> Put(Guid id, [FromBody] PutItemRequest putItemRequest)
    {
        await Task.Delay(10);

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (id != putItemRequest.Id)
        {
            return BadRequest("The id in the URL does not match the id in the request body.");
        }

        if (!_items.TryGetValue(id, out var item)) return NotFound();

        var updatedItem = item with
        {
            Name = putItemRequest.Name,
            Description = putItemRequest.Description,
            UpdatedAt = DateTime.Now
        };

        _items.TryUpdate(id, updatedItem, item);
        return Ok(updatedItem);
    }
}