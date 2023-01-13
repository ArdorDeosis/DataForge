using System.Diagnostics.CodeAnalysis;

namespace Graph;

/// <inheritdoc cref="GraphBase{TNodeData,TEdgeData}" />
/// <summary>
///   A graph containing nodes and edges.
/// </summary>
public sealed class OldGraph<TNodeData, TEdgeData> : GraphBase<TNodeData, TEdgeData>
{
  private readonly List<OldEdge<,,>> edges = new();
  private readonly List<OldNode<,,>> nodes = new();

  /// <inheritdoc />
  public override IEnumerable<OldNode<,,>> Nodes => nodes;

  /// <inheritdoc />
  public override IEnumerable<OldEdge<,,>> Edges => edges;

  /// <inheritdoc />
  public override int Order => nodes.Count;

  /// <inheritdoc />
  public override int Size => edges.Count;

  /// <inheritdoc />
  public override bool Contains(OldNode<,,> node)
  {
    return nodes.Contains(node);
  }

  /// <inheritdoc />
  public override bool Contains(OldEdge<,,> edge)
  {
    return edges.Contains(edge);
  }

  /// <summary>
  ///   Adds a node to this graph holding the provided data.
  /// </summary>
  /// <param name="data">The data the node will hold.</param>
  /// <returns>The added node.</returns>
  public OldNode<,,> AddNode(TNodeData data)
  {
    var node = MakeNode(data);
    nodes.Add(node);
    return node;
  }

  /// <summary>
  ///   Adds nodes to this graph holding the provided data, one for each element in the <see cref="dataList" /> parameter.
  /// </summary>
  /// <param name="dataList">A list of data to be held by the added nodes.</param>
  /// <returns>A list of the added nodes.</returns>
  [SuppressMessage("ReSharper", "ReturnTypeCanBeEnumerable.Global")]
  public IReadOnlyCollection<OldNode<,,>> AddNodes(IEnumerable<TNodeData> dataList)
  {
    return dataList.Select(AddNode).ToList();
  }

  /// <inheritdoc cref="AddNodes(System.Collections.Generic.IEnumerable{TNodeData})" />
  [SuppressMessage("ReSharper", "ReturnTypeCanBeEnumerable.Global")]
  public IReadOnlyCollection<OldNode<,,>> AddNodes(params TNodeData[] dataList)
  {
    return AddNodes(dataList.AsEnumerable());
  }

  /// <summary>
  ///   Removes the node from this graph. All edges coming from or going to this node are also removed.
  /// </summary>
  /// <param name="node">The node to remove.</param>
  /// <returns>
  ///   Whether the node was removed from this graph or not. A node may not get removed, if it is not part of this graph
  ///   or has been invalidated.
  /// </returns>
  /// <remarks>A successfully removed node and consequentially removed edges are invalidated.</remarks>
  public bool RemoveNode(OldNode<,,> node)
  {
    return RemoveNodeInternal(node);
  }

  /// <summary>
  ///   Removes all nodes from the graph which fulfill the <paramref name="predicate" />. All edges coming from or going to
  ///   a node that is removed are also removed.
  /// </summary>
  /// <param name="predicate">The predicate a node has to fulfill to get removed.</param>
  /// <remarks>Successfully removed nodes and consequentially removed edges are invalidated.</remarks>
  public void RemoveNodes(Func<OldNode<,,>, bool> predicate)
  {
    foreach (var node in nodes.Where(predicate).ToArray())
      RemoveNodeInternal(node);
  }

  /// <summary>
  ///   Adds an edge with the provided data to this graph, connecting the provided nodes.
  /// </summary>
  /// <param name="start">The node the edge goes from.</param>
  /// <param name="end">The node the edge goes to.</param>
  /// <param name="data">The data the edge will hold.</param>
  /// <returns>The added edge.</returns>
  /// <exception cref="ArgumentException">
  ///   If either the <paramref name="start" /> node or the <paramref name="end" /> node are not part of this graph or are
  ///   invalidated.
  /// </exception>
  public OldEdge<,,> AddEdge(OldNode<,,> start, OldNode<,,> end,
    TEdgeData data)
  {
    var edge = MakeEdge(start, end, data);
    edges.Add(edge);
    return edge;
  }

  /// <summary>
  ///   Removes the edge from this graph.
  /// </summary>
  /// <param name="edge">The edge to remove.</param>
  /// <returns>
  ///   Whether the edge was removed from this graph or not. An edge may not get removed, if it is not part of this graph
  ///   or has been invalidated.
  /// </returns>
  /// <remarks>A successfully removed edge is invalidated.</remarks>
  public bool RemoveEdge(OldEdge<,,> edge)
  {
    return RemoveEdgeInternal(edge);
  }

  /// <summary>
  ///   Removes all edges from the graph which fulfill the <paramref name="predicate" />.
  /// </summary>
  /// <param name="predicate">The predicate an edge has to fulfill to get removed.</param>
  /// <remarks>Successfully removed edges are invalidated.</remarks>
  public void RemoveEdges(Func<OldEdge<,,>, bool> predicate)
  {
    foreach (var edge in edges.Where(predicate).ToArray())
      RemoveEdgeInternal(edge);
  }

  /// <summary>
  ///   Creates a copy of this graph.
  /// </summary>
  /// <returns>The copied graph.</returns>
  /// <remarks>
  ///   If the data types <typeparamref name="TNodeData" /> and <typeparamref name="TEdgeData" /> are reference types, the
  ///   copied graph will have references to the same data instances as the original graph. If these should be copied,
  ///   too, use <see cref="Copy(Func{TNodeData, TNodeData}, Func{TEdgeData, TEdgeData})" /> or
  ///   <see cref="Transform{TTransformedNodeData,TTransformedEdgeData}" /> to provide custom copy logic.
  /// </remarks>
  public OldGraph<TNodeData, TEdgeData> Copy()
  {
    var result = new OldGraph<TNodeData, TEdgeData>();
    CopyInternal(this, result);
    return result;
  }

