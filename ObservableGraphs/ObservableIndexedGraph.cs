using System.Diagnostics.CodeAnalysis;
using DataForge.Graphs;

namespace ObservableGraphs;

public sealed class ObservableIndexedGraph<TIndex, TNodeData, TEdgeData> : 
  IObservableIndexedGraph<TIndex, TNodeData, TEdgeData>,
  IManuallyIndexedGraph<TIndex, TNodeData, TEdgeData> 
  where TIndex : notnull
{
  private readonly IndexedGraph<TIndex, TNodeData, TEdgeData> graph;
  
  public ObservableIndexedGraph(IEqualityComparer<TIndex>? nodeIndexEqualityComparer = null)
    : this(() => nodeIndexEqualityComparer) { }

  public ObservableIndexedGraph(Func<IEqualityComparer<TIndex>?> nodeIndexEqualityComparerFactoryMethod)
  {
    graph = new IndexedGraph<TIndex, TNodeData, TEdgeData>(nodeIndexEqualityComparerFactoryMethod);
  }

  IReadOnlyCollection<INode<TNodeData, TEdgeData>> IReadOnlyGraph<TNodeData, TEdgeData>.Nodes => graph.Nodes;

  IReadOnlyCollection<IndexedEdge<TIndex, TNodeData, TEdgeData>> IReadOnlyIndexedGraph<TIndex, TNodeData, TEdgeData>.Edges => graph.Edges;

  public IReadOnlyCollection<TIndex> Indices => graph.Indices;

  public IndexedNode<TIndex, TNodeData, TEdgeData> this[TIndex index] => graph[index];

  public IndexedNode<TIndex, TNodeData, TEdgeData> GetNode(TIndex index) => graph.GetNode(index);

  public IndexedNode<TIndex, TNodeData, TEdgeData>? GetNodeOrNull(TIndex index) => graph.GetNodeOrNull(index);

  public bool TryGetNode(TIndex index, [NotNullWhen(true)] out IndexedNode<TIndex, TNodeData, TEdgeData>? node) =>
    graph.TryGetNode(index, out node);

  public bool Contains(TIndex index) => graph.Contains(index);

  IReadOnlyCollection<IndexedNode<TIndex, TNodeData, TEdgeData>> IReadOnlyIndexedGraph<TIndex, TNodeData, TEdgeData>.Nodes => graph.Nodes;

  IReadOnlyCollection<IEdge<TNodeData, TEdgeData>> IReadOnlyGraph<TNodeData, TEdgeData>.Edges => graph.Edges;

  public bool Contains(INode<TNodeData, TEdgeData> node) => graph.Contains(node);

  public bool Contains(IEdge<TNodeData, TEdgeData> edge) => graph.Contains(edge);

  public int Order => graph.Order;
  public int Size => graph.Size;
  public bool RemoveNode(INode<TNodeData, TEdgeData> node)
  {
    var result = (graph as IGraph<TNodeData, TEdgeData>).RemoveNode(node);
    if (result)
      InvokeGraphChanged(IndexedGraphChangedEventArgs<TIndex, TNodeData, TEdgeData>.NodesRemoved((IndexedNode<TIndex, TNodeData, TEdgeData>)node));
    return result;
  }

  public bool RemoveEdge(IEdge<TNodeData, TEdgeData> edge)
  {
    var result = (graph as IGraph<TNodeData, TEdgeData>).RemoveEdge(edge);
    if (result)
      InvokeGraphChanged(IndexedGraphChangedEventArgs<TIndex, TNodeData, TEdgeData>.EdgesRemoved((IndexedEdge<TIndex, TNodeData, TEdgeData>)edge));
    return result;
  }

  public int RemoveNodesWhere(Predicate<TNodeData> predicate)
  {
    var removedNodes = graph.Nodes
      .Where(node => predicate(node.Data))
      .ToArray()
      .Where(node => graph.RemoveNode(node.Index))
      .ToArray();
    InvokeGraphChanged(IndexedGraphChangedEventArgs<TIndex, TNodeData, TEdgeData>.NodesRemoved(removedNodes));
    return removedNodes.Length;
  }

  public int RemoveEdgesWhere(Predicate<TEdgeData> predicate)
  {
    var removedEdges = graph.Edges
      .Where(edge => predicate(edge.Data))
      .ToArray()
      .Where(edge => graph.RemoveEdge(edge))
      .ToArray();
    InvokeGraphChanged(IndexedGraphChangedEventArgs<TIndex, TNodeData, TEdgeData>.EdgesRemoved(removedEdges));
    return removedEdges.Length;
  }

  public void Clear()
  {
    var eventArgs = new IndexedGraphChangedEventArgs<TIndex, TNodeData, TEdgeData>(
      Array.Empty<IndexedNode<TIndex, TNodeData, TEdgeData>>(),
      graph.Nodes.ToArray(),
      Array.Empty<IndexedEdge<TIndex, TNodeData, TEdgeData>>(),
      graph.Edges.ToArray());
    graph.Clear();
    InvokeGraphChanged(eventArgs);
  }

  public bool RemoveNode(IndexedNode<TIndex, TNodeData, TEdgeData> node)
  {
    var result = graph.RemoveNode(node);
    if (result)
      InvokeGraphChanged(IndexedGraphChangedEventArgs<TIndex, TNodeData, TEdgeData>.NodesRemoved(node));
    return result;
  }

  public bool RemoveNode(TIndex index)
  {
    if (!graph.TryGetNode(index, out var node))
      return false;
    var result = graph.RemoveNode(index);
    if (result)
      InvokeGraphChanged(IndexedGraphChangedEventArgs<TIndex, TNodeData, TEdgeData>.NodesRemoved(node));
    return result;
  }

  public bool RemoveNode(TIndex index, [NotNullWhen(true)] out IndexedNode<TIndex, TNodeData, TEdgeData>? node)
  {
    if (!graph.TryGetNode(index, out node))
      return false;
    var result = graph.RemoveNode(index);
    if (result)
      InvokeGraphChanged(IndexedGraphChangedEventArgs<TIndex, TNodeData, TEdgeData>.NodesRemoved(node));
    return result;
  }

  public IndexedEdge<TIndex, TNodeData, TEdgeData> AddEdge(TIndex origin, TIndex destination, TEdgeData data)
  {
    var edge = graph.AddEdge(origin, destination, data);
    InvokeGraphChanged(IndexedGraphChangedEventArgs<TIndex, TNodeData, TEdgeData>.EdgesAdded(edge));
    return edge;
  }

  public bool TryAddEdge(TIndex origin, TIndex destination, TEdgeData data, [NotNullWhen(true)] out IndexedEdge<TIndex, TNodeData, TEdgeData>? edge)
  {
    if (!graph.TryAddEdge(origin, destination, data, out edge)) return false;
    InvokeGraphChanged(IndexedGraphChangedEventArgs<TIndex, TNodeData, TEdgeData>.EdgesAdded(edge));
    return true;
  }

  public bool RemoveEdge(IndexedEdge<TIndex, TNodeData, TEdgeData> edge)
  {
    var result = graph.RemoveEdge(edge);
    if (result)
      InvokeGraphChanged(IndexedGraphChangedEventArgs<TIndex, TNodeData, TEdgeData>.EdgesRemoved(edge));
    return result;
  }

  public IndexedNode<TIndex, TNodeData, TEdgeData> AddNode(TIndex index, TNodeData data)
  {
    var node = graph.AddNode(index, data);
    InvokeGraphChanged(IndexedGraphChangedEventArgs<TIndex, TNodeData, TEdgeData>.NodesAdded(node));
    return node;
  }

  public bool TryAddNode(TIndex index, TNodeData data, [NotNullWhen(true)] out IndexedNode<TIndex, TNodeData, TEdgeData>? node)
  {
    if (!graph.TryAddNode(index, data, out node)) 
      return false;
    InvokeGraphChanged(IndexedGraphChangedEventArgs<TIndex, TNodeData, TEdgeData>.NodesAdded(node));
    return true;
  }

  public event EventHandler<IndexedGraphChangedEventArgs<TIndex, TNodeData, TEdgeData>>? GraphChanged;


  private void InvokeGraphChanged(IndexedGraphChangedEventArgs<TIndex, TNodeData, TEdgeData> eventArgs) => 
    GraphChanged?.Invoke(this, eventArgs);
}