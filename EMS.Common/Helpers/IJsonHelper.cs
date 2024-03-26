using System;
using System.Collections.Generic;
using System.Text.Json;

namespace EMS.Common.Helpers;

public interface IJsonHelper
{
    void WriteToFile(string filePath, string json);
    string ReadFromFile(string filePath);
    T Deserialize<T>(string json);
    string Serialize<T>(T obj);
}
