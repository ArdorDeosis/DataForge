namespace Graph;


public interface Node<NodeData>
{
    NodeData Data { get; }
}

public interface Edge<NodeData>
{
    IEnumerable<Node<NodeData>> Nodes { get; } 
}

public interface DirectedEdge<NodeData> : Edge<NodeData>
{
    Node<NodeData> Origin { get; }
    Node<NodeData> Destination { get; }
}

public interface DataEdge<NodeData, EdgeData> : Edge<NodeData>
{
    public EdgeData Data { get; }
}

public interface DirectedDataEdge<NodeData, EdgeData> : DirectedEdge<NodeData>, DataEdge<NodeData, EdgeData>
{
}

public interface Graph<NodeData>
{
    IEnumerable<Node<NodeData>> Nodes { get; }
    IEnumerable<Edge<NodeData>> Edges { get; }
}

public interface GraphWithEdgeData<NodeData, EdgeData> : Graph<NodeData>
{
    new IEnumerable<DataEdge<NodeData, EdgeData>> Edges { get; }
}