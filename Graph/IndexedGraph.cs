using System.Diagnostics.CodeAnalysis;

namespace Graph;

/// <summary>
/// A graph containing nodes with unique indices and edges.
/// </summary>
/// <typeparam name="TIndex">Type of the node indices.</typeparam>
/// <typeparam name="TNodeData">Type of the data the nodes are holding.</typeparam>
/// <typeparam name="TEdgeData">Type of the data the edges are holding.</typeparam>
public class IndexedGraph<TIndex, TNodeData, TEdgeData> : GraphBase<TNodeData, TEdgeData> where TIndex : notnull
{
  private readonly Dictionary<TIndex, Node<TNodeData, TEdgeData>> nodes = new();
  private readonly List<Edge<TNodeData, TEdgeData>> edges = new();

  /// <summary>
  /// All node indices in the graph. 
  /// </summary>
  public IEnumerable<TIndex> Indices => nodes.Keys;

  /// <inheritdoc />
  public override IEnumerable<Node<TNodeData, TEdgeData>> Nodes => nodes.Values;

  /// <inheritdoc />
  public override IEnumerable<Edge<TNodeData, TEdgeData>> Edges => edges;

  /// <summary>
  /// Gets the node associated with the specified index.
  /// </summary>
  /// <param name="index">The index of the node to get.</param>
  /// <exception cref="KeyNotFoundException">If no node with the provided key exists in this graph.</exception>
  public Node<TNodeData, TEdgeData> this[TIndex index] => nodes[index];

  /// <inheritdoc />
  public override bool Contains(Node<TNodeData, TEdgeData> node) => nodes.ContainsValue(node);

  /// <inheritdoc />
  public override bool Contains(Edge<TNodeData, TEdgeData> edge) => edges.Contains(edge);

  /// <summary>
  /// Whether this graph contains a node with the provided index.
  /// </summary>
  /// <param name="index">The index to check.</param>
  public bool Contains(TIndex index) => nodes.ContainsKey(index);

  /// <summary>
  /// Gets the node associated with the specified index.
  /// </summary>
  /// <param name="index">The index of the node to get.</param>
  /// <exception cref="KeyNotFoundException">If no node with the provided index exists in this graph.</exception>
  public Node<TNodeData, TEdgeData> GetNode(TIndex index) => nodes[index];

  /// <summary>
  /// Tries to get the node associated with the specified index.
  /// </summary>
  /// <param name="index">The index of the node to get.</param>
  /// <param name="node">
  /// The node associated with the specified index, if it exists; otherwise <tt>null</tt>. This parameter is passed
  /// uninitialized.
  /// </param>
  /// <returns>Whether this graph contains a node associated with the specified index.</returns>
  public bool TryGetNode(TIndex index, [MaybeNullWhen(false)] out Node<TNodeData, TEdgeData> node) =>
    nodes.TryGetValue(index, out node);

  /// <summary>
  /// Gets the index of the given node.
  /// </summary>
  /// <param name="node">The node whose index to get.</param>
  /// <returns>The index of the given node.</returns>
  /// <exception cref="InvalidOperationException">If the node is not part of this graph.</exception>
  public TIndex GetIndexOf(Node<TNodeData, TEdgeData> node) => nodes.First(pair => pair.Value == node).Key;

  /// <summary>
  /// Gets the index of the given node.
  /// </summary>
  /// <param name="node">The node whose index to get.</param>
  /// <param name="index">
  /// The index of the given node, if the node belongs to this graph; otherwise the default value for the type of the
  /// index. This parameter is passed uninitialized.
  /// </param>
  /// <returns>Whether this graph contains the given node.</returns>
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

  /// <summary>
  /// Adds a node with the provided index and data to this graph.
  /// </summary>
  /// <param name="index">The index of the node.</param>
  /// <param name="data">The data the node will hold.</param>
  /// <returns>The added node.</returns>
  /// <exception cref="InvalidOperationException">
  /// If a node with the same index already exists in this graph.
  /// </exception>
  public Node<TNodeData, TEdgeData> AddNode(TIndex index, TNodeData data) => AddNodeInternal(index, data);

