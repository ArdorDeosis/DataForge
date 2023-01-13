namespace Graph;

public sealed class StatelessIndexProvider<TData, TIndex> : IIndexProvider<TData, TIndex>
{
  private readonly Func<TData, TIndex> generatorFunction;

  public StatelessIndexProvider(Func<TData, TIndex> generatorFunction)
  {
    this.generatorFunction = generatorFunction;
  }

  public TIndex GetIndex(TData data)
  {
    return generatorFunction(data);
  }

  public static implicit operator StatelessIndexProvider<TData, TIndex>(Func<TData, TIndex> generatorFunction)
  {
    return new(generatorFunction);
  }
}