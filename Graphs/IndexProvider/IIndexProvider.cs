namespace DataForge.Graphs;

/// <summary>
/// An interface for an index provider generating indices based on provided data.
/// </summary>
/// <typeparam name="TData">The type of the data indices are generated for.</typeparam>
/// <typeparam name="TIndex">The type of the indices generated.</typeparam>
public interface IIndexProvider<in TData, out TIndex> where TIndex : notnull
{
  /// <summary>
  /// Moves the internal state of the index provider to the next index.
  /// </summary>
  void Move();

  /// <summary>
  /// Returns the current index for the provided data without changing the internal state of the index provider.
  /// </summary>
  /// <remarks>
  /// This is used to retrieve an index and check whether it exists in a graph before changing the internal state of
  /// the index provider.
  /// </remarks>
  TIndex GetCurrentIndex(TData data);
}