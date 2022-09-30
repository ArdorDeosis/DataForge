using Graph;
using JetBrains.Annotations;
using Utilities;

namespace GraphCreation;

[PublicAPI]
public readonly struct GridEdgeData<TNodeData, TEdgeData>
{
  public readonly IReadOnlyList<int> LowerCoordinate;
  public readonly IReadOnlyList<int> UpperCoordinate;
  public readonly Node<TNodeData, TEdgeData> LowerNode;
  public readonly Node<TNodeData, TEdgeData> UpperNode;

  public GridEdgeData() =>
    ThrowHelper.ThrowStructNotPubliclyConstructableException(nameof(GridEdgeData<TNodeData, TEdgeData>));

  internal GridEdgeData(IReadOnlyList<int> lowerCoordinate, IReadOnlyList<int> upperCoordinate,
    Node<TNodeData, TEdgeData> lowerNode,
    Node<TNodeData, TEdgeData> upperNode)
  {
    LowerCoordinate = lowerCoordinate;
    UpperCoordinate = upperCoordinate;
    LowerNode = lowerNode;
    UpperNode = upperNode;
  }
}