  /// <summary>
  /// Removes the node from this graph. All edges coming from or going to this node are also removed.
  /// </summary>
  /// <param name="node">The node to remove.</param>
  /// <returns>
  /// Whether the node was removed from this graph or not. A node may not get removed, if it is not part of this graph
  /// or has been invalidated.
  /// </returns>
  /// <remarks>A successfully removed node and consequentially removed edges are invalidated.</remarks>
  public bool RemoveNode(Node<TNodeData, TEdgeData> node) =>
    node.IsValid && node.IsIn(this) && RemoveNodeInternal(GetIndexOf(node));

  /// <summary>
  /// Removes the node with the provided index from this graph. All edges coming from or going to this node are also
  /// removed.
  /// </summary>
  /// <param name="index">The index of the node to remove.</param>
  /// <returns>
  /// Whether the node was removed from this graph or not. A node may not get removed, if it is not part of this graph
  /// or has been invalidated.
  /// </returns>
  /// <remarks>A successfully removed node and consequentially removed edges are invalidated.</remarks>
  public bool RemoveNode(TIndex index) => RemoveNodeInternal(index);

  /// <summary>
  /// Removes all nodes from the graph which fulfill the <paramref name="predicate"/>. All edges connected to a node
  /// that is removed are also removed.
  /// </summary>
  /// <param name="predicate">The predicate a node has to fulfill to get removed.</param>
  /// <remarks>Successfully removed nodes and consequentially removed edges are invalidated.</remarks>
  public void RemoveNodes(Func<Node<TNodeData, TEdgeData>, bool> predicate)
  {
    var indicesToRemove = nodes
      .Where(pair => predicate(pair.Value))
      .Select(pair => pair.Key)
      .ToArray();
    foreach (var index in indicesToRemove)
      RemoveNodeInternal(index);
  }

  /// <summary>
  /// Removes all nodes from the graph which fulfill the <paramref name="predicate"/>. All edges connected to a node
  /// that is removed are also removed.
  /// </summary>
  /// <param name="predicate">The predicate a node has to fulfill to get removed.</param>
  /// <remarks>Successfully removed nodes and consequentially removed edges are invalidated.</remarks>
  public void RemoveNodes(Func<Node<TNodeData, TEdgeData>, TIndex, bool> predicate)
  {
    var indicesToRemove = nodes
      .Where(pair => predicate(pair.Value, pair.Key))
      .Select(pair => pair.Key)
      .ToArray();
    foreach (var index in indicesToRemove)
      RemoveNodeInternal(index);
  }

  /// <summary>
  /// Adds an edge with the provided data to this graph, connecting the nodes with the provided indices.
  /// </summary>
  /// <param name="startIndex">The index of the node the edge goes from.</param>
  /// <param name="endIndex">The index of the node the edge goes to.</param>
  /// <param name="data">The data the edge will hold.</param>
  /// <returns>The added edge.</returns>
  /// <exception cref="KeyNotFoundException">
  /// If either the <paramref name="startIndex"/> or the <paramref name="endIndex"/> have no node associated.
  /// </exception>
  public Edge<TNodeData, TEdgeData> AddEdge(TIndex startIndex, TIndex endIndex, TEdgeData data)
  {
    if (!TryGetNode(startIndex, out var startNode))
      throw new KeyNotFoundException(nameof(startIndex));
    if (!TryGetNode(endIndex, out var endNode))
      throw new KeyNotFoundException(nameof(endIndex));
    return AddEdgeInternal(startNode, endNode, data);
  }

  /// <summary>
  /// Adds an edge with the provided data to this graph, connecting the provided nodes.
  /// </summary>
  /// <param name="start">The node the edge goes from.</param>
  /// <param name="end">The node the edge goes to.</param>
  /// <param name="data">The data the edge will hold.</param>
  /// <returns>The added edge.</returns>
  /// <exception cref="ArgumentException">
  /// If either the <paramref name="start"/> node or the <paramref name="end"/> node are not part of this graph or are
  /// invalidated.
  /// </exception>
  public Edge<TNodeData, TEdgeData> AddEdge(Node<TNodeData, TEdgeData> start, Node<TNodeData, TEdgeData> end,
    TEdgeData data) =>
    AddEdgeInternal(start, end, data);

  /// <summary>
  /// Removes the edge from this graph.
  /// </summary>
  /// <param name="edge">The edge to remove.</param>
  /// <returns>
  /// Whether the edge was removed from this graph or not. An edge may not get removed, if it is not part of this graph
  /// or has been invalidated.
  /// </returns>
  /// <remarks>A successfully removed edge is invalidated.</remarks>
  public bool RemoveEdge(Edge<TNodeData, TEdgeData> edge) => RemoveEdgeInternal(edge);

