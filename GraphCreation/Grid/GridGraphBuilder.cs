using Graph;
using GridUtilities;

namespace GraphCreation;

public class GridGraphBuilder<TNodeData, TEdgeData>
{
  internal GridGraphDimensionInformation[] DimensionsInformation = Array.Empty<GridGraphDimensionInformation>();
  internal Func<GridNodeData, TNodeData>? CreateNodeData;
  internal Func<GridEdgeData<TNodeData, TEdgeData>, TEdgeData>? CreateEdgeData;

  protected void Validate()
  {
    if (DimensionsInformation.Length < 1)
      throw new InvalidOperationException("There are no dimensions defined!");
    if (CreateNodeData is null)
      throw new InvalidOperationException("No method to generate node data is defined!");
    if (CreateEdgeData is null)
      throw new InvalidOperationException("No method to generate edge data is defined!");
  }

  public Graph<TNodeData, TEdgeData> Build()
  {
    Validate();
    var gridDefinition = DimensionsInformation.Select(info => info.ToGridDimensionInformation()).ToArray();
    var graph = new Graph<TNodeData, TEdgeData>();
    var nodes = new Dictionary<IReadOnlyList<int>, Node<TNodeData, TEdgeData>>(new CoordinateEqualityComparer());
    foreach (var coordinate in Grid.Coordinates(gridDefinition))
      nodes.Add(coordinate, graph.AddNode(CreateNodeData!(new GridNodeData(coordinate))));

    foreach (var info in Grid.EdgeInformation(gridDefinition))
    {
      var direction = DimensionsInformation[info.DimensionOfChange].EdgeDirection;
      if (direction == EdgeDirection.None)
        continue;
      var lowerNode = nodes[info.LowerCoordinate];
      var upperNode = nodes[info.UpperCoordinate];

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

  /// <summary>
  /// A <see cref="EqualityComparer{T}"/> to compare int arrays representing coordinates.
  /// </summary>
  /// <remarks>
  /// The <see cref="GetHashCode"/> functions only produces useful hashes for coordinates with up to 8 dimensions. After
  /// that, all further dimensions are ignored in regards to generating the hash. This could result in suboptimal
  /// performance for retrieving objects from a dictionary for coordinates with more than 8 dimensions. 
  /// </remarks>
  private class CoordinateEqualityComparer : EqualityComparer<IReadOnlyList<int>>
  {
    /// <inheritdoc/>
    public override bool Equals(IReadOnlyList<int>? left, IReadOnlyList<int>? right) =>
      left is not null && right is not null &&
      CoordinateHelpers.CoordinatesEqual(left, right);

    /// <inheritdoc/>
    public override int GetHashCode(IReadOnlyList<int> coordinate) => CoordinateHelpers.GetCoordinateHashCode(coordinate);
  }
}