namespace DataForge.Utilities;

public static class HashSetExtensions
{
  /// <summary>
  /// Creates a new read-only wrapper for the specified hash set.
  /// </summary>
  /// <typeparam name="T">The type of the elements in the hash set.</typeparam>
  /// <param name="set">The hash set to wrap.</param>
  /// <returns>A read-only wrapper for the specified hash set.</returns>
  public static ReadOnlyHashSet<T> ToReadOnly<T>(this HashSet<T> set) => new(set);
}