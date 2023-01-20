using System.Numerics;

namespace DataForge.Graphs;

internal sealed class IncrementalIndexProvider<TData> : IIndexProvider<TData, int>
{
  private int nextIndex;

  public IncrementalIndexProvider(int startAt = 0)
  {
    nextIndex = startAt;
  }

  public int GetIndex(TData data) => nextIndex++;
}