namespace Utilities;

public static class DictionaryExtensions
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
}