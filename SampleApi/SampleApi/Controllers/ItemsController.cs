using Microsoft.AspNetCore.Mvc;
using SampleApi.Models;
using SampleApi.Services;

namespace SampleApi.Controllers;

/// <summary>
/// Items controller is a simple CRUD controller for items.
/// </summary>
/// <param name="itemsService"></param>
[ApiController]
[Route("api/[controller]")]
public class ItemsController(ItemsService itemsService) : ControllerBase
{
    /// <summary>
    /// Get All Items
    /// </summary>
    /// <response code="200">Returns all items</response>
    /// <remarks>
    /// This is the remarks section in the XML docs, which appear to be mapped to
    /// the request description in Postman
    /// </remarks>
    /// <returns></returns>
    [HttpGet(Name = "GetAllItems")]
    [ProducesResponseType(typeof(IEnumerable<Item>), 200)]
    public async Task<ActionResult<IEnumerable<Item>>> Get()
    {
        await Task.Delay(10);

        var items = itemsService.GetItems();
        return Ok(items);
    }

    /// <summary>
    /// Get Item
    /// </summary>
    /// <param name="id"></param>
    /// <response code="200">Returns the item</response>
    /// <response code="404">The item does not exist</response>
    /// <returns></returns>
    [HttpGet("{id:guid}", Name = "GetItem")]
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
    /// Post Item
    /// </summary>
    /// <param name="postItemRequest"></param>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /Items
    ///     {
    ///        "name": "Item #1",
    ///        "description": "Description of item #1"
    ///     }
    ///
    /// </remarks>
    /// <response code="201">Returns the item and its resource path</response>
    /// <response code="400">Returns the issues associated with the request</response>
    /// <returns></returns>
    [HttpPost(Name = "PostItem")]
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
    /// Put Item
    /// </summary>
    /// <param name="id"></param>
    /// <param name="putItemRequest"></param>
    /// <response code="201">Returns the updated item</response>
    /// <response code="400">Returns the issues associated with the request</response>
    /// <response code="404">The item does not exist</response>
    /// <returns></returns>
    [HttpPut("{id:guid}", Name = "PutItem")]
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

    /// <summary>
    /// Delete Item
    /// </summary>
    /// <param name="id"></param>
    /// <response code="204">The item was deleted</response>
    /// <response code="404">The item does not exist</response>
    /// <returns></returns>
    [HttpDelete("{id:guid}", Name = "DeleteItem")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<ActionResult> Delete(Guid id)
    {
        await Task.Delay(10);

        var ok = itemsService.DeleteItem(id);
        if (!ok) return NotFound();

        return NoContent();
    }

    /// <summary>
    /// Get Item by Name
    /// </summary>
    /// <param name="name"></param>
    /// <response code="200">Returns the item</response>
    /// <response code="404">The item does not exist</response>
    /// <returns></returns>
    [HttpGet("name/{name}", Name = "GetItemByName")]
    [ProducesResponseType(typeof(Item), 200)]
    [ProducesResponseType(404)]
    [Obsolete("This endpoint is deprecated. Use the GET /items/{id} endpoint instead.")]
    public async Task<ActionResult<Item>> GetByName(string name)
    {
        await Task.Delay(10);

        var item = itemsService.GetItems().FirstOrDefault(x => x.Name == name);
        if (item is null) return NotFound();
        
        return Ok(item);
    }
}