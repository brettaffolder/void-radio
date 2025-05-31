using System.Collections.ObjectModel;

namespace VoidRadio.WinUI.Extensions;

public static class ObservableCollectionExtensions
{
    public static void AddRange<TSource>(this ObservableCollection<TSource> source, IEnumerable<TSource> items)
    {
        foreach (var item in items)
        {
            source.Add(item);
        }
    }

    public static void SortBy<TSource, TKey>(this ObservableCollection<TSource> source, Func<TSource, TKey> keySelector)
    {
        IEnumerable<TSource> items = [.. source.OrderBy(keySelector)];

        source.Clear();

        foreach (var item in items)
        {
            source.Add(item);
        }
    }

    public static void SortByDescending<TSource, TKey>(this ObservableCollection<TSource> source, Func<TSource, TKey> keySelector)
    {
        IEnumerable<TSource> items = [.. source.OrderByDescending(keySelector)];

        source.Clear();

        foreach (var item in items)
        {
            source.Add(item);
        }
    }
}