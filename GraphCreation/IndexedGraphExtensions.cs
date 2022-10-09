using Graph;

namespace GraphCreation;

internal static class GraphConvenienceExtensions
{
  internal static void AddEdgesForDirection<TIndex, TNodeData, TEdgeData>(
    this IndexedGraph<TIndex, TNodeData, TEdgeData> graph,
    EdgeDirection direction,
    TIndex lowerIndex,
    TIndex upperIndex,
    Func<IndexedGraphEdgeDefinition<TIndex, TNodeData>, TEdgeData> createEdgeData
  ) where TIndex : notnull
  {
    if (direction == EdgeDirection.None) return;

    if (direction.HasFlag(EdgeDirection.Forward))
    {
      graph.AddEdge(lowerIndex, upperIndex, createEdgeData(new IndexedGraphEdgeDefinition<TIndex, TNodeData>
      {
        OriginIndex = lowerIndex,
        DestinationIndex = upperIndex,
        OriginNodeData = graph[lowerIndex].Data,
        DestinationNodeData = graph[upperIndex].Data,
      }));
    }

    if (direction.HasFlag(EdgeDirection.Backward))
    {
      graph.AddEdge(upperIndex, lowerIndex, createEdgeData(new IndexedGraphEdgeDefinition<TIndex, TNodeData>
      {
        OriginIndex = upperIndex,
        DestinationIndex = lowerIndex,
        OriginNodeData = graph[upperIndex].Data,
        DestinationNodeData = graph[lowerIndex].Data,
      }));
    }
  }

  internal static void AddEdgesForDirection<TNodeData, TEdgeData>(
    this Graph<TNodeData, TEdgeData> graph,
    EdgeDirection direction,
    Node<TNodeData, TEdgeData> lowerNode,
    Node<TNodeData, TEdgeData> upperNode,
    Func<TNodeData, TNodeData, TEdgeData> createEdgeData
  )
  {
    if (direction.HasFlag(EdgeDirection.Forward))
      graph.AddEdge(lowerNode, upperNode, createEdgeData(lowerNode.Data, upperNode.Data));

    if (direction.HasFlag(EdgeDirection.Backward))
      graph.AddEdge(upperNode, lowerNode, createEdgeData(upperNode.Data, lowerNode.Data));
  }
}