namespace AdditionApi;

public class StorageService
{
    private readonly Dictionary<string, string> _store = new();

    public void SetItem(string key, string value)
    {
        _store[key] = value;
    }

    public string? GetItem(string key)
    {
        return _store.TryGetValue(key, out var value) ? value : null;
    }
}