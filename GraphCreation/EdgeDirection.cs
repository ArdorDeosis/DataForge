namespace DataForge.GraphCreation;

[Flags]
public enum EdgeDirection
{
  None = 0,
  Forward = 1,
  Backward = 1 << 1,
  ForwardAndBackward = Forward | Backward,
}