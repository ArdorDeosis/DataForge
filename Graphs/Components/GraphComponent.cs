namespace DataForge.Graphs;

public abstract class GraphComponent : IGraphComponent
{
  private protected InvalidOperationException ComponentInvalidException =>
    new("This graph component has been removed from its graph.");

  private protected InvalidOperationException DataImmutableException =>
    new("This graph component has been removed from its graph, data on it can not be changed.");

  public bool IsValid { get; private set; } = true;
  
  public abstract bool RemoveFromGraph();

  internal void Invalidate()
  {
    IsValid = false;
  }
}