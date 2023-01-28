using DataForge.Graphs;
using JetBrains.Annotations;

namespace ObservableGraphs;

[PublicAPI]
public sealed class IndexedNodeRemovedEventArgs<TIndex, TNodeData, TEdgeData> : EventArgs where TIndex : notnull
{
  public IndexedNodeRemovedEventArgs(IndexedNode<TIndex, TNodeData, TEdgeData> removedNode)
  {
    RemovedNode = removedNode;
  }
  public IndexedNode<TIndex, TNodeData, TEdgeData> RemovedNode {get;}
  
  public static implicit operator IndexedNodeRemovedEventArgs<TIndex, TNodeData, TEdgeData> (IndexedNode<TIndex, TNodeData, TEdgeData> node) => new (node);
}