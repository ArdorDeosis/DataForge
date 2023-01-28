using System.Diagnostics.CodeAnalysis;
using DataForge.Graphs;

namespace ObservableGraphs;

public sealed class ObservableGraph<TNodeData, TEdgeData> : IObservableUnindexedGraph<TNodeData, TEdgeData>
{
  private readonly Graph<TNodeData, TEdgeData> graph = new();

  IReadOnlyCollection<INode<TNodeData, TEdgeData>> IReadOnlyGraph<TNodeData, TEdgeData>.Nodes => graph.Nodes;

  IReadOnlyCollection<Edge<TNodeData, TEdgeData>> IReadOnlyUnindexedGraph<TNodeData, TEdgeData>.Edges => graph.Edges;

  IReadOnlyCollection<Node<TNodeData, TEdgeData>> IReadOnlyUnindexedGraph<TNodeData, TEdgeData>.Nodes => graph.Nodes;

  IReadOnlyCollection<IEdge<TNodeData, TEdgeData>> IReadOnlyGraph<TNodeData, TEdgeData>.Edges => graph.Edges;

  public bool Contains(INode<TNodeData, TEdgeData> node) => graph.Contains(node);

  public bool Contains(IEdge<TNodeData, TEdgeData> edge) => graph.Contains(edge);

  public int Order => graph.Order;
  public int Size => graph.Size;

  public bool RemoveNode(INode<TNodeData, TEdgeData> node)
  {
    var result = graph.RemoveNode(node);
    if (result)
      InvokeGraphChanged(GraphChangedEventArgs<TNodeData, TEdgeData>.NodesRemoved((Node<TNodeData, TEdgeData>)node));
    return result;
  }

  public bool RemoveEdge(IEdge<TNodeData, TEdgeData> edge)
  {
    var result = graph.RemoveEdge(edge);
    if (result)
      InvokeGraphChanged(GraphChangedEventArgs<TNodeData, TEdgeData>.EdgesRemoved((Edge<TNodeData, TEdgeData>)edge));
    return result;
  }

  public int RemoveNodesWhere(Predicate<TNodeData> predicate)
  {
    var removedNodes = graph.Nodes
      .Where(node => predicate(node.Data))
      .ToArray()
      .Where(node => graph.RemoveNode(node))
      .ToArray();
    InvokeGraphChanged(GraphChangedEventArgs<TNodeData, TEdgeData>.NodesRemoved(removedNodes));
    return removedNodes.Length;
  }

  public int RemoveEdgesWhere(Predicate<TEdgeData> predicate)
  {
    var removedEdges = graph.Edges
      .Where(edge => predicate(edge.Data))
      .ToArray()
      .Where(edge => graph.RemoveEdge(edge))
      .ToArray();
    InvokeGraphChanged(GraphChangedEventArgs<TNodeData, TEdgeData>.EdgesRemoved(removedEdges));
    return removedEdges.Length;
  }

  public void Clear()
  {
    var eventArgs = new GraphChangedEventArgs<TNodeData, TEdgeData>(
      Array.Empty<Node<TNodeData, TEdgeData>>(),
      graph.Nodes.ToArray(),
      Array.Empty<Edge<TNodeData, TEdgeData>>(),
      graph.Edges.ToArray());
    graph.Clear();
    InvokeGraphChanged(eventArgs);
  }

  public Node<TNodeData, TEdgeData> AddNode(TNodeData data)
  {
    var node = graph.AddNode(data);
    InvokeGraphChanged(GraphChangedEventArgs<TNodeData, TEdgeData>.NodesAdded(node));
    return node;
  }

  public IEnumerable<Node<TNodeData, TEdgeData>> AddNodes(IEnumerable<TNodeData> data)
  {
    var nodes = graph.AddNodes(data).ToArray();
    InvokeGraphChanged(GraphChangedEventArgs<TNodeData, TEdgeData>.NodesAdded(nodes));
    return nodes;
  }

  public Edge<TNodeData, TEdgeData> AddEdge(Node<TNodeData, TEdgeData> origin, Node<TNodeData, TEdgeData> destination,
    TEdgeData data)
  {
    var edge = graph.AddEdge(origin, destination, data);
    InvokeGraphChanged(GraphChangedEventArgs<TNodeData, TEdgeData>.EdgesAdded(edge));
    return edge;
  }

  public bool TryAddEdge(Node<TNodeData, TEdgeData> origin, Node<TNodeData, TEdgeData> destination, TEdgeData data,
    [NotNullWhen(true)] out Edge<TNodeData, TEdgeData>? edge)
  {
    if (!graph.TryAddEdge(origin, destination, data, out edge)) return false;
    InvokeGraphChanged(GraphChangedEventArgs<TNodeData, TEdgeData>.EdgesAdded(edge));
    return true;
  }

  public event EventHandler<GraphChangedEventArgs<TNodeData, TEdgeData>>? GraphChanged;

  private void InvokeGraphChanged(GraphChangedEventArgs<TNodeData, TEdgeData> eventArgs) =>
    GraphChanged?.Invoke(this, eventArgs);
}