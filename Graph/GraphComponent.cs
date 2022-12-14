using System.Diagnostics.CodeAnalysis;

namespace Graph;

/// <summary>
/// Base class for graph components (nodes or edges).
/// </summary>
/// <typeparam name="TNodeData">Type of the data the nodes are holding.</typeparam>
/// <typeparam name="TEdgeData">Type of the data the edge is holding.</typeparam>
public abstract class GraphComponent<TNodeIndex, TNodeData, TEdgeData>
{
  private GraphBase<TNodeData, TEdgeData>? graph;

  protected GraphComponent(GraphBase<TNodeData, TEdgeData> graph)
  {
    Graph = graph;
  }

  /// <summary>
  /// The graph this nodes component belongs to.
  /// </summary>
  /// <exception cref="InvalidOperationException">If this component has been invalidated.</exception>
  protected GraphBase<TNodeData, TEdgeData> Graph
  {
    get
    {
      ThrowIfInvalid();
      return graph!;
    }
    private init => graph = value;
  }

  /// <summary>
  /// Whether this component is valid.
  /// </summary>
  public bool IsValid => graph != null;

  /// <summary>
  /// Invalidates this component.
  /// </summary>
  /// <remarks>This should happen if the component is removed from its graph.</remarks>
  internal void Invalidate()
  {
    ThrowIfInvalid();
    graph = null;
  }

  /// <summary>
  /// Whether this component belongs to the provided graph.
  /// </summary>
  /// <param name="graph">The graph to check.</param>
  [SuppressMessage("ReSharper", "ParameterHidesMember")]
  internal bool IsIn(GraphBase<TNodeData, TEdgeData> graph) => Graph == graph;

  /// <summary>
  /// Whether this component belongs to the same graph as the provided component.
  /// </summary>
  /// <param name="other">The component to check.</param>
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