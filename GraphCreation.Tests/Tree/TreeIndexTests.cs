using System.Collections.Generic;
using NUnit.Framework;

namespace DataForge.GraphCreation.Tests.Tree;

internal class TreeIndexTests
{
  [Test]
  public void TestDefaultValues()
  {
    // ARRANGE
    var index = new TreeIndex();

    // ASSERT
    Assert.That(index.ChildIndex, Is.Zero);
    Assert.That(index.ParentIndex, Is.Null);
  }

  [Test]
  public void Initializer_InvalidChildIndex_ThrowsArgumentException() =>
    Assert.That(() => new TreeIndex(null!, -1), Throws.ArgumentException);

  [TestCaseSource(nameof(TreeIndicesAndExpectedDepths))]
  public void Depth_ExpectedValue(TreeIndex index, int expectedDepth) =>
    Assert.That(index.Depth, Is.EqualTo(expectedDepth));

  [TestCaseSource(nameof(EqualPairs))]
  public void Equals_ValueEquivalentPairs_True(TreeIndex a, TreeIndex b)
  {
    // ASSERT
    Assert.That(a.Equals(b), Is.True);
    Assert.That(a.Equals((object)b), Is.True);
  }

  [Test]
  public void Equals_NullTreeIndex_False()
  {
    // ASSERT
    Assert.That(new TreeIndex().Equals(null), Is.False);
  }

  [TestCaseSource(nameof(UnequalPairs))]
  public void Equals_NonEquivalentPairs_False(TreeIndex index, object? other)
  {
    // ASSERT
    Assert.That(index.Equals(other), Is.False);
  }

  [TestCaseSource(nameof(EqualPairs))]
  public void GetHashCode_SameValues_True(TreeIndex a, TreeIndex b) =>
    Assert.That(a.GetHashCode(), Is.EqualTo(b.GetHashCode()));

  private static IEnumerable<object[]> EqualPairs()
  {
    var instance = new TreeIndex();
    yield return new object[] { instance, instance };
    yield return new object[] { new TreeIndex(), new TreeIndex() };
    yield return new object[] { TreeIndexHelper.FromArray(0), TreeIndexHelper.FromArray(0) };
    yield return new object[]
    {
      TreeIndexHelper.FromArray(0xC0FFEE, 0xBEEF, 0xF00D),
      TreeIndexHelper.FromArray(0xC0FFEE, 0xBEEF, 0xF00D),
    };
  }

  private static IEnumerable<object?[]> UnequalPairs()
  {
    yield return new object?[] { new TreeIndex(), null };
    yield return new object?[] { new TreeIndex(), new { } };
    yield return new object[] { TreeIndexHelper.FromArray(0), TreeIndexHelper.FromArray(1) };
    yield return new object[] { TreeIndexHelper.FromArray(0), TreeIndexHelper.FromArray(0, 0) };
  }

  private static IEnumerable<object[]> TreeIndicesAndExpectedDepths()
  {
    yield return new object[] { new TreeIndex(), 0 };
    yield return new object[] { TreeIndexHelper.FromArray(0), 1 };
    yield return new object[] { TreeIndexHelper.FromArray(0, 0, 0), 3 };
    yield return new object[] { TreeIndexHelper.FromArray(0xC0FFEE, 0xBEEF, 0xF00D), 3 };
    yield return new object[] { TreeIndexHelper.FromArray(1, 2, 3, 4, 5, 6, 7, 8), 8 };
  }
}