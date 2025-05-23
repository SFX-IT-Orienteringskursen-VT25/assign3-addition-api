using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using Microsoft.OpenApi.Models;

namespace AdditionApi.Controllers
{
    [ApiController]
    [Route("storage")]
    public class StorageController : ControllerBase
    {
        private static ConcurrentDictionary<string, string> _storage = new();

        // POST /storage
        [HttpPost]
        public IActionResult SetItem([FromBody] StorageItem item)
        {
            if (string.IsNullOrWhiteSpace(item.Key) || item.Value == null)
                return BadRequest(new { message = "Key and Value are required." });

            _storage[item.Key] = item.Value;
            return Created($"/storage/{item.Key}", null);
        }

        // GET /storage/{key}
        [HttpGet("{key}")]
        public IActionResult GetItem(string key)
        {
            if (_storage.TryGetValue(key, out var value))
                return Ok(new { key, value });

            return NotFound();
        }
    }

    public class StorageItem
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}


