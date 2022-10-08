using Graph;

namespace GraphCreation;

public static partial class GraphCreator
{
  /// <summary>
  /// Creates a graph with a ring structure. The options define the
  /// <see cref="RingGraphCreationOption{TNodeData,TEdgeData}.Size">size</see> and
  /// <see cref="RingGraphCreationOption{TNodeData,TEdgeData}.EdgeDirection">edge direction</see>. The
  /// <see cref="RingGraphCreationOption{TNodeData,TEdgeData}.CreateNodeData"/> and
  /// <see cref="RingGraphCreationOption{TNodeData,TEdgeData}.CreateEdgeData"/> functions are used to produce data for
  /// the nodes and edges in the graph depending on their position in the ring.
  /// </summary>
  /// <param name="options">Definition of the grid structure.</param>
  /// <typeparam name="TNodeData">Type of the data the nodes are holding.</typeparam>
  /// <typeparam name="TEdgeData">Type of the data the edges are holding.</typeparam>
  /// <returns>The created graph.</returns>
  public static Graph<TNodeData, TEdgeData> MakeRing<TNodeData, TEdgeData>(
    RingGraphCreationOption<TNodeData, TEdgeData> options) =>
    MakeIndexedRing(options).ToNonIndexedGraph();

  /// <summary>
  /// Creates a graph with a ring structure. The nodes are indexed with their position in the ring. The options define
  /// the <see cref="RingGraphCreationOption{TNodeData,TEdgeData}.Size">size</see> and
  /// <see cref="RingGraphCreationOption{TNodeData,TEdgeData}.EdgeDirection">edge direction</see>. The
  /// <see cref="RingGraphCreationOption{TNodeData,TEdgeData}.CreateNodeData"/> and
  /// <see cref="RingGraphCreationOption{TNodeData,TEdgeData}.CreateEdgeData"/> functions are used to produce data for
  /// the nodes and edges in the graph depending on their position in the ring.
  /// </summary>
  /// <param name="options">Definition of the grid structure.</param>
  /// <typeparam name="TNodeData">Type of the data the nodes are holding.</typeparam>
  /// <typeparam name="TEdgeData">Type of the data the edges are holding.</typeparam>
  /// <returns>The created graph.</returns>
  public static IndexedGraph<int, TNodeData, TEdgeData> MakeIndexedRing<TNodeData, TEdgeData>(
    RingGraphCreationOption<TNodeData, TEdgeData> options)
  {
    return MakeIndexedGrid(new GridGraphCreationOption<TNodeData, TEdgeData>
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
      CreateEdgeData = gridEdgeData => options.CreateEdgeData(new IndexedGraphEdgeDefinition<int, TNodeData>
      {
        OriginIndex = gridEdgeData.OriginIndex[0],
        DestinationIndex = gridEdgeData.DestinationIndex[0],
        OriginNodeData = gridEdgeData.OriginNodeData,
        DestinationNodeData = gridEdgeData.DestinationNodeData,
      }),
    }).Transform(node => node, edge => edge, coordinate => coordinate[0]);
  }
}