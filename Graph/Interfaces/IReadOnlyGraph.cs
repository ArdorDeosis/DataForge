namespace Graph;

public interface IReadOnlyGraph<TNodeData, TEdgeData>
{
  IEnumerable<INode<TNodeData, TEdgeData>> Nodes { get; }
  IEnumerable<IEdge<TNodeData, TEdgeData>> Edges { get; }
  public int Order { get; }
  public int Size { get; }
}