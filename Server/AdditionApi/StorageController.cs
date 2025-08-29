using System.Collections.Concurrent;
using Microsoft.AspNetCore.Mvc;
namespace AdditionApi;

[ApiController]
[Route("api/[controller]")]
public class StorageController : ControllerBase
{
    private static ConcurrentDictionary<string, string> _store = new();

    [HttpPost]
    public IActionResult SetItem([FromBody] StorageItem item)
    {
        if (string.IsNullOrEmpty(item.Key))
            return BadRequest();
        
        _store[item.Key] = item.Value;  // Use the actual key
        return Ok(new { item.Key, item.Value });
    }

    [HttpGet("{key}")]
    public IActionResult GetItem(string key)
    {
        if (_store.TryGetValue(key, out var value))
        {
            return Ok(new { key, value });
        }

        return NotFound(new { message = "Key not found" });
    }
}