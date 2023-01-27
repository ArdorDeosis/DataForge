using System.Collections.Generic;
using NUnit.Framework;

namespace GraphCreation.Tests;

internal class DiskIndexTests
{
  [Test]
  public void TestDefaultValues()
  {
    // ARRANGE
    var index = new DiskIndex();

    // ASSERT
    Assert.That(index.Distance, Is.Zero);
    Assert.That(index.Meridian, Is.Zero);
  }

  public void Initializer_InvalidMeridian_ThrowsArgumentException() =>
    Assert.That(() => new DiskIndex(-1, 1), Throws.ArgumentException);

  [TestCase(0)]
  [TestCase(-1)]
  public void Initializer_InvalidDistance_ThrowsArgumentException(int invalidDistance) =>
    Assert.That(() => new DiskIndex(0, invalidDistance), Throws.ArgumentException);

  [TestCaseSource(nameof(StarIndicesAndExpectedIsCenter))]
  public void IsCenter_ExpectedValue(DiskIndex index, bool isCenter) =>
    Assert.That(index.IsCenter, Is.EqualTo(isCenter));

  [TestCaseSource(nameof(EqualPairs))]
  public void Equals_ValueEquivalentPairs_True(DiskIndex a, DiskIndex b)
  {
    // ASSERT
    Assert.That(a.Equals(b), Is.True);
    Assert.That(a.Equals((object)b), Is.True);
  }

  [Test]
  public void Equals_NullTreeIndex_False() => Assert.That(new DiskIndex().Equals(null), Is.False);

  [TestCaseSource(nameof(UnequalPairs))]
  public void Equals_NonEquivalentPairs_False(DiskIndex index, object? other) =>
    Assert.That(index.Equals(other), Is.False);

  [TestCaseSource(nameof(EqualPairs))]
  public void GetHashCode_SameValues_True(DiskIndex a, DiskIndex b) =>
    Assert.That(a.GetHashCode(), Is.EqualTo(b.GetHashCode()));

  private static IEnumerable<object[]> EqualPairs()
  {
    var instance = new DiskIndex();
    yield return new object[] { instance, instance };
    yield return new object[] { new DiskIndex(), new DiskIndex() };
    yield return new object[] { new DiskIndex(1, 2), new DiskIndex(1, 2) };
    yield return new object[] { new DiskIndex(0xC0FFEE, 0xBEEF), new DiskIndex(0xC0FFEE, 0xBEEF) };
  }

  private static IEnumerable<object?[]> UnequalPairs()
  {
    yield return new object?[] { new DiskIndex(), null };
    yield return new object?[] { new DiskIndex(), new { } };
    yield return new object[] { new DiskIndex(1, 1), new DiskIndex(1, 2) };
    yield return new object[] { new DiskIndex(1, 1), new DiskIndex(2, 1) };
  }

  private static IEnumerable<object[]> StarIndicesAndExpectedIsCenter()
  {
    yield return new object[] { new DiskIndex(), true };
    yield return new object[] { new DiskIndex(1, 1), false };
  }
}