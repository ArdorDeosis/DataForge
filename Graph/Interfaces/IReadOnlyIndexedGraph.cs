using System.Diagnostics.CodeAnalysis;

namespace Graph;

public interface IReadOnlyIndexedGraph<TNodeIndex, TNodeData, TEdgeData> where TNodeIndex : notnull
{
  IEnumerable<IIndexedNode<TNodeIndex, TNodeData, TEdgeData>> Nodes { get; }
  IEnumerable<IEdge<TNodeData, TEdgeData>> Edges { get; }
  IEnumerable<TNodeIndex> NodeIndices { get; }
  int Order { get; }
  int Size { get; }
  IIndexedNode<TNodeIndex, TNodeData, TEdgeData> this[TNodeIndex index] { get; }
  IIndexedNode<TNodeIndex, TNodeData, TEdgeData> GetNode(TNodeIndex index);

  bool TryGetNode(TNodeIndex index,
    [NotNullWhen(true)] out IIndexedNode<TNodeIndex, TNodeData, TEdgeData>? node);

  bool TryGetNodeIndex(INode<TNodeData, TEdgeData> node, [NotNullWhen(true)] out TNodeIndex? index);
  bool Contains(TNodeIndex index);
}