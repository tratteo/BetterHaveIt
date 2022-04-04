namespace BetterHaveIt.Repositories;

public interface IRepository<T>
{
    public T Data { get; }

    public event Action<bool> OnReloadTry;

    public void SaveAsync() => Task.Run(() => Save());

    public void Save();
}