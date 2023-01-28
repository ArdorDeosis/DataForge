using DataForge.Graphs;
using JetBrains.Annotations;

namespace ObservableGraphs;

[PublicAPI]
public sealed class NodeRemovedEventArgs<TNodeData, TEdgeData> : EventArgs {
  public NodeRemovedEventArgs(Node<TNodeData, TEdgeData> removedNode)
  {
    RemovedNode = removedNode;
  }
  public Node<TNodeData, TEdgeData> RemovedNode {get;}
  
  public static implicit operator NodeRemovedEventArgs<TNodeData, TEdgeData> (Node<TNodeData, TEdgeData> node) => new (node);
}