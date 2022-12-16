namespace Graph;

public interface IGraph<TNodeData, TEdgeData> : IWriteOnlyGraph, IReadOnlyGraph<TNodeData, TEdgeData> { }