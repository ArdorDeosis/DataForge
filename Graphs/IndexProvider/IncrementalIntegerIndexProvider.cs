using System.Numerics;

namespace DataForge.Graphs;

public sealed class IncrementalIndexProvider<TData, TIndex>
  : IIndexProvider<TData, TIndex>
  where TIndex : IIncrementOperators<TIndex>
{
  private TIndex nextIndex;

  public IncrementalIndexProvider(TIndex startAt) => nextIndex = startAt;

  public TIndex GetIndex(TData data) => nextIndex++;
}