using Graph;
using GridUtilities;

namespace GraphCreation;

public class IndexedGraphCreator<TIndex, TNodeData, TEdgeData> : GraphCreator
  where TIndex : notnull
{
  internal Func<GridNodeData, TIndex> CreateIndex;

  public IndexedGraphCreator(GraphCreator origin, Func<GridNodeData, TIndex> createIndex)
  {
    DimensionsInformation = origin.DimensionsInformation;
    CreateNodeData = origin.CreateNodeData;
    CreateEdgeData = origin.CreateEdgeData;
    CreateIndex = createIndex;
  }

  public new IndexedGraph<TIndex, TNodeData, TEdgeData> Build()
  {
    Validate();
    var gridDefinition = DimensionsInformation.Select(info => info.ToGridDimensionInformation()).ToArray();
    var graph = new IndexedGraph<TIndex, TNodeData, TEdgeData>();
    foreach (var coordinate in Grid.Coordinates(gridDefinition))
      graph.AddNode(CreateIndex(new GridNodeData(coordinate)), CreateNodeData!(new GridNodeData(coordinate)));

    foreach (var info in Grid.EdgeInformation(gridDefinition))
    {
      var direction = DimensionsInformation[info.DimensionOfChange].EdgeDirection;
      if (direction == EdgeDirection.None)
        continue;
      var lowerNode = graph[CreateIndex(new GridNodeData(info.LowerCoordinate))];
      var upperNode = graph[CreateIndex(new GridNodeData(info.UpperCoordinate))];

      if (direction.HasFlag(EdgeDirection.Forward))
      {
        graph.AddEdge(
          lowerNode,
          upperNode,
          CreateEdgeData!(new GridEdgeData<TNodeData, TEdgeData>(
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
          CreateEdgeData!(new GridEdgeData<TNodeData, TEdgeData>(
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