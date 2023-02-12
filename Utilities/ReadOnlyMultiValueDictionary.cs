namespace DataForge.Utilities;

public sealed class ReadOnlyMultiValueDictionary<TKey, TValue> : IReadOnlyMultiValueDictionary<TKey, TValue>
  where TKey : notnull
{
  private readonly MultiValueDictionary<TKey, TValue> dictionary;

  public ReadOnlyMultiValueDictionary(MultiValueDictionary<TKey, TValue> original)
  {
    dictionary = original;
  }

  /// <summary>
  /// Gets an empty read-only set.
  /// </summary>
  public static ReadOnlyMultiValueDictionary<TKey, TValue> Empty => new(new MultiValueDictionary<TKey, TValue>());

  public ReadOnlyHashSet<TValue> this[TKey key] => dictionary[key];
}