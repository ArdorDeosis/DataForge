using System.Diagnostics.CodeAnalysis;

namespace Graph;

public class IndexedGraph<TIndex, TNodeData, TEdgeData> : GraphBase<TNodeData, TEdgeData> where TIndex : notnull
{
  private readonly Dictionary<TIndex, Node<TNodeData, TEdgeData>> nodes = new();
  private readonly List<Edge<TNodeData, TEdgeData>> edges = new();

  public IEnumerable<TIndex> Indices => nodes.Keys;
  public override IEnumerable<Node<TNodeData, TEdgeData>> Nodes => nodes.Values;
  public override IEnumerable<Edge<TNodeData, TEdgeData>> Edges => edges;

  public Node<TNodeData, TEdgeData> this[TIndex index] => nodes[index];
  public Node<TNodeData, TEdgeData> GetNode(TIndex index) => nodes[index];

  public bool TryGetNode(TIndex index, [MaybeNullWhen(false)] out Node<TNodeData, TEdgeData> node) =>
    nodes.TryGetValue(index, out node);

  public override bool Contains(Node<TNodeData, TEdgeData> node) => nodes.ContainsValue(node);
  public override bool Contains(Edge<TNodeData, TEdgeData> edge) => edges.Contains(edge);
  public bool Contains(TIndex index) => nodes.ContainsKey(index);

  public TIndex GetIndexOf(Node<TNodeData, TEdgeData> node) => nodes.First(pair => pair.Value == node).Key;

  public bool TryGetIndexOf(Node<TNodeData, TEdgeData> node, out TIndex index)
  {
    if (nodes.ContainsValue(node))
    {
      index = nodes.First(pair => pair.Value == node).Key;
      return true;
    }

    index = default!;
    return false;
  }

  public Node<TNodeData, TEdgeData> AddNode(TIndex index, TNodeData data) => AddNodeInternal(index, data);

  // NOTE: why no AddRange functions? because a single add can fail, but the already inserted are inserted
  // an operation should always work in ful or not change the graph.
  // TODO: I could check every key for validity, every data retrieval method for validity etc.


  public bool RemoveNode(Node<TNodeData, TEdgeData> node) =>
    node.IsValid && node.IsIn(this) && RemoveNodeInternal(GetIndexOf(node));

  public bool RemoveNode(TIndex index) => RemoveNodeInternal(index);

  public void RemoveNodes(Func<Node<TNodeData, TEdgeData>, bool> predicate)
  {
    var indicesToRemove = nodes.Where(pair => predicate(pair.Value)).Select(pair => pair.Key).ToArray();
    foreach (var index in indicesToRemove)
      RemoveNodeInternal(index);
  }

  // TODO: bulk delete nodes by index?
  // TODO: I could check every key for validity, every data retrieval method for validity etc.


  public Edge<TNodeData, TEdgeData> AddEdge(TIndex startIndex, TIndex endIndex, TEdgeData data)
  {
    if (!TryGetNode(startIndex, out var startNode))
      throw new ArgumentOutOfRangeException(nameof(startIndex));
    if (!TryGetNode(endIndex, out var endNode))
      throw new ArgumentOutOfRangeException(nameof(endIndex));
    return AddEdgeInternal(startNode, endNode, data);
  }

  public Edge<TNodeData, TEdgeData> AddEdge(Node<TNodeData, TEdgeData> start, Node<TNodeData, TEdgeData> end,
    TEdgeData data) =>
    AddEdgeInternal(start, end, data);


  public bool RemoveEdge(Edge<TNodeData, TEdgeData> edge) => RemoveEdgeInternal(edge);

  public void RemoveEdges(Func<Edge<TNodeData, TEdgeData>, bool> predicate)
  {
    for (var index = edges.Count - 1; index >= 0; --index)
    {
      if (predicate(edges[index]))
        RemoveEdgeInternal(edges[index]);
    }
  }

