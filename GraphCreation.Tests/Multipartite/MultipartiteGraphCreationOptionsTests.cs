using NUnit.Framework;

namespace GraphCreation.Tests;

public class MultipartiteGraphCreationOptionsTests
{
  [TestCase(EdgeDirection.Forward, true)]
  [TestCase(EdgeDirection.Backward, false)]
  public void MultipartiteGraphCreationOptions_DefaultCreateEdgeFunction(EdgeDirection edgeDirection,
    bool expectedValue)
  {
    // ARRANGE
    var options = new MultipartiteGraphCreationOption<int, int>();

    // ASSERT
    Assert.That(options.CreateEdge(0, 0, edgeDirection), Is.EqualTo(expectedValue));
  }
}