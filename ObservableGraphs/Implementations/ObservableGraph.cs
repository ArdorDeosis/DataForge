using System.Diagnostics.CodeAnalysis;

namespace DataForge.Graphs.Observable;

public sealed class ObservableGraph<TNodeData, TEdgeData> : IObservableUnindexedGraph<TNodeData, TEdgeData>
{
  private readonly Graph<TNodeData, TEdgeData> graph;

  public ObservableGraph()
  {
    graph = new Graph<TNodeData, TEdgeData>();
  }

  internal ObservableGraph(Graph<TNodeData, TEdgeData> graph)
  {
    this.graph = graph;
  }

  public IReadOnlyCollection<Node<TNodeData, TEdgeData>> Nodes => graph.Nodes;

  public IReadOnlyCollection<Edge<TNodeData, TEdgeData>> Edges => graph.Edges;

  public event EventHandler<GraphChangedEventArgs<TNodeData, TEdgeData>>? GraphChanged;
  
  private event EventHandler<IGraphChangedEventArgs<TNodeData, TEdgeData>>? GraphChangedInterfaceImplementation;

  IReadOnlyCollection<INode<TNodeData, TEdgeData>> IReadOnlyGraph<TNodeData, TEdgeData>.Nodes => graph.Nodes;

  IReadOnlyCollection<INode<TNodeData, TEdgeData>> IReadOnlyUnindexedGraph<TNodeData, TEdgeData>.Nodes => Nodes;

  public bool Contains(INode<TNodeData, TEdgeData> node) => graph.Contains(node);

  IReadOnlyCollection<IEdge<TNodeData, TEdgeData>> IReadOnlyUnindexedGraph<TNodeData, TEdgeData>.Edges => Edges;

  IReadOnlyCollection<IEdge<TNodeData, TEdgeData>> IReadOnlyGraph<TNodeData, TEdgeData>.Edges => graph.Edges;

  public bool Contains(IEdge<TNodeData, TEdgeData> edge) => graph.Contains(edge);

  public int Order => graph.Order;

  public int Size => graph.Size;

  INode<TNodeData, TEdgeData> IUnindexedGraph<TNodeData, TEdgeData>.AddNode(TNodeData data) => AddNode(data);

  public bool RemoveNode(INode<TNodeData, TEdgeData> node)
  {
    if (node is not Node<TNodeData, TEdgeData> { IsValid: true } typedNode)
      return false;
    var adjacentEdges = graph.Edges.Where(edge =>
        edge.Origin == node ||
        edge.Destination == node)
      .ToArray();
    var result = graph.RemoveNode(node);
    if (result)
      InvokeGraphChanged(new GraphChangedEventArgs<TNodeData, TEdgeData>
      {
        RemovedEdges = adjacentEdges,
        RemovedNodes = new[] { typedNode },
      });
    return result;
  }

  public int RemoveNodesWhere(Predicate<TNodeData> predicate)
  {
    var edges = graph.Edges.ToArray();
    var removedNodes = graph.Nodes
      .Where(node => predicate(node.Data))
      .ToArray()
      .Where(node => graph.RemoveNode(node))
      .ToArray();
    InvokeGraphChanged(new GraphChangedEventArgs<TNodeData, TEdgeData>
    {
      RemovedNodes = removedNodes,
      RemovedEdges = edges.Where(edge => !edge.IsValid).ToArray(),
    });
    return removedNodes.Length;
  }

  IEdge<TNodeData, TEdgeData> IUnindexedGraph<TNodeData, TEdgeData>.AddEdge(INode<TNodeData, TEdgeData> origin,
    INode<TNodeData, TEdgeData> destination, TEdgeData data)
  {
    if (origin is not Node<TNodeData, TEdgeData> typedOrigin)
      throw new ArgumentException("The origin node is not part of this graph.");
    if (destination is not Node<TNodeData, TEdgeData> typedDestination)
      throw new ArgumentException("The destination node is not part of this graph.");
    return AddEdge(typedOrigin, typedDestination, data);
  }

  public bool RemoveEdge(IEdge<TNodeData, TEdgeData> edge)
  {
    if (edge is not Edge<TNodeData, TEdgeData> { IsValid: true } typedEdge)
      return false;
    var result = graph.RemoveEdge(typedEdge);
    if (result)
      InvokeGraphChanged(new GraphChangedEventArgs<TNodeData, TEdgeData> { RemovedEdges = new[] { typedEdge } });
    return result;
  }

  public void ClearEdges()
  {
    var eventArgs = new GraphChangedEventArgs<TNodeData, TEdgeData> { RemovedEdges = graph.Edges };
    graph.ClearEdges();
    InvokeGraphChanged(eventArgs);
  }

  public int RemoveEdgesWhere(Predicate<TEdgeData> predicate)
  {
    var removedEdges = graph.Edges
      .Where(edge => predicate(edge.Data))
      .ToArray()
      .Where(edge => graph.RemoveEdge(edge))
      .ToArray();
    InvokeGraphChanged(new GraphChangedEventArgs<TNodeData, TEdgeData> { RemovedEdges = removedEdges });
    return removedEdges.Length;
  }

