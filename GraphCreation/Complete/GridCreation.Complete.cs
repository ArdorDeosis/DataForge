using Graph;

namespace GraphCreation;

public static partial class GraphCreator
{
  public static Graph<TNodeData, TEdgeData> MakeComplete<TNodeData, TEdgeData>(
    CompleteGraphCreationOption<TNodeData, TEdgeData> options)
  {
    var graph = new Graph<TNodeData, TEdgeData>();

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