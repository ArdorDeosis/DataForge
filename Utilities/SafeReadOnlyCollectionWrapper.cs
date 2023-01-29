using System.Collections;

namespace DataForge.Utilities;

internal sealed class SafeReadOnlyCollectionWrapper<T> : IReadOnlyCollection<T>
{
  private readonly IReadOnlyCollection<T> collection;

  public SafeReadOnlyCollectionWrapper(IReadOnlyCollection<T> collection) => this.collection = collection;

  public IEnumerator<T> GetEnumerator() => collection.GetEnumerator();

  IEnumerator IEnumerable.GetEnumerator() => collection.GetEnumerator();

  public int Count => collection.Count;
}