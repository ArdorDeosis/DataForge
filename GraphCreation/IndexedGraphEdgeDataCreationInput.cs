using System.Diagnostics.CodeAnalysis;

namespace GraphCreation;

/// <summary>
/// Information about two nodes in an indexed graph to create edge data from.
/// </summary>
/// <typeparam name="TIndex">Index type of the graph.</typeparam>
/// <typeparam name="TNodeData">Node data type of the graph.</typeparam>
[ExcludeFromCodeCoverage]
public readonly struct IndexedGraphEdgeDataCreationInput<TIndex, TNodeData> where TIndex : notnull
{
  /// <summary>
  /// Index of the node the edge is coming from.
  /// </summary>
  public /*required*/ TIndex StartIndex { get; init; }

  /// <summary>
  /// Index of the node the edge is going to.
  /// </summary>
  public /*required*/ TIndex EndIndex { get; init; }

  /// <summary>
  /// Data of the node the edge is coming from.
  /// </summary>
  public /*required*/ TNodeData StartNodeData { get; init; }

  /// <summary>
  /// Data of the node the edge is going to.
  /// </summary>
  public /*required*/ TNodeData EndNodeData { get; init; }
}