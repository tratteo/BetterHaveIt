// Copyright Siamango

using Newtonsoft.Json;

namespace BetterHaveIt;

public class Serializer
{
    public static bool DeserializeJson<T>(string path, out T? json, JsonSerializerSettings? settings = null)
    {
        if (File.Exists(path))
        {
            var jsonString = File.ReadAllText(path);
            var metadata = JsonConvert.DeserializeObject<T>(jsonString, settings);
            json = metadata;
            return true;
        }
        json = default;
        return false;
    }

    public static void SerializeJson<T>(string path, T metadata, bool createPath = true, JsonSerializerSettings? settings = null) =>
        WriteAll(path, JsonConvert.SerializeObject(metadata, settings), createPath);

    public static void WriteAll(string path, object obj, bool createPath = true)
    {
        var dir = Path.GetDirectoryName(path);
        if (createPath && dir is not null && !dir.Equals(string.Empty))
        {
            Directory.CreateDirectory(dir);
        }
        File.WriteAllText(path, obj.ToString());
    }

    public static string ReadAll(string path) => File.Exists(path) ? File.ReadAllText(path) : string.Empty;
}