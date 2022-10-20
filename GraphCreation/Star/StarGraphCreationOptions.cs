using Ardalis.GuardClauses;
using JetBrains.Annotations;

namespace GraphCreation;

/// <summary>
/// Creation options for a graph with a star structure.
/// </summary>
/// <typeparam name="TNodeData">Type of the data the nodes are holding.</typeparam>
/// <typeparam name="TEdgeData">Type of the data the edges are holding.</typeparam>
/// TODO: use required keyword in C# 11
/// TODO: use field keyword in C# 12 (hopefully)
[PublicAPI]
public sealed class StarGraphCreationOptions<TNodeData, TEdgeData>
  : IndexedGraphDataCreationOptions<StarIndex, TNodeData, TEdgeData>
{
  private readonly int rayCount;

  /// <summary>
  /// Number of rays of the star.
  /// </summary>
  public /*required*/ int RayCount
  {
    get => rayCount;
    init => rayCount = Guard.Against.Negative(value, nameof(RayCount));
  }

  /// <summary>
  /// Direction in which edges are created.
  /// <see cref="EdgeDirection.Forward">EdgeDirection.Forward</see> is from the center outwards.
  /// </summary>
  public EdgeDirection EdgeDirection { get; init; } = EdgeDirection.Forward;

#pragma warning disable CS8618 // should go away with the use of the required keyword
  /// <summary>
  /// Function to calculate the length of a ray based on its index. Rays are numbered from 0 to
  /// <see cref="StarGraphCreationOptions{TNodeData,TEdgeData}.RayCount">
  /// StarGraphCreationOption&lt;TNodeData,TEdgeData&gt;.RayCount</see> - 1.
  /// </summary>
  public /*required*/ Func<int, int> CalculateRayLength { get; init; }
#pragma warning restore CS8618
}