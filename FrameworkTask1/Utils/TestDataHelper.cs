using FrameworkTask1.Model;
using Newtonsoft.Json.Linq;
using System.Text.Json;

namespace FrameworkTask1.Utils;

public static class TestDataHelper
{
    public static InstancesServiceConfiguration Get(string fileName)
    {
        string path = Path.Combine(Directory.GetCurrentDirectory(), "TestData", fileName);
        string jsonString = File.ReadAllText(path);
        return JsonSerializer.Deserialize<InstancesServiceConfiguration>(jsonString) ?? new();
    }
}
