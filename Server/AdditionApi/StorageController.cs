using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("storage")]
public class StorageController : ControllerBase
{
    private readonly StorageService _storage;

    public StorageController(StorageService storage)
    {
        _storage = storage;
    }

    // Equivalent to localStorage.setItem(key, value)
    [HttpPost("{key}")]
    public IActionResult SetItem(string key, [FromBody] StorageRequest request)
    {
        if (request?.Value == null)
            return BadRequest("Value is required.");

        _storage.SetItem(key, request.Value);

        return Ok(new { message = "Stored successfully", key });
    }

    // Equivalent to localStorage.getItem(key)
    [HttpGet("{key}")]
    public IActionResult GetItem(string key)
    {
        var value = _storage.GetItem(key);

        if (value == null)
            return NotFound(new { message = "Key not found", key });

        return Ok(new { key, value });
    }
}

public class StorageRequest
{
    public string? Value { get; set; }
}
