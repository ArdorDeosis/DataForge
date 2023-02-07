using NUnit.Framework;

namespace DataForge.GraphCreation.Tests.Multipartite;

internal class MultipartiteGraphCreationOptionsTests
{
  [TestCase(EdgeDirection.Forward, true)]
  [TestCase(EdgeDirection.Backward, false)]
  public void MultipartiteGraphCreationOptions_DefaultCreateEdgeFunction(EdgeDirection edgeDirection,
    bool expectedValue)
  {
    // ARRANGE
    var options = new MultipartiteGraphCreationOptions<int, int>();

    // ASSERT
    Assert.That(options.ShouldCreateEdge(0, 0, edgeDirection), Is.EqualTo(expectedValue));
  }
}