using System.Diagnostics.CodeAnalysis;
using Utilities;

namespace Graph;

// TODO: write tests
// just excluded it for the satisfaction of having 100% on the stuff that is actually finished.
// this should probably linger on its own branch somewhere...
[ExcludeFromCodeCoverage]
public static class Pathfinding
{
  private record PathFindingData<TNodeData, TEdgeData, TDistance>(OldEdge<,,>[] Path, TDistance Distance)
  {
    public readonly OldEdge<,,>[] Path = Path;
    public readonly TDistance Distance = Distance;
  }

  private static PathFindingData<TNodeData, TEdgeData, TDistance>? AStarPath<TNodeData, TEdgeData, TDistance>(
    OldNode<,,> start,
    OldNode<,,> end,
    TDistance initialDistance,
    Func<TDistance, TEdgeData, TDistance> calculateDistance,
    IComparer<TDistance> comparer)
  {
    if (!start.IsValid) throw new ArgumentException("start node is invalid");
    if (!end.IsValid) throw new ArgumentException("end node is invalid");
    if (!start.IsInSameGraphAs(end))
      throw new InvalidOperationException("start and end node are not part of the same graph");

    var pathFindingData =
      new Dictionary<OldNode<,,>, PathFindingData<TNodeData, TEdgeData, TDistance>>();
    var processedNodes = new HashSet<OldNode<,,>>();
    var frontier = new PriorityQueue<OldNode<,,>, TDistance>(comparer);

    pathFindingData.Add(
      start,
      new PathFindingData<TNodeData, TEdgeData, TDistance>(
        Array.Empty < OldEdge <,,  >> (),
    initialDistance
      )
      );
    frontier.Enqueue(start, initialDistance);

    while (frontier.Count > 0)
    {
      var nodeBeingProcessed = frontier.Dequeue();
      // if the end node is the closer than any other frontier node, we found the shortest path
      if (nodeBeingProcessed == end)
        return pathFindingData[nodeBeingProcessed];
      // if the discovered node has already been processed, continue, since it must have had a shorter distance than the
      // current path
      if (processedNodes.Contains(nodeBeingProcessed))
        continue;

      ProcessNode(nodeBeingProcessed);
    }

    // if the frontier is empty and we have not yet found a path, there is no path
    return null;

    void ProcessNode(OldNode<,,> nodeBeingProcessed)
    {
      foreach (var edge in nodeBeingProcessed.OutgoingEdges)
        ProcessEdge(edge);

      processedNodes.Add(nodeBeingProcessed);
    }

    void ProcessEdge(OldEdge<,,> edge)
    {
      var distanceToNextNode = calculateDistance(pathFindingData[edge.Start].Distance, edge.Data);
      var nextNode = edge.End;
      // if there is a shorter path to the discovered node already, continue
      if (pathFindingData.TryGetValue(nextNode, out var data) &&
          comparer.Compare(data.Distance, distanceToNextNode) < 0)
        return;
      frontier.Enqueue(nextNode, distanceToNextNode);
      pathFindingData.ForceAdd(nextNode, new PathFindingData<TNodeData, TEdgeData, TDistance>(
        pathFindingData[edge.Start].Path.Concat(new[] { edge }).ToArray(),
        distanceToNextNode
      ));
    }
  }

  private static int? EdgeDistance<TNodeData, TEdgeData>(OldNode<,,> start,
    OldNode<,,> end)
  {
    if (!start.IsValid) throw new ArgumentException("start node is invalid");
    if (!end.IsValid) throw new ArgumentException("end node is invalid");
    if (!start.IsInSameGraphAs(end))
      throw new InvalidOperationException("start and end node are not part of the same graph");

    var pathTo = new Dictionary<OldNode<,,>, OldEdge<,,>[]>();
    var processedNodes = new HashSet<OldNode<,,>>();
    var frontier = new PriorityQueue<OldNode<,,>, int>();

    pathTo.Add(start, Array.Empty < OldEdge <,,  >> ());
    frontier.Enqueue(start, 0);

    while (frontier.Count > 0)
    {
      var nodeBeingProcessed = frontier.Dequeue();
      // if the end node is the closer than any other frontier node, we found the shortest path
      if (nodeBeingProcessed == end)
        return pathTo[nodeBeingProcessed].Length;
      // if the discovered node has already been processed, continue, since it must have had a shorter distance than the
      // current path
      if (processedNodes.Contains(nodeBeingProcessed))
        continue;

      ProcessNode(nodeBeingProcessed);
    }

    // if the frontier is empty and we have not yet found a path, there is no path
    return null;

    void ProcessNode(OldNode<,,> nodeBeingProcessed)
    {
      foreach (var edge in nodeBeingProcessed.OutgoingEdges)
        ProcessEdge(edge);
      processedNodes.Add(nodeBeingProcessed);
    }

    void ProcessEdge(OldEdge<,,> edge)
    {
      var distanceToNextNode = pathTo[edge.Start].Length + 1;
      var nextNode = edge.End;
      // if there is a shorter path to the discovered node already, continue
      if (pathTo.TryGetValue(nextNode, out var pathToNextNode) && pathToNextNode.Length < distanceToNextNode)
        return;
      frontier.Enqueue(nextNode, distanceToNextNode);
      pathTo.ForceAdd(nextNode, pathTo[edge.Start].Concat(new[] { edge }).ToArray());
    }
  }
}