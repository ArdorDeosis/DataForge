namespace DataForge.Graphs.Tests;

internal sealed class IncrementalIntegerIndexProvider<TData> : IIndexProvider<TData, int>
{
  private int nextIndex;

  internal IncrementalIntegerIndexProvider(int startAt = 0)
  {
    nextIndex = startAt;
  }

  public int GetIndex(TData data) => nextIndex++;
}