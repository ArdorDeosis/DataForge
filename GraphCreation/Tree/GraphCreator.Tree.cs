using Graph;

namespace GraphCreation;

public static partial class GraphCreator
{
  /// <summary>
  /// Creates a graph with a tree structure. The <paramref name="options"/> define the
  /// <see cref="TreeGraphCreationOptions{TNodeData,TEdgeData}.EdgeDirection">edge direction</see> and an optional
  /// <see cref="TreeGraphCreationOptions{TNodeData,TEdgeData}.MaxDepth">maximum depth</see>. The
  /// <see cref="TreeGraphCreationOptions{TNodeData,TEdgeData}.CreateNodeData"/> and
  /// <see cref="TreeGraphCreationOptions{TNodeData,TEdgeData}.CreateEdgeData"/> functions are used to produce data for
  /// the nodes and edges in the graph depending on their position in the tree and with
  /// <see cref="TreeGraphCreationOptions{TNodeData,TEdgeData}.CalculateChildNodeCount"/> the number of child nodes for
  /// a particular node is calculated.
  /// </summary>
  /// <param name="options">Definition of the tree graph structure.</param>
  /// <typeparam name="TNodeData">Type of the data the nodes are holding.</typeparam>
  /// <typeparam name="TEdgeData">Type of the data the edges are holding.</typeparam>
  /// <returns>The created graph.</returns>
  public static OldGraph<TNodeData, TEdgeData> MakeTree<TNodeData, TEdgeData>(
    TreeGraphCreationOptions<TNodeData, TEdgeData> options) =>
    MakeIndexedTree(options).ToNonIndexedGraph();

  /// <summary>
  /// Creates an indexed graph with a tree structure. Nodes are indexed by their position on the star represented as a
  /// <see cref="TreeIndex"/>. The <paramref name="options"/> define the
  /// <see cref="TreeGraphCreationOptions{TNodeData,TEdgeData}.EdgeDirection">edge direction</see> and an optional
  /// <see cref="TreeGraphCreationOptions{TNodeData,TEdgeData}.MaxDepth">maximum depth</see>. The
  /// <see cref="TreeGraphCreationOptions{TNodeData,TEdgeData}.CreateNodeData"/> and
  /// <see cref="TreeGraphCreationOptions{TNodeData,TEdgeData}.CreateEdgeData"/> functions are used to produce data for
  /// the nodes and edges in the graph depending on their position in the tree and with
  /// <see cref="TreeGraphCreationOptions{TNodeData,TEdgeData}.CalculateChildNodeCount"/> the number of child nodes for
  /// a particular node is calculated.
  /// </summary>
  /// <param name="options">Definition of the tree graph structure.</param>
  /// <typeparam name="TNodeData">Type of the data the nodes are holding.</typeparam>
  /// <typeparam name="TEdgeData">Type of the data the edges are holding.</typeparam>
  /// <returns>The created graph.</returns>
  public static OldIndexedGraph<TreeIndex, TNodeData, TEdgeData> MakeIndexedTree<TNodeData, TEdgeData>(
    TreeGraphCreationOptions<TNodeData, TEdgeData> options)
  {
    var graph = new OldIndexedGraph<TreeIndex, TNodeData, TEdgeData>();

    var queue = new Queue<TreeIndex>();
    queue.Enqueue(new TreeIndex());

    while (queue.Count > 0)
    {
      var index = queue.Dequeue();

      var nodeData = options.CreateNodeData(index);
      graph.AddNode(index, nodeData);

      if (!index.IsRoot)
        graph.AddEdgesForDirection(options.EdgeDirection, index.ParentIndex, index, options.CreateEdgeData);

      EnqueueChildNodes(index, nodeData);
    }

    return graph;

    void EnqueueChildNodes(TreeIndex index, TNodeData data)
    {
      if (options.MaxDepth is not null && index.Depth >= options.MaxDepth)
        return;
      var childNodeCount = options.CalculateChildNodeCount(index, data);
      for (var n = 0; n < childNodeCount; n++)
      {
        var childAddress = new TreeIndex(parentIndex: index, childIndex: n);
        queue.Enqueue(childAddress);
      }
    }
  }
}