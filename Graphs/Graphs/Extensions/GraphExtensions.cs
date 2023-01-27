namespace DataForge.Graphs.Extensions;

public static class GraphExtensions
{
  public static void InsertNodeInto<TNodeData, TEdgeData>(this Edge<TNodeData, TEdgeData> edge, TNodeData data,
    TEdgeData incomingEdgeData, TEdgeData outgoingEdgeData)
  {
    var graph = edge.Graph;
    var node = graph.AddNode(data);
    graph.AddEdge(edge.Origin, node, incomingEdgeData);
    graph.AddEdge(node, edge.Destination, outgoingEdgeData);
    graph.RemoveEdge(edge);
  }
  
  public static void Reverse<TNodeData, TEdgeData>(this Edge<TNodeData, TEdgeData> edge)
  {
    edge.Graph.AddEdge(edge.Destination, edge.Origin, edge.Data);
    edge.Graph.RemoveEdge(edge);
  }
  
  public static void RemoveFromGraph<TNodeData, TEdgeData>(this Edge<TNodeData, TEdgeData> edge) => 
    edge.Graph.RemoveEdge(edge);
  
  public static void RemoveFromGraph<TNodeData, TEdgeData>(this Node<TNodeData, TEdgeData> node) => 
    node.Graph.RemoveNode(node);
}