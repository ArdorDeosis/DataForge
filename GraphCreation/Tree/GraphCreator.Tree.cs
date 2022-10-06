using Graph;
using GridUtilities;

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

      AddEdges(index, nodeData);

      EnqueueChildNodes(index, nodeData);
    }

    return graph;

    void AddEdges(TreeIndex nodeIndex, TNodeData nodeData)
    {
      if (nodeIndex.ParentIndex is null || options.EdgeDirection == EdgeDirection.None)
        return;

      if (options.EdgeDirection.HasFlag(EdgeDirection.Forward))
      {
        graph.AddEdge(nodeIndex.ParentIndex, nodeIndex, options.CreateEdgeData(new EdgeDefinition<TreeIndex, TNodeData>
        {
          OriginAddress = nodeIndex.ParentIndex,
          DestinationAddress = nodeIndex,
          OriginNodeData = graph[nodeIndex.ParentIndex].Data,
          DestinationNodeData = nodeData,
        }));
      }

      if (options.EdgeDirection.HasFlag(EdgeDirection.Backward))
      {
        graph.AddEdge(nodeIndex, nodeIndex.ParentIndex, options.CreateEdgeData(new EdgeDefinition<TreeIndex, TNodeData>
        {
          OriginAddress = nodeIndex,
          DestinationAddress = nodeIndex.ParentIndex,
          OriginNodeData = nodeData,
          DestinationNodeData = graph[nodeIndex.ParentIndex].Data,
        }));
      }
    }

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