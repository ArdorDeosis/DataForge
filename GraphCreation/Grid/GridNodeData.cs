using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace GraphCreation;

[PublicAPI]
[ExcludeFromCodeCoverage]
public readonly struct GridNodeData
{
  public /*required*/ IReadOnlyList<int> Coordinates { get; init; }
}