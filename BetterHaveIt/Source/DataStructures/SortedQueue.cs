// Copyright Matteo Beltrame

namespace BetterHaveIt.DataStructures;

public enum SortingType
{
    DESCENDING,
    ASCENDING
}

public class SortedQueue<T> where T : IComparable<T>
{
    private readonly SortingType sorting;
    private List<T> objects;

    public int Count => objects.Count;

    public SortedQueue(SortingType sorting = SortingType.DESCENDING)
    {
        objects = new List<T>();
        this.sorting = sorting;
    }

    public void Enqueue(T val)
    {
        objects.Add(val);
        switch (sorting)
        {
            case SortingType.ASCENDING:
                objects = objects.OrderBy((x) => x).ToList();
                break;

            case SortingType.DESCENDING:
                objects = objects.OrderByDescending((x) => x).ToList();
                break;
        }
    }

    public T? Dequeue()
    {
        if (objects.Count > 0)
        {
            T elem = objects.ElementAt(0);
            objects.RemoveAt(0);
            return elem;
        }
        else
        {
            return default;
        }
    }

    public T? Last() => objects.Count > 0 ? objects.ElementAt(objects.Count - 1) : default;

    public T? First() => objects.Count > 0 ? objects.ElementAt(0) : default;

    public bool Remove(T elem) => objects.Remove(elem);

    public T? Find(Predicate<T> predicate) => objects.Find(predicate);

    public void Clear() => objects.Clear();
}