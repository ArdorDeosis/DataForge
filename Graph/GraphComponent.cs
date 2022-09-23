using System.Diagnostics.CodeAnalysis;

namespace Graph;

public abstract class GraphComponent<TNodeData, TEdgeData>
{
  private GraphBase<TNodeData, TEdgeData>? graph;

  protected GraphComponent(GraphBase<TNodeData, TEdgeData> graph)
  {
    Graph = graph;
  }

  protected GraphBase<TNodeData, TEdgeData> Graph
  {
    get
    {
      ThrowIfInvalid();
      return graph!;
    }
    private init => graph = value;
  }

  internal bool IsValid => graph != null;

  internal void Invalidate()
  {
    ThrowIfInvalid();
    graph = null;
  }

  [SuppressMessage("ReSharper", "ParameterHidesMember")]
  internal bool IsIn(GraphBase<TNodeData, TEdgeData> graph) => Graph == graph;
  
  /// <remarks>
  /// This deliberately calls the <see cref="Graph"/> property for itself, but the <see cref="graph"/> field for
  /// <see cref="other"/>, since it should throw an exception if the component itself is invalid, but it should be
  /// possible to compare to an invalid component without exception.
  /// </remarks>
  internal bool IsInSameGraphAs(GraphComponent<TNodeData, TEdgeData> other) => Graph == other.graph;

  private void ThrowIfInvalid()
  {
    if (!IsValid)
      throw new InvalidOperationException($"{GetType().Name} has been removed from its graph and is no longer valid.");
  }
}