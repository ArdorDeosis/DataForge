using System.Diagnostics.CodeAnalysis;

namespace Graph;

public interface IReadOnlyIndexedGraph<TNodeIndex, TNodeData, TEdgeData> where TNodeIndex : notnull
{
  IEnumerable<InternalNode<TNodeIndex, TNodeData, TEdgeData>> Nodes { get; }
  IEnumerable<InternalEdge<,,,>> Edges { get; }
  IEnumerable<TNodeIndex> NodeIndices { get; }
  int Order { get; }
  int Size { get; }
  InternalNode<TNodeIndex, TNodeData, TEdgeData> this[TNodeIndex index] { get; }
  InternalNode<TNodeIndex, TNodeData, TEdgeData> GetNode(TNodeIndex index);

  bool TryGetNode(TNodeIndex index,
    [NotNullWhen(true)] out InternalNode<TNodeIndex, TNodeData, TEdgeData>? node);

  bool TryGetNodeIndex(INode<TNodeData, TEdgeData> node, [NotNullWhen(true)] out TNodeIndex? index);
  bool Contains(TNodeIndex index);
}