using Graph;
using GridUtilities;

namespace GraphCreation;

public static partial class GraphCreator
{
  public static Graph<TNodeData, TEdgeData> MakeTree<TNodeData, TEdgeData>(
    TreeGraphCreationOptions<TNodeData, TEdgeData> options) =>
    MakeIndexedTree(options).ToNonIndexedGraph();

  public static IndexedGraph<IReadOnlyList<int>, TNodeData, TEdgeData> MakeIndexedTree<TNodeData, TEdgeData>(
    TreeGraphCreationOptions<TNodeData, TEdgeData> options)
  {
    var graph = new IndexedGraph<IReadOnlyList<int>, TNodeData, TEdgeData>(new CoordinateHelpers.EqualityComparer());

    var queue = new Queue<IReadOnlyList<int>>();
    queue.Enqueue(new[] { 0 });

    while (queue.Count > 0)
    {
      var address = queue.Dequeue();

      var addressData = new TreeNodeData { Address = address };
      var nodeData = options.CreateNodeData(addressData);
      graph.AddNode(address, nodeData);

      AddEdges(addressData, nodeData);

      EnqueueChildNodes(address, addressData, nodeData);
    }

    return graph;

    void AddEdges(TreeNodeData nodeIndex, TNodeData nodeData)
    {
      if (nodeIndex.ParentAddress is null || options.EdgeDirection == EdgeDirection.None)
        return;

      if (options.EdgeDirection.HasFlag(EdgeDirection.Forward))
      {
        graph.AddEdge(nodeIndex.ParentAddress, nodeIndex.Address, options.CreateEdgeData(new TreeEdgeData<TNodeData>
        {
          OriginAddress = nodeIndex.ParentAddress,
          DestinationAddress = nodeIndex.Address,
          OriginNodeData = graph[nodeIndex.ParentAddress].Data,
          DestinationNodeData = nodeData,
        }));
      }

      if (options.EdgeDirection.HasFlag(EdgeDirection.Backward))
      {
        graph.AddEdge(nodeIndex.Address, nodeIndex.ParentAddress, options.CreateEdgeData(new TreeEdgeData<TNodeData>
        {
          OriginAddress = nodeIndex.Address,
          DestinationAddress = nodeIndex.ParentAddress,
          OriginNodeData = nodeData,
          DestinationNodeData = graph[nodeIndex.ParentAddress].Data,
        }));
      }
    }

    void EnqueueChildNodes(IReadOnlyList<int> address, TreeNodeData addressData, TNodeData nodeData)
    {
      if (options.MaxDepth is not null && address.Count >= options.MaxDepth)
        return;
      var childNodeCount = options.CalculateChildNodeCount(addressData, nodeData);
      for (var n = 0; n < childNodeCount; n++)
      {
        var childAddress = new List<int>(address) { n }.ToArray();
        queue.Enqueue(childAddress);
      }
    }
  }
}