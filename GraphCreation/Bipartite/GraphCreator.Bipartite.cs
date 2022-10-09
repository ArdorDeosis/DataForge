using Graph;

namespace GraphCreation;

public static partial class GraphCreator
{
  public static Graph<TNodeData, TEdgeData> MakeBipartite<TNodeData, TEdgeData>(
    BipartiteGraphCreationOption<TNodeData, TEdgeData> options) =>
    MakeMultipartite(new MultipartiteGraphCreationOption<TNodeData, TEdgeData>
    {
      CreateEdgeData = options.CreateEdgeData,
      NodeDataSets = new[] { options.NodeDataSetA, options.NodeDataSetB },
      CreateEdge = options.CreateEdge,
    });
}