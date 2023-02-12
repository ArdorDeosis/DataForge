using System;
using NUnit.Framework;

namespace DataForge.GraphCreation.Tests.Bipartite;

internal class BipartiteGraphCreationOptionsTests
{
  [TestCase(EdgeDirection.Forward, true)]
  [TestCase(EdgeDirection.Backward, false)]
  public void BipartiteGraphCreationOptions_DefaultCreateEdgeFunction(EdgeDirection edgeDirection, bool expectedValue)
  {
    // ARRANGE
    var options = new BipartiteGraphCreationOptions<int, int>
    {
      NodeDataSetA = Array.Empty<int>(),
      NodeDataSetB = Array.Empty<int>(),
    };

    // ASSERT
    Assert.That(options.ShouldCreateEdge(0, 0, edgeDirection), Is.EqualTo(expectedValue));
  }
}