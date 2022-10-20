using JetBrains.Annotations;

namespace GraphCreation;

/// <summary>
/// Creation options for a complete graph.
/// </summary>
/// <typeparam name="TNodeData">Type of the data the nodes are holding.</typeparam>
/// <typeparam name="TEdgeData">Type of the data the edges are holding.</typeparam>
/// TODO: use required keyword in C# 11
[PublicAPI]
public sealed class CompleteGraphCreationOptions<TNodeData, TEdgeData>
  : UnindexedGraphEdgeDataCreationOption<TNodeData, TEdgeData>
{
#pragma warning disable CS8618 // this should be unnecessary once the required keyword is in use
  /// <summary>
  /// A collection of node data to create the nodes of the graph from.
  /// </summary>
  public /*required*/ IEnumerable<TNodeData> NodeData { get; init; }
#pragma warning restore CS8618

  /// <summary>
  /// Direction in which edged in the graph should be created.
  /// </summary>
  public EdgeDirection EdgeDirection { get; init; } = EdgeDirection.Forward;
}