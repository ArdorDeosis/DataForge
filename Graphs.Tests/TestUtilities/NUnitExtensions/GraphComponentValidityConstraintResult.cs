using NUnit.Framework.Constraints;

namespace DataForge.Graphs.Tests;

internal sealed class GraphComponentValidityConstraintResult : ConstraintResult
{
  public GraphComponentValidityConstraintResult(IConstraint constraint, object actualValue, bool isSuccess) : base(
    constraint, actualValue, isSuccess) { }

  public override void WriteActualValueTo(MessageWriter writer) =>
    writer.WriteActualValue(new WrapperForPrinting(ActualValue));

  private readonly struct WrapperForPrinting
  {
    private readonly object? wrappedObject;

    public WrapperForPrinting(object? wrappedObject)
    {
      this.wrappedObject = wrappedObject;
    }

    public override string ToString() =>
      wrappedObject switch
      {
        GraphComponent { IsValid: true } => $"valid {nameof(GraphComponent)}",
        GraphComponent { IsValid: false } => $"invalid {nameof(GraphComponent)}",
        null => "null",
        _ => $"not a {nameof(GraphComponent)}",
      };
  }
}