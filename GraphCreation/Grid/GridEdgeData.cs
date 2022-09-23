using Graph;

namespace GraphCreation;

public readonly struct GridEdgeData<TNodeData, TEdgeData>
{
  public readonly IReadOnlyList<int> FromCoordinates;
  public readonly IReadOnlyList<int> ToCoordinates;
  public readonly Node<TNodeData, TEdgeData> FromNode;
  public readonly Node<TNodeData, TEdgeData> ToNode;

  public GridEdgeData(IReadOnlyList<int> fromCoordinates, IReadOnlyList<int> toCoordinates, Node<TNodeData, TEdgeData> fromNode,
    Node<TNodeData, TEdgeData> toNode)
  {
    FromCoordinates = fromCoordinates;
    ToCoordinates = toCoordinates;
    FromNode = fromNode;
    ToNode = toNode;
  }
}