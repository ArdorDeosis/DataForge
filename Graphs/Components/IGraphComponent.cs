namespace DataForge.Graphs;

public interface IGraphComponent
{
  bool IsValid { get; }

  bool RemoveFromGraph();
}