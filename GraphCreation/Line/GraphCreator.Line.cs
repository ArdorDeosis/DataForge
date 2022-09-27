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
    var graph = new Graph<TNodeData, TEdgeData>();

    var nodes = new Dictionary<int, Node<TNodeData, TEdgeData>>();
    for (var position = 0; position < options.Length; position++)
      nodes.Add(position, graph.AddNode(options.CreateNodeData(new LineNodeData(position))));

    for (var lowerPosition = 0; lowerPosition < options.Length - 1; lowerPosition++)
    {
      if (options.EdgeDirection == EdgeDirection.None)
        continue;
      var lowerNode = nodes[lowerPosition];
      var upperNode = nodes[lowerPosition + 1];

      if (options.EdgeDirection.HasFlag(EdgeDirection.Forward))
      {
        graph.AddEdge(
          lowerNode,
          upperNode,
          options.CreateEdgeData(new LineEdgeData<TNodeData, TEdgeData>(
            lowerPosition,
            lowerNode,
            upperNode)
          )
        );
      }

      if (options.EdgeDirection.HasFlag(EdgeDirection.Backward))
      {
        graph.AddEdge(
          upperNode,
          lowerNode,
          options.CreateEdgeData(new LineEdgeData<TNodeData, TEdgeData>(
            lowerPosition,
            upperNode,
            lowerNode)
          )
        );
      }
    }

    return graph;
  }
}