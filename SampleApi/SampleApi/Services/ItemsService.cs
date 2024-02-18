using System.Collections.Concurrent;
using SampleApi.Models;

namespace SampleApi.Services;

public class ItemsService
{
    private readonly ConcurrentDictionary<Guid, Item> _items = new();

    public ItemsService()
    {
        for (var i = 0; i < 5; i++)
        {
            var id = Guid.NewGuid();
            var item = new Item(id, $"Item {i}", $"Description {i}", DateTime.Now, null);
            _items.TryAdd(id, item);
        }
    }
    
    public IEnumerable<Item> GetItems()
    {
        return _items.Values.OrderBy(x => x.CreatedAt).ToList();
    }
    
    public Item? GetItem(Guid id)
    {
        return _items.GetValueOrDefault(id);
    }
    
    public Item AddItem(string name, string? description)
    {
        var item = new Item(Guid.NewGuid(), name, description, DateTime.Now, null);
        _items.TryAdd(item.Id, item);
        return item;
    }

    public (bool, Item?) UpdateItem(Guid id, string name, string? description)
    {
        if (!_items.TryGetValue(id, out var item))
        {
            return (false, null);
        }
        
        var updatedItem = item with
        {
            Name = name,
            Description = description,
            UpdatedAt = DateTime.Now
        };
        
        _items.TryUpdate(id, updatedItem, item);
        return (true, updatedItem);
    }
    
    public bool DeleteItem(Guid id)
    {
        return _items.TryRemove(id, out _);
    }
}