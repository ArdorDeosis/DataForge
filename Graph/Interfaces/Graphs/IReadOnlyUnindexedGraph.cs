namespace Graph;

public interface IReadOnlyUnindexedGraph<TNodeData, TEdgeData> : IReadOnlyGraph<TNodeData, TEdgeData>
{
  new IEnumerable<Node<TNodeData, TEdgeData>> Nodes { get; }
  new IEnumerable<Edge<TNodeData, TEdgeData>> Edges { get; }
}