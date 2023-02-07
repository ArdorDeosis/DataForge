using NUnit.Framework.Constraints;

namespace DataForge.Graphs.Tests;

public sealed class Is : NUnit.Framework.Is
{
  public static GraphComponentValidityConstraint Valid => new(true);

  public static GraphComponentValidityConstraint Invalid => new(false);
}

public sealed class AreAll
{
  public static AllItemsConstraint Valid => new(new GraphComponentValidityConstraint(true));

  public static AllItemsConstraint Invalid => new(new GraphComponentValidityConstraint(false));
}