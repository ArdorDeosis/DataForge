using System.Diagnostics.CodeAnalysis;

namespace Graph;

public interface IIndexedGraph<TNodeIndex, TNodeData, TEdgeData> : IGraph<TNodeData, TEdgeData>,
  IReadOnlyIndexedGraph<TNodeIndex, TNodeData, TEdgeData> where TNodeIndex : notnull
{
  void AddNode(TNodeIndex index, TNodeData data);
  bool TryAddNode(TNodeIndex index, TNodeData data);
  bool RemoveNode(INode<TNodeData, TEdgeData> node);
  bool RemoveNode(TNodeIndex index);
  void AddEdge(TNodeIndex start, TNodeIndex end, TEdgeData data);
  void AddEdge(INode<TNodeData, TEdgeData> start, INode<TNodeData, TEdgeData> end, TEdgeData data);
  bool RemoveEdge(IEdge<TNodeData, TEdgeData> edge);
}