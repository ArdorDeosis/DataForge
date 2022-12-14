using System.Diagnostics.CodeAnalysis;

namespace Graph;

public interface IReadOnlyIndexedGraph<TNodeIndex, TNodeData, TEdgeData> where TNodeIndex : notnull
{
  public IEnumerable<IIndexedNode<TNodeIndex, TNodeData, TEdgeData>> Nodes { get; }
  public IEnumerable<IIndexedEdge<TNodeIndex, TNodeData, TEdgeData>> Edges { get; }
  public IEnumerable<TNodeIndex> NodeIndices { get; }
  public int Order { get; }
  public int Size { get; }
  public IIndexedNode<TNodeIndex, TNodeData, TEdgeData> this[TNodeIndex index] { get; }
  public IIndexedNode<TNodeIndex, TNodeData, TEdgeData> GetNode(TNodeIndex index);

  public bool TryGetNode(TNodeIndex index,
    [NotNullWhen(true)] out IIndexedNode<TNodeIndex, TNodeData, TEdgeData>? node);

  public bool TryGetNodeIndex(INode<TNodeData, TEdgeData> node, [NotNullWhen(true)] out TNodeIndex? index);
  public bool Contains(TNodeIndex index);
}