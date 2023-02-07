namespace DataForge.Utilities;

public interface IReadOnlyMultiValueDictionary<in TKey, TValue> where TKey : notnull
{
  /// <summary>
  /// Gets the collection of values associated with the specified key.
  /// If the key is not present in the dictionary, an empty read-only hash set is returned.
  /// </summary>
  /// <param name="key">The key to look up in the dictionary.</param>
  /// <returns>A read-only hash set containing the values associated with the specified key.</returns>
  ReadOnlyHashSet<TValue> this[TKey key] { get; }
}