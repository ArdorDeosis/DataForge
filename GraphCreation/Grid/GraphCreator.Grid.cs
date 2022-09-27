using Graph;
using GridUtilities;

namespace GraphCreation;

public static partial class GraphCreator
{
  /// <summary>
  /// Creates a graph with a n-dimensional grid structure. The number of dimensions is defined by the size of the
  /// option's <see cref="GridGraphCreationOption{TNodeData,TEdgeData}.DimensionInformation"/>. The
  /// <see cref="GridGraphCreationOption{TNodeData,TEdgeData}.DimensionInformation"/> also define the
  /// <see cref="GridGraphDimensionInformation.Length">size</see> and
  /// <see cref="GridGraphDimensionInformation.EdgeDirection">edge direction</see> on a per dimension basis. The
  /// <see cref="GridGraphCreationOption{TNodeData,TEdgeData}.CreateNodeData"/> and
  /// <see cref="GridGraphCreationOption{TNodeData,TEdgeData}.CreateEdgeData"/> functions are used to produce data for
  /// the nodes and edges in the graph depending on their position in the grid.
  /// </summary>
  /// <param name="options">Definition of the grid structure.</param>
  /// <typeparam name="TNodeData">Type of the data the nodes are holding.</typeparam>
  /// <typeparam name="TEdgeData">Type of the data the edges are holding.</typeparam>
  /// <returns>The created graph.</returns>
  public static Graph<TNodeData, TEdgeData> MakeGrid<TNodeData, TEdgeData>(
    GridGraphCreationOption<TNodeData, TEdgeData> options)
  {
    var gridDefinition = options.DimensionInformation
      .Select(info => new GridDimensionInformation(info.Length))
      .ToArray();
    var graph = new Graph<TNodeData, TEdgeData>();
    var nodes =
      new Dictionary<IReadOnlyList<int>, Node<TNodeData, TEdgeData>>(new CoordinateHelpers.EqualityComparer());
    foreach (var coordinate in Grid.Coordinates(gridDefinition))
      nodes.Add(coordinate, graph.AddNode(options.CreateNodeData(new GridNodeData(coordinate))));

    foreach (var info in Grid.EdgeInformation(gridDefinition))
    {
      var direction = options.DimensionInformation[info.DimensionOfChange].EdgeDirection;
      if (direction == EdgeDirection.None)
        continue;
      var lowerNode = nodes[info.LowerCoordinate];
      var upperNode = nodes[info.UpperCoordinate];

      if (direction.HasFlag(EdgeDirection.Forward))
      {
        graph.AddEdge(
          lowerNode,
          upperNode,
          options.CreateEdgeData(new GridEdgeData<TNodeData, TEdgeData>(
            info.LowerCoordinate,
            info.UpperCoordinate,
            lowerNode,
            upperNode)
          )
        );
      }

      if (direction.HasFlag(EdgeDirection.Backward))
      {
        graph.AddEdge(
          upperNode,
          lowerNode,
          options.CreateEdgeData(new GridEdgeData<TNodeData, TEdgeData>(
            info.UpperCoordinate,
            info.LowerCoordinate,
            upperNode,
            lowerNode)
          )
        );
      }
    }

    return graph;
  }
}