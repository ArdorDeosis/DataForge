﻿using NUnit.Framework;

namespace DataForge.Graphs.Tests.GraphComponents;

internal class GraphComponentTests
{
  [Test]
  public void NewComponent_IsValid()
  {
    // ARRANGE
    var component = (GraphComponent)new Graph<int, int>().AddNode(0);

    // ASSERT
    Assert.That(component, Is.Valid);
  }

  [Test]
  public void Invalidate_ValidComponent_IsInvalid()
  {
    // ARRANGE
    var component = (GraphComponent)new Graph<int, int>().AddNode(0);

    // ACT
    component.Invalidate();

    // ASSERT
    Assert.That(component, Is.Invalid);
  }
}