  /// <summary>
  ///   Creates a copy of this graph using the provided custom copy logic for the data held by the nodes and edges.
  /// </summary>
  /// <param name="copyNodeData">Custom copy function for node data.</param>
  /// <param name="copyEdgeData">Custom copy function for edge data.</param>
  /// <returns>The copied graph.</returns>
  public OldGraph<TNodeData, TEdgeData> Copy(Func<TNodeData, TNodeData> copyNodeData,
    Func<TEdgeData, TEdgeData> copyEdgeData)
  {
    return Transform(copyNodeData, copyEdgeData);
  }

  /// <summary>
  ///   Transforms the data on the nodes and edges and creates a new graph with the same graph structure but the
  ///   transformed data.
  /// </summary>
  /// <param name="transformNodeData">Function to transform node data.</param>
  /// <param name="transformEdgeData">Function to transform edge data.</param>
  /// <typeparam name="TTransformedNodeData">The type of the transformed node data.</typeparam>
  /// <typeparam name="TTransformedEdgeData">The type of the transformed edge data.</typeparam>
  /// <returns>The created graph with transformed data.</returns>
  public OldGraph<TTransformedNodeData, TTransformedEdgeData> Transform<TTransformedNodeData, TTransformedEdgeData>(
    Func<TNodeData, TTransformedNodeData> transformNodeData,
    Func<TEdgeData, TTransformedEdgeData> transformEdgeData)
  {
    var result = new OldGraph<TTransformedNodeData, TTransformedEdgeData>();
    CopyTransformInternal(this, result, transformNodeData, transformEdgeData);
    return result;
  }

  /// <summary>
  ///   Merges this graph with other graphs into one new graph containing all nodes and edges from this and the other
  ///   graphs.
  /// </summary>
  /// <param name="others">The graphs to merge this graph with.</param>
  /// <returns>The created merged graph.</returns>
  /// <remarks>
  ///   If the data types <typeparamref name="TNodeData" /> and <typeparamref name="TEdgeData" /> are reference types, the
  ///   merged graph will have references to the same data instances as the original graph. If new instances should be
  ///   created, use <see cref="MergeTransform{TNewNodeData,TNewEdgeData}" /> to provide custom copy logic.
  /// </remarks>
  public OldGraph<TNodeData, TEdgeData> Merge(params GraphBase<TNodeData, TEdgeData>[] others)
  {
    var result = new OldGraph<TNodeData, TEdgeData>();
    CopyInternal(this, result);
    foreach (var graph in others)
      CopyInternal(graph, result);
    return result;
  }

  /// <summary>
  ///   Merges this graph with other graphs into one new graph containing all nodes and edges from this and the other
  ///   graphs with transformed data.
  /// </summary>
  /// <param name="transformNodeData">Function to transform node data.</param>
  /// <param name="transformEdgeData">Function to transform edge data.</param>
  /// <param name="others">The graphs to merge this graph with.</param>
  /// <typeparam name="TTransformedNodeData">The type of the transformed node data.</typeparam>
  /// <typeparam name="TTransformedEdgeData">The type of the transformed edge data.</typeparam>
  /// <returns>The merged graph with transformed data.</returns>
  public OldGraph<TTransformedNodeData, TTransformedEdgeData>
    MergeTransform<TTransformedNodeData, TTransformedEdgeData>(
      Func<TNodeData, TTransformedNodeData> transformNodeData,
      Func<TEdgeData, TTransformedEdgeData> transformEdgeData,
      params GraphBase<TNodeData, TEdgeData>[] others)
  {
    var result = new OldGraph<TTransformedNodeData, TTransformedEdgeData>();
    CopyTransformInternal(this, result, transformNodeData, transformEdgeData);
    foreach (var graph in others)
      CopyTransformInternal(graph, result, transformNodeData, transformEdgeData);
    return result;
  }

  private bool RemoveNodeInternal(OldNode<,,> node)
  {
    RemoveEdges(edge => edge.Contains(node));
    node.Invalidate();
    return nodes.Remove(node);
  }

  private bool RemoveEdgeInternal(OldEdge<,,> edge)
  {
    edge.Start.InternalOutgoingEdgeList.Remove(edge);
    edge.End.InternalIncomingEdgeList.Remove(edge);
    edge.Invalidate();
    return edges.Remove(edge);
  }

  private static void CopyInternal(GraphBase<TNodeData, TEdgeData> source, OldGraph<TNodeData, TEdgeData> target)
  {
    var nodeDictionary = source.Nodes.ToDictionary(node => node, node => target.AddNode(node.Data));
    foreach (var edge in source.Edges)
      target.AddEdge(nodeDictionary[edge.Start], nodeDictionary[edge.End], edge.Data);
  }

  private static void CopyTransformInternal<TNewNodeData, TNewEdgeData>(
    GraphBase<TNodeData, TEdgeData> source,
    OldGraph<TNewNodeData, TNewEdgeData> target,
    Func<TNodeData, TNewNodeData> transformNodeData,
    Func<TEdgeData, TNewEdgeData> transformEdgeData)
  {
    var nodeDictionary = source.Nodes.ToDictionary(node => node, node => target.AddNode(transformNodeData(node.Data)));
    foreach (var edge in source.Edges)
      target.AddEdge(nodeDictionary[edge.Start], nodeDictionary[edge.End], transformEdgeData(edge.Data));
  }
}