using Microsoft.AspNetCore.Mvc;
using SampleApi.Models;
using SampleApi.Services;

namespace SampleApi.Controllers;

/// <summary>
/// Items controller is a simple CRUD controller for items.
/// </summary>
/// <param name="itemsService"></param>
[ApiController]
[Route("[controller]")]
public class ItemsController(ItemsService itemsService) : ControllerBase
{
    /// <summary>
    /// Retrieves all items.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Item>), 200)]
    public async Task<ActionResult<IEnumerable<Item>>> Get()
    {
        await Task.Delay(10);

        var items = itemsService.GetItems();
        return Ok(items);
    }

    /// <summary>
    /// Gets a single item by its id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(Item), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<Item>> Get(Guid id)
    {
        await Task.Delay(10);

        var item = itemsService.GetItem(id);
        if (item is null) return NotFound();
        
        return Ok(item);
    }

    /// <summary>
    /// Adds a new item.
    /// </summary>
    /// <param name="postItemRequest"></param>
    /// <returns></returns>
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

        var item = itemsService.AddItem(postItemRequest.Name, postItemRequest.Description);

        return CreatedAtAction(nameof(Get), new {id = item.Id}, item);
    }

    /// <summary>
    /// Updates an existing item.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="putItemRequest"></param>
    /// <returns></returns>
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

        var (ok, updatedItem) = itemsService.UpdateItem(id, putItemRequest.Name, putItemRequest.Description);
        if (!ok) return NotFound();

        return Ok(updatedItem);
    }
}