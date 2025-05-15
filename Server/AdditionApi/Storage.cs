namespace AdditionApi;


public static class Storage
{
    private static readonly Dictionary<string, string> _store = new();

    public static void Set(string key, string value)
    {
        _store[key] = value;
    }

    public static string? Get(string key)
    {
        return _store.TryGetValue(key, out var value) ? value : null;
    }
}

public class StorageValue
{
    public string Value { get; set; } = string.Empty;
}
