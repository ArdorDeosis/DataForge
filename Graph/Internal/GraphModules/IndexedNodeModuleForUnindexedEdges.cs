using System.Diagnostics.CodeAnalysis;

namespace Graph;

internal sealed class IndexedNodeModuleForUnindexedEdges<TNodeIndex, TNodeData, TEdgeData> :
  GraphModule<TNodeIndex, TNodeData, uint, TEdgeData>,
  IManualSelfIndexedNodeWriteModule<TNodeIndex, TNodeData, TEdgeData>,
  ISelfIndexedNodeReadModule<TNodeIndex, TNodeData, TEdgeData> where TNodeIndex : notnull
{
  public IEnumerable<ISelfIndexedNode<TNodeIndex, TNodeData, TEdgeData>> Nodes => Graph.Nodes;
  IEnumerable<INode<TNodeData, TEdgeData>> INodeReadModule<TNodeData, TEdgeData>.Nodes => Graph.Nodes;

  public IEnumerable<TNodeIndex> NodeIndices => Graph.NodeIndices;

  internal IndexedNodeModuleForUnindexedEdges(InternalGraph<TNodeIndex, TNodeData, uint, TEdgeData> graph) :
    base(graph) { }

  public ISelfIndexedNode<TNodeIndex, TNodeData, TEdgeData> this[TNodeIndex index] => Graph.GetNode(index);

  public ISelfIndexedNode<TNodeIndex, TNodeData, TEdgeData> GetNode(TNodeIndex index) => Graph.GetNode(index);

  public ISelfIndexedNode<TNodeIndex, TNodeData, TEdgeData>? GetNodeOrNull(TNodeIndex index) =>
    Graph.GetNodeOrNull(index);

  public bool TryGetNode(TNodeIndex index,
    [NotNullWhen(true)] out ISelfIndexedNode<TNodeIndex, TNodeData, TEdgeData>? node)
  {
    var result = Graph.TryGetNode(index, out var retrievedNode);
    node = retrievedNode;
    return result;
  }

  public bool TryGetNodeIndex(INode<TNodeData, TEdgeData> node, [NotNullWhen(true)] out TNodeIndex? index) =>
    TryGetNodeIndexInThisGraph(node, out index);

  public bool Contains(TNodeIndex index) => ContainsNode(index);

  public bool ContainsNode(TNodeIndex index) => Graph.ContainsNode(index);

  public bool Contains(INode<TNodeData, TEdgeData> node) =>
    TryGetNodeIndexInThisGraph(node, out var index) && ContainsNode(index);

  public bool RemoveNode(INode<TNodeData, TEdgeData> node) =>
    TryGetNodeIndexInThisGraph(node, out var index) && Graph.RemoveNode(index);

  public bool RemoveNode(TNodeIndex index) => Graph.RemoveNode(index);

  public bool RemoveNode(TNodeIndex index,
    [NotNullWhen(true)] out ISelfIndexedNode<TNodeIndex, TNodeData, TEdgeData>? node)
  {
    var result = Graph.RemoveNode(index, out var removedNode);
    node = removedNode;
    return result;
  }

  public ISelfIndexedNode<TNodeIndex, TNodeData, TEdgeData> AddNode(TNodeIndex index, TNodeData data) =>
    Graph.AddNode(index, data);

  public bool TryAddNode(TNodeIndex index, TNodeData data,
    [NotNullWhen(true)] out ISelfIndexedNode<TNodeIndex, TNodeData, TEdgeData>? node)
  {
    var result = Graph.TryAddNode(index, data, out var createdNode);
    node = createdNode;
    return result;
  }
}