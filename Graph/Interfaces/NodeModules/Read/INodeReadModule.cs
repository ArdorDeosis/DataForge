namespace Graph;

public interface INodeReadModule<TNodeData, TEdgeData>
{
  IEnumerable<INode<TNodeData, TEdgeData>> Nodes { get; }
  bool Contains(INode<TNodeData, TEdgeData> node);
}