namespace Utilities;

/// <summary>
/// Represents a generic dictionary that maps keys to a collection of values.
/// </summary>
/// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
/// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
public sealed class HashSetDictionary<TKey, TValue> where TKey : notnull
{
  private readonly Dictionary<TKey, HashSet<TValue>> dictionary = new();

  /// <summary>
  /// Gets the collection of values associated with the specified key.
  /// If the key is not present in the dictionary, an empty read-only hash set is returned.
  /// </summary>
  /// <param name="key">The key to look up in the dictionary.</param>
  /// <returns>A read-only hash set containing the values associated with the specified key.</returns>
  public ReadOnlyHashSet<TValue> this[TKey key] =>
    dictionary.TryGetValue(key, out var set)
      ? set.AsReadOnly()
      : ReadOnlyHashSet<TValue>.Empty;

  /// <summary>
  /// Adds the specified value to the collection of values associated with the specified key.
  /// If the key is not present in the dictionary, a new entry is created.
  /// </summary>
  /// <param name="key">The key to associate the value with.</param>
  /// <param name="value">The value to add to the collection.</param>
  public void Add(TKey key, TValue value)
  {
    if (!dictionary.ContainsKey(key))
      dictionary.Add(key, new HashSet<TValue>());
    dictionary[key].Add(value);
  }

  /// <summary>
  /// Removes the specified key and its associated collection of values from the dictionary.
  /// </summary>
  /// <param name="key">The key to remove from the dictionary.</param>
  /// <returns>True if the key was found and removed from the dictionary, false otherwise.</returns>
  public bool Remove(TKey key) => dictionary.Remove(key);

  /// <summary>
  /// Removes all items from the collections of values in the dictionary that satisfy the specified predicate.
  /// </summary>
  /// <param name="predicate">A function to test each value for a condition.</param>
  public void RemoveAll(Predicate<TValue> predicate)
  {
    foreach (var key in dictionary.Keys.ToArray())
      RemoveAllFrom(key, predicate);
  }

  /// <summary>
  /// Removes the specified value from the collection of values associated with the specified key.
  /// </summary>
  /// <param name="key">The key whose associated collection should be modified.</param>
  /// <param name="value">The value to remove from the collection.</param>
  /// <returns>True if the value was found and removed from the collection, false otherwise.</returns>
  public bool RemoveFrom(TKey key, TValue value)
  {
    if (!dictionary.TryGetValue(key, out var set))
      return false;
    var result = set.Remove(value);
    if (set.Count == 0)
      dictionary.Remove(key);
    return result;
  }

  /// <summary>
  /// Removes all items from the collection of values associated with the specified key that satisfy the specified
  /// predicate.
  /// </summary>
  /// <param name="key">The key whose associated values will be removed.</param>
  /// <param name="predicate">A function to test each value for a condition.</param>
  /// <returns>The number of items removed from the collection of values.</returns>
  public int RemoveAllFrom(TKey key, Predicate<TValue> predicate)
  {
    if (!dictionary.TryGetValue(key, out var set))
      return 0;
    var result = set.RemoveWhere(predicate);
    if (set.Count == 0)
      dictionary.Remove(key);
    return result;
  }
}