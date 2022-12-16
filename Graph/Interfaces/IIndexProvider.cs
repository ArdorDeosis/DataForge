namespace Graph;

public interface IIndexProvider<TIndex, TData>
{
  TIndex GetIndex(TData data);
}