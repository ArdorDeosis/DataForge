using JetBrains.Annotations;

namespace DataForge.GraphCreation;

/// <summary>
/// Creation options for an indexed graph containing delegates to create node- and edge data.
/// </summary>
/// <typeparam name="TIndex">Index type of the graph to be created.</typeparam>
/// <typeparam name="TNodeData">Type of the node data of the graph to be created.</typeparam>
/// <typeparam name="TEdgeData">Type of the edge data of the graph to be created.</typeparam>
/// TODO: use required keyword in C# 11
[PublicAPI]
public abstract class IndexedGraphDataCreationOptions<TIndex, TNodeData, TEdgeData> where TIndex : notnull
{
#pragma warning disable CS8618 // these warnings should vanish when the required keyword comes
  /// <summary>
  /// Function to create node data based on the node's index in the graph.
  /// <code>TNodeData CreateNodeData(TIndex index)</code>
  /// It expects the node's index as parameter and returns the node's data.
  /// </summary>
  public /*required*/ Func<TIndex, TNodeData> CreateNodeData { get; init; }

  /// <summary>
  /// Function to create edge data based on the indices and data of the nodes the edge connects.
  /// <code>TEdgeData CreateEdgeData(IndexedGraphEdgeDataCreationInput&lt;TIndex,TNodeData&gt; nodeData)</code>
  /// It expects an <see cref="IndexedGraphEdgeDataCreationInput{TIndex,TNodeData}" /> object as input and returns the
  /// edge's data.
  /// </summary>
  public /*required*/
    Func<IndexedGraphEdgeDataCreationInput<TIndex, TNodeData>, TEdgeData> CreateEdgeData { get; init; }
#pragma warning restore CS8618
}