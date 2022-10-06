using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace GraphCreation;

[PublicAPI]
[ExcludeFromCodeCoverage]
public readonly struct TreeNodeData
{
  public /*required*/ IReadOnlyList<int> Address { get; init; }

  public IReadOnlyList<int>? ParentAddress => Address.Count == 0 ? null : Address.SkipLast(1).ToArray();
}