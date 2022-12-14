using System.Diagnostics.CodeAnalysis;
using Utilities;

namespace Graph;

internal sealed class InternalGraph<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>
    where TNodeIndex : notnull where TEdgeIndex : notnull
{
    private readonly Dictionary<TNodeIndex, InternalNode<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>> nodes;
    private readonly Dictionary<TEdgeIndex, InternalEdge<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>> edges;

    private readonly HashSetDictionary<TNodeIndex, TEdgeIndex>
        outgoingEdges = new();

    private readonly HashSetDictionary<TNodeIndex, TEdgeIndex>
        incomingEdges = new();

    private readonly Func<IEqualityComparer<TNodeIndex>?> nodeIndexEqualityComparerFactoryMethod;
    private readonly Func<IEqualityComparer<TEdgeIndex>?> edgeIndexEqualityComparerFactoryMethod;

    internal IEnumerable<TNodeIndex> NodeIndices => nodes.Keys;
    internal IEnumerable<TEdgeIndex> EdgeIndices => edges.Keys;

    // TODO: should these be read-only collections?
    internal IEnumerable<InternalNode<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>> Nodes => nodes.Values;
    internal IEnumerable<InternalEdge<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>> Edges => edges.Values;

    // TODO: should these have custom exceptions?
    internal InternalNode<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> GetNode(TNodeIndex index) => nodes[index];
    internal InternalEdge<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> GetEdge(TEdgeIndex index) => edges[index];

    internal bool TryGetNode(TNodeIndex index,
        [NotNullWhen(true)] out InternalNode<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>? node) =>
        nodes.TryGetValue(index, out node);
    
    internal bool TryGetEdge(TEdgeIndex index,
        [NotNullWhen(true)] out InternalEdge<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>? edge) =>
        edges.TryGetValue(index, out edge);

    internal int Order => nodes.Count;
    internal int Size => edges.Count;

    internal bool Contains(TNodeIndex index) => nodes.ContainsKey(index);

    // ### CONSTRUCTORS ###

    internal InternalGraph() : this(() => null, () => null)
    {
    }

    internal InternalGraph(IEqualityComparer<TNodeIndex>? nodeIndexEqualityComparer,
        IEqualityComparer<TEdgeIndex>? edgeIndexEqualityComparer)
        : this(() => nodeIndexEqualityComparer, () => edgeIndexEqualityComparer)
    {
    }

    internal InternalGraph(Func<IEqualityComparer<TNodeIndex>?> nodeIndexEqualityComparerFactoryMethod,
        Func<IEqualityComparer<TEdgeIndex>?> edgeIndexEqualityComparerFactoryMethod)
    {
        this.nodeIndexEqualityComparerFactoryMethod = nodeIndexEqualityComparerFactoryMethod;
        this.edgeIndexEqualityComparerFactoryMethod = edgeIndexEqualityComparerFactoryMethod;
        nodes = new Dictionary<TNodeIndex, InternalNode<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>>(
            nodeIndexEqualityComparerFactoryMethod());
        edges = new Dictionary<TEdgeIndex, InternalEdge<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>>(
            edgeIndexEqualityComparerFactoryMethod());
    }

    private InternalGraph(InternalGraph<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> origin)
        : this(origin, nodeData => nodeData, edgeData => edgeData)
    {
        // TODO
    }

    private InternalGraph(InternalGraph<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData> origin,
        Func<TNodeData, TNodeData> copyNodeData, Func<TEdgeData, TEdgeData> copyEdgeData)
    {
        // TODO
        throw new NotImplementedException();
    }

    // internal static InternalGraph<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>
    //   Transform<TOriginalNodeIndex, TOriginalNodeData, TOriginalEdgeIndex, TOriginalEdgeData>(
    //     InternalGraph<TOriginalNodeIndex, TOriginalNodeData, TOriginalEdgeData> origin,
    //     Func<TOriginalNodeIndex, TNodeIndex> transformNodeIndex,
    //     Func<TOriginalNodeData, TNodeData> transformNodeData,
    //     Func<TOriginalEdgeData, TEdgeData> transformEdgeData,
    //     Func<IEqualityComparer<TNodeIndex>?> nodeIndexEqualityComparerFactoryMethod)
    //   where TOriginalNodeIndex : notnull where TOriginalEdgeIndex : notnull
    // {
    //   // TODO
    //   throw new NotImplementedException();
    // }

    // ### ADDITION & REMOVAL ###

    internal void AddNode(TNodeIndex index, TNodeData data)
    {
        if (nodes.ContainsKey(index))
            throw new InvalidOperationException($"Node with index {index} already exists.");
        nodes.Add(index, new InternalNode<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>(this, index, data));
    }

    internal bool TryAddNode(TNodeIndex index, TNodeData data)
    {
        try
        {
            AddNode(index, data);
            return true;
        }
        catch (Exception _)
        {
            return false;
        }
    }

    internal bool RemoveNode(TNodeIndex index)
    {
        if (!nodes.Remove(index))
            return false;
        var edgeRemovalActions = incomingEdges[index].Concat(outgoingEdges[index])
            .Distinct().Select<TEdgeIndex, Action>(edge => () => RemoveEdge(edge));
        foreach (var action in edgeRemovalActions)
            action();
        return true;
    }

    internal void AddEdge(TEdgeIndex index, TNodeIndex start, TNodeIndex end, TEdgeData data)
    {
        if (!nodes.ContainsKey(start))
            throw new KeyNotFoundException("The start node does not exist in the graph.");
        if (!nodes.ContainsKey(end))
            throw new KeyNotFoundException("The end node does not exist in the graph.");
        var edge = new InternalEdge<TNodeIndex, TNodeData, TEdgeIndex, TEdgeData>(this, index, start, end, data);
        edges.Add(index, edge);
        outgoingEdges.Add(start, index);
        incomingEdges.Add(end, index);
    }
    internal bool TryAddEdge(TEdgeIndex index, TNodeIndex start, TNodeIndex end, TEdgeData data)
    {
        try
        {
            AddEdge(index, start, end, data);
            return true;
        }
        catch (Exception _)
        {
            return false;
        }
    }

    internal bool RemoveEdge(TEdgeIndex index)
    {
        if (!edges.Remove(index, out var edge))
            return false;
        outgoingEdges.RemoveFrom(edge.Origin, index);
        incomingEdges.RemoveFrom(edge.Destination, index);
        return true;
    }
}