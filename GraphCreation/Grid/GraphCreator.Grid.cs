using Graph;
using GridUtilities;

namespace GraphCreation;

public static partial class GraphCreator
{
  /// <summary>
  /// Creates a graph with an n-dimensional grid structure. The number of dimensions is defined by the size of the
  /// option's <see cref="GridGraphCreationOption{TNodeData,TEdgeData}.DimensionInformation"/>. The
  /// <see cref="GridGraphCreationOption{TNodeData,TEdgeData}.DimensionInformation"/> also define the
  /// <see cref="GridGraphDimensionInformation.Length">size</see>, <see cref="GridDimensionInformation.Wrap">wrap</see>
  /// and <see cref="GridGraphDimensionInformation.EdgeDirection">edge direction</see> on a per dimension basis. The
  /// <see cref="GridGraphCreationOption{TNodeData,TEdgeData}.CreateNodeData"/> and
  /// <see cref="GridGraphCreationOption{TNodeData,TEdgeData}.CreateEdgeData"/> functions are used to produce data for
  /// the nodes and edges in the graph depending on their position in the grid.
  /// </summary>
  /// <param name="options">Definition of the grid structure.</param>
  /// <typeparam name="TNodeData">Type of the data the nodes are holding.</typeparam>
  /// <typeparam name="TEdgeData">Type of the data the edges are holding.</typeparam>
  /// <returns>The created graph.</returns>
  public static Graph<TNodeData, TEdgeData> MakeGrid<TNodeData, TEdgeData>(
    GridGraphCreationOption<TNodeData, TEdgeData> options) =>
    MakeIndexedGrid(options).ToNonIndexedGraph();

  /// <summary>
  /// Creates a graph with an n-dimensional grid structure. The number of dimensions is defined by the size of the
  /// option's <see cref="GridGraphCreationOption{TNodeData,TEdgeData}.DimensionInformation"/>. Nodes are indexed by
  /// their position in the grid represented as an <see cref="IReadOnlyList{T}">IReadOnlyList&lt;int&gt;</see>. The
  /// <see cref="GridGraphCreationOption{TNodeData,TEdgeData}.DimensionInformation"/> also define the
  /// <see cref="GridGraphDimensionInformation.Length">size</see>, <see cref="GridDimensionInformation.Wrap">wrap</see>
  /// and <see cref="GridGraphDimensionInformation.EdgeDirection">edge direction</see> on a per dimension basis. The
  /// <see cref="GridGraphCreationOption{TNodeData,TEdgeData}.CreateNodeData"/> and
  /// <see cref="GridGraphCreationOption{TNodeData,TEdgeData}.CreateEdgeData"/> functions are used to produce data for
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
    GridGraphCreationOption<TNodeData, TEdgeData> options)
  {
    var gridDefinition = options.DimensionInformation
      .Select(info => new GridDimensionInformation(info.Length) { Wrap = info.Wrap })
      .ToArray();
    var graph = new IndexedGraph<IReadOnlyList<int>, TNodeData, TEdgeData>(new CoordinateHelpers.EqualityComparer());
    foreach (var coordinate in Grid.Coordinates(gridDefinition))
      graph.AddNode(coordinate, options.CreateNodeData(coordinate));

    foreach (var info in Grid.EdgeInformation(gridDefinition))
    {
      var direction = options.DimensionInformation[info.DimensionOfChange].EdgeDirection;
      if (direction == EdgeDirection.None)
        continue;
      var lowerNode = graph[info.LowerCoordinate];
      var upperNode = graph[info.UpperCoordinate];

      if (direction.HasFlag(EdgeDirection.Forward))
      {
        graph.AddEdge(
          lowerNode,
          upperNode,
          options.CreateEdgeData(new EdgeDefinition<IReadOnlyList<int>, TNodeData>
          {
            OriginIndex = info.LowerCoordinate,
            DestinationIndex = info.UpperCoordinate,
            OriginNodeData = lowerNode.Data,
            DestinationNodeData = upperNode.Data,
          })
        );
      }

      if (direction.HasFlag(EdgeDirection.Backward))
      {
        graph.AddEdge(
          upperNode,
          lowerNode,
          options.CreateEdgeData(new EdgeDefinition<IReadOnlyList<int>, TNodeData>
          {
            OriginIndex = info.UpperCoordinate,
            DestinationIndex = info.LowerCoordinate,
            OriginNodeData = upperNode.Data,
            DestinationNodeData = lowerNode.Data,
          })
        );
      }
    }

    return graph;
  }
}