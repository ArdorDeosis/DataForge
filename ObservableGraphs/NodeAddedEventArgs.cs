using DataForge.Graphs;
using JetBrains.Annotations;

namespace ObservableGraphs;

[PublicAPI]
public sealed class NodeAddedEventArgs<TNodeData, TEdgeData> : EventArgs {
  public NodeAddedEventArgs(Node<TNodeData, TEdgeData> addedNode)
  {
    AddedNode = addedNode;
  }
  public Node<TNodeData, TEdgeData> AddedNode {get;}
  
  public static implicit operator NodeAddedEventArgs<TNodeData, TEdgeData> (Node<TNodeData, TEdgeData> node) => new (node);
}