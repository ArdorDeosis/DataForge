using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace GraphCreation;

[PublicAPI]
[ExcludeFromCodeCoverage]
public readonly struct TreeNodeData
{
  public /*required*/ IReadOnlyList<int> Address { get; init; }
}