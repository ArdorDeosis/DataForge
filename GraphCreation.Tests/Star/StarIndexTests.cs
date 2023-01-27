using System.Collections.Generic;
using NUnit.Framework;

namespace GraphCreation.Tests;

internal class StarIndexTests
{
  [Test]
  public void TestDefaultValues()
  {
    // ARRANGE
    var index = new StarIndex();

    // ASSERT
    Assert.That(index.Distance, Is.Zero);
    Assert.That(index.Ray, Is.Zero);
  }

  [Test]
  public void Initializer_InvalidRayIndex_ThrowsArgumentException() =>
    Assert.That(() => new StarIndex(-1, 1), Throws.ArgumentException);

  [TestCase(0)]
  [TestCase(-1)]
  public void Initializer_InvalidDistanceIndex_ThrowsArgumentException(int invalidRayDistance) =>
    Assert.That(() => new StarIndex(1, invalidRayDistance), Throws.ArgumentException);

  [TestCaseSource(nameof(StarIndicesAndExpectedIsCenter))]
  public void IsCenter_ExpectedValue(StarIndex index, bool isCenter) =>
    Assert.That(index.IsCenter, Is.EqualTo(isCenter));

  [TestCaseSource(nameof(EqualPairs))]
  public void Equals_ValueEquivalentPairs_True(StarIndex a, StarIndex b)
  {
    // ASSERT
    Assert.That(a.Equals(b), Is.True);
    Assert.That(a.Equals((object)b), Is.True);
  }

  [Test]
  public void Equals_NullTreeIndex_False() => Assert.That(new StarIndex().Equals(null), Is.False);

  [TestCaseSource(nameof(UnequalPairs))]
  public void Equals_NonEquivalentPairs_False(StarIndex index, object? other) =>
    Assert.That(index.Equals(other), Is.False);

  [TestCaseSource(nameof(EqualPairs))]
  public void GetHashCode_SameValues_True(StarIndex a, StarIndex b) =>
    Assert.That(a.GetHashCode(), Is.EqualTo(b.GetHashCode()));

  private static IEnumerable<object[]> EqualPairs()
  {
    var instance = new StarIndex();
    yield return new object[] { instance, instance };
    yield return new object[] { new StarIndex(), new StarIndex() };
    yield return new object[] { new StarIndex(1, 2), new StarIndex(1, 2) };
    yield return new object[] { new StarIndex(0xC0FFEE, 0xBEEF), new StarIndex(0xC0FFEE, 0xBEEF) };
  }

  private static IEnumerable<object?[]> UnequalPairs()
  {
    yield return new object?[] { new StarIndex(), null };
    yield return new object?[] { new StarIndex(), new { } };
    yield return new object[] { new StarIndex(1, 1), new StarIndex(1, 2) };
    yield return new object[] { new StarIndex(1, 1), new StarIndex(2, 1) };
  }

  private static IEnumerable<object[]> StarIndicesAndExpectedIsCenter()
  {
    yield return new object[] { new StarIndex(), true };
    yield return new object[] { new StarIndex(1, 1), false };
  }
}