using System.Collections.Concurrent;
using Microsoft.AspNetCore.Mvc;

namespace AdditionApi.Controllers
{
    [ApiController]
    [Route("container")]
    public class LocalStorage : ControllerBase
    {
        private static ConcurrentDictionary<string, string> container = new();
        [HttpPost]
        public IActionResult Setdata([FromBody] StorageItem item)
        {
            container[item.Key] = item.Value;
            return Created($"/container/{item.Key}", item);
        }

        [HttpGet("{key}")]
        public IActionResult GetItem(string key)
        {
            if (container.TryGetValue(key, out var value))
            {
                return Ok(new { key, value });
            }
            return NotFound();
        }


        public class StorageItem
        {
            public string Key { get; set; }
            public string Value { get; set; }
        }
    

    
    }


}