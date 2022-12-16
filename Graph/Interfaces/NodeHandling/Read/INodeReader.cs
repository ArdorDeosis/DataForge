namespace Graph;

public interface INodeReader<TNodeData, TEdgeData>
{
  IEnumerable<INode<TNodeData, TEdgeData>> Nodes { get; }
  bool Contains(INode<TNodeData, TEdgeData> node);
}