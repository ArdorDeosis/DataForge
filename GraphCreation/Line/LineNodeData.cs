using Utilities;

namespace GraphCreation;

public readonly struct LineNodeData
{
  public readonly int Position;

  public LineNodeData() => ThrowHelper.ThrowStructNotPubliclyConstructableException(nameof(LineNodeData));

  internal LineNodeData(int position)
  {
    Position = position;
  }
}