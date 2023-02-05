using NUnit.Framework.Constraints;

namespace DataForge.Graphs.Tests;

public sealed class GraphComponentValidityConstraint : Constraint
{
  private readonly bool expectedValidity;

  public GraphComponentValidityConstraint(bool expectedValidity)
  {
    this.expectedValidity = expectedValidity;
    Description = $"{(expectedValidity ? "valid" : "invalid")} {nameof(GraphComponent)}";
  }

  public override ConstraintResult ApplyTo<TActual>(TActual actual) =>
    new GraphComponentValidityConstraintResult(this, actual,
      actual is GraphComponent graphComponent && graphComponent.IsValid == expectedValidity);
}