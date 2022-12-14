using Graph;
using GridUtilities;

namespace GraphCreation;

public static partial class GraphCreator
{
  /// <summary>
  /// Creates a graph with an n-dimensional grid structure. The number of dimensions is defined by the size of the
  /// option's <see cref="GridGraphCreationOptions{TNodeData,TEdgeData}.DimensionInformation"/>. The
  /// <see cref="GridGraphCreationOptions{TNodeData,TEdgeData}.DimensionInformation"/> also define the
  /// <see cref="GridGraphDimensionInformation.Length">size</see>, <see cref="GridDimensionInformation.Wrap">wrap</see>
  /// and <see cref="GridGraphDimensionInformation.EdgeDirection">edge direction</see> on a per dimension basis. The
  /// <see cref="GridGraphCreationOptions{TNodeData,TEdgeData}.CreateNodeData"/> and
  /// <see cref="GridGraphCreationOptions{TNodeData,TEdgeData}.CreateEdgeData"/> functions are used to produce data for
  /// the nodes and edges in the graph depending on their position in the grid.
  /// </summary>
  /// <param name="options">Definition of the grid structure.</param>
  /// <typeparam name="TNodeData">Type of the data the nodes are holding.</typeparam>
  /// <typeparam name="TEdgeData">Type of the data the edges are holding.</typeparam>
  /// <returns>The created graph.</returns>
  public static UnindexedGraph<TNodeData, TEdgeData> MakeGrid<TNodeData, TEdgeData>(
    GridGraphCreationOptions<TNodeData, TEdgeData> options) =>
    MakeIndexedGrid(options).ToNonIndexedGraph();

  /// <summary>
  /// Creates a graph with an n-dimensional grid structure. The number of dimensions is defined by the size of the
  /// option's <see cref="GridGraphCreationOptions{TNodeData,TEdgeData}.DimensionInformation"/>. Nodes are indexed by
  /// their position in the grid represented as an <see cref="IReadOnlyList{T}">IReadOnlyList&lt;int&gt;</see>. The
  /// <see cref="GridGraphCreationOptions{TNodeData,TEdgeData}.DimensionInformation"/> also define the
  /// <see cref="GridGraphDimensionInformation.Length">size</see>, <see cref="GridDimensionInformation.Wrap">wrap</see>
  /// and <see cref="GridGraphDimensionInformation.EdgeDirection">edge direction</see> on a per dimension basis. The
  /// <see cref="GridGraphCreationOptions{TNodeData,TEdgeData}.CreateNodeData"/> and
  /// <see cref="GridGraphCreationOptions{TNodeData,TEdgeData}.CreateEdgeData"/> functions are used to produce data for
  /// the nodes and edges in the graph depending on their position in the grid.
  /// </summary>
  /// <param name="options">Definition of the grid structure.</param>
  /// <typeparam name="TNodeData">Type of the data the nodes are holding.</typeparam>
  /// <typeparam name="TEdgeData">Type of the data the edges are holding.</typeparam>
  /// <returns>The created graph.</returns>
  /// <remarks>
  /// The resulting graph uses a special <see cref="CoordinateHelpers.EqualityComparer"/> for coordinates.
  /// </remarks>
  public static IndexedGraph<IReadOnlyList<int>, TNodeData, TEdgeData> MakeIndexedGrid<TNodeData, TEdgeData>(
    GridGraphCreationOptions<TNodeData, TEdgeData> options)
  {
    var gridDefinition = options.DimensionInformation
      .Select(info => new GridDimensionInformation(info.Length) { Wrap = info.Wrap })
      .ToArray();
    var graph = new IndexedGraph<IReadOnlyList<int>, TNodeData, TEdgeData>(new CoordinateHelpers.EqualityComparer());
    foreach (var coordinate in Grid.Coordinates(gridDefinition))
      graph.AddNode(coordinate, options.CreateNodeData(coordinate));

    foreach (var info in Grid.EdgeInformation(gridDefinition))
      graph.AddEdgesForDirection(
        options.DimensionInformation[info.DimensionOfChange].EdgeDirection,
        info.LowerCoordinate,
        info.UpperCoordinate,
        options.CreateEdgeData
      );

    return graph;
  }
}