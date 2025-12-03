namespace AdditionApi;

public class StoredData(string key, string value)
{
    public string Key { get; set; } = key;
    public string Value { get; set; } = value;
}
