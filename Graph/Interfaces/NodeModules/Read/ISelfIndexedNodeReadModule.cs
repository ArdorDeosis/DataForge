using System.Diagnostics.CodeAnalysis;

namespace Graph;

public interface ISelfIndexedNodeReadModule<TNodeIndex, TNodeData, TEdgeData> :
  INodeReadModule<TNodeData, TEdgeData>
  where TNodeIndex : notnull
{
  public new IEnumerable<ISelfIndexedNode<TNodeIndex, TNodeData, TEdgeData>> Nodes { get; }
  public IEnumerable<TNodeIndex> NodeIndices { get; }

  public ISelfIndexedNode<TNodeIndex, TNodeData, TEdgeData> this[TNodeIndex index] { get; }
  public ISelfIndexedNode<TNodeIndex, TNodeData, TEdgeData> GetNode(TNodeIndex index);
  public ISelfIndexedNode<TNodeIndex, TNodeData, TEdgeData>? GetNodeOrNull(TNodeIndex index);

  public bool TryGetNode(TNodeIndex index,
    [NotNullWhen(true)] out ISelfIndexedNode<TNodeIndex, TNodeData, TEdgeData>? node);

  public bool TryGetNodeIndex(INode<TNodeData, TEdgeData> node, [NotNullWhen(true)] out TNodeIndex? index);
  public bool Contains(TNodeIndex index);
  public bool ContainsNode(TNodeIndex index);
}