﻿namespace Graph;

public abstract class GraphComponent<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> :
  IGraphComponent
  where TNodeIndex : notnull
  where TEdgeIndex : notnull
{
  public bool IsValid { get; private set; } = true;

  internal readonly InternalGraph<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> Graph;

  internal void Invalidate() => IsValid = false;

  private protected GraphComponent(InternalGraph<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> graph)
  {
    Graph = graph ?? throw new ArgumentNullException(nameof(graph));
  }

  private protected InvalidOperationException ComponentInvalidException =>
    new("This graph component has been removed from its graph.");

  private protected InvalidOperationException DataImmutableException =>
    new("This graph component has been removed from its graph, data on it can not be changed.");
}