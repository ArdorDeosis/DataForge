using Graph;

namespace GraphCreation;

internal static class GraphConvenienceExtensions
{
  internal static void AddEdgesForDirection<TIndex, TNodeData, TEdgeData>(
    this NodeIndexedGraph<TIndex, TNodeData, TEdgeData> graph,
    EdgeDirection direction,
    TIndex lowerIndex,
    TIndex upperIndex,
    Func<IndexedGraphEdgeDataCreationInput<TIndex, TNodeData>, TEdgeData> createEdgeData
  ) where TIndex : notnull
  {
    if (direction == EdgeDirection.None) return;

    if (direction.HasFlag(EdgeDirection.Forward))
    {
      graph.AddEdge(lowerIndex, upperIndex, createEdgeData(new IndexedGraphEdgeDataCreationInput<TIndex, TNodeData>
      {
        StartIndex = lowerIndex,
        EndIndex = upperIndex,
        StartNodeData = graph[lowerIndex].Data,
        EndNodeData = graph[upperIndex].Data,
      }));
    }

    if (direction.HasFlag(EdgeDirection.Backward))
    {
      graph.AddEdge(upperIndex, lowerIndex, createEdgeData(new IndexedGraphEdgeDataCreationInput<TIndex, TNodeData>
      {
        StartIndex = upperIndex,
        EndIndex = lowerIndex,
        StartNodeData = graph[upperIndex].Data,
        EndNodeData = graph[lowerIndex].Data,
      }));
    }
  }

  internal static void AddEdgesForDirection<TNodeData, TEdgeData>(
    this OldGraph<TNodeData, TEdgeData> graph,
    EdgeDirection direction,
    OldNode<,,> lowerNode,
    OldNode<,,> upperNode,
    Func<TNodeData, TNodeData, TEdgeData> createEdgeData
  )
  {
    if (direction.HasFlag(EdgeDirection.Forward))
      graph.AddEdge(lowerNode, upperNode, createEdgeData(lowerNode.Data, upperNode.Data));

    if (direction.HasFlag(EdgeDirection.Backward))
      graph.AddEdge(upperNode, lowerNode, createEdgeData(upperNode.Data, lowerNode.Data));
  }
}