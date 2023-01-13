using System.Diagnostics.CodeAnalysis;

namespace Graph;

public interface IUnindexedGraph<TNodeData, TEdgeData> : 
  IReadOnlyUnindexedGraph<TNodeData, TEdgeData>, 
  IGraph<TNodeData, TEdgeData>
{
  public Node<TNodeData, TEdgeData> AddNode(TNodeData data);
  public IEnumerable<Node<TNodeData, TEdgeData>> AddNode(IEnumerable<TNodeData> data);
  
  public new Edge<TNodeData, TEdgeData> AddEdge(Node<TNodeData, TEdgeData> origin,
    Node<TNodeData, TEdgeData> destination, TEdgeData data);

  public bool TryAddEdge(Node<TNodeData, TEdgeData> origin,
    Node<TNodeData, TEdgeData> destination, TEdgeData data,
    [NotNullWhen(true)] out Edge<TNodeData, TEdgeData>? edge);
}