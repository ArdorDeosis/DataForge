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
  }

  public TreeIndex? ParentIndex { get; private init; }

  public int ChildIndex { get; private init; }

  public int Depth => ParentIndex is { Depth: var parentDepth } ? parentDepth + 1 : 0;
  public bool IsRoot => ParentIndex is null;


  /// <inheritdoc />
  public bool Equals(TreeIndex? other)
  {
    if (ReferenceEquals(null, other))
      return false;
    if (ReferenceEquals(this, other))
      return true;
    return Equals(ParentIndex, other.ParentIndex) && ChildIndex == other.ChildIndex;
  }

  /// <inheritdoc />
  public override bool Equals(object? obj)
  {
    if (ReferenceEquals(null, obj))
      return false;
    if (ReferenceEquals(this, obj))
      return true;
    return obj.GetType() == GetType() && Equals((TreeIndex)obj);
  }

  /// <inheritdoc />
  public override int GetHashCode() =>
    ParentIndex is null
      ? ChildIndex
      : HashCode.Combine(ParentIndex.GetHashCode(), ChildIndex);

  /// <inheritdoc />
  public override string ToString() => IsRoot ? "root" : $"{ParentIndex}-{ChildIndex}";
}