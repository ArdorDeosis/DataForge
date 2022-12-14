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
  private readonly Dictionary<TIndex, OldNode<,,>> nodes;
  private readonly List<OldEdge<,,>> edges = new();

  private readonly Func<IEqualityComparer<TIndex>?> equalityComparerFactoryMethod;

  /// <summary>
  /// All node indices in the graph. 
  /// </summary>
  public IEnumerable<TIndex> Indices => nodes.Keys;

  /// <inheritdoc />
  public override IEnumerable<OldNode<,,>> Nodes => nodes.Values;

  /// <inheritdoc />
  public override IEnumerable<OldEdge<,,>> Edges => edges;

  /// <inheritdoc />
  public override int Order => nodes.Count;

  /// <inheritdoc />
  public override int Size => edges.Count;

  /// <param name="equalityComparer">
  /// The <see cref="IEqualityComparer{TIndex}"/> implementation to use when comparing node indices, or null to use the
  /// default <see cref="EqualityComparer{T}"/> for the type of the index.
  /// </param>
  /// <remarks>
  /// Several operations on an <see cref="IndexedGraph{TIndex,TNodeData,TEdgeData}"/> produce a new instance of graph
  /// (e.g. <see cref="Copy()"/>). The <paramref name="equalityComparer"/> will be used for all of these produces
  /// graphs. If a new instance of <see cref="IEqualityComparer{TIndex}"/> should be created, use the constructor with
  /// the factory method parameter
  /// <see cref="IndexedGraph{TIndex,TNodeData,TEdgeData}(System.Func{System.Collections.Generic.IEqualityComparer{TIndex}?})"/>.
  /// </remarks>
  public IndexedGraph(IEqualityComparer<TIndex>? equalityComparer = null)
    : this(() => equalityComparer) { }

  /// <param name="equalityComparerFactoryMethod">
  /// A function to produce a <see cref="IEqualityComparer{TIndex}"/> implementation to use when comparing node indices.
  /// If this function produces <tt>null</tt>, the default <see cref="EqualityComparer{T}"/> for the type of the index
  /// is used.
  /// </param>
  /// <remarks>
  /// Several operations on an <see cref="IndexedGraph{TIndex,TNodeData,TEdgeData}"/> produce a new instance of graph
  /// (e.g. <see cref="Copy()"/>). The <paramref name="equalityComparerFactoryMethod"/> is used in these cases to
  /// produce a new instance of <see cref="IEqualityComparer{TIndex}"/>. If the same instance should be used for the
  /// copy, use the constructor with the instance parameter
  /// <see cref="IndexedGraph{TIndex,TNodeData,TEdgeData}(System.Collections.Generic.IEqualityComparer{TIndex}?)"/>.
  /// </remarks>
  public IndexedGraph(Func<IEqualityComparer<TIndex>?> equalityComparerFactoryMethod)
  {
    this.equalityComparerFactoryMethod = equalityComparerFactoryMethod;
    nodes = new Dictionary<TIndex, OldNode<,,>>(equalityComparerFactoryMethod());
  }

  /// <summary>
  /// Gets the node associated with the specified index.
  /// </summary>
  /// <param name="index">The index of the node to get.</param>
  /// <exception cref="KeyNotFoundException">If no node with the provided key exists in this graph.</exception>
  public OldNode<,,> this[TIndex index] => nodes[index];

  /// <inheritdoc />
  public override bool Contains(OldNode<,,> node) => nodes.ContainsValue(node);

  /// <inheritdoc />
  public override bool Contains(OldEdge<,,> edge) => edges.Contains(edge);

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
  public OldNode<,,> GetNode(TIndex index) => nodes[index];

  /// <summary>
  /// Tries to get the node associated with the specified index.
  /// </summary>
  /// <param name="index">The index of the node to get.</param>
  /// <param name="node">
  /// The node associated with the specified index, if it exists; otherwise <tt>null</tt>. This parameter is passed
  /// uninitialized.
  /// </param>
  /// <returns>Whether this graph contains a node associated with the specified index.</returns>
  public bool TryGetNode(TIndex index, [MaybeNullWhen(false)] out OldNode<,,> node) =>
    nodes.TryGetValue(index, out node);

  /// <summary>
  /// Gets the index of the given node.
  /// </summary>
  /// <param name="node">The node whose index to get.</param>
  /// <returns>The index of the given node.</returns>
  /// <exception cref="InvalidOperationException">If the node is not part of this graph.</exception>
  public TIndex GetIndexOf(OldNode<,,> node) => nodes.First(pair => pair.Value == node).Key;

  /// <summary>
  /// Gets the index of the given node.
  /// </summary>
  /// <param name="node">The node whose index to get.</param>
  /// <param name="index">
  /// The index of the given node, if the node belongs to this graph; otherwise the default value for the type of the
  /// index. This parameter is passed uninitialized.
  /// </param>
  /// <returns>Whether this graph contains the given node.</returns>
  public bool TryGetIndexOf(OldNode<,,> node, out TIndex index)
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
  public OldNode<,,> AddNode(TIndex index, TNodeData data) => AddNodeInternal(index, data);

  /// <summary>
  /// Removes the node from this graph. All edges coming from or going to this node are also removed.
  /// </summary>
  /// <param name="node">The node to remove.</param>
  /// <returns>
  /// Whether the node was removed from this graph or not. A node may not get removed, if it is not part of this graph
  /// or has been invalidated.
  /// </returns>
  /// <remarks>A successfully removed node and consequentially removed edges are invalidated.</remarks>
  public bool RemoveNode(OldNode<,,> node) => node.IsValid && node.IsIn(this) && RemoveNodeInternal(GetIndexOf(node));

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
  public void RemoveNodes(Func<OldNode<,,>, bool> predicate)
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
  public void RemoveNodes(Func<OldNode<,,>, TIndex, bool> predicate)
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
  public OldEdge<,,> AddEdge(TIndex startIndex, TIndex endIndex, TEdgeData data)
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
  public OldEdge<,,> AddEdge(OldNode<,,> start, OldNode<,,> end,
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
  public bool RemoveEdge(OldEdge<,,> edge) => RemoveEdgeInternal(edge);

  /// <summary>
  /// Removes all edges from the graph which fulfill the <paramref name="predicate"/>.
  /// </summary>
  /// <param name="predicate">The predicate an edge has to fulfill to get removed.</param>
  /// <remarks>Successfully removed edges are invalidated.</remarks>
  public void RemoveEdges(Func<OldEdge<,,>, bool> predicate)
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
    var result = new IndexedGraph<TIndex, TNodeData, TEdgeData>(equalityComparerFactoryMethod);
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
    var result = new IndexedGraph<TIndex, TTransformedNodeData, TTransformedEdgeData>(equalityComparerFactoryMethod);
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
  /// <param name="equalityComparer">
  /// The <see cref="IEqualityComparer{TIndex}"/> implementation to use when comparing node indices, or null to use the
  /// default <see cref="EqualityComparer{T}"/> for the transformed type of the index.
  /// </param>
  /// <typeparam name="TTransformedNodeData">The type of the transformed node data.</typeparam>
  /// <typeparam name="TTransformedEdgeData">The type of the transformed edge data.</typeparam>
  /// <typeparam name="TTransformedIndex">The type of the transformed indices.</typeparam>
  /// <returns>The created graph with transformed data and indices.</returns>
  /// <exception cref="InvalidOperationException">
  /// If the index transformation causes a collision of indices in the new graph.
  /// </exception>
  /// <remarks>
  /// Several operations on an <see cref="IndexedGraph{TIndex,TNodeData,TEdgeData}"/> produce a new instance of graph
  /// (e.g. <see cref="Copy()"/>). The <paramref name="equalityComparer"/> will be used for all of these produces
  /// graphs. If a new instance of <see cref="IEqualityComparer{TIndex}"/> should be created, use the overload with the
  /// factory method parameter.
  /// </remarks>
  public IndexedGraph<TTransformedIndex, TTransformedNodeData, TTransformedEdgeData>
    Transform<TTransformedIndex, TTransformedNodeData, TTransformedEdgeData>(
      Func<TNodeData, TTransformedNodeData> transformNodeData, Func<TEdgeData, TTransformedEdgeData> transformEdgeData,
      Func<TIndex, TTransformedIndex> transformIndex, IEqualityComparer<TTransformedIndex>? equalityComparer = null)
    where TTransformedIndex : notnull =>
    Transform(transformNodeData, transformEdgeData, transformIndex, () => equalityComparer);

  /// <summary>
  /// Transforms the data on the nodes and edges and creates a new graph with the same graph structure but the
  /// transformed data.
  /// </summary>
  /// <param name="transformNodeData">Logic to transform node data.</param>
  /// <param name="transformEdgeData">Logic to transform edge data.</param>
  /// <param name="transformIndex">Logic to transform indices.</param>
  /// <param name="equalityComparerFactoryMethod">
  /// A function to produce a <see cref="IEqualityComparer{TIndex}"/> implementation to use when comparing node indices.
  /// If this function produces <tt>null</tt>, the default <see cref="EqualityComparer{T}"/> for the type of the
  /// transformed index is used.
  /// </param>
  /// <typeparam name="TTransformedNodeData">The type of the transformed node data.</typeparam>
  /// <typeparam name="TTransformedEdgeData">The type of the transformed edge data.</typeparam>
  /// <typeparam name="TTransformedIndex">The type of the transformed indices.</typeparam>
  /// <returns>The created graph with transformed data and indices.</returns>
  /// <exception cref="InvalidOperationException">
  /// If the index transformation causes a collision of indices in the new graph.
  /// </exception>
  /// <remarks>
  /// Several operations on an <see cref="IndexedGraph{TIndex,TNodeData,TEdgeData}"/> produce a new instance of graph
  /// (e.g. <see cref="Copy()"/>). The <paramref name="equalityComparerFactoryMethod"/> is used in these cases to
  /// produce a new instance of <see cref="IEqualityComparer{TIndex}"/>. If the same instance should be used for the
  /// copy, use the overload with the instance parameter.
  /// </remarks>
  [SuppressMessage("ReSharper", "ParameterHidesMember")]
  public IndexedGraph<TTransformedIndex, TTransformedNodeData, TTransformedEdgeData>
    Transform<TTransformedIndex, TTransformedNodeData, TTransformedEdgeData>(
      Func<TNodeData, TTransformedNodeData> transformNodeData, Func<TEdgeData, TTransformedEdgeData> transformEdgeData,
      Func<TIndex, TTransformedIndex> transformIndex,
      Func<IEqualityComparer<TTransformedIndex>?> equalityComparerFactoryMethod) where TTransformedIndex : notnull
  {
    var result =
      new IndexedGraph<TTransformedIndex, TTransformedNodeData, TTransformedEdgeData>(equalityComparerFactoryMethod);
    CopyTransformInternal(this, result, transformNodeData, transformEdgeData, transformIndex);
    return result;
  }

  /// <summary>
  /// Converts this graph to a non-indexed <see cref="OldGraph{TNodeData,TEdgeData}"/> with the same graph structure and
  /// data.
  /// </summary>
  /// <remarks>
  /// If the data types <typeparamref name="TNodeData"/> and <typeparamref name="TEdgeData"/> are reference types, the
  /// copied graph will have references to the same data instances as the original graph. If these should be copied,
  /// too, use <see cref="ToNonIndexedGraph(Func{TNodeData, TNodeData}, Func{TEdgeData, TEdgeData})"/> or
  /// <see cref="TransformToNonIndexedGraph{TTransformedNodeData,TTransformedEdgeData}"/> to provide custom copy logic.
  /// </remarks>
  public OldGraph<TNodeData, TEdgeData> ToNonIndexedGraph()
  {
    var result = new OldGraph<TNodeData, TEdgeData>();
    var nodeDictionary = nodes.ToDictionary(entry => entry.Value, entry => result.AddNode(entry.Value.Data));
    foreach (var edge in edges)
      result.AddEdge(nodeDictionary[edge.Start], nodeDictionary[edge.End], edge.Data);
    return result;
  }

  /// <summary>
  /// Converts this graph to a non-indexed <see cref="OldGraph{TNodeData,TEdgeData}"/> with the same graph structure and
  /// data.
  /// </summary>
  /// <param name="copyNodeData">Copy function for node data.</param>
  /// <param name="copyEdgeData">Copy function for edge data.</param>
  public OldGraph<TNodeData, TEdgeData> ToNonIndexedGraph(
    Func<TNodeData, TNodeData> copyNodeData, Func<TEdgeData, TEdgeData> copyEdgeData) =>
    TransformToNonIndexedGraph(copyNodeData, copyEdgeData);


  /// <summary>
  /// Converts this graph to a non-indexed <see cref="OldGraph{TNodeData,TEdgeData}"/> with the same graph structure and
  /// data.
  /// </summary>
  /// <param name="transformNodeData">Transformation function for node data.</param>
  /// <param name="transformEdgeData">Transformation function for edge data.</param>
  /// <typeparam name="TTransformedNodeData">The type of the transformed node data.</typeparam>
  /// <typeparam name="TTransformedEdgeData">The type of the transformed edge data.</typeparam>
  public OldGraph<TTransformedNodeData, TTransformedEdgeData> TransformToNonIndexedGraph<TTransformedNodeData,
    TTransformedEdgeData>(
    Func<TNodeData, TTransformedNodeData> transformNodeData, Func<TEdgeData, TTransformedEdgeData> transformEdgeData)
  {
    var result = new OldGraph<TTransformedNodeData, TTransformedEdgeData>();
    var nodeDictionary =
      nodes.ToDictionary(entry => entry.Value, entry => result.AddNode(transformNodeData(entry.Value.Data)));
    foreach (var edge in edges)
      result.AddEdge(nodeDictionary[edge.Start], nodeDictionary[edge.End], transformEdgeData(edge.Data));
    return result;
  }

  private OldNode<,,> AddNodeInternal(TIndex index, TNodeData data)
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

  private OldEdge<,,> AddEdgeInternal(OldNode<,,> start, OldNode<,,> end,
    TEdgeData data)
  {
    var edge = MakeEdge(start, end, data);
    edges.Add(edge);
    return edge;
  }

  private bool RemoveEdgeInternal(OldEdge<,,> edge)
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