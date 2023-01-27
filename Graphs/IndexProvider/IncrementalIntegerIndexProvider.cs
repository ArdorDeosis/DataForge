using System.Numerics;

namespace DataForge.Graphs;

public sealed class IncrementalIndexProvider<TIndex>
  : IIndexProvider<object, TIndex>
  where TIndex : IIncrementOperators<TIndex>
{
  private TIndex nextIndex;

  public IncrementalIndexProvider(TIndex startAt) => nextIndex = startAt;

  public TIndex GetIndex(object data) => nextIndex++;
}