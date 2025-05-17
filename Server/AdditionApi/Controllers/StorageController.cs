using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;

namespace AdditionApi.Controllers
{
    [ApiController]
    [Route("storage")]
    public class StorageController : ControllerBase
    {
        // 模拟 "localStorage" 的内存数据库
        private static ConcurrentDictionary<string, string> _storage = new();

        // POST /storage
        [HttpPost]
        public IActionResult SetItem([FromBody] StorageItem item)
        {
            _storage[item.Key] = item.Value;
            return Created($"/storage/{item.Key}", null);
        }

        // GET /storage/{key}
        [HttpGet("{key}")]
        public IActionResult GetItem(string key)
        {
            if (_storage.TryGetValue(key, out var value))
            {
                return Ok(new { key, value });
            }
            return NotFound();
        }
    }

    // 请求体的数据结构
    public class StorageItem
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
