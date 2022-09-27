using Graph;

namespace GraphCreation;

public readonly struct LineEdgeData<TNodeData, TEdgeData>
{
  public readonly int LowerPosition;
  public readonly Node<TNodeData, TEdgeData> LowerNode;
  public readonly Node<TNodeData, TEdgeData> UpperNode;

  public int UpperPosition => LowerPosition + 1;

  public LineEdgeData(int lowerPosition, Node<TNodeData, TEdgeData> lowerNode, Node<TNodeData, TEdgeData> upperNode)
  {
    LowerPosition = lowerPosition;
    LowerNode = lowerNode;
    UpperNode = upperNode;
  }
}