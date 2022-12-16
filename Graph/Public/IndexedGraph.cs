using System.Diagnostics.CodeAnalysis;

namespace Graph;

public sealed class IndexedGraph<TNodeIndex, TNodeData, TEdgeData> : IIndexedGraph<TNodeIndex, TNodeData, TEdgeData>
  where TNodeIndex : notnull
{
  private readonly InternalGraph<TNodeIndex, TNodeData, uint, TEdgeData> graph;
  private readonly IndexedNodeModuleForUnindexedEdges<TNodeIndex, TNodeData, TEdgeData> nodeModule;
  private readonly UnindexedEdgeModuleForIndexedNodes<TNodeIndex, TNodeData, TEdgeData> edgeModule;

  public IndexedGraph()
  {
    graph = new InternalGraph<TNodeIndex, TNodeData, uint, TEdgeData>();
    nodeModule = new IndexedNodeModuleForUnindexedEdges<TNodeIndex, TNodeData, TEdgeData>(graph);
    edgeModule = new UnindexedEdgeModuleForIndexedNodes<TNodeIndex, TNodeData, TEdgeData>(graph);
  }

  IEnumerable<INode<TNodeData, TEdgeData>> INodeReader<TNodeData, TEdgeData>.Nodes => nodes;

  IEnumerable<ISelfIndexedNode<TNodeIndex, TNodeData, TEdgeData>>
    INodeIndexedNodeReader<TNodeIndex, TNodeData, TEdgeData>.Nodes =>
    nodes1;

  public IEnumerable<TNodeIndex> NodeIndices { get; }

  public ISelfIndexedNode<TNodeIndex, TNodeData, TEdgeData> this[TNodeIndex index] =>
    throw new NotImplementedException();

  public ISelfIndexedNode<TNodeIndex, TNodeData, TEdgeData> GetNode(TNodeIndex index) =>
    throw new NotImplementedException();

  public ISelfIndexedNode<TNodeIndex, TNodeData, TEdgeData>? GetNodeOrNull(TNodeIndex index) =>
    throw new NotImplementedException();

  public bool TryGetNode(TNodeIndex index, out ISelfIndexedNode<TNodeIndex, TNodeData, TEdgeData>? node) =>
    throw new NotImplementedException();

  public bool TryGetNodeIndex(INode<TNodeData, TEdgeData> node, out TNodeIndex? index) =>
    throw new NotImplementedException();

  public bool Contains(TNodeIndex index) => throw new NotImplementedException();

  public bool ContainsNode(TNodeIndex index) => throw new NotImplementedException();

  public bool Contains(INode<TNodeData, TEdgeData> node) => throw new NotImplementedException();

  public IEnumerable<IEdge<TNodeData, TEdgeData>> Edges { get; }
  public bool Contains(IEdge<TNodeData, TEdgeData> edge) => throw new NotImplementedException();

  public int Order { get; }
  public int Size { get; }
  public bool RemoveNode(INode<TNodeData, TEdgeData> node) => throw new NotImplementedException();

  public bool RemoveEdge(IEdge<TNodeData, TEdgeData> edge) => throw new NotImplementedException();

  public bool RemoveNode(TNodeIndex index) => throw new NotImplementedException();

  public bool RemoveNode(TNodeIndex index, out ISelfIndexedNode<TNodeIndex, TNodeData, TEdgeData>? node) =>
    throw new NotImplementedException();

  public IEdge<TNodeData, TEdgeData> AddEdge(INode<TNodeData, TEdgeData> origin,
    INode<TNodeData, TEdgeData> destination, TEdgeData data) =>
    throw new NotImplementedException();
}