  /// <summary>
  /// Removes all edges from the graph which fulfill the <paramref name="predicate"/>.
  /// </summary>
  /// <param name="predicate">The predicate an edge has to fulfill to get removed.</param>
  /// <remarks>Successfully removed edges are invalidated.</remarks>
  public void RemoveEdges(Func<Edge<TNodeData, TEdgeData>, bool> predicate)
  {
    for (var index = edges.Count - 1; index >= 0; --index)
    {
      if (predicate(edges[index]))
        RemoveEdgeInternal(edges[index]);
    }
  }

  /// <summary>
  /// Creates a copy of this graph.
  /// </summary>
  /// <returns>The copied graph.</returns>
  /// <remarks>
  /// If the data types <typeparamref name="TNodeData"/> and <typeparamref name="TEdgeData"/> are reference types, the
  /// copied graph will have references to the same data instances as the original graph. If these should be copied,
  /// too, use <see cref="Copy(Func{TNodeData, TNodeData}, Func{TEdgeData, TEdgeData})"/> or
  /// <see cref="Transform{TTransformedNodeData,TTransformedEdgeData}"/> to provide custom copy logic.
  /// </remarks>
  public IndexedGraph<TIndex, TNodeData, TEdgeData> Copy()
  {
    var result = new IndexedGraph<TIndex, TNodeData, TEdgeData>();
    CopyInternal(this, result);
    return result;
  }

  /// <summary>
  /// Creates a copy of this graph using the provided custom copy logic for the data held by the nodes and edges.
  /// </summary>
  /// <param name="copyNodeData">Copy function for node data.</param>
  /// <param name="copyEdgeData">Copy function for edge data.</param>
  /// <returns>The copied graph.</returns>
  public IndexedGraph<TIndex, TNodeData, TEdgeData> Copy(Func<TNodeData, TNodeData> copyNodeData,
    Func<TEdgeData, TEdgeData> copyEdgeData) =>
    Transform(copyNodeData, copyEdgeData);

  /// <summary>
  /// Transforms the data on the nodes and edges and creates a new graph with the same graph structure but the
  /// transformed data.
  /// </summary>
  /// <param name="transformNodeData">Transformation function for node data.</param>
  /// <param name="transformEdgeData">Transformation function for edge data.</param>
  /// <typeparam name="TTransformedNodeData">The type of the transformed node data.</typeparam>
  /// <typeparam name="TTransformedEdgeData">The type of the transformed edge data.</typeparam>
  /// <returns>The created graph with transformed data.</returns>
  public IndexedGraph<TIndex, TTransformedNodeData, TTransformedEdgeData> Transform<TTransformedNodeData,
    TTransformedEdgeData>(
    Func<TNodeData, TTransformedNodeData> transformNodeData, Func<TEdgeData, TTransformedEdgeData> transformEdgeData)
  {
    var result = new IndexedGraph<TIndex, TTransformedNodeData, TTransformedEdgeData>();
    CopyTransformInternal(this, result, transformNodeData, transformEdgeData, index => index);
    return result;
  }

  /// <summary>
  /// Transforms the data on the nodes and edges and creates a new graph with the same graph structure but the
  /// transformed data.
  /// </summary>
  /// <param name="transformNodeData">Logic to transform node data.</param>
  /// <param name="transformEdgeData">Logic to transform edge data.</param>
  /// <param name="transformIndex">Logic to transform indices.</param>
  /// <typeparam name="TTransformedNodeData">The type of the transformed node data.</typeparam>
  /// <typeparam name="TTransformedEdgeData">The type of the transformed edge data.</typeparam>
  /// <typeparam name="TTransformedIndex">The type of the transformed indices.</typeparam>
  /// <returns>The created graph with transformed data and indices.</returns>
  /// <exception cref="InvalidOperationException">
  /// If the index transformation causes a collision of indices in the new graph.
  /// </exception>
  public IndexedGraph<TTransformedIndex, TTransformedNodeData, TTransformedEdgeData>
    Transform<TTransformedIndex, TTransformedNodeData, TTransformedEdgeData>(
      Func<TNodeData, TTransformedNodeData> transformNodeData, Func<TEdgeData, TTransformedEdgeData> transformEdgeData,
      Func<TIndex, TTransformedIndex> transformIndex) where TTransformedIndex : notnull
  {
    var result = new IndexedGraph<TTransformedIndex, TTransformedNodeData, TTransformedEdgeData>();
    CopyTransformInternal(this, result, transformNodeData, transformEdgeData, transformIndex);
    return result;
  }

