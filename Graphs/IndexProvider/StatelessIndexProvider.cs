namespace DataForge.Graphs;

public sealed class StatelessIndexProvider<TData, TIndex> : IIndexProvider<TData, TIndex> where TIndex : notnull
{
  private readonly Func<TData, TIndex> generatorFunction;

  public StatelessIndexProvider(Func<TData, TIndex> generatorFunction)
  {
    this.generatorFunction = generatorFunction;
  }

  public TIndex GetIndex(TData data) => generatorFunction(data);

  public static implicit operator StatelessIndexProvider<TData, TIndex>(Func<TData, TIndex> generatorFunction) =>
    new(generatorFunction);
}