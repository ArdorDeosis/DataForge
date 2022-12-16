using Graph;

namespace GraphCreation;

public static partial class GraphCreator
{
  /// <summary>
  /// Creates an graph with a star structure. The <paramref name="options"/> define the number of
  /// <see cref="StarGraphCreationOptions{TNodeData,TEdgeData}.RayCount">rays</see> the star has and edge direction. With
  /// <see cref="StarGraphCreationOptions{TNodeData,TEdgeData}.CalculateRayLength"/> the length of the rays is
  /// calculated. The <see cref="StarGraphCreationOptions{TNodeData,TEdgeData}.CreateNodeData"/> and
  /// <see cref="StarGraphCreationOptions{TNodeData,TEdgeData}.CreateEdgeData"/> functions are used to produce data for
  /// the nodes and edges in the graph depending on their position on the star.
  /// </summary>
  /// <param name="options">Definition of the star structure.</param>
  /// <typeparam name="TNodeData">Type of the data the nodes are holding.</typeparam>
  /// <typeparam name="TEdgeData">Type of the data the edges are holding.</typeparam>
  /// <returns>The created graph.</returns>
  public static OldGraph<TNodeData, TEdgeData> MakeStar<TNodeData, TEdgeData>(
    StarGraphCreationOptions<TNodeData, TEdgeData> options) =>
    MakeIndexedStar(options).ToNonIndexedGraph();

  /// <summary>
  /// Creates an indexed graph with a star structure. Nodes are indexed by their position on the star represented as a
  /// <see cref="StarIndex"/>. The <paramref name="options"/> define the number of
  /// <see cref="StarGraphCreationOptions{TNodeData,TEdgeData}.RayCount">rays</see> the star has and edge direction. With
  /// <see cref="StarGraphCreationOptions{TNodeData,TEdgeData}.CalculateRayLength"/> the length of the rays is
  /// calculated. The <see cref="StarGraphCreationOptions{TNodeData,TEdgeData}.CreateNodeData"/> and
  /// <see cref="StarGraphCreationOptions{TNodeData,TEdgeData}.CreateEdgeData"/> functions are used to produce data for
  /// the nodes and edges in the graph depending on their position on the star.
  /// </summary>
  /// <param name="options">Definition of the star structure.</param>
  /// <typeparam name="TNodeData">Type of the data the nodes are holding.</typeparam>
  /// <typeparam name="TEdgeData">Type of the data the edges are holding.</typeparam>
  /// <returns>The created graph.</returns>
  public static NodeIndexedGraph<StarIndex, TNodeData, TEdgeData> MakeIndexedStar<TNodeData, TEdgeData>(
    StarGraphCreationOptions<TNodeData, TEdgeData> options)
  {
    var graph = new NodeIndexedGraph<StarIndex, TNodeData, TEdgeData>();
    var centerIndex = new StarIndex();
    graph.AddNode(centerIndex, options.CreateNodeData(centerIndex));
    for (var ray = 0; ray < options.RayCount; ray++)
    {
      var rayLength = options.CalculateRayLength(ray);
      for (var n = 1; n <= rayLength; n++)
      {
        var index = new StarIndex(ray, n);
        graph.AddNode(index, options.CreateNodeData(index));
        graph.AddEdgesForDirection(
          options.EdgeDirection,
          n == 1 ? centerIndex : new StarIndex(ray, n - 1),
          index,
          options.CreateEdgeData
        );
      }
    }

    return graph;
  }
}