  public IndexedGraph<TIndex, TNodeData, TEdgeData> Copy()
  {
    var result = new IndexedGraph<TIndex, TNodeData, TEdgeData>();
    var nodeDictionary =
      nodes.Keys.ToDictionary(index => nodes[index], index => result.AddNode(index, nodes[index].Data));
    foreach (var edge in edges)
      result.AddEdge(nodeDictionary[edge.Start], nodeDictionary[edge.End], edge.Data);
    return result;
  }

  public IndexedGraph<TIndex, TNodeData, TEdgeData> Copy(Func<TNodeData, TNodeData> copyNodeData,
    Func<TEdgeData, TEdgeData> copyEdgeData) =>
    Transform(copyNodeData, copyEdgeData);

  public IndexedGraph<TIndex, TNewNodeData, TNewEdgeData> Transform<TNewNodeData, TNewEdgeData>(
    Func<TNodeData, TNewNodeData> transformNodeData, Func<TEdgeData, TNewEdgeData> transformEdgeData)
  {
    var result = new IndexedGraph<TIndex, TNewNodeData, TNewEdgeData>();
    var nodeDictionary = nodes.Keys.ToDictionary(index => nodes[index],
      index => result.AddNode(index, transformNodeData(nodes[index].Data)));
    foreach (var edge in edges)
      result.AddEdge(nodeDictionary[edge.Start], nodeDictionary[edge.End], transformEdgeData(edge.Data));
    return result;
  }

  public Graph<TNodeData, TEdgeData> ToArbitraryGraph()
  {
    var result = new Graph<TNodeData, TEdgeData>();
    var nodeDictionary = nodes.ToDictionary(entry => entry.Value, entry => result.AddNode(entry.Value.Data));
    foreach (var edge in edges)
      result.AddEdge(nodeDictionary[edge.Start], nodeDictionary[edge.End], edge.Data);
    return result;
  }

  public Graph<TNodeData, TEdgeData> ToArbitraryGraph(
    Func<TNodeData, TNodeData> copyNodeData, Func<TEdgeData, TEdgeData> copyEdgeData) =>
    TransformToArbitraryGraph(copyNodeData, copyEdgeData);
  

  public Graph<TNewNodeData, TNewEdgeData> TransformToArbitraryGraph<TNewNodeData, TNewEdgeData>(
    Func<TNodeData, TNewNodeData> transformNodeData, Func<TEdgeData, TNewEdgeData> transformEdgeData)
  {
    var result = new Graph<TNewNodeData, TNewEdgeData>();
    var nodeDictionary =
      nodes.ToDictionary(entry => entry.Value, entry => result.AddNode(transformNodeData(entry.Value.Data)));
    foreach (var edge in edges)
      result.AddEdge(nodeDictionary[edge.Start], nodeDictionary[edge.End], transformEdgeData(edge.Data));
    return result;
  }

  private Node<TNodeData, TEdgeData> AddNodeInternal(TIndex index, TNodeData data)
  {
    if (nodes.ContainsKey(index))
      throw new InvalidOperationException($"Node with index {index} already exists.");
    var node = MakeNode(data);
    nodes.Add(index, node);
    return node;
  }

  private bool RemoveNodeInternal(TIndex index)
  {
    if (!nodes.TryGetValue(index, out var node))
      return false;
    RemoveEdges(edge => edge.Contains(node));
    nodes.Remove(index);
    node.Invalidate();
    return true;
  }

  private Edge<TNodeData, TEdgeData> AddEdgeInternal(Node<TNodeData, TEdgeData> start, Node<TNodeData, TEdgeData> end,
    TEdgeData data)
  {
    var edge = MakeEdge(start, end, data);
    edges.Add(edge);
    return edge;
  }

  private bool RemoveEdgeInternal(Edge<TNodeData, TEdgeData> edge)
  {
    edge.Start.InternalOutgoingEdgeList.Remove(edge);
    edge.End.InternalIncomingEdgeList.Remove(edge);
    edge.Invalidate();
    return edges.Remove(edge);
  }
}