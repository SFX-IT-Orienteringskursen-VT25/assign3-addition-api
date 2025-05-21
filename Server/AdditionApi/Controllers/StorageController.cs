using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;

namespace AdditionApi.Controllers
{
    [ApiController]
    [Route("storage")]
    public class StorageController : ControllerBase
    {
        private static ConcurrentDictionary<string, string> _storage = new();

        // PUT /storage/{key}
        [HttpPut("{key}")]
        public IActionResult SetItem(string key, [FromBody] string value)
        {
            _storage[key] = value;
            return NoContent(); // 204 No Content - matches localStorage.setItem() behavior
        }

        // GET /storage/{key}
[HttpGet("{key}")]
public IActionResult GetItem(string key)
{
    if (_storage.TryGetValue(key, out var value))
    {
        return Ok(new { key, value }); // nicer structure
    }
    return NotFound();
}

    }
}