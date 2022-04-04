using System.Text.Json;

namespace BetterHaveIt.Repositories;

public class RepositoryJson<T> : IRepository<T> where T : class, new()
{
    private readonly string path;
    private FileSystemWatcher? watcher;

    public T Data { get; private set; }

    public RepositoryJson(string path, bool create = true, bool reloadOnChange = true)
    {
        if (path == null || path == string.Empty)
        {
            throw new Exception($"Unable to deserialize {GetType().FullName}, path is empty");
        }
        this.path = path;
        Data = new T();
        if (!Serializer.DeserializeJson(path, out T? parsed))
        {
            parsed = null;
            if (!create)
            {
                throw new Exception($"Unable to deserialize {GetType().Name} at {AppDomain.CurrentDomain.BaseDirectory}{this.path}");
            }
            else
            {
                Serializer.SerializeJson(path, Data);
            }
        }
        if (parsed is not null)
        {
            Data = parsed;
        }
        if (reloadOnChange) SetupHotReload();
    }

    public event Action<bool> OnReloadTry = delegate { };

    public void Save()
    {
        if (watcher is not null) watcher.EnableRaisingEvents = false;
        Serializer.SerializeJson(path, Data);
        if (watcher is not null) watcher.EnableRaisingEvents = true;
    }

    private void SetupHotReload()
    {
        var dir = Path.GetDirectoryName(path);
        var name = Path.GetFileName(path);
        watcher = new FileSystemWatcher(dir is null ? string.Empty : dir)
        {
            NotifyFilter = NotifyFilters.LastWrite,
            Filter = name,
            IncludeSubdirectories = false,
            EnableRaisingEvents = true
        };
        watcher.Changed += OnFileChanged;
    }

    private async void OnFileChanged(object sender, FileSystemEventArgs args)
    {
        if (watcher is null) return;
        watcher.EnableRaisingEvents = false;
        var tries = 10;
        var attemptDelay = 50;
        var success = false;
        for (var i = 0; i <= tries; ++i)
        {
            try
            {
                if (Serializer.DeserializeJson(path, out T? parsed))
                {
                    if (parsed is not null)
                    {
                        success = true;
                        Data = parsed;
                        break;
                    }
                }
            }
            catch (Exception ex) when (i <= tries)
            {
                if (ex is IOException)
                {
                    await Task.Delay(attemptDelay);
                }
                else if (ex is JsonException)
                {
                    success = false;
                    break;
                }
            }
        }
        watcher.EnableRaisingEvents = true;
        OnReloadTry?.Invoke(success);
    }
}