  public void Clear()
  {
    var eventArgs = new GraphChangedEventArgs<TNodeData, TEdgeData>
    {
      RemovedNodes = graph.Nodes.ToArray(),
      RemovedEdges = graph.Edges.ToArray(),
    };
    graph.Clear();
    InvokeGraphChanged(eventArgs);
  }

  public Node<TNodeData, TEdgeData> AddNode(TNodeData data)
  {
    var node = graph.AddNode(data);
    InvokeGraphChanged(new GraphChangedEventArgs<TNodeData, TEdgeData> { AddedNodes = new[] { node } });
    return node;
  }

  public IEnumerable<Node<TNodeData, TEdgeData>> AddNodes(IEnumerable<TNodeData> data)
  {
    var nodes = graph.AddNodes(data).ToArray();
    InvokeGraphChanged(new GraphChangedEventArgs<TNodeData, TEdgeData> { AddedNodes = nodes });
    return nodes;
  }

  public IEnumerable<Node<TNodeData, TEdgeData>> AddNodes(params TNodeData[] data) => AddNodes(data.AsEnumerable());

  public Edge<TNodeData, TEdgeData> AddEdge(Node<TNodeData, TEdgeData> origin, Node<TNodeData, TEdgeData> destination,
    TEdgeData data)
  {
    var edge = graph.AddEdge(origin, destination, data);
    InvokeGraphChanged(new GraphChangedEventArgs<TNodeData, TEdgeData> { AddedEdges = new[] { edge } });
    return edge;
  }

  public bool TryAddEdge(Node<TNodeData, TEdgeData> origin, Node<TNodeData, TEdgeData> destination, TEdgeData data,
    [NotNullWhen(true)] out Edge<TNodeData, TEdgeData>? edge)
  {
    if (!graph.TryAddEdge(origin, destination, data, out edge)) return false;
    InvokeGraphChanged(new GraphChangedEventArgs<TNodeData, TEdgeData> { AddedEdges = new[] { edge } });
    return true;
  }

  public ObservableGraph<TNodeData, TEdgeData> Clone() => new(graph.Clone());

  public ObservableGraph<TNodeData, TEdgeData> Clone(Func<TNodeData, TNodeData> cloneNodeData,
    Func<TEdgeData, TEdgeData> cloneEdgeData) => new(graph.Clone(cloneNodeData, cloneEdgeData));

  public ObservableGraph<TNodeDataTransformed, TEdgeDataTransformed>
    Transform<TNodeDataTransformed, TEdgeDataTransformed>(
      Func<TNodeData, TNodeDataTransformed> nodeDataTransformation,
      Func<TEdgeData, TEdgeDataTransformed> edgeDataTransformation
    ) =>
    new(graph.Transform(nodeDataTransformation,
      edgeDataTransformation));

  public ObservableIndexedGraph<TIndex, TNodeData, TEdgeData> ToIndexedGraph<TIndex>(
    Func<TNodeData, TIndex> indexGeneratorFunction,
    IEqualityComparer<TIndex>? indexEqualityComparer = null)
    where TIndex : notnull =>
    new(graph.ToIndexedGraph(indexGeneratorFunction,
      indexEqualityComparer));

  public ObservableIndexedGraph<TIndex, TNodeData, TEdgeData> ToIndexedGraph<TIndex>(
    IIndexProvider<TNodeData, TIndex> indexProvider,
    IEqualityComparer<TIndex>? indexEqualityComparer = null)
    where TIndex : notnull =>
    new(graph.ToIndexedGraph(indexProvider,
      indexEqualityComparer));

  public ObservableIndexedGraph<TIndex, TNodeData, TEdgeData> ToIndexedGraph<TIndex>(
    Func<TNodeData, TIndex> indexGeneratorFunction,
    Func<IEqualityComparer<TIndex>?> indexEqualityComparerFactoryMethod) where TIndex : notnull =>
    new(graph.ToIndexedGraph(indexGeneratorFunction,
      indexEqualityComparerFactoryMethod));

  public ObservableIndexedGraph<TIndex, TNodeData, TEdgeData> ToIndexedGraph<TIndex>(
    IIndexProvider<TNodeData, TIndex> indexProvider,
    Func<IEqualityComparer<TIndex>?> indexEqualityComparerFactoryMethod) where TIndex : notnull =>
    new(graph.ToIndexedGraph(indexProvider,
      indexEqualityComparerFactoryMethod));

  public ObservableIndexedGraph<TIndex, TNodeData, TEdgeData> ToIndexedGraph<TIndex>(
    Func<TNodeData, TIndex> indexGeneratorFunction,
    Func<TNodeData, TNodeData> cloneNodeData,
    Func<TEdgeData, TEdgeData> cloneEdgeData,
    IEqualityComparer<TIndex>? indexEqualityComparer = null) where TIndex : notnull =>
    new(graph.ToIndexedGraph(indexGeneratorFunction, cloneNodeData,
      cloneEdgeData, indexEqualityComparer));

