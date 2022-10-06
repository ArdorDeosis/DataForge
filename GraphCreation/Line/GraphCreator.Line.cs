using Graph;

namespace GraphCreation;

public static partial class GraphCreator
{
  /// <summary>
  /// Creates a graph with a line structure. The options define the
  /// <see cref="LineGraphCreationOption{TNodeData,TEdgeData}.Length">length</see> and
  /// <see cref="LineGraphCreationOption{TNodeData,TEdgeData}.EdgeDirection">edge direction</see>. The
  /// <see cref="LineGraphCreationOption{TNodeData,TEdgeData}.CreateNodeData"/> and
  /// <see cref="LineGraphCreationOption{TNodeData,TEdgeData}.CreateEdgeData"/> functions are used to produce data for
  /// the nodes and edges in the graph depending on their position in the grid.
  /// </summary>
  /// <param name="options">Definition of the grid structure.</param>
  /// <typeparam name="TNodeData">Type of the data the nodes are holding.</typeparam>
  /// <typeparam name="TEdgeData">Type of the data the edges are holding.</typeparam>
  /// <returns>The created graph.</returns>
  public static Graph<TNodeData, TEdgeData> MakeLine<TNodeData, TEdgeData>(
    LineGraphCreationOption<TNodeData, TEdgeData> options) =>
    MakeIndexedLine(options).ToNonIndexedGraph();

  /// <summary>
  /// Creates a graph with a line structure. The nodes are indexed with their position in the line. The options define
  /// the <see cref="LineGraphCreationOption{TNodeData,TEdgeData}.Length">length</see> and
  /// <see cref="LineGraphCreationOption{TNodeData,TEdgeData}.EdgeDirection">edge direction</see>. The
  /// <see cref="LineGraphCreationOption{TNodeData,TEdgeData}.CreateNodeData"/> and
  /// <see cref="LineGraphCreationOption{TNodeData,TEdgeData}.CreateEdgeData"/> functions are used to produce data for
  /// the nodes and edges in the graph depending on their position in the grid.
  /// </summary>
  /// <param name="options">Definition of the grid structure.</param>
  /// <typeparam name="TNodeData">Type of the data the nodes are holding.</typeparam>
  /// <typeparam name="TEdgeData">Type of the data the edges are holding.</typeparam>
  /// <returns>The created graph.</returns>
  public static IndexedGraph<int, TNodeData, TEdgeData> MakeIndexedLine<TNodeData, TEdgeData>(
    LineGraphCreationOption<TNodeData, TEdgeData> options)
  {
    return MakeIndexedGrid(new GridGraphCreationOption<TNodeData, TEdgeData>
    {
      DimensionInformation = new[]
      {
        new GridGraphDimensionInformation
        {
          Length = options.Length,
          Wrap = false,
          EdgeDirection = options.EdgeDirection,
        },
      },
      CreateNodeData = coordinate =>
        options.CreateNodeData(coordinate[0]),
      CreateEdgeData = gridEdgeData => options.CreateEdgeData(new EdgeDefinition<int, TNodeData>
      {
        OriginIndex = gridEdgeData.OriginIndex[0],
        DestinationIndex = gridEdgeData.DestinationIndex[0],
        OriginNodeData = gridEdgeData.OriginNodeData,
        DestinationNodeData = gridEdgeData.DestinationNodeData,
      }),
    }).Transform(node => node, edge => edge, coordinate => coordinate[0]);
  }
}