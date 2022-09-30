using Graph;
using GridUtilities;

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
    LineGraphCreationOption<TNodeData, TEdgeData> options)
  {
    return MakeGrid(new GridGraphCreationOption<TNodeData, TEdgeData>
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
      CreateNodeData = gridNodeData => options.CreateNodeData(new LineNodeData(gridNodeData.Coordinates[0])),
      CreateEdgeData = gridEdgeData => options.CreateEdgeData(new LineEdgeData<TNodeData, TEdgeData>(
        gridEdgeData.LowerCoordinate[0],
        gridEdgeData.LowerNode,
        gridEdgeData.UpperNode)
      ),
    });
  }
}