  public ObservableIndexedGraph<TIndex, TNodeData, TEdgeData> ToIndexedGraph<TIndex>(
    IIndexProvider<TNodeData, TIndex> indexProvider,
    Func<TNodeData, TNodeData> cloneNodeData,
    Func<TEdgeData, TEdgeData> cloneEdgeData,
    IEqualityComparer<TIndex>? indexEqualityComparer = null) where TIndex : notnull =>
    new(graph.ToIndexedGraph(indexProvider, cloneNodeData,
      cloneEdgeData, indexEqualityComparer));

  public ObservableIndexedGraph<TIndex, TNodeData, TEdgeData> ToIndexedGraph<TIndex>(
    Func<TNodeData, TIndex> indexGeneratorFunction,
    Func<TNodeData, TNodeData> cloneNodeData,
    Func<TEdgeData, TEdgeData> cloneEdgeData,
    Func<IEqualityComparer<TIndex>?> indexEqualityComparerFactoryMethod) where TIndex : notnull =>
    new(graph.ToIndexedGraph(indexGeneratorFunction, cloneNodeData,
      cloneEdgeData,
      indexEqualityComparerFactoryMethod));

  public ObservableIndexedGraph<TIndex, TNodeData, TEdgeData> ToIndexedGraph<TIndex>(
    IIndexProvider<TNodeData, TIndex> indexProvider,
    Func<TNodeData, TNodeData> cloneNodeData,
    Func<TEdgeData, TEdgeData> cloneEdgeData,
    Func<IEqualityComparer<TIndex>?> indexEqualityComparerFactoryMethod) where TIndex : notnull =>
    new(graph.ToIndexedGraph(indexProvider, cloneNodeData,
      cloneEdgeData, indexEqualityComparerFactoryMethod));

  public ObservableIndexedGraph<TIndex, TNodeDataTransformed, TEdgeDataTransformed>
    TransformToIndexedGraph<TIndex, TNodeDataTransformed, TEdgeDataTransformed>(
      Func<TNodeData, TIndex> indexGeneratorFunction,
      Func<TNodeData, TNodeDataTransformed> nodeDataTransformation,
      Func<TEdgeData, TEdgeDataTransformed> edgeDataTransformation,
      IEqualityComparer<TIndex>? indexEqualityComparer = null) where TIndex : notnull =>
    new(graph.TransformToIndexedGraph(
      indexGeneratorFunction,
      nodeDataTransformation,
      edgeDataTransformation,
      indexEqualityComparer));

  public ObservableIndexedGraph<TIndex, TNodeDataTransformed, TEdgeDataTransformed>
    TransformToIndexedGraph<TIndex, TNodeDataTransformed, TEdgeDataTransformed>(
      IIndexProvider<TNodeData, TIndex> indexProvider,
      Func<TNodeData, TNodeDataTransformed> nodeDataTransformation,
      Func<TEdgeData, TEdgeDataTransformed> edgeDataTransformation,
      IEqualityComparer<TIndex>? indexEqualityComparer = null) where TIndex : notnull =>
    new(
      graph.TransformToIndexedGraph(
        indexProvider,
        nodeDataTransformation,
        edgeDataTransformation,
        indexEqualityComparer));

  public ObservableIndexedGraph<TIndex, TNodeDataTransformed, TEdgeDataTransformed>
    TransformToIndexedGraph<TIndex, TNodeDataTransformed, TEdgeDataTransformed>(
      Func<TNodeData, TIndex> indexGeneratorFunction,
      Func<TNodeData, TNodeDataTransformed> nodeDataTransformation,
      Func<TEdgeData, TEdgeDataTransformed> edgeDataTransformation,
      Func<IEqualityComparer<TIndex>?> indexEqualityComparerFactoryMethod) where TIndex : notnull =>
    new(graph.TransformToIndexedGraph(
      indexGeneratorFunction,
      nodeDataTransformation,
      edgeDataTransformation,
      indexEqualityComparerFactoryMethod));

  public ObservableIndexedGraph<TIndex, TNodeDataTransformed, TEdgeDataTransformed>
    TransformToIndexedGraph<TIndex, TNodeDataTransformed, TEdgeDataTransformed>(
      IIndexProvider<TNodeData, TIndex> indexProvider,
      Func<TNodeData, TNodeDataTransformed> nodeDataTransformation,
      Func<TEdgeData, TEdgeDataTransformed> edgeDataTransformation,
      Func<IEqualityComparer<TIndex>?> indexEqualityComparerFactoryMethod) where TIndex : notnull =>
    new(graph.TransformToIndexedGraph(
      indexProvider,
      nodeDataTransformation,
      edgeDataTransformation,
      indexEqualityComparerFactoryMethod));

  private void InvokeGraphChanged(GraphChangedEventArgs<TNodeData, TEdgeData> eventArgs)
  {
    GraphChanged?.Invoke(this, eventArgs);
    GraphChangedInterfaceImplementation?.Invoke(this, eventArgs);
  }

  event EventHandler<IGraphChangedEventArgs<TNodeData, TEdgeData>>? IObservableGraph<TNodeData, TEdgeData>.GraphChanged
  {
    add => GraphChangedInterfaceImplementation += value;
    remove => GraphChangedInterfaceImplementation -= value;
  }
}