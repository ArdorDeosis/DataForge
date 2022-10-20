using Graph;

namespace GraphCreation;

public static partial class GraphCreator
{
  /// <summary>
  /// Creates a bipartite graph from two sets of node data.
  /// </summary>
  /// <param name="options">Creation options for the graph creation.</param>
  /// <typeparam name="TNodeData">Type of the data the nodes are holding.</typeparam>
  /// <typeparam name="TEdgeData">Type of the data the edges are holding.</typeparam>
  /// <returns>The created graph.</returns>
  /// <remarks>
  /// This is a special form of a multipartite graph (with exactly two sets of nodes). To create a multipartite graph
  /// with more than two sets of nodes, see <see cref="MakeMultipartite{TNodeData,TEdgeData}"/>.
  /// </remarks>
  public static Graph<TNodeData, TEdgeData> MakeBipartite<TNodeData, TEdgeData>(
    BipartiteGraphCreationOptions<TNodeData, TEdgeData> options) =>
    MakeMultipartite(new MultipartiteGraphCreationOptions<TNodeData, TEdgeData>
    {
      CreateEdgeData = options.CreateEdgeData,
      NodeDataSets = new[] { options.NodeDataSetA, options.NodeDataSetB },
      ShouldCreateEdge = options.ShouldCreateEdge,
    });
}