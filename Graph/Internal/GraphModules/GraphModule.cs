using System.Diagnostics.CodeAnalysis;

namespace Graph;

internal abstract class GraphModule<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>
  where TNodeIndex : notnull where TEdgeIndex : notnull
{
  protected readonly InternalGraph<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> Graph;

  private protected GraphModule(InternalGraph<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> graph)
  {
    Graph = graph;
  }

  protected bool TryGetNodeIndexInThisGraph(INode<TNodeData, TEdgeData> node, [NotNullWhen(true)] out TNodeIndex? index)
  {
    if (node is Node<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> internalNode &&
        internalNode.Graph == Graph &&
        internalNode.TryGetIndex(out var retrievedIndex))
    {
      index = retrievedIndex;
      return true;
    }

    index = default;
    return false;
  }

  protected bool TryGetEdgeIndexInThisGraph(IEdge<TNodeData, TEdgeData> edge, [NotNullWhen(true)] out TEdgeIndex? index)
  {
    if (edge is Edge<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> internalNode &&
        internalNode.Graph == Graph &&
        internalNode.TryGetIndex(out var retrievedIndex))
    {
      index = retrievedIndex;
      return true;
    }

    index = default;
    return false;
  }
}