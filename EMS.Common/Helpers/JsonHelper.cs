using System;
using System.Collections.Generic;
using System.Text.Json;

namespace EmployeeManagement;

public class JsonHelper : IJsonHelper
{
    public void WriteToFile(string filePath, string json)
    {
        File.WriteAllText(filePath, json);
    }

    public string ReadFromFile(string filePath)
    {
        return File.ReadAllText(filePath);
    }

    public T Deserialize<T>(string json)
    {
        return JsonSerializer.Deserialize<T>(json);
    }

    public string Serialize<T>(T obj)
    {
        return JsonSerializer.Serialize(obj, new JsonSerializerOptions { WriteIndented = true });
    }
}
