using DataForge.Graphs;

namespace GraphCreation;

public static partial class GraphCreator
{
  /// <summary>
  /// Creates a graph with a line structure. The options define the
  /// <see cref="LineGraphCreationOptions{TNodeData,TEdgeData}.Length">length</see> and
  /// <see cref="LineGraphCreationOptions{TNodeData,TEdgeData}.EdgeDirection">edge direction</see>. The
  /// <see cref="LineGraphCreationOptions{TNodeData,TEdgeData}.CreateNodeData"/> and
  /// <see cref="LineGraphCreationOptions{TNodeData,TEdgeData}.CreateEdgeData"/> functions are used to produce data for
  /// the nodes and edges in the graph depending on their position on the line.
  /// </summary>
  /// <param name="options">Definition of the grid structure.</param>
  /// <typeparam name="TNodeData">Type of the data the nodes are holding.</typeparam>
  /// <typeparam name="TEdgeData">Type of the data the edges are holding.</typeparam>
  /// <returns>The created graph.</returns>
  public static Graph<TNodeData, TEdgeData> MakeLine<TNodeData, TEdgeData>(
    LineGraphCreationOptions<TNodeData, TEdgeData> options) =>
    MakeIndexedLine(options).ToUnindexedGraph();

  /// <summary>
  /// Creates a graph with a line structure. The nodes are indexed with their position in the line. The options define
  /// the <see cref="LineGraphCreationOptions{TNodeData,TEdgeData}.Length">length</see> and
  /// <see cref="LineGraphCreationOptions{TNodeData,TEdgeData}.EdgeDirection">edge direction</see>. The
  /// <see cref="LineGraphCreationOptions{TNodeData,TEdgeData}.CreateNodeData"/> and
  /// <see cref="LineGraphCreationOptions{TNodeData,TEdgeData}.CreateEdgeData"/> functions are used to produce data for
  /// the nodes and edges in the graph depending on their position on the line. Nodes are indexed by their position on
  /// the line from 0 to <see cref="LineGraphCreationOptions{TNodeData,TEdgeData}.Length">
  /// LineGraphCreationOption&lt;TNodeData,TEdgeData&gt;.Length</see> - 1.
  /// </summary>
  /// <param name="options">Definition of the line graph structure.</param>
  /// <typeparam name="TNodeData">Type of the data the nodes are holding.</typeparam>
  /// <typeparam name="TEdgeData">Type of the data the edges are holding.</typeparam>
  /// <returns>The created graph.</returns>
  public static IndexedGraph<int, TNodeData, TEdgeData> MakeIndexedLine<TNodeData, TEdgeData>(
    LineGraphCreationOptions<TNodeData, TEdgeData> options)
  {
    return MakeIndexedGrid(new GridGraphCreationOptions<TNodeData, TEdgeData>
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
      CreateEdgeData = gridEdgeData => options.CreateEdgeData(new IndexedGraphEdgeDataCreationInput<int, TNodeData>
      {
        StartIndex = gridEdgeData.StartIndex[0],
        EndIndex = gridEdgeData.EndIndex[0],
        StartNodeData = gridEdgeData.StartNodeData,
        EndNodeData = gridEdgeData.EndNodeData,
      }),
    }).Transform(node => node, edge => edge, coordinate => coordinate[0]);
  }
}