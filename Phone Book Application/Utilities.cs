using System.Text.Json;

namespace Phone_Book_Application;

public static class Utilities
{
    public static string ToJson(this object value, bool writeIndented = false)
    {
        return JsonSerializer.Serialize(value, new JsonSerializerOptions{ WriteIndented = writeIndented });
    }
}