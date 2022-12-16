using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Graph.Tests;

public partial class IndexedGraphTests
{
  [Test]
  public void Constructor_NullEqualityComparer_DoesNotThrow()
  {
    // ARRANGE
    IEqualityComparer<int> nullComparer = null!;

    // ASSERT
    Assert.That(() => new NodeIndexedGraph<int, int, int>(nullComparer), Throws.Nothing);
  }

  [Test]
  public void EqualityComparer_CustomImplementation_IsUsed()
  {
    // ARRANGE
    const int index = 1;
    var comparer = new TestEqualityComparer();
    var graph = new NodeIndexedGraph<int, int, int>(comparer);

    // ACT
    graph.AddNode(index, 0xBEEF);
    var _ = graph[index];

    // ASSERT
    Assert.That(comparer.CallsToEquals, Is.GreaterThan(0));
    Assert.That(comparer.CallsToGetHashCode, Is.GreaterThan(0));
  }

  [Test]
  public void EqualityComparer_CustomFactoryMethod_IsUsed()
  {
    // ARRANGE
    const int index = 1;
    var comparer = new TestEqualityComparer();
    var graph = new NodeIndexedGraph<int, int, int>(() => comparer);

    // ACT
    graph.AddNode(index, 0xBEEF);
    var _ = graph[index];

    // ASSERT
    Assert.That(comparer.CallsToEquals, Is.GreaterThan(0));
    Assert.That(comparer.CallsToGetHashCode, Is.GreaterThan(0));
  }

  [Test]
  public void Copy_WithCustomComparer_SameInstanceIsUsed()
  {
    // ARRANGE
    const int index = 1;
    var comparer = new TestEqualityComparer();
    var graph = new NodeIndexedGraph<int, int, int>(comparer);
    var copiedGraph = graph.Copy();

    // ACT
    copiedGraph.AddNode(index, 0xBEEF);
    var _ = copiedGraph[index];

    // ASSERT
    Assert.That(comparer.CallsToEquals, Is.GreaterThan(0));
    Assert.That(comparer.CallsToGetHashCode, Is.GreaterThan(0));
  }

  [Test]
  public void Copy_WithCustomComparerFactoryMethod_NewInstanceIsCreated()
  {
    // ARRANGE
    var factory = new TestEqualityComparerFactory();
    var graph = new NodeIndexedGraph<int, int, int>(factory.Produce);

    // ACT
    graph.Copy();

    // ASSERT
    Assert.That(factory.ComparersProduced, Is.EqualTo(2));
  }

  [Test]
  public void TransformWithoutIndexTransform_WithCustomComparer_SameInstanceIsUsed()
  {
    // ARRANGE
    const int index = 1;
    var comparer = new TestEqualityComparer();
    var graph = new NodeIndexedGraph<int, int, int>(comparer);

    // ACT
    var transformedGraph = graph.Transform(n => n, n => n);
    transformedGraph.AddNode(index, 0xF00D);
    var _ = transformedGraph[index];

    // ASSERT
    Assert.That(comparer.CallsToEquals, Is.GreaterThan(0));
    Assert.That(comparer.CallsToGetHashCode, Is.GreaterThan(0));
  }

  [Test]
  public void Transform_NoCustomComparer_NewDefaultIsUsed()
  {
    // ARRANGE
    const int key1 = 1;
    const int key2 = -1; // is equivalent to key1 in the test comparer
    var comparer = new TestEqualityComparer();
    var graph = new NodeIndexedGraph<int, int, int>(comparer);

    // ACT
    graph.AddNode(key1, 0xC0FFEE);
    var transformedGraph = graph.Transform(n => n, n => n, n => n);

    // ASSERT
    Assert.That(() => transformedGraph.AddNode(key2, 0xBEEF), Throws.Nothing);
    Assert.That(() => graph.AddNode(key2, 0xBEEF), Throws.Exception);
  }

  [Test]
  public void TransformThenCopy_CustomComparer_SameInstanceIsUsed()
  {
    // ARRANGE
    const int key = 1;
    var comparer = new TestEqualityComparer();
    var graph = new NodeIndexedGraph<int, int, int>();

    // ACT
    var transformedGraph = graph.Transform(n => n, n => n, n => n, comparer);
    var copiedGraph = transformedGraph.Copy();
    copiedGraph.AddNode(key, 0xF00D);
    var _ = copiedGraph[key];

    // ASSERT
    Assert.That(comparer.CallsToEquals, Is.GreaterThan(0));
    Assert.That(comparer.CallsToGetHashCode, Is.GreaterThan(0));
  }

  [Test]
  public void TransformThenCopy_CustomComparerFactoryMethod_NewInstanceIsCreated()
  {
    // ARRANGE
    var factory = new TestEqualityComparerFactory();
    var graph = new NodeIndexedGraph<int, int, int>();

    // ACT
    var transformedGraph = graph.Transform(n => n, n => n, n => n, factory.Produce);
    transformedGraph.Copy();

    // ASSERT
    Assert.That(factory.ComparersProduced, Is.EqualTo(2));
  }
}

/// <summary>
/// An equality comparer used for tests which has an ID and counts how often it is called. It also uses the absolute
/// values to compare integers.
/// </summary>
internal sealed class TestEqualityComparer : EqualityComparer<int>
{
  internal int CallsToEquals { get; private set; }
  internal int CallsToGetHashCode { get; private set; }

  public override bool Equals(int x, int y)
  {
    CallsToEquals++;
    return Math.Abs(x) == Math.Abs(y);
  }

  public override int GetHashCode(int obj)
  {
    CallsToGetHashCode++;
    return Math.Abs(obj);
  }
}

/// <summary>
/// An equality comparer factory counting how many comparers have been produced.
/// </summary>
internal sealed class TestEqualityComparerFactory
{
  internal int ComparersProduced { get; private set; }

  internal IEqualityComparer<int> Produce()
  {
    ComparersProduced++;
    var comparer = new TestEqualityComparer();
    return comparer;
  }
}