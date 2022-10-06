using Ardalis.GuardClauses;
using JetBrains.Annotations;

namespace GraphCreation;

[PublicAPI]
public class TreeIndex : IEquatable<TreeIndex>
{
  public TreeIndex()
  { }

  public TreeIndex(TreeIndex parentIndex, int childIndex)
  {
    ParentIndex = parentIndex;
    ChildIndex = Guard.Against.Negative(childIndex);
    Depth = parentIndex.Depth + 1;
  }

  public TreeIndex? ParentIndex { get; private init; }

  public int ChildIndex { get; private init; }

  public int Depth { get; private init; }

  public bool IsRoot => ParentIndex is null;


  /// <inheritdoc />
  public bool Equals(TreeIndex? other)
  {
    if (other is null) return false;
    if (ReferenceEquals(this, other)) return true;

    var a = this;
    var b = other;
    while (true)
    {
      if (a.ChildIndex != b.ChildIndex) return false;
      a = a.ParentIndex;
      b = b.ParentIndex;
      if (a is null && b is null) return true;
      if (a is null || b is null) return false;
    }
  }

  /// <inheritdoc />
  public override bool Equals(object? obj)
  {
    if (obj is null) return false;
    if (ReferenceEquals(this, obj)) return true;
    return obj is TreeIndex treeIndex && Equals(treeIndex);
  }

  /// <inheritdoc />
  public override int GetHashCode()
  {
    if (IsRoot) return 0xC0FFEE; // don't use 0; the first child added to root will have 0
    var currentHash = ChildIndex;
    for (var index = this; index.ParentIndex is not null; index = index.ParentIndex)
      currentHash = HashCode.Combine(currentHash, index.ParentIndex.ChildIndex);
    return currentHash;
  }

  /// <inheritdoc />
  public override string ToString() => IsRoot ? "root" : $"{ParentIndex}->{ChildIndex}";
}