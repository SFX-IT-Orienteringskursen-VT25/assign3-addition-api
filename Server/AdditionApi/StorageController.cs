// AdditionApi/StorageController.cs
using Microsoft.AspNetCore.Mvc;

namespace AdditionApi
{
    [ApiController]
    [Route("storage")]
    public class StorageController : ControllerBase
    {
        private static Dictionary<string, string> _storage = new();

        [HttpPost]
        public IActionResult SetItem([FromBody] StorageItem item)
        {
            _storage[item.Key] = item.Value;
            return Ok(new { message = "Item saved successfully" });
        }

        [HttpGet("{key}")]
        public IActionResult GetItem(string key)
        {
            if (_storage.TryGetValue(key, out var value))
            {
                return Ok(new { key, value });
            }

            return NotFound(new { message = "Key not found" });
        }
    }

    public class StorageItem
    {
        public string Key { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }
}
