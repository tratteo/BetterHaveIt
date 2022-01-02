using System.Threading.Tasks;

namespace BetterHaveIt.Repositories;

public interface IRepository<T>
{
    public T Data { get; }

    public void SaveAsync() => Task.Run(() => Save());

    public void Save();
}