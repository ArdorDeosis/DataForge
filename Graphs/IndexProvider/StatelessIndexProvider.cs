namespace DataForge.Graphs;

/// <summary>
/// A generic <see cref="IIndexProvider{TData,TIndex}"/> that uses a generator function to generate an index from
/// provided data.
/// </summary>
/// <inheritdoc />
public sealed class StatelessIndexProvider<TData, TIndex> : IIndexProvider<TData, TIndex> where TIndex : notnull
{
  private readonly Func<TData, TIndex> generatorFunction;

  public StatelessIndexProvider(Func<TData, TIndex> generatorFunction)
  {
    this.generatorFunction = generatorFunction;
  }

  /// <inheritdoc />
  public void Move() { }

  /// <inheritdoc />
  public TIndex GetCurrentIndex(TData data) => generatorFunction(data);
}