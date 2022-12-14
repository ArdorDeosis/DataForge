using System.Diagnostics.CodeAnalysis;

namespace Utilities;

public static class CollectionExtensions
{
  /// <summary>
  /// Adds an entry with the provided key and value to the <see cref="IDictionary{TKey,TValue}"/>. If an entry with the
  /// provided key already exists in the dictionary, its value is overwritten.
  /// </summary>
  public static void ForceAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
  {
    if (dictionary.ContainsKey(key))
      dictionary[key] = value;
    else
      dictionary.Add(key, value);
  }

  /// <summary>
  /// Attempts to retrieve the first element in the specified enumerable that satisfies the given predicate.
  /// </summary>
  /// <typeparam name="T">The type of elements in the enumerable.</typeparam>
  /// <param name="enumerable">The enumerable to search.</param>
  /// <param name="predicate">The predicate used to determine which element to return.</param>
  /// <param name="value">If successful, contains the first element in the enumerable that satisfies the given predicate; otherwise, the default value of the type.</param>
  /// <returns>True if an element is found that satisfies the given predicate; otherwise, false.</returns>
  public static bool TryGetFirst<T>(this IEnumerable<T> enumerable, Predicate<T> predicate, out T value)
  {
    if (enumerable == null)
      throw new ArgumentNullException(nameof(enumerable));

    if (predicate == null)
      throw new ArgumentNullException(nameof(predicate));

    foreach (var element in enumerable)
    {
      if (!predicate(element))
        continue;
      value = element;
      return true;
    }

    value = default!;
    return false;
  }
}