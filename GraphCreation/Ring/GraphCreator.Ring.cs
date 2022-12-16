using Graph;

namespace GraphCreation;

public static partial class GraphCreator
{
  /// <summary>
  /// Creates a graph with a ring structure. The options define the
  /// <see cref="RingGraphCreationOptions{TNodeData,TEdgeData}.Size">size</see> and
  /// <see cref="RingGraphCreationOptions{TNodeData,TEdgeData}.EdgeDirection">edge direction</see>. The
  /// <see cref="RingGraphCreationOptions{TNodeData,TEdgeData}.CreateNodeData"/> and
  /// <see cref="RingGraphCreationOptions{TNodeData,TEdgeData}.CreateEdgeData"/> functions are used to produce data for
  /// the nodes and edges in the graph depending on their position in the ring.
  /// </summary>
  /// <param name="options">Definition of the grid structure.</param>
  /// <typeparam name="TNodeData">Type of the data the nodes are holding.</typeparam>
  /// <typeparam name="TEdgeData">Type of the data the edges are holding.</typeparam>
  /// <returns>The created graph.</returns>
  public static OldGraph<TNodeData, TEdgeData> MakeRing<TNodeData, TEdgeData>(
    RingGraphCreationOptions<TNodeData, TEdgeData> options) =>
    MakeIndexedRing(options).ToNonIndexedGraph();

  /// <summary>
  /// Creates a graph with a ring structure. The nodes are indexed with their position in the ring. The options define
  /// the <see cref="RingGraphCreationOptions{TNodeData,TEdgeData}.Size">size</see> and
  /// <see cref="RingGraphCreationOptions{TNodeData,TEdgeData}.EdgeDirection">edge direction</see>. The
  /// <see cref="RingGraphCreationOptions{TNodeData,TEdgeData}.CreateNodeData"/> and
  /// <see cref="RingGraphCreationOptions{TNodeData,TEdgeData}.CreateEdgeData"/> functions are used to produce data for
  /// the nodes and edges in the graph depending on their position in the ring. Nodes are indexed by their position on
  /// the ring from 0 to <see cref="RingGraphCreationOptions{TNodeData,TEdgeData}.Size">
  /// RingGraphCreationOption&lt;TNodeData,TEdgeData&gt;.Size</see> - 1.
  /// </summary>
  /// <param name="options">Definition of the ring structure.</param>
  /// <typeparam name="TNodeData">Type of the data the nodes are holding.</typeparam>
  /// <typeparam name="TEdgeData">Type of the data the edges are holding.</typeparam>
  /// <returns>The created graph.</returns>
  public static NodeIndexedGraph<int, TNodeData, TEdgeData> MakeIndexedRing<TNodeData, TEdgeData>(
    RingGraphCreationOptions<TNodeData, TEdgeData> options)
  {
    return MakeIndexedGrid(new GridGraphCreationOptions<TNodeData, TEdgeData>
    {
      DimensionInformation = new[]
      {
        new GridGraphDimensionInformation
        {
          Length = options.Size,
          Wrap = true,
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