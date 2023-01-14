namespace Graph;

public interface IReadOnlyUnindexedGraph<TNodeData, TEdgeData> : IReadOnlyGraph<TNodeData, TEdgeData>
{
  new IReadOnlyCollection<Node<TNodeData, TEdgeData>> Nodes { get; }
  new IReadOnlyCollection<Edge<TNodeData, TEdgeData>> Edges { get; }
}