// Copyright Siamango

namespace BetterHaveIt;

public class PathExtensions
{
    public static (string, string) Split(string path)
    {
        var index = path.LastIndexOf("\\");
        if (index == -1)
        {
            index = path.LastIndexOf("/");
        }
        var folder = index < 0 ? string.Empty : path[..(index + 1)];
        var name = path.Substring(index + 1, path.Length - index - 1);
        return (folder, name);
    }
}