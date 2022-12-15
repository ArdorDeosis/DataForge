namespace Graph;

public sealed class AutoIndexedGraph<TNodeIndex, TNodeData, TEdgeData> { }

public interface IIndexProvider<TIndex, TData>
{
  TIndex GetIndex(TData data);
}