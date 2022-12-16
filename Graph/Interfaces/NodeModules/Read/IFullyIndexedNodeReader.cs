using System.Diagnostics.CodeAnalysis;

namespace Graph;

public interface IFullyIndexedNodeReader<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> :
  INodeIndexedNodeReader<TNodeIndex, TNodeData, TEdgeData>,
  IEdgeIndexedNodeReader<TNodeData, TEdgeIndex, TEdgeData>
  where TNodeIndex : notnull
  where TEdgeIndex : notnull
{
  public new IEnumerable<ISelfAndEdgeIndexedNode<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>> Nodes { get; }

  public new ISelfAndEdgeIndexedNode<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> this[TNodeIndex index] { get; }
  public new ISelfAndEdgeIndexedNode<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> GetNode(TNodeIndex index);
  public new ISelfAndEdgeIndexedNode<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>? GetNodeOrNull(TNodeIndex index);

  public bool TryGetNode(TNodeIndex index,
    [NotNullWhen(true)] out ISelfAndEdgeIndexedNode<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>? node);
}