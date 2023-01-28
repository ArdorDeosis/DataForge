using DataForge.Graphs;
using JetBrains.Annotations;

namespace ObservableGraphs;

[PublicAPI]
public sealed class IndexedNodeAddedEventArgs<TIndex, TNodeData, TEdgeData> : EventArgs where TIndex : notnull
{
  public IndexedNodeAddedEventArgs(IndexedNode<TIndex, TNodeData, TEdgeData> addedNode)
  {
    AddedNode = addedNode;
  }
  public IndexedNode<TIndex, TNodeData, TEdgeData> AddedNode {get;}
  
  public static implicit operator IndexedNodeAddedEventArgs<TIndex, TNodeData, TEdgeData> (IndexedNode<TIndex, TNodeData, TEdgeData> node) => new (node);
}