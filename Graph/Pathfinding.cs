using Utilities;

namespace Graph;

public static class Pathfinding
{
  private record PathFindingData<TNodeData, TEdgeData, TDistance>(Edge<TNodeData, TEdgeData>[] Path, TDistance Distance)
  {
    public readonly Edge<TNodeData, TEdgeData>[] Path = Path;
    public readonly TDistance Distance = Distance;
  }

  private static PathFindingData<TNodeData, TEdgeData, TDistance>? AStarPath<TNodeData, TEdgeData, TDistance>(
    Node<TNodeData, TEdgeData> start,
    Node<TNodeData, TEdgeData> end,
    TDistance initialDistance,
    Func<TDistance, TEdgeData, TDistance> calculateDistance,
    IComparer<TDistance> comparer)
  {
    if (!start.IsValid) throw new ArgumentException("start node is invalid");
    if (!end.IsValid) throw new ArgumentException("end node is invalid");
    if (!start.IsInSameGraphAs(end))
      throw new InvalidOperationException("start and end node are not part of the same graph");

    var pathFindingData =
      new Dictionary<Node<TNodeData, TEdgeData>, PathFindingData<TNodeData, TEdgeData, TDistance>>();
    var processedNodes = new HashSet<Node<TNodeData, TEdgeData>>();
    var frontier = new PriorityQueue<Node<TNodeData, TEdgeData>, TDistance>(comparer);

    pathFindingData.Add(
      start,
      new PathFindingData<TNodeData, TEdgeData, TDistance>(
        Array.Empty<Edge<TNodeData, TEdgeData>>(),
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

    void ProcessNode(Node<TNodeData, TEdgeData> nodeBeingProcessed)
    {
      foreach (var edge in nodeBeingProcessed.InternalOutgoingEdges)
        ProcessEdge(edge);

      processedNodes.Add(nodeBeingProcessed);
    }

    void ProcessEdge(Edge<TNodeData, TEdgeData> edge)
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

  private static int? EdgeDistance<TNodeData, TEdgeData>(Node<TNodeData, TEdgeData> start, Node<TNodeData, TEdgeData> end)
  {
    if (!start.IsValid) throw new ArgumentException("start node is invalid");
    if (!end.IsValid) throw new ArgumentException("end node is invalid");
    if (!start.IsInSameGraphAs(end))
      throw new InvalidOperationException("start and end node are not part of the same graph");

    var pathTo = new Dictionary<Node<TNodeData, TEdgeData>, Edge<TNodeData, TEdgeData>[]>();
    var processedNodes = new HashSet<Node<TNodeData, TEdgeData>>();
    var frontier = new PriorityQueue<Node<TNodeData, TEdgeData>, int>();

    pathTo.Add(start, Array.Empty<Edge<TNodeData, TEdgeData>>());
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

    void ProcessNode(Node<TNodeData, TEdgeData> nodeBeingProcessed)
    {
      foreach (var edge in nodeBeingProcessed.InternalOutgoingEdges)
        ProcessEdge(edge);
      processedNodes.Add(nodeBeingProcessed);
    }

    void ProcessEdge(Edge<TNodeData, TEdgeData> edge)
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