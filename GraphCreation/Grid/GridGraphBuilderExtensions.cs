namespace GraphCreation;

public static class GridGraphBuilderExtensions
{
  public static GridGraphBuilder<TNodeData, TEdgeData>
    WithDimensions<TBuilder, TNodeData, TEdgeData>(this TBuilder builder, params int[] dimensions)
    where TBuilder : GridGraphBuilder<TNodeData, TEdgeData> =>
    builder.WithDimensions<TBuilder, TNodeData, TEdgeData>(dimensions.AsEnumerable());

  public static GridGraphBuilder<TNodeData, TEdgeData> WithDimensions<TBuilder, TNodeData, TEdgeData>(
    this TBuilder builder, IEnumerable<int> dimensions) where TBuilder : GridGraphBuilder<TNodeData, TEdgeData>
  {
    builder.DimensionsInformation =
      dimensions.Select(dimension => new GridGraphDimensionInformation(dimension)).ToArray();
    return builder;
  }

  public static TBuilder WithDimensions<TBuilder, TNodeData, TEdgeData>(this TBuilder builder,
    params GridGraphDimensionInformation[] dimensions) where TBuilder : GridGraphBuilder<TNodeData, TEdgeData> =>
    builder.WithDimensions<TBuilder, TNodeData, TEdgeData>(dimensions.AsEnumerable());

  public static TBuilder WithDimensions<TBuilder, TNodeData, TEdgeData>(this TBuilder builder,
    IEnumerable<GridGraphDimensionInformation> dimensions) where TBuilder : GridGraphBuilder<TNodeData, TEdgeData>
  {
    builder.DimensionsInformation = dimensions.ToArray();
    return builder;
  }

  public static TBuilder WithoutEdges<TBuilder, TNodeData, TEdgeData>(this TBuilder builder)
    where TBuilder : GridGraphBuilder<TNodeData, TEdgeData> =>
    builder.WithEdges<TBuilder, TNodeData, TEdgeData>(EdgeDirection.None);

  public static TBuilder WithForwardEdges<TBuilder, TNodeData, TEdgeData>(this TBuilder builder)
    where TBuilder : GridGraphBuilder<TNodeData, TEdgeData> =>
    builder.WithEdges<TBuilder, TNodeData, TEdgeData>(EdgeDirection.Forward);

  public static TBuilder WithBackwardEdges<TBuilder, TNodeData, TEdgeData>(this TBuilder builder)
    where TBuilder : GridGraphBuilder<TNodeData, TEdgeData> =>
    builder.WithEdges<TBuilder, TNodeData, TEdgeData>(EdgeDirection.Backward);

  public static TBuilder WithForwardAndBackwardEdges<TBuilder, TNodeData, TEdgeData>(this TBuilder builder)
    where TBuilder : GridGraphBuilder<TNodeData, TEdgeData> =>
    builder.WithEdges<TBuilder, TNodeData, TEdgeData>(EdgeDirection.ForwardAndBackward);

  public static TBuilder WithEdges<TBuilder, TNodeData, TEdgeData>(this TBuilder builder, EdgeDirection direction)
    where TBuilder : GridGraphBuilder<TNodeData, TEdgeData>
  {
    builder.DimensionsInformation = builder.DimensionsInformation
      .Select(info => new GridGraphDimensionInformation(info.Size, info.Offset, direction)).ToArray();
    return builder;
  }

  public static TBuilder WithNodeData<TBuilder, TNodeData, TEdgeData>(this TBuilder builder,
    Func<GridNodeData, TNodeData> createNodeData) where TBuilder : GridGraphBuilder<TNodeData, TEdgeData>
  {
    builder.CreateNodeData = createNodeData;
    return builder;
  }

  public static TBuilder WithEdgeData<TBuilder, TNodeData, TEdgeData>(this TBuilder builder,
    Func<GridEdgeData<TNodeData, TEdgeData>, TEdgeData> createEdgeData)
    where TBuilder : GridGraphBuilder<TNodeData, TEdgeData>
  {
    builder.CreateEdgeData = createEdgeData;
    return builder;
  }

  public static IndexedGridGraphBuilder<TIndex, TNodeData, TEdgeData> WithNodeIndex<TBuilder, TIndex, TNodeData,
    TEdgeData>(this TBuilder builder, Func<GridNodeData, TIndex> createIndex)
    where TBuilder : GridGraphBuilder<TNodeData, TEdgeData> where TIndex : notnull
  {
    if (builder is not IndexedGridGraphBuilder<TIndex, TNodeData, TEdgeData> indexedBuilder)
      return new IndexedGridGraphBuilder<TIndex, TNodeData, TEdgeData>(builder, createIndex);
    indexedBuilder.CreateIndex = createIndex;
    return indexedBuilder;
  }
}