namespace DataForge.Graphs;

public abstract class GraphComponent
{
  public bool IsValid { get; private set; } = true;
  
  protected abstract string Description { get; }

  private protected InvalidOperationException ComponentInvalidException =>
    new($"This {Description} has been removed from its graph.");

  private protected InvalidOperationException DataImmutableException =>
    new($"This {Description} has been removed from its graph, data on it can not be changed.");

  public void Invalidate()
  {
    IsValid = false;
  }
}