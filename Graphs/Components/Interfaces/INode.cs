namespace DataForge.Graphs;

public interface INode<TNodeData, TEdgeData>
{
  public TNodeData Data { get; set; }
  
  public IReadOnlyCollection<IEdge<TNodeData, TEdgeData>> Edges { get; }
  
  public IReadOnlyCollection<IEdge<TNodeData, TEdgeData>> IncomingEdges { get; }
  
  public IReadOnlyCollection<IEdge<TNodeData, TEdgeData>> OutgoingEdges { get; }
  
  public IReadOnlyCollection<INode<TNodeData, TEdgeData>> Neighbours { get; }
}