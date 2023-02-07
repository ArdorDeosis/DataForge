namespace DataForge.Graphs;

public abstract class GraphComponent : IGraphComponent
{
  public bool IsValid { get; private set; } = true;

  private protected static InvalidOperationException ComponentInvalidException =>
    new("This graph component has been removed from its graph.");

  private protected static InvalidOperationException DataImmutableException =>
    new("This graph component has been removed from its graph, data on it can not be changed.");

  internal void Invalidate()
  {
    IsValid = false;
  }
}