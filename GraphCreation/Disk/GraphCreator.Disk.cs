using Graph;

namespace GraphCreation;

public static partial class GraphCreator
{
  /// <summary>
  /// Creates a graph with a disk structure. The <paramref name="options"/> define the number of
  /// <see cref="DiskGraphCreationOptions{TNodeData,TEdgeData}.RingCount">rings</see> and 
  /// <see cref="DiskGraphCreationOptions{TNodeData,TEdgeData}.MeridianCount">meridians</see>, edge directions and
  /// whether a center node should be created. The
  /// <see cref="DiskGraphCreationOptions{TNodeData,TEdgeData}.CreateNodeData"/> and
  /// <see cref="DiskGraphCreationOptions{TNodeData,TEdgeData}.CreateEdgeData"/> functions are used to produce data for
  /// the nodes and edges in the graph depending on their position on the disk.
  /// </summary>
  /// <param name="options">Definition of the disk structure.</param>
  /// <typeparam name="TNodeData">Type of the data the nodes are holding.</typeparam>
  /// <typeparam name="TEdgeData">Type of the data the edges are holding.</typeparam>
  /// <returns>The created graph.</returns>
  public static Graph<TNodeData, TEdgeData> MakeDisk<TNodeData, TEdgeData>(
    DiskGraphCreationOptions<TNodeData, TEdgeData> options) =>
    MakeIndexedDisk(options).ToNonIndexedGraph();

  /// <summary>
  /// Creates an indexed graph with a disk structure. Nodes are indexed by their position on the disk represented as a
  /// <see cref="DiskIndex"/>. The <paramref name="options"/> define the number of
  /// <see cref="DiskGraphCreationOptions{TNodeData,TEdgeData}.RingCount">rings</see> and 
  /// <see cref="DiskGraphCreationOptions{TNodeData,TEdgeData}.MeridianCount">meridians</see>, edge directions and
  /// whether a center node should be created. The
  /// <see cref="DiskGraphCreationOptions{TNodeData,TEdgeData}.CreateNodeData"/> and
  /// <see cref="DiskGraphCreationOptions{TNodeData,TEdgeData}.CreateEdgeData"/> functions are used to produce data for
  /// the nodes and edges in the graph depending on their position on the disk.
  /// </summary>
  /// <param name="options">Definition of the disk structure.</param>
  /// <typeparam name="TNodeData">Type of the data the nodes are holding.</typeparam>
  /// <typeparam name="TEdgeData">Type of the data the edges are holding.</typeparam>
  /// <returns>The created graph.</returns>
  public static IndexedGraph<DiskIndex, TNodeData, TEdgeData> MakeIndexedDisk<TNodeData, TEdgeData>(
    DiskGraphCreationOptions<TNodeData, TEdgeData> options)
  {
    var graph = new IndexedGraph<DiskIndex, TNodeData, TEdgeData>();
    var centerIndex = new DiskIndex();
    if (options.MakeCenterNode)
      graph.AddNode(centerIndex, options.CreateNodeData(centerIndex));
    for (var ring = 1; ring <= options.RingCount; ring++)
    {
      for (var meridian = 0; meridian < options.MeridianCount; meridian++)
      {
        var index = new DiskIndex(meridian, ring);
        graph.AddNode(index, options.CreateNodeData(index));
        if (meridian > 0)
        {
          graph.AddEdgesForDirection(
            options.RingEdgeDirection,
            new DiskIndex(meridian - 1, ring),
            index,
            options.CreateEdgeData
          );
        }

        if (ring > 1)
        {
          graph.AddEdgesForDirection(
            options.MeridianEdgeDirection,
            new DiskIndex(meridian, ring - 1),
            index,
            options.CreateEdgeData
          );
        }
        else if (options.MakeCenterNode)
        {
          graph.AddEdgesForDirection(
            options.MeridianEdgeDirection,
            centerIndex,
            new DiskIndex(meridian, 1),
            options.CreateEdgeData
          );
        }
      }

      graph.AddEdgesForDirection(
        options.RingEdgeDirection,
        new DiskIndex(options.MeridianCount - 1, ring),
        new DiskIndex(0, ring),
        options.CreateEdgeData
      );
    }

    return graph;
  }
}