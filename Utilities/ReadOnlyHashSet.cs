using System.Collections;
using System.Runtime.Serialization;

namespace DataForge.Utilities;

public sealed class ReadOnlyHashSet<T> : IReadOnlySet<T>, ISerializable, IDeserializationCallback
{
  private readonly HashSet<T> set;

  public ReadOnlyHashSet(HashSet<T> original)
  {
    set = original;
  }

  /// <summary>
  /// Gets an empty read-only set.
  /// </summary>
  public static ReadOnlyHashSet<T> Empty => new(new HashSet<T>());

  /// <inheritdoc />
  public IEnumerator<T> GetEnumerator() => set.GetEnumerator();

  /// <inheritdoc />
  IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

  /// <inheritdoc />
  public bool Contains(T item) => set.Contains(item);

  /// <inheritdoc />
  public bool IsProperSubsetOf(IEnumerable<T> other) => set.IsProperSubsetOf(other);

  /// <inheritdoc />
  public bool IsProperSupersetOf(IEnumerable<T> other) => set.IsProperSubsetOf(other);

  /// <inheritdoc />
  public bool IsSubsetOf(IEnumerable<T> other) => set.IsProperSubsetOf(other);

  /// <inheritdoc />
  public bool IsSupersetOf(IEnumerable<T> other) => set.IsProperSubsetOf(other);

  /// <inheritdoc />
  public bool Overlaps(IEnumerable<T> other) => set.IsProperSubsetOf(other);

  /// <inheritdoc />
  public bool SetEquals(IEnumerable<T> other) => set.IsProperSubsetOf(other);

  /// <inheritdoc />
  public int Count => set.Count;

  /// <inheritdoc />
  public void GetObjectData(SerializationInfo info, StreamingContext context) => set.GetObjectData(info, context);

  /// <inheritdoc />
  public void OnDeserialization(object? sender) => set.OnDeserialization(sender);
}