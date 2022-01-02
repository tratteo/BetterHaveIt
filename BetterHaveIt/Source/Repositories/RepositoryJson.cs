﻿using Newtonsoft.Json;

namespace BetterHaveIt.Repositories;

public class RepositoryJson<T> : IRepository<T> where T : class, new()
{
    private readonly string path;
    private readonly string name;
    private FileSystemWatcher? watcher;

    public T Data { get; private set; }

    public RepositoryJson(string compositePath, bool enableHotReload = true)
    {
        if (compositePath == null || compositePath == string.Empty)
        {
            throw new Exception($"Unable to deserialize {GetType().FullName}, path is empty");
        }

        Data = new T();
        (path, name) = PathExtensions.Split(compositePath!);
        if (enableHotReload) SetupHotReload();
        if (!Serializer.DeserializeJson($"{path}/", name, out T? parsed))
        {
            throw new Exception($"Unable to deserialize {GetType().Name} at {AppDomain.CurrentDomain.BaseDirectory}{path}/{name}");
        }
        if (parsed is not null)
        {
            Data = parsed;
        }
    }

    public event Action<bool> OnHotReloadTry = delegate { };

    public void Save()
    {
        if (watcher is not null) watcher.EnableRaisingEvents = false;
        Serializer.SerializeJson($"{path}/", name, Data);
        if (watcher is not null) watcher.EnableRaisingEvents = true;
    }

    private void SetupHotReload()
    {
        watcher = new FileSystemWatcher($"{path}")
        {
            NotifyFilter = NotifyFilters.LastWrite,
            Filter = name,
            IncludeSubdirectories = false,
            EnableRaisingEvents = true
        };
        watcher.Changed += async (sender, e) =>
        {
            watcher.EnableRaisingEvents = false;
            var tries = 10;
            var attemptDelay = 50;
            var success = false;
            for (var i = 0; i <= tries; ++i)
            {
                try
                {
                    if (Serializer.DeserializeJson($"{path}/", name, out T? parsed))
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
                    else if (ex is JsonReaderException)
                    {
                        success = false;
                        break;
                    }
                }
            }
            watcher.EnableRaisingEvents = true;
            OnHotReloadTry?.Invoke(success);
        };
    }
}