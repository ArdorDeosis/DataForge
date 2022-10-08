using Graph;

namespace GraphCreation;

public static partial class GraphCreator
{
  public static Graph<TNodeData, TEdgeData> MakeTree<TNodeData, TEdgeData>(
    TreeGraphCreationOptions<TNodeData, TEdgeData> options) =>
    MakeIndexedTree(options).ToNonIndexedGraph();

  public static IndexedGraph<TreeIndex, TNodeData, TEdgeData> MakeIndexedTree<TNodeData, TEdgeData>(
    TreeGraphCreationOptions<TNodeData, TEdgeData> options)
  {
    var graph = new IndexedGraph<TreeIndex, TNodeData, TEdgeData>();

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