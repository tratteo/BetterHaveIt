// Copyright Siamango

using System.Text.Json;

namespace BetterHaveIt;

public class Serializer
{
    public static bool DeserializeJson<T>(string path, string name, out T? json)
    {
        if (File.Exists(path + name))
        {
            var jsonString = File.ReadAllText(path + name);
            T? metadata = JsonSerializer.Deserialize<T>(jsonString);
            json = metadata;
            return true;
        }
        json = default;
        return false;
    }

    public static void SerializeJson<T>(string path, string name, T metadata, bool createPath = true)
    {
        WriteAll(path, name, JsonSerializer.Serialize(metadata, new JsonSerializerOptions() { WriteIndented = true }), createPath);
    }

    public static void WriteAll(string path, string name, object obj, bool createPath = true)
    {
        if (createPath && !path.Equals(string.Empty))
        {
            Directory.CreateDirectory(path);
        }
        File.WriteAllText(path + name, obj.ToString());
    }

    public static string ReadAll(string path, string name) => File.Exists(path + name) ? File.ReadAllText(path + name) : string.Empty;
}