  /// <summary>
  /// Converts this graph to a non-indexed <see cref="Graph{TNodeData,TEdgeData}"/> with the same graph structure and
  /// data.
  /// </summary>
  /// <remarks>
  /// If the data types <typeparamref name="TNodeData"/> and <typeparamref name="TEdgeData"/> are reference types, the
  /// copied graph will have references to the same data instances as the original graph. If these should be copied,
  /// too, use <see cref="ToNonIndexedGraph(Func{TNodeData, TNodeData}, Func{TEdgeData, TEdgeData})"/> or
  /// <see cref="TransformToNonIndexedGraph{TTransformedNodeData,TTransformedEdgeData}"/> to provide custom copy logic.
  /// </remarks>
  public Graph<TNodeData, TEdgeData> ToNonIndexedGraph()
  {
    var result = new Graph<TNodeData, TEdgeData>();
    var nodeDictionary = nodes.ToDictionary(entry => entry.Value, entry => result.AddNode(entry.Value.Data));
    foreach (var edge in edges)
      result.AddEdge(nodeDictionary[edge.Start], nodeDictionary[edge.End], edge.Data);
    return result;
  }

  /// <summary>
  /// Converts this graph to a non-indexed <see cref="Graph{TNodeData,TEdgeData}"/> with the same graph structure and
  /// data.
  /// </summary>
  /// <param name="copyNodeData">Copy function for node data.</param>
  /// <param name="copyEdgeData">Copy function for edge data.</param>
  public Graph<TNodeData, TEdgeData> ToNonIndexedGraph(
    Func<TNodeData, TNodeData> copyNodeData, Func<TEdgeData, TEdgeData> copyEdgeData) =>
    TransformToNonIndexedGraph(copyNodeData, copyEdgeData);


  /// <summary>
  /// Converts this graph to a non-indexed <see cref="Graph{TNodeData,TEdgeData}"/> with the same graph structure and
  /// data.
  /// </summary>
  /// <param name="transformNodeData">Transformation function for node data.</param>
  /// <param name="transformEdgeData">Transformation function for edge data.</param>
  /// <typeparam name="TTransformedNodeData">The type of the transformed node data.</typeparam>
  /// <typeparam name="TTransformedEdgeData">The type of the transformed edge data.</typeparam>
  public Graph<TTransformedNodeData, TTransformedEdgeData> TransformToNonIndexedGraph<TTransformedNodeData,
    TTransformedEdgeData>(
    Func<TNodeData, TTransformedNodeData> transformNodeData, Func<TEdgeData, TTransformedEdgeData> transformEdgeData)
  {
    var result = new Graph<TTransformedNodeData, TTransformedEdgeData>();
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

  private static void CopyInternal(IndexedGraph<TIndex, TNodeData, TEdgeData> source,
    IndexedGraph<TIndex, TNodeData, TEdgeData> target)
  {
    var nodeDictionary =
      source.nodes.Keys.ToDictionary(index => source.nodes[index],
        index => target.AddNode(index, source.nodes[index].Data));
    foreach (var edge in source.edges)
      target.AddEdge(nodeDictionary[edge.Start], nodeDictionary[edge.End], edge.Data);
  }

  private static void CopyTransformInternal<TTransformedIndex, TTransformedNodeData, TTransformedEdgeData>(
    IndexedGraph<TIndex, TNodeData, TEdgeData> source,
    IndexedGraph<TTransformedIndex, TTransformedNodeData, TTransformedEdgeData> target,
    Func<TNodeData, TTransformedNodeData> transformNodeData, Func<TEdgeData, TTransformedEdgeData> transformEdgeData,
    Func<TIndex, TTransformedIndex> transformIndex)
    where TTransformedIndex : notnull
  {
    var nodeDictionary = source.nodes.Keys.ToDictionary(index => source.nodes[index],
      index => target.AddNode(transformIndex(index), transformNodeData(source.nodes[index].Data)));
    foreach (var edge in source.edges)
      target.AddEdge(nodeDictionary[edge.Start], nodeDictionary[edge.End], transformEdgeData(edge.Data));
  }
}