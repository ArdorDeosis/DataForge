﻿using NUnit.Framework;

namespace GraphCreation.Tests;

public class BipartiteGraphCreationOptionsTests
{
  [TestCase(EdgeDirection.Forward, true)]
  [TestCase(EdgeDirection.Backward, false)]
  public void BipartiteGraphCreationOptions_DefaultCreateEdgeFunction(EdgeDirection edgeDirection,
    bool expectedValue)
  {
    // ARRANGE
    var options = new BipartiteGraphCreationOption<int, int>();

    // ASSERT
    Assert.That(options.CreateEdge(0, 0, edgeDirection), Is.EqualTo(expectedValue));
  }
}