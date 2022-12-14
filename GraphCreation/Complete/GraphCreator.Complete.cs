using Graph;

namespace GraphCreation;

public static partial class GraphCreator
{
  /// <summary>
  /// Creates a complete graph from a set of node data. The
  /// <see cref="CompleteGraphCreationOptions{TNodeData,TEdgeData}">options</see> provide the  
  /// <see cref="CompleteGraphCreationOptions{TNodeData,TEdgeData}.NodeData">node data</see>, 
  /// <see cref="CompleteGraphCreationOptions{TNodeData,TEdgeData}.EdgeDirection">edge direction</see> and a function to 
  /// <see cref="UnindexedGraphEdgeDataCreationOption{TNodeData,TEdgeData}.CreateEdgeData">generate edge data</see>.
  /// </summary>
  /// <param name="options">Graph creation options.</param>
  /// <typeparam name="TNodeData">Type of the data the nodes are holding.</typeparam>
  /// <typeparam name="TEdgeData">Type of the data the edges are holding.</typeparam>
  /// <returns>The created graph.</returns>
  public static OldGraph<TNodeData, TEdgeData> MakeComplete<TNodeData, TEdgeData>(
    CompleteGraphCreationOptions<TNodeData, TEdgeData> options)
  {
    var graph = new OldGraph<TNodeData, TEdgeData>();

    var nodeArray = graph.AddNodes(options.NodeData).ToArray();

    for (var i = 0; i < nodeArray.Length; i++)
    {
      for (var j = i + 1; j < nodeArray.Length; j++)
      {
        graph.AddEdgesForDirection(
          options.EdgeDirection,
          nodeArray[i],
          nodeArray[j],
          options.CreateEdgeData
        );
      }
    }

    return graph;
  }
}