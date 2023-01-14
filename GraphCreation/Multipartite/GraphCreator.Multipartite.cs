namespace GraphCreation;

public static partial class GraphCreator
{
  /// <summary>
  /// Creates a multipartite graph from two sets of node data.
  /// </summary>
  /// <param name="options">Creation options for the graph creation.</param>
  /// <typeparam name="TNodeData">Type of the data the nodes are holding.</typeparam>
  /// <typeparam name="TEdgeData">Type of the data the edges are holding.</typeparam>
  /// <returns>The created graph.</returns>
  public static OldGraph<TNodeData, TEdgeData> MakeMultipartite<TNodeData, TEdgeData>(
    MultipartiteGraphCreationOptions<TNodeData, TEdgeData> options)
  {
    var graph = new OldGraph<TNodeData, TEdgeData>();

    var nodeSets = options.NodeDataSets
      .Select(dataSet => graph.AddNodes(dataSet))
      .ToArray();

    for (var lowerSet = 0; lowerSet < nodeSets.Length; lowerSet++)
    {
      for (var upperSet = lowerSet + 1; upperSet < nodeSets.Length; upperSet++)
      {
        foreach (var lowerNode in nodeSets[lowerSet])
        {
          foreach (var upperNode in nodeSets[upperSet])
          {
            if (options.ShouldCreateEdge(lowerNode.Data, upperNode.Data, EdgeDirection.Forward))
              graph.AddEdge(lowerNode, upperNode, options.CreateEdgeData(lowerNode.Data, upperNode.Data));
            if (options.ShouldCreateEdge(upperNode.Data, lowerNode.Data, EdgeDirection.Backward))
              graph.AddEdge(upperNode, lowerNode, options.CreateEdgeData(upperNode.Data, lowerNode.Data));
          }
        }
      }
    }

    return graph;
  }
}