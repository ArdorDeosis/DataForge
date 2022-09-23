namespace Graph;

public sealed class Graph<TNodeData, TEdgeData> : GraphBase<TNodeData, TEdgeData>
{
  private readonly List<Node<TNodeData, TEdgeData>> nodes = new();
  private readonly List<Edge<TNodeData, TEdgeData>> edges = new();

  public override bool Contains(Node<TNodeData, TEdgeData> node) => nodes.Contains(node);

  public override bool Contains(Edge<TNodeData, TEdgeData> edge) => edges.Contains(edge);

  public override IEnumerable<Node<TNodeData, TEdgeData>> Nodes => nodes;
  public override IEnumerable<Edge<TNodeData, TEdgeData>> Edges => edges;

  public Node<TNodeData, TEdgeData> AddNode(TNodeData data)
  {
    var node = MakeNode(data);
    nodes.Add(node);
    return node;
  }

  public IEnumerable<Node<TNodeData, TEdgeData>> AddNodes(params TNodeData[] dataList) =>
    AddNodes(dataList.AsEnumerable());

  public IEnumerable<Node<TNodeData, TEdgeData>> AddNodes(IEnumerable<TNodeData> dataList) => dataList.Select(AddNode);

  public bool RemoveNode(Node<TNodeData, TEdgeData> node) => RemoveNodeInternal(node);

  public void RemoveNodes(Func<Node<TNodeData, TEdgeData>, bool> predicate)
  {
    for (var index = nodes.Count - 1; index >= 0; --index)
    {
      if (predicate(nodes[index]))
        RemoveNodeInternal(nodes[index]);
    }
  }

  public Edge<TNodeData, TEdgeData> AddEdge(Node<TNodeData, TEdgeData> start, Node<TNodeData, TEdgeData> end,
    TEdgeData data)
  {
    var edge = MakeEdge(start, end, data);
    edges.Add(edge);
    return edge;
  }

  public bool RemoveEdge(Edge<TNodeData, TEdgeData> edge) => RemoveEdgeInternal(edge);

  public void RemoveEdges(Func<Edge<TNodeData, TEdgeData>, bool> predicate)
  {
    for (var index = edges.Count - 1; index >= 0; --index)
    {
      if (predicate(edges[index]))
        RemoveEdgeInternal(edges[index]);
    }
  }

  public Graph<TNodeData, TEdgeData> Copy()
  {
    var result = new Graph<TNodeData, TEdgeData>();
    CopyInternal(this, result);
    return result;
  }

  public Graph<TNodeData, TEdgeData> Copy(Func<TNodeData, TNodeData> copyNodeData,
    Func<TEdgeData, TEdgeData> copyEdgeData) =>
    Transform(copyNodeData, copyEdgeData);

  public Graph<TNewNodeData, TNewEdgeData> Transform<TNewNodeData, TNewEdgeData>(
    Func<TNodeData, TNewNodeData> transformNodeData, Func<TEdgeData, TNewEdgeData> transformEdgeData)
  {
    var result = new Graph<TNewNodeData, TNewEdgeData>();
    CopyTransformInternal(this, result, transformNodeData, transformEdgeData);
    return result;
  }

  public Graph<TNodeData, TEdgeData> Merge(params GraphBase<TNodeData, TEdgeData>[] others)
  {
    var result = new Graph<TNodeData, TEdgeData>();
    foreach (var graph in others)
      CopyInternal(graph, result);
    return result;
  }

  public Graph<TNewNodeData, TNewEdgeData> MergeTransform<TNewNodeData, TNewEdgeData>(
    Func<TNodeData, TNewNodeData> transformNodeData, Func<TEdgeData, TNewEdgeData> transformEdgeData,
    params GraphBase<TNodeData, TEdgeData>[] others)
  {
    var result = new Graph<TNewNodeData, TNewEdgeData>();
    foreach (var graph in others)
      CopyTransformInternal(graph, result, transformNodeData, transformEdgeData);
    return result;
  }

  private bool RemoveNodeInternal(Node<TNodeData, TEdgeData> node)
  {
    RemoveEdges(edge => edge.Contains(node));
    node.Invalidate();
    return nodes.Remove(node);
  }

  private bool RemoveEdgeInternal(Edge<TNodeData, TEdgeData> edge)
  {
    edge.Start.InternalOutgoingEdgeList.Remove(edge);
    edge.End.InternalIncomingEdgeList.Remove(edge);
    edge.Invalidate();
    return edges.Remove(edge);
  }

  private static void CopyInternal(GraphBase<TNodeData, TEdgeData> source, Graph<TNodeData, TEdgeData> target)
  {
    var nodeDictionary = source.Nodes.ToDictionary(node => node, node => target.AddNode(node.Data));
    foreach (var edge in source.Edges)
      target.AddEdge(nodeDictionary[edge.Start], nodeDictionary[edge.End], edge.Data);
  }

  public static void CopyTransformInternal<TNewNodeData, TNewEdgeData>(GraphBase<TNodeData, TEdgeData> source,
    Graph<TNewNodeData, TNewEdgeData> target,
    Func<TNodeData, TNewNodeData> transformNodeData, Func<TEdgeData, TNewEdgeData> transformEdgeData)
  {
    var nodeDictionary = source.Nodes.ToDictionary(node => node, node => target.AddNode(transformNodeData(node.Data)));
    foreach (var edge in source.Edges)
      target.AddEdge(nodeDictionary[edge.Start], nodeDictionary[edge.End], transformEdgeData(edge.Data));
  }
}