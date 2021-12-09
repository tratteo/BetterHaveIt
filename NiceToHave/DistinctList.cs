// Copyright Matteo Beltrame

using System.Collections;
using System.Collections.Generic;

namespace NiceToHave;

public class DistinctList<T> : IList<T>
{
    private readonly List<T> list;

    public int Count => list.Count;

    public bool IsReadOnly => false;

    public DistinctList()
    {
        list = new List<T>();
    }

    public T this[int index]
    {
        get => list[index];
        set
        {
            if (list.Contains(value)) return;
            list[index] = value;
        }
    }

    public void Add(T item)
    {
        if (list.Contains(item)) return;
        list.Add(item);
    }

    public void Clear() => list.Clear();

    public bool Contains(T item) => list.Contains(item);

    public void CopyTo(T[] array, int arrayIndex) => list.CopyTo(array, arrayIndex);

    public IEnumerator<T> GetEnumerator() => list.GetEnumerator();

    public int IndexOf(T item) => list.IndexOf(item);

    public void Insert(int index, T item) => list.Insert(index, item);

    public bool Remove(T item) => list.Remove(item);

    public void RemoveAt(int index) => list.RemoveAt(index);

    IEnumerator IEnumerable.GetEnumerator() => list.GetEnumerator();
}