using System.Numerics;

namespace DataForge.Graphs;

/// <summary>
/// A generic <see cref="IIndexProvider{TData,TIndex}"/> generating increasing indices.
/// </summary>
/// <inheritdoc />
public sealed class IncrementalIndexProvider<TData, TIndex>
  : IIndexProvider<TData, TIndex>
  where TIndex : IIncrementOperators<TIndex>
{
  private TIndex nextIndex;

  public IncrementalIndexProvider(TIndex startAt) => nextIndex = startAt;

  /// <inheritdoc />
  public void Move() => ++nextIndex;

  /// <inheritdoc />
  public TIndex GetCurrentIndex(TData data) => nextIndex;
}